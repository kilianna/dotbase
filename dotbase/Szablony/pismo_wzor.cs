using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;

namespace DotBase.Szablony
{
    class pismo_wzor: DocxData
    {
        public int nrPisma;
        public bool poprawa;
        public int nrKarty;
        public DateTime dataWystawienia;
        public DateTime dataWykonania;
        public string uwaga;
        public int rokPisma;
        public bool przedluzonaWaznosc;
        public bool odlaczWykresMD;
        public bool odlaczWykresD;
        public Szablon.Row_Karta_przyjecia kartaPrzyjecia;
        public Szablon.Row_Dozymetry przyrzad;
        public Szablon.Row_Zlecenia zlecenie;
        public Szablon.Row_Zleceniodawca zleceniodawca;
        public Szablon.Row_Sondy[] sondy;
        public DateTime dataNastepna;
        public int ileD;
        public int ileMD;
        public int ileSD;
        public int ileSM;

        protected override string FileName
        {
            get
            {
                return String.Format(@"..\wyniki\PismoPrzewodnie\{0}\{1}{2}PismoPrzewodnieWynik{3}{4}.docx",
                    DirGroup(nrPisma), nrPisma, poprawa ? "P" : "", nrKarty, JezykTools.kocowka(jezyk));
            }
        }

        protected override bool PreProcess(IWin32Window owner)
        {
            kartaPrzyjecia = baza.Karta_przyjecia
                .WHERE().ID_karty(nrKarty)
                .GET_ONE();

            przyrzad = baza.Dozymetry
                .WHERE().ID_dozymetru(kartaPrzyjecia.ID_dozymetru ?? -1)
                .GET_ONE();

            zlecenie = baza.Zlecenia
                .WHERE().ID_zlecenia(kartaPrzyjecia.ID_zlecenia)
                .GET_ONE();

            zleceniodawca = baza.Zleceniodawca
                .WHERE().ID_zleceniodawcy(zlecenie.ID_zleceniodawcy ?? -1)
                .GET_ONE();

            var idSond = new HashSet<int>();

            var sondyTable1 = baza.wzorcowanie_cezem
                .SELECT().ID_sondy()
                .WHERE().ID_karty(nrKarty)
                .GET();
            foreach (var row in sondyTable1)
                if (row.ID_sondy != null)
                    idSond.Add(row.ID_sondy ?? 0);

            var sondyTable2 = baza.Wzorcowanie_zrodlami_powierzchniowymi
                .SELECT().ID_sondy()
                .WHERE().ID_karty(nrKarty)
                .GET();
            foreach (var row in sondyTable2)
                if (row.ID_sondy != null)
                    idSond.Add(row.ID_sondy ?? 0);

            var sondyTable3 = baza.TworzTabeleDanych(String.Format(@"
                SELECT * FROM Sondy
                WHERE ID_sondy IN ({0});", String.Join(",", idSond)));
            
            sondy = Szablon.Row_Sondy._GET(sondyTable3);

            ileD = baza.wzorcowanie_cezem
                .WHERE().ID_karty(nrKarty).Rodzaj_wzorcowania("d")
                .GET()
                .Length;

            ileMD = baza.wzorcowanie_cezem
                .WHERE().ID_karty(nrKarty).Rodzaj_wzorcowania("md")
                .GET()
                .Length;

            ileSD = baza.wzorcowanie_cezem
                .WHERE().ID_karty(nrKarty).Rodzaj_wzorcowania("sd")
                .GET()
                .Length;

            ileSM = baza.wzorcowanie_cezem
                .WHERE().ID_karty(nrKarty).Rodzaj_wzorcowania("sm")
                .GET()
                .Length;

            dataNastepna = dataWykonania.AddYears(przedluzonaWaznosc ? 2 : 1);

            return true;
        }
    }
}
