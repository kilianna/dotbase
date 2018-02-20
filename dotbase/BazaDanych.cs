using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;
using System.Diagnostics;

namespace DotBase
{
    class BazaDanychWrapper
    {
        private static string _ConnectionString;
        private static OleDbConnection _Polaczenie = null;
        private static DateTime _OstatniaAktywnosc = DateTime.Now;

        public DataTable Tabela { get; private set; }


        //--------------------------------------------------------------------
        public BazaDanychWrapper()
        //--------------------------------------------------------------------
        {
            Tabela = new DataTable();
        }


        //----------------------------------------------------------------------------------
        public bool WykonajPolecenie(string zapytanie, params object[] list)
        //----------------------------------------------------------------------------------
        {
            if (false == Polacz())
                return false;

            OleDbCommand polecenie = UtworzPolecenie(zapytanie, list);              // Tworzymy polecenie dla bazy danych.

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
        public static bool Polacz()
        //----------------------------------------------------------------------------------
        {
            _OstatniaAktywnosc = DateTime.Now;

            if (_Polaczenie != null && _Polaczenie.State == ConnectionState.Open)
            {
                Debug.WriteLine("Already connected");
                return true;
            }

            if (null == _ConnectionString)
                return false;

            try
            {
                Debug.WriteLine("Connecting...");
                _Polaczenie = new OleDbConnection(_ConnectionString);
                _Polaczenie.Open();
                Debug.WriteLine("Connected");
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Exception: " + ex.ToString());
                return false;
            }

            return true;
        }

        //----------------------------------------------------------------------------------
        public static void Rozlacz()
        //----------------------------------------------------------------------------------
        {
            _OstatniaAktywnosc = DateTime.Now;
            LogowanieForm.AktywujLicznikRozlaczania();
            Debug.WriteLine("Release");
        }

        //----------------------------------------------------------------------------------
        public void TworzConnectionString(string SciezkaDoBazy, string HasloDoBazy)
        //----------------------------------------------------------------------------------
        {
           // if (null == BazaDanychWrapper._ConnectionString)            
            BazaDanychWrapper._ConnectionString = String.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Jet OLEDB:Database Password={1};", SciezkaDoBazy, HasloDoBazy);
            
        }
        
        //----------------------------------------------------------------------------------
        public DataSet TworzZbiorDanych(string zapytanie, params object[] list)
        //----------------------------------------------------------------------------------
        {
            if (false == Polacz())
                return null;

            OleDbCommand polecenie = UtworzPolecenie(zapytanie, list);                      // Tworzymy polecenie dla bazy danych.
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

#region Tablice

        private OleDbType[] oleDbTypes = 
        {
            //
            // Summary:
            //     Date data, stored as a double (DBTYPE_DATE). The whole portion is the number
            //     of days since December 30, 1899, and the fractional portion is a fraction
            //     of a day. This maps to System.DateTime.
            OleDbType.Date,
            //
            // Summary:
            //     A long null-terminated Unicode string value (System.Data.OleDb.OleDbParameter
            //     only). This maps to System.String.
            OleDbType.LongVarWChar,
            //
            // Summary:
            //     A null-terminated stream of Unicode characters (DBTYPE_WSTR). This maps to
            //     System.String.
            OleDbType.WChar,
            // Summary:
            //     No value (DBTYPE_EMPTY).
            OleDbType.Empty,
            //
            // Summary:
            //     A 16-bit signed integer (DBTYPE_I2). This maps to System.Int16.
            OleDbType.SmallInt,
            //
            // Summary:
            //     A 32-bit signed integer (DBTYPE_I4). This maps to System.Int32.
            OleDbType.Integer,
            //
            // Summary:
            //     A floating-point number within the range of -3.40E +38 through 3.40E +38
            //     (DBTYPE_R4). This maps to System.Single.
            OleDbType.Single,
            //
            // Summary:
            //     A floating-point number within the range of -1.79E +308 through 1.79E +308
            //     (DBTYPE_R8). This maps to System.Double.
            OleDbType.Double,
            //
            // Summary:
            //     A currency value ranging from -2 63 (or -922,337,203,685,477.5808) to 2 63
            //     -1 (or +922,337,203,685,477.5807) with an accuracy to a ten-thousandth of
            //     a currency unit (DBTYPE_CY). This maps to System.Decimal.
            OleDbType.Currency,
            //
            // Summary:
            //     A null-terminated character string of Unicode characters (DBTYPE_BSTR). This
            //     maps to System.String.
            OleDbType.BSTR,
            //
            // Summary:
            //     A pointer to an IDispatch interface (DBTYPE_IDISPATCH). This maps to System.Object.
            OleDbType.IDispatch,
            //
            // Summary:
            //     A 32-bit error code (DBTYPE_ERROR). This maps to System.Exception.
            OleDbType.Error,
            //
            // Summary:
            //     A Boolean value (DBTYPE_BOOL). This maps to System.Boolean.
            OleDbType.Boolean,
            //
            // Summary:
            //     A special data type that can contain numeric, string, binary, or date data,
            //     and also the special values Empty and Null (DBTYPE_VARIANT). This type is
            //     assumed if no other is specified. This maps to System.Object.
            OleDbType.Variant,
            //
            // Summary:
            //     A pointer to an IUnknown interface (DBTYPE_UNKNOWN). This maps to System.Object.
            OleDbType.IUnknown,
            //
            // Summary:
            //     A fixed precision and scale numeric value between -10 38 -1 and 10 38 -1
            //     (DBTYPE_DECIMAL). This maps to System.Decimal.
            OleDbType.Decimal,
            //
            // Summary:
            //     A 8-bit signed integer (DBTYPE_I1). This maps to System.SByte.
            OleDbType.TinyInt,
            //
            // Summary:
            //     A 8-bit unsigned integer (DBTYPE_UI1). This maps to System.Byte.
            OleDbType.UnsignedTinyInt,
            //
            // Summary:
            //     A 16-bit unsigned integer (DBTYPE_UI2). This maps to System.UInt16.
            OleDbType.UnsignedSmallInt,
            //
            // Summary:
            //     A 32-bit unsigned integer (DBTYPE_UI4). This maps to System.UInt32.
            OleDbType.UnsignedInt,
            //
            // Summary:
            //     A 64-bit signed integer (DBTYPE_I8). This maps to System.Int64.
            OleDbType.BigInt,
            //
            // Summary:
            //     A 64-bit unsigned integer (DBTYPE_UI8). This maps to System.UInt64.
            OleDbType.UnsignedBigInt,
            //
            // Summary:
            //     A 64-bit unsigned integer representing the number of 100-nanosecond intervals
            //     since January 1, 1601 (DBTYPE_FILETIME). This maps to System.DateTime.
            OleDbType.Filetime,
            //
            // Summary:
            //     A globally unique identifier (or GUID) (DBTYPE_GUID). This maps to System.Guid.
            OleDbType.Guid,
            //
            // Summary:
            //     A stream of binary data (DBTYPE_BYTES). This maps to an System.Array of type
            //     System.Byte.
            OleDbType.Binary,
            //
            // Summary:
            //     A character string (DBTYPE_STR). This maps to System.String.
            OleDbType.Char,
            //
            // Summary:
            //     An exact numeric value with a fixed precision and scale (DBTYPE_NUMERIC).
            //     This maps to System.Decimal.
            OleDbType.Numeric,
            //
            // Summary:
            //     Date data in the format yyyymmdd (DBTYPE_DBDATE). This maps to System.DateTime.
            OleDbType.DBDate,
            //
            // Summary:
            //     Time data in the format hhmmss (DBTYPE_DBTIME). This maps to System.TimeSpan.
            OleDbType.DBTime,
            //
            // Summary:
            //     Data and time data in the format yyyymmddhhmmss (DBTYPE_DBTIMESTAMP). This
            //     maps to System.DateTime.
            OleDbType.DBTimeStamp,
            //
            // Summary:
            //     An automation PROPVARIANT (DBTYPE_PROP_VARIANT). This maps to System.Object.
            OleDbType.PropVariant,
            //
            // Summary:
            //     A variable-length numeric value (System.Data.OleDb.OleDbParameter only).
            //     This maps to System.Decimal.
            OleDbType.VarNumeric,
            //
            // Summary:
            //     A variable-length stream of non-Unicode characters (System.Data.OleDb.OleDbParameter
            //     only). This maps to System.String.
            OleDbType.VarChar,
            //
            // Summary:
            //     A long string value (System.Data.OleDb.OleDbParameter only). This maps to
            //     System.String.
            OleDbType.LongVarChar,
            //
            // Summary:
            //     A variable-length, null-terminated stream of Unicode characters (System.Data.OleDb.OleDbParameter
            //     only). This maps to System.String.
            OleDbType.VarWChar,
            //
            // Summary:
            //     A variable-length stream of binary data (System.Data.OleDb.OleDbParameter
            //     only). This maps to an System.Array of type System.Byte.
            OleDbType.VarBinary,
            //
            // Summary:
            //     A long binary value (System.Data.OleDb.OleDbParameter only). This maps to
            //     an System.Array of type System.Byte.
            OleDbType.LongVarBinary,
        };

        private Type[] dotNetTypes = 
        {
            //
            // Summary:
            //     Date data, stored as a double (DBTYPE_DATE). The whole portion is the number
            //     of days since December 30, 1899, and the fractional portion is a fraction
            //     of a day. This maps to System.DateTime.
            // Date = 7,
            typeof(DateTime),
            //
            // Summary:
            //     A long null-terminated Unicode string value (System.Data.OleDb.OleDbParameter
            //     only). This maps to System.String.
            // LongVarWChar = 203,
            typeof(String),
            //
            // Summary:
            //     A null-terminated stream of Unicode characters (DBTYPE_WSTR). This maps to
            //     System.String.
            // WChar = 130,
            typeof(String),
            // Summary:
            //     No value (DBTYPE_EMPTY).
            // Empty = 0,
            null,
            //
            // Summary:
            //     A 16-bit signed integer (DBTYPE_I2). This maps to System.Int16.
            // SmallInt = 2,
            typeof(Int16),
            //
            // Summary:
            //     A 32-bit signed integer (DBTYPE_I4). This maps to System.Int32.
            // Integer = 3,
            typeof(Int32),
            //
            // Summary:
            //     A floating-point number within the range of -3.40E +38 through 3.40E +38
            //     (DBTYPE_R4). This maps to System.Single.
            // Single = 4,
            typeof(Single),
            //
            // Summary:
            //     A floating-point number within the range of -1.79E +308 through 1.79E +308
            //     (DBTYPE_R8). This maps to System.Double.
            // Double = 5,
            typeof(Double),
            //
            // Summary:
            //     A currency value ranging from -2 63 (or -922,337,203,685,477.5808) to 2 63
            //     -1 (or +922,337,203,685,477.5807) with an accuracy to a ten-thousandth of
            //     a currency unit (DBTYPE_CY). This maps to System.Decimal.
            // Currency = 6,
            typeof(Decimal),
            //
            // Summary:
            //     A null-terminated character string of Unicode characters (DBTYPE_BSTR). This
            //     maps to System.String.
            // BSTR = 8,
            typeof(String),
            //
            // Summary:
            //     A pointer to an IDispatch interface (DBTYPE_IDISPATCH). This maps to System.Object.
            // IDispatch = 9,
            null,
            //
            // Summary:
            //     A 32-bit error code (DBTYPE_ERROR). This maps to System.Exception.
            // Error = 10,
            null,
            //
            // Summary:
            //     A Boolean value (DBTYPE_BOOL). This maps to System.Boolean.
            // Boolean = 11,
            typeof(Boolean),
            //
            // Summary:
            //     A special data type that can contain numeric, string, binary, or date data,
            //     and also the special values Empty and Null (DBTYPE_VARIANT). This type is
            //     assumed if no other is specified. This maps to System.Object.
            // Variant = 12,
            null,
            //
            // Summary:
            //     A pointer to an IUnknown interface (DBTYPE_UNKNOWN). This maps to System.Object.
            // IUnknown = 13,
            null,
            //
            // Summary:
            //     A fixed precision and scale numeric value between -10 38 -1 and 10 38 -1
            //     (DBTYPE_DECIMAL). This maps to System.Decimal.
            // Decimal = 14,
            typeof(Decimal),
            //
            // Summary:
            //     A 8-bit signed integer (DBTYPE_I1). This maps to System.SByte.
            // TinyInt = 16,
            typeof(SByte),
            //
            // Summary:
            //     A 8-bit unsigned integer (DBTYPE_UI1). This maps to System.Byte.
            // UnsignedTinyInt = 17,
            typeof(Byte),
            //
            // Summary:
            //     A 16-bit unsigned integer (DBTYPE_UI2). This maps to System.UInt16.
            // UnsignedSmallInt = 18,
            typeof(UInt16),
            //
            // Summary:
            //     A 32-bit unsigned integer (DBTYPE_UI4). This maps to System.UInt32.
            // UnsignedInt = 19,
            typeof(UInt32),
            //
            // Summary:
            //     A 64-bit signed integer (DBTYPE_I8). This maps to System.Int64.
            // BigInt = 20,
            typeof(Int64),
            //
            // Summary:
            //     A 64-bit unsigned integer (DBTYPE_UI8). This maps to System.UInt64.
            // UnsignedBigInt = 21,
            typeof(UInt64),
            //
            // Summary:
            //     A 64-bit unsigned integer representing the number of 100-nanosecond intervals
            //     since January 1, 1601 (DBTYPE_FILETIME). This maps to System.DateTime.
            // Filetime = 64,
            typeof(DateTime),
            //
            // Summary:
            //     A globally unique identifier (or GUID) (DBTYPE_GUID). This maps to System.Guid.
            // Guid = 72,
            null,
            //
            // Summary:
            //     A stream of binary data (DBTYPE_BYTES). This maps to an System.Array of type
            //     System.Byte.
            // Binary = 128,
            typeof(byte[]),
            //
            // Summary:
            //     A character string (DBTYPE_STR). This maps to System.String.
            // Char = 129,
            typeof(String),
            //
            // Summary:
            //     An exact numeric value with a fixed precision and scale (DBTYPE_NUMERIC).
            //     This maps to System.Decimal.
            // Numeric = 131,
            typeof(Decimal),
            //
            // Summary:
            //     Date data in the format yyyymmdd (DBTYPE_DBDATE). This maps to System.DateTime.
            // DBDate = 133,
            typeof(DateTime),
            //
            // Summary:
            //     Time data in the format hhmmss (DBTYPE_DBTIME). This maps to System.TimeSpan.
            // DBTime = 134,
            typeof(TimeSpan),
            //
            // Summary:
            //     Data and time data in the format yyyymmddhhmmss (DBTYPE_DBTIMESTAMP). This
            //     maps to System.DateTime.
            // DBTimeStamp = 135,
            typeof(TimeSpan),
            //
            // Summary:
            //     An automation PROPVARIANT (DBTYPE_PROP_VARIANT). This maps to System.Object.
            // PropVariant = 138,
            null,
            //
            // Summary:
            //     A variable-length numeric value (System.Data.OleDb.OleDbParameter only).
            //     This maps to System.Decimal.
            // VarNumeric = 139,
            typeof(Decimal),
            //
            // Summary:
            //     A variable-length stream of non-Unicode characters (System.Data.OleDb.OleDbParameter
            //     only). This maps to System.String.
            // VarChar = 200,
            typeof(String),
            //
            // Summary:
            //     A long string value (System.Data.OleDb.OleDbParameter only). This maps to
            //     System.String.
            // LongVarChar = 201,
            typeof(String),
            //
            // Summary:
            //     A variable-length, null-terminated stream of Unicode characters (System.Data.OleDb.OleDbParameter
            //     only). This maps to System.String.
            // VarWChar = 202,
            typeof(String),
            //
            // Summary:
            //     A variable-length stream of binary data (System.Data.OleDb.OleDbParameter
            //     only). This maps to an System.Array of type System.Byte.
            // VarBinary = 204,
            typeof(byte[]),
            //
            // Summary:
            //     A long binary value (System.Data.OleDb.OleDbParameter only). This maps to
            //     an System.Array of type System.Byte.
            // LongVarBinary = 205,
            typeof(byte[]),
        };

#endregion

        private OleDbCommand UtworzPolecenie(string zapytanie, object[] list)
        {           
            OleDbCommand polecenie = new OleDbCommand(zapytanie, _Polaczenie);              // Tworzymy polecenie dla bazy danych.
            for (int i = 0; i < list.Length; i++)
            {
                var p = list[i];
                var pType = p.GetType();
                OleDbType type = OleDbType.Empty;
                for (int k = 0; k < oleDbTypes.Length; k++)
                {
                    if (dotNetTypes[k] != null && dotNetTypes[k].Equals(pType))
                    {
                        type = oleDbTypes[k];
                        break;
                    }
                }
                if (type == OleDbType.Empty)
                {
                    string text = "Nieznany typ: " + pType.ToString();
                    MessageBox.Show(text);
                    throw new ApplicationException(text);
                }
                OleDbParameter param;
                if (p is string)
                {
                    param = polecenie.Parameters.Add("a" + i, type, ((string)p).Length);
                }
                else if (p is byte[])
                {
                    param = polecenie.Parameters.Add("a" + i, type, ((byte[])p).Length);
                }
                else
                {
                    param = polecenie.Parameters.Add("a" + i, type);
                }
                param.Value = p;
                param.Direction = ParameterDirection.Input;
            }
            return polecenie;
        }

        //----------------------------------------------------------------------------------
        public DataTable TworzTabeleDanych(string zapytanie, params object[] list)
        //----------------------------------------------------------------------------------
        {
            if (false == Polacz())
                return null;

            OleDbCommand polecenie = UtworzPolecenie(zapytanie, list);                      // Tworzymy polecenie dla bazy danych.
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
        public bool TworzTabeleDanychWPamieci(string zapytanie, params object[] list)
        //----------------------------------------------------------------------------------
        {
            if (false == Polacz())
                return false;

            OleDbCommand polecenie = UtworzPolecenie(zapytanie, list);                      // Tworzymy polecenie dla bazy danych.
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


        public static bool Zakoncz(bool wymus)
        {
            if (_Polaczenie == null || _Polaczenie.State != ConnectionState.Open)
            {
                Debug.WriteLine("Already closed");
                return true;
            }

            if (!wymus && DateTime.Now < (_OstatniaAktywnosc + new TimeSpan(0, 0, 0, 0, 300)))
            {
                Debug.WriteLine("Not ready to close");
                return false;
            }

            Debug.WriteLine("Closing...");
            try
            {
                _Polaczenie.Close();
            }
            catch (Exception) { }
            _Polaczenie = null;
            Debug.WriteLine("Closed");
            return true;
        }
    }
}
