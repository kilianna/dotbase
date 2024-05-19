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
        public Szablon.Row_Dozymetry przyrzad;
        public Szablon.Row_Karta_przyjecia kartaPrzyjecia;
        public Szablon.Row_Zlecenia zlecenie;
        public Szablon.Row_Zleceniodawca zleceniodawca;

        /*public DataRow jednostki;
        public DataRowCollection tabelaMD;
        public DataRowCollection tabelaD;
        public DataRowCollection tabelaSM;
        public DataRowCollection tabelaSD;
        public DataRowCollection tabelaS;*/

        protected override string FileName
        {
            get {
                return String.Format(@"..\wyniki\Swiadectwo\{0}\{1}SwiadectwoWynik{2}.docx", DirGroup(nr_karty), nr_karty, JezykTools.kocowka(jezyk));
            }
        }

        protected override bool PreProcess(IWin32Window owner)
        {
            var baza = new BazaDanychWrapper();

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

            return true;
        }
    }
}
