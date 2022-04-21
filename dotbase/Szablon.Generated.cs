using System;
using System.Data.OleDb;

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
            public Szablon_Atesty_zrodel Data_wzorcowania(DateTime value)
            {
                SetField("Data_wzorcowania", value, OleDbType.Date);
                return this;
            }
            public Szablon_Atesty_zrodel Emisja_powierzchniowa(double value)
            {
                SetField("Emisja_powierzchniowa", value, OleDbType.Double);
                return this;
            }
            public Szablon_Atesty_zrodel ID_atestu(short value)
            {
                SetField("ID_atestu", value, OleDbType.SmallInt);
                return this;
            }
            public Szablon_Atesty_zrodel ID_zrodla(short value)
            {
                SetField("ID_zrodla", value, OleDbType.SmallInt);
                return this;
            }
            public Szablon_Atesty_zrodel Niepewnosc(double value)
            {
                SetField("Niepewnosc", value, OleDbType.Double);
                return this;
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
            public Szablon_Budzetniepewnosci Uwagi(string value)
            {
                SetField("Uwagi", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Budzetniepewnosci wartosc(double value)
            {
                SetField("wartosc", value, OleDbType.Double);
                return this;
            }
            public Szablon_Budzetniepewnosci Wielkosc(string value)
            {
                SetField("Wielkosc", value, OleDbType.WChar);
                return this;
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
            public Szablon_Cennik Cena(double value)
            {
                SetField("Cena", value, OleDbType.Double);
                return this;
            }
            public Szablon_Cennik Usluga(string value)
            {
                SetField("Usluga", value, OleDbType.WChar);
                return this;
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
            public Szablon_Dozymetry ID_dozymetru(int value)
            {
                SetField("ID_dozymetru", value, OleDbType.Integer);
                return this;
            }
            public Szablon_Dozymetry Nazwa(string value)
            {
                SetField("Nazwa", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Dozymetry Nr_fabryczny(string value)
            {
                SetField("Nr_fabryczny", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Dozymetry Producent(string value)
            {
                SetField("Producent", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Dozymetry Rok_produkcji(string value)
            {
                SetField("Rok_produkcji", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Dozymetry Typ(string value)
            {
                SetField("Typ", value, OleDbType.WChar);
                return this;
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
            public Szablon_Hasla Haslo(string value)
            {
                SetField("Haslo", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Hasla Login(string value)
            {
                SetField("Login", value, OleDbType.WChar);
                return this;
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
            public Szablon_Jednostki ID_jednostki(int value)
            {
                SetField("ID_jednostki", value, OleDbType.Integer);
                return this;
            }
            public Szablon_Jednostki Jednostka(string value)
            {
                SetField("Jednostka", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Jednostki Przelicznik(float value)
            {
                SetField("Przelicznik", value, OleDbType.Single);
                return this;
            }
            public Szablon_Jednostki SI(bool value)
            {
                SetField("SI", value, OleDbType.Boolean);
                return this;
            }
            public Szablon_Jednostki Wielkosc_fizyczna(string value)
            {
                SetField("Wielkosc_fizyczna", value, OleDbType.WChar);
                return this;
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
            public Szablon_Karta_przyjecia Akcesoria(string value)
            {
                SetField("Akcesoria", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Karta_przyjecia Ameryk(bool value)
            {
                SetField("Ameryk", value, OleDbType.Boolean);
                return this;
            }
            public Szablon_Karta_przyjecia Cena(double value)
            {
                SetField("Cena", value, OleDbType.Double);
                return this;
            }
            public Szablon_Karta_przyjecia Chlor(bool value)
            {
                SetField("Chlor", value, OleDbType.Boolean);
                return this;
            }
            public Szablon_Karta_przyjecia Dawka(bool value)
            {
                SetField("Dawka", value, OleDbType.Boolean);
                return this;
            }
            public Szablon_Karta_przyjecia ID_dozymetru(int value)
            {
                SetField("ID_dozymetru", value, OleDbType.Integer);
                return this;
            }
            public Szablon_Karta_przyjecia ID_karty(int value)
            {
                SetField("ID_karty", value, OleDbType.Integer);
                return this;
            }
            public Szablon_Karta_przyjecia ID_zlecenia(int value)
            {
                SetField("ID_zlecenia", value, OleDbType.Integer);
                return this;
            }
            public Szablon_Karta_przyjecia Moc_dawki(bool value)
            {
                SetField("Moc_dawki", value, OleDbType.Boolean);
                return this;
            }
            public Szablon_Karta_przyjecia Nr_pisma(int value)
            {
                SetField("Nr_pisma", value, OleDbType.Integer);
                return this;
            }
            public Szablon_Karta_przyjecia Pluton(bool value)
            {
                SetField("Pluton", value, OleDbType.Boolean);
                return this;
            }
            public Szablon_Karta_przyjecia Rok(int value)
            {
                SetField("Rok", value, OleDbType.Integer);
                return this;
            }
            public Szablon_Karta_przyjecia Sprawdzenie(bool value)
            {
                SetField("Sprawdzenie", value, OleDbType.Boolean);
                return this;
            }
            public Szablon_Karta_przyjecia Stront_najsilniejszy(bool value)
            {
                SetField("Stront_najsilniejszy", value, OleDbType.Boolean);
                return this;
            }
            public Szablon_Karta_przyjecia Stront_silny(bool value)
            {
                SetField("Stront_silny", value, OleDbType.Boolean);
                return this;
            }
            public Szablon_Karta_przyjecia Stront_slaby(bool value)
            {
                SetField("Stront_slaby", value, OleDbType.Boolean);
                return this;
            }
            public Szablon_Karta_przyjecia Syg_dawki(bool value)
            {
                SetField("Syg_dawki", value, OleDbType.Boolean);
                return this;
            }
            public Szablon_Karta_przyjecia Syg_mocy_dawki(bool value)
            {
                SetField("Syg_mocy_dawki", value, OleDbType.Boolean);
                return this;
            }
            public Szablon_Karta_przyjecia Test_na_skazenia(string value)
            {
                SetField("Test_na_skazenia", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Karta_przyjecia Uszkodzony(bool value)
            {
                SetField("Uszkodzony", value, OleDbType.Boolean);
                return this;
            }
            public Szablon_Karta_przyjecia Uwagi(string value)
            {
                SetField("Uwagi", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Karta_przyjecia Wegiel_silny(bool value)
            {
                SetField("Wegiel_silny", value, OleDbType.Boolean);
                return this;
            }
            public Szablon_Karta_przyjecia Wegiel_slaby(bool value)
            {
                SetField("Wegiel_slaby", value, OleDbType.Boolean);
                return this;
            }
            public Szablon_Karta_przyjecia Wykonano(bool value)
            {
                SetField("Wykonano", value, OleDbType.Boolean);
                return this;
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
            public Szablon_Pomiary_cez Dolaczyc(bool value)
            {
                SetField("Dolaczyc", value, OleDbType.Boolean);
                return this;
            }
            public Szablon_Pomiary_cez ID_wzorcowania(int value)
            {
                SetField("ID_wzorcowania", value, OleDbType.Integer);
                return this;
            }
            public Szablon_Pomiary_cez ID_zrodla(short value)
            {
                SetField("ID_zrodla", value, OleDbType.SmallInt);
                return this;
            }
            public Szablon_Pomiary_cez Odleglosc(double value)
            {
                SetField("Odleglosc", value, OleDbType.Double);
                return this;
            }
            public Szablon_Pomiary_cez Wahanie(double value)
            {
                SetField("Wahanie", value, OleDbType.Double);
                return this;
            }
            public Szablon_Pomiary_cez Wskazanie(double value)
            {
                SetField("Wskazanie", value, OleDbType.Double);
                return this;
            }
            public Szablon_Pomiary_cez Zakres(string value)
            {
                SetField("Zakres", value, OleDbType.WChar);
                return this;
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
            public Szablon_Pomiary_dawka Czas(double value)
            {
                SetField("Czas", value, OleDbType.Double);
                return this;
            }
            public Szablon_Pomiary_dawka Dolaczyc(bool value)
            {
                SetField("Dolaczyc", value, OleDbType.Boolean);
                return this;
            }
            public Szablon_Pomiary_dawka ID_wzorcowania(int value)
            {
                SetField("ID_wzorcowania", value, OleDbType.Integer);
                return this;
            }
            public Szablon_Pomiary_dawka Wartosc_wzorcowa(double value)
            {
                SetField("Wartosc_wzorcowa", value, OleDbType.Double);
                return this;
            }
            public Szablon_Pomiary_dawka Wskazanie(double value)
            {
                SetField("Wskazanie", value, OleDbType.Double);
                return this;
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
            public Szablon_Pomiary_powierzchniowe ID_pomiaru(int value)
            {
                SetField("ID_pomiaru", value, OleDbType.Integer);
                return this;
            }
            public Szablon_Pomiary_powierzchniowe ID_wzorcowania(int value)
            {
                SetField("ID_wzorcowania", value, OleDbType.Integer);
                return this;
            }
            public Szablon_Pomiary_powierzchniowe Pomiar(double value)
            {
                SetField("Pomiar", value, OleDbType.Double);
                return this;
            }
            public Szablon_Pomiary_powierzchniowe Tlo(double value)
            {
                SetField("Tlo", value, OleDbType.Double);
                return this;
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
            public Szablon_Pomiary_wzorcowe ID_protokolu(short value)
            {
                SetField("ID_protokolu", value, OleDbType.SmallInt);
                return this;
            }
            public Szablon_Pomiary_wzorcowe ID_zrodla(short value)
            {
                SetField("ID_zrodla", value, OleDbType.SmallInt);
                return this;
            }
            public Szablon_Pomiary_wzorcowe Moc_kermy(double value)
            {
                SetField("Moc_kermy", value, OleDbType.Double);
                return this;
            }
            public Szablon_Pomiary_wzorcowe Niepewnosc(double value)
            {
                SetField("Niepewnosc", value, OleDbType.Double);
                return this;
            }
            public Szablon_Pomiary_wzorcowe Odleglosc(double value)
            {
                SetField("Odleglosc", value, OleDbType.Double);
                return this;
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
            public Szablon_Protokoly_kalibracji_lawy Data_kalibracji(DateTime value)
            {
                SetField("Data_kalibracji", value, OleDbType.Date);
                return this;
            }
            public Szablon_Protokoly_kalibracji_lawy ID_protokolu(short value)
            {
                SetField("ID_protokolu", value, OleDbType.SmallInt);
                return this;
            }
            public Szablon_Protokoly_kalibracji_lawy Nazwa(string value)
            {
                SetField("Nazwa", value, OleDbType.WChar);
                return this;
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
            public Szablon_Sondy ID_dozymetru(int value)
            {
                SetField("ID_dozymetru", value, OleDbType.Integer);
                return this;
            }
            public Szablon_Sondy ID_sondy(int value)
            {
                SetField("ID_sondy", value, OleDbType.Integer);
                return this;
            }
            public Szablon_Sondy Nr_fabryczny(string value)
            {
                SetField("Nr_fabryczny", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Sondy Typ(string value)
            {
                SetField("Typ", value, OleDbType.WChar);
                return this;
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
            public Szablon_Stale Nazwa(string value)
            {
                SetField("Nazwa", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Stale Uwagi(string value)
            {
                SetField("Uwagi", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Stale Wartosc(double value)
            {
                SetField("Wartosc", value, OleDbType.Double);
                return this;
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
            public Szablon_Swiadectwo Autoryzowal(string value)
            {
                SetField("Autoryzowal", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Swiadectwo Data_wykonania(DateTime value)
            {
                SetField("Data_wykonania", value, OleDbType.Date);
                return this;
            }
            public Szablon_Swiadectwo Data_wystawienia(DateTime value)
            {
                SetField("Data_wystawienia", value, OleDbType.Date);
                return this;
            }
            public Szablon_Swiadectwo Id_karty(int value)
            {
                SetField("Id_karty", value, OleDbType.Integer);
                return this;
            }
            public Szablon_Swiadectwo Poprawa(bool value)
            {
                SetField("Poprawa", value, OleDbType.Boolean);
                return this;
            }
            public Szablon_Swiadectwo Uwaga(string value)
            {
                SetField("Uwaga", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Swiadectwo UwagaD(string value)
            {
                SetField("UwagaD", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Swiadectwo UwagaMD(string value)
            {
                SetField("UwagaMD", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Swiadectwo UwagaS(string value)
            {
                SetField("UwagaS", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Swiadectwo UwagaSD(string value)
            {
                SetField("UwagaSD", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Swiadectwo UwagaSMD(string value)
            {
                SetField("UwagaSMD", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Swiadectwo Waznosc_dwa_lata(bool value)
            {
                SetField("Waznosc_dwa_lata", value, OleDbType.Boolean);
                return this;
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
            public Szablon_Sygnalizacja ID_wzorcowania(int value)
            {
                SetField("ID_wzorcowania", value, OleDbType.Integer);
                return this;
            }
            public Szablon_Sygnalizacja Niepewnosc(double value)
            {
                SetField("Niepewnosc", value, OleDbType.Double);
                return this;
            }
            public Szablon_Sygnalizacja odleglosc1(double value)
            {
                SetField("odleglosc1", value, OleDbType.Double);
                return this;
            }
            public Szablon_Sygnalizacja odleglosc2(double value)
            {
                SetField("odleglosc2", value, OleDbType.Double);
                return this;
            }
            public Szablon_Sygnalizacja Prog(double value)
            {
                SetField("Prog", value, OleDbType.Double);
                return this;
            }
            public Szablon_Sygnalizacja Uwagi(string value)
            {
                SetField("Uwagi", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Sygnalizacja Wartosc_zmierzona(double value)
            {
                SetField("Wartosc_zmierzona", value, OleDbType.Double);
                return this;
            }
            public Szablon_Sygnalizacja zrodlo1(double value)
            {
                SetField("zrodlo1", value, OleDbType.Double);
                return this;
            }
            public Szablon_Sygnalizacja zrodlo2(double value)
            {
                SetField("zrodlo2", value, OleDbType.Double);
                return this;
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
            public Szablon_Sygnalizacja_dawka Czas_zmierzony(double value)
            {
                SetField("Czas_zmierzony", value, OleDbType.Double);
                return this;
            }
            public Szablon_Sygnalizacja_dawka ID_wzorcowania(int value)
            {
                SetField("ID_wzorcowania", value, OleDbType.Integer);
                return this;
            }
            public Szablon_Sygnalizacja_dawka ID_zrodla(int value)
            {
                SetField("ID_zrodla", value, OleDbType.Integer);
                return this;
            }
            public Szablon_Sygnalizacja_dawka Odleglosc(double value)
            {
                SetField("Odleglosc", value, OleDbType.Double);
                return this;
            }
            public Szablon_Sygnalizacja_dawka Prog(double value)
            {
                SetField("Prog", value, OleDbType.Double);
                return this;
            }
            public Szablon_Sygnalizacja_dawka Wartosc_wzorcowa(double value)
            {
                SetField("Wartosc_wzorcowa", value, OleDbType.Double);
                return this;
            }
            public Szablon_Sygnalizacja_dawka Wartosc_zmierzona(double value)
            {
                SetField("Wartosc_zmierzona", value, OleDbType.Double);
                return this;
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
            public Szablon_Wyniki_dawka ID_wzorcowania(int value)
            {
                SetField("ID_wzorcowania", value, OleDbType.Integer);
                return this;
            }
            public Szablon_Wyniki_dawka ID_zrodla(int value)
            {
                SetField("ID_zrodla", value, OleDbType.Integer);
                return this;
            }
            public Szablon_Wyniki_dawka Niepewnosc(double value)
            {
                SetField("Niepewnosc", value, OleDbType.Double);
                return this;
            }
            public Szablon_Wyniki_dawka Odleglosc(double value)
            {
                SetField("Odleglosc", value, OleDbType.Double);
                return this;
            }
            public Szablon_Wyniki_dawka Wielkosc_fizyczna(int value)
            {
                SetField("Wielkosc_fizyczna", value, OleDbType.Integer);
                return this;
            }
            public Szablon_Wyniki_dawka Wspolczynnik(double value)
            {
                SetField("Wspolczynnik", value, OleDbType.Double);
                return this;
            }
            public Szablon_Wyniki_dawka Zakres(double value)
            {
                SetField("Zakres", value, OleDbType.Double);
                return this;
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
            public Szablon_wyniki_moc_dawki ID_wzorcowania(int value)
            {
                SetField("ID_wzorcowania", value, OleDbType.Integer);
                return this;
            }
            public Szablon_wyniki_moc_dawki Niepewnosc(double value)
            {
                SetField("Niepewnosc", value, OleDbType.Double);
                return this;
            }
            public Szablon_wyniki_moc_dawki Wspolczynnik(double value)
            {
                SetField("Wspolczynnik", value, OleDbType.Double);
                return this;
            }
            public Szablon_wyniki_moc_dawki ZAKRES(double value)
            {
                SetField("ZAKRES", value, OleDbType.Double);
                return this;
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
            public Szablon_wzorcowanie_cezem Cisnienie(double value)
            {
                SetField("Cisnienie", value, OleDbType.Double);
                return this;
            }
            public Szablon_wzorcowanie_cezem Data_wzorcowania(DateTime value)
            {
                SetField("Data_wzorcowania", value, OleDbType.Date);
                return this;
            }
            public Szablon_wzorcowanie_cezem Dolacz(bool value)
            {
                SetField("Dolacz", value, OleDbType.Boolean);
                return this;
            }
            public Szablon_wzorcowanie_cezem ID_arkusza(short value)
            {
                SetField("ID_arkusza", value, OleDbType.SmallInt);
                return this;
            }
            public Szablon_wzorcowanie_cezem ID_jednostki(int value)
            {
                SetField("ID_jednostki", value, OleDbType.Integer);
                return this;
            }
            public Szablon_wzorcowanie_cezem ID_karty(int value)
            {
                SetField("ID_karty", value, OleDbType.Integer);
                return this;
            }
            public Szablon_wzorcowanie_cezem ID_protokolu(short value)
            {
                SetField("ID_protokolu", value, OleDbType.SmallInt);
                return this;
            }
            public Szablon_wzorcowanie_cezem ID_sondy(int value)
            {
                SetField("ID_sondy", value, OleDbType.Integer);
                return this;
            }
            public Szablon_wzorcowanie_cezem ID_wzorcowania(int value)
            {
                SetField("ID_wzorcowania", value, OleDbType.Integer);
                return this;
            }
            public Szablon_wzorcowanie_cezem Inne_nastawy(string value)
            {
                SetField("Inne_nastawy", value, OleDbType.WChar);
                return this;
            }
            public Szablon_wzorcowanie_cezem Napiecie_zasilania_sondy(string value)
            {
                SetField("Napiecie_zasilania_sondy", value, OleDbType.WChar);
                return this;
            }
            public Szablon_wzorcowanie_cezem Osoba_sprawdzajaca(string value)
            {
                SetField("Osoba_sprawdzajaca", value, OleDbType.WChar);
                return this;
            }
            public Szablon_wzorcowanie_cezem Osoba_wzorcujaca(string value)
            {
                SetField("Osoba_wzorcujaca", value, OleDbType.WChar);
                return this;
            }
            public Szablon_wzorcowanie_cezem Rodzaj_wzorcowania(string value)
            {
                SetField("Rodzaj_wzorcowania", value, OleDbType.WChar);
                return this;
            }
            public Szablon_wzorcowanie_cezem Temperatura(double value)
            {
                SetField("Temperatura", value, OleDbType.Double);
                return this;
            }
            public Szablon_wzorcowanie_cezem Tlo(string value)
            {
                SetField("Tlo", value, OleDbType.WChar);
                return this;
            }
            public Szablon_wzorcowanie_cezem Uwagi(string value)
            {
                SetField("Uwagi", value, OleDbType.WChar);
                return this;
            }
            public Szablon_wzorcowanie_cezem Wielkosc_fizyczna(string value)
            {
                SetField("Wielkosc_fizyczna", value, OleDbType.WChar);
                return this;
            }
            public Szablon_wzorcowanie_cezem wilgotnosc(double value)
            {
                SetField("wilgotnosc", value, OleDbType.Double);
                return this;
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
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi Cisnienie(double value)
            {
                SetField("Cisnienie", value, OleDbType.Double);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi Data_wzorcowania(DateTime value)
            {
                SetField("Data_wzorcowania", value, OleDbType.Date);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi Dolacz(bool value)
            {
                SetField("Dolacz", value, OleDbType.Boolean);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi ID_arkusza(int value)
            {
                SetField("ID_arkusza", value, OleDbType.Integer);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi ID_jednostki(int value)
            {
                SetField("ID_jednostki", value, OleDbType.Integer);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi ID_karty(int value)
            {
                SetField("ID_karty", value, OleDbType.Integer);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi ID_sondy(int value)
            {
                SetField("ID_sondy", value, OleDbType.Integer);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi ID_wzorcowania(int value)
            {
                SetField("ID_wzorcowania", value, OleDbType.Integer);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi ID_zrodla(int value)
            {
                SetField("ID_zrodla", value, OleDbType.Integer);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi Inne_nastawy(string value)
            {
                SetField("Inne_nastawy", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi Mnoznik_korekcyjny(double value)
            {
                SetField("Mnoznik_korekcyjny", value, OleDbType.Double);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi Napiecie_zasilania_sondy(string value)
            {
                SetField("Napiecie_zasilania_sondy", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi Niepewnosc(double value)
            {
                SetField("Niepewnosc", value, OleDbType.Double);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi Odleglosc_zrodlo_sonda(double value)
            {
                SetField("Odleglosc_zrodlo_sonda", value, OleDbType.Double);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi Osoba_sprawdzajaca(string value)
            {
                SetField("Osoba_sprawdzajaca", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi Osoba_wzorcujaca(string value)
            {
                SetField("Osoba_wzorcujaca", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi Podstawka(string value)
            {
                SetField("Podstawka", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi Temperatura(double value)
            {
                SetField("Temperatura", value, OleDbType.Double);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi Tlo(double value)
            {
                SetField("Tlo", value, OleDbType.Double);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi Uwagi(string value)
            {
                SetField("Uwagi", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi Wilgotnosc(double value)
            {
                SetField("Wilgotnosc", value, OleDbType.Double);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi wspolczynnik(double value)
            {
                SetField("wspolczynnik", value, OleDbType.Double);
                return this;
            }
            public Szablon_Wzorcowanie_zrodlami_powierzchniowymi Zakres(string value)
            {
                SetField("Zakres", value, OleDbType.WChar);
                return this;
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
            public Szablon_Zlecenia Adres_platnika(string value)
            {
                SetField("Adres_platnika", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Zlecenia Data_przyjecia(DateTime value)
            {
                SetField("Data_przyjecia", value, OleDbType.Date);
                return this;
            }
            public Szablon_Zlecenia Data_zwrotu(DateTime value)
            {
                SetField("Data_zwrotu", value, OleDbType.Date);
                return this;
            }
            public Szablon_Zlecenia Ekspres(bool value)
            {
                SetField("Ekspres", value, OleDbType.Boolean);
                return this;
            }
            public Szablon_Zlecenia Forma_przyjecia(string value)
            {
                SetField("Forma_przyjecia", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Zlecenia Forma_zwrotu(string value)
            {
                SetField("Forma_zwrotu", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Zlecenia ID_zlecenia(int value)
            {
                SetField("ID_zlecenia", value, OleDbType.Integer);
                return this;
            }
            public Szablon_Zlecenia ID_zleceniodawcy(int value)
            {
                SetField("ID_zleceniodawcy", value, OleDbType.Integer);
                return this;
            }
            public Szablon_Zlecenia Nazwa_platnika(string value)
            {
                SetField("Nazwa_platnika", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Zlecenia NIP_platnika(string value)
            {
                SetField("NIP_platnika", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Zlecenia Nr_zlecenia_klienta(string value)
            {
                SetField("Nr_zlecenia_klienta", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Zlecenia Nr_zlecenia_rejestr(int value)
            {
                SetField("Nr_zlecenia_rejestr", value, OleDbType.Integer);
                return this;
            }
            public Szablon_Zlecenia Osoba_przyjmujaca(string value)
            {
                SetField("Osoba_przyjmujaca", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Zlecenia Uwagi(string value)
            {
                SetField("Uwagi", value, OleDbType.WChar);
                return this;
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
            public Szablon_Zleceniodawca Adres(string value)
            {
                SetField("Adres", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Zleceniodawca email(string value)
            {
                SetField("email", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Zleceniodawca Faks(string value)
            {
                SetField("Faks", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Zleceniodawca ID_zleceniodawcy(int value)
            {
                SetField("ID_zleceniodawcy", value, OleDbType.Integer);
                return this;
            }
            public Szablon_Zleceniodawca IFJ(bool value)
            {
                SetField("IFJ", value, OleDbType.Boolean);
                return this;
            }
            public Szablon_Zleceniodawca NIP(string value)
            {
                SetField("NIP", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Zleceniodawca Osoba_kontaktowa(string value)
            {
                SetField("Osoba_kontaktowa", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Zleceniodawca Rabat(string value)
            {
                SetField("Rabat", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Zleceniodawca Telefon(string value)
            {
                SetField("Telefon", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Zleceniodawca Uwagi(string value)
            {
                SetField("Uwagi", value, OleDbType.WChar);
                return this;
            }
            public Szablon_Zleceniodawca Zleceniodawca(string value)
            {
                SetField("Zleceniodawca", value, OleDbType.WChar);
                return this;
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
            public Szablon_zrodla_powierzchniowe Czas_polowicznego_rozpadu(float value)
            {
                SetField("Czas_polowicznego_rozpadu", value, OleDbType.Single);
                return this;
            }
            public Szablon_zrodla_powierzchniowe Id_zrodla(int value)
            {
                SetField("Id_zrodla", value, OleDbType.Integer);
                return this;
            }
            public Szablon_zrodla_powierzchniowe Nazwa(string value)
            {
                SetField("Nazwa", value, OleDbType.WChar);
                return this;
            }
            public Szablon_zrodla_powierzchniowe Numer(int value)
            {
                SetField("Numer", value, OleDbType.Integer);
                return this;
            }
        }
    }
    
    partial class BazaDanychWrapper
    {
        public Szablon.Szablon_Atesty_zrodel Atesty_zrodel { get { return new Szablon.Szablon_Atesty_zrodel(this, "Atesty_zrodel"); } }
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
