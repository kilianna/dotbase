using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;

namespace DotBase
{
    class BazaDanychWrapper
    {
        static string _ConnectionString;
        public DataTable Tabela {get; private set;}

        OleDbConnection _Polaczenie;

        //--------------------------------------------------------------------
        public BazaDanychWrapper()
        //--------------------------------------------------------------------
        {
            Tabela = new DataTable();
        }


        //--------------------------------------------------------------------
        string ConnectionString
        //--------------------------------------------------------------------
        {
            get
            {
                return BazaDanychWrapper._ConnectionString;
            }
        }

        //----------------------------------------------------------------------------------
        public bool WykonajPolecenie(string zapytanie)
        //----------------------------------------------------------------------------------
        {
            if (false == Polacz())
                return false;
            
            OleDbCommand polecenie = new OleDbCommand(zapytanie, _Polaczenie);              // Tworzymy polecenie dla bazy danych.

            try
            {
                polecenie.ExecuteNonQuery();
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                Rozlacz();
            }

            return true;
        }

        //----------------------------------------------------------------------------------
        public bool Polacz()
        //----------------------------------------------------------------------------------
        {
            if (null == _ConnectionString)
                return false;

            try
            {
                _Polaczenie = new OleDbConnection(_ConnectionString);
                _Polaczenie.Open();
            }
            catch(Exception)
            {
                return false;
            }

            return true;
        }

        //----------------------------------------------------------------------------------
        public void Rozlacz()
        //----------------------------------------------------------------------------------
        {
            _Polaczenie.Close();
        }

        //----------------------------------------------------------------------------------
        public void TworzConnectionString(string SciezkaDoBazy, string HasloDoBazy)
        //----------------------------------------------------------------------------------
        {
           // if (null == BazaDanychWrapper._ConnectionString)            
            BazaDanychWrapper._ConnectionString = String.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Jet OLEDB:Database Password={1};", SciezkaDoBazy, HasloDoBazy);
            
        }
        
        //----------------------------------------------------------------------------------
        public DataSet TworzZbiorDanych(string zapytanie)
        //----------------------------------------------------------------------------------
        {
            if (false == Polacz())
                return null;

            OleDbCommand polecenie = new OleDbCommand(zapytanie, _Polaczenie);              // Tworzymy polecenie dla bazy danych.
            OleDbDataAdapter adapter = new OleDbDataAdapter(polecenie);                     // Wyniki zostaną przekonwertowane do obiektu DataSet przez adapter

            DataSet dane = new DataSet();

            try
            {
                adapter.Fill(dane);                                                         // wypełnienie obiektu danymi z zapytania
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                Rozlacz();
            }

            return dane;
        }

        //----------------------------------------------------------------------------------
        public DataTable TworzTabeleDanych(string zapytanie)
        //----------------------------------------------------------------------------------
        {
            if (false == Polacz())
                return null;

            OleDbCommand polecenie = new OleDbCommand(zapytanie, _Polaczenie);              // Tworzymy polecenie dla bazy danych.
            OleDbDataAdapter adapter = new OleDbDataAdapter(polecenie);                     // Wyniki zostaną przekonwertowane do obiektu DataSet przez adapter

            DataTable dane = new DataTable();

            try
            {
                adapter.Fill(dane);                                                         // wypełnienie obiektu danymi z zapytania
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                Rozlacz();
            }

            return dane;
        }



        //----------------------------------------------------------------------------------
        public bool TworzTabeleDanychWPamieci(string zapytanie)
        //----------------------------------------------------------------------------------
        {
            if (false == Polacz())
                return false;

            OleDbCommand polecenie = new OleDbCommand(zapytanie, _Polaczenie);              // Tworzymy polecenie dla bazy danych.
            OleDbDataAdapter adapter = new OleDbDataAdapter(polecenie);                     // Wyniki zostaną przekonwertowane do obiektu DataSet przez adapter

            Tabela.Clear();

            try
            {
                adapter.Fill(Tabela);                                                     // wypełnienie obiektu danymi z zapytania
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                Rozlacz();
            }

            return true;
        }
    }
}
