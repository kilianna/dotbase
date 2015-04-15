using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotBase
{
    class SwiadectwoModel
    {
        public DanePodstawoweModel modelDanePodstawowe;
        public DanePrzyrzaduModel modelDanePrzyrzadu;
        public DaneWarunkowModel modelDaneWarunkow;
        public DaneWspolczynnikowModel modelDaneWspolczynnikow;

        
        public SwiadectwoModel(DanePodstawoweModel modelDanePodstawowe, DanePrzyrzaduModel modelDanePrzyrzadu, DaneWarunkowModel modelDaneWarunkow, DaneWspolczynnikowModel modelDaneWspolczynnikow)
        {
            this.modelDanePodstawowe = modelDanePodstawowe;
            this.modelDanePrzyrzadu = modelDanePrzyrzadu;
            this.modelDaneWarunkow = modelDaneWarunkow;
            this.modelDaneWspolczynnikow = modelDaneWspolczynnikow;
        }
    }
}
