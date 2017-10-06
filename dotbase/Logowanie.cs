using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Data;

namespace DotBase
{
    class Logowanie
    {
        private string     _SciezkaDoBazy;
        private string     _HasloDoBazy;
        private string     _HasloPracownika;
        public string      _Login;
        private BazaDanychWrapper _BazaDanych;

        //----------------------------------------------------------------------------------
        public Logowanie(string sciezka, string hasloBaza, string hasloPracownik)
        //----------------------------------------------------------------------------------
        {
            _SciezkaDoBazy      = sciezka;
            _HasloDoBazy        = hasloBaza;
            _HasloPracownika    = hasloPracownik;
        }

        //----------------------------------------------------------------------------------
        public bool LogujDoBazy()
        //----------------------------------------------------------------------------------
        {
            if (0 == String.Empty.CompareTo(_SciezkaDoBazy) ||
                0 == String.Empty.CompareTo(_HasloDoBazy)   ||
                0 == String.Empty.CompareTo(_HasloPracownika))
            {
                return false;
            }

            _BazaDanych = new BazaDanychWrapper();
            _BazaDanych.TworzConnectionString(_SciezkaDoBazy, _HasloDoBazy);
            
            if (false == _BazaDanych.Polacz())
                return false;

            string zapytanie = String.Format("SELECT haslo FROM Hasla WHERE login='{0}'", _Login);

            DataTable dane = _BazaDanych.TworzTabeleDanych(zapytanie);

            if( false == TestujHasloPracownika(dane.Rows[0].Field<string>(0)))
            {
                dane = null;
                return false;
            }

            dane = null;

            _BazaDanych.Rozlacz();

            return true;
        }

        //----------------------------------------------------------------------------------
        public string Login
        //----------------------------------------------------------------------------------
        {
            set
            {
                _Login = value;
            }

            get
            {
                return _Login;
            }
        }

        //----------------------------------------------------------------------------------
        public bool TestujHasloPracownika(string haslo)
        //----------------------------------------------------------------------------------
        {
            if (0 == _HasloPracownika.CompareTo(haslo))
                return true;

            return false;
        }
    }
}
