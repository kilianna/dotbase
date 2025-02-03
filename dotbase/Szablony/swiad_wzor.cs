using System;

namespace DotBase.Szablony
{

    class swiad_wzor : DocxData
    {
        protected override string FileName
        {
            get { return String.Format(@"..\wyniki\Swiadectwo\{0}\{1}SwiadectwoWynik{2}.docx", DirGroup(nr_karty), nr_karty, JezykTools.kocowka(jezyk)); }
        }

        public enum Metoda
        {
            Wzor1,
            Wzor2,
            Wzor1Wzor2,
        }

        public string nr_karty;
        public Jezyk jezyk;
        public string nazwa;
        public string typ;
        public string nr_fab;
        public string rok;
        public string producent;
        public string zleceniodawca;
        public string adres;
        public double cisnienie_min;
        public double cisnienie_max;
        public double temperatura_min;
        public double temperatura_max;
        public double wilgotnosc_min;
        public double wilgotnosc_max;
        public string data_przyjecia;
        public string data_wykonania;
        public string data_wydania;
        public string sprawdzil;
        public bool poprawa;
        public string uwMD;
        public string uwD;
        public string uwS;
        public string uwSMD;
        public string uwSD;
        public string rok_produkcji;
        public Metoda metoda;
        public bool si;
    }
}
