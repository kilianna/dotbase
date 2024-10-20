

async function main() {
    let time = Date.now();
    let counterNode = document.querySelector('#cnt');;
    let textNode = document.querySelector('#text');;
    let res = await fetch('/list');
    let fileList = await res.json();
    for (let i = 0; i < fileList.length; i++) {
        counterNode.innerHTML = `${i+1} of ${fileList.length}`;
        let file = fileList[i];
        let res = await fetch('/html/' + file);
        let value = await res.text();
        let m = value.match(/<body>([\s\S]*)<\/body>/i);
        if (!m) throw new Error();
        textNode.innerHTML = m[1];
        let text = textNode.innerText;
        textNode.innerHTML = '';
        console.log(text.length);
        res = await fetch('/out', {
            method: 'POST', // or 'PUT'
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                file,
                text,
            }),
        });
        value = await res.text();
        console.log(value);
    }
    fetch('/close');
    counterNode.innerHTML = `Done`;
}

window.onload = () => { main(); };

