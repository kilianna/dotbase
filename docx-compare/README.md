
Wymaganie:
    Terminal z dostępnym `node.js` i `git`.

1. Konwersja HTML -> TXT
    * Uruchom test w programie dotbase, żeby wygenerować pliki wejściowe.
    * Uruchom `node convert-html.mjs`
    * Otwórz `convert-html/index.html` w przegądarce
    * Skopiuj wyjście do `convert-html/out.json`
2. Porównanie
    * Otwórz terminal gdzie `git` jest dostępny
    * Uruchom `npx tsx compare.ts`
    * Wyniki są w `result.html`
