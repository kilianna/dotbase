using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;
using System.Dynamic;
using System.Collections.Specialized;

namespace DotBase
{
    class Order {
        public string keyword;
        public Order(string keyword)
        {
            this.keyword = keyword;
        }
        public static Order ASC = new Order("");
        public static Order DESC = new Order(" DESC");
    }

    partial class BazaDanychWrapper
    {
        private static string _ConnectionString;
        private static OleDbConnection _Polaczenie = null;
        private static DateTime _OstatniaAktywnosc = DateTime.Now;

        private static string _SciezkaLog;
        private static string _ConnectionStringLog;
        private static OleDbConnection _PolaczenieLog = null;

        //--------------------------------------------------------------------
        public BazaDanychWrapper()
        //--------------------------------------------------------------------
        {
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
                _PolaczenieLog = new OleDbConnection(_ConnectionStringLog);
                _PolaczenieLog.Open();
                Debug.WriteLine("Connected");
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Exception: " + ex.ToString());
                LastException = ex;
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
            BazaDanychWrapper._ConnectionString = String.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source='{0}';Jet OLEDB:Database Password={1};", SciezkaDoBazy, HasloDoBazy);
            _SciezkaLog = SciezkaDoLogu(SciezkaDoBazy);
            if (!File.Exists(_SciezkaLog))
            {
                try
                {
                    UtworzPustyLog(_SciezkaLog, HasloDoBazy);
                }
                catch (Exception)
                {
                    try
                    {
                        File.Delete(_SciezkaLog);
                    }
                    catch (Exception) { }
                    throw;
                }
            }
            BazaDanychWrapper._ConnectionStringLog = String.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source='{0}';Jet OLEDB:Database Password={1};", _SciezkaLog, HasloDoBazy);
        }

        private void UtworzPustyLog(string logPath, string HasloDoBazy)
        {
            byte[] gz = Properties.Resources.logTemplate;
            byte[] buffer = new byte[65536];
            GZipStream stream = new GZipStream(new MemoryStream(gz), CompressionMode.Decompress);
            FileStream output = new FileStream(logPath, FileMode.CreateNew);
            do
            {
                int n = stream.Read(buffer, 0, buffer.Length);
                if (n <= 0) break;
                output.Write(buffer, 0, n);
            } while (true);
            stream.Close();
            output.Close();
            OleDbConnection connection = new OleDbConnection(String.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source='{0}';Mode=Share Exclusive", logPath));
            connection.Open();
            OleDbCommand polecenie = new OleDbCommand("ALTER DATABASE PASSWORD `" + HasloDoBazy + "` ``", connection);
            polecenie.ExecuteNonQuery();
            connection.Close();
        }

        private string SciezkaDoLogu(string sciezkaDoBazy)
        {
            int pos = sciezkaDoBazy.LastIndexOf('.');
            string date = DateTime.Now.ToString("yyyy-MM");
            if (pos < 0) return sciezkaDoBazy + "." + date + ".log.accdb";
            return sciezkaDoBazy.Substring(0, pos) + "." + date + ".log" + sciezkaDoBazy.Substring(pos);
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

        public static Exception LastException { get; set; }

#endregion

        public OleDbCommand UtworzProstePolecenie(string zapytanie)
        {
            if (!Polacz())
                throw new ApplicationException("Cannot connect to database");
            return new OleDbCommand(zapytanie, _Polaczenie);
        }

        private OleDbCommand UtworzPolecenie(string zapytanie, object[] list)
        {
            OleDbCommand polecenie = new OleDbCommand(zapytanie, _Polaczenie);
            OleDbCommand selectPolecenie = null;
            Log.Wpis wpis = null;
            bool dodajDoLogu = true;
            StringBuilder sb = null;

            for (int i = 0; i < list.Length; i++)
            {
                if (list[i] is Log.Wpis)
                {
                    wpis = (Log.Wpis)list[i];
                    continue;
                }
            }

            if (wpis != null) dodajDoLogu = wpis.dodaj;

            if (dodajDoLogu)
            {
                if (zapytanie.Trim().ToLower().StartsWith("select "))
                {
                    dodajDoLogu = false;
                }
                else if (zapytanie.Trim().ToLower().StartsWith("update "))
                {
                    string select;
                    if (wpis != null && wpis.zapytanieSelect != null && wpis.zapytanieSelect != "")
                    {
                        select = wpis.zapytanieSelect;
                    }
                    else
                    {
                        select = Regex.Replace(zapytanie, "UPDATE", "SELECT COUNT(*) FROM", RegexOptions.IgnoreCase);
                        select = Regex.Replace(select, "WHERE", ") AND (", RegexOptions.IgnoreCase);
                        select = Regex.Replace(select, ", ", ") AND (", RegexOptions.IgnoreCase);
                        select = Regex.Replace(select, "SET", "WHERE (", RegexOptions.IgnoreCase);
                        select = select + ")";
                    }
                    try
                    {
                        selectPolecenie = new OleDbCommand(select, _Polaczenie);
                    }
                    catch (Exception ex)
                    {
                        Log.log(this, "Błąd tworzenia polecenia SELECT sprawdzającego zmiany w poleceniu UPDATE: " + ex.Message, select);
                    }
                }
                
            }

            if (dodajDoLogu)
            {
                sb = new StringBuilder(zapytanie.Length + 24 * list.Length);
                sb.Append(zapytanie);
            }

            for (int i = 0; i < list.Length; i++)
            {
                var p = list[i];
                if (p is Log.Wpis) continue;
                Type pType = p.GetType();
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
                    MyMessageBox.Show(text);
                    throw new ApplicationException(text);
                }
                OleDbParameter param = null;
                OleDbParameter selectParam = null;
                if (p is string)
                {
                    param = polecenie.Parameters.Add("a" + i, type, ((string)p).Length);
                    if (selectPolecenie != null) selectParam = selectPolecenie.Parameters.Add("a" + i, type, ((string)p).Length);
                    if (dodajDoLogu) sb.AppendFormat(" ||| \"{0}\"", (string)p);
                }
                else if (p is byte[])
                {
                    param = polecenie.Parameters.Add("a" + i, type, ((byte[])p).Length);
                    if (selectPolecenie != null) selectParam = selectPolecenie.Parameters.Add("a" + i, type, ((byte[])p).Length);
                    if (dodajDoLogu) sb.AppendFormat(" ||| [{0}]", BitConverter.ToString((byte[])p));
                }
                else
                {
                    param = polecenie.Parameters.Add("a" + i, type);
                    if (selectPolecenie != null) selectParam = selectPolecenie.Parameters.Add("a" + i, type);
                    if (dodajDoLogu) sb.AppendFormat(" ||| {0} ({1})", p.ToString(), pType.Name);
                }
                param.Value = p;
                param.Direction = ParameterDirection.Input;
                if (selectPolecenie != null)
                {
                    selectParam.Value = p;
                    selectParam.Direction = ParameterDirection.Input;
                }
            }

            if (dodajDoLogu && selectPolecenie != null)
            {
                try
                {
                    object n = selectPolecenie.ExecuteScalar();
                    dodajDoLogu = (Int64.Parse(n.ToString()) == 0L);
                }
                catch (Exception ex)
                {
                    Log.log(this, "Błąd wykonywania polecenia SELECT sprawdzającego zmiany w poleceniu UPDATE: " + ex.Message, selectPolecenie.CommandText);
                }
            }

            if (dodajDoLogu)
            {
                Log.log(this, (wpis != null && wpis.wiadomosc != null) ? wpis.wiadomosc : "", sb.ToString());
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

        public static bool Zakoncz(bool wymus)
        {
            if (_Polaczenie == null || _Polaczenie.State != ConnectionState.Open)
            {
                Debug.WriteLine("Already closed");
                return true;
            }

            if (!wymus && DateTime.Now < (_OstatniaAktywnosc + new TimeSpan(0, 0, 0, 0, 1000)))
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
            try
            {
                _PolaczenieLog.Close();
            }
            catch (Exception) { }
            _PolaczenieLog = null;
            Debug.WriteLine("Closed");
            return true;
        }

        internal static bool ZmienHaslo(string SciezkaDoBazy, string noweHaslo, string stareHaslo)
        {
            try
            {
                Zakoncz(true);

                _Polaczenie = new OleDbConnection(String.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source='{0}';Jet OLEDB:Database Password={1};Mode=Share Exclusive", SciezkaDoBazy, stareHaslo));
                _Polaczenie.Open();
                var baza = new BazaDanychWrapper();
                OleDbCommand polecenie = baza.UtworzPolecenie("ALTER DATABASE PASSWORD [" + noweHaslo + "] [" + stareHaslo + "]", new object[0]);
                polecenie.ExecuteNonQuery();

                _PolaczenieLog = new OleDbConnection(String.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source='{0}';Jet OLEDB:Database Password={1};Mode=Share Exclusive", _SciezkaLog, stareHaslo));
                _PolaczenieLog.Open();
                polecenie = new OleDbCommand("ALTER DATABASE PASSWORD [" + noweHaslo + "] [" + stareHaslo + "]", _PolaczenieLog);
                polecenie.ExecuteNonQuery();
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                Zakoncz(true);
            }

            return true;
        }

        public delegate void TransformujBazeDelegate(BazaDanychWrapper baza);

        public static bool Eksportuj(string staraSciezka, string stareHaslo, string nowaSciezka, string noweHaslo, TransformujBazeDelegate transformacja)
        {
            bool usunGdyNiepowodzenie = false;
            try
            {
                Zakoncz(true);
                File.Copy(staraSciezka, nowaSciezka, true);
                usunGdyNiepowodzenie = true;
                _Polaczenie = new OleDbConnection(String.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source='{0}';Jet OLEDB:Database Password={1};Mode=Share Exclusive", nowaSciezka, stareHaslo));
                _Polaczenie.Open();
                var baza = new BazaDanychWrapper();
                OleDbCommand polecenie = baza.UtworzPolecenie("ALTER DATABASE PASSWORD [" + noweHaslo + "] [" + stareHaslo + "]", new object[0]);
                polecenie.ExecuteNonQuery();
                if (transformacja != null)
                {
                    transformacja(baza);
                }
                Zakoncz(true);
            }
            catch (Exception)
            {
                if (usunGdyNiepowodzenie)
                {
                    try
                    {
                        File.Delete(nowaSciezka);
                    }
                    catch (Exception) { }
                }
                return false;
            }
            finally
            {
                Zakoncz(true);
            }

            return true;
        }

        public static Dictionary<string, int> stackTraceCache = new Dictionary<string, int>();

        public void log(DateTime now, string user, string wiadomosc, string zapytanie, string stackTrace, string dodatkowe)
        {
            OleDbCommand polecenie;
            int programId = 0;
            if (stackTraceCache.ContainsKey(stackTrace))
            {
                programId = stackTraceCache[stackTrace];
            }
            else
            {
                polecenie = new OleDbCommand("SELECT Id FROM Program WHERE StackTrace=?", _PolaczenieLog);
                addParameter(polecenie, stackTrace);
                object id = polecenie.ExecuteScalar();
                if (id == null)
                {
                    polecenie = new OleDbCommand("INSERT INTO Program (StackTrace) VALUES (?)", _PolaczenieLog);
                    addParameter(polecenie, stackTrace);
                    polecenie.ExecuteNonQuery();
                    polecenie = new OleDbCommand("SELECT @@Identity", _PolaczenieLog);
                    id = polecenie.ExecuteScalar();
                }
                if (id == null)
                {
                    id = 0;
                }
                programId = Int32.Parse(id.ToString());
                stackTraceCache[stackTrace] = programId;
            }
            polecenie = new OleDbCommand("INSERT INTO Historia (Czas, Kto, Opis, Zapytanie, Dodatkowe, Program) VALUES (?,?,?,?,?,?)", _PolaczenieLog);
            addParameter(polecenie, now);
            addParameter(polecenie, user);
            addParameter(polecenie, wiadomosc);
            addParameter(polecenie, zapytanie);
            addParameter(polecenie, dodatkowe);
            addParameter(polecenie, programId);
            polecenie.ExecuteNonQuery();
        }

        private void addParameter(OleDbCommand polecenie, object p)
        {
            Type pType = p.GetType();
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
                MyMessageBox.Show(text);
                throw new ApplicationException(text);
            }
            OleDbParameter param = null;
            if (p is string)
            {
                param = polecenie.Parameters.Add("a" + polecenie.Parameters.Count, type, ((string)p).Length);
            }
            else if (p is byte[])
            {
                param = polecenie.Parameters.Add("a" + polecenie.Parameters.Count, type, ((byte[])p).Length);
            }
            else
            {
                param = polecenie.Parameters.Add("a" + polecenie.Parameters.Count, type);
            }
            param.Value = p;
            param.Direction = ParameterDirection.Input;
        }

#if DEBUG

        private static string F(int addIndent, params object[] args)
        {
            var result = new StringBuilder();
            var addIndentStr = new String(' ', 4 * addIndent);    
            string format = args[args.Length - 1] as string;
            args = args.Take(args.Length - 1).ToArray();
            var lines = format.Split('\n');
            if (lines.Length == 1)
            {
                result.Append(addIndentStr);
                result.AppendLine(lines[0].Trim());
                return String.Format(result.ToString(), args);
            }
            if (lines.Length <= 1 || lines[0].Trim() != "")
            {
                throw new ApplicationException("First line must be empty.");
            }
            lines = lines.Skip(1).ToArray();
            var minIndent = 999999999;
            foreach (var line in lines)
            {
                if (line.Trim() == "") continue;
                var indent = line.Length - line.TrimStart().Length;
                minIndent = Math.Min(minIndent, indent);
            }        
            foreach (var lineRaw in lines)
            {
                if (lineRaw.Trim() == "")
                {
                    result.AppendLine();
                }
                else
                {
                    var line = lineRaw.TrimEnd('\r').Substring(minIndent);
                    result.Append(addIndentStr);
                    result.AppendLine(line);
                }
            }
            return String.Format(result.ToString(), args);
        }

        internal void TworzSzablon()
        {
            string tabele = "";
            string szablon = F(0, @"
                using System;
                using System.Data;
                using System.Data.OleDb;
                using System.Collections.Generic;

                namespace DotBase
                {{
                    partial class Szablon
                    {{");
            DataTable table = _Polaczenie.GetSchema("tables");
            foreach (DataRow row in table.Rows)
            {
                string tabela = row.Field<string>("TABLE_NAME");
                string typTabeli = row.Field<string>("TABLE_TYPE");
                if (tabela.StartsWith("~") || tabela.StartsWith("MSys") || typTabeli != "TABLE") continue;
                string tabelaId = tabela.Replace(' ', '_');
                szablon += F(2, tabelaId, @"
                    public class Szablon_{0} : Tabela
                    {{
                        public Szablon_{0}(BazaDanychWrapper baza, string nazwa) : base(baza, nazwa) {{ }}
                        public Szablon_{0} UPDATE() {{ _UPDATE(); return this; }}
                        public Szablon_{0} INSERT() {{ _INSERT(); return this; }}
                        public Szablon_{0} DELETE() {{ _DELETE(); return this; }}
                        public Szablon_{0} WHERE() {{ _WHERE(); return this; }}
                        public Szablon_{0} INFO(string text) {{ _INFO(text); return this; }}
                        public Szablon_{0} SELECT() {{ _SELECT(); return this; }}
                        public Szablon_{0} ORDER_BY() {{ _ORDER_BY(); return this; }}
                        public Row_{0}[] GET(int min = 0, int max = 999999999, bool allowException = false) {{ return Row_{0}._GET(_GET(min, max, allowException)); }}
                        public Row_{0} GET_ONE() {{ return Row_{0}._GET(_GET(1, 1))[0]; }}
                        public Row_{0} GET_OPTIONAL() {{ var r = Row_{0}._GET(_GET(0, 1)); return r.Length > 0 ? r[0] : null; }}");
                tabele += F(2, tabelaId, tabela, @"
                        public Szablon.Szablon_{0} {0} {{ get {{ return new Szablon.Szablon_{0}(this, ""{1}""); }} }}");
                var wiersz = F(2, tabelaId, @"
                    public class Row_{0} : Wiersz
                    {{");
                var wierszConstr = "";
                DataTable schemaTable = _Polaczenie.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Columns, new object[] { null, null, tabela, null });
                foreach (DataRow col in schemaTable.Rows)
                {
                    int oleDbTypeNumber = col.Field<int>(11);
                    OleDbType oleDbType = (OleDbType)oleDbTypeNumber;
                    string colName = col.Field<string>(3);
                    var colIsNullable = col.Field<bool>(10);
                    var csTuple = oleDbToNetTypeConverter(oleDbTypeNumber);
                    var csType = csTuple.Item1;
                    var csIsNullable = csTuple.Item2;
                    if (colIsNullable && !csIsNullable)
                        csType += "?";
                    szablon += F(3, tabelaId, colName, csType, oleDbType.ToString(), @"
                        public Szablon_{0} {1}({2} value)
                        {{
                            SetField(""{1}"", value, OleDbType.{3});
                            return this;
                        }}
                        public Szablon_{0} {1}(Order order)
                        {{
                            SetOrder(""{1}"", order);
                            return this;
                        }}
                        public Szablon_{0} {1}()
                        {{
                            AddField(""{1}"");
                            return this;
                        }}"
                        /*public Szablon_{0} {1}(Tabela subquery) // Jeżeli będzie potrzeba obsłużyć podzapytania
                        {{
                            SetFieldSubquery(""{1}"", subquery);
                            return this;
                        }}
                        "*/);
                    wiersz += F(3, tabelaId, colName, csType, oleDbType.ToString(), @"
                        public {2} {1};");
                    wierszConstr += F(4, tabelaId, colName, csType, oleDbType.ToString(), @"
                        if (cols.ContainsKey(""{1}""))
                            {1} = row.Field<{2}>(cols[""{1}""]);");
                }
                wiersz += F(2, tabelaId, "\r\n" + wierszConstr, @"
                        public Row_{0}(DataRow row)
                        {{
                            _init(row, GetColsDict(row));
                        }}
                        public Row_{0}(DataRow row, Dictionary<string, int> cols)
                        {{
                            _init(row, cols);
                        }}
                        private void _init(DataRow row, Dictionary<string, int> cols)
                        {{{1}            }}
                        internal static Row_{0}[] _GET(DataTable dane)
                        {{
                            var result = new Row_{0}[dane.Rows.Count];
                            var cols = GetColsDict(dane);
                            var index = 0;
                            foreach (DataRow row in dane.Rows)
                                result[index++] = new Row_{0}(row, cols);
                            return result;
                        }}
                    }}");
                szablon += F(2, "}}");
                szablon += wiersz;
            }
            szablon += F(0, "\r\n" + tabele, @"
                    }}
    
                    partial class BazaDanychWrapper
                    {{{0}    }}
                }}");
            string path = (new Uri(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase) + @"\..\..\Szablon.Generated.cs")).LocalPath;
            bool write = true;
            try
            {
                string old = File.ReadAllText(path);
                write = (old != szablon);
            }
            catch (Exception) { };
            if (write)
            {
                File.WriteAllText(path, szablon);
            }

            StringBuilder sb = new StringBuilder();
            foreach (DataRow row in table.Rows)
            {
                string tabela = row.Field<string>("TABLE_NAME");
                string typTabeli = row.Field<string>("TABLE_TYPE");
                if (tabela.StartsWith("~") || tabela.StartsWith("MSys") || typTabeli != "TABLE") continue;
                sb.AppendLine("");
                sb.AppendLine("---------------------------------------------------------------------");
                sb.AppendLine(String.Format(" {0}", tabela));
                sb.AppendLine("---------------------------------------------------------------------");
                DataTable schemaTable = _Polaczenie.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Columns,new object[] { null, null, tabela, null });
                List<string> strRows = new List<string>();
                foreach (DataRow row2 in schemaTable.Rows)
                {
                    StringBuilder sb2 = new StringBuilder();
                    var type = (OleDbType)row2.Field<int>("DATA_TYPE");
                    var pos = row2.Field<long>("ORDINAL_POSITION");
                    sb2.AppendFormat("    {0}.{1}, typ {2}",
                        row2.Field<string>("TABLE_NAME"),
                        row2.Field<string>("COLUMN_NAME"),
                        type.ToString());
                    if (row2.Field<bool>("COLUMN_HASDEFAULT"))
                    {
                        prostePoleSzablonu(row2, sb2, "domyślnie", "COLUMN_DEFAULT");
                    }
                    if (type != OleDbType.Boolean)
                    {
                        prostePoleSzablonu(row2, sb2, "długość", "CHARACTER_MAXIMUM_LENGTH");
                    }
                    prostePoleSzablonu(row2, sb2, "precyzja", "NUMERIC_PRECISION");
                    prostePoleSzablonu(row2, sb2, "precyzja", "DATETIME_PRECISION");
                    prostePoleSzablonu(row2, sb2, "skala", "NUMERIC_SCALE");
                    prostePoleSzablonu(row2, sb2, "niewymagane", "IS_NULLABLE");
                    prostePoleSzablonu(row2, sb2, "pozycja", "ORDINAL_POSITION");
                    sb2.AppendFormat(", flagi {0}", flagiSzablonu(row2.Field<long>("COLUMN_FLAGS")));
                    prostePoleSzablonu(row2, sb2, "opis", "DESCRIPTION");
                    while (strRows.Count <= pos)
                    {
                        strRows.Add("");
                    }
                    strRows[(int)pos] = sb2.ToString();
                }
                sb.AppendLine(string.Join("\r\n", strRows));
            }
            szablon = sb.ToString();

            path = (new Uri(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase) + @"\..\..\SzablonBazy.txt")).LocalPath;
            write = true;
            try
            {
                string old = File.ReadAllText(path);
                write = (old != szablon);
            }
            catch (Exception) { };
            if (write)
            {
                File.WriteAllText(path, szablon);
            }

        }

        private string flagiSzablonu(long p)
        {
            string result = "";
            do
            {
                var x = Convert.ToString((p & 0xF) | 0x10, 2).Substring(1).Replace('0', '-').Replace('1', 'X');
                result += " " + x;
                p >>= 4;
            } while (p != 0);
            return result.Substring(1);
        }

        private void prostePoleSzablonu(DataRow row2, StringBuilder sb, string format, string name)
        {
            var value = row2.Field<object>(name);
            var fmt = ", " + format + " {0}";
            if (value == null)
            {
                return;
            }
            if (value is bool)
            {
                sb.AppendFormat(fmt, (bool)value ? "Tak" : "Nie");
            }
            else
            {
                sb.AppendFormat(fmt, value.ToString());
            }
        }

#endif

        private Tuple<string, bool> oleDbToNetTypeConverter(int oleDbTypeNumber)
        {
            switch (oleDbTypeNumber)
            {
                case 0: return Tuple.Create("object", true);
                case 2: return Tuple.Create("short", false);
                case 3: return Tuple.Create("int", false);
                case 4: return Tuple.Create("float", false);
                case 5: return Tuple.Create("double", false);
                case 6: return Tuple.Create("decimal", false);
                case 7: return Tuple.Create("DateTime", false);
                case 8: return Tuple.Create("string", true);
                case 9: return Tuple.Create("object", true);
                case 10: return Tuple.Create("Exception", true);
                case 11: return Tuple.Create("bool", false);
                case 12: return Tuple.Create("object", true);
                case 13: return Tuple.Create("object", true);
                case 14: return Tuple.Create("decimal", false);
                case 16: return Tuple.Create("sbyte", false);
                case 17: return Tuple.Create("byte", false);
                case 18: return Tuple.Create("ushort", false);
                case 19: return Tuple.Create("uint", false);
                case 20: return Tuple.Create("long", false);
                case 21: return Tuple.Create("ulong", false);
                case 64: return Tuple.Create("DateTime", false);
                case 72: return Tuple.Create("Guid", false);
                case 128: return Tuple.Create("byte[]", true);
                case 129: return Tuple.Create("string", true);
                case 130: return Tuple.Create("string", true);
                case 131: return Tuple.Create("decimal", false);
                case 133: return Tuple.Create("DateTime", false);
                case 134: return Tuple.Create("TimeSpan", false);
                case 135: return Tuple.Create("DateTime", false);
                case 138: return Tuple.Create("object", true);
                case 139: return Tuple.Create("decimal", false);
                case 200: return Tuple.Create("string", true);
                case 201: return Tuple.Create("string", true);
                case 202: return Tuple.Create("string", true);
                case 203: return Tuple.Create("string", true);
                case 204: return Tuple.Create("byte[]", true);
                case 205: return Tuple.Create("byte[]", true);
            }
            throw (new Exception("DataType Not Supported"));
        }

        internal DataTable TworzTabeleDanych(Szablon.Szablon_Sondy zapytanie1)
        {
            throw new NotImplementedException();
        }

        internal string LikeEscape(string text)
        {
            return text.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]");
        }
    }
}
