using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotBase;
using System.Data;

namespace PrzestrzenSody
{
    class Sonda
    {
        private int _idPrzyrzadu;
        private BazaDanychWrapper _BazaDanych;
        private String _Zapytanie;

        public int IdSondy { get; private set; }
        public string Typ { get; private set; }
        public string NrFabryczny { get; private set; }

        //**************************************************
        public Sonda(int idPrzyrzadu)
        //**************************************************
        {
            _BazaDanych = new BazaDanychWrapper();
            _idPrzyrzadu = idPrzyrzadu;
        }

        //**************************************************
        public void AktualizujDaneSondy(int idSondy, string typ, string nrFabryczny)
        //**************************************************
        {
            _Zapytanie = String.Format("UPDATE Sondy SET typ = '{0}', nr_fabryczny = '{1}' WHERE id_sondy = {2}", typ, nrFabryczny, idSondy);

            _BazaDanych.WykonajPolecenie(_Zapytanie);
        }

        //**************************************************
        public bool DodajSonde(int idSondy, string typ, string nrFabryczny)
        //**************************************************
        {
            if (idSondy < 0)
                return false;

            _Zapytanie = String.Format("INSERT INTO Sondy (id_sondy, typ, nr_fabryczny, id_dozymetru) VALUES ({0},'{1}','{2}',{3})", 
                         idSondy, typ, nrFabryczny, _idPrzyrzadu);

            _BazaDanych.WykonajPolecenie(_Zapytanie);

            return true;
        }

        //**************************************************
        public bool PobierzDane(int id_sondy)
        //**************************************************
        {
            _Zapytanie = String.Format("SELECT typ, nr_fabryczny FROM Sondy where id_sondy = {0}", id_sondy);

            DataRow rekord = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0];

            try
            {
                Typ = rekord.Field<string>(0);
                NrFabryczny = rekord.Field<string>(1);
                IdSondy = id_sondy;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //**************************************************
        public int UtworzNowyNumerSondy()
        //**************************************************
        {
            int temp = ZnajdzOstatniaSonde();
            if (temp == -1)
                return -1;

            return temp + 1;
        }

        //**************************************************
        public int ZnajdzNumerSondy(int idDozymetru, string typ, string nrFabryczny)
        //**************************************************
        {
            _Zapytanie = String.Format("SELECT id_sondy FROM Sondy WHERE id_dozymetru = {0} AND typ = '{1}' AND nr_fabryczny = '{2}'", idDozymetru, typ, nrFabryczny);
            return _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<int>(0);
        }

        //****************************************
        public int ZnajdzNastepna(int i)
        //****************************************
        {
            _Zapytanie = String.Format("SELECT MIN(id_sondy) FROM Sondy WHERE id_sondy > {0}", i);

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
        public int ZnajdzPoprzednia(int i)
        //****************************************
        {
            _Zapytanie = String.Format("SELECT MAX(id_sondy) FROM Sondy WHERE id_sondy < {0}", i);

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
        public int ZnajdzOstatniaSonde()
        //****************************************
        {
            _Zapytanie = "SELECT MAX(id_sondy) FROM Sondy";

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
