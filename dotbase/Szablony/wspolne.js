
const ustawieniaJezykow = {
    PL: {
        alfabet: 'abcdefghijklmnoprstuwyz',
        kropka: ',',
    },
    EN: {
        alfabet: 'abcdefghijklmnopqrstuvwxyz',
        kropka: '.',
    }
}

ustawieniaJezyka = ustawieniaJezykow[jezyk];

function escape(unsafe)
{
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
        return x.tekst[jezyk];
    } else {
        throw Error('Nie da się tej wartości przekształcić na tekst.');
    }
}

function multiline(x) {
    return x
        .trim()
        .replace(/&mu;/g, 'μ')
        .replace(/&nbsp;/g, '\xA0')
        .split(/\r?\n/)
        .map(v => escape(v))
        .join('<br/>');
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
    return x.replace(/u/g, 'μ');
}

function blad(x) {
    console.log(`ERROR: ${x}`);
    return `⚠ ⚠ 𝐄𝐑𝐑𝐎𝐑: ${x} ⚠ ⚠`;
}

function _getSignificant(digits, significant, data) {
    let minSignificant = Infinity;
    let cutOff = Infinity;
    for (let num of data) {
        num *= 1;
        let str = num.toFixed(digits)
            .replace(/\./, '')
            .replace(/^0+/, '');
        minSignificant = Math.min(minSignificant, str.length);
        cutOff = Math.min(cutOff, Math.max(str.match(/0*$/)[0].length, str.length - significant));
    }
    return [minSignificant, cutOff];
}

function _fractionDigits(significant, data) {

    let minSignificant, cutOff;
    let digits = 20;

    for (let i = 0; i < 20; i++) {
        [minSignificant, cutOff] = _getSignificant(i, significant, data);
        if (minSignificant >= significant) {
            digits = i;
            let [minSignificant1, cutOff1] = _getSignificant(i + 1, significant, data);
            if (minSignificant1 == significant) {
                digits = i + 1;
                cutOff = cutOff1;
            }
            break;
        }
    }
    digits = Math.max(0, digits - cutOff);
    return digits;
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

function calcDigits(...args) {
    let digits = 0;
    let min = 0;
    let max = 20;
    for (let i = 0; i < args.length; ) {
        let significant = args[i++];
        if (typeof significant === 'object') {
            if (significant.min !== undefined) min = significant.min;
            if (significant.max !== undefined) max = significant.max;
            continue;
        }
        let data = args[i++];
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
