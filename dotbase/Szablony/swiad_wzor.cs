using System;
using System.Data;

namespace DotBase.Szablony
{

    class swiad_wzor : DocxData
    {
        protected override string FileName
        {
            get { return String.Format(@"..\wyniki\Swiadectwo\{0}\{1}SwiadectwoWynik{2}.docx", DirGroup(nr_karty), nr_karty, JezykTools.kocowka(jezyk)); }
        }

        public Jezyk jezyk;
        public string nr_karty;
        public DateTime data_przyjecia;
        public DateTime data_wykonania;
        public DateTime data_wydania;
        public string sprawdzil;
        public DataRow zleceniodawca;
        public DataRow przyrzad;
        public DataRow jednostki;
        public bool poprawa;
        public string uwMD;
        public string uwD;
        public string uwS;
        public string uwSMD;
        public string uwSD;

        public DataRowCollection tabelaMD;
        public DataRowCollection tabelaD;
        public DataRowCollection tabelaSM;
        public DataRowCollection tabelaSD;
        public DataRowCollection tabelaS;
    }
}
