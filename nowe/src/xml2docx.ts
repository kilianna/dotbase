
import * as fs from 'node:fs';
import * as path from "node:path";
import * as process from "node:process";
import * as util from 'node:util';
import * as docx from "docx";
import { template } from 'underscore';
import { convert } from './converter';
import { fromTemplate } from './template';
import { writeSources } from './dev';

let debugMode = false;
export let error: string[] = [];

function help() {
    console.log('Usage: xml2doc output.docx template.xml [data.json|data.xml]');
}

async function mainInner(args: string[]) {

    let outputFile = args[0];
    let templateFile = args[1];
    let dataFile = args[2];
    let templateText: string;
    let xmlText: string;

    error.push(`Could not read template file "${templateFile}".`);
    templateText = fs.readFileSync(templateFile, 'utf-8');
    error.pop();

    let templateData: any;
    if (!dataFile) {
        templateData = {};
    } else {
        let dataText: string;

        error.push(`Could not read data file "${dataFile}".`);
        dataText = fs.readFileSync(dataFile, 'utf-8');
        error.pop();

        if (dataFile.toLowerCase().endsWith('.json')) {

            error.push(`Could not parse JSON data file "${dataFile}".`);
            templateData = (new Function(`return ${dataText};`))();
            error.pop();

        } else if (dataFile.toLowerCase().endsWith('.xml')) {
            error.push('XML data file not implemeted.'); // TODO: XML data file support
            throw new Error();
        } else {
            error.push('Only XML and JSON data files allowed.');
            throw new Error();
        }
    }

    if (debugMode) {
        error.push('Could not dump debug file.');
        fs.writeFileSync(outputFile + '.data.json', JSON.stringify(templateData, null, 2));
        error.pop();
    }

    error.push(`Could not create XML from template.`);
    xmlText = fromTemplate(templateFile, templateText, dataFile || '[no data]', templateData);
    error.pop();

    if (debugMode) {
        error.push('Could not dump debug file.');
        fs.writeFileSync(outputFile + '.source.xml', xmlText);
        error.pop();
    }

    error.push('Could not convert to docx.');
    let buffer = await convert(templateFile, xmlText);
    error.pop();

    error.push(`Could not write to output file "${outputFile}".`);
    fs.writeFileSync(outputFile, buffer);
    error.pop();
}

async function main() {

    let args = process.argv.slice(2);

    while (args.length > 0 && args[0].startsWith('-')) {
        if (args[0] == '-h' || args[0] == '--help') {
            help();
            process.exit(0);
        } else if (args[0] == '-d' || args[0] == '--debug') {
            debugMode = true;
        } else if (args[0] == '--sources') {
            writeSources();
            process.exit(0);
        }
        args.shift();
    }

    if (args.length != 3 && args.length != 2) {
        help();
        process.exit(1);
    }

    try {
        await mainInner(args);
    } catch (err) {
        if (error.length) {
            for (let err of error) {
                console.error(err);
            }
        } else {
            console.error('Unexpected error!');
        }
        console.error(`${(err as any).name}: ${(err as any).message}`);
        if (debugMode) {
            throw err;
        } else if ((err as any).code === 'EBUSY') {
            console.error('Output file cannout be open in any other program!');
            process.exit(45);
        }
        process.exit(2);
    }
}

main();
