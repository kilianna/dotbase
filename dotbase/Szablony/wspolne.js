
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
    return x.trim().split(/\r?\n/).map(v => escape(v)).join('<br/>');
}
