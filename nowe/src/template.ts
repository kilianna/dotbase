import { template } from "underscore";
import { error } from "./xml2docx";
import path from "node:path";
import * as fs from "node:fs";

const commonUtils = {
    templateFile: '',
    dataFile: '',
    data: ({} as any),
    templateDir: '',
    include: function (fileName: string) {
        error.push(`Could not read include file "${fileName}".`);
        let includeFile = path.resolve(this.templateDir, fileName);
        let templateFile = fs.readFileSync(includeFile, 'utf-8');
        error.pop();
        error.push(`Could not create XML from template.`);
        let xmlText = fromTemplate(includeFile, templateFile, this.dataFile, this.data);
        error.pop();
        return xmlText;
    }
};

export function fromTemplate(templateFile: string, templateText: string, dataFile: string, data: any) {

    error.push(`Could not parse template "${templateFile}".`);
    let compiled = template(templateText);
    error.pop();

    error.push(`Could not execute template "${templateFile}" with data from "${dataFile}".`);
    let utils:{[key:string]:any} = {...commonUtils};
    utils.templateFile = templateFile;
    utils.dataFile = dataFile;
    utils.data = data;
    utils.templateDir = path.dirname(templateFile);
    let result = compiled({ utils: utils, ...data, __utils__: utils });
    error.pop();

    return result;
}
