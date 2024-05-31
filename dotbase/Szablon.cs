using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;
using System.Windows.Forms;

namespace DotBase
{
    partial class Szablon
    {

        private enum TypPolecenia
        {
            NONE,
            UPDATE,
            INSERT,
            DELETE,
            SELECT,
        }

        private struct ValueWithOleDbType
        {
            public object value;
            public OleDbType oleDbType;
            public ValueWithOleDbType(object value, OleDbType oleDbType)
            {
                this.value = value;
                this.oleDbType = oleDbType;
            }
        }

        public class Tabela
        {
            private BazaDanychWrapper baza;
            private string nazwa;
            private TypPolecenia typPolecenia = TypPolecenia.NONE;
            private bool where = false;
            private string orderBy = null;
            private List<ValueWithOleDbType> parameters = new List<ValueWithOleDbType>();
            private string query = "";
            private string selectQuery = "";
            private string values = "";
            private string logParameters = "";
            private string logInfo = "";
            private bool firstEntry = true;
            private int affectedRows;
            private long lastId;
            private bool lastIdValid = false;

            public Tabela(BazaDanychWrapper baza, string nazwa)
            {
                this.baza = baza;
                this.nazwa = nazwa;
            }

            protected void SetField(string name, object value, OleDbType oleDbType)
            {
                if (where)
                {
                    if (!firstEntry)
                    {
                        query += " AND ";
                        selectQuery += " AND ";
                        logParameters += ",\r\n";
                    }
                    firstEntry = false;
                    query += "(" + name + "=?)";
                    selectQuery += "(" + name + "=?)";
                    logParameters += "WHERE " + name + "=" + toLogString(value);
                    parameters.Add(new ValueWithOleDbType(value, oleDbType));
                }
                else if (typPolecenia == TypPolecenia.UPDATE)
                {
                    if (!firstEntry)
                    {
                        query += ", ";
                        selectQuery += " AND ";
                        logParameters += ",\r\n";
                    }
                    firstEntry = false;
                    query += name + "=?";
                    selectQuery += "(" + name + "<>?)";
                    logParameters += "SET " + name + "=" + toLogString(value);
                    parameters.Add(new ValueWithOleDbType(value, oleDbType));
                }
                else if (typPolecenia == TypPolecenia.INSERT)
                {
                    if (!firstEntry)
                    {
                        query += ", ";
                        values += ", ";
                        logParameters += ",\r\n";
                    }
                    firstEntry = false;
                    query += name;
                    values += "?";
                    logParameters += "SET " + name + "=" + toLogString(value);
                    parameters.Add(new ValueWithOleDbType(value, oleDbType));
                }
                else
                {
                    throw new ApplicationException("Nieprawidłowa składnia polecenia!");
                }
            }

            private string toLogString(object value)
            {
                if (value is string)
                {
                    return "\"" + (value as string).Replace("\\", "\\\\").Replace("\"", "\\\"") + "\"";
                }
                return value.ToString();
            }

            protected void AddField(string name)
            {
                if (orderBy != null)
                {
                    if (orderBy.Length > 0) orderBy += ", ";
                    orderBy += name;
                }
                else if (!where && typPolecenia == TypPolecenia.SELECT)
                {
                    if (!firstEntry)
                    {
                        query += ", ";
                    }
                    firstEntry = false;
                    query += name;
                }
                else
                {
                    throw new ApplicationException("Nieprawidłowa składnia polecenia!");
                }
            }

            protected void SetOrder(string name, Order order)
            {
                if (orderBy == null)
                {
                    throw new ApplicationException("Nieprawidłowa składnia polecenia!");
                }
                if (orderBy.Length > 0) orderBy += ", ";
                orderBy += name + order.keyword;
            }

            protected void SetFieldSubquery(string name, Tabela subquery)
            {
                throw new NotImplementedException();
            }


            protected void _UPDATE()
            {
                if (typPolecenia != TypPolecenia.NONE)
                    throw new ApplicationException("Nieprawidłowa składnia polecenia!");
                typPolecenia = TypPolecenia.UPDATE;
                query = "UPDATE " + nazwa + " SET ";
                selectQuery = "SELECT COUNT(*) FROM " + nazwa + " WHERE ";
                firstEntry = true;
            }

            protected void _INSERT()
            {
                if (typPolecenia != TypPolecenia.NONE)
                    throw new ApplicationException("Nieprawidłowa składnia polecenia!");
                typPolecenia = TypPolecenia.INSERT;
                query = "INSERT INTO " + nazwa + "(";
                firstEntry = true;
            }

            protected void _DELETE()
            {
                if (typPolecenia != TypPolecenia.NONE)
                    throw new ApplicationException("Nieprawidłowa składnia polecenia!");
                typPolecenia = TypPolecenia.DELETE;
                query = "DELETE FROM " + nazwa;
            }

            protected void _SELECT()
            {
                if (typPolecenia != TypPolecenia.NONE)
                    throw new ApplicationException("Nieprawidłowa składnia polecenia!");
                typPolecenia = TypPolecenia.SELECT;
                query = "SELECT ";
            }

            protected void _ORDER_BY()
            {
                if (typPolecenia != TypPolecenia.NONE && typPolecenia != TypPolecenia.SELECT)
                    throw new ApplicationException("Nieprawidłowa składnia polecenia!");
                orderBy = "";
            }

            protected void _WHERE()
            {
                if (typPolecenia == TypPolecenia.INSERT || where)
                    throw new ApplicationException("Nieprawidłowa składnia polecenia!");

                if (typPolecenia == TypPolecenia.NONE)
                {
                    typPolecenia = TypPolecenia.SELECT;
                    query = "SELECT ";
                }

                if (typPolecenia == TypPolecenia.SELECT)
                {
                    if (firstEntry)
                        query += "*";
                    query += " FROM " + nazwa;
                }
                query += " WHERE ";
                selectQuery += " AND ";
                logParameters += ",\r\n";
                where = true;
                firstEntry = true;
            }

            public bool EXECUTE(bool returnResult = false)
            {
                OleDbCommand cmd;
                CloseQuery();
                bool skipLog = false;

                if (typPolecenia == TypPolecenia.SELECT)
                {
                    throw new ApplicationException("Nieprawidłowa składnia polecenia!");
                }

                if (typPolecenia == TypPolecenia.UPDATE)
                {
                    try
                    {
                        cmd = baza.UtworzProstePolecenie(selectQuery);
                        addParameters(cmd);
                        int count = (int)cmd.ExecuteScalar();
                        skipLog = (count == 0);
                    }
                    catch (Exception ex)
                    {
                        Log.log(baza, "Błąd wykonywania polecenia SELECT sprawdzającego zmiany w poleceniu UPDATE: " + ex.Message, selectQuery, logParameters);
                    }
                }

                try
                {
                    cmd = baza.UtworzProstePolecenie(query);
                    addParameters(cmd);
                    affectedRows = cmd.ExecuteNonQuery();

                    if (typPolecenia == TypPolecenia.INSERT)
                    {
                        try
                        {
                            cmd = baza.UtworzProstePolecenie("SELECT @@Identity");
                            lastId = (long)cmd.ExecuteScalar();
                            lastIdValid = true;
                        }
                        catch (Exception)
                        {
                            lastId = -1;
                            lastIdValid = false;
                        }
                    }

                    if (typPolecenia == TypPolecenia.DELETE)
                    {
                        skipLog = (affectedRows == 0);
                    }

                    if (!skipLog)
                    {
                        if (typPolecenia == TypPolecenia.INSERT)
                        {
                            Log.log(baza, logInfo, query, String.Format("{0},\r\n[AFFECTED ROWS]={1},\r\n[LAST ID]={2}", logParameters, affectedRows, lastIdValid ? lastId.ToString() : "[invalid]"));
                        }
                        else
                        {
                            Log.log(baza, logInfo, query, String.Format("{0},\r\n[AFFECTED ROWS]={1}", logParameters, affectedRows));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.log(baza, "Błąd wykonywania polecenia: " + ex.Message, query, logParameters);
                    if (returnResult)
                    {
                        return false;
                    }
                    else
                    {
                        throw;
                    }
                }

                return true;
            }

            protected DataTable _GET(int min = 0, int max = 999999999, bool allowException = false)
            {
                if (typPolecenia == TypPolecenia.NONE)
                {
                    typPolecenia = TypPolecenia.SELECT;
                    query = "SELECT * FROM " + nazwa;
                }
                else if (typPolecenia != TypPolecenia.SELECT)
                {
                    throw new ApplicationException("Nieprawidłowa składnia polecenia!");
                }
                if (orderBy != null && orderBy.Length > 0)
                {
                    query += " ORDER BY " + orderBy;
                }
                DataTable dane = new DataTable();
                var cmd = baza.UtworzProstePolecenie(query);
                try
                {
                    addParameters(cmd);
                    OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
                    adapter.Fill(dane);
                }
                catch (Exception ex)
                {
                    if (!allowException)
                    {
                        string message = String.Format("W wyniku działa kwerendy otrzymano niespodziewany wyjątek: {0}\r\n" +
                                "Kwerenda SQL:\r\n" +
                                "{1}{2}\r\n" +
                                "Miejsce wystąpienia:\r\n" +
                                "{3}", ex.Message, query, logParameters, ex.StackTrace);
                        MessageBox.Show(message, "Niespodziewany wyjątek", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    throw;
                }
                finally
                {
                    BazaDanychWrapper.Rozlacz();
                }
                if (dane.Rows.Count < min || dane.Rows.Count > max)
                {
                    string message = (min == max) ?
                        String.Format("W wyniku kwerendy spodziewano się dokładnie {0} rekordów, otrzymano {1}.\r\n" +
                            "Kwerenda SQL:\r\n" +
                            "{2}{3}", min, dane.Rows.Count, query, logParameters) :
                        String.Format("W wyniku kwerendy spodziewano się od {0} do {1} rekordów, otrzymano {2}.\r\n" +
                            "Kwerenda SQL:\r\n" +
                            "{3}{3}", min, max, dane.Rows.Count, query, logParameters);
                    MessageBox.Show(message, "Niespodziewany wynik kwerendy", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    throw new ApplicationException("Unexpected SELECT result row count.");
                }
                return dane;
            }

            private void addParameters(OleDbCommand cmd)
            {
                for (int i = 0; i < parameters.Count; i++)
                {
                    OleDbParameter param;
                    var obj = parameters[i];
                    if (obj.value is byte[])
                    {
                        param = cmd.Parameters.Add("a" + i, obj.oleDbType, ((byte[])obj.value).Length);
                    }
                    else if (obj.value is string)
                    {
                        param = cmd.Parameters.Add("a" + i, obj.oleDbType, ((string)obj.value).Length);
                    }
                    else
                    {
                        param = cmd.Parameters.Add("a" + i, obj.oleDbType);
                    }
                    param.Value = obj.value;
                    param.Direction = ParameterDirection.Input;
                }
            }
                       
            protected void _INFO(string text)
            {
                logInfo = text;
            }

            private void CloseQuery()
            {
                if (typPolecenia == TypPolecenia.INSERT && values.Length > 0)
                {
                    query += ") VALUES (" + values + ")";
                    values = "";
                }
            }

            public int AffectedRows { get { return affectedRows; } }
            public long LastId { get { if (!lastIdValid) throw new ApplicationException("Last row ID is invalid!"); return lastId; } }
        }

        public class Wiersz
        {
            public static Dictionary<string, int> GetColsDict(DataRow row)
            {
                return GetColsDict(row.Table.Columns);
            }

            public static Dictionary<string, int> GetColsDict(DataTable table)
            {
                return GetColsDict(table.Columns);
            }

            public static Dictionary<string, int> GetColsDict(DataColumnCollection dataColumnCollection)
            {
                var result = new Dictionary<string, int>(dataColumnCollection.Count);
                for (var i = 0; i < dataColumnCollection.Count; i++) {
                    var col = dataColumnCollection[i];
                    result[col.ColumnName] = i;
                }
                return result;
            }
        }
    }
}
