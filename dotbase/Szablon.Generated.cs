using System;
using System.Data;
using System.Data.OleDb;
using System.Collections.Generic;

namespace DotBase
{
    partial class Szablon
    {
        public class Szablon_Atesty_zrodel : Tabela
        {
            public Szablon_Atesty_zrodel(BazaDanychWrapper baza, string nazwa) : base(baza, nazwa) { }
            public Szablon_Atesty_zrodel UPDATE() { _UPDATE(); return this; }
            public Szablon_Atesty_zrodel INSERT() { _INSERT(); return this; }
            public Szablon_Atesty_zrodel DELETE() { _DELETE(); return this; }
            public Szablon_Atesty_zrodel WHERE() { _WHERE(); return this; }
            public Szablon_Atesty_zrodel INFO(string text) { _INFO(text); return this; }
            public Szablon_Atesty_zrodel SELECT() { _SELECT(); return this; }
            public Szablon_Atesty_zrodel ORDER_BY() { _ORDER_BY(); return this; }
            public Row_Atesty_zrodel[] GET(int min = 0, int max = 999999999, bool allowException = false) { return Row_Atesty_zrodel._GET(_GET(min, max, allowException)); }
            public Row_Atesty_zrodel GET_ONE() { return Row_Atesty_zrodel._GET(_GET(1, 1))[0]; }
            public Row_Atesty_zrodel GET_OPTIONAL() { var r = Row_Atesty_zrodel._GET(_GET(0, 1)); return r.Length > 0 ? r[0] : null; }
            public Szablon_Atesty_zrodel Data_wzorcowania(DateTime? value)
            {
                SetField("Data_wzorcowania", value, OleDbType.Date);
                return this;
            }
            public Szablon_Atesty_zrodel Data_wzorcowania(DateTime? value, string oper)
            {
                SetField("Data_wzorcowania", value, OleDbType.Date, oper);
                return this;
            }
            public Szablon_Atesty_zrodel Data_wzorcowania(Order order)
            {
                SetOrder("Data_wzorcowania", order);
                return this;
            }
            public Szablon_Atesty_zrodel Data_wzorcowania()
            {
                AddField("Data_wzorcowania");
                return this;
            }
            public Szablon_Atesty_zrodel Emisja_powierzchniowa(double? value)
            {
                SetField("Emisja_powierzchniowa", value, OleDbType.Double);
                return this;
            }
            public Szablon_Atesty_zrodel Emisja_powierzchniowa(double? value, string oper)
            {
                SetField("Emisja_powierzchniowa", value, OleDbType.Double, oper);
                return this;
            }
            public Szablon_Atesty_zrodel Emisja_powierzchniowa(Order order)
            {
                SetOrder("Emisja_powierzchniowa", order);
                return this;
            }
            public Szablon_Atesty_zrodel Emisja_powierzchniowa()
            {
                AddField("Emisja_powierzchniowa");
                return this;
            }
            public Szablon_Atesty_zrodel ID_atestu(short? value)
            {
                SetField("ID_atestu", value, OleDbType.SmallInt);
                return this;
            }
            public Szablon_Atesty_zrodel ID_atestu(short? value, string oper)
            {
                SetField("ID_atestu", value, OleDbType.SmallInt, oper);
                return this;
            }
            public Szablon_Atesty_zrodel ID_atestu(Order order)
            {
                SetOrder("ID_atestu", order);
                return this;
            }
            public Szablon_Atesty_zrodel ID_atestu()
            {
                AddField("ID_atestu");
                return this;
            }
            public Szablon_Atesty_zrodel ID_zrodla(short? value)
            {
                SetField("ID_zrodla", value, OleDbType.SmallInt);
                return this;
            }
            public Szablon_Atesty_zrodel ID_zrodla(short? value, string oper)
            {
                SetField("ID_zrodla", value, OleDbType.SmallInt, oper);
                return this;
            }
            public Szablon_Atesty_zrodel ID_zrodla(Order order)
            {
                SetOrder("ID_zrodla", order);
                return this;
            }
            public Szablon_Atesty_zrodel ID_zrodla()
            {
                AddField("ID_zrodla");
                return this;
            }
            public Szablon_Atesty_zrodel Niepewnosc(double? value)
            {
                SetField("Niepewnosc", value, OleDbType.Double);
                return this;
            }
            public Szablon_Atesty_zrodel Niepewnosc(double? value, string oper)
            {
                SetField("Niepewnosc", value, OleDbType.Double, oper);
                return this;
            }
            public Szablon_Atesty_zrodel Niepewnosc(Order order)
            {
                SetOrder("Niepewnosc", order);
                return this;
            }
            public Szablon_Atesty_zrodel Niepewnosc()
            {
                AddField("Niepewnosc");
                return this;
            }
        }
        public class Row_Atesty_zrodel : Wiersz
        {
            public DateTime? Data_wzorcowania;
            public double? Emisja_powierzchniowa;
            public short? ID_atestu;
            public short? ID_zrodla;
            public double? Niepewnosc;
            public Row_Atesty_zrodel(DataRow row)
            {
                _init(row, GetColsDict(row));
            }
            public Row_Atesty_zrodel(DataRow row, Dictionary<string, int> cols)
            {
                _init(row, cols);
            }
            private void _init(DataRow row, Dictionary<string, int> cols)
            {
                if (cols.ContainsKey("Data_wzorcowania"))
                    Data_wzorcowania = row.Field<DateTime?>(cols["Data_wzorcowania"]);
                if (cols.ContainsKey("Emisja_powierzchniowa"))
                    Emisja_powierzchniowa = row.Field<double?>(cols["Emisja_powierzchniowa"]);
                if (cols.ContainsKey("ID_atestu"))
                    ID_atestu = row.Field<short?>(cols["ID_atestu"]);
                if (cols.ContainsKey("ID_zrodla"))
                    ID_zrodla = row.Field<short?>(cols["ID_zrodla"]);
                if (cols.ContainsKey("Niepewnosc"))
                    Niepewnosc = row.Field<double?>(cols["Niepewnosc"]);
            }
            internal static Row_Atesty_zrodel[] _GET(DataTable dane)
            {
                var result = new Row_Atesty_zrodel[dane.Rows.Count];
                var cols = GetColsDict(dane);
                var index = 0;
                foreach (DataRow row in dane.Rows)
                    result[index++] = new Row_Atesty_zrodel(row, cols);
                return result;
            }
        }
        public class Szablon_Błędy_wklejania : Tabela
        {
            public Szablon_Błędy_wklejania(BazaDanychWrapper baza, string nazwa) : base(baza, nazwa) { }
            public Szablon_Błędy_wklejania UPDATE() { _UPDATE(); return this; }
            public Szablon_Błędy_wklejania INSERT() { _INSERT(); return this; }
            public Szablon_Błędy_wklejania DELETE() { _DELETE(); return this; }
            public Szablon_Błędy_wklejania WHERE() { _WHERE(); return this; }
            public Szablon_Błędy_wklejania INFO(string text) { _INFO(text); return this; }
            public Szablon_Błędy_wklejania SELECT() { _SELECT(); return this; }
            public Szablon_Błędy_wklejania ORDER_BY() { _ORDER_BY(); return this; }
            public Row_Błędy_wklejania[] GET(int min = 0, int max = 999999999, bool allowException = false) { return Row_Błędy_wklejania._GET(_GET(min, max, allowException)); }
            public Row_Błędy_wklejania GET_ONE() { return Row_Błędy_wklejania._GET(_GET(1, 1))[0]; }
            public Row_Błędy_wklejania GET_OPTIONAL() { var r = Row_Błędy_wklejania._GET(_GET(0, 1)); return r.Length > 0 ? r[0] : null; }
            public Szablon_Błędy_wklejania Pole0(string value)
            {
                SetField("Pole0", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Błędy_wklejania Pole0(string value, string oper)
            {
                SetField("Pole0", value, OleDbType.WChar, oper);
                return this;
            }
            public Szablon_Błędy_wklejania Pole0(Order order)
            {
                SetOrder("Pole0", order);
                return this;
            }
            public Szablon_Błędy_wklejania Pole0()
            {
                AddField("Pole0");
                return this;
            }
        }
        public class Row_Błędy_wklejania : Wiersz
        {
            public string Pole0;
            public Row_Błędy_wklejania(DataRow row)
            {
                _init(row, GetColsDict(row));
            }
            public Row_Błędy_wklejania(DataRow row, Dictionary<string, int> cols)
            {
                _init(row, cols);
            }
            private void _init(DataRow row, Dictionary<string, int> cols)
            {
                if (cols.ContainsKey("Pole0"))
                    Pole0 = row.Field<string>(cols["Pole0"]);
            }
            internal static Row_Błędy_wklejania[] _GET(DataTable dane)
            {
                var result = new Row_Błędy_wklejania[dane.Rows.Count];
                var cols = GetColsDict(dane);
                var index = 0;
                foreach (DataRow row in dane.Rows)
                    result[index++] = new Row_Błędy_wklejania(row, cols);
                return result;
            }
        }
        public class Szablon_Budzetniepewnosci : Tabela
        {
            public Szablon_Budzetniepewnosci(BazaDanychWrapper baza, string nazwa) : base(baza, nazwa) { }
            public Szablon_Budzetniepewnosci UPDATE() { _UPDATE(); return this; }
            public Szablon_Budzetniepewnosci INSERT() { _INSERT(); return this; }
            public Szablon_Budzetniepewnosci DELETE() { _DELETE(); return this; }
            public Szablon_Budzetniepewnosci WHERE() { _WHERE(); return this; }
            public Szablon_Budzetniepewnosci INFO(string text) { _INFO(text); return this; }
            public Szablon_Budzetniepewnosci SELECT() { _SELECT(); return this; }
            public Szablon_Budzetniepewnosci ORDER_BY() { _ORDER_BY(); return this; }
            public Row_Budzetniepewnosci[] GET(int min = 0, int max = 999999999, bool allowException = false) { return Row_Budzetniepewnosci._GET(_GET(min, max, allowException)); }
            public Row_Budzetniepewnosci GET_ONE() { return Row_Budzetniepewnosci._GET(_GET(1, 1))[0]; }
            public Row_Budzetniepewnosci GET_OPTIONAL() { var r = Row_Budzetniepewnosci._GET(_GET(0, 1)); return r.Length > 0 ? r[0] : null; }
            public Szablon_Budzetniepewnosci Uwagi(string value)
            {
                SetField("Uwagi", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Budzetniepewnosci Uwagi(string value, string oper)
            {
                SetField("Uwagi", value, OleDbType.WChar, oper);
                return this;
            }
            public Szablon_Budzetniepewnosci Uwagi(Order order)
            {
                SetOrder("Uwagi", order);
                return this;
            }
            public Szablon_Budzetniepewnosci Uwagi()
            {
                AddField("Uwagi");
                return this;
            }
            public Szablon_Budzetniepewnosci wartosc(double? value)
            {
                SetField("wartosc", value, OleDbType.Double);
                return this;
            }
            public Szablon_Budzetniepewnosci wartosc(double? value, string oper)
            {
                SetField("wartosc", value, OleDbType.Double, oper);
                return this;
            }
            public Szablon_Budzetniepewnosci wartosc(Order order)
            {
                SetOrder("wartosc", order);
                return this;
            }
            public Szablon_Budzetniepewnosci wartosc()
            {
                AddField("wartosc");
                return this;
            }
            public Szablon_Budzetniepewnosci Wielkosc(string value)
            {
                SetField("Wielkosc", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Budzetniepewnosci Wielkosc(string value, string oper)
            {
                SetField("Wielkosc", value, OleDbType.WChar, oper);
                return this;
            }
            public Szablon_Budzetniepewnosci Wielkosc(Order order)
            {
                SetOrder("Wielkosc", order);
                return this;
            }
            public Szablon_Budzetniepewnosci Wielkosc()
            {
                AddField("Wielkosc");
                return this;
            }
        }
        public class Row_Budzetniepewnosci : Wiersz
        {
            public string Uwagi;
            public double? wartosc;
            public string Wielkosc;
            public Row_Budzetniepewnosci(DataRow row)
            {
                _init(row, GetColsDict(row));
            }
            public Row_Budzetniepewnosci(DataRow row, Dictionary<string, int> cols)
            {
                _init(row, cols);
            }
            private void _init(DataRow row, Dictionary<string, int> cols)
            {
                if (cols.ContainsKey("Uwagi"))
                    Uwagi = row.Field<string>(cols["Uwagi"]);
                if (cols.ContainsKey("wartosc"))
                    wartosc = row.Field<double?>(cols["wartosc"]);
                if (cols.ContainsKey("Wielkosc"))
                    Wielkosc = row.Field<string>(cols["Wielkosc"]);
            }
            internal static Row_Budzetniepewnosci[] _GET(DataTable dane)
            {
                var result = new Row_Budzetniepewnosci[dane.Rows.Count];
                var cols = GetColsDict(dane);
                var index = 0;
                foreach (DataRow row in dane.Rows)
                    result[index++] = new Row_Budzetniepewnosci(row, cols);
                return result;
            }
        }
        public class Szablon_Cennik : Tabela
        {
            public Szablon_Cennik(BazaDanychWrapper baza, string nazwa) : base(baza, nazwa) { }
            public Szablon_Cennik UPDATE() { _UPDATE(); return this; }
            public Szablon_Cennik INSERT() { _INSERT(); return this; }
            public Szablon_Cennik DELETE() { _DELETE(); return this; }
            public Szablon_Cennik WHERE() { _WHERE(); return this; }
            public Szablon_Cennik INFO(string text) { _INFO(text); return this; }
            public Szablon_Cennik SELECT() { _SELECT(); return this; }
            public Szablon_Cennik ORDER_BY() { _ORDER_BY(); return this; }
            public Row_Cennik[] GET(int min = 0, int max = 999999999, bool allowException = false) { return Row_Cennik._GET(_GET(min, max, allowException)); }
            public Row_Cennik GET_ONE() { return Row_Cennik._GET(_GET(1, 1))[0]; }
            public Row_Cennik GET_OPTIONAL() { var r = Row_Cennik._GET(_GET(0, 1)); return r.Length > 0 ? r[0] : null; }
            public Szablon_Cennik Cena(double? value)
            {
                SetField("Cena", value, OleDbType.Double);
                return this;
            }
            public Szablon_Cennik Cena(double? value, string oper)
            {
                SetField("Cena", value, OleDbType.Double, oper);
                return this;
            }
            public Szablon_Cennik Cena(Order order)
            {
                SetOrder("Cena", order);
                return this;
            }
            public Szablon_Cennik Cena()
            {
                AddField("Cena");
                return this;
            }
            public Szablon_Cennik Usluga(string value)
            {
                SetField("Usluga", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Cennik Usluga(string value, string oper)
            {
                SetField("Usluga", value, OleDbType.WChar, oper);
                return this;
            }
            public Szablon_Cennik Usluga(Order order)
            {
                SetOrder("Usluga", order);
                return this;
            }
            public Szablon_Cennik Usluga()
            {
                AddField("Usluga");
                return this;
            }
        }
        public class Row_Cennik : Wiersz
        {
            public double? Cena;
            public string Usluga;
            public Row_Cennik(DataRow row)
            {
                _init(row, GetColsDict(row));
            }
            public Row_Cennik(DataRow row, Dictionary<string, int> cols)
            {
                _init(row, cols);
            }
            private void _init(DataRow row, Dictionary<string, int> cols)
            {
                if (cols.ContainsKey("Cena"))
                    Cena = row.Field<double?>(cols["Cena"]);
                if (cols.ContainsKey("Usluga"))
                    Usluga = row.Field<string>(cols["Usluga"]);
            }
            internal static Row_Cennik[] _GET(DataTable dane)
            {
                var result = new Row_Cennik[dane.Rows.Count];
                var cols = GetColsDict(dane);
                var index = 0;
                foreach (DataRow row in dane.Rows)
                    result[index++] = new Row_Cennik(row, cols);
                return result;
            }
        }
        public class Szablon_Dozymetry : Tabela
        {
            public Szablon_Dozymetry(BazaDanychWrapper baza, string nazwa) : base(baza, nazwa) { }
            public Szablon_Dozymetry UPDATE() { _UPDATE(); return this; }
            public Szablon_Dozymetry INSERT() { _INSERT(); return this; }
            public Szablon_Dozymetry DELETE() { _DELETE(); return this; }
            public Szablon_Dozymetry WHERE() { _WHERE(); return this; }
            public Szablon_Dozymetry INFO(string text) { _INFO(text); return this; }
            public Szablon_Dozymetry SELECT() { _SELECT(); return this; }
            public Szablon_Dozymetry ORDER_BY() { _ORDER_BY(); return this; }
            public Row_Dozymetry[] GET(int min = 0, int max = 999999999, bool allowException = false) { return Row_Dozymetry._GET(_GET(min, max, allowException)); }
            public Row_Dozymetry GET_ONE() { return Row_Dozymetry._GET(_GET(1, 1))[0]; }
            public Row_Dozymetry GET_OPTIONAL() { var r = Row_Dozymetry._GET(_GET(0, 1)); return r.Length > 0 ? r[0] : null; }
            public Szablon_Dozymetry ID_dozymetru(int value)
            {
                SetField("ID_dozymetru", value, OleDbType.Integer);
                return this;
            }
            public Szablon_Dozymetry ID_dozymetru(int value, string oper)
            {
                SetField("ID_dozymetru", value, OleDbType.Integer, oper);
                return this;
            }
            public Szablon_Dozymetry ID_dozymetru(Order order)
            {
                SetOrder("ID_dozymetru", order);
                return this;
            }
            public Szablon_Dozymetry ID_dozymetru()
            {
                AddField("ID_dozymetru");
                return this;
            }
            public Szablon_Dozymetry Nazwa(string value)
            {
                SetField("Nazwa", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Dozymetry Nazwa(string value, string oper)
            {
                SetField("Nazwa", value, OleDbType.WChar, oper);
                return this;
            }
            public Szablon_Dozymetry Nazwa(Order order)
            {
                SetOrder("Nazwa", order);
                return this;
            }
            public Szablon_Dozymetry Nazwa()
            {
                AddField("Nazwa");
                return this;
            }
            public Szablon_Dozymetry Nr_fabryczny(string value)
            {
                SetField("Nr_fabryczny", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Dozymetry Nr_fabryczny(string value, string oper)
            {
                SetField("Nr_fabryczny", value, OleDbType.WChar, oper);
                return this;
            }
            public Szablon_Dozymetry Nr_fabryczny(Order order)
            {
                SetOrder("Nr_fabryczny", order);
                return this;
            }
            public Szablon_Dozymetry Nr_fabryczny()
            {
                AddField("Nr_fabryczny");
                return this;
            }
            public Szablon_Dozymetry Producent(string value)
            {
                SetField("Producent", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Dozymetry Producent(string value, string oper)
            {
                SetField("Producent", value, OleDbType.WChar, oper);
                return this;
            }
            public Szablon_Dozymetry Producent(Order order)
            {
                SetOrder("Producent", order);
                return this;
            }
            public Szablon_Dozymetry Producent()
            {
                AddField("Producent");
                return this;
            }
            public Szablon_Dozymetry Rok_produkcji(string value)
            {
                SetField("Rok_produkcji", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Dozymetry Rok_produkcji(string value, string oper)
            {
                SetField("Rok_produkcji", value, OleDbType.WChar, oper);
                return this;
            }
            public Szablon_Dozymetry Rok_produkcji(Order order)
            {
                SetOrder("Rok_produkcji", order);
                return this;
            }
            public Szablon_Dozymetry Rok_produkcji()
            {
                AddField("Rok_produkcji");
                return this;
            }
            public Szablon_Dozymetry Typ(string value)
            {
                SetField("Typ", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Dozymetry Typ(string value, string oper)
            {
                SetField("Typ", value, OleDbType.WChar, oper);
                return this;
            }
            public Szablon_Dozymetry Typ(Order order)
            {
                SetOrder("Typ", order);
                return this;
            }
            public Szablon_Dozymetry Typ()
            {
                AddField("Typ");
                return this;
            }
        }
        public class Row_Dozymetry : Wiersz
        {
            public int ID_dozymetru;
            public string Nazwa;
            public string Nr_fabryczny;
            public string Producent;
            public string Rok_produkcji;
            public string Typ;
            public Row_Dozymetry(DataRow row)
            {
                _init(row, GetColsDict(row));
            }
            public Row_Dozymetry(DataRow row, Dictionary<string, int> cols)
            {
                _init(row, cols);
            }
            private void _init(DataRow row, Dictionary<string, int> cols)
            {
                if (cols.ContainsKey("ID_dozymetru"))
                    ID_dozymetru = row.Field<int>(cols["ID_dozymetru"]);
                if (cols.ContainsKey("Nazwa"))
                    Nazwa = row.Field<string>(cols["Nazwa"]);
                if (cols.ContainsKey("Nr_fabryczny"))
                    Nr_fabryczny = row.Field<string>(cols["Nr_fabryczny"]);
                if (cols.ContainsKey("Producent"))
                    Producent = row.Field<string>(cols["Producent"]);
                if (cols.ContainsKey("Rok_produkcji"))
                    Rok_produkcji = row.Field<string>(cols["Rok_produkcji"]);
                if (cols.ContainsKey("Typ"))
                    Typ = row.Field<string>(cols["Typ"]);
            }
            internal static Row_Dozymetry[] _GET(DataTable dane)
            {
                var result = new Row_Dozymetry[dane.Rows.Count];
                var cols = GetColsDict(dane);
                var index = 0;
                foreach (DataRow row in dane.Rows)
                    result[index++] = new Row_Dozymetry(row, cols);
                return result;
            }
        }
        public class Szablon_Hasla : Tabela
        {
            public Szablon_Hasla(BazaDanychWrapper baza, string nazwa) : base(baza, nazwa) { }
            public Szablon_Hasla UPDATE() { _UPDATE(); return this; }
            public Szablon_Hasla INSERT() { _INSERT(); return this; }
            public Szablon_Hasla DELETE() { _DELETE(); return this; }
            public Szablon_Hasla WHERE() { _WHERE(); return this; }
            public Szablon_Hasla INFO(string text) { _INFO(text); return this; }
            public Szablon_Hasla SELECT() { _SELECT(); return this; }
            public Szablon_Hasla ORDER_BY() { _ORDER_BY(); return this; }
            public Row_Hasla[] GET(int min = 0, int max = 999999999, bool allowException = false) { return Row_Hasla._GET(_GET(min, max, allowException)); }
            public Row_Hasla GET_ONE() { return Row_Hasla._GET(_GET(1, 1))[0]; }
            public Row_Hasla GET_OPTIONAL() { var r = Row_Hasla._GET(_GET(0, 1)); return r.Length > 0 ? r[0] : null; }
            public Szablon_Hasla Haslo(string value)
            {
                SetField("Haslo", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Hasla Haslo(string value, string oper)
            {
                SetField("Haslo", value, OleDbType.WChar, oper);
                return this;
            }
            public Szablon_Hasla Haslo(Order order)
            {
                SetOrder("Haslo", order);
                return this;
            }
            public Szablon_Hasla Haslo()
            {
                AddField("Haslo");
                return this;
            }
            public Szablon_Hasla Login(string value)
            {
                SetField("Login", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Hasla Login(string value, string oper)
            {
                SetField("Login", value, OleDbType.WChar, oper);
                return this;
            }
            public Szablon_Hasla Login(Order order)
            {
                SetOrder("Login", order);
                return this;
            }
            public Szablon_Hasla Login()
            {
                AddField("Login");
                return this;
            }
        }
        public class Row_Hasla : Wiersz
        {
            public string Haslo;
            public string Login;
            public Row_Hasla(DataRow row)
            {
                _init(row, GetColsDict(row));
            }
            public Row_Hasla(DataRow row, Dictionary<string, int> cols)
            {
                _init(row, cols);
            }
            private void _init(DataRow row, Dictionary<string, int> cols)
            {
                if (cols.ContainsKey("Haslo"))
                    Haslo = row.Field<string>(cols["Haslo"]);
                if (cols.ContainsKey("Login"))
                    Login = row.Field<string>(cols["Login"]);
            }
            internal static Row_Hasla[] _GET(DataTable dane)
            {
                var result = new Row_Hasla[dane.Rows.Count];
                var cols = GetColsDict(dane);
                var index = 0;
                foreach (DataRow row in dane.Rows)
                    result[index++] = new Row_Hasla(row, cols);
                return result;
            }
        }
        public class Szablon_Jednostki : Tabela
        {
            public Szablon_Jednostki(BazaDanychWrapper baza, string nazwa) : base(baza, nazwa) { }
            public Szablon_Jednostki UPDATE() { _UPDATE(); return this; }
            public Szablon_Jednostki INSERT() { _INSERT(); return this; }
            public Szablon_Jednostki DELETE() { _DELETE(); return this; }
            public Szablon_Jednostki WHERE() { _WHERE(); return this; }
            public Szablon_Jednostki INFO(string text) { _INFO(text); return this; }
            public Szablon_Jednostki SELECT() { _SELECT(); return this; }
            public Szablon_Jednostki ORDER_BY() { _ORDER_BY(); return this; }
            public Row_Jednostki[] GET(int min = 0, int max = 999999999, bool allowException = false) { return Row_Jednostki._GET(_GET(min, max, allowException)); }
            public Row_Jednostki GET_ONE() { return Row_Jednostki._GET(_GET(1, 1))[0]; }
            public Row_Jednostki GET_OPTIONAL() { var r = Row_Jednostki._GET(_GET(0, 1)); return r.Length > 0 ? r[0] : null; }
            public Szablon_Jednostki ID_jednostki(int value)
            {
                SetField("ID_jednostki", value, OleDbType.Integer);
                return this;
            }
            public Szablon_Jednostki ID_jednostki(int value, string oper)
            {
                SetField("ID_jednostki", value, OleDbType.Integer, oper);
                return this;
            }
            public Szablon_Jednostki ID_jednostki(Order order)
            {
                SetOrder("ID_jednostki", order);
                return this;
            }
            public Szablon_Jednostki ID_jednostki()
            {
                AddField("ID_jednostki");
                return this;
            }
            public Szablon_Jednostki Jednostka(string value)
            {
                SetField("Jednostka", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Jednostki Jednostka(string value, string oper)
            {
                SetField("Jednostka", value, OleDbType.WChar, oper);
                return this;
            }
            public Szablon_Jednostki Jednostka(Order order)
            {
                SetOrder("Jednostka", order);
                return this;
            }
            public Szablon_Jednostki Jednostka()
            {
                AddField("Jednostka");
                return this;
            }
            public Szablon_Jednostki Przelicznik(float? value)
            {
                SetField("Przelicznik", value, OleDbType.Single);
                return this;
            }
            public Szablon_Jednostki Przelicznik(float? value, string oper)
            {
                SetField("Przelicznik", value, OleDbType.Single, oper);
                return this;
            }
            public Szablon_Jednostki Przelicznik(Order order)
            {
                SetOrder("Przelicznik", order);
                return this;
            }
            public Szablon_Jednostki Przelicznik()
            {
                AddField("Przelicznik");
                return this;
            }
            public Szablon_Jednostki SI(bool value)
            {
                SetField("SI", value, OleDbType.Boolean);
                return this;
            }
            public Szablon_Jednostki SI(bool value, string oper)
            {
                SetField("SI", value, OleDbType.Boolean, oper);
                return this;
            }
            public Szablon_Jednostki SI(Order order)
            {
                SetOrder("SI", order);
                return this;
            }
            public Szablon_Jednostki SI()
            {
                AddField("SI");
                return this;
            }
            public Szablon_Jednostki Wielkosc_fizyczna(string value)
            {
                SetField("Wielkosc_fizyczna", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Jednostki Wielkosc_fizyczna(string value, string oper)
            {
                SetField("Wielkosc_fizyczna", value, OleDbType.WChar, oper);
                return this;
            }
            public Szablon_Jednostki Wielkosc_fizyczna(Order order)
            {
                SetOrder("Wielkosc_fizyczna", order);
                return this;
            }
            public Szablon_Jednostki Wielkosc_fizyczna()
            {
                AddField("Wielkosc_fizyczna");
                return this;
            }
        }
        public class Row_Jednostki : Wiersz
        {
            public int ID_jednostki;
            public string Jednostka;
            public float? Przelicznik;
            public bool SI;
            public string Wielkosc_fizyczna;
            public Row_Jednostki(DataRow row)
            {
                _init(row, GetColsDict(row));
            }
            public Row_Jednostki(DataRow row, Dictionary<string, int> cols)
            {
                _init(row, cols);
            }
            private void _init(DataRow row, Dictionary<string, int> cols)
            {
                if (cols.ContainsKey("ID_jednostki"))
                    ID_jednostki = row.Field<int>(cols["ID_jednostki"]);
                if (cols.ContainsKey("Jednostka"))
                    Jednostka = row.Field<string>(cols["Jednostka"]);
                if (cols.ContainsKey("Przelicznik"))
                    Przelicznik = row.Field<float?>(cols["Przelicznik"]);
                if (cols.ContainsKey("SI"))
                    SI = row.Field<bool>(cols["SI"]);
                if (cols.ContainsKey("Wielkosc_fizyczna"))
                    Wielkosc_fizyczna = row.Field<string>(cols["Wielkosc_fizyczna"]);
            }
            internal static Row_Jednostki[] _GET(DataTable dane)
            {
                var result = new Row_Jednostki[dane.Rows.Count];
                var cols = GetColsDict(dane);
                var index = 0;
                foreach (DataRow row in dane.Rows)
                    result[index++] = new Row_Jednostki(row, cols);
                return result;
            }
        }
        public class Szablon_Karta_przyjecia : Tabela
        {
            public Szablon_Karta_przyjecia(BazaDanychWrapper baza, string nazwa) : base(baza, nazwa) { }
            public Szablon_Karta_przyjecia UPDATE() { _UPDATE(); return this; }
            public Szablon_Karta_przyjecia INSERT() { _INSERT(); return this; }
            public Szablon_Karta_przyjecia DELETE() { _DELETE(); return this; }
            public Szablon_Karta_przyjecia WHERE() { _WHERE(); return this; }
            public Szablon_Karta_przyjecia INFO(string text) { _INFO(text); return this; }
            public Szablon_Karta_przyjecia SELECT() { _SELECT(); return this; }
            public Szablon_Karta_przyjecia ORDER_BY() { _ORDER_BY(); return this; }
            public Row_Karta_przyjecia[] GET(int min = 0, int max = 999999999, bool allowException = false) { return Row_Karta_przyjecia._GET(_GET(min, max, allowException)); }
            public Row_Karta_przyjecia GET_ONE() { return Row_Karta_przyjecia._GET(_GET(1, 1))[0]; }
            public Row_Karta_przyjecia GET_OPTIONAL() { var r = Row_Karta_przyjecia._GET(_GET(0, 1)); return r.Length > 0 ? r[0] : null; }
            public Szablon_Karta_przyjecia Akcesoria(string value)
            {
                SetField("Akcesoria", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Karta_przyjecia Akcesoria(string value, string oper)
            {
                SetField("Akcesoria", value, OleDbType.WChar, oper);
                return this;
            }
            public Szablon_Karta_przyjecia Akcesoria(Order order)
            {
                SetOrder("Akcesoria", order);
                return this;
            }
            public Szablon_Karta_przyjecia Akcesoria()
            {
                AddField("Akcesoria");
                return this;
            }
            public Szablon_Karta_przyjecia Ameryk(bool value)
            {
                SetField("Ameryk", value, OleDbType.Boolean);
                return this;
            }
            public Szablon_Karta_przyjecia Ameryk(bool value, string oper)
            {
                SetField("Ameryk", value, OleDbType.Boolean, oper);
                return this;
            }
            public Szablon_Karta_przyjecia Ameryk(Order order)
            {
                SetOrder("Ameryk", order);
                return this;
            }
            public Szablon_Karta_przyjecia Ameryk()
            {
                AddField("Ameryk");
                return this;
            }
            public Szablon_Karta_przyjecia Cena(double? value)
            {
                SetField("Cena", value, OleDbType.Double);
                return this;
            }
            public Szablon_Karta_przyjecia Cena(double? value, string oper)
            {
                SetField("Cena", value, OleDbType.Double, oper);
                return this;
            }
            public Szablon_Karta_przyjecia Cena(Order order)
            {
                SetOrder("Cena", order);
                return this;
            }
            public Szablon_Karta_przyjecia Cena()
            {
                AddField("Cena");
                return this;
            }
            public Szablon_Karta_przyjecia Chlor(bool value)
            {
                SetField("Chlor", value, OleDbType.Boolean);
                return this;
            }
            public Szablon_Karta_przyjecia Chlor(bool value, string oper)
            {
                SetField("Chlor", value, OleDbType.Boolean, oper);
                return this;
            }
            public Szablon_Karta_przyjecia Chlor(Order order)
            {
                SetOrder("Chlor", order);
                return this;
            }
            public Szablon_Karta_przyjecia Chlor()
            {
                AddField("Chlor");
                return this;
            }
            public Szablon_Karta_przyjecia Dawka(bool value)
            {
                SetField("Dawka", value, OleDbType.Boolean);
                return this;
            }
            public Szablon_Karta_przyjecia Dawka(bool value, string oper)
            {
                SetField("Dawka", value, OleDbType.Boolean, oper);
                return this;
            }
            public Szablon_Karta_przyjecia Dawka(Order order)
            {
                SetOrder("Dawka", order);
                return this;
            }
            public Szablon_Karta_przyjecia Dawka()
            {
                AddField("Dawka");
                return this;
            }
            public Szablon_Karta_przyjecia ID_dozymetru(int? value)
            {
                SetField("ID_dozymetru", value, OleDbType.Integer);
                return this;
            }
            public Szablon_Karta_przyjecia ID_dozymetru(int? value, string oper)
            {
                SetField("ID_dozymetru", value, OleDbType.Integer, oper);
                return this;
            }
            public Szablon_Karta_przyjecia ID_dozymetru(Order order)
            {
                SetOrder("ID_dozymetru", order);
                return this;
            }
            public Szablon_Karta_przyjecia ID_dozymetru()
            {
                AddField("ID_dozymetru");
                return this;
            }
            public Szablon_Karta_przyjecia ID_karty(int? value)
            {
                SetField("ID_karty", value, OleDbType.Integer);
                return this;
            }
            public Szablon_Karta_przyjecia ID_karty(int? value, string oper)
            {
                SetField("ID_karty", value, OleDbType.Integer, oper);
                return this;
            }
            public Szablon_Karta_przyjecia ID_karty(Order order)
            {
                SetOrder("ID_karty", order);
                return this;
            }
            public Szablon_Karta_przyjecia ID_karty()
            {
                AddField("ID_karty");
                return this;
            }
            public Szablon_Karta_przyjecia ID_zlecenia(int? value)
            {
                SetField("ID_zlecenia", value, OleDbType.Integer);
                return this;
            }
            public Szablon_Karta_przyjecia ID_zlecenia(int? value, string oper)
            {
                SetField("ID_zlecenia", value, OleDbType.Integer, oper);
                return this;
            }
            public Szablon_Karta_przyjecia ID_zlecenia(Order order)
            {
                SetOrder("ID_zlecenia", order);
                return this;
            }
            public Szablon_Karta_przyjecia ID_zlecenia()
            {
                AddField("ID_zlecenia");
                return this;
            }
            public Szablon_Karta_przyjecia Moc_dawki(bool value)
            {
                SetField("Moc_dawki", value, OleDbType.Boolean);
                return this;
            }
            public Szablon_Karta_przyjecia Moc_dawki(bool value, string oper)
            {
                SetField("Moc_dawki", value, OleDbType.Boolean, oper);
                return this;
            }
            public Szablon_Karta_przyjecia Moc_dawki(Order order)
            {
                SetOrder("Moc_dawki", order);
                return this;
            }
            public Szablon_Karta_przyjecia Moc_dawki()
            {
                AddField("Moc_dawki");
                return this;
            }
            public Szablon_Karta_przyjecia Nr_pisma(int? value)
            {
                SetField("Nr_pisma", value, OleDbType.Integer);
                return this;
            }
            public Szablon_Karta_przyjecia Nr_pisma(int? value, string oper)
            {
                SetField("Nr_pisma", value, OleDbType.Integer, oper);
                return this;
            }
            public Szablon_Karta_przyjecia Nr_pisma(Order order)
            {
                SetOrder("Nr_pisma", order);
                return this;
            }
            public Szablon_Karta_przyjecia Nr_pisma()
            {
                AddField("Nr_pisma");
                return this;
            }
            public Szablon_Karta_przyjecia Pluton(bool value)
            {
                SetField("Pluton", value, OleDbType.Boolean);
                return this;
            }
            public Szablon_Karta_przyjecia Pluton(bool value, string oper)
            {
                SetField("Pluton", value, OleDbType.Boolean, oper);
                return this;
            }
            public Szablon_Karta_przyjecia Pluton(Order order)
            {
                SetOrder("Pluton", order);
                return this;
            }
            public Szablon_Karta_przyjecia Pluton()
            {
                AddField("Pluton");
                return this;
            }
            public Szablon_Karta_przyjecia Rok(int? value)
            {
                SetField("Rok", value, OleDbType.Integer);
                return this;
            }
            public Szablon_Karta_przyjecia Rok(int? value, string oper)
            {
                SetField("Rok", value, OleDbType.Integer, oper);
                return this;
            }
            public Szablon_Karta_przyjecia Rok(Order order)
            {
                SetOrder("Rok", order);
                return this;
            }
            public Szablon_Karta_przyjecia Rok()
            {
                AddField("Rok");
                return this;
            }
            public Szablon_Karta_przyjecia Rok_pisma(int? value)
            {
                SetField("Rok_pisma", value, OleDbType.Integer);
                return this;
            }
            public Szablon_Karta_przyjecia Rok_pisma(int? value, string oper)
            {
                SetField("Rok_pisma", value, OleDbType.Integer, oper);
                return this;
            }
            public Szablon_Karta_przyjecia Rok_pisma(Order order)
            {
                SetOrder("Rok_pisma", order);
                return this;
            }
            public Szablon_Karta_przyjecia Rok_pisma()
            {
                AddField("Rok_pisma");
                return this;
            }
            public Szablon_Karta_przyjecia Sprawdzenie(bool value)
            {
                SetField("Sprawdzenie", value, OleDbType.Boolean);
                return this;
            }
            public Szablon_Karta_przyjecia Sprawdzenie(bool value, string oper)
            {
                SetField("Sprawdzenie", value, OleDbType.Boolean, oper);
                return this;
            }
            public Szablon_Karta_przyjecia Sprawdzenie(Order order)
            {
                SetOrder("Sprawdzenie", order);
                return this;
            }
            public Szablon_Karta_przyjecia Sprawdzenie()
            {
                AddField("Sprawdzenie");
                return this;
            }
            public Szablon_Karta_przyjecia Stront_najsilniejszy(bool value)
            {
                SetField("Stront_najsilniejszy", value, OleDbType.Boolean);
                return this;
            }
            public Szablon_Karta_przyjecia Stront_najsilniejszy(bool value, string oper)
            {
                SetField("Stront_najsilniejszy", value, OleDbType.Boolean, oper);
                return this;
            }
            public Szablon_Karta_przyjecia Stront_najsilniejszy(Order order)
            {
                SetOrder("Stront_najsilniejszy", order);
                return this;
            }
            public Szablon_Karta_przyjecia Stront_najsilniejszy()
            {
                AddField("Stront_najsilniejszy");
                return this;
            }
            public Szablon_Karta_przyjecia Stront_silny(bool value)
            {
                SetField("Stront_silny", value, OleDbType.Boolean);
                return this;
            }
            public Szablon_Karta_przyjecia Stront_silny(bool value, string oper)
            {
                SetField("Stront_silny", value, OleDbType.Boolean, oper);
                return this;
            }
            public Szablon_Karta_przyjecia Stront_silny(Order order)
            {
                SetOrder("Stront_silny", order);
                return this;
            }
            public Szablon_Karta_przyjecia Stront_silny()
            {
                AddField("Stront_silny");
                return this;
            }
            public Szablon_Karta_przyjecia Stront_slaby(bool value)
            {
                SetField("Stront_slaby", value, OleDbType.Boolean);
                return this;
            }
            public Szablon_Karta_przyjecia Stront_slaby(bool value, string oper)
            {
                SetField("Stront_slaby", value, OleDbType.Boolean, oper);
                return this;
            }
            public Szablon_Karta_przyjecia Stront_slaby(Order order)
            {
                SetOrder("Stront_slaby", order);
                return this;
            }
            public Szablon_Karta_przyjecia Stront_slaby()
            {
                AddField("Stront_slaby");
                return this;
            }
            public Szablon_Karta_przyjecia Syg_dawki(bool value)
            {
                SetField("Syg_dawki", value, OleDbType.Boolean);
                return this;
            }
            public Szablon_Karta_przyjecia Syg_dawki(bool value, string oper)
            {
                SetField("Syg_dawki", value, OleDbType.Boolean, oper);
                return this;
            }
            public Szablon_Karta_przyjecia Syg_dawki(Order order)
            {
                SetOrder("Syg_dawki", order);
                return this;
            }
            public Szablon_Karta_przyjecia Syg_dawki()
            {
                AddField("Syg_dawki");
                return this;
            }
            public Szablon_Karta_przyjecia Syg_mocy_dawki(bool value)
            {
                SetField("Syg_mocy_dawki", value, OleDbType.Boolean);
                return this;
            }
            public Szablon_Karta_przyjecia Syg_mocy_dawki(bool value, string oper)
            {
                SetField("Syg_mocy_dawki", value, OleDbType.Boolean, oper);
                return this;
            }
            public Szablon_Karta_przyjecia Syg_mocy_dawki(Order order)
            {
                SetOrder("Syg_mocy_dawki", order);
                return this;
            }
            public Szablon_Karta_przyjecia Syg_mocy_dawki()
            {
                AddField("Syg_mocy_dawki");
                return this;
            }
            public Szablon_Karta_przyjecia Test_na_skazenia(string value)
            {
                SetField("Test_na_skazenia", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Karta_przyjecia Test_na_skazenia(string value, string oper)
            {
                SetField("Test_na_skazenia", value, OleDbType.WChar, oper);
                return this;
            }
            public Szablon_Karta_przyjecia Test_na_skazenia(Order order)
            {
                SetOrder("Test_na_skazenia", order);
                return this;
            }
            public Szablon_Karta_przyjecia Test_na_skazenia()
            {
                AddField("Test_na_skazenia");
                return this;
            }
            public Szablon_Karta_przyjecia Uszkodzony(bool value)
            {
                SetField("Uszkodzony", value, OleDbType.Boolean);
                return this;
            }
            public Szablon_Karta_przyjecia Uszkodzony(bool value, string oper)
            {
                SetField("Uszkodzony", value, OleDbType.Boolean, oper);
                return this;
            }
            public Szablon_Karta_przyjecia Uszkodzony(Order order)
            {
                SetOrder("Uszkodzony", order);
                return this;
            }
            public Szablon_Karta_przyjecia Uszkodzony()
            {
                AddField("Uszkodzony");
                return this;
            }
            public Szablon_Karta_przyjecia Uwagi(string value)
            {
                SetField("Uwagi", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Karta_przyjecia Uwagi(string value, string oper)
            {
                SetField("Uwagi", value, OleDbType.WChar, oper);
                return this;
            }
            public Szablon_Karta_przyjecia Uwagi(Order order)
            {
                SetOrder("Uwagi", order);
                return this;
            }
            public Szablon_Karta_przyjecia Uwagi()
            {
                AddField("Uwagi");
                return this;
            }
            public Szablon_Karta_przyjecia Wegiel_silny(bool value)
            {
                SetField("Wegiel_silny", value, OleDbType.Boolean);
                return this;
            }
            public Szablon_Karta_przyjecia Wegiel_silny(bool value, string oper)
            {
                SetField("Wegiel_silny", value, OleDbType.Boolean, oper);
                return this;
            }
            public Szablon_Karta_przyjecia Wegiel_silny(Order order)
            {
                SetOrder("Wegiel_silny", order);
                return this;
            }
            public Szablon_Karta_przyjecia Wegiel_silny()
            {
                AddField("Wegiel_silny");
                return this;
            }
            public Szablon_Karta_przyjecia Wegiel_slaby(bool value)
            {
                SetField("Wegiel_slaby", value, OleDbType.Boolean);
                return this;
            }
            public Szablon_Karta_przyjecia Wegiel_slaby(bool value, string oper)
            {
                SetField("Wegiel_slaby", value, OleDbType.Boolean, oper);
                return this;
            }
            public Szablon_Karta_przyjecia Wegiel_slaby(Order order)
            {
                SetOrder("Wegiel_slaby", order);
                return this;
            }
            public Szablon_Karta_przyjecia Wegiel_slaby()
            {
                AddField("Wegiel_slaby");
                return this;
            }
            public Szablon_Karta_przyjecia Wykonano(bool value)
            {
                SetField("Wykonano", value, OleDbType.Boolean);
                return this;
            }
            public Szablon_Karta_przyjecia Wykonano(bool value, string oper)
            {
                SetField("Wykonano", value, OleDbType.Boolean, oper);
                return this;
            }
            public Szablon_Karta_przyjecia Wykonano(Order order)
            {
                SetOrder("Wykonano", order);
                return this;
            }
            public Szablon_Karta_przyjecia Wykonano()
            {
                AddField("Wykonano");
                return this;
            }
        }
        public class Row_Karta_przyjecia : Wiersz
        {
            public string Akcesoria;
            public bool Ameryk;
            public double? Cena;
            public bool Chlor;
            public bool Dawka;
            public int? ID_dozymetru;
            public int? ID_karty;
            public int? ID_zlecenia;
            public bool Moc_dawki;
            public int? Nr_pisma;
            public bool Pluton;
            public int? Rok;
            public int? Rok_pisma;
            public bool Sprawdzenie;
            public bool Stront_najsilniejszy;
            public bool Stront_silny;
            public bool Stront_slaby;
            public bool Syg_dawki;
            public bool Syg_mocy_dawki;
            public string Test_na_skazenia;
            public bool Uszkodzony;
            public string Uwagi;
            public bool Wegiel_silny;
            public bool Wegiel_slaby;
            public bool Wykonano;
            public Row_Karta_przyjecia(DataRow row)
            {
                _init(row, GetColsDict(row));
            }
            public Row_Karta_przyjecia(DataRow row, Dictionary<string, int> cols)
            {
                _init(row, cols);
            }
            private void _init(DataRow row, Dictionary<string, int> cols)
            {
                if (cols.ContainsKey("Akcesoria"))
                    Akcesoria = row.Field<string>(cols["Akcesoria"]);
                if (cols.ContainsKey("Ameryk"))
                    Ameryk = row.Field<bool>(cols["Ameryk"]);
                if (cols.ContainsKey("Cena"))
                    Cena = row.Field<double?>(cols["Cena"]);
                if (cols.ContainsKey("Chlor"))
                    Chlor = row.Field<bool>(cols["Chlor"]);
                if (cols.ContainsKey("Dawka"))
                    Dawka = row.Field<bool>(cols["Dawka"]);
                if (cols.ContainsKey("ID_dozymetru"))
                    ID_dozymetru = row.Field<int?>(cols["ID_dozymetru"]);
                if (cols.ContainsKey("ID_karty"))
                    ID_karty = row.Field<int?>(cols["ID_karty"]);
                if (cols.ContainsKey("ID_zlecenia"))
                    ID_zlecenia = row.Field<int?>(cols["ID_zlecenia"]);
                if (cols.ContainsKey("Moc_dawki"))
                    Moc_dawki = row.Field<bool>(cols["Moc_dawki"]);
                if (cols.ContainsKey("Nr_pisma"))
                    Nr_pisma = row.Field<int?>(cols["Nr_pisma"]);
                if (cols.ContainsKey("Pluton"))
                    Pluton = row.Field<bool>(cols["Pluton"]);
                if (cols.ContainsKey("Rok"))
                    Rok = row.Field<int?>(cols["Rok"]);
                if (cols.ContainsKey("Rok_pisma"))
                    Rok_pisma = row.Field<int?>(cols["Rok_pisma"]);
                if (cols.ContainsKey("Sprawdzenie"))
                    Sprawdzenie = row.Field<bool>(cols["Sprawdzenie"]);
                if (cols.ContainsKey("Stront_najsilniejszy"))
                    Stront_najsilniejszy = row.Field<bool>(cols["Stront_najsilniejszy"]);
                if (cols.ContainsKey("Stront_silny"))
                    Stront_silny = row.Field<bool>(cols["Stront_silny"]);
                if (cols.ContainsKey("Stront_slaby"))
                    Stront_slaby = row.Field<bool>(cols["Stront_slaby"]);
                if (cols.ContainsKey("Syg_dawki"))
                    Syg_dawki = row.Field<bool>(cols["Syg_dawki"]);
                if (cols.ContainsKey("Syg_mocy_dawki"))
                    Syg_mocy_dawki = row.Field<bool>(cols["Syg_mocy_dawki"]);
                if (cols.ContainsKey("Test_na_skazenia"))
                    Test_na_skazenia = row.Field<string>(cols["Test_na_skazenia"]);
                if (cols.ContainsKey("Uszkodzony"))
                    Uszkodzony = row.Field<bool>(cols["Uszkodzony"]);
                if (cols.ContainsKey("Uwagi"))
                    Uwagi = row.Field<string>(cols["Uwagi"]);
                if (cols.ContainsKey("Wegiel_silny"))
                    Wegiel_silny = row.Field<bool>(cols["Wegiel_silny"]);
                if (cols.ContainsKey("Wegiel_slaby"))
                    Wegiel_slaby = row.Field<bool>(cols["Wegiel_slaby"]);
                if (cols.ContainsKey("Wykonano"))
                    Wykonano = row.Field<bool>(cols["Wykonano"]);
            }
            internal static Row_Karta_przyjecia[] _GET(DataTable dane)
            {
                var result = new Row_Karta_przyjecia[dane.Rows.Count];
                var cols = GetColsDict(dane);
                var index = 0;
                foreach (DataRow row in dane.Rows)
                    result[index++] = new Row_Karta_przyjecia(row, cols);
                return result;
            }
        }
        public class Szablon_Pomiary_cez : Tabela
        {
            public Szablon_Pomiary_cez(BazaDanychWrapper baza, string nazwa) : base(baza, nazwa) { }
            public Szablon_Pomiary_cez UPDATE() { _UPDATE(); return this; }
            public Szablon_Pomiary_cez INSERT() { _INSERT(); return this; }
            public Szablon_Pomiary_cez DELETE() { _DELETE(); return this; }
            public Szablon_Pomiary_cez WHERE() { _WHERE(); return this; }
            public Szablon_Pomiary_cez INFO(string text) { _INFO(text); return this; }
            public Szablon_Pomiary_cez SELECT() { _SELECT(); return this; }
            public Szablon_Pomiary_cez ORDER_BY() { _ORDER_BY(); return this; }
            public Row_Pomiary_cez[] GET(int min = 0, int max = 999999999, bool allowException = false) { return Row_Pomiary_cez._GET(_GET(min, max, allowException)); }
            public Row_Pomiary_cez GET_ONE() { return Row_Pomiary_cez._GET(_GET(1, 1))[0]; }
            public Row_Pomiary_cez GET_OPTIONAL() { var r = Row_Pomiary_cez._GET(_GET(0, 1)); return r.Length > 0 ? r[0] : null; }
            public Szablon_Pomiary_cez Dolaczyc(bool value)
            {
                SetField("Dolaczyc", value, OleDbType.Boolean);
                return this;
            }
            public Szablon_Pomiary_cez Dolaczyc(bool value, string oper)
            {
                SetField("Dolaczyc", value, OleDbType.Boolean, oper);
                return this;
            }
            public Szablon_Pomiary_cez Dolaczyc(Order order)
            {
                SetOrder("Dolaczyc", order);
                return this;
            }
            public Szablon_Pomiary_cez Dolaczyc()
            {
                AddField("Dolaczyc");
                return this;
            }
            public Szablon_Pomiary_cez ID_wzorcowania(int? value)
            {
                SetField("ID_wzorcowania", value, OleDbType.Integer);
                return this;
            }
            public Szablon_Pomiary_cez ID_wzorcowania(int? value, string oper)
            {
                SetField("ID_wzorcowania", value, OleDbType.Integer, oper);
                return this;
            }
            public Szablon_Pomiary_cez ID_wzorcowania(Order order)
            {
                SetOrder("ID_wzorcowania", order);
                return this;
            }
            public Szablon_Pomiary_cez ID_wzorcowania()
            {
                AddField("ID_wzorcowania");
                return this;
            }
            public Szablon_Pomiary_cez ID_zrodla(short? value)
            {
                SetField("ID_zrodla", value, OleDbType.SmallInt);
                return this;
            }
            public Szablon_Pomiary_cez ID_zrodla(short? value, string oper)
            {
                SetField("ID_zrodla", value, OleDbType.SmallInt, oper);
                return this;
            }
            public Szablon_Pomiary_cez ID_zrodla(Order order)
            {
                SetOrder("ID_zrodla", order);
                return this;
            }
            public Szablon_Pomiary_cez ID_zrodla()
            {
                AddField("ID_zrodla");
                return this;
            }
            public Szablon_Pomiary_cez Odleglosc(double? value)
            {
                SetField("Odleglosc", value, OleDbType.Double);
                return this;
            }
            public Szablon_Pomiary_cez Odleglosc(double? value, string oper)
            {
                SetField("Odleglosc", value, OleDbType.Double, oper);
                return this;
            }
            public Szablon_Pomiary_cez Odleglosc(Order order)
            {
                SetOrder("Odleglosc", order);
                return this;
            }
            public Szablon_Pomiary_cez Odleglosc()
            {
                AddField("Odleglosc");
                return this;
            }
            public Szablon_Pomiary_cez Wahanie(double? value)
            {
                SetField("Wahanie", value, OleDbType.Double);
                return this;
            }
            public Szablon_Pomiary_cez Wahanie(double? value, string oper)
            {
                SetField("Wahanie", value, OleDbType.Double, oper);
                return this;
            }
            public Szablon_Pomiary_cez Wahanie(Order order)
            {
                SetOrder("Wahanie", order);
                return this;
            }
            public Szablon_Pomiary_cez Wahanie()
            {
                AddField("Wahanie");
                return this;
            }
            public Szablon_Pomiary_cez Wskazanie(double? value)
            {
                SetField("Wskazanie", value, OleDbType.Double);
                return this;
            }
            public Szablon_Pomiary_cez Wskazanie(double? value, string oper)
            {
                SetField("Wskazanie", value, OleDbType.Double, oper);
                return this;
            }
            public Szablon_Pomiary_cez Wskazanie(Order order)
            {
                SetOrder("Wskazanie", order);
                return this;
            }
            public Szablon_Pomiary_cez Wskazanie()
            {
                AddField("Wskazanie");
                return this;
            }
            public Szablon_Pomiary_cez Zakres(string value)
            {
                SetField("Zakres", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Pomiary_cez Zakres(string value, string oper)
            {
                SetField("Zakres", value, OleDbType.WChar, oper);
                return this;
            }
            public Szablon_Pomiary_cez Zakres(Order order)
            {
                SetOrder("Zakres", order);
                return this;
            }
            public Szablon_Pomiary_cez Zakres()
            {
                AddField("Zakres");
                return this;
            }
        }
        public class Row_Pomiary_cez : Wiersz
        {
            public bool Dolaczyc;
            public int? ID_wzorcowania;
            public short? ID_zrodla;
            public double? Odleglosc;
            public double? Wahanie;
            public double? Wskazanie;
            public string Zakres;
            public Row_Pomiary_cez(DataRow row)
            {
                _init(row, GetColsDict(row));
            }
            public Row_Pomiary_cez(DataRow row, Dictionary<string, int> cols)
            {
                _init(row, cols);
            }
            private void _init(DataRow row, Dictionary<string, int> cols)
            {
                if (cols.ContainsKey("Dolaczyc"))
                    Dolaczyc = row.Field<bool>(cols["Dolaczyc"]);
                if (cols.ContainsKey("ID_wzorcowania"))
                    ID_wzorcowania = row.Field<int?>(cols["ID_wzorcowania"]);
                if (cols.ContainsKey("ID_zrodla"))
                    ID_zrodla = row.Field<short?>(cols["ID_zrodla"]);
                if (cols.ContainsKey("Odleglosc"))
                    Odleglosc = row.Field<double?>(cols["Odleglosc"]);
                if (cols.ContainsKey("Wahanie"))
                    Wahanie = row.Field<double?>(cols["Wahanie"]);
                if (cols.ContainsKey("Wskazanie"))
                    Wskazanie = row.Field<double?>(cols["Wskazanie"]);
                if (cols.ContainsKey("Zakres"))
                    Zakres = row.Field<string>(cols["Zakres"]);
            }
            internal static Row_Pomiary_cez[] _GET(DataTable dane)
            {
                var result = new Row_Pomiary_cez[dane.Rows.Count];
                var cols = GetColsDict(dane);
                var index = 0;
                foreach (DataRow row in dane.Rows)
                    result[index++] = new Row_Pomiary_cez(row, cols);
                return result;
            }
        }
        public class Szablon_Pomiary_dawka : Tabela
        {
            public Szablon_Pomiary_dawka(BazaDanychWrapper baza, string nazwa) : base(baza, nazwa) { }
            public Szablon_Pomiary_dawka UPDATE() { _UPDATE(); return this; }
            public Szablon_Pomiary_dawka INSERT() { _INSERT(); return this; }
            public Szablon_Pomiary_dawka DELETE() { _DELETE(); return this; }
            public Szablon_Pomiary_dawka WHERE() { _WHERE(); return this; }
            public Szablon_Pomiary_dawka INFO(string text) { _INFO(text); return this; }
            public Szablon_Pomiary_dawka SELECT() { _SELECT(); return this; }
            public Szablon_Pomiary_dawka ORDER_BY() { _ORDER_BY(); return this; }
            public Row_Pomiary_dawka[] GET(int min = 0, int max = 999999999, bool allowException = false) { return Row_Pomiary_dawka._GET(_GET(min, max, allowException)); }
            public Row_Pomiary_dawka GET_ONE() { return Row_Pomiary_dawka._GET(_GET(1, 1))[0]; }
            public Row_Pomiary_dawka GET_OPTIONAL() { var r = Row_Pomiary_dawka._GET(_GET(0, 1)); return r.Length > 0 ? r[0] : null; }
            public Szablon_Pomiary_dawka Czas(double? value)
            {
                SetField("Czas", value, OleDbType.Double);
                return this;
            }
            public Szablon_Pomiary_dawka Czas(double? value, string oper)
            {
                SetField("Czas", value, OleDbType.Double, oper);
                return this;
            }
            public Szablon_Pomiary_dawka Czas(Order order)
            {
                SetOrder("Czas", order);
                return this;
            }
            public Szablon_Pomiary_dawka Czas()
            {
                AddField("Czas");
                return this;
            }
            public Szablon_Pomiary_dawka Dolaczyc(bool value)
            {
                SetField("Dolaczyc", value, OleDbType.Boolean);
                return this;
            }
            public Szablon_Pomiary_dawka Dolaczyc(bool value, string oper)
            {
                SetField("Dolaczyc", value, OleDbType.Boolean, oper);
                return this;
            }
            public Szablon_Pomiary_dawka Dolaczyc(Order order)
            {
                SetOrder("Dolaczyc", order);
                return this;
            }
            public Szablon_Pomiary_dawka Dolaczyc()
            {
                AddField("Dolaczyc");
                return this;
            }
            public Szablon_Pomiary_dawka ID_wzorcowania(int? value)
            {
                SetField("ID_wzorcowania", value, OleDbType.Integer);
                return this;
            }
            public Szablon_Pomiary_dawka ID_wzorcowania(int? value, string oper)
            {
                SetField("ID_wzorcowania", value, OleDbType.Integer, oper);
                return this;
            }
            public Szablon_Pomiary_dawka ID_wzorcowania(Order order)
            {
                SetOrder("ID_wzorcowania", order);
                return this;
            }
            public Szablon_Pomiary_dawka ID_wzorcowania()
            {
                AddField("ID_wzorcowania");
                return this;
            }
            public Szablon_Pomiary_dawka Wartosc_wzorcowa(double? value)
            {
                SetField("Wartosc_wzorcowa", value, OleDbType.Double);
                return this;
            }
            public Szablon_Pomiary_dawka Wartosc_wzorcowa(double? value, string oper)
            {
                SetField("Wartosc_wzorcowa", value, OleDbType.Double, oper);
                return this;
            }
            public Szablon_Pomiary_dawka Wartosc_wzorcowa(Order order)
            {
                SetOrder("Wartosc_wzorcowa", order);
                return this;
            }
            public Szablon_Pomiary_dawka Wartosc_wzorcowa()
            {
                AddField("Wartosc_wzorcowa");
                return this;
            }
            public Szablon_Pomiary_dawka Wskazanie(double? value)
            {
                SetField("Wskazanie", value, OleDbType.Double);
                return this;
            }
            public Szablon_Pomiary_dawka Wskazanie(double? value, string oper)
            {
                SetField("Wskazanie", value, OleDbType.Double, oper);
                return this;
            }
            public Szablon_Pomiary_dawka Wskazanie(Order order)
            {
                SetOrder("Wskazanie", order);
                return this;
            }
            public Szablon_Pomiary_dawka Wskazanie()
            {
                AddField("Wskazanie");
                return this;
            }
        }
        public class Row_Pomiary_dawka : Wiersz
        {
            public double? Czas;
            public bool Dolaczyc;
            public int? ID_wzorcowania;
            public double? Wartosc_wzorcowa;
            public double? Wskazanie;
            public Row_Pomiary_dawka(DataRow row)
            {
                _init(row, GetColsDict(row));
            }
            public Row_Pomiary_dawka(DataRow row, Dictionary<string, int> cols)
            {
                _init(row, cols);
            }
            private void _init(DataRow row, Dictionary<string, int> cols)
            {
                if (cols.ContainsKey("Czas"))
                    Czas = row.Field<double?>(cols["Czas"]);
                if (cols.ContainsKey("Dolaczyc"))
                    Dolaczyc = row.Field<bool>(cols["Dolaczyc"]);
                if (cols.ContainsKey("ID_wzorcowania"))
                    ID_wzorcowania = row.Field<int?>(cols["ID_wzorcowania"]);
                if (cols.ContainsKey("Wartosc_wzorcowa"))
                    Wartosc_wzorcowa = row.Field<double?>(cols["Wartosc_wzorcowa"]);
                if (cols.ContainsKey("Wskazanie"))
                    Wskazanie = row.Field<double?>(cols["Wskazanie"]);
            }
            internal static Row_Pomiary_dawka[] _GET(DataTable dane)
            {
                var result = new Row_Pomiary_dawka[dane.Rows.Count];
                var cols = GetColsDict(dane);
                var index = 0;
                foreach (DataRow row in dane.Rows)
                    result[index++] = new Row_Pomiary_dawka(row, cols);
                return result;
            }
        }
        public class Szablon_Pomiary_powierzchniowe : Tabela
        {
            public Szablon_Pomiary_powierzchniowe(BazaDanychWrapper baza, string nazwa) : base(baza, nazwa) { }
            public Szablon_Pomiary_powierzchniowe UPDATE() { _UPDATE(); return this; }
            public Szablon_Pomiary_powierzchniowe INSERT() { _INSERT(); return this; }
            public Szablon_Pomiary_powierzchniowe DELETE() { _DELETE(); return this; }
            public Szablon_Pomiary_powierzchniowe WHERE() { _WHERE(); return this; }
            public Szablon_Pomiary_powierzchniowe INFO(string text) { _INFO(text); return this; }
            public Szablon_Pomiary_powierzchniowe SELECT() { _SELECT(); return this; }
            public Szablon_Pomiary_powierzchniowe ORDER_BY() { _ORDER_BY(); return this; }
            public Row_Pomiary_powierzchniowe[] GET(int min = 0, int max = 999999999, bool allowException = false) { return Row_Pomiary_powierzchniowe._GET(_GET(min, max, allowException)); }
            public Row_Pomiary_powierzchniowe GET_ONE() { return Row_Pomiary_powierzchniowe._GET(_GET(1, 1))[0]; }
            public Row_Pomiary_powierzchniowe GET_OPTIONAL() { var r = Row_Pomiary_powierzchniowe._GET(_GET(0, 1)); return r.Length > 0 ? r[0] : null; }
            public Szablon_Pomiary_powierzchniowe ID_pomiaru(int value)
            {
                SetField("ID_pomiaru", value, OleDbType.Integer);
                return this;
            }
            public Szablon_Pomiary_powierzchniowe ID_pomiaru(int value, string oper)
            {
                SetField("ID_pomiaru", value, OleDbType.Integer, oper);
                return this;
            }
            public Szablon_Pomiary_powierzchniowe ID_pomiaru(Order order)
            {
                SetOrder("ID_pomiaru", order);
                return this;
            }
            public Szablon_Pomiary_powierzchniowe ID_pomiaru()
            {
                AddField("ID_pomiaru");
                return this;
            }
            public Szablon_Pomiary_powierzchniowe ID_wzorcowania(int? value)
            {
                SetField("ID_wzorcowania", value, OleDbType.Integer);
                return this;
            }
            public Szablon_Pomiary_powierzchniowe ID_wzorcowania(int? value, string oper)
            {
                SetField("ID_wzorcowania", value, OleDbType.Integer, oper);
                return this;
            }
            public Szablon_Pomiary_powierzchniowe ID_wzorcowania(Order order)
            {
                SetOrder("ID_wzorcowania", order);
                return this;
            }
            public Szablon_Pomiary_powierzchniowe ID_wzorcowania()
            {
                AddField("ID_wzorcowania");
                return this;
            }
            public Szablon_Pomiary_powierzchniowe Pomiar(double? value)
            {
                SetField("Pomiar", value, OleDbType.Double);
                return this;
            }
            public Szablon_Pomiary_powierzchniowe Pomiar(double? value, string oper)
            {
                SetField("Pomiar", value, OleDbType.Double, oper);
                return this;
            }
            public Szablon_Pomiary_powierzchniowe Pomiar(Order order)
            {
                SetOrder("Pomiar", order);
                return this;
            }
            public Szablon_Pomiary_powierzchniowe Pomiar()
            {
                AddField("Pomiar");
                return this;
            }
            public Szablon_Pomiary_powierzchniowe Tlo(double? value)
            {
                SetField("Tlo", value, OleDbType.Double);
                return this;
            }
            public Szablon_Pomiary_powierzchniowe Tlo(double? value, string oper)
            {
                SetField("Tlo", value, OleDbType.Double, oper);
                return this;
            }
            public Szablon_Pomiary_powierzchniowe Tlo(Order order)
            {
                SetOrder("Tlo", order);
                return this;
            }
            public Szablon_Pomiary_powierzchniowe Tlo()
            {
                AddField("Tlo");
                return this;
            }
        }
        public class Row_Pomiary_powierzchniowe : Wiersz
        {
            public int ID_pomiaru;
            public int? ID_wzorcowania;
            public double? Pomiar;
            public double? Tlo;
            public Row_Pomiary_powierzchniowe(DataRow row)
            {
                _init(row, GetColsDict(row));
            }
            public Row_Pomiary_powierzchniowe(DataRow row, Dictionary<string, int> cols)
            {
                _init(row, cols);
            }
            private void _init(DataRow row, Dictionary<string, int> cols)
            {
                if (cols.ContainsKey("ID_pomiaru"))
                    ID_pomiaru = row.Field<int>(cols["ID_pomiaru"]);
                if (cols.ContainsKey("ID_wzorcowania"))
                    ID_wzorcowania = row.Field<int?>(cols["ID_wzorcowania"]);
                if (cols.ContainsKey("Pomiar"))
                    Pomiar = row.Field<double?>(cols["Pomiar"]);
                if (cols.ContainsKey("Tlo"))
                    Tlo = row.Field<double?>(cols["Tlo"]);
            }
            internal static Row_Pomiary_powierzchniowe[] _GET(DataTable dane)
            {
                var result = new Row_Pomiary_powierzchniowe[dane.Rows.Count];
                var cols = GetColsDict(dane);
                var index = 0;
                foreach (DataRow row in dane.Rows)
                    result[index++] = new Row_Pomiary_powierzchniowe(row, cols);
                return result;
            }
        }
        public class Szablon_Pomiary_wzorcowe : Tabela
        {
            public Szablon_Pomiary_wzorcowe(BazaDanychWrapper baza, string nazwa) : base(baza, nazwa) { }
            public Szablon_Pomiary_wzorcowe UPDATE() { _UPDATE(); return this; }
            public Szablon_Pomiary_wzorcowe INSERT() { _INSERT(); return this; }
            public Szablon_Pomiary_wzorcowe DELETE() { _DELETE(); return this; }
            public Szablon_Pomiary_wzorcowe WHERE() { _WHERE(); return this; }
            public Szablon_Pomiary_wzorcowe INFO(string text) { _INFO(text); return this; }
            public Szablon_Pomiary_wzorcowe SELECT() { _SELECT(); return this; }
            public Szablon_Pomiary_wzorcowe ORDER_BY() { _ORDER_BY(); return this; }
            public Row_Pomiary_wzorcowe[] GET(int min = 0, int max = 999999999, bool allowException = false) { return Row_Pomiary_wzorcowe._GET(_GET(min, max, allowException)); }
            public Row_Pomiary_wzorcowe GET_ONE() { return Row_Pomiary_wzorcowe._GET(_GET(1, 1))[0]; }
            public Row_Pomiary_wzorcowe GET_OPTIONAL() { var r = Row_Pomiary_wzorcowe._GET(_GET(0, 1)); return r.Length > 0 ? r[0] : null; }
            public Szablon_Pomiary_wzorcowe ID_protokolu(short? value)
            {
                SetField("ID_protokolu", value, OleDbType.SmallInt);
                return this;
            }
            public Szablon_Pomiary_wzorcowe ID_protokolu(short? value, string oper)
            {
                SetField("ID_protokolu", value, OleDbType.SmallInt, oper);
                return this;
            }
            public Szablon_Pomiary_wzorcowe ID_protokolu(Order order)
            {
                SetOrder("ID_protokolu", order);
                return this;
            }
            public Szablon_Pomiary_wzorcowe ID_protokolu()
            {
                AddField("ID_protokolu");
                return this;
            }
            public Szablon_Pomiary_wzorcowe ID_zrodla(short? value)
            {
                SetField("ID_zrodla", value, OleDbType.SmallInt);
                return this;
            }
            public Szablon_Pomiary_wzorcowe ID_zrodla(short? value, string oper)
            {
                SetField("ID_zrodla", value, OleDbType.SmallInt, oper);
                return this;
            }
            public Szablon_Pomiary_wzorcowe ID_zrodla(Order order)
            {
                SetOrder("ID_zrodla", order);
                return this;
            }
            public Szablon_Pomiary_wzorcowe ID_zrodla()
            {
                AddField("ID_zrodla");
                return this;
            }
            public Szablon_Pomiary_wzorcowe Moc_kermy(double? value)
            {
                SetField("Moc_kermy", value, OleDbType.Double);
                return this;
            }
            public Szablon_Pomiary_wzorcowe Moc_kermy(double? value, string oper)
            {
                SetField("Moc_kermy", value, OleDbType.Double, oper);
                return this;
            }
            public Szablon_Pomiary_wzorcowe Moc_kermy(Order order)
            {
                SetOrder("Moc_kermy", order);
                return this;
            }
            public Szablon_Pomiary_wzorcowe Moc_kermy()
            {
                AddField("Moc_kermy");
                return this;
            }
            public Szablon_Pomiary_wzorcowe Niepewnosc(double? value)
            {
                SetField("Niepewnosc", value, OleDbType.Double);
                return this;
            }
            public Szablon_Pomiary_wzorcowe Niepewnosc(double? value, string oper)
            {
                SetField("Niepewnosc", value, OleDbType.Double, oper);
                return this;
            }
            public Szablon_Pomiary_wzorcowe Niepewnosc(Order order)
            {
                SetOrder("Niepewnosc", order);
                return this;
            }
            public Szablon_Pomiary_wzorcowe Niepewnosc()
            {
                AddField("Niepewnosc");
                return this;
            }
            public Szablon_Pomiary_wzorcowe Odleglosc(double? value)
            {
                SetField("Odleglosc", value, OleDbType.Double);
                return this;
            }
            public Szablon_Pomiary_wzorcowe Odleglosc(double? value, string oper)
            {
                SetField("Odleglosc", value, OleDbType.Double, oper);
                return this;
            }
            public Szablon_Pomiary_wzorcowe Odleglosc(Order order)
            {
                SetOrder("Odleglosc", order);
                return this;
            }
            public Szablon_Pomiary_wzorcowe Odleglosc()
            {
                AddField("Odleglosc");
                return this;
            }
        }
        public class Row_Pomiary_wzorcowe : Wiersz
        {
            public short? ID_protokolu;
            public short? ID_zrodla;
            public double? Moc_kermy;
            public double? Niepewnosc;
            public double? Odleglosc;
            public Row_Pomiary_wzorcowe(DataRow row)
            {
                _init(row, GetColsDict(row));
            }
            public Row_Pomiary_wzorcowe(DataRow row, Dictionary<string, int> cols)
            {
                _init(row, cols);
            }
            private void _init(DataRow row, Dictionary<string, int> cols)
            {
                if (cols.ContainsKey("ID_protokolu"))
                    ID_protokolu = row.Field<short?>(cols["ID_protokolu"]);
                if (cols.ContainsKey("ID_zrodla"))
                    ID_zrodla = row.Field<short?>(cols["ID_zrodla"]);
                if (cols.ContainsKey("Moc_kermy"))
                    Moc_kermy = row.Field<double?>(cols["Moc_kermy"]);
                if (cols.ContainsKey("Niepewnosc"))
                    Niepewnosc = row.Field<double?>(cols["Niepewnosc"]);
                if (cols.ContainsKey("Odleglosc"))
                    Odleglosc = row.Field<double?>(cols["Odleglosc"]);
            }
            internal static Row_Pomiary_wzorcowe[] _GET(DataTable dane)
            {
                var result = new Row_Pomiary_wzorcowe[dane.Rows.Count];
                var cols = GetColsDict(dane);
                var index = 0;
                foreach (DataRow row in dane.Rows)
                    result[index++] = new Row_Pomiary_wzorcowe(row, cols);
                return result;
            }
        }
        public class Szablon_Protokoly_kalibracji_lawy : Tabela
        {
            public Szablon_Protokoly_kalibracji_lawy(BazaDanychWrapper baza, string nazwa) : base(baza, nazwa) { }
            public Szablon_Protokoly_kalibracji_lawy UPDATE() { _UPDATE(); return this; }
            public Szablon_Protokoly_kalibracji_lawy INSERT() { _INSERT(); return this; }
            public Szablon_Protokoly_kalibracji_lawy DELETE() { _DELETE(); return this; }
            public Szablon_Protokoly_kalibracji_lawy WHERE() { _WHERE(); return this; }
            public Szablon_Protokoly_kalibracji_lawy INFO(string text) { _INFO(text); return this; }
            public Szablon_Protokoly_kalibracji_lawy SELECT() { _SELECT(); return this; }
            public Szablon_Protokoly_kalibracji_lawy ORDER_BY() { _ORDER_BY(); return this; }
            public Row_Protokoly_kalibracji_lawy[] GET(int min = 0, int max = 999999999, bool allowException = false) { return Row_Protokoly_kalibracji_lawy._GET(_GET(min, max, allowException)); }
            public Row_Protokoly_kalibracji_lawy GET_ONE() { return Row_Protokoly_kalibracji_lawy._GET(_GET(1, 1))[0]; }
            public Row_Protokoly_kalibracji_lawy GET_OPTIONAL() { var r = Row_Protokoly_kalibracji_lawy._GET(_GET(0, 1)); return r.Length > 0 ? r[0] : null; }
            public Szablon_Protokoly_kalibracji_lawy Data_kalibracji(DateTime? value)
            {
                SetField("Data_kalibracji", value, OleDbType.Date);
                return this;
            }
            public Szablon_Protokoly_kalibracji_lawy Data_kalibracji(DateTime? value, string oper)
            {
                SetField("Data_kalibracji", value, OleDbType.Date, oper);
                return this;
            }
            public Szablon_Protokoly_kalibracji_lawy Data_kalibracji(Order order)
            {
                SetOrder("Data_kalibracji", order);
                return this;
            }
            public Szablon_Protokoly_kalibracji_lawy Data_kalibracji()
            {
                AddField("Data_kalibracji");
                return this;
            }
            public Szablon_Protokoly_kalibracji_lawy ID_protokolu(short? value)
            {
                SetField("ID_protokolu", value, OleDbType.SmallInt);
                return this;
            }
            public Szablon_Protokoly_kalibracji_lawy ID_protokolu(short? value, string oper)
            {
                SetField("ID_protokolu", value, OleDbType.SmallInt, oper);
                return this;
            }
            public Szablon_Protokoly_kalibracji_lawy ID_protokolu(Order order)
            {
                SetOrder("ID_protokolu", order);
                return this;
            }
            public Szablon_Protokoly_kalibracji_lawy ID_protokolu()
            {
                AddField("ID_protokolu");
                return this;
            }
            public Szablon_Protokoly_kalibracji_lawy Nazwa(string value)
            {
                SetField("Nazwa", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Protokoly_kalibracji_lawy Nazwa(string value, string oper)
            {
                SetField("Nazwa", value, OleDbType.WChar, oper);
                return this;
            }
            public Szablon_Protokoly_kalibracji_lawy Nazwa(Order order)
            {
                SetOrder("Nazwa", order);
                return this;
            }
            public Szablon_Protokoly_kalibracji_lawy Nazwa()
            {
                AddField("Nazwa");
                return this;
            }
        }
        public class Row_Protokoly_kalibracji_lawy : Wiersz
        {
            public DateTime? Data_kalibracji;
            public short? ID_protokolu;
            public string Nazwa;
            public Row_Protokoly_kalibracji_lawy(DataRow row)
            {
                _init(row, GetColsDict(row));
            }
            public Row_Protokoly_kalibracji_lawy(DataRow row, Dictionary<string, int> cols)
            {
                _init(row, cols);
            }
            private void _init(DataRow row, Dictionary<string, int> cols)
            {
                if (cols.ContainsKey("Data_kalibracji"))
                    Data_kalibracji = row.Field<DateTime?>(cols["Data_kalibracji"]);
                if (cols.ContainsKey("ID_protokolu"))
                    ID_protokolu = row.Field<short?>(cols["ID_protokolu"]);
                if (cols.ContainsKey("Nazwa"))
                    Nazwa = row.Field<string>(cols["Nazwa"]);
            }
            internal static Row_Protokoly_kalibracji_lawy[] _GET(DataTable dane)
            {
                var result = new Row_Protokoly_kalibracji_lawy[dane.Rows.Count];
                var cols = GetColsDict(dane);
                var index = 0;
                foreach (DataRow row in dane.Rows)
                    result[index++] = new Row_Protokoly_kalibracji_lawy(row, cols);
                return result;
            }
        }
        public class Szablon_Slownik : Tabela
        {
            public Szablon_Slownik(BazaDanychWrapper baza, string nazwa) : base(baza, nazwa) { }
            public Szablon_Slownik UPDATE() { _UPDATE(); return this; }
            public Szablon_Slownik INSERT() { _INSERT(); return this; }
            public Szablon_Slownik DELETE() { _DELETE(); return this; }
            public Szablon_Slownik WHERE() { _WHERE(); return this; }
            public Szablon_Slownik INFO(string text) { _INFO(text); return this; }
            public Szablon_Slownik SELECT() { _SELECT(); return this; }
            public Szablon_Slownik ORDER_BY() { _ORDER_BY(); return this; }
            public Row_Slownik[] GET(int min = 0, int max = 999999999, bool allowException = false) { return Row_Slownik._GET(_GET(min, max, allowException)); }
            public Row_Slownik GET_ONE() { return Row_Slownik._GET(_GET(1, 1))[0]; }
            public Row_Slownik GET_OPTIONAL() { var r = Row_Slownik._GET(_GET(0, 1)); return r.Length > 0 ? r[0] : null; }
            public Szablon_Slownik EN(string value)
            {
                SetField("EN", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Slownik EN(string value, string oper)
            {
                SetField("EN", value, OleDbType.WChar, oper);
                return this;
            }
            public Szablon_Slownik EN(Order order)
            {
                SetOrder("EN", order);
                return this;
            }
            public Szablon_Slownik EN()
            {
                AddField("EN");
                return this;
            }
            public Szablon_Slownik PL(string value)
            {
                SetField("PL", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Slownik PL(string value, string oper)
            {
                SetField("PL", value, OleDbType.WChar, oper);
                return this;
            }
            public Szablon_Slownik PL(Order order)
            {
                SetOrder("PL", order);
                return this;
            }
            public Szablon_Slownik PL()
            {
                AddField("PL");
                return this;
            }
        }
        public class Row_Slownik : Wiersz
        {
            public string EN;
            public string PL;
            public Row_Slownik(DataRow row)
            {
                _init(row, GetColsDict(row));
            }
            public Row_Slownik(DataRow row, Dictionary<string, int> cols)
            {
                _init(row, cols);
            }
            private void _init(DataRow row, Dictionary<string, int> cols)
            {
                if (cols.ContainsKey("EN"))
                    EN = row.Field<string>(cols["EN"]);
                if (cols.ContainsKey("PL"))
                    PL = row.Field<string>(cols["PL"]);
            }
            internal static Row_Slownik[] _GET(DataTable dane)
            {
                var result = new Row_Slownik[dane.Rows.Count];
                var cols = GetColsDict(dane);
                var index = 0;
                foreach (DataRow row in dane.Rows)
                    result[index++] = new Row_Slownik(row, cols);
                return result;
            }
        }
        public class Szablon_Sondy : Tabela
        {
            public Szablon_Sondy(BazaDanychWrapper baza, string nazwa) : base(baza, nazwa) { }
            public Szablon_Sondy UPDATE() { _UPDATE(); return this; }
            public Szablon_Sondy INSERT() { _INSERT(); return this; }
            public Szablon_Sondy DELETE() { _DELETE(); return this; }
            public Szablon_Sondy WHERE() { _WHERE(); return this; }
            public Szablon_Sondy INFO(string text) { _INFO(text); return this; }
            public Szablon_Sondy SELECT() { _SELECT(); return this; }
            public Szablon_Sondy ORDER_BY() { _ORDER_BY(); return this; }
            public Row_Sondy[] GET(int min = 0, int max = 999999999, bool allowException = false) { return Row_Sondy._GET(_GET(min, max, allowException)); }
            public Row_Sondy GET_ONE() { return Row_Sondy._GET(_GET(1, 1))[0]; }
            public Row_Sondy GET_OPTIONAL() { var r = Row_Sondy._GET(_GET(0, 1)); return r.Length > 0 ? r[0] : null; }
            public Szablon_Sondy ID_dozymetru(int? value)
            {
                SetField("ID_dozymetru", value, OleDbType.Integer);
                return this;
            }
            public Szablon_Sondy ID_dozymetru(int? value, string oper)
            {
                SetField("ID_dozymetru", value, OleDbType.Integer, oper);
                return this;
            }
            public Szablon_Sondy ID_dozymetru(Order order)
            {
                SetOrder("ID_dozymetru", order);
                return this;
            }
            public Szablon_Sondy ID_dozymetru()
            {
                AddField("ID_dozymetru");
                return this;
            }
            public Szablon_Sondy ID_sondy(int value)
            {
                SetField("ID_sondy", value, OleDbType.Integer);
                return this;
            }
            public Szablon_Sondy ID_sondy(int value, string oper)
            {
                SetField("ID_sondy", value, OleDbType.Integer, oper);
                return this;
            }
            public Szablon_Sondy ID_sondy(Order order)
            {
                SetOrder("ID_sondy", order);
                return this;
            }
            public Szablon_Sondy ID_sondy()
            {
                AddField("ID_sondy");
                return this;
            }
            public Szablon_Sondy Nr_fabryczny(string value)
            {
                SetField("Nr_fabryczny", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Sondy Nr_fabryczny(string value, string oper)
            {
                SetField("Nr_fabryczny", value, OleDbType.WChar, oper);
                return this;
            }
            public Szablon_Sondy Nr_fabryczny(Order order)
            {
                SetOrder("Nr_fabryczny", order);
                return this;
            }
            public Szablon_Sondy Nr_fabryczny()
            {
                AddField("Nr_fabryczny");
                return this;
            }
            public Szablon_Sondy Typ(string value)
            {
                SetField("Typ", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Sondy Typ(string value, string oper)
            {
                SetField("Typ", value, OleDbType.WChar, oper);
                return this;
            }
            public Szablon_Sondy Typ(Order order)
            {
                SetOrder("Typ", order);
                return this;
            }
            public Szablon_Sondy Typ()
            {
                AddField("Typ");
                return this;
            }
        }
        public class Row_Sondy : Wiersz
        {
            public int? ID_dozymetru;
            public int ID_sondy;
            public string Nr_fabryczny;
            public string Typ;
            public Row_Sondy(DataRow row)
            {
                _init(row, GetColsDict(row));
            }
            public Row_Sondy(DataRow row, Dictionary<string, int> cols)
            {
                _init(row, cols);
            }
            private void _init(DataRow row, Dictionary<string, int> cols)
            {
                if (cols.ContainsKey("ID_dozymetru"))
                    ID_dozymetru = row.Field<int?>(cols["ID_dozymetru"]);
                if (cols.ContainsKey("ID_sondy"))
                    ID_sondy = row.Field<int>(cols["ID_sondy"]);
                if (cols.ContainsKey("Nr_fabryczny"))
                    Nr_fabryczny = row.Field<string>(cols["Nr_fabryczny"]);
                if (cols.ContainsKey("Typ"))
                    Typ = row.Field<string>(cols["Typ"]);
            }
            internal static Row_Sondy[] _GET(DataTable dane)
            {
                var result = new Row_Sondy[dane.Rows.Count];
                var cols = GetColsDict(dane);
                var index = 0;
                foreach (DataRow row in dane.Rows)
                    result[index++] = new Row_Sondy(row, cols);
                return result;
            }
        }
        public class Szablon_Stale : Tabela
        {
            public Szablon_Stale(BazaDanychWrapper baza, string nazwa) : base(baza, nazwa) { }
            public Szablon_Stale UPDATE() { _UPDATE(); return this; }
            public Szablon_Stale INSERT() { _INSERT(); return this; }
            public Szablon_Stale DELETE() { _DELETE(); return this; }
            public Szablon_Stale WHERE() { _WHERE(); return this; }
            public Szablon_Stale INFO(string text) { _INFO(text); return this; }
            public Szablon_Stale SELECT() { _SELECT(); return this; }
            public Szablon_Stale ORDER_BY() { _ORDER_BY(); return this; }
            public Row_Stale[] GET(int min = 0, int max = 999999999, bool allowException = false) { return Row_Stale._GET(_GET(min, max, allowException)); }
            public Row_Stale GET_ONE() { return Row_Stale._GET(_GET(1, 1))[0]; }
            public Row_Stale GET_OPTIONAL() { var r = Row_Stale._GET(_GET(0, 1)); return r.Length > 0 ? r[0] : null; }
            public Szablon_Stale Nazwa(string value)
            {
                SetField("Nazwa", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Stale Nazwa(string value, string oper)
            {
                SetField("Nazwa", value, OleDbType.WChar, oper);
                return this;
            }
            public Szablon_Stale Nazwa(Order order)
            {
                SetOrder("Nazwa", order);
                return this;
            }
            public Szablon_Stale Nazwa()
            {
                AddField("Nazwa");
                return this;
            }
            public Szablon_Stale Uwagi(string value)
            {
                SetField("Uwagi", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Stale Uwagi(string value, string oper)
            {
                SetField("Uwagi", value, OleDbType.WChar, oper);
                return this;
            }
            public Szablon_Stale Uwagi(Order order)
            {
                SetOrder("Uwagi", order);
                return this;
            }
            public Szablon_Stale Uwagi()
            {
                AddField("Uwagi");
                return this;
            }
            public Szablon_Stale Wartosc(double? value)
            {
                SetField("Wartosc", value, OleDbType.Double);
                return this;
            }
            public Szablon_Stale Wartosc(double? value, string oper)
            {
                SetField("Wartosc", value, OleDbType.Double, oper);
                return this;
            }
            public Szablon_Stale Wartosc(Order order)
            {
                SetOrder("Wartosc", order);
                return this;
            }
            public Szablon_Stale Wartosc()
            {
                AddField("Wartosc");
                return this;
            }
        }
        public class Row_Stale : Wiersz
        {
            public string Nazwa;
            public string Uwagi;
            public double? Wartosc;
            public Row_Stale(DataRow row)
            {
                _init(row, GetColsDict(row));
            }
            public Row_Stale(DataRow row, Dictionary<string, int> cols)
            {
                _init(row, cols);
            }
            private void _init(DataRow row, Dictionary<string, int> cols)
            {
                if (cols.ContainsKey("Nazwa"))
                    Nazwa = row.Field<string>(cols["Nazwa"]);
                if (cols.ContainsKey("Uwagi"))
                    Uwagi = row.Field<string>(cols["Uwagi"]);
                if (cols.ContainsKey("Wartosc"))
                    Wartosc = row.Field<double?>(cols["Wartosc"]);
            }
            internal static Row_Stale[] _GET(DataTable dane)
            {
                var result = new Row_Stale[dane.Rows.Count];
                var cols = GetColsDict(dane);
                var index = 0;
                foreach (DataRow row in dane.Rows)
                    result[index++] = new Row_Stale(row, cols);
                return result;
            }
        }
        public class Szablon_Swiadectwo : Tabela
        {
            public Szablon_Swiadectwo(BazaDanychWrapper baza, string nazwa) : base(baza, nazwa) { }
            public Szablon_Swiadectwo UPDATE() { _UPDATE(); return this; }
            public Szablon_Swiadectwo INSERT() { _INSERT(); return this; }
            public Szablon_Swiadectwo DELETE() { _DELETE(); return this; }
            public Szablon_Swiadectwo WHERE() { _WHERE(); return this; }
            public Szablon_Swiadectwo INFO(string text) { _INFO(text); return this; }
            public Szablon_Swiadectwo SELECT() { _SELECT(); return this; }
            public Szablon_Swiadectwo ORDER_BY() { _ORDER_BY(); return this; }
            public Row_Swiadectwo[] GET(int min = 0, int max = 999999999, bool allowException = false) { return Row_Swiadectwo._GET(_GET(min, max, allowException)); }
            public Row_Swiadectwo GET_ONE() { return Row_Swiadectwo._GET(_GET(1, 1))[0]; }
            public Row_Swiadectwo GET_OPTIONAL() { var r = Row_Swiadectwo._GET(_GET(0, 1)); return r.Length > 0 ? r[0] : null; }
            public Szablon_Swiadectwo Autoryzowal(string value)
            {
                SetField("Autoryzowal", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Swiadectwo Autoryzowal(string value, string oper)
            {
                SetField("Autoryzowal", value, OleDbType.WChar, oper);
                return this;
            }
            public Szablon_Swiadectwo Autoryzowal(Order order)
            {
                SetOrder("Autoryzowal", order);
                return this;
            }
            public Szablon_Swiadectwo Autoryzowal()
            {
                AddField("Autoryzowal");
                return this;
            }
            public Szablon_Swiadectwo Data_wykonania(DateTime? value)
            {
                SetField("Data_wykonania", value, OleDbType.Date);
                return this;
            }
            public Szablon_Swiadectwo Data_wykonania(DateTime? value, string oper)
            {
                SetField("Data_wykonania", value, OleDbType.Date, oper);
                return this;
            }
            public Szablon_Swiadectwo Data_wykonania(Order order)
            {
                SetOrder("Data_wykonania", order);
                return this;
            }
            public Szablon_Swiadectwo Data_wykonania()
            {
                AddField("Data_wykonania");
                return this;
            }
            public Szablon_Swiadectwo Data_wystawienia(DateTime? value)
            {
                SetField("Data_wystawienia", value, OleDbType.Date);
                return this;
            }
            public Szablon_Swiadectwo Data_wystawienia(DateTime? value, string oper)
            {
                SetField("Data_wystawienia", value, OleDbType.Date, oper);
                return this;
            }
            public Szablon_Swiadectwo Data_wystawienia(Order order)
            {
                SetOrder("Data_wystawienia", order);
                return this;
            }
            public Szablon_Swiadectwo Data_wystawienia()
            {
                AddField("Data_wystawienia");
                return this;
            }
            public Szablon_Swiadectwo Id_karty(int value)
            {
                SetField("Id_karty", value, OleDbType.Integer);
                return this;
            }
            public Szablon_Swiadectwo Id_karty(int value, string oper)
            {
                SetField("Id_karty", value, OleDbType.Integer, oper);
                return this;
            }
            public Szablon_Swiadectwo Id_karty(Order order)
            {
                SetOrder("Id_karty", order);
                return this;
            }
            public Szablon_Swiadectwo Id_karty()
            {
                AddField("Id_karty");
                return this;
            }
            public Szablon_Swiadectwo Poprawa(bool value)
            {
                SetField("Poprawa", value, OleDbType.Boolean);
                return this;
            }
            public Szablon_Swiadectwo Poprawa(bool value, string oper)
            {
                SetField("Poprawa", value, OleDbType.Boolean, oper);
                return this;
            }
            public Szablon_Swiadectwo Poprawa(Order order)
            {
                SetOrder("Poprawa", order);
                return this;
            }
            public Szablon_Swiadectwo Poprawa()
            {
                AddField("Poprawa");
                return this;
            }
            public Szablon_Swiadectwo Uwaga(string value)
            {
                SetField("Uwaga", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Swiadectwo Uwaga(string value, string oper)
            {
                SetField("Uwaga", value, OleDbType.WChar, oper);
                return this;
            }
            public Szablon_Swiadectwo Uwaga(Order order)
            {
                SetOrder("Uwaga", order);
                return this;
            }
            public Szablon_Swiadectwo Uwaga()
            {
                AddField("Uwaga");
                return this;
            }
            public Szablon_Swiadectwo UwagaD(string value)
            {
                SetField("UwagaD", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Swiadectwo UwagaD(string value, string oper)
            {
                SetField("UwagaD", value, OleDbType.WChar, oper);
                return this;
            }
            public Szablon_Swiadectwo UwagaD(Order order)
            {
                SetOrder("UwagaD", order);
                return this;
            }
            public Szablon_Swiadectwo UwagaD()
            {
                AddField("UwagaD");
                return this;
            }
            public Szablon_Swiadectwo UwagaMD(string value)
            {
                SetField("UwagaMD", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Swiadectwo UwagaMD(string value, string oper)
            {
                SetField("UwagaMD", value, OleDbType.WChar, oper);
                return this;
            }
            public Szablon_Swiadectwo UwagaMD(Order order)
            {
                SetOrder("UwagaMD", order);
                return this;
            }
            public Szablon_Swiadectwo UwagaMD()
            {
                AddField("UwagaMD");
                return this;
            }
            public Szablon_Swiadectwo UwagaS(string value)
            {
                SetField("UwagaS", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Swiadectwo UwagaS(string value, string oper)
            {
                SetField("UwagaS", value, OleDbType.WChar, oper);
                return this;
            }
            public Szablon_Swiadectwo UwagaS(Order order)
            {
                SetOrder("UwagaS", order);
                return this;
            }
            public Szablon_Swiadectwo UwagaS()
            {
                AddField("UwagaS");
                return this;
            }
            public Szablon_Swiadectwo UwagaSD(string value)
            {
                SetField("UwagaSD", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Swiadectwo UwagaSD(string value, string oper)
            {
                SetField("UwagaSD", value, OleDbType.WChar, oper);
                return this;
            }
            public Szablon_Swiadectwo UwagaSD(Order order)
            {
                SetOrder("UwagaSD", order);
                return this;
            }
            public Szablon_Swiadectwo UwagaSD()
            {
                AddField("UwagaSD");
                return this;
            }
            public Szablon_Swiadectwo UwagaSMD(string value)
            {
                SetField("UwagaSMD", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Swiadectwo UwagaSMD(string value, string oper)
            {
                SetField("UwagaSMD", value, OleDbType.WChar, oper);
                return this;
            }
            public Szablon_Swiadectwo UwagaSMD(Order order)
            {
                SetOrder("UwagaSMD", order);
                return this;
            }
            public Szablon_Swiadectwo UwagaSMD()
            {
                AddField("UwagaSMD");
                return this;
            }
            public Szablon_Swiadectwo Waznosc_dwa_lata(bool value)
            {
                SetField("Waznosc_dwa_lata", value, OleDbType.Boolean);
                return this;
            }
            public Szablon_Swiadectwo Waznosc_dwa_lata(bool value, string oper)
            {
                SetField("Waznosc_dwa_lata", value, OleDbType.Boolean, oper);
                return this;
            }
            public Szablon_Swiadectwo Waznosc_dwa_lata(Order order)
            {
                SetOrder("Waznosc_dwa_lata", order);
                return this;
            }
            public Szablon_Swiadectwo Waznosc_dwa_lata()
            {
                AddField("Waznosc_dwa_lata");
                return this;
            }
        }
        public class Row_Swiadectwo : Wiersz
        {
            public string Autoryzowal;
            public DateTime? Data_wykonania;
            public DateTime? Data_wystawienia;
            public int Id_karty;
            public bool Poprawa;
            public string Uwaga;
            public string UwagaD;
            public string UwagaMD;
            public string UwagaS;
            public string UwagaSD;
            public string UwagaSMD;
            public bool Waznosc_dwa_lata;
            public Row_Swiadectwo(DataRow row)
            {
                _init(row, GetColsDict(row));
            }
            public Row_Swiadectwo(DataRow row, Dictionary<string, int> cols)
            {
                _init(row, cols);
            }
            private void _init(DataRow row, Dictionary<string, int> cols)
            {
                if (cols.ContainsKey("Autoryzowal"))
                    Autoryzowal = row.Field<string>(cols["Autoryzowal"]);
                if (cols.ContainsKey("Data_wykonania"))
                    Data_wykonania = row.Field<DateTime?>(cols["Data_wykonania"]);
                if (cols.ContainsKey("Data_wystawienia"))
                    Data_wystawienia = row.Field<DateTime?>(cols["Data_wystawienia"]);
                if (cols.ContainsKey("Id_karty"))
                    Id_karty = row.Field<int>(cols["Id_karty"]);
                if (cols.ContainsKey("Poprawa"))
                    Poprawa = row.Field<bool>(cols["Poprawa"]);
                if (cols.ContainsKey("Uwaga"))
                    Uwaga = row.Field<string>(cols["Uwaga"]);
                if (cols.ContainsKey("UwagaD"))
                    UwagaD = row.Field<string>(cols["UwagaD"]);
                if (cols.ContainsKey("UwagaMD"))
                    UwagaMD = row.Field<string>(cols["UwagaMD"]);
                if (cols.ContainsKey("UwagaS"))
                    UwagaS = row.Field<string>(cols["UwagaS"]);
                if (cols.ContainsKey("UwagaSD"))
                    UwagaSD = row.Field<string>(cols["UwagaSD"]);
                if (cols.ContainsKey("UwagaSMD"))
                    UwagaSMD = row.Field<string>(cols["UwagaSMD"]);
                if (cols.ContainsKey("Waznosc_dwa_lata"))
                    Waznosc_dwa_lata = row.Field<bool>(cols["Waznosc_dwa_lata"]);
            }
            internal static Row_Swiadectwo[] _GET(DataTable dane)
            {
                var result = new Row_Swiadectwo[dane.Rows.Count];
                var cols = GetColsDict(dane);
                var index = 0;
                foreach (DataRow row in dane.Rows)
                    result[index++] = new Row_Swiadectwo(row, cols);
                return result;
            }
        }
        public class Szablon_Sygnalizacja : Tabela
        {
            public Szablon_Sygnalizacja(BazaDanychWrapper baza, string nazwa) : base(baza, nazwa) { }
            public Szablon_Sygnalizacja UPDATE() { _UPDATE(); return this; }
            public Szablon_Sygnalizacja INSERT() { _INSERT(); return this; }
            public Szablon_Sygnalizacja DELETE() { _DELETE(); return this; }
            public Szablon_Sygnalizacja WHERE() { _WHERE(); return this; }
            public Szablon_Sygnalizacja INFO(string text) { _INFO(text); return this; }
            public Szablon_Sygnalizacja SELECT() { _SELECT(); return this; }
            public Szablon_Sygnalizacja ORDER_BY() { _ORDER_BY(); return this; }
            public Row_Sygnalizacja[] GET(int min = 0, int max = 999999999, bool allowException = false) { return Row_Sygnalizacja._GET(_GET(min, max, allowException)); }
            public Row_Sygnalizacja GET_ONE() { return Row_Sygnalizacja._GET(_GET(1, 1))[0]; }
            public Row_Sygnalizacja GET_OPTIONAL() { var r = Row_Sygnalizacja._GET(_GET(0, 1)); return r.Length > 0 ? r[0] : null; }
            public Szablon_Sygnalizacja ID_wzorcowania(int? value)
            {
                SetField("ID_wzorcowania", value, OleDbType.Integer);
                return this;
            }
            public Szablon_Sygnalizacja ID_wzorcowania(int? value, string oper)
            {
                SetField("ID_wzorcowania", value, OleDbType.Integer, oper);
                return this;
            }
            public Szablon_Sygnalizacja ID_wzorcowania(Order order)
            {
                SetOrder("ID_wzorcowania", order);
                return this;
            }
            public Szablon_Sygnalizacja ID_wzorcowania()
            {
                AddField("ID_wzorcowania");
                return this;
            }
            public Szablon_Sygnalizacja Niepewnosc(double? value)
            {
                SetField("Niepewnosc", value, OleDbType.Double);
                return this;
            }
            public Szablon_Sygnalizacja Niepewnosc(double? value, string oper)
            {
                SetField("Niepewnosc", value, OleDbType.Double, oper);
                return this;
            }
            public Szablon_Sygnalizacja Niepewnosc(Order order)
            {
                SetOrder("Niepewnosc", order);
                return this;
            }
            public Szablon_Sygnalizacja Niepewnosc()
            {
                AddField("Niepewnosc");
                return this;
            }
            public Szablon_Sygnalizacja Niepewnosc_Wspolczynnika(double value)
            {
                SetField("Niepewnosc_Wspolczynnika", value, OleDbType.Double);
                return this;
            }
            public Szablon_Sygnalizacja Niepewnosc_Wspolczynnika(double value, string oper)
            {
                SetField("Niepewnosc_Wspolczynnika", value, OleDbType.Double, oper);
                return this;
            }
            public Szablon_Sygnalizacja Niepewnosc_Wspolczynnika(Order order)
            {
                SetOrder("Niepewnosc_Wspolczynnika", order);
                return this;
            }
            public Szablon_Sygnalizacja Niepewnosc_Wspolczynnika()
            {
                AddField("Niepewnosc_Wspolczynnika");
                return this;
            }
            public Szablon_Sygnalizacja odleglosc1(double? value)
            {
                SetField("odleglosc1", value, OleDbType.Double);
                return this;
            }
            public Szablon_Sygnalizacja odleglosc1(double? value, string oper)
            {
                SetField("odleglosc1", value, OleDbType.Double, oper);
                return this;
            }
            public Szablon_Sygnalizacja odleglosc1(Order order)
            {
                SetOrder("odleglosc1", order);
                return this;
            }
            public Szablon_Sygnalizacja odleglosc1()
            {
                AddField("odleglosc1");
                return this;
            }
            public Szablon_Sygnalizacja odleglosc2(double? value)
            {
                SetField("odleglosc2", value, OleDbType.Double);
                return this;
            }
            public Szablon_Sygnalizacja odleglosc2(double? value, string oper)
            {
                SetField("odleglosc2", value, OleDbType.Double, oper);
                return this;
            }
            public Szablon_Sygnalizacja odleglosc2(Order order)
            {
                SetOrder("odleglosc2", order);
                return this;
            }
            public Szablon_Sygnalizacja odleglosc2()
            {
                AddField("odleglosc2");
                return this;
            }
            public Szablon_Sygnalizacja Prog(double? value)
            {
                SetField("Prog", value, OleDbType.Double);
                return this;
            }
            public Szablon_Sygnalizacja Prog(double? value, string oper)
            {
                SetField("Prog", value, OleDbType.Double, oper);
                return this;
            }
            public Szablon_Sygnalizacja Prog(Order order)
            {
                SetOrder("Prog", order);
                return this;
            }
            public Szablon_Sygnalizacja Prog()
            {
                AddField("Prog");
                return this;
            }
            public Szablon_Sygnalizacja Uwagi(string value)
            {
                SetField("Uwagi", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Sygnalizacja Uwagi(string value, string oper)
            {
                SetField("Uwagi", value, OleDbType.WChar, oper);
                return this;
            }
            public Szablon_Sygnalizacja Uwagi(Order order)
            {
                SetOrder("Uwagi", order);
                return this;
            }
            public Szablon_Sygnalizacja Uwagi()
            {
                AddField("Uwagi");
                return this;
            }
            public Szablon_Sygnalizacja Wartosc_zmierzona(double? value)
            {
                SetField("Wartosc_zmierzona", value, OleDbType.Double);
                return this;
            }
            public Szablon_Sygnalizacja Wartosc_zmierzona(double? value, string oper)
            {
                SetField("Wartosc_zmierzona", value, OleDbType.Double, oper);
                return this;
            }
            public Szablon_Sygnalizacja Wartosc_zmierzona(Order order)
            {
                SetOrder("Wartosc_zmierzona", order);
                return this;
            }
            public Szablon_Sygnalizacja Wartosc_zmierzona()
            {
                AddField("Wartosc_zmierzona");
                return this;
            }
            public Szablon_Sygnalizacja Wspolczynnik(double value)
            {
                SetField("Wspolczynnik", value, OleDbType.Double);
                return this;
            }
            public Szablon_Sygnalizacja Wspolczynnik(double value, string oper)
            {
                SetField("Wspolczynnik", value, OleDbType.Double, oper);
                return this;
            }
            public Szablon_Sygnalizacja Wspolczynnik(Order order)
            {
                SetOrder("Wspolczynnik", order);
                return this;
            }
            public Szablon_Sygnalizacja Wspolczynnik()
            {
                AddField("Wspolczynnik");
                return this;
            }
            public Szablon_Sygnalizacja zrodlo1(double? value)
            {
                SetField("zrodlo1", value, OleDbType.Double);
                return this;
            }
            public Szablon_Sygnalizacja zrodlo1(double? value, string oper)
            {
                SetField("zrodlo1", value, OleDbType.Double, oper);
                return this;
            }
            public Szablon_Sygnalizacja zrodlo1(Order order)
            {
                SetOrder("zrodlo1", order);
                return this;
            }
            public Szablon_Sygnalizacja zrodlo1()
            {
                AddField("zrodlo1");
                return this;
            }
            public Szablon_Sygnalizacja zrodlo2(double? value)
            {
                SetField("zrodlo2", value, OleDbType.Double);
                return this;
            }
            public Szablon_Sygnalizacja zrodlo2(double? value, string oper)
            {
                SetField("zrodlo2", value, OleDbType.Double, oper);
                return this;
            }
            public Szablon_Sygnalizacja zrodlo2(Order order)
            {
                SetOrder("zrodlo2", order);
                return this;
            }
            public Szablon_Sygnalizacja zrodlo2()
            {
                AddField("zrodlo2");
                return this;
            }
        }
        public class Row_Sygnalizacja : Wiersz
        {
            public int? ID_wzorcowania;
            public double? Niepewnosc;
            public double Niepewnosc_Wspolczynnika;
            public double? odleglosc1;
            public double? odleglosc2;
            public double? Prog;
            public string Uwagi;
            public double? Wartosc_zmierzona;
            public double Wspolczynnik;
            public double? zrodlo1;
            public double? zrodlo2;
            public Row_Sygnalizacja(DataRow row)
            {
                _init(row, GetColsDict(row));
            }
            public Row_Sygnalizacja(DataRow row, Dictionary<string, int> cols)
            {
                _init(row, cols);
            }
            private void _init(DataRow row, Dictionary<string, int> cols)
            {
                if (cols.ContainsKey("ID_wzorcowania"))
                    ID_wzorcowania = row.Field<int?>(cols["ID_wzorcowania"]);
                if (cols.ContainsKey("Niepewnosc"))
                    Niepewnosc = row.Field<double?>(cols["Niepewnosc"]);
                if (cols.ContainsKey("Niepewnosc_Wspolczynnika"))
                    Niepewnosc_Wspolczynnika = row.Field<double>(cols["Niepewnosc_Wspolczynnika"]);
                if (cols.ContainsKey("odleglosc1"))
                    odleglosc1 = row.Field<double?>(cols["odleglosc1"]);
                if (cols.ContainsKey("odleglosc2"))
                    odleglosc2 = row.Field<double?>(cols["odleglosc2"]);
                if (cols.ContainsKey("Prog"))
                    Prog = row.Field<double?>(cols["Prog"]);
                if (cols.ContainsKey("Uwagi"))
                    Uwagi = row.Field<string>(cols["Uwagi"]);
                if (cols.ContainsKey("Wartosc_zmierzona"))
                    Wartosc_zmierzona = row.Field<double?>(cols["Wartosc_zmierzona"]);
                if (cols.ContainsKey("Wspolczynnik"))
                    Wspolczynnik = row.Field<double>(cols["Wspolczynnik"]);
                if (cols.ContainsKey("zrodlo1"))
                    zrodlo1 = row.Field<double?>(cols["zrodlo1"]);
                if (cols.ContainsKey("zrodlo2"))
                    zrodlo2 = row.Field<double?>(cols["zrodlo2"]);
            }
            internal static Row_Sygnalizacja[] _GET(DataTable dane)
            {
                var result = new Row_Sygnalizacja[dane.Rows.Count];
                var cols = GetColsDict(dane);
                var index = 0;
                foreach (DataRow row in dane.Rows)
                    result[index++] = new Row_Sygnalizacja(row, cols);
                return result;
            }
        }
        public class Szablon_Sygnalizacja_dawka : Tabela
        {
            public Szablon_Sygnalizacja_dawka(BazaDanychWrapper baza, string nazwa) : base(baza, nazwa) { }
            public Szablon_Sygnalizacja_dawka UPDATE() { _UPDATE(); return this; }
            public Szablon_Sygnalizacja_dawka INSERT() { _INSERT(); return this; }
            public Szablon_Sygnalizacja_dawka DELETE() { _DELETE(); return this; }
            public Szablon_Sygnalizacja_dawka WHERE() { _WHERE(); return this; }
            public Szablon_Sygnalizacja_dawka INFO(string text) { _INFO(text); return this; }
            public Szablon_Sygnalizacja_dawka SELECT() { _SELECT(); return this; }
            public Szablon_Sygnalizacja_dawka ORDER_BY() { _ORDER_BY(); return this; }
            public Row_Sygnalizacja_dawka[] GET(int min = 0, int max = 999999999, bool allowException = false) { return Row_Sygnalizacja_dawka._GET(_GET(min, max, allowException)); }
            public Row_Sygnalizacja_dawka GET_ONE() { return Row_Sygnalizacja_dawka._GET(_GET(1, 1))[0]; }
            public Row_Sygnalizacja_dawka GET_OPTIONAL() { var r = Row_Sygnalizacja_dawka._GET(_GET(0, 1)); return r.Length > 0 ? r[0] : null; }
            public Szablon_Sygnalizacja_dawka Czas_zmierzony(double? value)
            {
                SetField("Czas_zmierzony", value, OleDbType.Double);
                return this;
            }
            public Szablon_Sygnalizacja_dawka Czas_zmierzony(double? value, string oper)
            {
                SetField("Czas_zmierzony", value, OleDbType.Double, oper);
                return this;
            }
            public Szablon_Sygnalizacja_dawka Czas_zmierzony(Order order)
            {
                SetOrder("Czas_zmierzony", order);
                return this;
            }
            public Szablon_Sygnalizacja_dawka Czas_zmierzony()
            {
                AddField("Czas_zmierzony");
                return this;
            }
            public Szablon_Sygnalizacja_dawka ID_wzorcowania(int? value)
            {
                SetField("ID_wzorcowania", value, OleDbType.Integer);
                return this;
            }
            public Szablon_Sygnalizacja_dawka ID_wzorcowania(int? value, string oper)
            {
                SetField("ID_wzorcowania", value, OleDbType.Integer, oper);
                return this;
            }
            public Szablon_Sygnalizacja_dawka ID_wzorcowania(Order order)
            {
                SetOrder("ID_wzorcowania", order);
                return this;
            }
            public Szablon_Sygnalizacja_dawka ID_wzorcowania()
            {
                AddField("ID_wzorcowania");
                return this;
            }
            public Szablon_Sygnalizacja_dawka ID_zrodla(int? value)
            {
                SetField("ID_zrodla", value, OleDbType.Integer);
                return this;
            }
            public Szablon_Sygnalizacja_dawka ID_zrodla(int? value, string oper)
            {
                SetField("ID_zrodla", value, OleDbType.Integer, oper);
                return this;
            }
            public Szablon_Sygnalizacja_dawka ID_zrodla(Order order)
            {
                SetOrder("ID_zrodla", order);
                return this;
            }
            public Szablon_Sygnalizacja_dawka ID_zrodla()
            {
                AddField("ID_zrodla");
                return this;
            }
            public Szablon_Sygnalizacja_dawka Niepewnosc(double value)
            {
                SetField("Niepewnosc", value, OleDbType.Double);
                return this;
            }
            public Szablon_Sygnalizacja_dawka Niepewnosc(double value, string oper)
            {
                SetField("Niepewnosc", value, OleDbType.Double, oper);
                return this;
            }
            public Szablon_Sygnalizacja_dawka Niepewnosc(Order order)
            {
                SetOrder("Niepewnosc", order);
                return this;
            }
            public Szablon_Sygnalizacja_dawka Niepewnosc()
            {
                AddField("Niepewnosc");
                return this;
            }
            public Szablon_Sygnalizacja_dawka Niepewnosc_wsp(double value)
            {
                SetField("Niepewnosc_wsp", value, OleDbType.Double);
                return this;
            }
            public Szablon_Sygnalizacja_dawka Niepewnosc_wsp(double value, string oper)
            {
                SetField("Niepewnosc_wsp", value, OleDbType.Double, oper);
                return this;
            }
            public Szablon_Sygnalizacja_dawka Niepewnosc_wsp(Order order)
            {
                SetOrder("Niepewnosc_wsp", order);
                return this;
            }
            public Szablon_Sygnalizacja_dawka Niepewnosc_wsp()
            {
                AddField("Niepewnosc_wsp");
                return this;
            }
            public Szablon_Sygnalizacja_dawka Odleglosc(double? value)
            {
                SetField("Odleglosc", value, OleDbType.Double);
                return this;
            }
            public Szablon_Sygnalizacja_dawka Odleglosc(double? value, string oper)
            {
                SetField("Odleglosc", value, OleDbType.Double, oper);
                return this;
            }
            public Szablon_Sygnalizacja_dawka Odleglosc(Order order)
            {
                SetOrder("Odleglosc", order);
                return this;
            }
            public Szablon_Sygnalizacja_dawka Odleglosc()
            {
                AddField("Odleglosc");
                return this;
            }
            public Szablon_Sygnalizacja_dawka Prog(double? value)
            {
                SetField("Prog", value, OleDbType.Double);
                return this;
            }
            public Szablon_Sygnalizacja_dawka Prog(double? value, string oper)
            {
                SetField("Prog", value, OleDbType.Double, oper);
                return this;
            }
            public Szablon_Sygnalizacja_dawka Prog(Order order)
            {
                SetOrder("Prog", order);
                return this;
            }
            public Szablon_Sygnalizacja_dawka Prog()
            {
                AddField("Prog");
                return this;
            }
            public Szablon_Sygnalizacja_dawka Wartosc_wzorcowa(double? value)
            {
                SetField("Wartosc_wzorcowa", value, OleDbType.Double);
                return this;
            }
            public Szablon_Sygnalizacja_dawka Wartosc_wzorcowa(double? value, string oper)
            {
                SetField("Wartosc_wzorcowa", value, OleDbType.Double, oper);
                return this;
            }
            public Szablon_Sygnalizacja_dawka Wartosc_wzorcowa(Order order)
            {
                SetOrder("Wartosc_wzorcowa", order);
                return this;
            }
            public Szablon_Sygnalizacja_dawka Wartosc_wzorcowa()
            {
                AddField("Wartosc_wzorcowa");
                return this;
            }
            public Szablon_Sygnalizacja_dawka Wartosc_zmierzona(double? value)
            {
                SetField("Wartosc_zmierzona", value, OleDbType.Double);
                return this;
            }
            public Szablon_Sygnalizacja_dawka Wartosc_zmierzona(double? value, string oper)
            {
                SetField("Wartosc_zmierzona", value, OleDbType.Double, oper);
                return this;
            }
            public Szablon_Sygnalizacja_dawka Wartosc_zmierzona(Order order)
            {
                SetOrder("Wartosc_zmierzona", order);
                return this;
            }
            public Szablon_Sygnalizacja_dawka Wartosc_zmierzona()
            {
                AddField("Wartosc_zmierzona");
                return this;
            }
            public Szablon_Sygnalizacja_dawka Wspolczynnik(double value)
            {
                SetField("Wspolczynnik", value, OleDbType.Double);
                return this;
            }
            public Szablon_Sygnalizacja_dawka Wspolczynnik(double value, string oper)
            {
                SetField("Wspolczynnik", value, OleDbType.Double, oper);
                return this;
            }
            public Szablon_Sygnalizacja_dawka Wspolczynnik(Order order)
            {
                SetOrder("Wspolczynnik", order);
                return this;
            }
            public Szablon_Sygnalizacja_dawka Wspolczynnik()
            {
                AddField("Wspolczynnik");
                return this;
            }
        }
        public class Row_Sygnalizacja_dawka : Wiersz
        {
            public double? Czas_zmierzony;
            public int? ID_wzorcowania;
            public int? ID_zrodla;
            public double Niepewnosc;
            public double Niepewnosc_wsp;
            public double? Odleglosc;
            public double? Prog;
            public double? Wartosc_wzorcowa;
            public double? Wartosc_zmierzona;
            public double Wspolczynnik;
            public Row_Sygnalizacja_dawka(DataRow row)
            {
                _init(row, GetColsDict(row));
            }
            public Row_Sygnalizacja_dawka(DataRow row, Dictionary<string, int> cols)
            {
                _init(row, cols);
            }
            private void _init(DataRow row, Dictionary<string, int> cols)
            {
                if (cols.ContainsKey("Czas_zmierzony"))
                    Czas_zmierzony = row.Field<double?>(cols["Czas_zmierzony"]);
                if (cols.ContainsKey("ID_wzorcowania"))
                    ID_wzorcowania = row.Field<int?>(cols["ID_wzorcowania"]);
                if (cols.ContainsKey("ID_zrodla"))
                    ID_zrodla = row.Field<int?>(cols["ID_zrodla"]);
                if (cols.ContainsKey("Niepewnosc"))
                    Niepewnosc = row.Field<double>(cols["Niepewnosc"]);
                if (cols.ContainsKey("Niepewnosc_wsp"))
                    Niepewnosc_wsp = row.Field<double>(cols["Niepewnosc_wsp"]);
                if (cols.ContainsKey("Odleglosc"))
                    Odleglosc = row.Field<double?>(cols["Odleglosc"]);
                if (cols.ContainsKey("Prog"))
                    Prog = row.Field<double?>(cols["Prog"]);
                if (cols.ContainsKey("Wartosc_wzorcowa"))
                    Wartosc_wzorcowa = row.Field<double?>(cols["Wartosc_wzorcowa"]);
                if (cols.ContainsKey("Wartosc_zmierzona"))
                    Wartosc_zmierzona = row.Field<double?>(cols["Wartosc_zmierzona"]);
                if (cols.ContainsKey("Wspolczynnik"))
                    Wspolczynnik = row.Field<double>(cols["Wspolczynnik"]);
            }
            internal static Row_Sygnalizacja_dawka[] _GET(DataTable dane)
            {
                var result = new Row_Sygnalizacja_dawka[dane.Rows.Count];
                var cols = GetColsDict(dane);
                var index = 0;
                foreach (DataRow row in dane.Rows)
                    result[index++] = new Row_Sygnalizacja_dawka(row, cols);
                return result;
            }
        }
        public class Szablon_Wyniki_dawka : Tabela
        {
            public Szablon_Wyniki_dawka(BazaDanychWrapper baza, string nazwa) : base(baza, nazwa) { }
            public Szablon_Wyniki_dawka UPDATE() { _UPDATE(); return this; }
            public Szablon_Wyniki_dawka INSERT() { _INSERT(); return this; }
            public Szablon_Wyniki_dawka DELETE() { _DELETE(); return this; }
            public Szablon_Wyniki_dawka WHERE() { _WHERE(); return this; }
            public Szablon_Wyniki_dawka INFO(string text) { _INFO(text); return this; }
            public Szablon_Wyniki_dawka SELECT() { _SELECT(); return this; }
            public Szablon_Wyniki_dawka ORDER_BY() { _ORDER_BY(); return this; }
            public Row_Wyniki_dawka[] GET(int min = 0, int max = 999999999, bool allowException = false) { return Row_Wyniki_dawka._GET(_GET(min, max, allowException)); }
            public Row_Wyniki_dawka GET_ONE() { return Row_Wyniki_dawka._GET(_GET(1, 1))[0]; }
            public Row_Wyniki_dawka GET_OPTIONAL() { var r = Row_Wyniki_dawka._GET(_GET(0, 1)); return r.Length > 0 ? r[0] : null; }
            public Szablon_Wyniki_dawka ID_wzorcowania(int? value)
            {
                SetField("ID_wzorcowania", value, OleDbType.Integer);
                return this;
            }
            public Szablon_Wyniki_dawka ID_wzorcowania(int? value, string oper)
            {
                SetField("ID_wzorcowania", value, OleDbType.Integer, oper);
                return this;
            }
            public Szablon_Wyniki_dawka ID_wzorcowania(Order order)
            {
                SetOrder("ID_wzorcowania", order);
                return this;
            }
            public Szablon_Wyniki_dawka ID_wzorcowania()
            {
                AddField("ID_wzorcowania");
                return this;
            }
            public Szablon_Wyniki_dawka ID_zrodla(int? value)
            {
                SetField("ID_zrodla", value, OleDbType.Integer);
                return this;
            }
            public Szablon_Wyniki_dawka ID_zrodla(int? value, string oper)
            {
                SetField("ID_zrodla", value, OleDbType.Integer, oper);
                return this;
            }
            public Szablon_Wyniki_dawka ID_zrodla(Order order)
            {
                SetOrder("ID_zrodla", order);
                return this;
            }
            public Szablon_Wyniki_dawka ID_zrodla()
            {
                AddField("ID_zrodla");
                return this;
            }
            public Szablon_Wyniki_dawka Niepewnosc(double? value)
            {
                SetField("Niepewnosc", value, OleDbType.Double);
                return this;
            }
            public Szablon_Wyniki_dawka Niepewnosc(double? value, string oper)
            {
                SetField("Niepewnosc", value, OleDbType.Double, oper);
                return this;
            }
            public Szablon_Wyniki_dawka Niepewnosc(Order order)
            {
                SetOrder("Niepewnosc", order);
                return this;
            }
            public Szablon_Wyniki_dawka Niepewnosc()
            {
                AddField("Niepewnosc");
                return this;
            }
            public Szablon_Wyniki_dawka Odleglosc(double? value)
            {
                SetField("Odleglosc", value, OleDbType.Double);
                return this;
            }
            public Szablon_Wyniki_dawka Odleglosc(double? value, string oper)
            {
                SetField("Odleglosc", value, OleDbType.Double, oper);
                return this;
            }
            public Szablon_Wyniki_dawka Odleglosc(Order order)
            {
                SetOrder("Odleglosc", order);
                return this;
            }
            public Szablon_Wyniki_dawka Odleglosc()
            {
                AddField("Odleglosc");
                return this;
            }
            public Szablon_Wyniki_dawka Wielkosc_fizyczna(int? value)
            {
                SetField("Wielkosc_fizyczna", value, OleDbType.Integer);
                return this;
            }
            public Szablon_Wyniki_dawka Wielkosc_fizyczna(int? value, string oper)
            {
                SetField("Wielkosc_fizyczna", value, OleDbType.Integer, oper);
                return this;
            }
            public Szablon_Wyniki_dawka Wielkosc_fizyczna(Order order)
            {
                SetOrder("Wielkosc_fizyczna", order);
                return this;
            }
            public Szablon_Wyniki_dawka Wielkosc_fizyczna()
            {
                AddField("Wielkosc_fizyczna");
                return this;
            }
            public Szablon_Wyniki_dawka Wspolczynnik(double? value)
            {
                SetField("Wspolczynnik", value, OleDbType.Double);
                return this;
            }
            public Szablon_Wyniki_dawka Wspolczynnik(double? value, string oper)
            {
                SetField("Wspolczynnik", value, OleDbType.Double, oper);
                return this;
            }
            public Szablon_Wyniki_dawka Wspolczynnik(Order order)
            {
                SetOrder("Wspolczynnik", order);
                return this;
            }
            public Szablon_Wyniki_dawka Wspolczynnik()
            {
                AddField("Wspolczynnik");
                return this;
            }
            public Szablon_Wyniki_dawka Zakres(double? value)
            {
                SetField("Zakres", value, OleDbType.Double);
                return this;
            }
            public Szablon_Wyniki_dawka Zakres(double? value, string oper)
            {
                SetField("Zakres", value, OleDbType.Double, oper);
                return this;
            }
            public Szablon_Wyniki_dawka Zakres(Order order)
            {
                SetOrder("Zakres", order);
                return this;
            }
            public Szablon_Wyniki_dawka Zakres()
            {
                AddField("Zakres");
                return this;
            }
        }
        public class Row_Wyniki_dawka : Wiersz
        {
            public int? ID_wzorcowania;
            public int? ID_zrodla;
            public double? Niepewnosc;
            public double? Odleglosc;
            public int? Wielkosc_fizyczna;
            public double? Wspolczynnik;
            public double? Zakres;
            public Row_Wyniki_dawka(DataRow row)
            {
                _init(row, GetColsDict(row));
            }
            public Row_Wyniki_dawka(DataRow row, Dictionary<string, int> cols)
            {
                _init(row, cols);
            }
            private void _init(DataRow row, Dictionary<string, int> cols)
            {
                if (cols.ContainsKey("ID_wzorcowania"))
                    ID_wzorcowania = row.Field<int?>(cols["ID_wzorcowania"]);
                if (cols.ContainsKey("ID_zrodla"))
                    ID_zrodla = row.Field<int?>(cols["ID_zrodla"]);
                if (cols.ContainsKey("Niepewnosc"))
                    Niepewnosc = row.Field<double?>(cols["Niepewnosc"]);
                if (cols.ContainsKey("Odleglosc"))
                    Odleglosc = row.Field<double?>(cols["Odleglosc"]);
                if (cols.ContainsKey("Wielkosc_fizyczna"))
                    Wielkosc_fizyczna = row.Field<int?>(cols["Wielkosc_fizyczna"]);
                if (cols.ContainsKey("Wspolczynnik"))
                    Wspolczynnik = row.Field<double?>(cols["Wspolczynnik"]);
                if (cols.ContainsKey("Zakres"))
                    Zakres = row.Field<double?>(cols["Zakres"]);
            }
            internal static Row_Wyniki_dawka[] _GET(DataTable dane)
            {
                var result = new Row_Wyniki_dawka[dane.Rows.Count];
                var cols = GetColsDict(dane);
                var index = 0;
                foreach (DataRow row in dane.Rows)
                    result[index++] = new Row_Wyniki_dawka(row, cols);
                return result;
            }
        }
        public class Szablon_wyniki_moc_dawki : Tabela
        {
            public Szablon_wyniki_moc_dawki(BazaDanychWrapper baza, string nazwa) : base(baza, nazwa) { }
            public Szablon_wyniki_moc_dawki UPDATE() { _UPDATE(); return this; }
            public Szablon_wyniki_moc_dawki INSERT() { _INSERT(); return this; }
            public Szablon_wyniki_moc_dawki DELETE() { _DELETE(); return this; }
            public Szablon_wyniki_moc_dawki WHERE() { _WHERE(); return this; }
            public Szablon_wyniki_moc_dawki INFO(string text) { _INFO(text); return this; }
            public Szablon_wyniki_moc_dawki SELECT() { _SELECT(); return this; }
            public Szablon_wyniki_moc_dawki ORDER_BY() { _ORDER_BY(); return this; }
            public Row_wyniki_moc_dawki[] GET(int min = 0, int max = 999999999, bool allowException = false) { return Row_wyniki_moc_dawki._GET(_GET(min, max, allowException)); }
            public Row_wyniki_moc_dawki GET_ONE() { return Row_wyniki_moc_dawki._GET(_GET(1, 1))[0]; }
            public Row_wyniki_moc_dawki GET_OPTIONAL() { var r = Row_wyniki_moc_dawki._GET(_GET(0, 1)); return r.Length > 0 ? r[0] : null; }
            public Szablon_wyniki_moc_dawki ID_wzorcowania(int? value)
            {
                SetField("ID_wzorcowania", value, OleDbType.Integer);
                return this;
            }
            public Szablon_wyniki_moc_dawki ID_wzorcowania(int? value, string oper)
            {
                SetField("ID_wzorcowania", value, OleDbType.Integer, oper);
                return this;
            }
            public Szablon_wyniki_moc_dawki ID_wzorcowania(Order order)
            {
                SetOrder("ID_wzorcowania", order);
                return this;
            }
            public Szablon_wyniki_moc_dawki ID_wzorcowania()
            {
                AddField("ID_wzorcowania");
                return this;
            }
            public Szablon_wyniki_moc_dawki Niepewnosc(double? value)
            {
                SetField("Niepewnosc", value, OleDbType.Double);
                return this;
            }
            public Szablon_wyniki_moc_dawki Niepewnosc(double? value, string oper)
            {
                SetField("Niepewnosc", value, OleDbType.Double, oper);
                return this;
            }
            public Szablon_wyniki_moc_dawki Niepewnosc(Order order)
            {
                SetOrder("Niepewnosc", order);
                return this;
            }
            public Szablon_wyniki_moc_dawki Niepewnosc()
            {
                AddField("Niepewnosc");
                return this;
            }
            public Szablon_wyniki_moc_dawki Wspolczynnik(double? value)
            {
                SetField("Wspolczynnik", value, OleDbType.Double);
                return this;
            }
            public Szablon_wyniki_moc_dawki Wspolczynnik(double? value, string oper)
            {
                SetField("Wspolczynnik", value, OleDbType.Double, oper);
                return this;
            }
            public Szablon_wyniki_moc_dawki Wspolczynnik(Order order)
            {
                SetOrder("Wspolczynnik", order);
                return this;
            }
            public Szablon_wyniki_moc_dawki Wspolczynnik()
            {
                AddField("Wspolczynnik");
                return this;
            }
            public Szablon_wyniki_moc_dawki ZAKRES(double? value)
            {
                SetField("ZAKRES", value, OleDbType.Double);
                return this;
            }
            public Szablon_wyniki_moc_dawki ZAKRES(double? value, string oper)
            {
                SetField("ZAKRES", value, OleDbType.Double, oper);
                return this;
            }
            public Szablon_wyniki_moc_dawki ZAKRES(Order order)
            {
                SetOrder("ZAKRES", order);
                return this;
            }
            public Szablon_wyniki_moc_dawki ZAKRES()
            {
                AddField("ZAKRES");
                return this;
            }
        }
        public class Row_wyniki_moc_dawki : Wiersz
        {
            public int? ID_wzorcowania;
            public double? Niepewnosc;
            public double? Wspolczynnik;
            public double? ZAKRES;
            public Row_wyniki_moc_dawki(DataRow row)
            {
                _init(row, GetColsDict(row));
            }
            public Row_wyniki_moc_dawki(DataRow row, Dictionary<string, int> cols)
            {
                _init(row, cols);
            }
            private void _init(DataRow row, Dictionary<string, int> cols)
            {
                if (cols.ContainsKey("ID_wzorcowania"))
                    ID_wzorcowania = row.Field<int?>(cols["ID_wzorcowania"]);
                if (cols.ContainsKey("Niepewnosc"))
                    Niepewnosc = row.Field<double?>(cols["Niepewnosc"]);
                if (cols.ContainsKey("Wspolczynnik"))
                    Wspolczynnik = row.Field<double?>(cols["Wspolczynnik"]);
                if (cols.ContainsKey("ZAKRES"))
                    ZAKRES = row.Field<double?>(cols["ZAKRES"]);
            }
            internal static Row_wyniki_moc_dawki[] _GET(DataTable dane)
            {
                var result = new Row_wyniki_moc_dawki[dane.Rows.Count];
                var cols = GetColsDict(dane);
                var index = 0;
                foreach (DataRow row in dane.Rows)
                    result[index++] = new Row_wyniki_moc_dawki(row, cols);
                return result;
            }
        }
        public class Szablon_wzorcowanie_cezem : Tabela
        {
            public Szablon_wzorcowanie_cezem(BazaDanychWrapper baza, string nazwa) : base(baza, nazwa) { }
            public Szablon_wzorcowanie_cezem UPDATE() { _UPDATE(); return this; }
            public Szablon_wzorcowanie_cezem INSERT() { _INSERT(); return this; }
            public Szablon_wzorcowanie_cezem DELETE() { _DELETE(); return this; }
            public Szablon_wzorcowanie_cezem WHERE() { _WHERE(); return this; }
            public Szablon_wzorcowanie_cezem INFO(string text) { _INFO(text); return this; }
            public Szablon_wzorcowanie_cezem SELECT() { _SELECT(); return this; }
            public Szablon_wzorcowanie_cezem ORDER_BY() { _ORDER_BY(); return this; }
            public Row_wzorcowanie_cezem[] GET(int min = 0, int max = 999999999, bool allowException = false) { return Row_wzorcowanie_cezem._GET(_GET(min, max, allowException)); }
            public Row_wzorcowanie_cezem GET_ONE() { return Row_wzorcowanie_cezem._GET(_GET(1, 1))[0]; }
            public Row_wzorcowanie_cezem GET_OPTIONAL() { var r = Row_wzorcowanie_cezem._GET(_GET(0, 1)); return r.Length > 0 ? r[0] : null; }
            public Szablon_wzorcowanie_cezem Cisnienie(double? value)
            {
                SetField("Cisnienie", value, OleDbType.Double);
                return this;
            }
            public Szablon_wzorcowanie_cezem Cisnienie(double? value, string oper)
            {
                SetField("Cisnienie", value, OleDbType.Double, oper);
                return this;
            }
            public Szablon_wzorcowanie_cezem Cisnienie(Order order)
            {
                SetOrder("Cisnienie", order);
                return this;
            }
            public Szablon_wzorcowanie_cezem Cisnienie()
            {
                AddField("Cisnienie");
                return this;
            }
            public Szablon_wzorcowanie_cezem Data_wzorcowania(DateTime? value)
            {
                SetField("Data_wzorcowania", value, OleDbType.Date);
                return this;
            }
            public Szablon_wzorcowanie_cezem Data_wzorcowania(DateTime? value, string oper)
            {
                SetField("Data_wzorcowania", value, OleDbType.Date, oper);
                return this;
            }
            public Szablon_wzorcowanie_cezem Data_wzorcowania(Order order)
            {
                SetOrder("Data_wzorcowania", order);
                return this;
            }
            public Szablon_wzorcowanie_cezem Data_wzorcowania()
            {
                AddField("Data_wzorcowania");
                return this;
            }
            public Szablon_wzorcowanie_cezem Dolacz(bool value)
            {
                SetField("Dolacz", value, OleDbType.Boolean);
                return this;
            }
            public Szablon_wzorcowanie_cezem Dolacz(bool value, string oper)
            {
                SetField("Dolacz", value, OleDbType.Boolean, oper);
                return this;
            }
            public Szablon_wzorcowanie_cezem Dolacz(Order order)
            {
                SetOrder("Dolacz", order);
                return this;
            }
            public Szablon_wzorcowanie_cezem Dolacz()
            {
                AddField("Dolacz");
                return this;
            }
            public Szablon_wzorcowanie_cezem ID_arkusza(short? value)
            {
                SetField("ID_arkusza", value, OleDbType.SmallInt);
                return this;
            }
            public Szablon_wzorcowanie_cezem ID_arkusza(short? value, string oper)
            {
                SetField("ID_arkusza", value, OleDbType.SmallInt, oper);
                return this;
            }
            public Szablon_wzorcowanie_cezem ID_arkusza(Order order)
            {
                SetOrder("ID_arkusza", order);
                return this;
            }
            public Szablon_wzorcowanie_cezem ID_arkusza()
            {
                AddField("ID_arkusza");
                return this;
            }
            public Szablon_wzorcowanie_cezem ID_jednostki(int? value)
            {
                SetField("ID_jednostki", value, OleDbType.Integer);
                return this;
            }
            public Szablon_wzorcowanie_cezem ID_jednostki(int? value, string oper)
            {
                SetField("ID_jednostki", value, OleDbType.Integer, oper);
                return this;
            }
            public Szablon_wzorcowanie_cezem ID_jednostki(Order order)
            {
                SetOrder("ID_jednostki", order);
                return this;
            }
            public Szablon_wzorcowanie_cezem ID_jednostki()
            {
                AddField("ID_jednostki");
                return this;
            }
            public Szablon_wzorcowanie_cezem ID_karty(int? value)
            {
                SetField("ID_karty", value, OleDbType.Integer);
                return this;
            }
            public Szablon_wzorcowanie_cezem ID_karty(int? value, string oper)
            {
                SetField("ID_karty", value, OleDbType.Integer, oper);
                return this;
            }
            public Szablon_wzorcowanie_cezem ID_karty(Order order)
            {
                SetOrder("ID_karty", order);
                return this;
            }
            public Szablon_wzorcowanie_cezem ID_karty()
            {
                AddField("ID_karty");
                return this;
            }
            public Szablon_wzorcowanie_cezem ID_protokolu(short? value)
            {
                SetField("ID_protokolu", value, OleDbType.SmallInt);
                return this;
            }
            public Szablon_wzorcowanie_cezem ID_protokolu(short? value, string oper)
            {
                SetField("ID_protokolu", value, OleDbType.SmallInt, oper);
                return this;
            }
            public Szablon_wzorcowanie_cezem ID_protokolu(Order order)
            {
                SetOrder("ID_protokolu", order);
                return this;
            }
            public Szablon_wzorcowanie_cezem ID_protokolu()
            {
                AddField("ID_protokolu");
                return this;
            }
            public Szablon_wzorcowanie_cezem ID_sondy(int? value)
            {
                SetField("ID_sondy", value, OleDbType.Integer);
                return this;
            }
            public Szablon_wzorcowanie_cezem ID_sondy(int? value, string oper)
            {
                SetField("ID_sondy", value, OleDbType.Integer, oper);
                return this;
            }
            public Szablon_wzorcowanie_cezem ID_sondy(Order order)
            {
                SetOrder("ID_sondy", order);
                return this;
            }
            public Szablon_wzorcowanie_cezem ID_sondy()
            {
                AddField("ID_sondy");
                return this;
            }
            public Szablon_wzorcowanie_cezem ID_wzorcowania(int value)
            {
                SetField("ID_wzorcowania", value, OleDbType.Integer);
                return this;
            }
            public Szablon_wzorcowanie_cezem ID_wzorcowania(int value, string oper)
            {
                SetField("ID_wzorcowania", value, OleDbType.Integer, oper);
                return this;
            }
            public Szablon_wzorcowanie_cezem ID_wzorcowania(Order order)
            {
                SetOrder("ID_wzorcowania", order);
                return this;
            }
            public Szablon_wzorcowanie_cezem ID_wzorcowania()
            {
                AddField("ID_wzorcowania");
                return this;
            }
            public Szablon_wzorcowanie_cezem Inne_nastawy(string value)
            {
                SetField("Inne_nastawy", value, OleDbType.WChar);
                return this;
            }
            public Szablon_wzorcowanie_cezem Inne_nastawy(string value, string oper)
            {
                SetField("Inne_nastawy", value, OleDbType.WChar, oper);
                return this;
            }
            public Szablon_wzorcowanie_cezem Inne_nastawy(Order order)
            {
                SetOrder("Inne_nastawy", order);
                return this;
            }
            public Szablon_wzorcowanie_cezem Inne_nastawy()
            {
                AddField("Inne_nastawy");
                return this;
            }
            public Szablon_wzorcowanie_cezem Napiecie_zasilania_sondy(string value)
            {
                SetField("Napiecie_zasilania_sondy", value, OleDbType.WChar);
                return this;
            }
            public Szablon_wzorcowanie_cezem Napiecie_zasilania_sondy(string value, string oper)
            {
                SetField("Napiecie_zasilania_sondy", value, OleDbType.WChar, oper);
                return this;
            }
            public Szablon_wzorcowanie_cezem Napiecie_zasilania_sondy(Order order)
            {
                SetOrder("Napiecie_zasilania_sondy", order);
                return this;
            }
            public Szablon_wzorcowanie_cezem Napiecie_zasilania_sondy()
            {
                AddField("Napiecie_zasilania_sondy");
                return this;
            }
            public Szablon_wzorcowanie_cezem Osoba_sprawdzajaca(string value)
            {
                SetField("Osoba_sprawdzajaca", value, OleDbType.WChar);
                return this;
            }
            public Szablon_wzorcowanie_cezem Osoba_sprawdzajaca(string value, string oper)
            {
                SetField("Osoba_sprawdzajaca", value, OleDbType.WChar, oper);
                return this;
            }
            public Szablon_wzorcowanie_cezem Osoba_sprawdzajaca(Order order)
            {
                SetOrder("Osoba_sprawdzajaca", order);
                return this;
            }
            public Szablon_wzorcowanie_cezem Osoba_sprawdzajaca()
            {
                AddField("Osoba_sprawdzajaca");
                return this;
            }
            public Szablon_wzorcowanie_cezem Osoba_wzorcujaca(string value)
            {
                SetField("Osoba_wzorcujaca", value, OleDbType.WChar);
                return this;
            }
            public Szablon_wzorcowanie_cezem Osoba_wzorcujaca(string value, string oper)
            {
                SetField("Osoba_wzorcujaca", value, OleDbType.WChar, oper);
                return this;
            }
            public Szablon_wzorcowanie_cezem Osoba_wzorcujaca(Order order)
            {
                SetOrder("Osoba_wzorcujaca", order);
                return this;
            }
            public Szablon_wzorcowanie_cezem Osoba_wzorcujaca()
            {
                AddField("Osoba_wzorcujaca");
                return this;
            }
            public Szablon_wzorcowanie_cezem Rodzaj_wzorcowania(string value)
            {
                SetField("Rodzaj_wzorcowania", value, OleDbType.WChar);
                return this;
            }
            public Szablon_wzorcowanie_cezem Rodzaj_wzorcowania(string value, string oper)
            {
                SetField("Rodzaj_wzorcowania", value, OleDbType.WChar, oper);
                return this;
            }
            public Szablon_wzorcowanie_cezem Rodzaj_wzorcowania(Order order)
            {
                SetOrder("Rodzaj_wzorcowania", order);
                return this;
            }
            public Szablon_wzorcowanie_cezem Rodzaj_wzorcowania()
            {
                AddField("Rodzaj_wzorcowania");
                return this;
            }
            public Szablon_wzorcowanie_cezem Temperatura(double? value)
            {
                SetField("Temperatura", value, OleDbType.Double);
                return this;
            }
            public Szablon_wzorcowanie_cezem Temperatura(double? value, string oper)
            {
                SetField("Temperatura", value, OleDbType.Double, oper);
                return this;
            }
            public Szablon_wzorcowanie_cezem Temperatura(Order order)
            {
                SetOrder("Temperatura", order);
                return this;
            }
            public Szablon_wzorcowanie_cezem Temperatura()
            {
                AddField("Temperatura");
                return this;
            }
            public Szablon_wzorcowanie_cezem Tlo(string value)
            {
                SetField("Tlo", value, OleDbType.WChar);
                return this;
            }
            public Szablon_wzorcowanie_cezem Tlo(string value, string oper)
            {
                SetField("Tlo", value, OleDbType.WChar, oper);
                return this;
            }
            public Szablon_wzorcowanie_cezem Tlo(Order order)
            {
                SetOrder("Tlo", order);
                return this;
            }
            public Szablon_wzorcowanie_cezem Tlo()
            {
                AddField("Tlo");
                return this;
            }
            public Szablon_wzorcowanie_cezem Uwagi(string value)
            {
                SetField("Uwagi", value, OleDbType.WChar);
                return this;
            }
            public Szablon_wzorcowanie_cezem Uwagi(string value, string oper)
            {
                SetField("Uwagi", value, OleDbType.WChar, oper);
                return this;
            }
            public Szablon_wzorcowanie_cezem Uwagi(Order order)
            {
                SetOrder("Uwagi", order);
                return this;
            }
            public Szablon_wzorcowanie_cezem Uwagi()
            {
                AddField("Uwagi");
                return this;
            }
            public Szablon_wzorcowanie_cezem Wielkosc_fizyczna(string value)
            {
                SetField("Wielkosc_fizyczna", value, OleDbType.WChar);
                return this;
            }
            public Szablon_wzorcowanie_cezem Wielkosc_fizyczna(string value, string oper)
            {
                SetField("Wielkosc_fizyczna", value, OleDbType.WChar, oper);
                return this;
            }
            public Szablon_wzorcowanie_cezem Wielkosc_fizyczna(Order order)
            {
                SetOrder("Wielkosc_fizyczna", order);
                return this;
            }
            public Szablon_wzorcowanie_cezem Wielkosc_fizyczna()
            {
                AddField("Wielkosc_fizyczna");
                return this;
            }
            public Szablon_wzorcowanie_cezem wilgotnosc(double? value)
            {
                SetField("wilgotnosc", value, OleDbType.Double);
                return this;
            }
            public Szablon_wzorcowanie_cezem wilgotnosc(double? value, string oper)
            {
                SetField("wilgotnosc", value, OleDbType.Double, oper);
                return this;
            }
            public Szablon_wzorcowanie_cezem wilgotnosc(Order order)
            {
                SetOrder("wilgotnosc", order);
                return this;
            }
            public Szablon_wzorcowanie_cezem wilgotnosc()
            {
                AddField("wilgotnosc");
                return this;
            }
        }
        public class Row_wzorcowanie_cezem : Wiersz
        {
            public double? Cisnienie;
            public DateTime? Data_wzorcowania;
            public bool Dolacz;
            public short? ID_arkusza;
            public int? ID_jednostki;
            public int? ID_karty;
            public short? ID_protokolu;
            public int? ID_sondy;
            public int ID_wzorcowania;
            public string Inne_nastawy;
            public string Napiecie_zasilania_sondy;
            public string Osoba_sprawdzajaca;
            public string Osoba_wzorcujaca;
            public string Rodzaj_wzorcowania;
            public double? Temperatura;
            public string Tlo;
            public string Uwagi;
            public string Wielkosc_fizyczna;
            public double? wilgotnosc;
            public Row_wzorcowanie_cezem(DataRow row)
            {
                _init(row, GetColsDict(row));
            }
            public Row_wzorcowanie_cezem(DataRow row, Dictionary<string, int> cols)
            {
                _init(row, cols);
            }
            private void _init(DataRow row, Dictionary<string, int> cols)
            {
                if (cols.ContainsKey("Cisnienie"))
                    Cisnienie = row.Field<double?>(cols["Cisnienie"]);
                if (cols.ContainsKey("Data_wzorcowania"))
                    Data_wzorcowania = row.Field<DateTime?>(cols["Data_wzorcowania"]);
                if (cols.ContainsKey("Dolacz"))
                    Dolacz = row.Field<bool>(cols["Dolacz"]);
                if (cols.ContainsKey("ID_arkusza"))
                    ID_arkusza = row.Field<short?>(cols["ID_arkusza"]);
                if (cols.ContainsKey("ID_jednostki"))
                    ID_jednostki = row.Field<int?>(cols["ID_jednostki"]);
                if (cols.ContainsKey("ID_karty"))
                    ID_karty = row.Field<int?>(cols["ID_karty"]);
                if (cols.ContainsKey("ID_protokolu"))
                    ID_protokolu = row.Field<short?>(cols["ID_protokolu"]);
                if (cols.ContainsKey("ID_sondy"))
                    ID_sondy = row.Field<int?>(cols["ID_sondy"]);
                if (cols.ContainsKey("ID_wzorcowania"))
                    ID_wzorcowania = row.Field<int>(cols["ID_wzorcowania"]);
                if (cols.ContainsKey("Inne_nastawy"))
                    Inne_nastawy = row.Field<string>(cols["Inne_nastawy"]);
                if (cols.ContainsKey("Napiecie_zasilania_sondy"))
                    Napiecie_zasilania_sondy = row.Field<string>(cols["Napiecie_zasilania_sondy"]);
                if (cols.ContainsKey("Osoba_sprawdzajaca"))
                    Osoba_sprawdzajaca = row.Field<string>(cols["Osoba_sprawdzajaca"]);
                if (cols.ContainsKey("Osoba_wzorcujaca"))
                    Osoba_wzorcujaca = row.Field<string>(cols["Osoba_wzorcujaca"]);
                if (cols.ContainsKey("Rodzaj_wzorcowania"))
                    Rodzaj_wzorcowania = row.Field<string>(cols["Rodzaj_wzorcowania"]);
                if (cols.ContainsKey("Temperatura"))
                    Temperatura = row.Field<double?>(cols["Temperatura"]);
                if (cols.ContainsKey("Tlo"))
                    Tlo = row.Field<string>(cols["Tlo"]);
                if (cols.ContainsKey("Uwagi"))
                    Uwagi = row.Field<string>(cols["Uwagi"]);
                if (cols.ContainsKey("Wielkosc_fizyczna"))
                    Wielkosc_fizyczna = row.Field<string>(cols["Wielkosc_fizyczna"]);
                if (cols.ContainsKey("wilgotnosc"))
                    wilgotnosc = row.Field<double?>(cols["wilgotnosc"]);
            }
            internal static Row_wzorcowanie_cezem[] _GET(DataTable dane)
            {
                var result = new Row_wzorcowanie_cezem[dane.Rows.Count];
                var cols = GetColsDict(dane);
                var index = 0;
                foreach (DataRow row in dane.Rows)
                    result[index++] = new Row_wzorcowanie_cezem(row, cols);
                return result;
            }
        }
        public class Szablon_Wzorcowanie_zrodlami_powierzchniowymi : Tabela
        {
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi(BazaDanychWrapper baza, string nazwa) : base(baza, nazwa) { }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi UPDATE() { _UPDATE(); return this; }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi INSERT() { _INSERT(); return this; }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi DELETE() { _DELETE(); return this; }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi WHERE() { _WHERE(); return this; }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi INFO(string text) { _INFO(text); return this; }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi SELECT() { _SELECT(); return this; }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi ORDER_BY() { _ORDER_BY(); return this; }
            public Row_Wzorcowanie_zrodlami_powierzchniowymi[] GET(int min = 0, int max = 999999999, bool allowException = false) { return Row_Wzorcowanie_zrodlami_powierzchniowymi._GET(_GET(min, max, allowException)); }
            public Row_Wzorcowanie_zrodlami_powierzchniowymi GET_ONE() { return Row_Wzorcowanie_zrodlami_powierzchniowymi._GET(_GET(1, 1))[0]; }
            public Row_Wzorcowanie_zrodlami_powierzchniowymi GET_OPTIONAL() { var r = Row_Wzorcowanie_zrodlami_powierzchniowymi._GET(_GET(0, 1)); return r.Length > 0 ? r[0] : null; }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi Cisnienie(double? value)
            {
                SetField("Cisnienie", value, OleDbType.Double);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi Cisnienie(double? value, string oper)
            {
                SetField("Cisnienie", value, OleDbType.Double, oper);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi Cisnienie(Order order)
            {
                SetOrder("Cisnienie", order);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi Cisnienie()
            {
                AddField("Cisnienie");
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi Data_wzorcowania(DateTime? value)
            {
                SetField("Data_wzorcowania", value, OleDbType.Date);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi Data_wzorcowania(DateTime? value, string oper)
            {
                SetField("Data_wzorcowania", value, OleDbType.Date, oper);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi Data_wzorcowania(Order order)
            {
                SetOrder("Data_wzorcowania", order);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi Data_wzorcowania()
            {
                AddField("Data_wzorcowania");
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi Dolacz(bool value)
            {
                SetField("Dolacz", value, OleDbType.Boolean);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi Dolacz(bool value, string oper)
            {
                SetField("Dolacz", value, OleDbType.Boolean, oper);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi Dolacz(Order order)
            {
                SetOrder("Dolacz", order);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi Dolacz()
            {
                AddField("Dolacz");
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi ID_arkusza(int? value)
            {
                SetField("ID_arkusza", value, OleDbType.Integer);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi ID_arkusza(int? value, string oper)
            {
                SetField("ID_arkusza", value, OleDbType.Integer, oper);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi ID_arkusza(Order order)
            {
                SetOrder("ID_arkusza", order);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi ID_arkusza()
            {
                AddField("ID_arkusza");
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi ID_jednostki(int? value)
            {
                SetField("ID_jednostki", value, OleDbType.Integer);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi ID_jednostki(int? value, string oper)
            {
                SetField("ID_jednostki", value, OleDbType.Integer, oper);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi ID_jednostki(Order order)
            {
                SetOrder("ID_jednostki", order);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi ID_jednostki()
            {
                AddField("ID_jednostki");
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi ID_karty(int? value)
            {
                SetField("ID_karty", value, OleDbType.Integer);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi ID_karty(int? value, string oper)
            {
                SetField("ID_karty", value, OleDbType.Integer, oper);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi ID_karty(Order order)
            {
                SetOrder("ID_karty", order);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi ID_karty()
            {
                AddField("ID_karty");
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi ID_sondy(int? value)
            {
                SetField("ID_sondy", value, OleDbType.Integer);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi ID_sondy(int? value, string oper)
            {
                SetField("ID_sondy", value, OleDbType.Integer, oper);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi ID_sondy(Order order)
            {
                SetOrder("ID_sondy", order);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi ID_sondy()
            {
                AddField("ID_sondy");
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi ID_wzorcowania(int value)
            {
                SetField("ID_wzorcowania", value, OleDbType.Integer);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi ID_wzorcowania(int value, string oper)
            {
                SetField("ID_wzorcowania", value, OleDbType.Integer, oper);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi ID_wzorcowania(Order order)
            {
                SetOrder("ID_wzorcowania", order);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi ID_wzorcowania()
            {
                AddField("ID_wzorcowania");
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi ID_zrodla(int? value)
            {
                SetField("ID_zrodla", value, OleDbType.Integer);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi ID_zrodla(int? value, string oper)
            {
                SetField("ID_zrodla", value, OleDbType.Integer, oper);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi ID_zrodla(Order order)
            {
                SetOrder("ID_zrodla", order);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi ID_zrodla()
            {
                AddField("ID_zrodla");
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi Inne_nastawy(string value)
            {
                SetField("Inne_nastawy", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi Inne_nastawy(string value, string oper)
            {
                SetField("Inne_nastawy", value, OleDbType.WChar, oper);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi Inne_nastawy(Order order)
            {
                SetOrder("Inne_nastawy", order);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi Inne_nastawy()
            {
                AddField("Inne_nastawy");
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi Mnoznik_korekcyjny(double? value)
            {
                SetField("Mnoznik_korekcyjny", value, OleDbType.Double);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi Mnoznik_korekcyjny(double? value, string oper)
            {
                SetField("Mnoznik_korekcyjny", value, OleDbType.Double, oper);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi Mnoznik_korekcyjny(Order order)
            {
                SetOrder("Mnoznik_korekcyjny", order);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi Mnoznik_korekcyjny()
            {
                AddField("Mnoznik_korekcyjny");
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi Napiecie_zasilania_sondy(string value)
            {
                SetField("Napiecie_zasilania_sondy", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi Napiecie_zasilania_sondy(string value, string oper)
            {
                SetField("Napiecie_zasilania_sondy", value, OleDbType.WChar, oper);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi Napiecie_zasilania_sondy(Order order)
            {
                SetOrder("Napiecie_zasilania_sondy", order);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi Napiecie_zasilania_sondy()
            {
                AddField("Napiecie_zasilania_sondy");
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi Niepewnosc(double? value)
            {
                SetField("Niepewnosc", value, OleDbType.Double);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi Niepewnosc(double? value, string oper)
            {
                SetField("Niepewnosc", value, OleDbType.Double, oper);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi Niepewnosc(Order order)
            {
                SetOrder("Niepewnosc", order);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi Niepewnosc()
            {
                AddField("Niepewnosc");
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi Odleglosc_zrodlo_sonda(double? value)
            {
                SetField("Odleglosc_zrodlo_sonda", value, OleDbType.Double);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi Odleglosc_zrodlo_sonda(double? value, string oper)
            {
                SetField("Odleglosc_zrodlo_sonda", value, OleDbType.Double, oper);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi Odleglosc_zrodlo_sonda(Order order)
            {
                SetOrder("Odleglosc_zrodlo_sonda", order);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi Odleglosc_zrodlo_sonda()
            {
                AddField("Odleglosc_zrodlo_sonda");
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi Osoba_sprawdzajaca(string value)
            {
                SetField("Osoba_sprawdzajaca", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi Osoba_sprawdzajaca(string value, string oper)
            {
                SetField("Osoba_sprawdzajaca", value, OleDbType.WChar, oper);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi Osoba_sprawdzajaca(Order order)
            {
                SetOrder("Osoba_sprawdzajaca", order);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi Osoba_sprawdzajaca()
            {
                AddField("Osoba_sprawdzajaca");
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi Osoba_wzorcujaca(string value)
            {
                SetField("Osoba_wzorcujaca", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi Osoba_wzorcujaca(string value, string oper)
            {
                SetField("Osoba_wzorcujaca", value, OleDbType.WChar, oper);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi Osoba_wzorcujaca(Order order)
            {
                SetOrder("Osoba_wzorcujaca", order);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi Osoba_wzorcujaca()
            {
                AddField("Osoba_wzorcujaca");
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi Podstawka(string value)
            {
                SetField("Podstawka", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi Podstawka(string value, string oper)
            {
                SetField("Podstawka", value, OleDbType.WChar, oper);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi Podstawka(Order order)
            {
                SetOrder("Podstawka", order);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi Podstawka()
            {
                AddField("Podstawka");
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi Temperatura(double? value)
            {
                SetField("Temperatura", value, OleDbType.Double);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi Temperatura(double? value, string oper)
            {
                SetField("Temperatura", value, OleDbType.Double, oper);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi Temperatura(Order order)
            {
                SetOrder("Temperatura", order);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi Temperatura()
            {
                AddField("Temperatura");
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi Tlo(double? value)
            {
                SetField("Tlo", value, OleDbType.Double);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi Tlo(double? value, string oper)
            {
                SetField("Tlo", value, OleDbType.Double, oper);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi Tlo(Order order)
            {
                SetOrder("Tlo", order);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi Tlo()
            {
                AddField("Tlo");
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi Uwagi(string value)
            {
                SetField("Uwagi", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi Uwagi(string value, string oper)
            {
                SetField("Uwagi", value, OleDbType.WChar, oper);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi Uwagi(Order order)
            {
                SetOrder("Uwagi", order);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi Uwagi()
            {
                AddField("Uwagi");
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi Wilgotnosc(double? value)
            {
                SetField("Wilgotnosc", value, OleDbType.Double);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi Wilgotnosc(double? value, string oper)
            {
                SetField("Wilgotnosc", value, OleDbType.Double, oper);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi Wilgotnosc(Order order)
            {
                SetOrder("Wilgotnosc", order);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi Wilgotnosc()
            {
                AddField("Wilgotnosc");
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi wspolczynnik(double? value)
            {
                SetField("wspolczynnik", value, OleDbType.Double);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi wspolczynnik(double? value, string oper)
            {
                SetField("wspolczynnik", value, OleDbType.Double, oper);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi wspolczynnik(Order order)
            {
                SetOrder("wspolczynnik", order);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi wspolczynnik()
            {
                AddField("wspolczynnik");
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi Zakres(string value)
            {
                SetField("Zakres", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi Zakres(string value, string oper)
            {
                SetField("Zakres", value, OleDbType.WChar, oper);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi Zakres(Order order)
            {
                SetOrder("Zakres", order);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi Zakres()
            {
                AddField("Zakres");
                return this;
            }
        }
        public class Row_Wzorcowanie_zrodlami_powierzchniowymi : Wiersz
        {
            public double? Cisnienie;
            public DateTime? Data_wzorcowania;
            public bool Dolacz;
            public int? ID_arkusza;
            public int? ID_jednostki;
            public int? ID_karty;
            public int? ID_sondy;
            public int ID_wzorcowania;
            public int? ID_zrodla;
            public string Inne_nastawy;
            public double? Mnoznik_korekcyjny;
            public string Napiecie_zasilania_sondy;
            public double? Niepewnosc;
            public double? Odleglosc_zrodlo_sonda;
            public string Osoba_sprawdzajaca;
            public string Osoba_wzorcujaca;
            public string Podstawka;
            public double? Temperatura;
            public double? Tlo;
            public string Uwagi;
            public double? Wilgotnosc;
            public double? wspolczynnik;
            public string Zakres;
            public Row_Wzorcowanie_zrodlami_powierzchniowymi(DataRow row)
            {
                _init(row, GetColsDict(row));
            }
            public Row_Wzorcowanie_zrodlami_powierzchniowymi(DataRow row, Dictionary<string, int> cols)
            {
                _init(row, cols);
            }
            private void _init(DataRow row, Dictionary<string, int> cols)
            {
                if (cols.ContainsKey("Cisnienie"))
                    Cisnienie = row.Field<double?>(cols["Cisnienie"]);
                if (cols.ContainsKey("Data_wzorcowania"))
                    Data_wzorcowania = row.Field<DateTime?>(cols["Data_wzorcowania"]);
                if (cols.ContainsKey("Dolacz"))
                    Dolacz = row.Field<bool>(cols["Dolacz"]);
                if (cols.ContainsKey("ID_arkusza"))
                    ID_arkusza = row.Field<int?>(cols["ID_arkusza"]);
                if (cols.ContainsKey("ID_jednostki"))
                    ID_jednostki = row.Field<int?>(cols["ID_jednostki"]);
                if (cols.ContainsKey("ID_karty"))
                    ID_karty = row.Field<int?>(cols["ID_karty"]);
                if (cols.ContainsKey("ID_sondy"))
                    ID_sondy = row.Field<int?>(cols["ID_sondy"]);
                if (cols.ContainsKey("ID_wzorcowania"))
                    ID_wzorcowania = row.Field<int>(cols["ID_wzorcowania"]);
                if (cols.ContainsKey("ID_zrodla"))
                    ID_zrodla = row.Field<int?>(cols["ID_zrodla"]);
                if (cols.ContainsKey("Inne_nastawy"))
                    Inne_nastawy = row.Field<string>(cols["Inne_nastawy"]);
                if (cols.ContainsKey("Mnoznik_korekcyjny"))
                    Mnoznik_korekcyjny = row.Field<double?>(cols["Mnoznik_korekcyjny"]);
                if (cols.ContainsKey("Napiecie_zasilania_sondy"))
                    Napiecie_zasilania_sondy = row.Field<string>(cols["Napiecie_zasilania_sondy"]);
                if (cols.ContainsKey("Niepewnosc"))
                    Niepewnosc = row.Field<double?>(cols["Niepewnosc"]);
                if (cols.ContainsKey("Odleglosc_zrodlo_sonda"))
                    Odleglosc_zrodlo_sonda = row.Field<double?>(cols["Odleglosc_zrodlo_sonda"]);
                if (cols.ContainsKey("Osoba_sprawdzajaca"))
                    Osoba_sprawdzajaca = row.Field<string>(cols["Osoba_sprawdzajaca"]);
                if (cols.ContainsKey("Osoba_wzorcujaca"))
                    Osoba_wzorcujaca = row.Field<string>(cols["Osoba_wzorcujaca"]);
                if (cols.ContainsKey("Podstawka"))
                    Podstawka = row.Field<string>(cols["Podstawka"]);
                if (cols.ContainsKey("Temperatura"))
                    Temperatura = row.Field<double?>(cols["Temperatura"]);
                if (cols.ContainsKey("Tlo"))
                    Tlo = row.Field<double?>(cols["Tlo"]);
                if (cols.ContainsKey("Uwagi"))
                    Uwagi = row.Field<string>(cols["Uwagi"]);
                if (cols.ContainsKey("Wilgotnosc"))
                    Wilgotnosc = row.Field<double?>(cols["Wilgotnosc"]);
                if (cols.ContainsKey("wspolczynnik"))
                    wspolczynnik = row.Field<double?>(cols["wspolczynnik"]);
                if (cols.ContainsKey("Zakres"))
                    Zakres = row.Field<string>(cols["Zakres"]);
            }
            internal static Row_Wzorcowanie_zrodlami_powierzchniowymi[] _GET(DataTable dane)
            {
                var result = new Row_Wzorcowanie_zrodlami_powierzchniowymi[dane.Rows.Count];
                var cols = GetColsDict(dane);
                var index = 0;
                foreach (DataRow row in dane.Rows)
                    result[index++] = new Row_Wzorcowanie_zrodlami_powierzchniowymi(row, cols);
                return result;
            }
        }
        public class Szablon_Zlecenia : Tabela
        {
            public Szablon_Zlecenia(BazaDanychWrapper baza, string nazwa) : base(baza, nazwa) { }
            public Szablon_Zlecenia UPDATE() { _UPDATE(); return this; }
            public Szablon_Zlecenia INSERT() { _INSERT(); return this; }
            public Szablon_Zlecenia DELETE() { _DELETE(); return this; }
            public Szablon_Zlecenia WHERE() { _WHERE(); return this; }
            public Szablon_Zlecenia INFO(string text) { _INFO(text); return this; }
            public Szablon_Zlecenia SELECT() { _SELECT(); return this; }
            public Szablon_Zlecenia ORDER_BY() { _ORDER_BY(); return this; }
            public Row_Zlecenia[] GET(int min = 0, int max = 999999999, bool allowException = false) { return Row_Zlecenia._GET(_GET(min, max, allowException)); }
            public Row_Zlecenia GET_ONE() { return Row_Zlecenia._GET(_GET(1, 1))[0]; }
            public Row_Zlecenia GET_OPTIONAL() { var r = Row_Zlecenia._GET(_GET(0, 1)); return r.Length > 0 ? r[0] : null; }
            public Szablon_Zlecenia Adres_platnika(string value)
            {
                SetField("Adres_platnika", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Zlecenia Adres_platnika(string value, string oper)
            {
                SetField("Adres_platnika", value, OleDbType.WChar, oper);
                return this;
            }
            public Szablon_Zlecenia Adres_platnika(Order order)
            {
                SetOrder("Adres_platnika", order);
                return this;
            }
            public Szablon_Zlecenia Adres_platnika()
            {
                AddField("Adres_platnika");
                return this;
            }
            public Szablon_Zlecenia Data_przyjecia(DateTime? value)
            {
                SetField("Data_przyjecia", value, OleDbType.Date);
                return this;
            }
            public Szablon_Zlecenia Data_przyjecia(DateTime? value, string oper)
            {
                SetField("Data_przyjecia", value, OleDbType.Date, oper);
                return this;
            }
            public Szablon_Zlecenia Data_przyjecia(Order order)
            {
                SetOrder("Data_przyjecia", order);
                return this;
            }
            public Szablon_Zlecenia Data_przyjecia()
            {
                AddField("Data_przyjecia");
                return this;
            }
            public Szablon_Zlecenia Data_zwrotu(DateTime? value)
            {
                SetField("Data_zwrotu", value, OleDbType.Date);
                return this;
            }
            public Szablon_Zlecenia Data_zwrotu(DateTime? value, string oper)
            {
                SetField("Data_zwrotu", value, OleDbType.Date, oper);
                return this;
            }
            public Szablon_Zlecenia Data_zwrotu(Order order)
            {
                SetOrder("Data_zwrotu", order);
                return this;
            }
            public Szablon_Zlecenia Data_zwrotu()
            {
                AddField("Data_zwrotu");
                return this;
            }
            public Szablon_Zlecenia Ekspres(bool value)
            {
                SetField("Ekspres", value, OleDbType.Boolean);
                return this;
            }
            public Szablon_Zlecenia Ekspres(bool value, string oper)
            {
                SetField("Ekspres", value, OleDbType.Boolean, oper);
                return this;
            }
            public Szablon_Zlecenia Ekspres(Order order)
            {
                SetOrder("Ekspres", order);
                return this;
            }
            public Szablon_Zlecenia Ekspres()
            {
                AddField("Ekspres");
                return this;
            }
            public Szablon_Zlecenia Forma_przyjecia(string value)
            {
                SetField("Forma_przyjecia", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Zlecenia Forma_przyjecia(string value, string oper)
            {
                SetField("Forma_przyjecia", value, OleDbType.WChar, oper);
                return this;
            }
            public Szablon_Zlecenia Forma_przyjecia(Order order)
            {
                SetOrder("Forma_przyjecia", order);
                return this;
            }
            public Szablon_Zlecenia Forma_przyjecia()
            {
                AddField("Forma_przyjecia");
                return this;
            }
            public Szablon_Zlecenia Forma_zwrotu(string value)
            {
                SetField("Forma_zwrotu", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Zlecenia Forma_zwrotu(string value, string oper)
            {
                SetField("Forma_zwrotu", value, OleDbType.WChar, oper);
                return this;
            }
            public Szablon_Zlecenia Forma_zwrotu(Order order)
            {
                SetOrder("Forma_zwrotu", order);
                return this;
            }
            public Szablon_Zlecenia Forma_zwrotu()
            {
                AddField("Forma_zwrotu");
                return this;
            }
            public Szablon_Zlecenia ID_zlecenia(int? value)
            {
                SetField("ID_zlecenia", value, OleDbType.Integer);
                return this;
            }
            public Szablon_Zlecenia ID_zlecenia(int? value, string oper)
            {
                SetField("ID_zlecenia", value, OleDbType.Integer, oper);
                return this;
            }
            public Szablon_Zlecenia ID_zlecenia(Order order)
            {
                SetOrder("ID_zlecenia", order);
                return this;
            }
            public Szablon_Zlecenia ID_zlecenia()
            {
                AddField("ID_zlecenia");
                return this;
            }
            public Szablon_Zlecenia ID_zleceniodawcy(int? value)
            {
                SetField("ID_zleceniodawcy", value, OleDbType.Integer);
                return this;
            }
            public Szablon_Zlecenia ID_zleceniodawcy(int? value, string oper)
            {
                SetField("ID_zleceniodawcy", value, OleDbType.Integer, oper);
                return this;
            }
            public Szablon_Zlecenia ID_zleceniodawcy(Order order)
            {
                SetOrder("ID_zleceniodawcy", order);
                return this;
            }
            public Szablon_Zlecenia ID_zleceniodawcy()
            {
                AddField("ID_zleceniodawcy");
                return this;
            }
            public Szablon_Zlecenia Nazwa_platnika(string value)
            {
                SetField("Nazwa_platnika", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Zlecenia Nazwa_platnika(string value, string oper)
            {
                SetField("Nazwa_platnika", value, OleDbType.WChar, oper);
                return this;
            }
            public Szablon_Zlecenia Nazwa_platnika(Order order)
            {
                SetOrder("Nazwa_platnika", order);
                return this;
            }
            public Szablon_Zlecenia Nazwa_platnika()
            {
                AddField("Nazwa_platnika");
                return this;
            }
            public Szablon_Zlecenia NIP_platnika(string value)
            {
                SetField("NIP_platnika", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Zlecenia NIP_platnika(string value, string oper)
            {
                SetField("NIP_platnika", value, OleDbType.WChar, oper);
                return this;
            }
            public Szablon_Zlecenia NIP_platnika(Order order)
            {
                SetOrder("NIP_platnika", order);
                return this;
            }
            public Szablon_Zlecenia NIP_platnika()
            {
                AddField("NIP_platnika");
                return this;
            }
            public Szablon_Zlecenia Nr_zlecenia_klienta(string value)
            {
                SetField("Nr_zlecenia_klienta", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Zlecenia Nr_zlecenia_klienta(string value, string oper)
            {
                SetField("Nr_zlecenia_klienta", value, OleDbType.WChar, oper);
                return this;
            }
            public Szablon_Zlecenia Nr_zlecenia_klienta(Order order)
            {
                SetOrder("Nr_zlecenia_klienta", order);
                return this;
            }
            public Szablon_Zlecenia Nr_zlecenia_klienta()
            {
                AddField("Nr_zlecenia_klienta");
                return this;
            }
            public Szablon_Zlecenia Nr_zlecenia_rejestr(int? value)
            {
                SetField("Nr_zlecenia_rejestr", value, OleDbType.Integer);
                return this;
            }
            public Szablon_Zlecenia Nr_zlecenia_rejestr(int? value, string oper)
            {
                SetField("Nr_zlecenia_rejestr", value, OleDbType.Integer, oper);
                return this;
            }
            public Szablon_Zlecenia Nr_zlecenia_rejestr(Order order)
            {
                SetOrder("Nr_zlecenia_rejestr", order);
                return this;
            }
            public Szablon_Zlecenia Nr_zlecenia_rejestr()
            {
                AddField("Nr_zlecenia_rejestr");
                return this;
            }
            public Szablon_Zlecenia Osoba_przyjmujaca(string value)
            {
                SetField("Osoba_przyjmujaca", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Zlecenia Osoba_przyjmujaca(string value, string oper)
            {
                SetField("Osoba_przyjmujaca", value, OleDbType.WChar, oper);
                return this;
            }
            public Szablon_Zlecenia Osoba_przyjmujaca(Order order)
            {
                SetOrder("Osoba_przyjmujaca", order);
                return this;
            }
            public Szablon_Zlecenia Osoba_przyjmujaca()
            {
                AddField("Osoba_przyjmujaca");
                return this;
            }
            public Szablon_Zlecenia Uwagi(string value)
            {
                SetField("Uwagi", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Zlecenia Uwagi(string value, string oper)
            {
                SetField("Uwagi", value, OleDbType.WChar, oper);
                return this;
            }
            public Szablon_Zlecenia Uwagi(Order order)
            {
                SetOrder("Uwagi", order);
                return this;
            }
            public Szablon_Zlecenia Uwagi()
            {
                AddField("Uwagi");
                return this;
            }
        }
        public class Row_Zlecenia : Wiersz
        {
            public string Adres_platnika;
            public DateTime? Data_przyjecia;
            public DateTime? Data_zwrotu;
            public bool Ekspres;
            public string Forma_przyjecia;
            public string Forma_zwrotu;
            public int? ID_zlecenia;
            public int? ID_zleceniodawcy;
            public string Nazwa_platnika;
            public string NIP_platnika;
            public string Nr_zlecenia_klienta;
            public int? Nr_zlecenia_rejestr;
            public string Osoba_przyjmujaca;
            public string Uwagi;
            public Row_Zlecenia(DataRow row)
            {
                _init(row, GetColsDict(row));
            }
            public Row_Zlecenia(DataRow row, Dictionary<string, int> cols)
            {
                _init(row, cols);
            }
            private void _init(DataRow row, Dictionary<string, int> cols)
            {
                if (cols.ContainsKey("Adres_platnika"))
                    Adres_platnika = row.Field<string>(cols["Adres_platnika"]);
                if (cols.ContainsKey("Data_przyjecia"))
                    Data_przyjecia = row.Field<DateTime?>(cols["Data_przyjecia"]);
                if (cols.ContainsKey("Data_zwrotu"))
                    Data_zwrotu = row.Field<DateTime?>(cols["Data_zwrotu"]);
                if (cols.ContainsKey("Ekspres"))
                    Ekspres = row.Field<bool>(cols["Ekspres"]);
                if (cols.ContainsKey("Forma_przyjecia"))
                    Forma_przyjecia = row.Field<string>(cols["Forma_przyjecia"]);
                if (cols.ContainsKey("Forma_zwrotu"))
                    Forma_zwrotu = row.Field<string>(cols["Forma_zwrotu"]);
                if (cols.ContainsKey("ID_zlecenia"))
                    ID_zlecenia = row.Field<int?>(cols["ID_zlecenia"]);
                if (cols.ContainsKey("ID_zleceniodawcy"))
                    ID_zleceniodawcy = row.Field<int?>(cols["ID_zleceniodawcy"]);
                if (cols.ContainsKey("Nazwa_platnika"))
                    Nazwa_platnika = row.Field<string>(cols["Nazwa_platnika"]);
                if (cols.ContainsKey("NIP_platnika"))
                    NIP_platnika = row.Field<string>(cols["NIP_platnika"]);
                if (cols.ContainsKey("Nr_zlecenia_klienta"))
                    Nr_zlecenia_klienta = row.Field<string>(cols["Nr_zlecenia_klienta"]);
                if (cols.ContainsKey("Nr_zlecenia_rejestr"))
                    Nr_zlecenia_rejestr = row.Field<int?>(cols["Nr_zlecenia_rejestr"]);
                if (cols.ContainsKey("Osoba_przyjmujaca"))
                    Osoba_przyjmujaca = row.Field<string>(cols["Osoba_przyjmujaca"]);
                if (cols.ContainsKey("Uwagi"))
                    Uwagi = row.Field<string>(cols["Uwagi"]);
            }
            internal static Row_Zlecenia[] _GET(DataTable dane)
            {
                var result = new Row_Zlecenia[dane.Rows.Count];
                var cols = GetColsDict(dane);
                var index = 0;
                foreach (DataRow row in dane.Rows)
                    result[index++] = new Row_Zlecenia(row, cols);
                return result;
            }
        }
        public class Szablon_Zleceniodawca : Tabela
        {
            public Szablon_Zleceniodawca(BazaDanychWrapper baza, string nazwa) : base(baza, nazwa) { }
            public Szablon_Zleceniodawca UPDATE() { _UPDATE(); return this; }
            public Szablon_Zleceniodawca INSERT() { _INSERT(); return this; }
            public Szablon_Zleceniodawca DELETE() { _DELETE(); return this; }
            public Szablon_Zleceniodawca WHERE() { _WHERE(); return this; }
            public Szablon_Zleceniodawca INFO(string text) { _INFO(text); return this; }
            public Szablon_Zleceniodawca SELECT() { _SELECT(); return this; }
            public Szablon_Zleceniodawca ORDER_BY() { _ORDER_BY(); return this; }
            public Row_Zleceniodawca[] GET(int min = 0, int max = 999999999, bool allowException = false) { return Row_Zleceniodawca._GET(_GET(min, max, allowException)); }
            public Row_Zleceniodawca GET_ONE() { return Row_Zleceniodawca._GET(_GET(1, 1))[0]; }
            public Row_Zleceniodawca GET_OPTIONAL() { var r = Row_Zleceniodawca._GET(_GET(0, 1)); return r.Length > 0 ? r[0] : null; }
            public Szablon_Zleceniodawca Adres(string value)
            {
                SetField("Adres", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Zleceniodawca Adres(string value, string oper)
            {
                SetField("Adres", value, OleDbType.WChar, oper);
                return this;
            }
            public Szablon_Zleceniodawca Adres(Order order)
            {
                SetOrder("Adres", order);
                return this;
            }
            public Szablon_Zleceniodawca Adres()
            {
                AddField("Adres");
                return this;
            }
            public Szablon_Zleceniodawca email(string value)
            {
                SetField("email", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Zleceniodawca email(string value, string oper)
            {
                SetField("email", value, OleDbType.WChar, oper);
                return this;
            }
            public Szablon_Zleceniodawca email(Order order)
            {
                SetOrder("email", order);
                return this;
            }
            public Szablon_Zleceniodawca email()
            {
                AddField("email");
                return this;
            }
            public Szablon_Zleceniodawca Faks(string value)
            {
                SetField("Faks", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Zleceniodawca Faks(string value, string oper)
            {
                SetField("Faks", value, OleDbType.WChar, oper);
                return this;
            }
            public Szablon_Zleceniodawca Faks(Order order)
            {
                SetOrder("Faks", order);
                return this;
            }
            public Szablon_Zleceniodawca Faks()
            {
                AddField("Faks");
                return this;
            }
            public Szablon_Zleceniodawca ID_zleceniodawcy(int value)
            {
                SetField("ID_zleceniodawcy", value, OleDbType.Integer);
                return this;
            }
            public Szablon_Zleceniodawca ID_zleceniodawcy(int value, string oper)
            {
                SetField("ID_zleceniodawcy", value, OleDbType.Integer, oper);
                return this;
            }
            public Szablon_Zleceniodawca ID_zleceniodawcy(Order order)
            {
                SetOrder("ID_zleceniodawcy", order);
                return this;
            }
            public Szablon_Zleceniodawca ID_zleceniodawcy()
            {
                AddField("ID_zleceniodawcy");
                return this;
            }
            public Szablon_Zleceniodawca IFJ(bool value)
            {
                SetField("IFJ", value, OleDbType.Boolean);
                return this;
            }
            public Szablon_Zleceniodawca IFJ(bool value, string oper)
            {
                SetField("IFJ", value, OleDbType.Boolean, oper);
                return this;
            }
            public Szablon_Zleceniodawca IFJ(Order order)
            {
                SetOrder("IFJ", order);
                return this;
            }
            public Szablon_Zleceniodawca IFJ()
            {
                AddField("IFJ");
                return this;
            }
            public Szablon_Zleceniodawca NIP(string value)
            {
                SetField("NIP", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Zleceniodawca NIP(string value, string oper)
            {
                SetField("NIP", value, OleDbType.WChar, oper);
                return this;
            }
            public Szablon_Zleceniodawca NIP(Order order)
            {
                SetOrder("NIP", order);
                return this;
            }
            public Szablon_Zleceniodawca NIP()
            {
                AddField("NIP");
                return this;
            }
            public Szablon_Zleceniodawca Osoba_kontaktowa(string value)
            {
                SetField("Osoba_kontaktowa", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Zleceniodawca Osoba_kontaktowa(string value, string oper)
            {
                SetField("Osoba_kontaktowa", value, OleDbType.WChar, oper);
                return this;
            }
            public Szablon_Zleceniodawca Osoba_kontaktowa(Order order)
            {
                SetOrder("Osoba_kontaktowa", order);
                return this;
            }
            public Szablon_Zleceniodawca Osoba_kontaktowa()
            {
                AddField("Osoba_kontaktowa");
                return this;
            }
            public Szablon_Zleceniodawca Rabat(string value)
            {
                SetField("Rabat", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Zleceniodawca Rabat(string value, string oper)
            {
                SetField("Rabat", value, OleDbType.WChar, oper);
                return this;
            }
            public Szablon_Zleceniodawca Rabat(Order order)
            {
                SetOrder("Rabat", order);
                return this;
            }
            public Szablon_Zleceniodawca Rabat()
            {
                AddField("Rabat");
                return this;
            }
            public Szablon_Zleceniodawca Telefon(string value)
            {
                SetField("Telefon", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Zleceniodawca Telefon(string value, string oper)
            {
                SetField("Telefon", value, OleDbType.WChar, oper);
                return this;
            }
            public Szablon_Zleceniodawca Telefon(Order order)
            {
                SetOrder("Telefon", order);
                return this;
            }
            public Szablon_Zleceniodawca Telefon()
            {
                AddField("Telefon");
                return this;
            }
            public Szablon_Zleceniodawca Uwagi(string value)
            {
                SetField("Uwagi", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Zleceniodawca Uwagi(string value, string oper)
            {
                SetField("Uwagi", value, OleDbType.WChar, oper);
                return this;
            }
            public Szablon_Zleceniodawca Uwagi(Order order)
            {
                SetOrder("Uwagi", order);
                return this;
            }
            public Szablon_Zleceniodawca Uwagi()
            {
                AddField("Uwagi");
                return this;
            }
            public Szablon_Zleceniodawca Zleceniodawca(string value)
            {
                SetField("Zleceniodawca", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Zleceniodawca Zleceniodawca(string value, string oper)
            {
                SetField("Zleceniodawca", value, OleDbType.WChar, oper);
                return this;
            }
            public Szablon_Zleceniodawca Zleceniodawca(Order order)
            {
                SetOrder("Zleceniodawca", order);
                return this;
            }
            public Szablon_Zleceniodawca Zleceniodawca()
            {
                AddField("Zleceniodawca");
                return this;
            }
        }
        public class Row_Zleceniodawca : Wiersz
        {
            public string Adres;
            public string email;
            public string Faks;
            public int ID_zleceniodawcy;
            public bool IFJ;
            public string NIP;
            public string Osoba_kontaktowa;
            public string Rabat;
            public string Telefon;
            public string Uwagi;
            public string Zleceniodawca;
            public Row_Zleceniodawca(DataRow row)
            {
                _init(row, GetColsDict(row));
            }
            public Row_Zleceniodawca(DataRow row, Dictionary<string, int> cols)
            {
                _init(row, cols);
            }
            private void _init(DataRow row, Dictionary<string, int> cols)
            {
                if (cols.ContainsKey("Adres"))
                    Adres = row.Field<string>(cols["Adres"]);
                if (cols.ContainsKey("email"))
                    email = row.Field<string>(cols["email"]);
                if (cols.ContainsKey("Faks"))
                    Faks = row.Field<string>(cols["Faks"]);
                if (cols.ContainsKey("ID_zleceniodawcy"))
                    ID_zleceniodawcy = row.Field<int>(cols["ID_zleceniodawcy"]);
                if (cols.ContainsKey("IFJ"))
                    IFJ = row.Field<bool>(cols["IFJ"]);
                if (cols.ContainsKey("NIP"))
                    NIP = row.Field<string>(cols["NIP"]);
                if (cols.ContainsKey("Osoba_kontaktowa"))
                    Osoba_kontaktowa = row.Field<string>(cols["Osoba_kontaktowa"]);
                if (cols.ContainsKey("Rabat"))
                    Rabat = row.Field<string>(cols["Rabat"]);
                if (cols.ContainsKey("Telefon"))
                    Telefon = row.Field<string>(cols["Telefon"]);
                if (cols.ContainsKey("Uwagi"))
                    Uwagi = row.Field<string>(cols["Uwagi"]);
                if (cols.ContainsKey("Zleceniodawca"))
                    Zleceniodawca = row.Field<string>(cols["Zleceniodawca"]);
            }
            internal static Row_Zleceniodawca[] _GET(DataTable dane)
            {
                var result = new Row_Zleceniodawca[dane.Rows.Count];
                var cols = GetColsDict(dane);
                var index = 0;
                foreach (DataRow row in dane.Rows)
                    result[index++] = new Row_Zleceniodawca(row, cols);
                return result;
            }
        }
        public class Szablon_zrodla_powierzchniowe : Tabela
        {
            public Szablon_zrodla_powierzchniowe(BazaDanychWrapper baza, string nazwa) : base(baza, nazwa) { }
            public Szablon_zrodla_powierzchniowe UPDATE() { _UPDATE(); return this; }
            public Szablon_zrodla_powierzchniowe INSERT() { _INSERT(); return this; }
            public Szablon_zrodla_powierzchniowe DELETE() { _DELETE(); return this; }
            public Szablon_zrodla_powierzchniowe WHERE() { _WHERE(); return this; }
            public Szablon_zrodla_powierzchniowe INFO(string text) { _INFO(text); return this; }
            public Szablon_zrodla_powierzchniowe SELECT() { _SELECT(); return this; }
            public Szablon_zrodla_powierzchniowe ORDER_BY() { _ORDER_BY(); return this; }
            public Row_zrodla_powierzchniowe[] GET(int min = 0, int max = 999999999, bool allowException = false) { return Row_zrodla_powierzchniowe._GET(_GET(min, max, allowException)); }
            public Row_zrodla_powierzchniowe GET_ONE() { return Row_zrodla_powierzchniowe._GET(_GET(1, 1))[0]; }
            public Row_zrodla_powierzchniowe GET_OPTIONAL() { var r = Row_zrodla_powierzchniowe._GET(_GET(0, 1)); return r.Length > 0 ? r[0] : null; }
            public Szablon_zrodla_powierzchniowe Czas_polowicznego_rozpadu(float? value)
            {
                SetField("Czas_polowicznego_rozpadu", value, OleDbType.Single);
                return this;
            }
            public Szablon_zrodla_powierzchniowe Czas_polowicznego_rozpadu(float? value, string oper)
            {
                SetField("Czas_polowicznego_rozpadu", value, OleDbType.Single, oper);
                return this;
            }
            public Szablon_zrodla_powierzchniowe Czas_polowicznego_rozpadu(Order order)
            {
                SetOrder("Czas_polowicznego_rozpadu", order);
                return this;
            }
            public Szablon_zrodla_powierzchniowe Czas_polowicznego_rozpadu()
            {
                AddField("Czas_polowicznego_rozpadu");
                return this;
            }
            public Szablon_zrodla_powierzchniowe Czas_polowicznego_rozpadu_dni(float? value)
            {
                SetField("Czas_polowicznego_rozpadu_dni", value, OleDbType.Single);
                return this;
            }
            public Szablon_zrodla_powierzchniowe Czas_polowicznego_rozpadu_dni(float? value, string oper)
            {
                SetField("Czas_polowicznego_rozpadu_dni", value, OleDbType.Single, oper);
                return this;
            }
            public Szablon_zrodla_powierzchniowe Czas_polowicznego_rozpadu_dni(Order order)
            {
                SetOrder("Czas_polowicznego_rozpadu_dni", order);
                return this;
            }
            public Szablon_zrodla_powierzchniowe Czas_polowicznego_rozpadu_dni()
            {
                AddField("Czas_polowicznego_rozpadu_dni");
                return this;
            }
            public Szablon_zrodla_powierzchniowe Id_zrodla(int value)
            {
                SetField("Id_zrodla", value, OleDbType.Integer);
                return this;
            }
            public Szablon_zrodla_powierzchniowe Id_zrodla(int value, string oper)
            {
                SetField("Id_zrodla", value, OleDbType.Integer, oper);
                return this;
            }
            public Szablon_zrodla_powierzchniowe Id_zrodla(Order order)
            {
                SetOrder("Id_zrodla", order);
                return this;
            }
            public Szablon_zrodla_powierzchniowe Id_zrodla()
            {
                AddField("Id_zrodla");
                return this;
            }
            public Szablon_zrodla_powierzchniowe Nazwa(string value)
            {
                SetField("Nazwa", value, OleDbType.WChar);
                return this;
            }
            public Szablon_zrodla_powierzchniowe Nazwa(string value, string oper)
            {
                SetField("Nazwa", value, OleDbType.WChar, oper);
                return this;
            }
            public Szablon_zrodla_powierzchniowe Nazwa(Order order)
            {
                SetOrder("Nazwa", order);
                return this;
            }
            public Szablon_zrodla_powierzchniowe Nazwa()
            {
                AddField("Nazwa");
                return this;
            }
            public Szablon_zrodla_powierzchniowe Niepewnosc(float? value)
            {
                SetField("Niepewnosc", value, OleDbType.Single);
                return this;
            }
            public Szablon_zrodla_powierzchniowe Niepewnosc(float? value, string oper)
            {
                SetField("Niepewnosc", value, OleDbType.Single, oper);
                return this;
            }
            public Szablon_zrodla_powierzchniowe Niepewnosc(Order order)
            {
                SetOrder("Niepewnosc", order);
                return this;
            }
            public Szablon_zrodla_powierzchniowe Niepewnosc()
            {
                AddField("Niepewnosc");
                return this;
            }
            public Szablon_zrodla_powierzchniowe Numer(int? value)
            {
                SetField("Numer", value, OleDbType.Integer);
                return this;
            }
            public Szablon_zrodla_powierzchniowe Numer(int? value, string oper)
            {
                SetField("Numer", value, OleDbType.Integer, oper);
                return this;
            }
            public Szablon_zrodla_powierzchniowe Numer(Order order)
            {
                SetOrder("Numer", order);
                return this;
            }
            public Szablon_zrodla_powierzchniowe Numer()
            {
                AddField("Numer");
                return this;
            }
        }
        public class Row_zrodla_powierzchniowe : Wiersz
        {
            public float? Czas_polowicznego_rozpadu;
            public float? Czas_polowicznego_rozpadu_dni;
            public int Id_zrodla;
            public string Nazwa;
            public float? Niepewnosc;
            public int? Numer;
            public Row_zrodla_powierzchniowe(DataRow row)
            {
                _init(row, GetColsDict(row));
            }
            public Row_zrodla_powierzchniowe(DataRow row, Dictionary<string, int> cols)
            {
                _init(row, cols);
            }
            private void _init(DataRow row, Dictionary<string, int> cols)
            {
                if (cols.ContainsKey("Czas_polowicznego_rozpadu"))
                    Czas_polowicznego_rozpadu = row.Field<float?>(cols["Czas_polowicznego_rozpadu"]);
                if (cols.ContainsKey("Czas_polowicznego_rozpadu_dni"))
                    Czas_polowicznego_rozpadu_dni = row.Field<float?>(cols["Czas_polowicznego_rozpadu_dni"]);
                if (cols.ContainsKey("Id_zrodla"))
                    Id_zrodla = row.Field<int>(cols["Id_zrodla"]);
                if (cols.ContainsKey("Nazwa"))
                    Nazwa = row.Field<string>(cols["Nazwa"]);
                if (cols.ContainsKey("Niepewnosc"))
                    Niepewnosc = row.Field<float?>(cols["Niepewnosc"]);
                if (cols.ContainsKey("Numer"))
                    Numer = row.Field<int?>(cols["Numer"]);
            }
            internal static Row_zrodla_powierzchniowe[] _GET(DataTable dane)
            {
                var result = new Row_zrodla_powierzchniowe[dane.Rows.Count];
                var cols = GetColsDict(dane);
                var index = 0;
                foreach (DataRow row in dane.Rows)
                    result[index++] = new Row_zrodla_powierzchniowe(row, cols);
                return result;
            }
        }
    }

    partial class BazaDanychWrapper
    {
        public Szablon.Szablon_Atesty_zrodel Atesty_zrodel { get { return new Szablon.Szablon_Atesty_zrodel(this, "Atesty_zrodel"); } }
        public Szablon.Szablon_Błędy_wklejania Błędy_wklejania { get { return new Szablon.Szablon_Błędy_wklejania(this, "Błędy wklejania"); } }
        public Szablon.Szablon_Budzetniepewnosci Budzetniepewnosci { get { return new Szablon.Szablon_Budzetniepewnosci(this, "Budzetniepewnosci"); } }
        public Szablon.Szablon_Cennik Cennik { get { return new Szablon.Szablon_Cennik(this, "Cennik"); } }
        public Szablon.Szablon_Dozymetry Dozymetry { get { return new Szablon.Szablon_Dozymetry(this, "Dozymetry"); } }
        public Szablon.Szablon_Hasla Hasla { get { return new Szablon.Szablon_Hasla(this, "Hasla"); } }
        public Szablon.Szablon_Jednostki Jednostki { get { return new Szablon.Szablon_Jednostki(this, "Jednostki"); } }
        public Szablon.Szablon_Karta_przyjecia Karta_przyjecia { get { return new Szablon.Szablon_Karta_przyjecia(this, "Karta_przyjecia"); } }
        public Szablon.Szablon_Pomiary_cez Pomiary_cez { get { return new Szablon.Szablon_Pomiary_cez(this, "Pomiary_cez"); } }
        public Szablon.Szablon_Pomiary_dawka Pomiary_dawka { get { return new Szablon.Szablon_Pomiary_dawka(this, "Pomiary_dawka"); } }
        public Szablon.Szablon_Pomiary_powierzchniowe Pomiary_powierzchniowe { get { return new Szablon.Szablon_Pomiary_powierzchniowe(this, "Pomiary_powierzchniowe"); } }
        public Szablon.Szablon_Pomiary_wzorcowe Pomiary_wzorcowe { get { return new Szablon.Szablon_Pomiary_wzorcowe(this, "Pomiary_wzorcowe"); } }
        public Szablon.Szablon_Protokoly_kalibracji_lawy Protokoly_kalibracji_lawy { get { return new Szablon.Szablon_Protokoly_kalibracji_lawy(this, "Protokoly_kalibracji_lawy"); } }
        public Szablon.Szablon_Slownik Slownik { get { return new Szablon.Szablon_Slownik(this, "Slownik"); } }
        public Szablon.Szablon_Sondy Sondy { get { return new Szablon.Szablon_Sondy(this, "Sondy"); } }
        public Szablon.Szablon_Stale Stale { get { return new Szablon.Szablon_Stale(this, "Stale"); } }
        public Szablon.Szablon_Swiadectwo Swiadectwo { get { return new Szablon.Szablon_Swiadectwo(this, "Swiadectwo"); } }
        public Szablon.Szablon_Sygnalizacja Sygnalizacja { get { return new Szablon.Szablon_Sygnalizacja(this, "Sygnalizacja"); } }
        public Szablon.Szablon_Sygnalizacja_dawka Sygnalizacja_dawka { get { return new Szablon.Szablon_Sygnalizacja_dawka(this, "Sygnalizacja_dawka"); } }
        public Szablon.Szablon_Wyniki_dawka Wyniki_dawka { get { return new Szablon.Szablon_Wyniki_dawka(this, "Wyniki_dawka"); } }
        public Szablon.Szablon_wyniki_moc_dawki wyniki_moc_dawki { get { return new Szablon.Szablon_wyniki_moc_dawki(this, "wyniki_moc_dawki"); } }
        public Szablon.Szablon_wzorcowanie_cezem wzorcowanie_cezem { get { return new Szablon.Szablon_wzorcowanie_cezem(this, "wzorcowanie_cezem"); } }
        public Szablon.Szablon_Wzorcowanie_zrodlami_powierzchniowymi Wzorcowanie_zrodlami_powierzchniowymi { get { return new Szablon.Szablon_Wzorcowanie_zrodlami_powierzchniowymi(this, "Wzorcowanie_zrodlami_powierzchniowymi"); } }
        public Szablon.Szablon_Zlecenia Zlecenia { get { return new Szablon.Szablon_Zlecenia(this, "Zlecenia"); } }
        public Szablon.Szablon_Zleceniodawca Zleceniodawca { get { return new Szablon.Szablon_Zleceniodawca(this, "Zleceniodawca"); } }
        public Szablon.Szablon_zrodla_powierzchniowe zrodla_powierzchniowe { get { return new Szablon.Szablon_zrodla_powierzchniowe(this, "zrodla_powierzchniowe"); } }
    }
}
