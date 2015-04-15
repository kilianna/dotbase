using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotBase
{
    class WzorcowanieSygnalizacjaMocyDawkiDataModel
    {
        public DanePodstawoweModel modelDanePodstawowe;
        public DanePrzyrzaduModel modelDanePrzyrzadu;
        public DaneWarunkowModel modelDaneWarunkow;
        public DaneWspolczynnikowModel modelDaneWspolczynnikow;

        public string jednostka;
        public string uwagiDoWarunkow;
        public string uwagiWzorcowe;
        public System.Windows.Forms.DataGridView dataGridView;

        public WzorcowanieSygnalizacjaMocyDawkiDataModel(DanePodstawoweModel modelDanePodstawowe, DanePrzyrzaduModel modelDanePrzyrzadu, DaneWarunkowModel modelDaneWarunkow, DaneWspolczynnikowModel modelDaneWspolczynnikow)
        {
            this.modelDanePodstawowe = modelDanePodstawowe;
            this.modelDanePrzyrzadu = modelDanePrzyrzadu;
            this.modelDaneWarunkow = modelDaneWarunkow;
            this.modelDaneWspolczynnikow = modelDaneWspolczynnikow;
        }
    }
}
