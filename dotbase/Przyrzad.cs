using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DotBase
{
    class Przyrzad
    {
        public string Nazwa { get; private set; }
        public string NumerFabryczny { get; private set; }
        public int NumerPrzyrzadu { get; private set; }
        public string Producent { get; private set; }
        public string RokProdukcji { get; private set; }
        public string Typ { get; private set; }

        public Narzedzia.ListaSond Sondy { get; private set; }

        private BazaDanychWrapper _BazaDanych;
        private string _Zapytanie;

        //****************************************
        private Przyrzad()
        //****************************************
        {
            _BazaDanych = new BazaDanychWrapper();
            Sondy = new Narzedzia.ListaSond();
        }

        //****************************************
        public Przyrzad(int idKarty) 
            : this()
        //****************************************
        {
            StorzNowyNumerPrzyrzadu();    
        }

        //****************************************
        public Przyrzad(string typ, string nrFabryczny)
            : this()
        //****************************************
        {
            ZnajdzIdDozymetru(typ,nrFabryczny);
        }

        //****************************************
        public bool PobierzDane(int numer)
        //****************************************
        {
            _Zapytanie = String.Format("SELECT typ, nr_fabryczny, rok_produkcji, nazwa, producent FROM Dozymetry WHERE id_dozymetru = {0}", numer);

            try
            {
                DataRow rekord = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0];

                Typ = rekord.Field<string>(0);
                NumerFabryczny = rekord.Field<string>(1);
                RokProdukcji = rekord.Field<string>(2);
                Nazwa = rekord.Field<string>(3);
                Producent = rekord.Field<string>(4);

                NumerPrzyrzadu = numer;

                PobierzSondy();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //****************************************
        public void PobierzSondy()
        //****************************************
        {
            Sondy.Lista.Clear();

            _Zapytanie = String.Format("SELECT typ, nr_fabryczny FROM Sondy WHERE id_dozymetru = {0}", NumerPrzyrzadu);
            
            if( null != _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows )
            {
                foreach( DataRow wiersz in _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows)
                {
                    Sondy.Lista.Add( new Narzedzia.Sonda(wiersz.Field<string>(0), wiersz.Field<string>(1)) );
                }
            }
        }

        //****************************************
        public int StorzNowyNumerPrzyrzadu()
        //****************************************
        {
            NumerPrzyrzadu = ZnajdzOstatniPrzyrzad() + 1;
            return NumerPrzyrzadu;
        }

        //****************************************
        public bool SprawdzCzyPrzyrzadIsnieje(int idPrzyrzadu)
        //****************************************
        {
            _Zapytanie = String.Format("SELECT 1 FROM Dozymetry WHERE id_dozymetru = {0}", idPrzyrzadu);

            try
            {
                _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<int>(0);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        //****************************************
        public bool SprawdzCzyPrzyrzadNieIsnieje(string typ, string nrFabryczny)
        //****************************************
        {
            _Zapytanie = String.Format("SELECT 1 FROM Dozymetry WHERE typ = '{0}' AND nr_fabryczny = '{1}'", typ, nrFabryczny);
            
            try
            {
                _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<int>(0);
            }
            catch (Exception)
            {
                return true;
            }

            return false;
        }

        //****************************************
        public void ZnajdzIdDozymetru(string typ, string nrFabryczny)
        //****************************************
        {
            _Zapytanie = String.Format("SELECT id_dozymetru, rok_produkcji, nazwa, producent FROM Dozymetry WHERE typ = '{0}' AND nr_fabryczny = '{1}'", typ, nrFabryczny);

            Typ = typ;
            NumerFabryczny = nrFabryczny;

            try
            {
                DataTable dane = _BazaDanych.TworzTabeleDanych(_Zapytanie);

                NumerPrzyrzadu = dane.Rows[0].Field<int>(0);
                RokProdukcji = dane.Rows[0].Field<string>(1);
                Nazwa = dane.Rows[0].Field<string>(2);
                Producent = dane.Rows[0].Field<string>(3);
            }
            catch (Exception)
            {
                NumerPrzyrzadu = -1;
            }
        }

        //****************************************
        public void NapiszDane(string typ, string nrFabryczny, string rokProdukcji, string producent, string nazwa)
        //****************************************
        {
            _Zapytanie = String.Format("UPDATE Dozymetry SET typ = '{0}', nr_fabryczny = '{1}', rok_produkcji = '{2}', ",
                         typ, nrFabryczny, rokProdukcji)
                       + String.Format("producent = '{0}', nazwa = '{1}' WHERE id_dozymetru = {2}",
                         producent, nazwa, NumerPrzyrzadu);

            _BazaDanych.WykonajPolecenie(_Zapytanie);
        }

        //****************************************
        public void Zapisz(string typ, string nrFabryczny, string rokProdukcji, string producent, string nazwa)
        //****************************************
        {
            _Zapytanie = "INSERT INTO Dozymetry (id_dozymetru, typ, nr_fabryczny, rok_produkcji, producent, nazwa) VALUES "
                       + String.Format("({0},'{1}','{2}','{3}','{4}','{5}')", NumerPrzyrzadu, typ, nrFabryczny, rokProdukcji, producent, nazwa);

            _BazaDanych.WykonajPolecenie(_Zapytanie);
        }

        //****************************************
        public int ZnajdzNastepny(int i)
        //****************************************
        {
            _Zapytanie = String.Format("SELECT MIN(id_dozymetru) FROM Dozymetry WHERE id_dozymetru > {0}", i);

            try
            {
                return _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<int>(0);
            }
            catch (Exception)
            {
                return -1;
            }
        }

        //****************************************
        public int ZnajdzPoprzedni(int i)
        //****************************************
        {
            _Zapytanie = String.Format("SELECT MAX(id_dozymetru) FROM Dozymetry WHERE id_dozymetru < {0}", i);

            try
            {
                return _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<int>(0);
            }
            catch (Exception)
            {
                return -1;
            }
        }

        //****************************************
        public int ZnajdzOstatniPrzyrzad()
        //****************************************
        {
            _Zapytanie = "SELECT MAX(id_dozymetru) FROM Dozymetry";

            try
            {
                return _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<int>(0);
            }
            catch (Exception)
            {
                return -1;
            }
        }
    }
}
