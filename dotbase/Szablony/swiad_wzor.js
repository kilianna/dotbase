
// ------------------------------------ STAŁE ------------------------------------

const metody = {
	wzor1: 'Procedura pomiarowa WZOR-1 (wyd. 7 z dn. 15.09.2023)',
	wzor2: 'Procedura pomiarowa WZOR-2 (wyd. 6 z dn. 15.09.2023)',
	wzor1wzor2: 'Procedura pomiarowa WZOR-1 (wyd. 7 z dn. 15.09.2023) oraz procedura pomiarowa WZOR-2 (wyd. 6 z dn. 15.09.2023)',
};

const spojnosciPomiarowe = {
    gum: 'Świadectwo jest wydane w ramach porozumienia EA MLA w zakresie wzorcowania i potwierdza spójność wyników pomiarów z wzorcami utrzymanymi w GUM.',
    si: 'Świadectwo jest wydane w ramach porozumienia EA MLA w zakresie wzorcowania i potwierdza spójność wyników pomiarów z jednostkami miar Międzynarodowego Układu Jednostek Miar (SI).',
};

// ------------------------------------ METODA ------------------------------------

let ogolemIlosc = tabelaS.length + tabelaMD.length + tabelaD.length + tabelaSM.length + tabelaSD.length;

metoda =
    tabelaS.length > 0 && ogolemIlosc > tabelaS.length ? metody.wzor1wzor2 :
    tabelaS.length > 0 ? metody.wzor2 :
	metody.wzor1;

if (ogolemIlosc == 0)
{
    throw new Error("Nie ma wierszy w żadnej z tabel: MD, D, SM, SD, S!");
}
    
// ------------------------------ Warunki: temp., ciśn., wilgo. ------------------------------

tabeleMax = {};
tabeleMin = {};
for (let wiersz of [...tabelaMD, ...tabelaD, ...tabelaSM, ...tabelaSD, ...tabelaS]) {
    for (let nazwa in wiersz) {
        tabeleMax[nazwa] = Math.max(...[wiersz[nazwa], tabeleMax[nazwa]].filter(x => x !== undefined));
        tabeleMin[nazwa] = Math.min(...[wiersz[nazwa], tabeleMin[nazwa]].filter(x => x !== undefined));
    }
}

// ------------------------------ Nr świadectwa ------------------------------

nr_swiadectwa =  nr_karty + '/' + data_wydania.rok;
if (poprawa) {
    nr_swiadectwa += 'P';
}

// ------------------------------ Spojnosci Pomiarowe ------------------------------

spojnoscPomiarowa =
    tabelaMD.length == ogolemIlosc && jednostki && !jednostki.si
    ? spojnosciPomiarowe.gum
    : spojnosciPomiarowe.si;


/* TODO:

                var skazeniaIlosc = szablon.tabelaS.Count;
                var nieSkazeniaIlosc = szablon.tabelaMD.Count + szablon.tabelaD.Count + szablon.tabelaSM.Count + szablon.tabelaSD.Count;

                szablon.metoda = // TODO: Może to można przenieść do szablonu?
                    skazeniaIlosc > 0 && nieSkazeniaIlosc > 0 ? swiad_wzor.Metoda.Wzor1Wzor2 :
                    skazeniaIlosc > 0 ? swiad_wzor.Metoda.Wzor2 :
                    swiad_wzor.Metoda.Wzor1;



                // ------------------------------ Warunki: temp., ciśn., wilgo. ------------------------------

                var pierwszyWiersz =
                    szablon.tabelaMD.Count > 0 ? szablon.tabelaMD[0] :
                    szablon.tabelaD.Count > 0 ? szablon.tabelaD[0] :
                    szablon.tabelaSM.Count > 0 ? szablon.tabelaSM[0] :
                    szablon.tabelaSD.Count > 0 ? szablon.tabelaSD[0] :
                    szablon.tabelaS.Count > 0 ? szablon.tabelaS[0] :
                    null;

                if (pierwszyWiersz == null)
                {
                    throw new ApplicationException("Brak wierszy");
                }

                var value = pierwszyWiersz.Field<double>(0);
                szablon.cisnienie_min = value - Constants.getInstance().UNCERTAINITY_PRESSURE_VALUE;
                szablon.cisnienie_max = value + Constants.getInstance().UNCERTAINITY_PRESSURE_VALUE;
                value = pierwszyWiersz.Field<double>(1);
                szablon.temperatura_min = value - Constants.getInstance().UNCERTAINITY_TEMPERATURE_VALUE;
                szablon.temperatura_max = value + Constants.getInstance().UNCERTAINITY_TEMPERATURE_VALUE;
                value = pierwszyWiersz.Field<double>(2);
                szablon.wilgotnosc_min = value - Constants.getInstance().UNCERTAINITY_HUMIDITY_VALUE;
                szablon.wilgotnosc_max = value + Constants.getInstance().UNCERTAINITY_HUMIDITY_VALUE;
*/

