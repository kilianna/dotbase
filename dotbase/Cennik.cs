using System;
using System.Collections.Generic;
using System.Data;
using System.Text;


namespace DotBase
{
    class Cennik
    {
        private BazaDanychWrapper _BazaDanych;

        private int _NrKarty;

		public int Ile_moc_dawki{ get; private set; }
        public int Ile_skazenia{ get; private set; }
        public int Ile_dawka{ get; private set; }
        public int Ile_sygnalizator{ get; private set; }

        private double _Cena_Moc_dawki_podst;
        private double _Cena_Dawka_podst;
        private double _Cena_Skażenia_podst;
        private double _Cena_Sygnalizacja_3progi_podst;
        private double _Cena_Moc_dawki_dod;
        private double _Cena_Skazenia_dod;
        private double _Cena_Dawka_dod;
        private double _Cena_Sygnalizacja_3progi_dod;
        private double _Cena_Moc_dawki_rozsz;
        private double _Cena_Moc_dawki_trudna;
        private double _Cena_Skazenia_dlugie;
        private double _Cena_Dawka_dluga;
        private double _Cena_Sygnalizacja_dodatkowy_prog;
        private double _Cena_Ekspres;
        private double _Cena_Sprawdzenie;

        //-------------------------------------------------------
        public Cennik()
        //-------------------------------------------------------
        {
            _BazaDanych = new BazaDanychWrapper();
            _NrKarty = 0;
        }

        //-------------------------------------------------------
        public Cennik(int nrKarty)
            : this()
        //-------------------------------------------------------
        {
            _NrKarty = nrKarty;
        }

        //-------------------------------------------------------
        public bool Inicjalizuj()
        //-------------------------------------------------------
        {
            return WczytajCenyUslug();
        }

        private double JEŻELI(bool warunek, double prawda, double falsz)
        {
            return warunek ? prawda : falsz;
        }

        private double SUMA(params double[] x)
        {
            double sum = 0.0;
            foreach (double v in x)sum += v;
            return sum;
        }

        private double NUM(bool x)
        {
            return x ? 1 : 0;
        }

        //-------------------------------------------------------
        public double LiczSume(int ileMd,
                               int ileSk,
                               int ileD, 
                               int ileSyg, 
                               bool RozszerzoneWzorcowanie,
                               bool TrudneWzorcowanie,
                               bool DawkaDluga,
                               bool SkazeniaDlugie,
                               bool Ekspres, 
                               bool sprawdzenie,
                               bool zepsuty,
                               uint liczbaProgow,
                               double rabat,
                               double transport)
        //-------------------------------------------------------
        {
            Ile_moc_dawki = ileMd;
            Ile_skazenia = ileSk;
            Ile_dawka = ileD;
            Ile_sygnalizator = ileSyg;

            if (zepsuty)
            {
                return transport;
            }

            if (sprawdzenie)
            {
                return _Cena_Sprawdzenie * (1.0 - rabat / 100.0) + transport;
            }
            
            var C5 = Ile_moc_dawki;
            var D5 = Ile_skazenia;
            var E5 = Ile_sygnalizator;
            var F5 = Ile_dawka;

            var D10 = RozszerzoneWzorcowanie;
            var E10 = sprawdzenie;
            var F10 = Ekspres;
            var G10 = liczbaProgow;
            var H10 = TrudneWzorcowanie;
            var I10 = DawkaDluga;
            var J10 = SkazeniaDlugie;
            
            var L14 = _Cena_Moc_dawki_podst;
            var L15 = _Cena_Dawka_podst;
            var L16 = _Cena_Skażenia_podst;
            var L17 = _Cena_Sygnalizacja_3progi_podst;
            var L18 = _Cena_Moc_dawki_dod;
            var L19 = _Cena_Skazenia_dod;
            var L20 = _Cena_Dawka_dod;
            var L21 = _Cena_Sygnalizacja_3progi_dod;
            var L22 = _Cena_Moc_dawki_rozsz;
            var L23 = _Cena_Moc_dawki_trudna;
            var L24 = _Cena_Skazenia_dlugie;
            var L25 = _Cena_Dawka_dluga;
            var L26 = _Cena_Sygnalizacja_dodatkowy_prog;
            var L27 = _Cena_Ekspres;
            var L28 = _Cena_Sprawdzenie;

            var C12 = rabat;
            var C13 = transport;

            double subtotal_moc_dawki = (L14 + (C5 - 1) * L18) * NUM(C5 != 0);
            double subtotal_skazenia = JEŻELI(C5 + F5 + E5 > 0, D5 * L19, (L16 + (D5 - 1) * L19) * NUM(D5 != 0));
            double subtotal_sygnalizator = JEŻELI(SUMA(C5, F5) > 0, E5 * L21, (L17 + (E5 - 1) * L21) * NUM(E5 != 0));
            double subtotal_dawka = JEŻELI(C5 > 0, F5 * L20, (L15 + (F5 - 1) * L20) * NUM(F5 != 0));

            double dopl_rozszerzone_wzorcowanie = JEŻELI(D10, L22, 0);
            double dopl_sprawdzenie = JEŻELI(E10, L28, 0);
            double dopl_ekspres = JEŻELI(F10, L27, 0);
            double dopl_dodatkowe_progi = G10 * L26;
            double dopl_trudny = JEŻELI(H10, L23, 0);
            double dopl_dawka_dluga = JEŻELI(I10, L25, 0);
            double dopl_skazenia_dlugie = JEŻELI(J10, L24, 0);

            double total = C13 + (1 - C12 / 100) * SUMA(subtotal_moc_dawki, subtotal_skazenia, subtotal_sygnalizator,
                subtotal_dawka, dopl_rozszerzone_wzorcowanie, dopl_sprawdzenie, dopl_ekspres,
                dopl_dodatkowe_progi, dopl_trudny, dopl_dawka_dluga, dopl_skazenia_dlugie);

            return total;
        }

        //-------------------------------------------------------
        private bool WczytajCenyUslug()
        //-------------------------------------------------------
        {
            string usluga;

            try
            {
                foreach(DataRow row in _BazaDanych.TworzTabeleDanych("SELECT usluga, cena FROM Cennik").Rows)
                {
                    usluga = row.Field<string>("usluga");

                    switch(usluga)
                    {
                        case "Moc dawki podst":
                            _Cena_Moc_dawki_podst = row.Field<double>("cena");
                            break;
                        case "Dawka podst":
                            _Cena_Dawka_podst = row.Field<double>("cena");
                            break;
                        case "Skażenia podst":
                            _Cena_Skażenia_podst = row.Field<double>("cena");
                            break;
                        case "Sygnalizacja 3progi podst":
                            _Cena_Sygnalizacja_3progi_podst = row.Field<double>("cena");
                            break;
                        case "Moc dawki dod":
                            _Cena_Moc_dawki_dod = row.Field<double>("cena");
                            break;
                        case "Skażenia dod":
                            _Cena_Skazenia_dod = row.Field<double>("cena");
                            break;
                        case "Dawka dod":
                            _Cena_Dawka_dod = row.Field<double>("cena");
                            break;
                        case "Sygnalizacja 3progi dod":
                            _Cena_Sygnalizacja_3progi_dod = row.Field<double>("cena");
                            break;
                        case "Moc dawki rozsz":
                            _Cena_Moc_dawki_rozsz = row.Field<double>("cena");
                            break;
                        case "Moc dawki trudna":
                            _Cena_Moc_dawki_trudna = row.Field<double>("cena");
                            break;
                        case "Skażenia długie":
                            _Cena_Skazenia_dlugie = row.Field<double>("cena");
                            break;
                        case "Dawka długa":
                            _Cena_Dawka_dluga = row.Field<double>("cena");
                            break;
                        case "Sygnalizacja dodatkowy próg":
                            _Cena_Sygnalizacja_dodatkowy_prog = row.Field<double>("cena");
                            break;
                        case "Ekspres":
                            _Cena_Ekspres = row.Field<double>("cena");
                            break;
                        case "Sprawdzenie":
                            _Cena_Sprawdzenie = row.Field<double>("cena");
                            break;
                        default:
                            return false;
                    }
                }
            }
            catch(Exception)
            {
                return false;
            }
            
            return true;
        }

        //-----------------------------------------------------------------------------
        public void Zapisz(string suma)
        //-----------------------------------------------------------------------------
        {
            double cena;

            if( _NrKarty > 0 && N.doubleTryParse(suma, out cena) )
            {
                _BazaDanych.Karta_przyjecia
                    .UPDATE()
                        .Cena(N.doubleParse(suma))
                    .WHERE()
                        .ID_karty(_NrKarty)
                    .INFO("Zapis ceny w karcie przyjęcia")
                    .EXECUTE();
	        }
        }

    }
}
