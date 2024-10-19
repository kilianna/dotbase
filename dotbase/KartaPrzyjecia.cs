using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Narzedzia;

namespace DotBase
{
    namespace PrzestrzenKartyPrzyjecia
    {

        #region Dane Dodatkowe
        public class DaneDodatkowe
        {
            //----------------------------------------------
            public DaneDodatkowe(string akcesoria, string uwagi, bool uszkodzony, bool sprawdzenie)
            //----------------------------------------------
            {
                Akcesoria = akcesoria;
                Uwagi = uwagi;
                Uszkodzony = uszkodzony;
                Sprawdzenie = sprawdzenie;
            }

            public string Akcesoria { get; private set; }
            public string Uwagi { get;  private set; }
            public bool Uszkodzony { get; private set; }
            public bool Sprawdzenie { get; private set; }
        }
        #endregion
        
        #region Dane Przyrządu
        public class DanePrzyrzadu
        {
            public int IDdozymetru { get; set; }
            public string NrFabryczny { get; set; }
            public string Typ { get; set; }
            public ListaSond ListaSond { get; set; }

            //------------------------------------------------------------------
            public DanePrzyrzadu(string typ, string nrFab)
            //------------------------------------------------------------------
            {
                Typ = typ;
                NrFabryczny = nrFab;
                ListaSond = new ListaSond();
            }
        }
        #endregion

        #region Wymagania Dotyczące kalibracji
        public class WymaganiaKalibracji
        {
            public enum Stale { AMERYK = 0, CHLOR, DAWKA, MOC_DAWKI, PLUTON, STRONT_SLABY, STRONT_SILNY, SYGNALIZACJA_DAWKI, SYGNALIZACJA_MOCY_DAWKI, WEGIEL_SLABY, WEGIEL_SILNY, STRONT_NAJSILNIEJSZY, LICZBA_WYMAGAN };
            public bool[] dane;

            //----------------------------------------------------------------------------------------------------------
            public WymaganiaKalibracji(bool ameryk, bool chlor, bool dawka, bool moc_dawki, bool pluton, bool stront_slaby, bool stront_silny, bool sygnalizacja_dawki, bool sygnalizacja_mocy_dawki, bool wegiel_slaby, bool wegiel_silny, bool stront_najsilniejszy)
            //----------------------------------------------------------------------------------------------------------
            {
                dane = new bool[(int)Stale.LICZBA_WYMAGAN];

                dane[(int)Stale.AMERYK] = ameryk;
                dane[(int)Stale.CHLOR] = chlor;
                dane[(int)Stale.DAWKA] = dawka;
                dane[(int)Stale.MOC_DAWKI] = moc_dawki;
                dane[(int)Stale.PLUTON] = pluton;
                dane[(int)Stale.STRONT_SLABY] = stront_slaby;
                dane[(int)Stale.STRONT_SILNY] = stront_silny;
                dane[(int)Stale.SYGNALIZACJA_DAWKI] = sygnalizacja_dawki;
                dane[(int)Stale.SYGNALIZACJA_MOCY_DAWKI] = sygnalizacja_mocy_dawki;
                dane[(int)Stale.WEGIEL_SLABY] = wegiel_slaby;
                dane[(int)Stale.WEGIEL_SILNY] = wegiel_silny;
                dane[(int)Stale.STRONT_NAJSILNIEJSZY] = stront_najsilniejszy;
            }
        }
        #endregion

        #region Dane Karty Przyjęcia
        public class DaneKartyPrzyjecia
        {
            public int IdKarty { get; set; }
            public int NrZlecenia { get; set; }
            public int idDozymetru { get; set; }
            public int rok { get; set; }
            public DaneDodatkowe DaneDodatkowe { get; set; }
            public DanePrzyrzadu Przyrzad { get; set; }
            public WymaganiaKalibracji Wymagania  { get; set; }
            public bool Wykonano { get; set; }

            //----------------------------------------------------------------------------------------------
            public DaneKartyPrzyjecia()
            //----------------------------------------------------------------------------------------------
            {
                
            }

            //----------------------------------------------------------------------------------------------
            public DaneKartyPrzyjecia(int idKarty, int nrZlecenia, int rok)
            //----------------------------------------------------------------------------------------------
            {
                this.IdKarty = idKarty;
                this.NrZlecenia = nrZlecenia;
                this.rok = rok;
            }

            //----------------------------------------------------------------------------------------------
            public void UstawDaneDodatkowe(string akcesoria, string uwagi, bool uszkodzony, bool sprawdzenie)
            //----------------------------------------------------------------------------------------------
            {
                DaneDodatkowe = new DaneDodatkowe(akcesoria, uwagi, uszkodzony, sprawdzenie);
            }

            //----------------------------------------------------------------------------------------------
            public void UstawDanePodstawowe(int idKarty, int nrZlecenia)
            //----------------------------------------------------------------------------------------------
            {
                IdKarty = idKarty;
                NrZlecenia = nrZlecenia;
            }

            //----------------------------------------------------------------------------------------------
            public void UstawDanePrzyrzadu(string typ, string nrFab)
            //----------------------------------------------------------------------------------------------
            {
                Przyrzad = new DanePrzyrzadu(typ, nrFab);
            }

            //----------------------------------------------------------------------------------------------
            public void UstawDaneWymagan(bool ameryk, bool chlor, bool dawka, bool moc_dawki, bool pluton, bool stront_slaby, bool stront_silny,
                                         bool sygnalizacja_dawki, bool sygnalizacja_mocy_dawki, bool wegiel_slaby, bool wegiel_silny, bool stront_najsilniejszy)
            //----------------------------------------------------------------------------------------------
            {
                Wymagania = new WymaganiaKalibracji(ameryk, chlor, dawka, moc_dawki, pluton, stront_slaby, stront_silny, sygnalizacja_dawki, sygnalizacja_mocy_dawki, wegiel_slaby, wegiel_silny, stront_najsilniejszy);
            }
        }
        #endregion

        #region Dane Protokołu
        public class DaneProtokolu
        {
            public List<string> DanePodstawowe;
            public List<string> DaneTabelowe;

            public DaneProtokolu(List<string> danePodst, List<string> daneTabel)
            {
                DanePodstawowe = danePodst;
                DaneTabelowe = daneTabel;
            }
        }
        #endregion
        

        public class KartaPrzyjecia
        {
            private string _Zapytanie;
            private BazaDanychWrapper _BazaDanych;
            private DataTable _OdpowiedzBazy;
            public DaneKartyPrzyjecia DaneKartyPrzyjecia { get; set; }
            private List<string> _WszyskieTypyDozymetrow;
            private List<string> _WszyskieNumeryFabryczneDlaDanegoTypuDozymetru;
            public DaneProtokolu _DaneProtokolu { get; private set; }            

            //------------------------------------------------------------------
            public KartaPrzyjecia()
            //------------------------------------------------------------------
            {
                _BazaDanych = new BazaDanychWrapper();
                DaneKartyPrzyjecia = new DaneKartyPrzyjecia();

                _WszyskieTypyDozymetrow = new List<string>();
                _WszyskieNumeryFabryczneDlaDanegoTypuDozymetru = new List<string>();

                ZnajdzWszystkieTypyDozymetrow();
            }

            //------------------------------------------------------------------
            public KartaPrzyjecia(int idKarty, int nrZlecenia, int rok)
            //------------------------------------------------------------------
            {
                _BazaDanych = new BazaDanychWrapper();
                DaneKartyPrzyjecia = new DaneKartyPrzyjecia(idKarty, nrZlecenia, rok);

                _WszyskieTypyDozymetrow = new List<string>();
                _WszyskieNumeryFabryczneDlaDanegoTypuDozymetru = new List<string>();

                ZnajdzWszystkieTypyDozymetrow();
            }

            //------------------------------------------------------------------
            public DataTable Dane
            //------------------------------------------------------------------
            {
                get
                {
                    return _OdpowiedzBazy;
                }
            }

            //------------------------------------------------------------------
            public DanePrzyrzadu Przyrzad
            //------------------------------------------------------------------
            {
                get
                {
                    return DaneKartyPrzyjecia.Przyrzad;
                }
            }

            //------------------------------------------------------------------
            public List<string> NrFabryczne
            //------------------------------------------------------------------
            {
                get
                {
                    return _WszyskieNumeryFabryczneDlaDanegoTypuDozymetru;
                }
            }

            //------------------------------------------------------------------
            public void ZapiszKarte(ref DaneKartyPrzyjecia dane)
            //------------------------------------------------------------------
            {
                int idDozymetru = ZnajdzIdDozymetru(dane.Przyrzad.Typ, dane.Przyrzad.NrFabryczny);

                /*_Zapytanie = "INSERT INTO Karta_przyjecia (id_karty, id_zlecenia, rok, ameryk, chlor, dawka, moc_dawki, "
	                       + "pluton, stront_slaby, stront_silny, syg_dawki, syg_mocy_dawki, wegiel_slaby, wegiel_silny, stront_najsilniejszy, "
                           + "akcesoria, uwagi, uszkodzony, Sprawdzenie, id_dozymetru, test_na_skazenia, wykonano) VALUES "
                           + String.Format("({0},{1},'{2}',{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},'{15}','{16}',{17},{18},{19},'brak skażeń',{20})",
                           dane.IdKarty, dane.NrZlecenia, dane.rok, dane.Wymagania.dane[(int)WymaganiaKalibracji.Stale.AMERYK],
                           dane.Wymagania.dane[(int)WymaganiaKalibracji.Stale.CHLOR],dane.Wymagania.dane[(int)WymaganiaKalibracji.Stale.DAWKA],
                           dane.Wymagania.dane[(int)WymaganiaKalibracji.Stale.MOC_DAWKI],dane.Wymagania.dane[(int)WymaganiaKalibracji.Stale.PLUTON],
                           dane.Wymagania.dane[(int)WymaganiaKalibracji.Stale.STRONT_SLABY],dane.Wymagania.dane[(int)WymaganiaKalibracji.Stale.STRONT_SILNY],
                           dane.Wymagania.dane[(int)WymaganiaKalibracji.Stale.SYGNALIZACJA_DAWKI],dane.Wymagania.dane[(int)WymaganiaKalibracji.Stale.SYGNALIZACJA_MOCY_DAWKI],
                           dane.Wymagania.dane[(int)WymaganiaKalibracji.Stale.WEGIEL_SLABY],dane.Wymagania.dane[(int)WymaganiaKalibracji.Stale.WEGIEL_SILNY],
                           dane.Wymagania.dane[(int)WymaganiaKalibracji.Stale.STRONT_NAJSILNIEJSZY], dane.DaneDodatkowe.Akcesoria,
                           dane.DaneDodatkowe.Uwagi, dane.DaneDodatkowe.Uszkodzony, dane.DaneDodatkowe.Sprawdzenie, idDozymetru, dane.Wykonano);
                _BazaDanych.WykonajPolecenie(_Zapytanie);*/

                _BazaDanych.Karta_przyjecia
                    .INSERT()
                        .ID_karty(dane.IdKarty)
                        .ID_zlecenia(dane.NrZlecenia)
                        .Rok(dane.rok)
                        .Ameryk(dane.Wymagania.dane[(int)WymaganiaKalibracji.Stale.AMERYK])
                        .Chlor(dane.Wymagania.dane[(int)WymaganiaKalibracji.Stale.CHLOR])
                        .Dawka(dane.Wymagania.dane[(int)WymaganiaKalibracji.Stale.DAWKA])
                        .Moc_dawki(dane.Wymagania.dane[(int)WymaganiaKalibracji.Stale.MOC_DAWKI])
                        .Pluton(dane.Wymagania.dane[(int)WymaganiaKalibracji.Stale.PLUTON])
                        .Stront_slaby(dane.Wymagania.dane[(int)WymaganiaKalibracji.Stale.STRONT_SLABY])
                        .Stront_silny(dane.Wymagania.dane[(int)WymaganiaKalibracji.Stale.STRONT_SILNY])
                        .Syg_dawki(dane.Wymagania.dane[(int)WymaganiaKalibracji.Stale.SYGNALIZACJA_DAWKI])
                        .Syg_mocy_dawki(dane.Wymagania.dane[(int)WymaganiaKalibracji.Stale.SYGNALIZACJA_MOCY_DAWKI])
                        .Wegiel_slaby(dane.Wymagania.dane[(int)WymaganiaKalibracji.Stale.WEGIEL_SLABY])
                        .Wegiel_silny(dane.Wymagania.dane[(int)WymaganiaKalibracji.Stale.WEGIEL_SILNY])
                        .Stront_najsilniejszy(dane.Wymagania.dane[(int)WymaganiaKalibracji.Stale.STRONT_NAJSILNIEJSZY])
                        .Akcesoria(dane.DaneDodatkowe.Akcesoria)
                        .Uwagi(dane.DaneDodatkowe.Uwagi)
                        .Uszkodzony(dane.DaneDodatkowe.Uszkodzony)
                        .Sprawdzenie(dane.DaneDodatkowe.Sprawdzenie)
                        .ID_dozymetru(idDozymetru)
                        .Test_na_skazenia("brak skażeń")
                        .Wykonano(dane.Wykonano)
                    .INFO("Dodanie karty przyjęcia")
                    .EXECUTE();
            }

            //------------------------------------------------------------------
            public void NadpiszKarte(ref DaneKartyPrzyjecia dane)
            //------------------------------------------------------------------
            {
                if (null == dane)
                    return;

                int idDozymetru = ZnajdzIdDozymetru(dane.Przyrzad.Typ, dane.Przyrzad.NrFabryczny);

                _BazaDanych.Karta_przyjecia
                    .UPDATE()
                        .ID_zlecenia(dane.NrZlecenia)
                        .Rok(dane.rok)
                        .Ameryk(dane.Wymagania.dane[(int)WymaganiaKalibracji.Stale.AMERYK])
                        .Chlor(dane.Wymagania.dane[(int)WymaganiaKalibracji.Stale.CHLOR])
                        .Dawka(dane.Wymagania.dane[(int)WymaganiaKalibracji.Stale.DAWKA])
                        .Moc_dawki(dane.Wymagania.dane[(int)WymaganiaKalibracji.Stale.MOC_DAWKI])
                        .Pluton(dane.Wymagania.dane[(int)WymaganiaKalibracji.Stale.PLUTON])
                        .Stront_slaby(dane.Wymagania.dane[(int)WymaganiaKalibracji.Stale.STRONT_SLABY])
                        .Stront_silny(dane.Wymagania.dane[(int)WymaganiaKalibracji.Stale.STRONT_SILNY])
                        .Syg_dawki(dane.Wymagania.dane[(int)WymaganiaKalibracji.Stale.SYGNALIZACJA_DAWKI])
                        .Syg_mocy_dawki(dane.Wymagania.dane[(int)WymaganiaKalibracji.Stale.SYGNALIZACJA_MOCY_DAWKI])
                        .Wegiel_slaby(dane.Wymagania.dane[(int)WymaganiaKalibracji.Stale.WEGIEL_SLABY])
                        .Wegiel_silny(dane.Wymagania.dane[(int)WymaganiaKalibracji.Stale.WEGIEL_SILNY])
                        .Stront_najsilniejszy(dane.Wymagania.dane[(int)WymaganiaKalibracji.Stale.STRONT_NAJSILNIEJSZY])
                        .Akcesoria(dane.DaneDodatkowe.Akcesoria)
                        .Uwagi(dane.DaneDodatkowe.Uwagi)
                        .Uszkodzony(dane.DaneDodatkowe.Uszkodzony)
                        .Sprawdzenie(dane.DaneDodatkowe.Sprawdzenie)
                        .ID_dozymetru(idDozymetru)
                        .Wykonano(dane.Wykonano)
                    .WHERE()
                        .ID_karty(dane.IdKarty)
                    .INFO("Zmiana danych karty przyjęcia")
                    .EXECUTE();
            }

            //--------------------------------------------------------------------
            public string[] GetMozliweNrFabryczneDlaDozymetru(string klucz)
            //--------------------------------------------------------------------
            {
                var znalezione = from numer in _WszyskieNumeryFabryczneDlaDanegoTypuDozymetru where numer.ToLower().Contains(klucz.ToLower()) select numer;

                return znalezione.ToArray();
            }

            //--------------------------------------------------------------------
            public string[] GetMozliweTypyDozymetrow(string klucz)
            //--------------------------------------------------------------------
            {
                var znalezione = from typ in _WszyskieTypyDozymetrow where typ.ToLower().Contains(klucz.ToLower()) select typ;

                return znalezione.ToArray();
            }

            //--------------------------------------------------------------------
            public bool PobierzDanePoczatkowe()
            //--------------------------------------------------------------------
            {
                return ZnajdzDanePodstawowe() && ZaladujDanePrzyrzadu() && ZaladujDaneKalibracji() && ZaladujDaneDodatkowe();
            }

            //--------------------------------------------------------------------
            public bool PobierzDane(int idKarty)
            //--------------------------------------------------------------------
            {
                return ZnajdzDanePodstawowe(idKarty) && ZaladujDanePrzyrzadu() && ZaladujDaneKalibracji() && ZaladujDaneDodatkowe();
            }

            //--------------------------------------------------------------------
            public string PobierzRok(int nrKarty)
            //--------------------------------------------------------------------
            {
                string rok;
                
                _Zapytanie = String.Format("SELECT rok FROM Karta_przyjecia WHERE id_karty = {0}", nrKarty);

                try
                {
                    rok = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<int>(0).ToString();
                }
                catch (Exception)
                {
                    rok = "";
                }

                return rok;
            }

            //--------------------------------------------------------------------
            private DaneProtokolu PobierzDaneDoProtokolu(DaneKartyPrzyjecia dane)
            //--------------------------------------------------------------------
            {
                List<string> danePodstawowe = new List<string>(50);
                List<string> daneTabela = new List<string>(50);

                // dodanie id karty
                danePodstawowe.Add(dane.IdKarty.ToString());

                // dodanie roku
                danePodstawowe.Add( PobierzRok(dane.IdKarty) );

                _Zapytanie = "SELECT zleceniodawca, adres, osoba_kontaktowa, Z.id_zleceniodawcy, telefon, faks, email FROM Zleceniodawca "
                           + "AS Z INNER JOIN ZLECENIA AS ZL ON Z.id_zleceniodawcy = ZL.id_zleceniodawcy WHERE "
                           + String.Format("ZL.id_zlecenia={0}", dane.NrZlecenia);
                _OdpowiedzBazy = _BazaDanych.TworzTabeleDanych(_Zapytanie);

                // dodanie zleceniodawcy
                danePodstawowe.Add(_OdpowiedzBazy.Rows[0].Field<string>("Zleceniodawca"));
                // dodanie adresu
                danePodstawowe.Add(_OdpowiedzBazy.Rows[0].Field<string>("Adres"));
                // dodanie osoby kontaktowe
                danePodstawowe.Add(_OdpowiedzBazy.Rows[0].Field<string>("Osoba_kontaktowa"));
                // dodanie id zleceniodawcy
                danePodstawowe.Add(_OdpowiedzBazy.Rows[0].Field<int>("Id_Zleceniodawcy").ToString());
                // dodanie telefonu
                danePodstawowe.Add(_OdpowiedzBazy.Rows[0].Field<string>("Telefon"));
                // dodanie faksu
                danePodstawowe.Add(_OdpowiedzBazy.Rows[0].Field<string>("Faks"));
                // dodanie e-mail'u
                danePodstawowe.Add(_OdpowiedzBazy.Rows[0].Field<string>("Email"));


                _Zapytanie = "SELECT typ, nr_fabryczny, K.id_dozymetru FROM Dozymetry AS D INNER JOIN Karta_przyjecia AS K ON "
                           + String.Format("D.id_dozymetru=K.id_dozymetru WHERE K.id_karty = {0}", dane.IdKarty);
                _OdpowiedzBazy = _BazaDanych.TworzTabeleDanych(_Zapytanie);

                // dodanie typu
                danePodstawowe.Add(_OdpowiedzBazy.Rows[0].Field<string>("typ"));
                // dodanie typu
                danePodstawowe.Add(_OdpowiedzBazy.Rows[0].Field<string>("nr_fabryczny"));
                // dodanie id dozymetru
                danePodstawowe.Add(_OdpowiedzBazy.Rows[0].Field<int>("id_dozymetru").ToString());


                _Zapytanie = "SELECT typ, nr_fabryczny, S.id_sondy FROM Sondy AS S INNER JOIN Karta_przyjecia AS K ON S.id_dozymetru="
                           + String.Format("K.id_dozymetru WHERE K.id_karty = {0}", dane.IdKarty);
                char licznik = '1';

                foreach (DataRow row in _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows)
                {
                    daneTabela.Add(licznik.ToString());
                    ++licznik;
                    daneTabela.Add(row.Field<string>(0));
                    daneTabela.Add(row.Field<string>(1));
                    daneTabela.Add(row.Field<int>(2).ToString());
                }


                // pobranie informacji na temat akcesoriów
                _Zapytanie = String.Format("SELECT Akcesoria FROM Karta_przyjecia WHERE id_karty={0}", dane.IdKarty);
                danePodstawowe.Add(_BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<string>(0));


                // pobranie inf. na temat wymagań
                _Zapytanie = "SELECT moc_dawki, dawka, syg_dawki, syg_mocy_dawki, stront_slaby, stront_silny, wegiel_slaby, wegiel_silny, "
                           + String.Format("ameryk, pluton, chlor FROM Karta_przyjecia WHERE id_karty={0}", dane.IdKarty);
                _OdpowiedzBazy = _BazaDanych.TworzTabeleDanych(_Zapytanie);

                _Zapytanie = "";

                string[] nazwy = new string[12]{"moc dawki Cs-137", "dawka", "sygnalizacja dawki", "sygnalizacja mocy dawki", "Sr-90/Y-90",
                                                "Sr-90/Y-90 (silny)","C-14", "C-14 (silny)", "Am-241", "Pu-239", "Cl-36", "Sr-90/Y-90 (najsilniejszy)"};
                for (int i = 1; i <= 12; ++i)
                {
                    if (false != _OdpowiedzBazy.Rows[0].Field<bool>(i - 1))
                    {
                        _Zapytanie += String.Format("{0}, ", nazwy[i - 1]);
                    }
                }

                danePodstawowe.Add(_Zapytanie);

                _Zapytanie = String.Format("SELECT ekspres FROM Zlecenia WHERE id_zlecenia={0}", dane.NrZlecenia);
                danePodstawowe.Add(_BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<bool>(0).ToString());

                _Zapytanie = String.Format("SELECT data_przyjecia, data_zwrotu FROM Zlecenia WHERE id_zlecenia={0}", dane.NrZlecenia);
                _OdpowiedzBazy = _BazaDanych.TworzTabeleDanych(_Zapytanie);
                danePodstawowe.Add(_OdpowiedzBazy.Rows[0].Field<DateTime>(0).ToShortDateString());
                danePodstawowe.Add(_OdpowiedzBazy.Rows[0].Field<DateTime>(1).ToShortDateString());

                // test na skażenia
                _Zapytanie = String.Format("SELECT test_na_skazenia FROM Karta_przyjecia WHERE id_karty={0}", DaneKartyPrzyjecia.IdKarty);
                danePodstawowe.Add(_BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<string>(0).ToString());

                // osoba przyjmująca i uwagi
                _Zapytanie = String.Format("SELECT osoba_przyjmujaca, uwagi FROM Zlecenia WHERE id_zlecenia={0}", dane.NrZlecenia);
                _OdpowiedzBazy = _BazaDanych.TworzTabeleDanych(_Zapytanie);
                danePodstawowe.Add(_OdpowiedzBazy.Rows[0].Field<string>(0));
                danePodstawowe.Add(_OdpowiedzBazy.Rows[0].Field<string>(1));

                return new DaneProtokolu(danePodstawowe, daneTabela);
            }

            //--------------------------------------------------------------------
            public bool TworzProtokol(DaneKartyPrzyjecia dane)
            //--------------------------------------------------------------------
            {
                try
                {
                    _DaneProtokolu = PobierzDaneDoProtokolu(dane);
                }
                catch(Exception)
                {
                       return false;
                }
                 
                return true;
            }

            //------------------------------------------------------------------
            // Tworzy nowe id o 1 większe od dotychczas największego. Pozwala to
            // na zachowanie unikalności numerów. IdZlecenia jest pobierane z okna
            // Zlecenia. Właśnie dla danego id zostanie utworzona nowa karta.
            public bool UtworzNowaKartePrzyjecia(int idZlecenia)
            //------------------------------------------------------------------
            {
                _OdpowiedzBazy = _BazaDanych.TworzTabeleDanych("SELECT id_karty FROM Karta_przyjecia WHERE id_karty=(SELECT MAX(id_karty) FROM Karta_przyjecia)");

                try
                {
                    DaneKartyPrzyjecia.IdKarty = _OdpowiedzBazy.Rows[0].Field<int>(0) + 1;
                    DaneKartyPrzyjecia.NrZlecenia = idZlecenia;
                }
                catch (Exception)
                {
                    return false;
                }

                return true;
            }

            //------------------------------------------------------------------
            public bool ZaladujDaneDodatkowe()
            //------------------------------------------------------------------
            {
                _Zapytanie = String.Format("SELECT akcesoria, uwagi, uszkodzony, Sprawdzenie FROM Karta_przyjecia WHERE id_karty = {0}", DaneKartyPrzyjecia.IdKarty);

                _OdpowiedzBazy = _BazaDanych.TworzTabeleDanych(_Zapytanie);

                try
                {
                    DaneKartyPrzyjecia.DaneDodatkowe = new DaneDodatkowe(_OdpowiedzBazy.Rows[0].Field<string>(0),
                                                                         _OdpowiedzBazy.Rows[0].Field<string>(1),
                                                                         _OdpowiedzBazy.Rows[0].Field<bool>(2),
                                                                         _OdpowiedzBazy.Rows[0].Field<bool>(3));
                }
                catch (Exception)
                {
                    return false;
                }

                return true;
            }

            //-------------------------------------------------------------
            public bool SprawdzIstnieniePrzyrzadu(string typ, string numer_fabryczny)
            //-------------------------------------------------------------
            {
                _OdpowiedzBazy = _BazaDanych.TworzTabeleDanych(String.Format("SELECT 1 FROM Dozymetry WHERE typ='{0}' AND nr_fabryczny='{1}'", typ, numer_fabryczny));
                
                if(_OdpowiedzBazy == null || _OdpowiedzBazy.Rows.Count == 0)
                    return false;
                else
                    return true;
            }

            //------------------------------------------------------------------
            public bool ZaladujDaneKalibracji()
            //------------------------------------------------------------------
            {
                _Zapytanie = String.Format("SELECT ameryk, chlor, dawka, moc_dawki, pluton, stront_slaby, stront_silny, syg_dawki, syg_mocy_dawki, wegiel_slaby, wegiel_silny, stront_najsilniejszy FROM Karta_przyjecia WHERE id_karty = {0}", DaneKartyPrzyjecia.IdKarty);

                _OdpowiedzBazy = _BazaDanych.TworzTabeleDanych(_Zapytanie);

                try
                {
                    DaneKartyPrzyjecia.Wymagania = new WymaganiaKalibracji(_OdpowiedzBazy.Rows[0].Field<bool>("ameryk"),
                                                                           _OdpowiedzBazy.Rows[0].Field<bool>("chlor"),
                                                                           _OdpowiedzBazy.Rows[0].Field<bool>("dawka"),
                                                                           _OdpowiedzBazy.Rows[0].Field<bool>("moc_dawki"),
                                                                           _OdpowiedzBazy.Rows[0].Field<bool>("pluton"),
                                                                           _OdpowiedzBazy.Rows[0].Field<bool>("stront_slaby"),
                                                                           _OdpowiedzBazy.Rows[0].Field<bool>("stront_silny"),
                                                                           _OdpowiedzBazy.Rows[0].Field<bool>("syg_dawki"),
                                                                           _OdpowiedzBazy.Rows[0].Field<bool>("syg_mocy_dawki"),
                                                                           _OdpowiedzBazy.Rows[0].Field<bool>("wegiel_slaby"),
                                                                           _OdpowiedzBazy.Rows[0].Field<bool>("wegiel_silny"),
                                                                           _OdpowiedzBazy.Rows[0].Field<bool>("stront_najsilniejszy")
                                                                          );
                }
                catch (Exception)
                {
                    return false;
                }

                return true;
            }

            //------------------------------------------------------------------
            public bool ZaladujDanePrzyrzadu()
            //------------------------------------------------------------------
            {
                //-------------------sparsowanie typ i numeru fabrycznego-------------------
                _Zapytanie = String.Format("SELECT typ, nr_fabryczny FROM Dozymetry WHERE id_dozymetru = (SELECT id_dozymetru FROM Karta_przyjecia WHERE id_karty = {0})", DaneKartyPrzyjecia.IdKarty );
                _OdpowiedzBazy = _BazaDanych.TworzTabeleDanych(_Zapytanie);

                try
                {
                    DaneKartyPrzyjecia.Przyrzad = new DanePrzyrzadu(_OdpowiedzBazy.Rows[0].Field<string>("Typ"), _OdpowiedzBazy.Rows[0].Field<string>("Nr_fabryczny"));
                }
                catch (Exception)
                {
                    return false;
                }

                //-------------------sparsowanie id dozymtru--------------------------------
                _Zapytanie = String.Format("SELECT id_dozymetru FROM Dozymetry WHERE typ = '{0}' AND nr_fabryczny = '{1}'",
                                           DaneKartyPrzyjecia.Przyrzad.Typ, DaneKartyPrzyjecia.Przyrzad.NrFabryczny);
                _OdpowiedzBazy = _BazaDanych.TworzTabeleDanych(_Zapytanie);

                try
                {
                    DaneKartyPrzyjecia.idDozymetru = _OdpowiedzBazy.Rows[0].Field<int>("id_dozymetru");
                }
                catch (Exception)
                {
                    return false;
                }

                //----sparsowanie typ i numeru fabrycznego sond dla danego dozymetru--------
                _Zapytanie = String.Format("SELECT Typ, Nr_fabryczny FROM Sondy WHERE Sondy.ID_dozymetru = {0}", DaneKartyPrzyjecia.idDozymetru);
                _OdpowiedzBazy = _BazaDanych.TworzTabeleDanych(_Zapytanie);

                try
                {
                    foreach (DataRow row in _OdpowiedzBazy.Rows)
                    {
                        DaneKartyPrzyjecia.Przyrzad.ListaSond.Lista.Add(new Sonda(row.Field<string>("Typ"), row.Field<string>("Nr_fabryczny")));
                    }
                }
                catch (Exception)
                {
                    return false;
                }

                return true;
            }

            //--------------------------------------------------------------------------
            public int ZnajdzIdDozymetru(string typ, string nr_fabryczny)
            //--------------------------------------------------------------------------
            {
                _Zapytanie = String.Format("SELECT id_dozymetru FROM Dozymetry WHERE typ='{0}' AND nr_fabryczny='{1}'", typ, nr_fabryczny);

                int idDozymetru;

                try
                {
                    idDozymetru = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<int>(0);
                }
                catch(Exception)
                {
                    idDozymetru = 0;
                }

                return idDozymetru;
            }

            //--------------------------------------------------------------------------
            public int ZnajdzMaxIdDozymetru()
            //--------------------------------------------------------------------------
            {
                _Zapytanie = "SELECT MAX(id_dozymetru) FROM Dozymetry";

                int idDozymetru;

                try
                {
                    idDozymetru = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<int>(0);
                }
                catch (Exception)
                {
                    idDozymetru = 0;
                }

                return idDozymetru;
            }

            //--------------------------------------------------------------------------
            public string ZnajdzTyp(int idDozymetru)
            //--------------------------------------------------------------------------
            {
                _Zapytanie = String.Format("SELECT typ FROM Dozymetry WHERE id_dozymetru={0}", idDozymetru);

                string typ;

                try
                {
                    typ = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<string>(0);
                }
                catch (Exception)
                {
                    typ = "";
                }

                return typ;
            }

            //--------------------------------------------------------------------------
            public string ZnajdzNrFabryczny(int idDozymetru)
            //--------------------------------------------------------------------------
            {
                _Zapytanie = String.Format("SELECT nr_fabryczny FROM Dozymetry WHERE id_dozymetru={0}", idDozymetru);

                string nrFabryczny;

                try
                {
                    nrFabryczny = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<string>(0);
                }
                catch (Exception)
                {
                    nrFabryczny = "";
                }

                return nrFabryczny;
            }

            //------------------------------------------------------------------
            public int ZnajdzNrPoprzedniejKalibracji()
            //------------------------------------------------------------------
            {
                _Zapytanie = String.Format("SELECT MAX(id_karty) FROM Karta_przyjecia WHERE id_karty < {0} AND Id_dozymetru = {1}", DaneKartyPrzyjecia.IdKarty, DaneKartyPrzyjecia.idDozymetru);
                _OdpowiedzBazy = _BazaDanych.TworzTabeleDanych(_Zapytanie);

                if (null == _OdpowiedzBazy || null == _OdpowiedzBazy.Rows || null == _OdpowiedzBazy.Rows[0])
                    return 0;

                try
                {
                    return _OdpowiedzBazy.Rows[0].Field<int>(0);
                }
                catch (Exception)
                {
                    return 0;
                }
            }

            //------------------------------------------------------------------
            public int ZnajdzNrPoprzedniejKalibracji(string typDozymetru, string nrDozymetru)
            //------------------------------------------------------------------
            {
                if (typDozymetru == "" || nrDozymetru == "")
                    return 0;

                _Zapytanie = String.Format("SELECT id_dozymetru FROM Dozymetry WHERE typ = '{0}' AND nr_fabryczny = '{1}'", typDozymetru, nrDozymetru);
                DaneKartyPrzyjecia.idDozymetru = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<int>(0);
                
                _Zapytanie = String.Format("SELECT MAX(id_karty) FROM Karta_przyjecia WHERE id_karty < {0} AND Id_dozymetru = {1}", DaneKartyPrzyjecia.IdKarty, DaneKartyPrzyjecia.idDozymetru);
                _OdpowiedzBazy = _BazaDanych.TworzTabeleDanych(_Zapytanie);

                if (null == _OdpowiedzBazy || null == _OdpowiedzBazy.Rows || null == _OdpowiedzBazy.Rows[0])
                    return 0;

                try
                {
                    return _OdpowiedzBazy.Rows[0].Field<int>(0);
                }
                catch (Exception)
                {
                    return 0;
                }
            }

            //------------------------------------------------------------------
            // Znajduje ostatnią zapisaną kartę w bazie oraz id_zlecenia jej przypisany
            public bool ZnajdzDanePodstawowe()
            //------------------------------------------------------------------
            {
                _OdpowiedzBazy = _BazaDanych.TworzTabeleDanych("SELECT id_karty, id_zlecenia, rok, wykonano FROM Karta_przyjecia WHERE id_karty=(SELECT MAX(id_karty) FROM Karta_przyjecia)");

                try
                {
                    DaneKartyPrzyjecia.IdKarty = _OdpowiedzBazy.Rows[0].Field<int>(0);
                    DaneKartyPrzyjecia.NrZlecenia = _OdpowiedzBazy.Rows[0].Field<int>(1);
                    DaneKartyPrzyjecia.rok = _OdpowiedzBazy.Rows[0].Field<int>(2);
                    DaneKartyPrzyjecia.Wykonano = _OdpowiedzBazy.Rows[0].Field<bool>(3);
                }
                catch (Exception)
                {
                    return false;
                }

                return true;
            }

            //------------------------------------------------------------------
            // Znajduje ostatnią zapisaną kartę w bazie oraz id_zlecenia jej przypisany
            public bool ZnajdzDanePodstawowe(int idKarty)
            //------------------------------------------------------------------
            {
                _Zapytanie = String.Format("SELECT id_zlecenia, rok, wykonano FROM Karta_przyjecia WHERE id_karty={0}", idKarty);
                
                try
                {
                    DaneKartyPrzyjecia.IdKarty = idKarty;
                    DataTable response = _BazaDanych.TworzTabeleDanych(_Zapytanie);
                    DaneKartyPrzyjecia.NrZlecenia = response.Rows[0].Field<int>(0);
                    DaneKartyPrzyjecia.rok = response.Rows[0].Field<int>(1);
                    DaneKartyPrzyjecia.Wykonano = response.Rows[0].Field<bool>(2);
                }
                catch (Exception)
                {
                    return false;
                }

                return true;
            }

            //------------------------------------------------------------------
            // Znajduje ostatnią zapisaną kartę w bazie.
            public bool ZnajdzOstatniNumerKartyOrazJejIdZlecenia()
            //------------------------------------------------------------------
            {
                _OdpowiedzBazy = _BazaDanych.TworzTabeleDanych("SELECT id_karty, id_zlecenia FROM Karta_przyjecia WHERE id_karty=(SELECT MAX(id_karty) FROM Karta_przyjecia)");

                try
                {
                    DaneKartyPrzyjecia.IdKarty = _OdpowiedzBazy.Rows[0].Field<int>(0);
                    DaneKartyPrzyjecia.NrZlecenia = _OdpowiedzBazy.Rows[0].Field<int>(1);
                }
                catch (Exception)
                {
                    return false;
                }

                return true;
            }

            //------------------------------------------------------------------
            public bool ZnajdzSondy(string typ, string numer)
            //------------------------------------------------------------------
            {
                _Zapytanie = "SELECT typ, nr_fabryczny FROM Sondy WHERE id_dozymetru=(SELECT id_dozymetru FROM Dozymetry WHERE "
                           + String.Format("typ='{0}' AND nr_fabryczny='{1}')", typ, numer);

                DaneKartyPrzyjecia.Przyrzad = new DanePrzyrzadu(typ, numer);

                try
                {
                    foreach (DataRow row in _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows)
                    {
                        DaneKartyPrzyjecia.Przyrzad.ListaSond.Lista.Add(new Sonda(row.Field<string>(0), row.Field<string>(1)));
                    }
                }
                catch (Exception)
                {
                    return false;
                }

                return true;
            }

            //------------------------------------------------------------------
            // Metoda wywoływana przy każdorazowej zmianie 
            // wybranej sondy przez użytkownika
            public void ZnajdzWszystkieNrFabryczneDlaSondy(string typSondy)
            //------------------------------------------------------------------
            {
                _Zapytanie = String.Format("SELECT nr_fabryczny FROM Dozymetry WHERE typ = '{0}'", typSondy);

                DataTable dane = _BazaDanych.TworzTabeleDanych(_Zapytanie);

                _WszyskieNumeryFabryczneDlaDanegoTypuDozymetru.Clear();

                foreach (DataRow row in dane.Rows)
                {
                    _WszyskieNumeryFabryczneDlaDanegoTypuDozymetru.Add(row.Field<string>(0));
                }
            }

            //------------------------------------------------------------------
            // Metoda wywoływanan jest tylko raz podczas tworzenia okna. 
            // Następnie w zależności od potrzeb uzyskane przez nią dane są 
            // filtrowane i udostępniane użytkownikowi
            public void ZnajdzWszystkieTypyDozymetrow()
            //------------------------------------------------------------------
            {
                _Zapytanie = "SELECT DISTINCT typ FROM Dozymetry";

                DataTable dane = _BazaDanych.TworzTabeleDanych(_Zapytanie);

                _WszyskieTypyDozymetrow.Clear();

                foreach (DataRow row in dane.Rows)
                {
                    _WszyskieTypyDozymetrow.Add(row.Field<string>(0));
                }
            }


        }
    }
}
