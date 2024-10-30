using System;
using System.Data;
using System.Windows.Forms;

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

            return true;
        }
    }
}
