using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotBase
{
    class DanePrzyrzaduModel
    {
        public string typ;
        public string nrFabryczny;
        public string inneNastawy;
        public string sondaTyp;
        public string sondaNrFabryczny;
        public string napiecieZasilaniaSondy;

        public DanePrzyrzaduModel(string typ, string nrFabryczny, string inneNastawy, string sondaTyp, string sondaNrFabryczny, string napiecieZasilaniaSondy)
        {
            this.typ = typ;
            this.nrFabryczny = nrFabryczny;
            this.inneNastawy = inneNastawy;
            this.sondaTyp = sondaTyp;
            this.sondaNrFabryczny = sondaNrFabryczny;
            this.napiecieZasilaniaSondy = napiecieZasilaniaSondy;
        }
    }
}
