using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DotBase.Szablony
{
    class wzorcowanie_wzor : DocxData
    {

        public enum Typ
        {
            MOC_DAWKI,
            EMISJA_POWIERZCHNIOWA,
        };

        public class Przyrzad
        {
            public string typ;
            public string nrFabryczny;
            public string inneNastawy;
            public string sondaTyp;
            public string sondaNrFabryczny;
            public string napiecieZasilaniaSondy;
        };

        public class Warunki
        {
            public string cisnienie;
            public string temperatura;
            public string wilgotnosc;
        };

        public Typ typ;
        public string jednostka;
        public string wielkoscFizyczna;
        public string tlo;
        public bool dolaczZakres;
        public string nrKarty;
        public string nrArkusza;
        public DateTime data;
        public Przyrzad przyrzad = new Przyrzad();
        public Warunki warunki = new Warunki();
        public string sprawdzil;
        public string wykonal;
        public DataGridViewRowCollection tabela;
        public string uwagi;
        public string znakSprawy;
        public Szablon.Row_Karta_przyjecia kartaPrzyjecia;
        public Szablon.Row_Zlecenia zlecenie;
        public Szablon.Row_Zleceniodawca zleceniodawca;
        public Szablon.Row_Sondy sonda;
        public Szablon.Row_Metody[] metody;
        public DataGridViewRowCollection obliczone;

        protected override string FileName
        {
            get
            {
                return String.Format(@"..\Wyniki\Protokoly\MocDawki\{0}\{1}MocDawki.docx", DirGroup(nrKarty), nrKarty);
            }
        }

        protected override bool PreProcess(IWin32Window owner)
        {
            kartaPrzyjecia = baza.Karta_przyjecia.SELECT().WHERE().ID_karty(Int32.Parse(nrKarty)).GET_ONE();
            int idZlecenia = kartaPrzyjecia.ID_zlecenia ?? -1;
            zlecenie = baza.Zlecenia.SELECT().WHERE().ID_zlecenia(idZlecenia).GET_ONE();
            int idZleceniodawcy = zlecenie.ID_zleceniodawcy ?? -1;
            zleceniodawca = baza.Zleceniodawca.SELECT().WHERE().ID_zleceniodawcy(idZleceniodawcy).GET_ONE();
            znakSprawy = String.Format("NLW.4851.{0}.{1}.W", idZlecenia, zlecenie.Data_przyjecia.Value.Year);

            var sondy = baza.Sondy.SELECT()
                .WHERE()
                    .ID_dozymetru(kartaPrzyjecia.ID_dozymetru)
                    .Typ(przyrzad.sondaTyp)
                    .Nr_fabryczny(przyrzad.sondaNrFabryczny)
                .GET();

            if (sondy.Length != 1)
            {
                MyMessageBox.Show(owner, "Nie znaleziono wybranej sondy", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            sonda = sondy[0];

            metody = baza.Metody.SELECT().GET();

            return true;
        }
    }
}
