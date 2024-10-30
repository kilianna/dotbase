

function main() {
    let output = {};
    let i = 0;
    let body = document.body;
    for (let [key, value] of Object.entries(input)) {
        let m = value.match(/<body>([\s\S]*)<\/body>/i);
        if (!m) throw new Error();
        body.innerHTML = m[1];
        let t = body.innerText;
        output[key] = t;
        console.log(`${++i} of ${Object.keys(input).length}: ${key}, length: ${m[1].length} => ${t.length}`);
    }
    let o = JSON.stringify(output, undefined, 2);
    console.log(`Total ${o.length}`);
    body.innerHTML = `<textarea style="width: 100%; height: 700px"></textarea>`;
    body.querySelector('textarea').value = o;
}

window.onload = () => { main(); };

