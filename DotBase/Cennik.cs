using System;
using System.Collections.Generic;
using System.Data;
using System.Text;


namespace DotBase
{
    class Cennik
    {
        private BazaDanychWrapper _BazaDanych;
        private String _Zapytanie;

        private int _NrKarty;

		public int Ile_moc_dawki{ get; private set; }
        public int Ile_skazenia{ get; private set; }
        public int Ile_dawka{ get; private set; }
        public int Ile_sygnalizator{ get; private set; }

        private double _Cena_moc_dawki;
        private double _Cena_moc_dawki_rozsz;
        private double _Cena_skazenia;
        private double _Cena_skazenia_dod;
        private double _Cena_dawka;
        private double _Cena_dawka_dod;
        private double _Cena_sygnalizator;
        private double _Cena_sygnalizator_dod;
        private double _Cena_ekspres;
        private double _Cena_dodatkowy_prog;

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

        //-------------------------------------------------------
        public double LiczSumeAutomatycznie()
        //-------------------------------------------------------
        {
            // ostateczne kwoty wybrane z danych zawartych przez klasę (zostały de facto pobrane z bazy)
            // wybrane na podstawie poniższych zależności
            double k_moc_dawki, suma = 0.0;

            k_moc_dawki = _Cena_moc_dawki;


            if (Ile_moc_dawki + Ile_skazenia + Ile_sygnalizator > 0)
                suma = (Ile_moc_dawki + Ile_skazenia + Ile_sygnalizator - 1) * _Cena_skazenia_dod
                        + k_moc_dawki + Ile_dawka * _Cena_dawka_dod;
            else
                suma = Ile_dawka * _Cena_dawka;

            return suma;
        }

        //-------------------------------------------------------
        public double LiczSume(int ileSk, int ileMd, int ileD, int ileSyg, bool RozszerzoneWzorcowanie, bool Ekspres, uint liczbaProgow, uint rabat)
        //-------------------------------------------------------
        {
            Ile_moc_dawki = ileMd;
            Ile_skazenia = ileSk;
            Ile_dawka = ileD;
            Ile_sygnalizator = ileSyg;

	        // ostateczne kwoty wybrane z danych zawartych przez klasę (zostały de facto pobrane z bazy)
	        // wybrane na podstawie poniższych zależności
	        double k_moc_dawki, suma = 0.0;

	        // sprawdzenie czy wybrane jest rozszerzone wzorcowanie
            if (RozszerzoneWzorcowanie)
                k_moc_dawki = _Cena_moc_dawki_rozsz;
            else
                k_moc_dawki = _Cena_moc_dawki;
	

	        if( Ile_moc_dawki + Ile_skazenia + Ile_sygnalizator > 0 )
		        suma = (Ile_moc_dawki + Ile_skazenia + Ile_sygnalizator-1) * _Cena_skazenia_dod 
                        + k_moc_dawki + Ile_dawka * _Cena_dawka_dod;
	        else
		        suma = Ile_dawka*_Cena_dawka;

            if (Ekspres)
		        suma += _Cena_ekspres;

            if (liczbaProgow + 3 > 3)
		        suma += _Cena_dodatkowy_prog * liczbaProgow;

	        if(rabat > 0)
		        suma = suma*(1.0 - rabat / 100.0);

            return suma;
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
                            _Cena_moc_dawki = row.Field<double>("cena");
                            break;
                        case "Moc dawki rozsz":
                            _Cena_moc_dawki_rozsz  = row.Field<double>("cena");;
                            break;
                        case "Skażenia same":
                            _Cena_skazenia  = row.Field<double>("cena");;
                            break;
                        case "Skażenia dodatkowe":
                            _Cena_skazenia_dod  = row.Field<double>("cena");;
                            break;
                        case "Dawka sama":
                            _Cena_dawka  = row.Field<double>("cena");;
                            break;
                        case "Dawka dodatkowa":
                            _Cena_dawka_dod  = row.Field<double>("cena");;
                            break;
                        case "Sygnalizacja 3progi":
                            _Cena_sygnalizator  = row.Field<double>("cena");;
                            break;
                        case "Sygnalizacja 3progi dodatkowa":
                            _Cena_sygnalizator_dod  = row.Field<double>("cena");;
                            break;
                        case "Sygnalizacja dodatkowy próg":
                            _Cena_dodatkowy_prog  = row.Field<double>("cena");;
                            break;
                        case "Ekspres":
                            _Cena_ekspres  = row.Field<double>("cena");;
                            break;
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
        public bool ZliczLiczbeWzorcowanDlaMocyDawki()
        //-----------------------------------------------------------------------------
        {
	        _Zapytanie = "SELECT COUNT(id_arkusza) FROM Wzorcowanie_cezem WHERE id_karty="
                       + String.Format("{0} AND rodzaj_wzorcowania='md'", _NrKarty);
	        try
            {
                Ile_moc_dawki = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<int>(0);
	        }
            catch(Exception)
            {
                return false;
            }

            return true;
		}

        //-----------------------------------------------------------------------------
        public bool ZliczLiczbeWzorcowanDlaSkazen()
        //-----------------------------------------------------------------------------
        {
	        _Zapytanie = "SELECT COUNT(id_arkusza) FROM Wzorcowanie_zrodlami_powierzchniowymi WHERE "
                       + String.Format("id_karty = {0}", _NrKarty);
	        try
            {
                Ile_skazenia = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<int>(0);
	        }
            catch(Exception)
            {
                return false;
            }

            return true;
		}

        //-----------------------------------------------------------------------------
        public bool ZliczLiczbeWzorcowanDlaDawki()
        //-----------------------------------------------------------------------------
        {
	        _Zapytanie = "SELECT COUNT(id_arkusza) FROM Wzorcowanie_cezem WHERE id_karty="
                       + String.Format("{0} AND rodzaj_wzorcowania='d'", _NrKarty);
	        try
            {
                Ile_dawka = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<int>(0);
	        }
            catch(Exception)
            {
                return false;
            }

            return true;
		}

        //-----------------------------------------------------------------------------
        public bool ZliczLiczbeWzorcowanDlaSygnalizacji()
        //-----------------------------------------------------------------------------
        {
	        _Zapytanie = "SELECT COUNT(id_arkusza) FROM Wzorcowanie_cezem WHERE id_karty="
                       + String.Format("{0} AND rodzaj_wzorcowania='sd'", _NrKarty);
	        try
            {
                Ile_sygnalizator = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<int>(0);
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

            if( _NrKarty > 0 && Double.TryParse(suma, out cena) )
            {
                _Zapytanie = String.Format("UPDATE Karta_przyjecia SET Cena = {0} WHERE id_karty = {1}", cena, _NrKarty);
	            _BazaDanych.WykonajPolecenie(_Zapytanie);
	        }
        }

    }
}
