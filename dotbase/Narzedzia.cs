using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Narzedzia
{
    public class SkazeniaWartosciWzorcowoPomiarowe
    {
        public class SkazeniaWartoscWzorcowoPomiarowa
        {
            public double Wskazanie { get; set; }
            public double Tlo { get; set; }

            //********************************************************************
            public SkazeniaWartoscWzorcowoPomiarowa()
            //********************************************************************
            {

            }
        }

        public List<SkazeniaWartoscWzorcowoPomiarowa> Dane { get; set; }
        public string jednostka { get; set; }
        public string uwagi { get; set; }


        //********************************************************************
        public SkazeniaWartosciWzorcowoPomiarowe()
        //********************************************************************
        {
            Dane = new List<SkazeniaWartoscWzorcowoPomiarowa>();
        }
    }

    public class Pair<T,U>
    {
        public Pair(T first, U second)
        {
            First = first;
            Second = second;
        }

        public T First {get; private set;}
        public U Second {get; private set;}
    };

    public static class Precyzja
    {
        public static string Ustaw(double liczba)
        {
            double zwracana = liczba - (int)liczba;

            if (zwracana == 0)
                return "0";
            else if (liczba > 1)
                return "0.00";
            else
            {
                string temp = "0.0";
                int t = 0;
                double l = Math.Abs(Math.Log10(zwracana));

                if ((l - (int)l) == 0.0)
                {
                    t = (int)Math.Abs(Math.Log10(zwracana));
                }
                else
                {
                    t = (int)Math.Abs(Math.Log10(zwracana)) + 1;
                }   

                for (int i = 0; i < t; ++i)
                {
                    temp += '0';
                }

                return temp;
            }
        }
    }

    static class StaleWzorcowan
    {
        public enum stale
        {
            PLIK_POMOCNICZY_DAWKA,
            PLIK_POMOCNICZY_MOC_DAWKI,
            PLIK_POMOCNICZY_SKAZENIA,
            PLIK_POMOCNICZY_SYGNALIZACJA_DAWKI,
            PLIK_POMOCNICZY_SYGNALIZACJA_MOCY_DAWKI,
            PLIK_POMOCNICZY_SYGNALIZACJA_MOCY_DAWKI_OPIS,

            KARTA_PRZYJECIA,
            MELDUNEK,
            PISMO_PRZEWODNIE,

            PROTOKOL_DAWKA,
            PROTOKOL_MOC_DAWKI,
            PROTOKOL_SKAZENIA,
            PROTOKOL_SYGNALIZACJA_DAWKI,
            PROTOKOL_SYGNALIZACJA_MOCY_DAWKI_OPIS,
            PROTOKOL_SYGNALIZACJA_MOCY_DAWKI_TABELA,

            SWIADECTWO
        }
    }

    public class Sonda
    {
        public string Typ { get; private set; }
        public string NrFabryczny { get; private set; }

        public Sonda(string typ, string nrFabryczny)
        {
            Typ = typ;
            NrFabryczny = nrFabryczny;
        }
    }

    public class ListaSond
    {
        private List<Sonda> _ListaSond;

        //----------------------------------------
        public ListaSond()
        //----------------------------------------
        {
            _ListaSond = new List<Sonda>();
        }

        //----------------------------------------
        public void DodajSonde(Sonda sonda)
        //----------------------------------------
        {
            _ListaSond.Add(sonda);
        }

        //----------------------------------------
        public List<Sonda> Lista
        //----------------------------------------
        {
            get
            {
                return _ListaSond;
            }
        }
    }

    //----------------------------------------
    public class DaneOgolneDoZapisu
    //----------------------------------------
    {
        public string IdKarty { get; set; }
        public string IdWzorcowania { get; set; }
        public string IdArkusza { get; set; }
        public string Data { get; set; }
    }

    //----------------------------------------
    public class Warunki
    //----------------------------------------
    {
        private double _Cisnienie;
        private double _Temperatura;
        private double _Wilgotnosc;

        public double Cisnienie
        {
            get
            {
                return _Cisnienie;
            }
            set
            {
                if (value < 0)
                    _Cisnienie = 0;
                else
                    _Cisnienie = value;
            }
        }

        public double Temperatura
        {
            get
            {
                return _Temperatura;
            }
            set
            {
                if (value < 0)
                    _Temperatura = 0;
                else
                    _Temperatura = value;
            }
        }

        public double Wilgotnosc
        {
            get
            {
                return _Wilgotnosc;
            }
            set
            {
                if (value < 0)
                    _Wilgotnosc = 0;
                else
                    _Wilgotnosc = value;
            }
        }

        public string Uwagi { get; set; }

        public Warunki()
        {
            Cisnienie = 0.0;
            Temperatura = 0.0;
            Wilgotnosc = 0.0;
            Uwagi = "";
        }

        public Warunki(double cisnienie, double temperatura, double wilgotnosc, string uwagi)
        {
            Cisnienie = cisnienie;
            Temperatura = temperatura;
            Wilgotnosc = wilgotnosc;
            Uwagi = uwagi;
        }
    }

    //----------------------------------------
    public class DanePrzyrzaduDoZapisu
    //----------------------------------------
    {
        //----------------------------------------
        public DanePrzyrzaduDoZapisu()
        //----------------------------------------
        {
            Sondy = new Narzedzia.ListaSond();
        }
        
        public string TypDozymetru { get; set; }
        public string NrFabrycznyDozymetru { get; set; }
        public ListaSond Sondy { get; set; }
    }

    //----------------------------------------
    public class DaneProtokoluDoZapisu
    //----------------------------------------
    {
        public string Sprawdzajacy { get; set; }
        public string Wzorcujacy { get; set; }
    }

    //----------------------------------------
    class Jednostki
    //----------------------------------------
    {
        public List<int> Id { get; private set; }
        public List<string> Nazwy { get; private set; }

        public Jednostki()
        {
            Id = new List<int>();
            Nazwy = new List<string>();
        }

        public void Dodaj(int id, string nazwa)
        {
            Id.Add(id);
            Nazwy.Add(nazwa);
        }

        public void Czysc()
        {
            Id.Clear();
            Nazwy.Clear();
        }
    }

    //----------------------------------------
    class Protokoly
    //----------------------------------------
    {
        public List<int> Id { get; private set; }
        public List<DateTime> Daty { get; private set; }

        public Protokoly()
        {
            Id = new List<int>();
            Daty = new List<DateTime>();
        }

        public void Dodaj(int id, DateTime data)
        {
            Id.Add(id);
            Daty.Add(data);
        }

        public void Czysc()
        {
            Id.Clear();
            Daty.Clear();
        }
    }

    class Format
    {
        public static string PoprawFormat(string wejscie, uint liczbaMiejscPoPrzecinku)
        {
            if (wejscie.Length == 0)
                return "0,0";

            // Jeśli nie znaleziono częsci ułamkowej - dodają ją
            if (-1 == wejscie.IndexOf(','))
                wejscie += ",0";

            if (!Char.IsNumber(wejscie[wejscie.Length - 1]))
                return "0,0";

            int pozycjaPrzecinka = wejscie.IndexOf(',');
            if (0 == pozycjaPrzecinka)
                return "0,0";

            for (int i = 0; i < pozycjaPrzecinka; ++i)
            {
                if (!Char.IsNumber(wejscie[i]))
                    return "0,0";
            }

            // Liczba ma mieć jedynie 1 miejsce po przecinku
            return wejscie.Substring(0, pozycjaPrzecinka + 2);
        }
    }
}

namespace KlasyPomocniczeCez
{
    public class DaneOgolne : Narzedzia.DaneOgolneDoZapisu
    {
        public DaneOgolne()
            : base()
        {

        }
    }

    public class Przyrzad : Narzedzia.DanePrzyrzaduDoZapisu
    {
        public Przyrzad()
            : base()
        {

        }

        public string InneNastawy { get; set; }
        public string NapiecieZasilaniaSondy { get; set; }
        public bool PoprawneDane { get; private set; }
    }

    public class Warunki : Narzedzia.Warunki
    {
        //********************************************************************
        public Warunki()
            : base()
        //********************************************************************
        { }

        //********************************************************************
        public Warunki(double cisnienie, double temperatura, double wilgotnosc, string uwagi)
            : base(cisnienie, temperatura, wilgotnosc, uwagi)
        //********************************************************************
        { }
    }

    public class Protokol : Narzedzia.DaneProtokoluDoZapisu
    {
        //********************************************************************
        public Protokol()
            : base()
        { }
        //********************************************************************

        public bool Dolacz { get; set; }
    }
}

namespace KlasyPomocniczeDawka
{
    public class DawkaWartosciWzorcowoPomiarowe
    {
        public class DawkaWartoscWzorcowoPomiarowa
        {
            //********************************************************************
            public DawkaWartoscWzorcowoPomiarowa()
            //********************************************************************
            {
                WartoscWzorcowa = 0.0;
                Czas = 0.0;
                Wskazanie = 0.0;
                Dolaczyc = false;
            }

            //********************************************************************
            public DawkaWartoscWzorcowoPomiarowa(double wartoscWzorcowa, double czas, double wskazanie, bool dolaczyc)
            //********************************************************************
            {
                WartoscWzorcowa = wartoscWzorcowa;
                Czas = czas;
                Wskazanie = wskazanie;
                Dolaczyc = dolaczyc;
            }

            public double WartoscWzorcowa { get; set; }
            public double Czas { get; set; }
            public double Wskazanie { get; set; }
            public bool Dolaczyc { get; set; }
        }

        public List<DawkaWartoscWzorcowoPomiarowa> Dane { get; set; }

        public string jednostka { get; set; }
        public string protokol  { get; set; }
        public string odleglosc { get; set; }
        public string zrodlo    { get; set; }

        //********************************************************************
        public DawkaWartosciWzorcowoPomiarowe()
        //********************************************************************
        {
            Dane = new List<DawkaWartoscWzorcowoPomiarowa>();
        }
    }

    public class DawkaWspolczynniki : KlasyPomocniczeCez.Protokol
    {
        public double Wspolczynnik { get; set; }
        public double Niepewnosc   { get; set; }
        public double Odleglosc { get; set; }
        public double Zakres { get; set; }
        public int RownowaznikDawki { get; set; }

        //********************************************************************
        public DawkaWspolczynniki()
            : base()
        //********************************************************************
        {
        
        }
    }
}

namespace KlasyPomocniczeMocyDawki
{
    public class MocDawkiWartosciWzorcowoPomiarowe
    {
        public class MocDawkiWartoscWzorcowoPomiarowa
        {
            //********************************************************************
            public MocDawkiWartoscWzorcowoPomiarowa()
            //********************************************************************
            {
                IdZrodla = 0;
                Odleglosc = 0.0;
                Wskazanie = 0.0;
                Wahanie = 0.0;
                Zakres = "";
                Dolaczyc = false;
            }

            //********************************************************************
            public MocDawkiWartoscWzorcowoPomiarowa(short idZrodla, double odleglosc, double wskazanie, double wahanie, string zakres, bool dolaczyc)
            //********************************************************************
            {
                IdZrodla = idZrodla;
                Odleglosc = odleglosc;
                Wskazanie = wskazanie;
                Wahanie = wahanie;
                Zakres = zakres;
                Dolaczyc = dolaczyc;
            }

            public short IdZrodla { get; set; }
            public double Odleglosc { get; set; }
            public double Wskazanie { get; set; }
            public double Wahanie { get; set; }
            public string Zakres { get; set; }
            public bool Dolaczyc { get; set; }

        }

        public List<MocDawkiWartoscWzorcowoPomiarowa> Dane { get; set; }

        public string jednostka { get; set; }
        public string protokol { get; set; }
        public string tlo { get; set; }
        public string WielkoscFizyczna { get; set; }

        //********************************************************************
        public MocDawkiWartosciWzorcowoPomiarowe()
        //********************************************************************
        {
            Dane = new List<MocDawkiWartoscWzorcowoPomiarowa>();
        }
    }

    public class MocDawkiWspolczynniki : KlasyPomocniczeCez.Protokol
    {
        public class Wspolczynnik
        {
            public Wspolczynnik()
            { }

            public Wspolczynnik(double wartosc, double niepewnosc, double zakres)
            {
                Niepewnosc = niepewnosc;
                Wartosc = wartosc;
                Zakres = zakres;
            }

            public double Niepewnosc { get; set; }
            public double Wartosc { get; set; }
            public double Zakres { get; set; }
        }

        public List<Wspolczynnik> Dane { get; set; }

        //********************************************************************
        public MocDawkiWspolczynniki()
            : base()
        //********************************************************************
        {
            Dane = new List<Wspolczynnik>();
        }
    }
}

namespace KlasyPomocniczeSkazenia
{
    public class SkazeniaDaneOgolne : Narzedzia.DaneOgolneDoZapisu
    {
        public SkazeniaDaneOgolne()
            : base()
        {

        }

        public int IdZrodla { get; set; }
    }

    public class SkazeniaPrzyrzad : Narzedzia.DanePrzyrzaduDoZapisu
    {
        public SkazeniaPrzyrzad()
            : base()
        {}
        
        public string InneNastawy { get; set; }
        public string NapiecieZasilaniaSondy { get; set; }
        public string Zakres { get; set; }
    }

    public class SkazeniaWarunki : Narzedzia.Warunki
    {
        //********************************************************************
        public SkazeniaWarunki()
            : base()
        //********************************************************************
        { }

        public string Podstawka { get; set; }
        public double OdlegloscZrodloSonda { get; set; }
        public double WspolczynnikKorekcyjny { get; set; }
    }

    public class SkazeniaWspolczynniki : Narzedzia.DaneProtokoluDoZapisu
    {
        public double WspolczynnikKalibracyjny { get; set; }
        public double Niepewnosc { get; set; }
        public bool Dolaczyc { get; set; }

        //********************************************************************
        public SkazeniaWspolczynniki()
            : base()
        //********************************************************************
        {
        }
    }


}

namespace KlasyPomocniczeSygDawki
{
    public class DawkaWartosciWzorcowoPomiarowe
    {
        public class DawkaWartoscWzorcowoPomiarowa
        {
            public double Prog          { get; set; }
            public double Twzorcowy     { get; set; }
            public double Tzmierzony    { get; set; }
            public double WartRzeczywista { get; set; }
            public double Niepewnosc { get; set; }
            public double WartZmierzona { get; set; }
            public double Wspolczynnik {get; set; }
            public double Niepewnosc_wsp { get; set; }

            //********************************************************************
            public DawkaWartoscWzorcowoPomiarowa()
            //********************************************************************
            {
                Prog = Twzorcowy = Tzmierzony = WartRzeczywista = WartRzeczywista = Niepewnosc = WartZmierzona = Wspolczynnik = Niepewnosc_wsp = 0.0;
            }

            //********************************************************************
            public DawkaWartoscWzorcowoPomiarowa(double prog, double twzorcowy, double tzmierzony, double wartRzeczywista, double niepewnosc, double wartZmierzona, double wspolczynnik, double niepewnosc_wsp)
            //********************************************************************
            {
                Prog = prog;
                Twzorcowy = twzorcowy;
                Tzmierzony = tzmierzony;
                WartRzeczywista = wartRzeczywista;
                WartRzeczywista = wartRzeczywista;
                Niepewnosc = niepewnosc;
                WartZmierzona = wartZmierzona;
                Wspolczynnik = wspolczynnik;
                Niepewnosc_wsp = niepewnosc_wsp;
            }
        }

        public List<DawkaWartoscWzorcowoPomiarowa> Dane { get; set; }

        public string jednostka { get; set; }
        public string protokol { get; set; }
        public string odleglosc { get; set; }
        public string zrodlo { get; set; }

        //********************************************************************
        public DawkaWartosciWzorcowoPomiarowe()
        //********************************************************************
        {
            Dane = new List<DawkaWartoscWzorcowoPomiarowa>();
        }
    }
}

namespace KlasyPomocniczeSygMocyDawki
{
    public class DawkaWartosciWzorcowoPomiarowe
    {
        public class DawkaWartoscWzorcowoPomiarowa
        {
            public double Prog { get; set; }
            public double Niepewnosc { get; set; }
            public double Odleglosc1 { get; set; }
            public double Odleglosc2 { get; set; }
            public double WartoscZmierzona { get; set; }
            public double Zrodlo1 { get; set; }
            public double Zrodlo2 { get; set; }
            public double Wspolczynnik { get; set; }
            public double NiepewnoscWspolczynnika { get; set; }

            //********************************************************************
            public DawkaWartoscWzorcowoPomiarowa()
            //********************************************************************
            {
                Prog = Niepewnosc = Odleglosc1 = Odleglosc2 = Zrodlo1 = Zrodlo2 = Wspolczynnik = NiepewnoscWspolczynnika = 0.0;
            }

            //********************************************************************
            public DawkaWartoscWzorcowoPomiarowa(double prog, double nipewnosc, double odl1, double odl2, double zr1, double zr2, double wartosc, double wsp, double niep_wsp)
            //********************************************************************
            {
                Prog = prog;
                Niepewnosc = nipewnosc;
                Odleglosc1 = odl1;
                Odleglosc2 = odl2;
                Zrodlo1 = zr1;
                Zrodlo2 = zr2;
                WartoscZmierzona = wartosc;
                Wspolczynnik = wsp;
                NiepewnoscWspolczynnika = niep_wsp;
            }
        }

        public List<DawkaWartoscWzorcowoPomiarowa> Dane { get; set; }

        public string Jednostka { get; set; }
        public string Protokol { get; set; }
        public string Uwagi { get; set; }
        

        //********************************************************************
        public DawkaWartosciWzorcowoPomiarowe()
        //********************************************************************
        {
            Dane = new List<DawkaWartoscWzorcowoPomiarowa>();
        }
    }
}
