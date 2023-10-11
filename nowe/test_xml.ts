
import * as fs from 'node:fs';
import * as path from "node:path";
import * as util from 'node:util';
import * as convert from 'xml-js';
import * as docx from "docx";

let fileDir = '.';

let sections: docx.ISectionOptions[] = [];
let currentSection: docx.ISectionOptions;
let paragraphStyles: docx.IParagraphStyleOptions[] = [];
let characterStyles: docx.ICharacterStyleOptions[] = [];
let aliases: { [id: string]: any } = {};

interface Element {
    type: 'element';
    name: string;
    attributes?: { [key: string]: string };
    elements?: Node[];
}

interface Text {
    type: 'text';
    text: string;
}

interface CData {
    type: 'cdata';
    cdata: string;
}

interface Instruction {
    type: 'instruction';
    name: string;
    instruction: string;
}

type docxFileChild = docx.Paragraph | docx.Table | docx.TableOfContents;

type Node = Element | Text | CData | Instruction;

let xml = convert.xml2js(fs.readFileSync('test1.xml', 'utf-8'), { ignoreComment: true });

//console.log(util.inspect(xml, false, null, true));

if (!xml.elements || xml.elements.length != 1 || (xml.elements[0] as Node).type !== 'element' || (xml.elements[0] as Element).name !== 'document') {
    throw new Error('Invalid top level structure of the document. Required one top level <document> element.');
}

let document = xml.elements[0] as Element;

function extractName(nameWithFilter: string) {
    let arr = nameWithFilter.split(':');
    if (arr.length != 2) {
        return nameWithFilter;
    } else {
        return arr[0];
    }
}

function extractFilter(nameWithFilter: string) {
    let arr = nameWithFilter.split(':');
    if (arr.length != 2) {
        return null;
    } else {
        return arr[1];
    }
}

function convertSize(value: string | number | string[] | number[], nonZero: boolean = false, fraction: boolean = false, targetUnitsPerPt: number = 1): any {
    if (typeof (value) == 'string') {
        value = value.trim();
        let scale: number;
        if (value.endsWith('mm')) {
            scale = targetUnitsPerPt * 360 / 127;
        } else if (value.endsWith('cm')) {
            scale = targetUnitsPerPt * 3600 / 127;
        } else if (value.endsWith('in')) {
            scale = targetUnitsPerPt * 72;
        } else if (value.endsWith('pt')) {
            scale = targetUnitsPerPt * 1;
        } else if (value.endsWith('pc')) {
            scale = targetUnitsPerPt * 12;
        } else {
            throw Error(`Unknown units at "${value}"`);
        }
        let result = parseFloat(value.substring(0, value.length - 2)) * scale;
        if (!fraction) {
            result = Math.round(result);
        }
        if (nonZero && result < 1) {
            result = 1;
        }
        return result;
    } else if (typeof (value) == 'object' && value instanceof Array) {
        return value.map((x: string | number) => convertSize(x, nonZero, fraction, targetUnitsPerPt));
    } else {
        return value;
    }
}

const boolValues: { [key: string]: boolean } = {
    'true': true,
    't': true,
    'yes': true,
    'y': true,
    '1': true,
    'on': true,
    'false': false,
    'f': false,
    'no': false,
    'n': false,
    '0': false,
    'off': false,
};

const filters: { [key: string]: (value: any) => any } = {
    'file': (value: any) => {
        let filePath = path.resolve(fileDir, value as string);
        return fs.readFileSync(filePath);
    },
    'int': (value: any) => Math.round(parseFloat(value as string)),
    'pt': (value: any) => convertSize(value as any, false, false),
    'pt3q': (value: any) => convertSize(value as any, false, false, 4 / 3),
    'pt8': (value: any) => convertSize(value as any, false, false, 8),
    'pt20': (value: any) => convertSize(value as any, false, false, 20),
    'dxa': (value: any) => convertSize(value as any, false, false, 20),
    'emu': (value: any) => convertSize(value as any, false, false, 12700),
    'bool': (value: any) => {
        let v = ('' + value).toLowerCase();
        if (v in boolValues) {
            return boolValues[v];
        } else {
            throw new Error(`Invalid boolean value "${value}"`);
        }
    },
    'alias': (value: any) => {
        if (value === undefined || value === null || value === false) {
            return false;
        } else if (value === true) {
            return true;
        } else if (value in aliases) {
            return JSON.parse(JSON.stringify(aliases[value]));
        } else {
            throw new Error(`Undefined alias "${value}"`);
        }
    },
    'json': (value: any) => (new Function(`return ${value};`))(),
    'new': (value: any) => value,
    'FileChildren': (value: any) => value,
    'ParagraphChildren': (value: any) => value,
};

function filter(filterName: string, value: any, passUndefined: boolean = false): any {
    if (passUndefined && value === undefined) return value;
    let filter = extractFilter(filterName);
    if (filter === null) {
        return value;
    }
    if (filter in filters) {
        return filters[filter](value);
    } else {
        throw new Error(`Unknown filter ${filter}`);
    }
}

function attributesToOptions(element: Element) {
    let ref: { [key: string]: any } = {};
    let obj: { [key: string]: any } = {};
    for (let [key, value] of Object.entries({ ...(element.attributes || {}) })) {
        if (key == '_') {
            ref = {...ref, ...aliases[value] };
        } else {
            obj[extractName(key)] = filter(key, value);
        }
    }
    return {...ref, ...obj};
}

function elementToOptions(element: Element): any {
    /*
    Type of elements:
        String: <str>The text</str>
        CData: <data><![CDATA[ ... ]]></data>
        Array: <arr><_ ...></_><_ .../></arr>
            empty array: <arr><?empty?></arr>
        Object: <obj prop1="..."><prop2>...</prop2><prop3>...</prop3></obj>
        FileChildren array: <children><?FileChildren?>...<children>
        ParagraphChildren array: <children><?ParagraphChildren?>...<children>
    */
    let elements = element.elements || [];

    if (elements.length == 1 && elements[0].type == 'text') {

        return filter(element.name, elements[0].text);

    } else if (elements.length == 1 && elements[0].type == 'cdata') {

        return filter(element.name, elements[0].cdata);

    } else if (extractFilter(element.name) == 'FileChildren') {

        let arr: docxFileChild[] = [];
        processFileChildren(element, arr);
        return arr;

    } else if (extractFilter(element.name) == 'ParagraphChildren') {

        let arr: docx.ParagraphChild[] = [];
        processParagraphChildren(element, arr, {});
        return arr;

    } else if (elements.length == 1 && elements[0].type == 'element' && extractFilter(elements[0].name) == 'new') {

        let obj = new ((docx as any)[extractName(elements[0].name)])(elementToOptions(elements[0]));
        return obj;

    } else if (elements.length == 1 && elements[0].type == 'instruction' && elements[0].name == 'empty') {

        return [];

    } else if (elements.length > 0 && elements[0].type == 'element' && extractName(elements[0].name) == '_') {

        let arr: any[] = [];
        if (element.attributes && Object.entries(element.attributes).length > 0) {
            throw new Error(`Array <${element.name}> does not allow attribute.`);
        }
        for (let sub of elements) {
            if (sub.type == 'element' && extractName(sub.name) == '_') {
                let value = elementToOptions(sub);
                arr.push(value);
            } else {
                throw new Error(`Expecting only items in array <${element.name}>.`);
            }
        }
        return filter(element.name, arr);

    } else {

        let obj: { [key: string]: any } = attributesToOptions(element);
        for (let sub of elements) {
            if (sub.type == 'element') {
                let value = elementToOptions(sub);
                obj[extractName(sub.name)] = value;
            } else {
                throw new Error(`Expecting only elements in <${element.name}>.`);
            }
        }
        return filter(element.name, obj);

    }
    // TODO: More error checking
}


function processParagraphChild(node: Node, target: docx.ParagraphChild[], state: docx.IRunOptions) {
    if (node.type == 'instruction') {
        throw new Error(`Unexpected instruction <?${node.name}?>`);
    } else if (node.type == 'text') {
        target.push(new docx.TextRun({ ...state, text: node.text }));
    } else if (node.type == 'cdata') {
        target.push(new docx.TextRun({ ...state, text: node.cdata }));
    } else if (node.name == 'b') {
        processParagraphChildren(node, target, { ...state, bold: true });
    } else if (node.name == 'i') {
        processParagraphChildren(node, target, { ...state, italics: true });
    } else if (node.name == 'sub') {
        processParagraphChildren(node, target, { ...state, subScript: true });
    } else if (node.name == 'super') {
        processParagraphChildren(node, target, { ...state, superScript: true });
    } else if (node.name == 'img') {
        addImage(node, target);
    } else if (node.name == 'TotalPages') {
        target.push(new docx.TextRun({ ...state, children:[docx.PageNumber.TOTAL_PAGES] }));
    } else if (node.name == 'CurrentPageNumber') {
        target.push(new docx.TextRun({ ...state, children:[docx.PageNumber.CURRENT] }));
    } else if (node.type == 'element') {
        let name = extractName(node.name);
        let obj = new ((docx as any)[name])(elementToOptions(node));
        target.push(obj);
    }
}

function processParagraphChildren(parent: Element, target: docx.ParagraphChild[], state: docx.IRunOptions) {
    for (let child of (parent.elements || [])) {
        processParagraphChild(child, target, state);
    }
}

function processCells(parent: Element, target: docx.TableCell[]) {
    for (let element of (parent.elements || [])) {
        if (element.type != 'element' || extractName(element.name) != 'td') {
            throw new Error(`Expecting only <td> in <tr>.`);
        }
        let children: docxFileChild[] = [];
        processFileChildren(element, children);
        let opt = attributesToOptions(element);
        let cell = new docx.TableCell({ ...opt, children });
        target.push(cell);
    }
}

function processRows(parent: Element, target: docx.TableRow[]) {
    for (let element of (parent.elements || [])) {
        if (element.type != 'element' || extractName(element.name) != 'tr') {
            throw new Error(`Expecting only <tr> in <table>.`);
        }
        let children: docx.TableCell[] = [];
        processCells(element, children);
        let row = new docx.TableRow({ ...attributesToOptions(element), children });
        target.push(row);
    }
}

function processFileChild(element: Node, target: docxFileChild[]) {

    if (element.type == 'instruction' && element.name == 'FileChildren') {
        return;
    } else if (element.type != 'element') {
        throw new Error('Only element nodes are allowed as file child element.'); // TODO: better message
    }

    if (element.name == 'p') {
        let children: docx.ParagraphChild[] = [];
        processParagraphChildren(element, children, {});
        let paragraph = new docx.Paragraph({ ...attributesToOptions(element), children });
        target.push(paragraph);
    } else if (element.name.match(/^h[1-9]$/)) {
        let children: docx.ParagraphChild[] = [];
        processParagraphChildren(element, children, {});
        let style = 'Heading' + element.name.substring(1);
        let paragraph = new docx.Paragraph({ style, ...attributesToOptions(element), children });
        target.push(paragraph);
    } else if (element.name == 'table') {
        let rows: docx.TableRow[] = [];
        processRows(element, rows);
        let options = attributesToOptions(element);
        if (options.columnWidths) {
            options.columnWidths = (options.columnWidths as string)
                .split(',')
                .map(x => filter(':dxa', x.trim()));
        }
        let table = new docx.Table({ ...options, rows });
        target.push(table);
    } else if (element.name == 'Paragraph' || element.name == 'Table' || element.name == 'TableOfContents') {
        let options = elementToOptions(element);
        let constr = (docx as any)[element.name];
        target.push(new constr(options));
    } else {
        throw new Error(`Unexpected file child element <${element.name}>`);
    }
}

function processFileChildren(parent: Element, target: docxFileChild[]) {
    for (let child of (parent.elements || [])) {
        processFileChild(child, target);
    }
}

function processTopLevel(document: Element) {
    for (let element of (document.elements || [])) {
        if (element.type != 'element') {
            throw new Error(`Only element nodes are allowed inside top level element. Found ${element.type}.`);
        }
        if (element.name == 'ParagraphStyle') {
            let obj = elementToOptions(element);
            paragraphStyles.push(obj);
            continue;
        } else if (element.name == 'CharacterStyle') {
            let obj = elementToOptions(element);
            characterStyles.push(obj);
            continue;
        } else if (element.name == 'Alias') {
            let id = element.attributes?.id;
            if (!id) {
                throw new Error('Alias without id');
            }
            delete element.attributes;
            let obj = elementToOptions(element);
            aliases[id] = obj;
            continue;
        } else if (element.name == 'Section') {
            let obj = elementToOptions(element);
            currentSection = { ...obj, children: [] };
            sections.push(currentSection);
            continue;
        } else {
            processFileChild(element, currentSection.children as docxFileChild[]);
        }
    }
}

processTopLevel(document);

//console.log(util.inspect(paragraphStyles, false, null, true));
//console.log(util.inspect(characterStyles, false, null, true));
//console.log(util.inspect(sections, false, null, true));

const doc = new docx.Document({ sections, styles: { paragraphStyles, characterStyles } });

docx.Packer.toBuffer(doc).then((buffer) => {
    fs.writeFileSync("doc.docx", buffer);
});

function removeUndefinedProperties(value: any) {
    let result = false;
    if (typeof value != 'object') {
        return true;
    } else if (value instanceof Array) {
        for (let i = 0; i < value.length; i++) {
            if (removeUndefinedProperties(value[i])) {
                result = true;
            }
        }
    } else {
        for (let key of [...Object.keys(value)]) {
            if (value[key] === undefined) {
                delete value[key];
            } else {
                if (removeUndefinedProperties(value[key])) {
                    result = true;
                } else {
                    delete value[key];
                }
            }
        }
    }
    return result;
}

function addImage(node: Element, target: docx.ParagraphChild[]) {

    let input = elementToOptions(node);

    if (!input.src || !('width' in input) || !('height' in input)) {
        throw new Error(`Missing required attributes in <img>`);
    }

    let options: docx.IImageOptions = {
        data: filter(':file', input.src),
        transformation: {
            width: filter(':pt3q', input.width, true),
            height: filter(':pt3q', input.height, true),
            rotation: filter(':int', input.rotation, true),
            flip: {
                horizontal: filter(':bool', input.flipHorizontal, true),
                vertical: filter(':bool', input.flipVertical, true),
            }
        },
        floating: {
            allowOverlap: filter(':bool', input.allowOverlap, true),
            behindDocument: filter(':bool', input.behindDocument, true),
            layoutInCell: filter(':bool', input.layoutInCell, true),
            lockAnchor: filter(':bool', input.lockAnchor, true),
            zIndex: filter(':int', input.zIndex, true),
            horizontalPosition: {
                align: input.horizontalAlign,
                relative: input.horizontalRelative,
                offset: filter(':emu', input.horizontalOffset, true),
            },
            verticalPosition: {
                align: input.verticalAlign,
                relative: input.verticalRelative,
                offset: filter(':emu', input.verticalOffset, true),
            },
            margins: {
                bottom: filter(':emu', input.marginBottom, true),
                left: filter(':emu', input.marginLeft, true),
                right: filter(':emu', input.marginRight, true),
                top: filter(':emu', input.marginTop, true),
            },
            wrap: {
                margins: {
                    distB: filter(':emu', input.wrapMarginBottom, true),
                    distL: filter(':emu', input.wrapMarginLeft, true),
                    distR: filter(':emu', input.wrapMarginRight, true),
                    distT: filter(':emu', input.wrapMarginTop, true),
                },
                side: input.wrapSide,
                type: input.wrapType,
            }
        }
    };

    removeUndefinedProperties(options);

    target.push(new docx.ImageRun(options));
}

