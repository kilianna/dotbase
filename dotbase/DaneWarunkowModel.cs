using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotBase
{
    class DaneWarunkowModel
    {
        public string cisnienie;
        public string temperatura;
        public string wilgotnosc;
        public string uwagi;

        public DaneWarunkowModel(string cisnienie, string temperatura, string wilgotnosc, string uwagi)
        {
            this.cisnienie = cisnienie;
            this.temperatura = temperatura;
            this.wilgotnosc = wilgotnosc;
            this.uwagi = uwagi;
        }

    }
}
