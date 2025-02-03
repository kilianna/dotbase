
import * as fs from 'fs';

let DIR = 'C:\\work\\ania\\dotbase\\dotbase\\bin\\wyniki\\Swiadectwo\\';

let out = {};

for (let file of fs.readdirSync(DIR)) {
    if (file.match(/^[0-9]+SwiadectwoWynik\.html$/i)) {
        let c = fs.readFileSync(DIR + file, 'utf8');
        out[file] = c;
    }
}

fs.writeFileSync('convert-html/in.js', `let input = ${JSON.stringify(out, undefined, 2)}`);
