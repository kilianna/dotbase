
import { convertXmlToText, normalizeText } from './convert-xml';
import * as fs from 'node:fs';
import * as child_process from 'node:child_process';
import * as util from 'util';

let oldDataBase: { [key: string]: string } = JSON.parse(fs.readFileSync('convert-html/out.json', 'utf-8'));
let idList: number[] = fs.readFileSync('../dotbase/bin/wyniki/Swiadectwo/done.txt', 'utf-8').split(/(?:\r?\n)+/).filter(x => x).map(x => parseInt(x));

function idGroup(x: number): string {
    if (x < 100) return 'xx';
    let r = x.toString();
    return r.substring(0, r.length - 2) + 'xx';
}

function getNewText(id: number): string {
    let xmlFile = `../dotbase/bin/wyniki/Swiadectwo/${idGroup(id)}/${id}SwiadectwoWynik.tmp.docx.executed.xml`;
    if (fs.existsSync(xmlFile)) {
        return convertXmlToText(fs.readFileSync(xmlFile, 'utf-8'));
    }
    let messageFile = `../dotbase/bin/wyniki/Swiadectwo/new-${id}.txt`;
    if (fs.existsSync(messageFile)) {
        return normalizeText(fs.readFileSync(messageFile, 'utf-8'));
    }
    return '[EMPTY]';
}

function getOldText(id: number): string {
    let key = `${id}SwiadectwoWynik.html`;
    if (oldDataBase[key]) {
        return normalizeText(oldDataBase[key]);
    }
    let messageFile = `../dotbase/bin/wyniki/Swiadectwo/old-${id}.txt`;
    if (fs.existsSync(messageFile)) {
        return normalizeText(fs.readFileSync(messageFile, 'utf-8'));
    }
    return '[EMPTY]';
}

type DiffEntry = string | {
    old: string[],
    new: string[],
};

type DiffGroup = DiffEntry[];
type Diff = DiffGroup[];

function parseDiff(diffText: string) {
    let diff: Diff = [];
    let group: DiffGroup = [];
    for (let line of diffText.split(/(?:\r?\n)+/)) {
        if (line.startsWith('@')) {
            group = [];
            diff.push(group);
        } else if (line.startsWith('-')) {
            let last = group.at(-1);
            if (typeof last === 'object') {
                last.old.push(line.substring(1));
            } else {
                group.push({ new: [], old: [line.substring(1)] });
            }
        } else if (line.startsWith('+')) {
            let last = group.at(-1);
            if (typeof last === 'object') {
                last.new.push(line.substring(1));
            } else {
                group.push({ old: [], new: [line.substring(1)] });
            }
        } else if (line.startsWith(' ')) {
            group.push(line.substring(1));
        }
    }
    return diff;
}

function num(text: string): number {
    return parseFloat(text.replace(',', '.'));
}

function replaceRange(entry: DiffEntry | undefined, invert: boolean, min: number, max: number): DiffEntry {
    if (typeof entry !== 'object') return entry as DiffEntry;
    if (entry.new.length != 1) return entry;
    if (entry.old.length != 1) return entry;
    let newNumber = num(entry.new[0]);
    let oldNumber = num(entry.old[0]);
    let result = `${oldNumber}-->${newNumber}`;
    if (invert) {
        [newNumber, oldNumber] = [oldNumber, newNumber];
    }
    if (oldNumber < newNumber) {
        return {
            new: [entry.new[0] + ' - INVALID RANGE'],
            old: entry.old,
        }
    }
    return result;
}

function removeExceptions(diff: Diff): void {
    for (let group of diff) {
        for (let i = 0; i < group.length; i++) {
            let entry = group[i];
            let next1 = group[i + 1] as DiffEntry | undefined;
            let next2 = group[i + 2] as DiffEntry | undefined;
            let next3 = group[i + 3] as DiffEntry | undefined;
            let next4 = group[i + 4] as DiffEntry | undefined;
            let next5 = group[i + 5] as DiffEntry | undefined;

            if (entry === 'Strona'
                && typeof next1 === 'object'
                && next1.new.length === 1 && next1.old.length === 1
                && next1.new[0].match(/^[0-9]\/[0-9]$/) && next1.old[0].match(/^[0-9]\/[0-9]$/)
            ) {
                group[i + 1] = next1.old[0] + '-->' + next1.new[0];
                continue;
            }

            if (typeof entry === 'object'
                && entry.new.length === 1 && entry.new[0] === 'źródłami'
                && entry.old.length === 1 && entry.old[0] === 'źródami'
            ) {
                group[i] = entry.new[0];
                continue;
            }

            if (typeof entry === 'object'
                && entry.new.length === 2
                && entry.old.length === 1 && entry.old[0] === entry.new[0].replace('-', '') + entry.new[1]
            ) {
                group[i] = entry.old[0];
                continue;
            }

            if (typeof entry === 'object'
                && entry.old.length === 2
                && entry.new.length === 1 && entry.new[0] === entry.old[0].replace('-', '') + entry.old[1]
            ) {
                group[i] = entry.new[0];
                continue;
            }

            if (entry === 'Ciśnienie' && next1 === '(' && next3 === '-' && next5 === ')') {
                group[i + 2] = replaceRange(next2, false, 900, 1100);
                group[i + 4] = replaceRange(next4, true, 900, 1100);
                continue;
            }

            if (entry === 'Temperatura' && next1 === '(' && next3 === '-' && next5 === ')') {
                group[i + 2] = replaceRange(next2, false, 1, 45);
                group[i + 4] = replaceRange(next4, true, 1, 45);
                continue;
            }

            if (entry === 'Wilgotność' && next1 === '(' && next3 === '-' && next5 === ')') {
                group[i + 2] = replaceRange(next2, false, 10, 95);
                group[i + 4] = replaceRange(next4, true, 10, 95);
                continue;
            }
        }
    }
    diff.splice(0, diff.length, ...diff.filter(group => group.some(entry => typeof entry === 'object')));
}

function escape(text: string): string {
    return text
        .replace(/&/g, '&amp;')
        .replace(/</g, '&lt;')
        .replace(/>/g, '&gt;');
}


function compare(id: number) {
    let newText = getNewText(id);
    let oldText = getOldText(id);
    fs.writeFileSync('new.txt', newText);
    fs.writeFileSync('old.txt', oldText);
    let diffText: string;
    try {
        diffText = child_process.execSync('git diff -U5 --no-index -- old.txt new.txt', {
            encoding: 'utf-8',
        });
    } catch (e) {
        diffText = e.stdout;
    }
    let diff = parseDiff(diffText);
    removeExceptions(diff);
    let changes = 0;
    for (let group of diff)
        for (let entry of group)
            if (typeof entry === 'object')
                changes++;
    if (changes === 0) return;
    output += `
        <tr class="entry">
            <td class="id">${id}</td>
            <td class="changes">${changes}</td>
        </tr>`;
    for (let group of diff) {
        output += `<tr class="diff"><td colspan="2">`;
        for (let entry of group) {
            if (typeof entry === 'string') {
                output += `<span class="normal">${escape(entry)}</span> `;
            } else {
                for (let word of entry.old)
                    output += `<span class="old">${escape(word)}</span> `;
                for (let word of entry.new)
                    output += `<span class="new">${escape(word)}</span> `;
            }
        }
        output += '</td></tr>';
    }
}


let output = `
    <html>
        <head>
            <link rel="stylesheet" type="text/css" href="result.css">
        </head>
        <body>
            <table>`;

for (let i = 0; i < idList.length; i++) {
    let id = idList[i];
    process.stdout.write(`\r${id} ${Math.round(i / idList.length * 100)}%        `)
    compare(id);
}
process.stdout.write('\n');

output += `
            </table>
        </body>
    </html>`;

fs.writeFileSync('result.html', output);

/*
let c = fs.readFileSync('C:/work/ania/dotbase/dotbase/bin/wyniki/Swiadectwo/190xx/19000SwiadectwoWynik.tmp.docx.executed.xml', 'utf-8');
let a = convertXmlToText(c);

let bt = fs.readFileSync('convert-html/out.json', 'utf-8');
let bo : {[key: string] : string} = JSON.parse(bt);
let b = normalizeText(bo["19000SwiadectwoWynik.html"]);

fs.writeFileSync('a.txt', a);
fs.writeFileSync('b.txt', b);

console.log(a);
*/