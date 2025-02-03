
import { parse, Node, Element, Text } from './xml';

const PAGE_BREAK = '\n__PAGE_BREAK__\n';

class Result {
    public header1 = '';
    public header2 = '';
    public footer1 = '';
    public footer2 = '';
    public text = '';
    public output = '';
}

function convertText(result: Result, node: Text) {
    result.output += node.text;
}

function convertChildren(result: Result, element: Element, skipText: boolean) {
    for (let node of element.elements || []) {
        if (skipText && node.type === 'text') continue;
        convertNode(result, node);
    }
}

function convertElement(result: Result, element: Element) {
    let tmp: string;
    switch (element.name.split(':')[0]) {
        case 'document':
            convertChildren(result, element, true);
            result.text = result.output;
            result.output = '';
            break;
        case 'b':
        case 'sub':
        case 'sup':
        case 'i':
        case 'font':
            convertChildren(result, element, false);
            break;
        case 'table':
        case 'tr':
            tmp = result.output;
            result.output = '';
            convertChildren(result, element, true);
            if (!tmp.endsWith('\n')) tmp += '\n';
            result.output = tmp + result.output.trim() + '\n';
            break;
        case 'p':
            tmp = result.output;
            result.output = '';
            convertChildren(result, element, false);
            if (!tmp.endsWith('\n')) tmp += '\n';
            result.output = tmp + result.output.trim() + '\n';
            break;
        case 'td':
            tmp = result.output;
            result.output = '';
            convertChildren(result, element, false);
            result.output = tmp + '\t' + result.output.trim();
            break;
        case 'section':
            tmp = result.output;
            result.output = '';
            convertChildren(result, element, true);
            result.output = tmp;
        case 'footer':
        case 'header':
            tmp = result.output;
            result.output = '';
            convertChildren(result, element, false);
            if (element.name === 'header') {
                if (element.attributes?.page === 'first') {
                    result.header1 = result.output;
                } else {
                    result.header2 = result.output;
                }
            } else {
                if (element.attributes?.page === 'first') {
                    result.footer1 = result.output;
                } else {
                    result.footer2 = result.output;
                }
            }
        case 'br':
            result.output += '\n';
            break;
        case 'tab':
            result.output += '\t';
            break;
        case 'page-number':
            result.output += '1';
            break;
        case 'total-pages':
            result.output += '2';
            break;
        case 'PageBreak':
            result.output += PAGE_BREAK;
            break;
        case 'tc':
        case 'img':
        case 'p-style':
            break;
        default:
            console.error('TODO:', element.name);
            process.exit(1);
    }
}

function convertNode(result: Result, node: Node) {
    switch (node.type) {
        case 'element': return convertElement(result, node);
        case 'text': return convertText(result, node);
        case 'cdata': throw new Error('XML CData not supported');
        case 'instruction': throw new Error('XML instruction not supported');
        default: throw new Error('Not supported');
    }
}

export function convertXmlToText(xmlSource: string): string {
    let root = parse(xmlSource, false, true);
    let result = new Result();
    convertNode(result, root);
    let r = (result.header1 + result.text + result.footer2).replace(PAGE_BREAK, result.footer1 + result.header2);
    return normalizeText(r);
}

export function normalizeText(text: string): string {
    return text
        .replace('‑', '-')
        .replace(/[\x01- \s]+/gi, '\n')
        .replace(/μ/g, 'µ')
        .trim();
}
