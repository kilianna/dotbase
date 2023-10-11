const fs = require('node:fs');

let files = {
    'dist/xml2docx.js': fs.readFileSync('dist/xml2docx.js', 'utf-8'),
    'dist/xml2docx.js.map': fs.readFileSync('dist/xml2docx.js.map', 'utf-8'),
    'package.json': fs.readFileSync('package.json', 'utf-8'),
};

for (let file of fs.readdirSync('src')) {
    if (file.endsWith('.ts') || file.endsWith('.js')) {
        files[`src/${file}`] = fs.readFileSync(`src/${file}`, 'utf-8');
    }
}

for (let file of fs.readdirSync('scripts')) {
    if (file.endsWith('.ts') || file.endsWith('.js')) {
        files[`scripts/${file}`] = fs.readFileSync(`scripts/${file}`, 'utf-8');
    }
}

fs.writeFileSync('dist/map.js', `exports.files = ${JSON.stringify(files)};`);

let src = files['dist/xml2docx.js'].replace('(eaDsfsDe9f)', '("./map.js")');
fs.writeFileSync('dist/xml2docx.js', src);
