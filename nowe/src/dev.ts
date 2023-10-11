import * as fs from 'node:fs';
import path from 'node:path';

export function writeSources() {
    let eaDsfsDe9f='./map.js';
    const mod = require(eaDsfsDe9f);
    for (let [file, content] of Object.entries(mod.files)) {
        file = '_src/' + file;
        let dir = path.dirname(file);
        try {
            fs.mkdirSync(dir, { recursive: true });
        } catch (err) { }
        fs.writeFileSync(file, content as string);
        console.log(`Written to ${file}, bytes: ${(content as string).length}`);
    }
}
