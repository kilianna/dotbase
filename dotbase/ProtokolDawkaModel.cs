using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DotBase
{
    class ProtokolDawkaModel
    {
        public DanePodstawoweModel modelDanePodstawowe;
        public DanePrzyrzaduModel modelDanePrzyrzadu;
        public DaneWarunkowModel modelDaneWarunkow;
        public DaneWspolczynnikowModel modelDaneWspolczynnikow;

        public string zrodlo;
        public string odleglosc;
        public string wspolczynnik;
        public string niepewnosc;
        public DataGridViewRowCollection tabela;

        public ProtokolDawkaModel(DanePodstawoweModel modelDanePodstawowe, DanePrzyrzaduModel modelDanePrzyrzadu, DaneWarunkowModel modelDaneWarunkow, DaneWspolczynnikowModel modelDaneWspolczynnikow)
        {
            this.modelDanePodstawowe = modelDanePodstawowe;
            this.modelDanePrzyrzadu = modelDanePrzyrzadu;
            this.modelDaneWarunkow = modelDaneWarunkow;
            this.modelDaneWspolczynnikow = modelDaneWspolczynnikow;
        }
    }
}
