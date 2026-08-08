
let _data;
let _utils;

function initialize(utils) {
    _utils = utils;
    _data = utils.data;
    try {
        ustawieniaJezyka = ustawieniaJezykow[_data.jezyk];
    } catch (e) {
        ustawieniaJezyka = undefined;
    }
    if (!ustawieniaJezyka) {
        ustawieniaJezyka = ustawieniaJezykow.PL;
    }
    ustawieniaJezyka.slownik = Object.create(null);
    for (let texts of _data.slownik) {
        let translated = texts[ustawieniaJezyka.jezyk];
        if (translated) {
            ustawieniaJezyka.slownik[texts.PL] = translated;
        }
    }
}

const ustawieniaJezykow = {
    PL: {
        jezyk: 'PL',
        alfabet: 'abcdefghijklmnoprstuwyz',
        kropka: ',',
        slownik: {},
    },
    EN: {
        jezyk: 'EN',
        alfabet: 'abcdefghijklmnopqrstuvwxyz',
        kropka: '.',
        slownik: {},
    }
}

let ustawieniaJezyka;

function escape(unsafe) {
    return unsafe
        .replace(/&/g, "&amp;")
        .replace(/</g, "&lt;")
        .replace(/>/g, "&gt;");
}

function tekst(x, y) {
    if (typeof x === 'number') {
        y = y || 6;
        let sign = Math.sign(x);
        let abs = Math.abs(x);
        let max = Math.pow(10, y - 1);
        if (Math.round(abs) >= max) {
            return (sign * Math.round(abs)).toFixed(0);
        }
        if (abs < 1e-20) {
            return x.toString();
        }
        let frac = 0;
        while (abs < max) {
            max /= 10;
            frac++;
        }
        if (frac > 20) {
            return x.toString();
        }
        let result = x.toFixed(frac);
        while (result.indexOf('.') >= 0 && (result[result.length - 1] == '0' || result[result.length - 1] == '.')) {
            result = result.substring(0, result.length - 1);
        }
        result = result.replace('.', ustawieniaJezyka.kropka);
        return result;
    } else if (typeof x === 'string') {
        return x;
    } else if (typeof x !== 'object') {
        throw Error('Nie da się tej wartości przekształcić na tekst.');
    } else if (x instanceof Array) {
        return x.map(v => tekst(v, y)).join(', ');
    } else if (typeof x.tekst === 'object') {
        y = y || 'tekst';
        return x[y][jezyk];
    } else {
        throw Error('Nie da się tej wartości przekształcić na tekst.');
    }
}

function multiline(x, sep) {
    if (sep) {
        return x
            .split(new RegExp(`\\s*[\\r\\n${sep}]\\s*`))
            .map(v => escape(v.trim()))
            .join('<br/>');
    } else {
        return x
            .split(/\r?\n/)
            .map(v => escape(v.trim()))
            .join('<br/>');
    }
}

function litera(x) {
    const alfabet = ustawieniaJezyka.alfabet;
    if (x < alfabet.length) {
        return alfabet[x];
    } else {
        return alfabet[Math.floor(x / alfabet.length) - 1] + alfabet[x % alfabet.length];
    }
}

function mikro(x) {
    return x.replace(/u/g, 'µ');
}

function blad(x) {
    console.log(`ERROR: ${x}`);
    return `⚠ ⚠ 𝐄𝐑𝐑𝐎𝐑: ${x} ⚠ ⚠`;
}

function _fractionDigits(significant, data) {
    let result = 0;
    for (let x of data) {
        let digits;
        for (digits = 20; digits > 0; digits--) {
            let str = x.toFixed(digits)
                .replace(/[.-]/g, '')
                .replace(/^0+/, '');
            if (str.length <= significant) {
                break;
            }
        }
        if (digits == 20) {
            continue;
        }
        result = Math.max(result, digits);
    }
    return result;
}

function _flattenAbs(value, arr) {
    if (value instanceof Array) {
        for (let x of value) {
            _flattenAbs(x, arr);
        }
    } else {
        arr.push(Math.abs(1 * value));
    }
    return arr;
}

let currentDigits = 0;

function toNumber(x) {
    if (typeof x === 'number') {
        return x;
    } else if (typeof x === 'string') {
        return parseFloat(x.replace(',', '.').trim());
    } else if (Array.isArray(x)) {
        return x.map(v => toNumber(v));
    } else {
        return NaN;
    }
}

function calcDigits(...args) {
    let digits = 0;
    let min = 0;
    let max = 20;
    for (let i = 0; i < args.length;) {
        let significant = args[i++];
        if (typeof significant === 'object') {
            if (significant.min !== undefined) min = significant.min;
            if (significant.max !== undefined) max = significant.max;
            continue;
        }
        let data = toNumber(args[i++]);
        data = _flattenAbs(data, []).filter(x => x > 1e-20);
        if (data.length == 0) continue;
        let fd = _fractionDigits(significant, data);
        digits = Math.max(digits, fd);
    }
    currentDigits = Math.min(max, Math.max(min, digits));
    return currentDigits;
}

function fixed(value, digits) {
    let result;
    value = toNumber(value);
    if (digits === undefined) { // TODO: Skip "digits" argument where possible
        digits = currentDigits;
    }
    if (value instanceof Array) {
        result = value.map(x => fixed(x, digits));
    } else {
        result = (1 * value).toFixed(digits);
    }
    result = result.replace('.', ustawieniaJezyka.kropka);
    return result;
}

function numberAsIs(value) {
    value = toNumber(value);
    const numberSignificantDigits = 13;
    let intDigits = Math.abs(value).toFixed(0).length;
    let fracDigits = Math.min(20, Math.max(0, numberSignificantDigits - intDigits));
    return value.toFixed(fracDigits)
        .replace(/0/g, ' ').trimEnd().replace(/ /g, '0')
        .replace('.', ' ').trimEnd().replace(' ', ustawieniaJezyka.kropka);
}

function nbsp(text) {
    return text.replace(/\s/g, ' ');
}

function simpleHtml(x, paragraph) {
    let tokens = x
        .split(/(&[a-z]+;|[ \t]*\r?\n|<\/?[a-z]+(?:[^a-z>][^>]*)?>)/gi)
        .filter(x => x.length);
    let res = '';
    let afterBreak = false;
    let tagsStack = [];
    nextTokenLoop:
    for (let token of tokens) {

        if (token.startsWith('&')) {
            afterBreak = false;
            switch (token.toLowerCase()) {
                case '&nbsp;':
                case '&amp;':
                case '&lt;':
                case '&gt;':
                case '&quot;':
                    res += token.toLowerCase();
                    continue nextTokenLoop;
                case '&mu;':
                    res += 'µ'
                    continue nextTokenLoop;
            }
        }

        if (token.endsWith('\n')) {
            if (!afterBreak) {
                res += '<br/>';
            }
            afterBreak = false;
            continue nextTokenLoop;
        }

        if (token.startsWith('</')) {
            afterBreak = false;
            let name = token.match(/^<\/([a-z]+)/i)[1].toLowerCase();
            let index = tagsStack.lastIndexOf(name);
            if (index >= 0) {
                while (tagsStack.length > index) {
                    let name = tagsStack.pop();
                    res += `</${name}>`;
                }
                continue nextTokenLoop;
            }
        } else if (token.startsWith('<')) {
            afterBreak = false;
            let name = token.match(/^<([a-z]+)/i)[1].toLowerCase();
            switch (name) {
                case 'br':
                    res += '<br/>';
                    afterBreak = true;
                    continue nextTokenLoop;
                case 'b':
                case 'i':
                case 'u':
                case 'sup':
                case 'sub':
                    res += `<${name}>`;
                    tagsStack.push(name);
                    continue nextTokenLoop;
            }
        }

        afterBreak = false;

        res += escape(token);
    }
    while (tagsStack.length > 0) {
        let name = tagsStack.pop();
        res += `</${name}>`;
    }
    if (paragraph) {
        res = res
            .split('<br/>')
            .map(x => paragraph.replace('***', x))
            .join('');
    }
    return res;
}

function nb(text) {
    return text
        .replace(/ /g, ' ')
        .replace(/-/g, '‑');
}

let numeracja = {};

function numeruj(name) {
    name = name || 'default';
    if (!numeracja[name]) {
        numeracja[name] = 0;
    }
    return ++numeracja[name];
}

function zerujNumeracje(name) {
    name = name || 'default';
    numeracja[name] = 0;
}

let tr_formatters = [
    (text) => {
        // no change
        return text;
    },
    (text) => {
        // lowercase
        return text.toLowerCase();
    },
    (text) => {
        // uppercase
        return text.toUpperCase();
    },
    (text) => {
        // First letter uppercase
        return text.charAt(0).toUpperCase() + text.slice(1).toLowerCase();
    },
    (text) => {
        // First letter of each word uppercase
        return text.replace(/\b\w/g, c => c.toUpperCase());
    }
];

function tr(text, allowNoTranslation) {

    /*
    using System.Text.RegularExpressions;

string CapitalizeWords(string text)
{
    return Regex.Replace(text, @"\b\w", m => m.Value.ToUpper());
}
    */
    text = text.trim();

    if (text.match(/^[0-9.,-]+$/)) {
        return text.replace(/[.,]/g, ustawieniaJezyka.kropka);
    }

    if (ustawieniaJezyka.jezyk === 'PL') {
        return text;
    }

    for (let formatter of tr_formatters) {
        for (let [pl, other] of Object.entries(ustawieniaJezyka.slownik)) {
            if (formatter(pl) === text) {
                return formatter(other);
            }
        }
    }

    if (allowNoTranslation) {
        return text;
    } else {
        console.log(`TRANSLATE: {{{${text}}}}`);
        return `⚠ ⚠ 𝐍𝐎 𝐓𝐑𝐀𝐍𝐒𝐋𝐀𝐓𝐈𝐎𝐍: ${text} ⚠ ⚠`;
    }
}


function trOpt(text) {
    return tr(text, true);
}

globalThis.initialize = initialize;
globalThis.escape = escape;
globalThis.tekst = tekst;
globalThis.multiline = multiline;
globalThis.litera = litera;
globalThis.mikro = mikro;
globalThis.blad = blad;
globalThis._fractionDigits = _fractionDigits;
globalThis._flattenAbs = _flattenAbs;
globalThis.toNumber = toNumber;
globalThis.calcDigits = calcDigits;
globalThis.fixed = fixed;
globalThis.numberAsIs = numberAsIs;
globalThis.nbsp = nbsp;
globalThis.simpleHtml = simpleHtml;
globalThis.nb = nb;
globalThis.test = test;
globalThis.numeruj = numeruj;
globalThis.zerujNumeracje = zerujNumeracje;
globalThis.tr = tr;
globalThis.trOpt = trOpt;


function test() {

    function test_simpleHtml() {
        console.log(simpleHtml('test'));
        console.log(simpleHtml('test<br />'));
        console.log(simpleHtml(`<b><i>bold</i></b>
            some<br>
            asdfdskfjslkdjf<br>  
            asdfasf<break>    
            ssadsd<BR> fg
            df<b>i</b><i>
            gdfg
            &amp;
            &mu;
            `));
    }

    function test_tr() {
        console.log(ustawieniaJezyka);
        console.log(tr('mocy kermy w powietrzu'));
        console.log(tr('Mocy kermy w powietrzu'));
        console.log(tr('Mocy Kermy W Powietrzu'));
        console.log(tr('dozymetr'));
        console.log(tr('Dozymetr \t'));
        console.log(tr(' DOZYMETR '));
        console.log(tr('Nowe wyrażenie'));
        console.log(tr('2024'));
    }

    //test_simpleHtml();
    test_tr();
}

function test_get_utils() {
    let fs = require('fs');
    console.log('ERROR: If you see this message outside of a test environment, something is wrong.');
    let csFiles = new Set();
    let jsonFiles = new Set();
    for (let file of fs.readdirSync(__dirname)) {
        if (file.endsWith('.cs')) {
            csFiles.add(file.substring(0, file.length - 3));
        } else if (file.endsWith('.json')) {
            jsonFiles.add(file.substring(0, file.length - 5));
        }
    }
    let commonFiles = new Set([...csFiles].filter(x => jsonFiles.has(x)));
    if (commonFiles.size === 0) {
        throw new Error('No common .cs and .json files found in the directory.');
    }
    let commonFile = [...commonFiles][0];
    console.log(`Using JSON file: ${commonFile}.json`);
    let utils = {
        data: JSON.parse(fs.readFileSync(__dirname + '/' + commonFile + '.json', 'utf8')),
    };
    utils.data.jezyk = 'EN';
    return utils;
}


if (typeof __utils__ !== 'undefined') {
    initialize(__utils__);
} else {
    initialize(test_get_utils());
    test();
}
