using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotBase
{
    class MeldunekModel
    {
        public IList<String> nrKart = new List<String>();
        public string nip;
        public string zleceniodawca;
        public string adresZleceniodawcy;
        public string nazwaPlatnika;
        public string adresPlatnika;
        public bool innyPlatnik;
    }
}
