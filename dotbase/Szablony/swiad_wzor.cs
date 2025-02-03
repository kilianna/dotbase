using System;
using System.Data;
using System.Windows.Forms;

namespace DotBase.Szablony
{
    class swiad_wzor : DocxData
    {
        public int nr_karty;
        public DateTime data_wydania;
        public DateTime data_wykonania;
        public DateTime data_przyjecia;
        public string sprawdzil;
        public bool poprawa;
        public string uwMD;
        public string uwD;
        public string uwS;
        public string uwSMD;
        public string uwSD;

        /*public DataRow zleceniodawca;
        public DataRow przyrzad;
        public DataRow jednostki;
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
            return true;
        }
    }
}
