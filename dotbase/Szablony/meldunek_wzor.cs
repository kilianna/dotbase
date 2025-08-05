
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;

namespace DotBase.Szablony
{
    class meldunek_wzor: DocxData
    {
        public class Wpis {
            public string typ;
            public double cena;
            public List<string> nrFabryczne;
        }
        public int nrZlecenia;
        public string adresZleceniodawcy;
        public string nip;
        public string nazwaPlatnika;
        public string adresPlatnika;
        public bool innyPlatnik;
        public string nrZleceniaKlienta;
        public string zleceniodawca;
        public List<string> nrKart = new List<string>();
        public DateTime dataSprzedazy;
        public DateTime dataDzisiejsza;
        public List<Wpis> wpisy = new List<Wpis>();

        protected override string FileName
        {
            get
            {
                return String.Format(@"..\wyniki\Meldunek\{0}\{1}Meldunek.docx",
                    DirGroup(nrZlecenia), nrZlecenia);
            }
        }

        protected override bool PreProcess(IWin32Window owner)
        {
            dataDzisiejsza = DateTime.Today;
            dataSprzedazy = new DateTime(0);
            var map = new Dictionary<Tuple<string, double>, List<string>>();
            foreach (var nrKarty in nrKart)
            {
                // id_karty ---Swiadectwo--> Data_wystawienia
                var row1 = baza.Swiadectwo
                    .SELECT().Data_wystawienia()
                    .WHERE().Id_karty(Int32.Parse(nrKarty))
                    .GET_FIRST();
                var data = row1.Data_wystawienia ?? new DateTime(0);
                if (data > dataSprzedazy) dataSprzedazy = data;

                // id_karty ---Karta_przyjecia--> id_dozymetru, cena
                var row2 = baza.Karta_przyjecia
                    .SELECT().ID_dozymetru().Cena()
                    .WHERE().ID_karty(Int32.Parse(nrKarty))
                    .GET_ONE();
                var idDozymetru = row2.ID_dozymetru ?? -1;
                var cena = row2.Cena ?? 0.0;

                // id_dozymetru ---Dozymetry--> typ, nr_fabryczny
                var row3 = baza.Dozymetry
                    .SELECT().Typ().Nr_fabryczny()
                    .WHERE().ID_dozymetru(idDozymetru)
                    .GET_ONE();
                var typ = row3.Typ;
                var nrFabryczny = row3.Nr_fabryczny;
                
                // [typ, cena] ==> list: nr_fabryczny
                var key = new Tuple<string,double>(typ, Math.Round(cena, 4));
                if (!map.ContainsKey(key))
                {
                    map[key] = new List<string>();
                }
                map[key].Add(nrFabryczny);
            }

            foreach (var item in map)
            {
                var wpis = new Wpis();
                wpis.typ = item.Key.Item1;
                wpis.cena = item.Key.Item2;
                wpis.nrFabryczne = item.Value;
                wpisy.Add(wpis);
            }

            return true;
        }
    }
}
