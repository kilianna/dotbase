import * as yaml from 'yaml';

let value;

value = "te\nst long long long long long long long long long long long long long long long long long long long long long long long long long long long long long long long long long long long long long long long long long long long long long long long ";
/*value = [1, 2, 3, 'ab\nc',
    'long long long lon',
    {
        abc: 123,
        'next: yes': null,
    }
];*/

console.log(yaml.stringify(value, {
    blockQuote: false,
    collectionStyle: 'flow',
    defaultStringType: 'QUOTE_DOUBLE',
    doubleQuotedMinMultiLineLength: 1000000000,
    lineWidth: 2000000000,
    flowCollectionPadding: false,
}));
