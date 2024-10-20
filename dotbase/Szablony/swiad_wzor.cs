using System;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;

namespace DotBase.Szablony
{
    class swiad_wzor : DocxData
    {
        public enum Metoda
        {
            wzor1,
            wzor2,
            wzor1wzor2,
        };
        
        public enum SpojnoscPomiarowa
        {
            gum,
            si,
        };

        public class Warunki
        {
            public double cisnienie;
            public double wilgotnosc;
            public double temperatura;
            public Warunki(double x)
            {
                cisnienie = wilgotnosc = temperatura = x;
            }
            public static double Min(double a, double? b)
            {
                return (b == null) ? a : Math.Min(a, (double)b);
            }
            public static double Max(double a, double? b)
            {
                return (b == null) ? a : Math.Max(a, (double)b);
            }
        }

        public class Wynik
        {
            public double prog;
            public double wartosc_zmierzona;
            public double wartosc_wzorcowa;
            public double niepewnosc;
            public double wspolczynnik;
            public double niepewnosc_wspolczynnika;
            public double zakres;
            public int wielkosc_fizyczna;
        }

        public class Wyniki
        {
            public Szablon.Row_wzorcowanie_cezem wzorcowanie_cezem;
            public Szablon.Row_Wzorcowanie_zrodlami_powierzchniowymi wzorcowanie_zrodlami_powierzchniowymi;
            public Szablon.Row_Sondy sonda;
            public Szablon.Row_Jednostki jednostka;
            public List<Wynik> tabela = new List<Wynik>();
        }

        // Dane wejściowe
        public int nr_karty;
        public DateTime data_wydania;
        public DateTime data_wykonania;
        public string sprawdzil;
        public bool poprawa;
        public string uwMD;
        public string uwD;
        public string uwS;
        public string uwSMD;
        public string uwSD;

        // Dane wyliczone w PreProcess
        public Metoda metoda;
        public SpojnoscPomiarowa spojnoscPomiarowa;
        public Warunki warunkiMin;
        public Warunki warunkiMax;
        public Szablon.Row_Dozymetry przyrzad;
        public Szablon.Row_Karta_przyjecia kartaPrzyjecia;
        public Szablon.Row_Zlecenia zlecenie;
        public Szablon.Row_Zleceniodawca zleceniodawca;
        public List<Wyniki> wyniki;

        protected override string FileName
        {
            get {
                return String.Format(@"..\wyniki\Swiadectwo\{0}\{1}SwiadectwoWynik{2}.docx", DirGroup(nr_karty), nr_karty, JezykTools.kocowka(jezyk));
            }
        }

        protected override bool PreProcess(IWin32Window owner)
        {
            kartaPrzyjecia = baza.Karta_przyjecia
                .WHERE().ID_karty(nr_karty)
                .GET_ONE();

            przyrzad = baza.Dozymetry
                .WHERE().ID_dozymetru(kartaPrzyjecia.ID_dozymetru)
                .GET_ONE();

            zlecenie = baza.Zlecenia
                .WHERE().ID_zlecenia(kartaPrzyjecia.ID_zlecenia)
                .GET_ONE();

            zleceniodawca = baza.Zleceniodawca
                .WHERE().ID_zleceniodawcy(zlecenie.ID_zleceniodawcy)
                .GET_ONE();

            var tabelaMD = baza.wzorcowanie_cezem
                .WHERE()
                    .ID_karty(nr_karty)
                    .Rodzaj_wzorcowania("md")
                    .Dolacz(true)
                .GET();

            var tabelaD = baza.wzorcowanie_cezem
                .WHERE()
                    .ID_karty(nr_karty)
                    .Rodzaj_wzorcowania("d")
                    .Dolacz(true)
                .GET();

            var tabelaSM = baza.wzorcowanie_cezem
                .WHERE()
                    .ID_karty(nr_karty)
                    .Rodzaj_wzorcowania("sm")
                    .Dolacz(true)
                .GET();

            var tabelaSD = baza.wzorcowanie_cezem
                .WHERE()
                    .ID_karty(nr_karty)
                    .Rodzaj_wzorcowania("sd")
                    .Dolacz(true)
                .GET();

            var tabelaS = baza.Wzorcowanie_zrodlami_powierzchniowymi
                .WHERE()
                    .ID_karty(nr_karty)
                    .Dolacz(true)
                .GET();

            var ogolemIlosc = tabelaMD.Length + tabelaD.Length + tabelaSM.Length + tabelaSD.Length + tabelaS.Length;

            warunkiMin = new Warunki(999999);
            warunkiMax = new Warunki(-999999);

            foreach (var tabela in new Szablon.Row_wzorcowanie_cezem[][] { tabelaMD, tabelaD, tabelaSM, tabelaSD }) {
                foreach (var row in tabela) {
                    warunkiMin.temperatura = Warunki.Min(warunkiMin.temperatura, row.Temperatura);
                    warunkiMin.wilgotnosc = Warunki.Min(warunkiMin.wilgotnosc, row.wilgotnosc);
                    warunkiMin.cisnienie = Warunki.Min(warunkiMin.cisnienie, row.Cisnienie);
                    warunkiMax.temperatura = Warunki.Max(warunkiMax.temperatura, row.Temperatura);
                    warunkiMax.wilgotnosc = Warunki.Max(warunkiMax.wilgotnosc, row.wilgotnosc);
                    warunkiMax.cisnienie = Warunki.Max(warunkiMax.cisnienie, row.Cisnienie);
                }
            }

            foreach (var row in tabelaS)
            {
                warunkiMin.temperatura = Warunki.Min(warunkiMin.temperatura, row.Temperatura);
                warunkiMin.wilgotnosc = Warunki.Min(warunkiMin.wilgotnosc, row.Wilgotnosc);
                warunkiMin.cisnienie = Warunki.Min(warunkiMin.cisnienie, row.Cisnienie);
                warunkiMax.temperatura = Warunki.Max(warunkiMax.temperatura, row.Temperatura);
                warunkiMax.wilgotnosc = Warunki.Max(warunkiMax.wilgotnosc, row.Wilgotnosc);
                warunkiMax.cisnienie = Warunki.Max(warunkiMax.cisnienie, row.Cisnienie);
            }

            metoda =
                tabelaS.Length > 0 && ogolemIlosc > tabelaS.Length ? Metoda.wzor1wzor2 :
                tabelaS.Length > 0 ? Metoda.wzor2 :
                Metoda.wzor1;

            spojnoscPomiarowa = SpojnoscPomiarowa.si;
            if (tabelaMD.Length > 0 && tabelaMD.Length == ogolemIlosc)
            {
                var jednostka = baza.Jednostki
                    .SELECT().SI()
                    .WHERE().ID_jednostki(tabelaMD[0].ID_jednostki)
                    .GET_OPTIONAL();
                if (jednostka != null && jednostka.SI)
                    spojnoscPomiarowa = SpojnoscPomiarowa.gum;
            }

            wyniki = new List<Wyniki>();

            foreach (var row in tabelaMD) DodajWyniki(row);
            foreach (var row in tabelaD) DodajWyniki(row);
            foreach (var row in tabelaSM) DodajWyniki(row);
            foreach (var row in tabelaSD) DodajWyniki(row);
            foreach (var row in tabelaS) DodajWyniki(row);

            return true;
        }

        private void DodajWyniki(Szablon.Row_Wzorcowanie_zrodlami_powierzchniowymi row)
        {
            var wyn = new Wyniki();
            wyn.wzorcowanie_zrodlami_powierzchniowymi = row;

            wyn.sonda = baza.Sondy
                .WHERE()
                    .ID_sondy(row.ID_sondy ?? -1)
                .GET_OPTIONAL();

            wyn.jednostka = baza.Jednostki
                .WHERE().ID_jednostki(row.ID_jednostki ?? -1)
                .GET_OPTIONAL();

            wyniki.Add(wyn);
        }

        private void DodajWyniki(Szablon.Row_wzorcowanie_cezem row)
        {
            var wyn = new Wyniki();
            wyn.wzorcowanie_cezem = row;

            wyn.sonda = baza.Sondy
                .WHERE().ID_sondy(row.ID_sondy ?? -1)
                .GET_OPTIONAL();

            wyn.jednostka = baza.Jednostki
                .WHERE().ID_jednostki(row.ID_jednostki)
                .GET_OPTIONAL();

            if (row.Rodzaj_wzorcowania == "md")
            {
                DodajTabeleMD(row, wyn);
            }
            else if (row.Rodzaj_wzorcowania == "d")
            {
                DodajTabeleD(row, wyn);
            }
            else if (row.Rodzaj_wzorcowania == "sm")
            {
                DodajTabeleSM(row, wyn);
            }
            else if (row.Rodzaj_wzorcowania == "sd")
            {
                DodajTabeleSD(row, wyn);
            }

            wyniki.Add(wyn);
        }

        private void DodajTabeleMD(Szablon.Row_wzorcowanie_cezem row, Wyniki wyn)
        {
            var tab = baza.wyniki_moc_dawki
                .WHERE().ID_wzorcowania(row.ID_wzorcowania)
                .GET();
            for (int i = 0; i < tab.Length; i++)
            {
                var tabRow = tab[i];
                var w = new Wynik();
                w.zakres = Wymagany(tabRow.ZAKRES, "Zakres w tabeli 'Dawka' jest wymagany");
                w.wspolczynnik = Wymagany(tabRow.Wspolczynnik, "Wspolczynnik w tabeli 'Dawka' jest wymagany");
                w.niepewnosc = Wymagany(tabRow.Niepewnosc, "Niepewnosc w tabeli 'Dawka' jest wymagana");
                wyn.tabela.Add(w);
            }
        }

        private void DodajTabeleD(Szablon.Row_wzorcowanie_cezem row, Wyniki wyn)
        {
            var tab = baza.Wyniki_dawka
                .WHERE().ID_wzorcowania(row.ID_wzorcowania)
                .GET();
            for (int i = 0; i < tab.Length; i++)
            {
                var tabRow = tab[i];
                var w = new Wynik();
                w.zakres = Wymagany(tabRow.Zakres, "Zakres w tabeli 'Dawka' jest wymagany");
                w.wspolczynnik = Wymagany(tabRow.Wspolczynnik, "Wspolczynnik w tabeli 'Dawka' jest wymagany");
                w.niepewnosc = Wymagany(tabRow.Niepewnosc, "Niepewnosc w tabeli 'Dawka' jest wymagana");
                w.wielkosc_fizyczna = Wymagany(tabRow.Wielkosc_fizyczna, "Wielkosc_fizyczna w tabeli 'Dawka' jest wymagana");
                wyn.tabela.Add(w);
            }
        }

        private void DodajTabeleSM(Szablon.Row_wzorcowanie_cezem row, Wyniki wyn)
        {
            IList<double> computedFactors = null;
            IList<double> computedUncertainity = null;

            if (!N.proceduraOd20230915(data_wykonania))
            {
                computedFactors = SygnalizacjaMocyDawkiUtils.computeFactors(nr_karty.ToString());
                computedUncertainity = SygnalizacjaMocyDawkiUtils.computeUncertainity(nr_karty.ToString());
            }

            var tab = baza.Sygnalizacja
                .WHERE().ID_wzorcowania(row.ID_wzorcowania)
                .GET();
            for (int i = 0; i < tab.Length; i++)
            {
                var tabRow = tab[i];
                var w = new Wynik();
                w.prog = Wymagany(tabRow.Prog, "Prog jest NULL");
                w.wartosc_zmierzona = Wymagany(tabRow.Wartosc_zmierzona, "Wartosc_zmierzona jest NULL");
                w.niepewnosc = Wymagany(tabRow.Niepewnosc, "Niepewnosc jest NULL");
                if (computedFactors == null)
                {
                    w.wspolczynnik = tabRow.Wspolczynnik;
                    w.niepewnosc_wspolczynnika = tabRow.Niepewnosc_Wspolczynnika;
                }
                else
                {
                    w.wspolczynnik = computedFactors[i];
                    w.niepewnosc_wspolczynnika = computedUncertainity[i];
                }
                wyn.tabela.Add(w);
            }
        }

        private void DodajTabeleSD(Szablon.Row_wzorcowanie_cezem row, Wyniki wyn)
        {
            IList<double> computedFactors = null;
            IList<double> computedUncertainity = null;

            if (!N.proceduraOd20230915(data_wykonania))
            {
                computedFactors = SygnalizacjaDawkiUtils.computeFactors(nr_karty.ToString());
                computedUncertainity = SygnalizacjaDawkiUtils.computeUncertainity(nr_karty.ToString());
            }

            var tab = baza.Sygnalizacja_dawka
                .WHERE().ID_wzorcowania(row.ID_wzorcowania)
                .GET();
            for (int i = 0; i < tab.Length; i++)
            {
                var tabRow = tab[i];
                var w = new Wynik();
                w.prog = Wymagany(tabRow.Prog, "Prog jest NULL");
                w.wartosc_wzorcowa = Wymagany(tabRow.Wartosc_wzorcowa, "Wartosc_wzorcowa jest NULL");
                w.wartosc_zmierzona = Wymagany(tabRow.Wartosc_zmierzona, "Wartosc_zmierzona jest NULL");
                w.niepewnosc = tabRow.Niepewnosc;
                if (computedFactors == null)
                {
                    w.wspolczynnik = tabRow.Wspolczynnik;
                    w.niepewnosc_wspolczynnika = tabRow.Niepewnosc_wsp;
                }
                else
                {
                    w.wspolczynnik = computedFactors[i];
                    w.niepewnosc_wspolczynnika = computedUncertainity[i];
                }
                wyn.tabela.Add(w);
            }
        }
    }
}
