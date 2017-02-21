using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DotBase
{
    namespace PrzestrzenZlecenia
    {
        public struct DanePrzyrzad
        {
            public int IdKarty { get; set; }
            public string Typ { get; set; }
            public string NrFabryczny { get; set; }
            public bool Wykalibrowany { get; set; }

            //--------------------------------------------------------------------
            public DanePrzyrzad(int id, string typ, string nrFab, bool wyk)
                : this()
            //--------------------------------------------------------------------
            {
                IdKarty = id;
                Typ = typ;
                NrFabryczny = nrFab;
                Wykalibrowany = wyk;
            }
        }

        public struct DaneZleceniodawcy
        {
            public string Adres { get; set; }
            public string Faks { get; set; }
            public int Id { get; set; }
            public string Nazwa { get; set; }
            public string Nip { get; set; }
            public string OsobaKontaktowa { get; set; }
            public string Telefon { get; set; }
            public string NazwaPlatnika { get; set; }
            public string AdresPlatnika { get; set; }
            public string NipPlatnika { get; set; }

            public DaneZleceniodawcy(string Adres, string Faks, int Id, string Nazwa, string Nip, string OsobaKon, string Telefon, string NazwaPlatnika, string AdresPlatnika, string NipPlatnika)
                : this()
            {
                this.Adres = Adres;
                this.Faks = Faks;
                this.Id = Id;
                this.Nazwa = Nazwa;
                this.Nip = Nip;
                this.OsobaKontaktowa = OsobaKon;
                this.Telefon = Telefon;
                this.NazwaPlatnika = NazwaPlatnika;
                this.AdresPlatnika = AdresPlatnika;
                this.NipPlatnika = NipPlatnika;
            }
        }

        public struct DaneZlecenia
        {
            public int Id;
            public int Nr_rejestru;
            public DateTime DataPrzyjecia;
            public DateTime DataZwrotu;
            public bool Ekspress;
            public string FormaPrzyjecia { get; set; }
            public string FormaZwrotu { get; set; }
            public string OsobaPrzyjmujaca { get; set; }
            public List<DanePrzyrzad> Przyrzady;
            public string Uwagi { get; set; }
            public string Nip { get; set; }
            public bool InnyPlatnik { get; set; }
            public DaneZleceniodawcy ZleceniodawcaInfo;
        }

        public class Zlecenie
        {
            private BazaDanychWrapper _BazaDanych;
            private DaneZlecenia _DaneZlecenia;
            private DataTable _DaneTabela;
            private List<string> _WszyscyZleceniodawcy;
            private string _Zapytanie;

            //--------------------------------------------------------------------
            public Zlecenie()
            //--------------------------------------------------------------------
            {
                _BazaDanych = new BazaDanychWrapper();
                _WszyscyZleceniodawcy = new List<string>();
                ZnajdzDostepnychZleceniodawcow();
            }

            //--------------------------------------------------------------------
            // Zwraca wypełnioną strukturę z danymi na temat danych zlecenia, zleceniodawcy
            // i przyrządów. Ze struktury dane przekazywane są do kontrolek w oknie.
            public DaneZlecenia Dane
            //--------------------------------------------------------------------
            {
                get
                {
                    return _DaneZlecenia;
                }
            }


            //--------------------------------------------------------------------
            public bool DodajZleceniodawce(ref DaneZleceniodawcy zleceniodawca)
            //--------------------------------------------------------------------
            {
                _Zapytanie = "INSERT INTO Zleceniodawca (Adres, Faks, ID_zleceniodawcy, Nip, Osoba_kontaktowa, Telefon, Zleceniodawca, Nazwa_platnika, Adres_platnika, NIP_platnika) VALUES" +
                             String.Format("('{0}','{1}',{2},'{3}','{4}','{5}','{6}','{7}','{8}','{9}')", zleceniodawca.Adres, zleceniodawca.Faks, zleceniodawca.Id,
                             zleceniodawca.Nip, zleceniodawca.OsobaKontaktowa, zleceniodawca.Telefon, zleceniodawca.Nazwa,
                             zleceniodawca.NazwaPlatnika, zleceniodawca.AdresPlatnika, zleceniodawca.NipPlatnika);

                return _BazaDanych.WykonajPolecenie(_Zapytanie);
            }

            //--------------------------------------------------------------------
            public bool EdytujZleceniodawce(ref DaneZleceniodawcy zleceniodawca)
            //--------------------------------------------------------------------
            {
                _Zapytanie = String.Format("UPDATE Zleceniodawca SET zleceniodawca='{0}', Adres='{1}', Faks='{2}', ",
                                           zleceniodawca.Nazwa, zleceniodawca.Adres, zleceniodawca.Faks)
                           + String.Format(" telefon='{0}', Osoba_kontaktowa='{1}', nip='{2}', ",
                                            zleceniodawca.Telefon, zleceniodawca.OsobaKontaktowa, zleceniodawca.Nip)
                           + String.Format(" Nazwa_platnika='{0}', Adres_platnika='{1}', NIP_platnika='{2}'",
                                            zleceniodawca.NazwaPlatnika, zleceniodawca.AdresPlatnika, zleceniodawca.NipPlatnika)
                           + String.Format(" WHERE id_zleceniodawcy = {0}", zleceniodawca.Id);

                _BazaDanych.WykonajPolecenie(_Zapytanie);

                return true;
            }

            //--------------------------------------------------------------------
            public string[] Zleceniodawcy { get { return _WszyscyZleceniodawcy.ToArray(); } }
            //--------------------------------------------------------------------

            //--------------------------------------------------------------------
            // Gdy użytkownik wpsiuje nazwę zleceniodawcy jest ona dynamicznie dopasowywana
            // do wsyzstkich aktualnie dostępnych zleceniodawców. Są oni następnie wyświetlani
            // jako podpowiedzi, które można wybrać pomijając dalsze wpisywanie
            public string[] GetMozliwychZleceniodawcow(string klucz)
            //--------------------------------------------------------------------
            {
                var znalezieni = from zleceniodawca in _WszyscyZleceniodawcy where zleceniodawca.ToLower().Contains(klucz.ToLower()) select zleceniodawca;

                return znalezieni.ToArray();
            }

            //--------------------------------------------------------------------
            public void DodajZlecenie(ref DaneZlecenia dane)
            //--------------------------------------------------------------------
            {
                _Zapytanie = String.Format("INSERT INTO Zlecenia VALUES ({0},{1},'{2}','{3}','{4}','{5}','{6}','{7}',{8},{9},{10})",
                                           dane.Id, dane.ZleceniodawcaInfo.Id, dane.DataPrzyjecia.ToShortDateString(),
                                           dane.DataZwrotu.ToShortDateString(), dane.FormaPrzyjecia, dane.FormaZwrotu, dane.OsobaPrzyjmujaca,
                                           dane.Uwagi, dane.Ekspress, dane.Nr_rejestru, dane.InnyPlatnik);

                _BazaDanych.WykonajPolecenie(_Zapytanie);
            }

            //--------------------------------------------------------------------
            public void UsunKarty(int idZlecenia)
            //--------------------------------------------------------------------
            {
                _Zapytanie = String.Format("DELETE * FROM Karta_przyjecia WHERE id_zlecenia = {0}", idZlecenia);

                _BazaDanych.WykonajPolecenie(_Zapytanie);
            }

            //--------------------------------------------------------------------
            public void UsunZlecenie(int idZlecenia)
            //--------------------------------------------------------------------
            {
                _Zapytanie = String.Format("DELETE * FROM Zlecenia WHERE id_zlecenia = {0}", idZlecenia);

                _BazaDanych.WykonajPolecenie(_Zapytanie);
            }

            //--------------------------------------------------------------------
            // Wypełnia odpowiednią strukturę "DaneZlecenia" wszelkimi danymi
            // wymaganymi do wyświetlenia. Dane te są wcześniej ładowane z bazy do 
            // kalsy DataTable która odwzorowuje tabele relacyjnej bazy w obiekt
            //--------------------------------------------------------------------
            private void WypelnijDanePrzyrzadow()
            //--------------------------------------------------------------------
            {
                _DaneZlecenia.Przyrzady = new List<DanePrzyrzad>();

                foreach (DataRow wiersz in _DaneTabela.Rows)
                {
                    _DaneZlecenia.Przyrzady.Add(new DanePrzyrzad(wiersz.Field<int>("Id_Karty"), wiersz.Field<string>("Typ"), wiersz.Field<string>("Nr_fabryczny"), wiersz.Field<bool>("Wykonano")));
                }
            }

            //--------------------------------------------------------------------
            // Wypełnia odpowiednią strukturę "DaneZlecenia" wszelkimi danymi
            // wymaganymi do wyświetlenia. Dane te są wcześniej ładowane z bazy do 
            // kalsy DataTable która odwzorowuje tabele relacyjnej bazy w obiekt
            //--------------------------------------------------------------------
            private void WypelnijDaneZlecenia()
            //--------------------------------------------------------------------
            {
                _DaneZlecenia.Id = _DaneTabela.Rows[0].Field<int>("id_zlecenia");
                _DaneZlecenia.Nr_rejestru = _DaneTabela.Rows[0].Field<int>("Nr_zlecenia_rejestr");
                _DaneZlecenia.DataPrzyjecia = _DaneTabela.Rows[0].Field<DateTime>("Data_przyjecia");
                _DaneZlecenia.DataZwrotu = _DaneTabela.Rows[0].Field<DateTime>("Data_zwrotu");
                _DaneZlecenia.FormaPrzyjecia = _DaneTabela.Rows[0].Field<string>("Forma_przyjecia");
                _DaneZlecenia.FormaZwrotu = _DaneTabela.Rows[0].Field<string>("Forma_zwrotu");
                _DaneZlecenia.Uwagi = _DaneTabela.Rows[0].Field<string>("Uwagi");
                _DaneZlecenia.OsobaPrzyjmujaca = _DaneTabela.Rows[0].Field<string>("Osoba_przyjmujaca");
                _DaneZlecenia.Ekspress = _DaneTabela.Rows[0].Field<bool>("Ekspres");
                _DaneZlecenia.InnyPlatnik = _DaneTabela.Rows[0].Field<bool>("Inny_platnik");
            }

            //--------------------------------------------------------------------
            public bool ZaladujDanePrzyrzadow(int idZlecenia)
            //--------------------------------------------------------------------
            {
                if (idZlecenia < 0)
                    return false;

                _Zapytanie = "SELECT id_karty, typ, nr_fabryczny, wykonano FROM Karta_przyjecia AS K "
                           + "INNER JOIN Dozymetry AS D ON K.id_dozymetru=D.id_dozymetru WHERE "
                           + String.Format("id_zlecenia = {0}", idZlecenia);

                _DaneTabela = _BazaDanych.TworzTabeleDanych(_Zapytanie);

                if (null == _DaneTabela)
                    return false;

                WypelnijDanePrzyrzadow();

                return true;
            }

            //--------------------------------------------------------------------
            // Wypełnia odpowiednią strukturę "DaneZlecenia" wszelkimi danymi
            // wymaganymi do wyświetlenia. Dane te są wcześniej ładowane z bazy do 
            // kalsy DataTable która odwzorowuje tabele relacyjnej bazy w obiekt
            //--------------------------------------------------------------------
            public void WypelnijDaneZleceniodawcy()
            //--------------------------------------------------------------------
            {
                _DaneZlecenia.ZleceniodawcaInfo.Adres = _DaneTabela.Rows[0].Field<string>("Adres");
                _DaneZlecenia.ZleceniodawcaInfo.Faks = _DaneTabela.Rows[0].Field<string>("Faks");
                _DaneZlecenia.ZleceniodawcaInfo.Id = _DaneTabela.Rows[0].Field<int>("ID_zleceniodawcy");
                _DaneZlecenia.ZleceniodawcaInfo.Nazwa = _DaneTabela.Rows[0].Field<string>("Zleceniodawca");
                _DaneZlecenia.ZleceniodawcaInfo.Nip = _DaneTabela.Rows[0].Field<string>("Nip");
                _DaneZlecenia.ZleceniodawcaInfo.OsobaKontaktowa = _DaneTabela.Rows[0].Field<string>("Osoba_kontaktowa");
                _DaneZlecenia.ZleceniodawcaInfo.Telefon = _DaneTabela.Rows[0].Field<string>("Telefon");
                _DaneZlecenia.ZleceniodawcaInfo.NazwaPlatnika = _DaneTabela.Rows[0].Field<string>("Nazwa_platnika");
                _DaneZlecenia.ZleceniodawcaInfo.AdresPlatnika = _DaneTabela.Rows[0].Field<string>("Adres_platnika");
                _DaneZlecenia.ZleceniodawcaInfo.NipPlatnika = _DaneTabela.Rows[0].Field<string>("NIP_platnika");
            }

            //--------------------------------------------------------------------
            public bool ZaladujDaneZleceniodawcyOrazZlecenia(int idZlecenia)
            //--------------------------------------------------------------------
            {
                if (idZlecenia <= 0)
                    return false;

                _Zapytanie = "SELECT Zleceniodawca, Adres, Osoba_kontaktowa, Telefon, Faks, email, Nip, ID_zlecenia, "
                           + "Zlecenia.ID_zleceniodawcy, Data_przyjecia, Data_zwrotu, Forma_przyjecia, "
                           + "Forma_zwrotu, Osoba_przyjmujaca, Zlecenia.Uwagi, Ekspres, Inny_platnik, Nr_zlecenia_rejestr, Nazwa_platnika, Adres_platnika, NIP_platnika "
                           + "FROM Zlecenia INNER JOIN "
                           + "Zleceniodawca ON Zlecenia.ID_Zleceniodawcy=Zleceniodawca.ID_Zleceniodawcy "
                           + String.Format("WHERE id_zlecenia = {0}", idZlecenia);

                _DaneTabela = _BazaDanych.TworzTabeleDanych(_Zapytanie);

                if (null == _DaneTabela)
                    return false;

                WypelnijDaneZlecenia();
                WypelnijDaneZleceniodawcy();

                return true;
            }

            //--------------------------------------------------------------------
            public bool ZaladujDaneZleceniodawcy(int idZlecenia)
            //--------------------------------------------------------------------
            {
                if (idZlecenia <= 0)
                    return false;

                _Zapytanie = "SELECT Zleceniodawca, Adres, Osoba_kontaktowa, Telefon, Faks, email, Nip, id_zleceniodawcy, Nazwa_platnika, Adres_platnika, NIP_platnika "
                           + String.Format("FROM Zleceniodawca WHERE ID_zleceniodawcy = {0}", idZlecenia);

                _DaneTabela = _BazaDanych.TworzTabeleDanych(_Zapytanie);

                if (null == _DaneTabela)
                    return false;

                WypelnijDaneZleceniodawcy();

                return true;
            }

            //--------------------------------------------------------------------
            public bool ZaladujWszystkichZleceniodawcow(string nazwaZleceniodawcy)
            //--------------------------------------------------------------------
            {
                if (null == nazwaZleceniodawcy || 0 == String.Empty.CompareTo(nazwaZleceniodawcy))
                    return false;

                _Zapytanie = String.Format("SELECT * FROM Zleceniodawca WHERE Zleceniodawca = '{0}'", nazwaZleceniodawcy);

                _DaneTabela = _BazaDanych.TworzTabeleDanych(_Zapytanie);

                if (null == _DaneTabela)
                    return false;

                return true;
            }

            //--------------------------------------------------------------------
            public bool ZaladujWszystkieDane(int idZlecenia)
            //--------------------------------------------------------------------
            {
                if (false == ZaladujDaneZleceniodawcyOrazZlecenia(idZlecenia) || false == ZaladujDanePrzyrzadow(idZlecenia))
                    return false;

                return true;
            }

            //--------------------------------------------------------------------
            public void ZapiszDane(DaneZlecenia daneDoZapisu)
            //--------------------------------------------------------------------
            {
                // jako, że id_zlecenia mogło zostać zmienione (np. przejście do innego zlecenia przez użytkownika)
                // Id zlecenia bierzemy nie wprost z okna, ale z odczytanych wcześniej danych z bazy
                // Dlatego też poniżej widnieje _DaneZlecenia.Id
                _Zapytanie = String.Format("UPDATE Zlecenia SET data_przyjecia='{0}', data_zwrotu='{1}', forma_przyjecia='{2}', forma_zwrotu='{3}', uwagi='{4}', ekspres={5}, osoba_przyjmujaca='{6}', id_zleceniodawcy={7}, nr_zlecenia_rejestr={8}, Inny_platnik={9} WHERE id_zlecenia={10}",
                                           daneDoZapisu.DataPrzyjecia.ToShortDateString(), daneDoZapisu.DataZwrotu.ToShortDateString(),
                                           daneDoZapisu.FormaPrzyjecia, daneDoZapisu.FormaZwrotu, daneDoZapisu.Uwagi, daneDoZapisu.Ekspress,
                                           daneDoZapisu.OsobaPrzyjmujaca, daneDoZapisu.ZleceniodawcaInfo.Id, daneDoZapisu.Nr_rejestru, daneDoZapisu.InnyPlatnik, _DaneZlecenia.Id);

                _BazaDanych.WykonajPolecenie(_Zapytanie);
            }

            //--------------------------------------------------------------------
            public int ZnajdzMaxIDZleceniodawcy()
            //--------------------------------------------------------------------
            {
                _Zapytanie = "SELECT id_zleceniodawcy FROM Zleceniodawca WHERE id_zleceniodawcy = (SELECT MAX(id_zleceniodawcy) FROM Zleceniodawca)";

                _DaneTabela = _BazaDanych.TworzTabeleDanych(_Zapytanie);

                if (null == _DaneTabela)
                    return 0;

                return _DaneTabela.Rows[0].Field<int>("ID_Zleceniodawcy");
            }

            //--------------------------------------------------------------------
            // Znajduje w bazie wszelkich dostępnych zleceniodawców
            public bool ZnajdzDostepnychZleceniodawcow()
            //--------------------------------------------------------------------
            {
                _Zapytanie = "SELECT Zleceniodawca FROM Zleceniodawca";
                _DaneTabela = _BazaDanych.TworzTabeleDanych(_Zapytanie);

                if (null == _DaneTabela)
                    return false;

                foreach (DataRow row in _DaneTabela.Rows)
                    _WszyscyZleceniodawcy.Add(row.Field<string>("Zleceniodawca"));

                return true;
            }

            //--------------------------------------------------------------------
            private bool ZnajdzPoNrZlecenia(int numer)
            //--------------------------------------------------------------------
            {
                return ZaladujDaneZleceniodawcyOrazZlecenia(numer);
            }

            //--------------------------------------------------------------------
            // ZNajduje ostatnie zlecenie w bazie danych
            public bool ZnajdzPoIdZleceniodawcy(string idZleceniodawcy)
            //--------------------------------------------------------------------
            {
                try
                {
                    return ZaladujDaneZleceniodawcy(int.Parse(idZleceniodawcy));
                }
                catch (Exception)
                {
                    return false;
                }
            }

            //--------------------------------------------------------------------
            // ZNajduje ostatnie zlecenie w bazie danych
            public int ZnajdzOstatnieZlecenie()
            //--------------------------------------------------------------------
            {
                _Zapytanie = "SELECT id_zlecenia FROM Zlecenia WHERE id_zlecenia = (SELECT MAX(id_zlecenia) FROM Zlecenia)";

                _DaneTabela = _BazaDanych.TworzTabeleDanych(_Zapytanie);

                if (null == _DaneTabela)
                    return 0;

                return _DaneTabela.Rows[0].Field<int>("ID_Zlecenia");
            }
        }
    }
}
