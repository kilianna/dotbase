using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
using KlasyPomocniczeSkazenia;
using Narzedzia;

namespace DotBase
{
    class WzorcowanieZrodlamiPow : WspolneDaneWzorcowan
    {
        #region Klasy Pomocnicze
        public class Przyrzad
        {
            public string typDozymetru { get; set; }
            public string nrFabrycznyDozymetru { get; set; }
            public Narzedzia.ListaSond sondy { get; set; }
            
            public string typSondy { get; set; }
            public string nrFabrycznySondy { get; set; }

            public string napiecieZasilaniaSondy { get; set; }
            public string zakres { get; set; }
            public string inneNastawy { get; set; }
            public DateTime _Data;

            public Przyrzad()
            {
                _Data = new DateTime();
                sondy = new ListaSond();
            }
        }

        public class Warunki
        {
            public double cisnienie { get; set; }
            public double wilgotnosc { get; set; }
            public double temperatura { get; set; }
            public string podstawka { get; set; }
            public double odleglosc { get; set; }
            public double mnoznikKorekcyjny { get; set; }
        }

        public class Wspolczynnik
        {
            public double wspolczynnik { get; set; }
            public double niepewnnosc { get; set; }
            public double wspolczynnikPoprzedni { get; set; }
            public double niepewnoscPoprzednia { get; set; }
            public string osobaWzorcujaca { get; set; }
            public string osobaSprawdzajaca { get; set; }
            public bool dolaczyc { get; set; }

        }

        public class Pomiary
        {
            public List<double> pomiar;
            public List<double> tlo;
            public string jednostka;
            public string uwagi;

            //---------------------------------------------------------------
            public Pomiary()
            //---------------------------------------------------------------
            {
                pomiar = new List<double>();
                tlo = new List<double>();
            }
        }

        #endregion

        // określa typ wzorcowania dla jakiego stworzono klasę. Uniemożlwi to prztwarzanie innych
        // typów wzorcowań przez stworzony obiekt.
        private int _IdZrodlaReferencyjnego;
        public Pomiary DanePomiary { get; private set; }
        public Warunki DaneWarunki { get; private set; }
        public Przyrzad DanePrzyrzadow { get; private set; }
        public Wspolczynnik DaneWspolczynnikow { get; private set; }

        #region Dane będące jedynie zapisywanymi
        private SkazeniaDaneOgolne _DaneOgolneDoZapisu;
        private SkazeniaPrzyrzad _PrzyrzadDoZapisu;
        private SkazeniaWarunki _WarunkiDoZapisu;
        private SkazeniaWspolczynniki _WspolczynnikiDoZapisu;
        private SkazeniaWartosciWzorcowoPomiarowe _WartosciWzorcowoPomiaroweDoZapisu;
        #endregion

        //---------------------------------------------------------------
        public DataTable Dane
        //---------------------------------------------------------------
        {
            get
            {
                return _OdpowiedzBazy;
            }
        }

        //---------------------------------------------------------------
        public bool LiczWspolczynnikiOrazNiepewnoscOld(out double wspolczynnik_kalibracyjny, out double niepewnoscWspolczynnika, out int precyzja, ref DataGridView dataGridView1, DateTime data, double wspolczynnik_korekcyjny)
        //---------------------------------------------------------------
        {
            // sprawdź czy źródło ok
            wspolczynnik_kalibracyjny = 0;
            niepewnoscWspolczynnika = 0;

            List<double> wskazania = new List<double>();
            List<double> tla = new List<double>();

            //-------------Obliczenie średniej wskazań tła----------------
            for (int i = 0; i < dataGridView1.Rows.Count - 1; ++i)
            {
                DataGridViewRow wiersz = dataGridView1.Rows[i];

                try
                {
                    wskazania.Add(N.doubleParse(wiersz.Cells["Wskazanie"].Value.ToString()));
                    tla.Add(N.doubleParse(wiersz.Cells["Tlo"].Value.ToString()));
                }
                catch (Exception)
                {
                    MyMessageBox.Show("Część danych została prawdopodobnie źle wpisana.", "Uwaga");
                }
            }

            double sredniaWskazan;
            double sredniaTel;

            if (wskazania.Count != 0)
                sredniaWskazan = wskazania.Average();
            else
                sredniaWskazan = 0;

            if (wskazania.Count != 0)
                sredniaTel = tla.Average();
            else
                sredniaTel = 0;

            //-----------------------------------------------------------

            _Zapytanie = String.Format("SELECT czas_polowicznego_rozpadu FROM Zrodla_powierzchniowe WHERE id_zrodla = {0}", IdZrodla);
            double czas_polowicznego_rozpadu = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<float>(0);

            _Zapytanie = String.Format("SELECT emisja_powierzchniowa, niepewnosc, data_wzorcowania FROM Atesty_zrodel WHERE ", data.ToShortDateString())
                       + String.Format("id_zrodla = {0} AND data_wzorcowania=(SELECT MAX(data_wzorcowania) FROM Atesty_zrodel WHERE id_zrodla = {0})", IdZrodla);

            double emisja_powierzchniowa = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<double>(0);
            double niepewnosc = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<double>(1);
            double czas = N.doubleParse((data - _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<DateTime>(2)).Days.ToString());


            //--------------------------------------OBLICZENIE WSPÓŁCZYNNIKA KALIBRACYJNEGO-------------------------------------------
            // 365.25 - zamiana dni na lata
            // poniższy wzór to
            // współ_korekcyjny * emisja_powierzchniowa * exp(-różnica w dniach / 365.25 * ln2 / czas_połowicznego_rozpadu) / (srednia_pomiaru - srednia_tla)

            wspolczynnik_kalibracyjny = wspolczynnik_korekcyjny * emisja_powierzchniowa * Math.Exp(-czas / 365.25 * Math.Log(2.0)
                                        / czas_polowicznego_rozpadu) / (sredniaWskazan - sredniaTel);

            //---------------------------------------LICZENIE NIEPEWNOŚCI---------------------------------------------------------------
            double odchylenie_czesc_pomiar = 0;

            // liczenie odchylenia standardowego
            for (int i = 0; i < wskazania.Count; ++i)
            {
                odchylenie_czesc_pomiar += Math.Pow((wskazania[i] - sredniaWskazan), 2.0);
            }

            double odchylenie_pomiar = (1.0 / Math.Sqrt(wskazania.Count * (wskazania.Count - 1.0)) * Math.Sqrt(odchylenie_czesc_pomiar)) / (sredniaWskazan - sredniaTel);

            double odchylenie_czesc_tlo = 0;

            for (int i = 0; i < tla.Count; ++i)
            {
                odchylenie_czesc_tlo += Math.Pow((tla[i] - sredniaTel), 2.0);
            }

            double odchylenie_tlo = 1.0 / Math.Sqrt(tla.Count * (tla.Count - 1.0)) * Math.Sqrt(odchylenie_czesc_tlo) / (sredniaWskazan - sredniaTel);

            double ukj = _BazaDanych.TworzTabeleDanych("SELECT Wartosc FROM Stale WHERE Nazwa='ukj'").Rows[0].Field<double>(0);
            double ukw = _BazaDanych.TworzTabeleDanych("SELECT Wartosc FROM Stale WHERE Nazwa='ukw'").Rows[0].Field<double>(0);

            niepewnoscWspolczynnika = Math.Sqrt(Math.Pow(odchylenie_pomiar, 2.0) + Math.Pow(odchylenie_tlo, 2.0) + Math.Pow(niepewnosc, 2.0) + Math.Pow(ukw, 2.0) + Math.Pow(ukj, 2.0)) * 2.0 * wspolczynnik_kalibracyjny;
            precyzja = 0;//Narzedzia.Precyzja.Ustaw(niepewnoscWspolczynnika);

            return true;
        }

        //---------------------------------------------------------------
        public bool LiczWspolczynnikiOrazNiepewnosc20230915(out double wspolczynnik_kalibracyjny, out double niepewnoscWspolczynnika, out int precyzja, ref DataGridView dataGridView1, DateTime data, double wspolczynnik_korekcyjny)
        //---------------------------------------------------------------
        {
            // sprawdź czy źródło ok
            wspolczynnik_kalibracyjny = 0;
            niepewnoscWspolczynnika = 0;

            List<double> wskazania = new List<double>();
            List<double> tla = new List<double>();

            //-------------Obliczenie średniej wskazań tła----------------
            for (int i = 0; i < dataGridView1.Rows.Count - 1; ++i)
            {
                DataGridViewRow wiersz = dataGridView1.Rows[i];

                try
                {
                    wskazania.Add(N.doubleParse(wiersz.Cells["Wskazanie"].Value.ToString()));
                    tla.Add(N.doubleParse(wiersz.Cells["Tlo"].Value.ToString()));
                }
                catch (Exception)
                {
                    MyMessageBox.Show("Część danych została prawdopodobnie źle wpisana.", "Uwaga");
                }
            }

            double sredniaWskazan;
            double sredniaTel;

            if (wskazania.Count != 0)
                sredniaWskazan = wskazania.Average();
            else
                sredniaWskazan = 0;

            if (wskazania.Count != 0)
                sredniaTel = tla.Average();
            else
                sredniaTel = 0;

            //-----------------------------------------------------------

            _Zapytanie = String.Format("SELECT czas_polowicznego_rozpadu_dni, Niepewnosc FROM Zrodla_powierzchniowe WHERE id_zrodla = {0}", IdZrodla);
            double czas_polowicznego_rozpadu_dni = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<float>(0);
            double niepewnosc_czas_pol_rozpadu = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<float>(1);

            _Zapytanie = String.Format("SELECT emisja_powierzchniowa, niepewnosc, data_wzorcowania FROM Atesty_zrodel WHERE ", data.ToShortDateString())
                       + String.Format("id_zrodla = {0} AND data_wzorcowania=(SELECT MAX(data_wzorcowania) FROM Atesty_zrodel WHERE id_zrodla = {0})", IdZrodla);

            double emisja_powierzchniowa = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<double>(0);
            double niepewnosc = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<double>(1);
            double czas = N.doubleParse((data - _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<DateTime>(2)).Days.ToString());


            //--------------------------------------OBLICZENIE WSPÓŁCZYNNIKA KALIBRACYJNEGO-------------------------------------------
            // 365.25 - zamiana dni na lata
            // poniższy wzór to
            // współ_korekcyjny * emisja_powierzchniowa * exp(-różnica w dniach * ln2 / czas_połowicznego_rozpadu) / (srednia_pomiaru - srednia_tla)

            wspolczynnik_kalibracyjny = wspolczynnik_korekcyjny * emisja_powierzchniowa * Math.Exp(-czas * Math.Log(2.0)
                                        / czas_polowicznego_rozpadu_dni) / (sredniaWskazan - sredniaTel);

            //---------------------------------------LICZENIE NIEPEWNOŚCI---------------------------------------------------------------
            double odchylenie_czesc_pomiar = 0;

            // liczenie odchylenia standardowego
            for (int i = 0; i < wskazania.Count; ++i)
            {
                odchylenie_czesc_pomiar += Math.Pow((wskazania[i] - sredniaWskazan), 2.0);
            }

            double odchylenie_pomiar = (1.0 / Math.Sqrt(wskazania.Count * (wskazania.Count - 1.0)) * Math.Sqrt(odchylenie_czesc_pomiar)) / (sredniaWskazan - sredniaTel);

            double odchylenie_czesc_tlo = 0;

            for (int i = 0; i < tla.Count; ++i)
            {
                odchylenie_czesc_tlo += Math.Pow((tla[i] - sredniaTel), 2.0);
            }

            double odchylenie_tlo = 1.0 / Math.Sqrt(tla.Count * (tla.Count - 1.0)) * Math.Sqrt(odchylenie_czesc_tlo) / (sredniaWskazan - sredniaTel);

            double ukj = _BazaDanych.TworzTabeleDanych("SELECT Wartosc FROM Stale WHERE Nazwa='ukj'").Rows[0].Field<double>(0);
            double ukw = _BazaDanych.TworzTabeleDanych("SELECT Wartosc FROM Stale WHERE Nazwa='ukw'").Rows[0].Field<double>(0);

            int[] szybkoRozpadajaceZrodla = { 7, 18, 8, 2 }; // Am-241, Sr-90/Y-90 (najsilniejszy), Sr-90/Y-90 (silny), Sr-90/Y-90 (słaby)

            if (szybkoRozpadajaceZrodla.Contains(IdZrodla))
            {
                double niepewnosc_rozpadu = Math.Log(2) * czas / czas_polowicznego_rozpadu_dni * Math.Sqrt(1.0 / Math.Pow(czas, 2) + Math.Pow(niepewnosc_czas_pol_rozpadu / czas_polowicznego_rozpadu_dni, 2));
                niepewnoscWspolczynnika = Math.Sqrt(Math.Pow(odchylenie_pomiar, 2.0) + Math.Pow(odchylenie_tlo, 2.0) + Math.Pow(niepewnosc, 2.0) + Math.Pow(ukw, 2.0) + Math.Pow(ukj, 2.0) + Math.Pow(niepewnosc_rozpadu, 2.0)) * 2.0 * wspolczynnik_kalibracyjny;
            }
            else
            {
                niepewnoscWspolczynnika = Math.Sqrt(Math.Pow(odchylenie_pomiar, 2.0) + Math.Pow(odchylenie_tlo, 2.0) + Math.Pow(niepewnosc, 2.0) + Math.Pow(ukw, 2.0) + Math.Pow(ukj, 2.0)) * 2.0 * wspolczynnik_kalibracyjny;
            }
            precyzja = 0;//Narzedzia.Precyzja.Ustaw(niepewnoscWspolczynnika);

            return true;
        }

        //---------------------------------------------------------------
        public WzorcowanieZrodlamiPow(int idKarty, int idZrodla)
        //---------------------------------------------------------------
        {
            _BazaDanych = new BazaDanychWrapper();
            DanePomiary = new Pomiary();
            DaneWarunki = new Warunki();
            DaneWspolczynnikow = new Wspolczynnik();
            DanePrzyrzadow = new Przyrzad();

            this.IdKarty = idKarty;
            _IdZrodlaReferencyjnego = IdZrodla = idZrodla;
            UstawPoczatkowyArkusz();
        }

        //---------------------------------------------------------------
        override public bool Inicjalizuj()
        //---------------------------------------------------------------
        {
            return PobierzIdWzorcowania();
        }

        #region Pobierz Dane

        //---------------------------------------------------------------
        public bool PobierzPodstawoweDanePrzyrzadu()
        //---------------------------------------------------------------
        {
            _Zapytanie = "SELECT D.Typ, D.Nr_fabryczny FROM Dozymetry D INNER JOIN Karta_przyjecia K ON D.Id_dozymetru=K.Id_dozymetru "
                       + String.Format("WHERE K.Id_karty = {0}", IdKarty);

            DataRow wiersz = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0];

            if (null == wiersz)
                return false;

            DanePrzyrzadow.typDozymetru = wiersz.Field<string>(0);
            DanePrzyrzadow.nrFabrycznyDozymetru = wiersz.Field<string>(1);

            return true;
        }

        //---------------------------------------------------------------
        public void PobierzMozliweSondy()
        //---------------------------------------------------------------
        {
            _Zapytanie = "SELECT Typ, Nr_fabryczny FROM Sondy WHERE id_dozymetru = (SELECT id_dozymetru FROM Dozymetry WHERE "
                       + String.Format("typ = '{0}' AND nr_fabryczny = '{1}')", DanePrzyrzadow.typDozymetru, DanePrzyrzadow.nrFabrycznyDozymetru);

            DanePrzyrzadow.sondy.Lista.Clear();

            foreach(DataRow wiersz in _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows)
            {
                DanePrzyrzadow.sondy.Lista.Add(new Sonda(wiersz.Field<string>(0), wiersz.Field<string>(1)));
            }
        }

        //---------------------------------------------------------------
        public bool PobierzDanePrzyrzadow()
        //---------------------------------------------------------------
        {
            _Zapytanie = "SELECT D.Typ, D.Nr_fabryczny, S.Typ, S.Nr_fabryczny FROM " +
            "((Wzorcowanie_zrodlami_powierzchniowymi AS W INNER JOIN Sondy AS S ON S.id_sondy = W.id_sondy) INNER JOIN Dozymetry AS D " +
            String.Format("ON S.id_dozymetru=D.id_dozymetru) WHERE id_wzorcowania = {0}", IdWzorcowania);

            _OdpowiedzBazy = _BazaDanych.TworzTabeleDanych(_Zapytanie);

            try
            {
                DanePrzyrzadow.typDozymetru = _OdpowiedzBazy.Rows[0].Field<string>(0);
                DanePrzyrzadow.nrFabrycznyDozymetru = _OdpowiedzBazy.Rows[0].Field<string>(1);
                DanePrzyrzadow.typSondy = _OdpowiedzBazy.Rows[0].Field<string>(2);
                DanePrzyrzadow.nrFabrycznySondy = _OdpowiedzBazy.Rows[0].Field<string>(3);
                
                PobierzMozliweSondy();

                _Zapytanie = String.Format("SELECT Napiecie_zasilania_sondy, Zakres, Inne_nastawy, Data_wzorcowania FROM Wzorcowanie_zrodlami_powierzchniowymi WHERE Id_wzorcowania = {0}", IdWzorcowania);
                _OdpowiedzBazy = _BazaDanych.TworzTabeleDanych(_Zapytanie);
                DanePrzyrzadow.napiecieZasilaniaSondy = _OdpowiedzBazy.Rows[0].Field<string>(0);
                DanePrzyrzadow.zakres = _OdpowiedzBazy.Rows[0].Field<string>(1);
                DanePrzyrzadow.inneNastawy = _OdpowiedzBazy.Rows[0].Field<string>(2);
                DanePrzyrzadow._Data = _OdpowiedzBazy.Rows[0].Field<DateTime>("Data_wzorcowania");
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        //---------------------------------------------------------------
        public bool PobierzDanePomiarow()
        //---------------------------------------------------------------
        {
            _Zapytanie = String.Format("SELECT Pomiar, Tlo FROM Pomiary_powierzchniowe WHERE id_wzorcowania = {0}", IdWzorcowania);
            _OdpowiedzBazy = _BazaDanych.TworzTabeleDanych(_Zapytanie);

            DanePomiary.pomiar.Clear();
            DanePomiary.tlo.Clear();

            try
            {
                foreach (DataRow row in _OdpowiedzBazy.Rows)
                {
                    DanePomiary.pomiar.Add(row.Field<double>("Pomiar"));
                    DanePomiary.tlo.Add(row.Field<double>("Tlo"));
                }

                _Zapytanie = String.Format("SELECT uwagi, (SELECT jednostka FROM Jednostki WHERE id_jednostki=(SELECT id_jednostki FROM Wzorcowanie_zrodlami_powierzchniowymi WHERE id_wzorcowania = {0})) FROM Wzorcowanie_zrodlami_powierzchniowymi WHERE id_wzorcowania = {0}", IdWzorcowania);
                _OdpowiedzBazy = _BazaDanych.TworzTabeleDanych(_Zapytanie);

                DanePomiary.uwagi = _OdpowiedzBazy.Rows[0].Field<string>(0);
                DanePomiary.jednostka = _OdpowiedzBazy.Rows[0].Field<string>(1);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        //---------------------------------------------------------------
        public bool PobierzDaneWarunkow()
        //---------------------------------------------------------------
        {
            _Zapytanie = "SELECT Cisnienie, Temperatura, Wilgotnosc, Podstawka, Odleglosc_Zrodlo_Sonda, Mnoznik_korekcyjny " +
                         String.Format("FROM Wzorcowanie_zrodlami_powierzchniowymi WHERE Id_wzorcowania = {0}", IdWzorcowania);

            _OdpowiedzBazy = _BazaDanych.TworzTabeleDanych(_Zapytanie);

            try
            {
                DaneWarunki.cisnienie = _OdpowiedzBazy.Rows[0].Field<double>("Cisnienie");
                DaneWarunki.wilgotnosc = _OdpowiedzBazy.Rows[0].Field<double>("Wilgotnosc");
                DaneWarunki.temperatura = _OdpowiedzBazy.Rows[0].Field<double>("Temperatura");
                DaneWarunki.podstawka = _OdpowiedzBazy.Rows[0].Field<string>("Podstawka");
                DaneWarunki.odleglosc = _OdpowiedzBazy.Rows[0].Field<double>("Odleglosc_Zrodlo_Sonda");
                DaneWarunki.mnoznikKorekcyjny = _OdpowiedzBazy.Rows[0].Field<double>("Mnoznik_Korekcyjny");
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        //---------------------------------------------------------------
        public bool PobierzDaneWspolczynnikow()
        //---------------------------------------------------------------
        {
            _Zapytanie = "SELECT Wspolczynnik, Niepewnosc, Osoba_wzorcujaca, Osoba_sprawdzajaca, Dolacz " +
                         String.Format("FROM Wzorcowanie_zrodlami_powierzchniowymi WHERE Id_wzorcowania = {0}", IdWzorcowania);

            _OdpowiedzBazy = _BazaDanych.TworzTabeleDanych(_Zapytanie);

            try
            {
                DaneWspolczynnikow.wspolczynnik = _OdpowiedzBazy.Rows[0].Field<double>("Wspolczynnik");
                DaneWspolczynnikow.niepewnnosc = _OdpowiedzBazy.Rows[0].Field<double>("Niepewnosc");
                DaneWspolczynnikow.osobaWzorcujaca = _OdpowiedzBazy.Rows[0].Field<string>("Osoba_wzorcujaca");
                DaneWspolczynnikow.osobaSprawdzajaca = _OdpowiedzBazy.Rows[0].Field<string>("Osoba_sprawdzajaca");
                DaneWspolczynnikow.dolaczyc = _OdpowiedzBazy.Rows[0].Field<bool>("Dolacz");

            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        //---------------------------------------------------------------
        private bool PobierzIdWzorcowania()
        //---------------------------------------------------------------
        {
            try
            {
                _Zapytanie = String.Format("SELECT id_wzorcowania FROM Wzorcowanie_zrodlami_powierzchniowymi WHERE Id_karty = {0} AND id_zrodla = {1} AND id_arkusza = {2}", IdKarty, IdZrodla, IdArkusza);
                _OdpowiedzBazy = _BazaDanych.TworzTabeleDanych(_Zapytanie);
                IdWzorcowania = _OdpowiedzBazy.Rows[0].Field<int>(0);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        //---------------------------------------------------------------
        public bool PobierzIdPoprzedniegoWzorcowania()
        //---------------------------------------------------------------
        {
            _Zapytanie = String.Format("SELECT id_dozymetru FROM dozymetry WHERE typ = '{0}' AND nr_fabryczny='{1}'", DanePrzyrzadow.typDozymetru, DanePrzyrzadow.nrFabrycznyDozymetru);
            _OdpowiedzBazy = _BazaDanych.TworzTabeleDanych(_Zapytanie);

            try
            {
                int idDozymetru = _OdpowiedzBazy.Rows[0].Field<int>(0);

                _Zapytanie = String.Format("SELECT id_sondy FROM sondy WHERE id_dozymetru = {0} AND typ = '{1}' AND nr_fabryczny = '{2}'", idDozymetru, DanePrzyrzadow.sondy.Lista[0].Typ, DanePrzyrzadow.sondy.Lista[0].NrFabryczny);
                _OdpowiedzBazy = _BazaDanych.TworzTabeleDanych(_Zapytanie);
                int idSondy = _OdpowiedzBazy.Rows[0].Field<int>(0);

                _Zapytanie = "SELECT wspolczynnik, niepewnosc FROM Wzorcowanie_zrodlami_powierzchniowymi WHERE id_sondy = "
                           + String.Format("{0} AND id_zrodla= {1} AND id_karty <> {2} AND data_wzorcowania=(SELECT MAX(data_wzorcowania) FROM ", idSondy, IdZrodla, IdKarty)
                           + String.Format("Wzorcowanie_zrodlami_powierzchniowymi WHERE data_wzorcowania<#{2}# AND id_sondy = {0} AND id_zrodla = {1})",
                                           idSondy, IdZrodla, DanePrzyrzadow._Data.ToShortDateString());

                _OdpowiedzBazy = _BazaDanych.TworzTabeleDanych(_Zapytanie);

                DaneWspolczynnikow.wspolczynnikPoprzedni = _OdpowiedzBazy.Rows[0].Field<double>(0);
                DaneWspolczynnikow.niepewnoscPoprzednia = _OdpowiedzBazy.Rows[0].Field<double>(1);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        //--------------------------------------------------------------------
        public string PobierzRok(int nrKarty)
        //--------------------------------------------------------------------
        {
            string rok;

            _Zapytanie = String.Format("SELECT rok FROM Karta_przyjecia WHERE id_karty = {0}", nrKarty);

            try
            {
                rok = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<int>(0).ToString();
            }
            catch (Exception)
            {
                rok = "";
            }

            return rok;
        }

        #endregion


        //---------------------------------------------------------------
        override public bool StworzNowyArkusz()
        //---------------------------------------------------------------
        {
            if (false == ZnajdzMaksymalnyArkusz())
                return false;

            ++IdArkusza;

            return true;
        }

        //---------------------------------------------------------------
        public List<String> ZnajdzJednostki()
        //---------------------------------------------------------------
        {
            List<String> listaJednostek = new List<string>();

            _OdpowiedzBazy = _BazaDanych.TworzTabeleDanych("SELECT DISTINCT jednostka FROM Jednostki");

            foreach (DataRow row in _OdpowiedzBazy.Rows)
            {
                listaJednostek.Add(row.Field<string>(0));
            }

            return listaJednostek;
        }

        //---------------------------------------------------------------
        public DataTable ZnajdzSondy(string typPrzyrzadu, string nrFabryczny)
        //---------------------------------------------------------------
        {
            DataTable dane = new DataTable();
            int idDozymetru = 0;

            try
            {
                _Zapytanie = String.Format("SELECT id_dozymetru FROM dozymetry WHERE typ = '{0}' AND nr_fabryczny = '{1}'", typPrzyrzadu, nrFabryczny);
                idDozymetru = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<int>(0);

                _Zapytanie = String.Format("SELECT typ, nr_fabryczny FROM sondy WHERE id_dozymetru = {0}", idDozymetru, typPrzyrzadu, nrFabryczny);
                dane = _BazaDanych.TworzTabeleDanych(_Zapytanie);
            }
            catch (Exception)
            {
                return null;
            }

            return dane;
        }

        //---------------------------------------------------------------
        public bool ZnajdzPoprzedniWspolczynnikOrazNiepewnosc(out double wspolczynnik, out double niepewnosc, DateTime data)
        //---------------------------------------------------------------
        {
            wspolczynnik = 0.0;
            niepewnosc = 0.0;

            try
            {
                _Zapytanie = String.Format("SELECT id_dozymetru FROM dozymetry WHERE typ='{0}' AND nr_fabryczny='{1}'", DanePrzyrzadow.typDozymetru, DanePrzyrzadow.nrFabrycznyDozymetru);
                int idDozymmetru = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<int>(0);


                _Zapytanie = String.Format("SELECT id_sondy FROM sondy WHERE id_dozymetru={0} AND typ='{1}' AND nr_fabryczny='{2}'", idDozymmetru, DanePrzyrzadow.sondy.Lista[0].Typ, DanePrzyrzadow.sondy.Lista[0].NrFabryczny);
                int idSondy = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<int>(0);



                _Zapytanie = "SELECT wspolczynnik, niepewnosc FROM Wzorcowanie_zrodlami_powierzchniowymi WHERE "
                           + String.Format("id_sondy={0} AND id_zrodla={1} AND data_wzorcowania=(SELECT MAX(data_wzorcowania) FROM ", idSondy, IdZrodla)
                           + String.Format("Wzorcowanie_zrodlami_powierzchniowymi WHERE data_wzorcowania<#{0}# AND id_sondy={1} AND id_zrodla={2})",
                                           data.ToShortDateString(), idSondy, IdZrodla);
                wspolczynnik = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<double>(0);
                niepewnosc = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<double>(1);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        //---------------------------------------------------------------
        private void UstawPoczatkowyArkusz()
        //---------------------------------------------------------------
        {
            if (false == ZnajdzMinimalnyArkusz())
            {
                if (false == ZnajdzMaksymalnyArkusz())
                {
                    IdArkusza = 1;
                }
                else
                {
                    ++IdArkusza;
                }
            }
        }

        //---------------------------------------------------------------
        public bool SprawdzCzyArkuszJestJuzZapisany()
        //---------------------------------------------------------------
        {
            _Zapytanie = String.Format("SELECT 1 FROM Wzorcowanie_zrodlami_powierzchniowymi WHERE id_karty = {0} AND id_arkusza = {1}", IdKarty, IdArkusza);
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

        //---------------------------------------------------------------
        public bool SprawdzCzyArkuszJestJuzZapisany(int id_zrodla)
        //---------------------------------------------------------------
        {
            _Zapytanie = String.Format("SELECT 1 FROM Wzorcowanie_zrodlami_powierzchniowymi WHERE id_karty = {0} AND id_arkusza = {1} AND id_zrodla = {2}", IdKarty, IdArkusza, id_zrodla);

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

        //---------------------------------------------------------------
        public bool SprawdzSpojnoscRodzajowWzorcowania()
        //---------------------------------------------------------------
        {
            return IdZrodla == _IdZrodlaReferencyjnego;
        }

        //-------------------------------------------------
        public override bool ZnajdzMinimalnyArkusz()
        //-------------------------------------------------
        {
            _Zapytanie = String.Format("SELECT MIN(Id_arkusza) FROM Wzorcowanie_zrodlami_powierzchniowymi WHERE id_karty={0} AND id_zrodla={1}",
                                       IdKarty, IdZrodla);

            _OdpowiedzBazy = _BazaDanych.TworzTabeleDanych(_Zapytanie);

            try
            {
                IdArkusza = _OdpowiedzBazy.Rows[0].Field<int>(0);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        //---------------------------------------------------------------
        public override bool ZnajdzMaksymalnyArkusz()
        //---------------------------------------------------------------
        {
            _Zapytanie = String.Format("SELECT MAX(Id_arkusza) FROM Wzorcowanie_zrodlami_powierzchniowymi WHERE id_karty = {0}", IdKarty);

            _OdpowiedzBazy = _BazaDanych.TworzTabeleDanych(_Zapytanie);

            try
            {
                IdArkusza = _OdpowiedzBazy.Rows[0].Field<int>(0);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        //---------------------------------------------------------------
        public override bool ZnajdzMniejszyArkusz()
        //---------------------------------------------------------------
        {
            _Zapytanie = "SELECT Max(Id_arkusza) FROM Wzorcowanie_zrodlami_powierzchniowymi WHERE "
                       + String.Format("id_karty={0} AND id_zrodla={1} AND id_arkusza < {2}",
                         IdKarty, IdZrodla, IdArkusza);

            _OdpowiedzBazy = _BazaDanych.TworzTabeleDanych(_Zapytanie);

            try
            {
                IdArkusza = _OdpowiedzBazy.Rows[0].Field<int>(0);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        //---------------------------------------------------------------
        public override bool ZnajdzWiekszyArkusz()
        //---------------------------------------------------------------
        {
            _Zapytanie = "SELECT Min(Id_arkusza) FROM Wzorcowanie_zrodlami_powierzchniowymi WHERE "
                       + String.Format("id_karty={0} AND id_zrodla={1} AND id_arkusza > {2}",
                        IdKarty, IdZrodla, IdArkusza);

            _OdpowiedzBazy = _BazaDanych.TworzTabeleDanych(_Zapytanie);

            try
            {
                IdArkusza = _OdpowiedzBazy.Rows[0].Field<int>(0);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }


        #region Nadpisywanie Danych

        //---------------------------------------------------------------
        public void NadpiszDaneOgolne()
        //---------------------------------------------------------------
        {
            /*_Zapytanie = String.Format("UPDATE Wzorcowanie_zrodlami_powierzchniowymi SET Data_wzorcowania=#{0}# WHERE Id_wzorcowania = {1}",
                                       _DaneOgolneDoZapisu.Data, _DaneOgolneDoZapisu.IdWzorcowania);
            _BazaDanych.WykonajPolecenie(_Zapytanie);*/
            _BazaDanych.Wzorcowanie_zrodlami_powierzchniowymi
                .UPDATE()
                    .Data_wzorcowania(_DaneOgolneDoZapisu.Data)
                .WHERE()
                    .ID_wzorcowania(_DaneOgolneDoZapisu.IdWzorcowania)
                .EXECUTE();
        }

        //---------------------------------------------------------------
        public void NadpiszDaneWzorcoweIPomiarowe()
        //---------------------------------------------------------------
        {
            /*_Zapytanie = String.Format("DELETE FROM Pomiary_powierzchniowe WHERE id_wzorcowania = {0}", _DaneOgolneDoZapisu.IdWzorcowania);
            _BazaDanych.WykonajPolecenie(_Zapytanie);*/
            _BazaDanych.Pomiary_powierzchniowe
                .DELETE()
                .WHERE().ID_wzorcowania(_DaneOgolneDoZapisu.IdWzorcowania)
                .EXECUTE();

            ZapiszWartosciWzorcowoPomiarowe();
        }

        //---------------------------------------------------------------
        public void NadpiszDaneObliczonychWspolczynnikow()
        //---------------------------------------------------------------
        {
            ZapiszDaneObliczonychWspolczynnikow();
        }

        //---------------------------------------------------------------
        public void NadpiszDanePrzyrzadu()
        //---------------------------------------------------------------
        {
            ZapiszDanePrzyrzadu();
        }

        //---------------------------------------------------------------
        public void NadpiszDaneWarunkow()
        //---------------------------------------------------------------
        {
            ZapiszDaneWarunkow();
        }

        #endregion

        #region Przygotowywanie danych do zapisu oraz sprawdzanie ich poprawności

        //---------------------------------------------------------------
        public bool PrzygotujDaneOgolneDoZapisu(string idKarty, string idArkusza, DateTime data, bool trybNadpisywania, string zrodlo)
        //---------------------------------------------------------------
        {
            _DaneOgolneDoZapisu = new SkazeniaDaneOgolne();

            _DaneOgolneDoZapisu.IdKarty = N.intParse(idKarty);
            _DaneOgolneDoZapisu.IdArkusza = N.intParse(idArkusza);
            _DaneOgolneDoZapisu.Data = data;

            try
            {

                _DaneOgolneDoZapisu.IdZrodla = int.Parse(zrodlo);

                if (trybNadpisywania)
                {
                    _Zapytanie = "SELECT id_wzorcowania FROM Wzorcowanie_zrodlami_powierzchniowymi WHERE "
                               + String.Format("id_karty={0} AND Id_arkusza={1} AND id_zrodla = {2}", idKarty, idArkusza, _DaneOgolneDoZapisu.IdZrodla);
                    _DaneOgolneDoZapisu.IdWzorcowania = ZnajdzIdWzorcowania(idArkusza, idKarty, zrodlo);
                }
                else
                {
                    _Zapytanie = "SELECT MAX(Id_wzorcowania) FROM Wzorcowanie_zrodlami_powierzchniowymi";
                    _DaneOgolneDoZapisu.IdWzorcowania = ZnajdzMaksymalneIdWzorcowania();

                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        //---------------------------------------------------------------
        public bool PrzygotujDanePrzyrzaduDoZapisu(string typ, string nrFabryczny, string napiecieZasilania, string inneNastawy, string sondaTyp, string sondaNrFab, string zakres)
        //---------------------------------------------------------------
        {
            _PrzyrzadDoZapisu = new SkazeniaPrzyrzad();

            _PrzyrzadDoZapisu.TypDozymetru = typ;
            _PrzyrzadDoZapisu.NrFabrycznyDozymetru = nrFabryczny;
            _PrzyrzadDoZapisu.NapiecieZasilaniaSondy = napiecieZasilania;
            _PrzyrzadDoZapisu.InneNastawy = inneNastawy;
            _PrzyrzadDoZapisu.Zakres = zakres;
            _PrzyrzadDoZapisu.Sondy.Lista.Add(new Sonda(sondaTyp, sondaNrFab));

            return true;
        }

        //---------------------------------------------------------------
        public bool PrzygotujDaneWarunkowDoZapisu(string cisnienie, string temperatura, string wilgotnosc, string podstawka, string odlegloscZrSonda, string wspolKorekcyjny)
        //---------------------------------------------------------------
        {
            _WarunkiDoZapisu = new SkazeniaWarunki();

            try
            {
                _WarunkiDoZapisu.Cisnienie = N.doubleParse(cisnienie);
                _WarunkiDoZapisu.Temperatura = N.doubleParse(temperatura);
                _WarunkiDoZapisu.Wilgotnosc = N.doubleParse(wilgotnosc);
                _WarunkiDoZapisu.Podstawka = podstawka;
                _WarunkiDoZapisu.OdlegloscZrodloSonda = N.doubleParse(odlegloscZrSonda);
                _WarunkiDoZapisu.WspolczynnikKorekcyjny = N.doubleParse(wspolKorekcyjny);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        //---------------------------------------------------------------
        public bool PrzygotujDaneWzorcowoPomiaroweDoZapisu(ref DataGridView tabela, string jednostka, string uwagi)
        //---------------------------------------------------------------
        {
            _WartosciWzorcowoPomiaroweDoZapisu = new SkazeniaWartosciWzorcowoPomiarowe();

            string sTemp;

            _WartosciWzorcowoPomiaroweDoZapisu.jednostka = jednostka;
            _WartosciWzorcowoPomiaroweDoZapisu.uwagi = uwagi;

            try
            {
                for (int i = 0; tabela.Rows[i].Cells[0].Value != null; ++i)
                {
                    SkazeniaWartosciWzorcowoPomiarowe.SkazeniaWartoscWzorcowoPomiarowa temp = new SkazeniaWartosciWzorcowoPomiarowe.SkazeniaWartoscWzorcowoPomiarowa();

                    // wskazanie
                    sTemp = tabela.Rows[i].Cells[0].Value.ToString();

                    if (sTemp != "")
                        temp.Wskazanie = N.doubleParse(sTemp);
                    else
                        temp.Wskazanie = 0.0;

                    // niepewność
                    sTemp = tabela.Rows[i].Cells[1].Value.ToString();

                    if (sTemp != "")
                        temp.Tlo = N.doubleParse(sTemp);
                    else
                        temp.Tlo = 0.0;

                    _WartosciWzorcowoPomiaroweDoZapisu.Dane.Add(temp);
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        //---------------------------------------------------------------
        public bool PrzygotujDaneWspolczynnikowDoZapisu(string wzorcujacy, string sprawdzajacy, string wspolczynnikKlaibracyjny, string niepewnosc, bool dolaczyc)
        //---------------------------------------------------------------
        {
            _WspolczynnikiDoZapisu = new SkazeniaWspolczynniki();

            _WspolczynnikiDoZapisu.Wzorcujacy = wzorcujacy;
            _WspolczynnikiDoZapisu.Sprawdzajacy = sprawdzajacy;
            _WspolczynnikiDoZapisu.Dolaczyc = dolaczyc;

            try
            {
                _WspolczynnikiDoZapisu.WspolczynnikKalibracyjny = N.doubleParse(wspolczynnikKlaibracyjny);
                _WspolczynnikiDoZapisu.Niepewnosc = N.doubleParse(niepewnosc);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        #endregion

        #region Zapisywanie Danych

        //---------------------------------------------------------------
        public void ZapiszDaneOgolne()
        //---------------------------------------------------------------
        {
            /*_Zapytanie = "INSERT INTO Wzorcowanie_zrodlami_powierzchniowymi (Id_karty, Id_arkusza, Id_zrodla, Data_wzorcowania, Id_wzorcowania) "
                       + String.Format("VALUES ({0},{1},{2},#{3}#,{4})", _DaneOgolneDoZapisu.IdKarty, _DaneOgolneDoZapisu.IdArkusza, _DaneOgolneDoZapisu.IdZrodla, _DaneOgolneDoZapisu.Data, _DaneOgolneDoZapisu.IdWzorcowania);
            _BazaDanych.WykonajPolecenie(_Zapytanie);*/
            _BazaDanych.Wzorcowanie_zrodlami_powierzchniowymi
                .INSERT()
                    .ID_karty(_DaneOgolneDoZapisu.IdKarty)
                    .ID_arkusza(_DaneOgolneDoZapisu.IdArkusza)
                    .ID_zrodla(_DaneOgolneDoZapisu.IdZrodla)
                    .Data_wzorcowania(_DaneOgolneDoZapisu.Data)
                    .ID_wzorcowania(_DaneOgolneDoZapisu.IdWzorcowania)
                .EXECUTE();
        }

        //---------------------------------------------------------------
        public void ZapiszDanePrzyrzadu()
        //---------------------------------------------------------------
        {
            _Zapytanie = "SELECT Id_sondy FROM Sondy WHERE Id_dozymetru=(SELECT Id_dozymetru FROM Karta_przyjecia WHERE "
                       + String.Format("Id_karty = {0}) AND typ = '{1}' AND nr_fabryczny = '{2}'", _DaneOgolneDoZapisu.IdKarty, _PrzyrzadDoZapisu.Sondy.Lista[0].Typ, _PrzyrzadDoZapisu.Sondy.Lista[0].NrFabryczny);
            int idSondy = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<int>(0);

            /*_Zapytanie = String.Format("UPDATE Wzorcowanie_zrodlami_powierzchniowymi SET Id_sondy={0}, Napiecie_zasilania_sondy='{1}', ",
                                      idSondy, _PrzyrzadDoZapisu.NapiecieZasilaniaSondy)
                       + String.Format("Zakres='{0}', Inne_nastawy='{1}' WHERE Id_wzorcowania={2}",
                                      _PrzyrzadDoZapisu.Zakres, _PrzyrzadDoZapisu.InneNastawy, _DaneOgolneDoZapisu.IdWzorcowania);
            _BazaDanych.WykonajPolecenie(_Zapytanie);*/
            _BazaDanych.Wzorcowanie_zrodlami_powierzchniowymi
                .UPDATE()
                    .ID_sondy(idSondy)
                    .Napiecie_zasilania_sondy(_PrzyrzadDoZapisu.NapiecieZasilaniaSondy)
                    .Zakres(_PrzyrzadDoZapisu.Zakres)
                    .Inne_nastawy(_PrzyrzadDoZapisu.InneNastawy)
                .WHERE()
                    .ID_wzorcowania(_DaneOgolneDoZapisu.IdWzorcowania)
                .EXECUTE();
        }

        //---------------------------------------------------------------
        public void ZapiszDaneWarunkow()
        //---------------------------------------------------------------
        {
            /*_Zapytanie = String.Format("UPDATE Wzorcowanie_zrodlami_powierzchniowymi SET Cisnienie='{0}',Temperatura='{1}', Wilgotnosc='{2}', ",
                                       _WarunkiDoZapisu.Cisnienie, _WarunkiDoZapisu.Temperatura, _WarunkiDoZapisu.Wilgotnosc)
                       + String.Format("Podstawka='{0}',Odleglosc_Zrodlo_Sonda='{1}', Mnoznik_korekcyjny='{2}' WHERE Id_wzorcowania={3}",
                                       _WarunkiDoZapisu.Podstawka, _WarunkiDoZapisu.OdlegloscZrodloSonda, _WarunkiDoZapisu.WspolczynnikKorekcyjny, _DaneOgolneDoZapisu.IdWzorcowania);
            _BazaDanych.WykonajPolecenie(_Zapytanie);*/
            _BazaDanych.Wzorcowanie_zrodlami_powierzchniowymi
                .UPDATE()
                    .Cisnienie(_WarunkiDoZapisu.Cisnienie)
                    .Temperatura(_WarunkiDoZapisu.Temperatura)
                    .Wilgotnosc(_WarunkiDoZapisu.Wilgotnosc)
                    .Podstawka(_WarunkiDoZapisu.Podstawka)
                    .Odleglosc_zrodlo_sonda(_WarunkiDoZapisu.OdlegloscZrodloSonda)
                    .Mnoznik_korekcyjny(_WarunkiDoZapisu.WspolczynnikKorekcyjny)
                .WHERE()
                    .ID_wzorcowania(_DaneOgolneDoZapisu.IdWzorcowania)
                .EXECUTE();
        }

        //---------------------------------------------------------------
        public void ZapiszWartosciWzorcowoPomiarowe()
        //---------------------------------------------------------------
        {
            for (int i = 0; i < _WartosciWzorcowoPomiaroweDoZapisu.Dane.Count; ++i)
            {
                /*_Zapytanie = "INSERT INTO Pomiary_powierzchniowe (Id_wzorcowania, Pomiar, Tlo) VALUES "
                           + String.Format("({0}, '{1}', '{2}')", _DaneOgolneDoZapisu.IdWzorcowania, _WartosciWzorcowoPomiaroweDoZapisu.Dane[i].Wskazanie, _WartosciWzorcowoPomiaroweDoZapisu.Dane[i].Tlo);
                _BazaDanych.WykonajPolecenie(_Zapytanie);*/
                _BazaDanych.Pomiary_powierzchniowe
                    .INSERT()
                        .ID_wzorcowania(_DaneOgolneDoZapisu.IdWzorcowania)
                        .Pomiar(_WartosciWzorcowoPomiaroweDoZapisu.Dane[i].Wskazanie)
                        .Tlo(_WartosciWzorcowoPomiaroweDoZapisu.Dane[i].Tlo)
                    .EXECUTE();
            }


            _Zapytanie = String.Format("SELECT id_jednostki FROM Jednostki WHERE jednostka='{0}'", _WartosciWzorcowoPomiaroweDoZapisu.jednostka);
            int idJednostki = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<int>(0);

            /*_Zapytanie = String.Format("UPDATE Wzorcowanie_zrodlami_powierzchniowymi SET uwagi='{0}', id_jednostki={1} WHERE id_wzorcowania={2}",
                                      _WartosciWzorcowoPomiaroweDoZapisu.uwagi, idJednostki, _DaneOgolneDoZapisu.IdWzorcowania);
            _BazaDanych.WykonajPolecenie(_Zapytanie);*/
            _BazaDanych.Wzorcowanie_zrodlami_powierzchniowymi
                .UPDATE()
                    .Uwagi(_WartosciWzorcowoPomiaroweDoZapisu.uwagi)
                    .ID_jednostki(idJednostki)
                .WHERE()
                    .ID_wzorcowania(_DaneOgolneDoZapisu.IdWzorcowania)
                .EXECUTE();
        }

        //---------------------------------------------------------------
        public void ZapiszDaneObliczonychWspolczynnikow()
        //---------------------------------------------------------------
        {
            /*_Zapytanie = String.Format("UPDATE Wzorcowanie_zrodlami_powierzchniowymi SET Wspolczynnik='{0}', Niepewnosc='{1}', ", _WspolczynnikiDoZapisu.WspolczynnikKalibracyjny, _WspolczynnikiDoZapisu.Niepewnosc)
                       + String.Format("Osoba_wzorcujaca='{0}', Osoba_sprawdzajaca='{1}', Dolacz={2} WHERE Id_wzorcowania={3}", _WspolczynnikiDoZapisu.Wzorcujacy, _WspolczynnikiDoZapisu.Sprawdzajacy, _WspolczynnikiDoZapisu.Dolaczyc, _DaneOgolneDoZapisu.IdWzorcowania);
            _BazaDanych.WykonajPolecenie(_Zapytanie);*/
            _BazaDanych.Wzorcowanie_zrodlami_powierzchniowymi
                .UPDATE()
                    .wspolczynnik(_WspolczynnikiDoZapisu.WspolczynnikKalibracyjny)
                    .Niepewnosc(_WspolczynnikiDoZapisu.Niepewnosc)
                    .Osoba_wzorcujaca(_WspolczynnikiDoZapisu.Wzorcujacy)
                    .Osoba_sprawdzajaca(_WspolczynnikiDoZapisu.Sprawdzajacy)
                    .Dolacz(_WspolczynnikiDoZapisu.Dolaczyc)
                .WHERE()
                    .ID_wzorcowania(_DaneOgolneDoZapisu.IdWzorcowania)
                .EXECUTE();
        }
        #endregion

        #region Wyszukiwanie id wzrocowania
        //---------------------------------------------------------------
        public int ZnajdzIdWzorcowania(string idArkusza, string idKarty, string idZrodla)
        //---------------------------------------------------------------
        {
            _Zapytanie = String.Format("SELECT id_wzorcowania FROM Wzorcowanie_zrodlami_powierzchniowymi WHERE id_karty={0} AND id_arkusza={1} AND id_zrodla={2}", idKarty, idArkusza, idZrodla);
            return _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<int>(0);
        }

        //---------------------------------------------------------------
        public int ZnajdzMaksymalneIdWzorcowania()
        //---------------------------------------------------------------
        {
            _Zapytanie = "SELECT MAX(Id_wzorcowania) FROM Wzorcowanie_zrodlami_powierzchniowymi";
            return _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<int>(0) + 1;
        }

        //---------------------------------------------------------------
        public void ZnajdzPoprzedniWspolczynnikINiepewnosc(ref TextBox textBox19, ref TextBox textBox20, string idKarty, string idZrodla, string typDozymetru, string nrDozymetru, string typSondy, string nrSondy)
        //---------------------------------------------------------------
        {
            _Zapytanie = "SELECT MAX(id_karty) FROM Karta_przyjecia WHERE id_dozymetru=(SELECT id_dozymetru FROM "
                        + String.Format("Karta_przyjecia WHERE id_karty = {0}) AND id_karty < {0}", idKarty);

            int maxIdKarty = 0;
            int idWzorcowania = 0;

            try
            {
                maxIdKarty = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<int>(0);

                _Zapytanie = String.Format("SELECT id_wzorcowania FROM Wzorcowanie_zrodlami_powierzchniowymi WHERE id_karty = {0} AND id_zrodla = {1} AND id_sondy =", maxIdKarty, idZrodla)
                               + String.Format("(SELECT id_sondy FROM Sondy WHERE typ = '{0}' AND nr_fabryczny = '{1}' AND id_dozymetru =", typSondy, nrSondy)
                               + String.Format("(SELECT id_dozymetru FROM Dozymetry WHERE typ = '{0}' AND nr_fabryczny = '{1}'))", typDozymetru, nrDozymetru);
                idWzorcowania = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<int>(0);

                _Zapytanie = String.Format("SELECT wspolczynnik, niepewnosc FROM Wzorcowanie_zrodlami_powierzchniowymi WHERE id_wzorcowania = {0}", idWzorcowania);

                _OdpowiedzBazy = _BazaDanych.TworzTabeleDanych(_Zapytanie);

                textBox19.Text = _OdpowiedzBazy.Rows[0].Field<double>(0).ToString();
                textBox20.Text = _OdpowiedzBazy.Rows[0].Field<double>(1).ToString();
            }
            catch (Exception)
            {
                textBox19.Text = "Brak wyników dla wybranego wzorcowania";
                textBox20.Text = "Brak wyników dla wybranego wzorcowania";
            }
        }

        //------------------------------------------------------------------
        public int ZnajdzNrPoprzedniejKalibracji(string typDozymetru, string nrDozymetru)
        //------------------------------------------------------------------
        {
            if (typDozymetru == "" || nrDozymetru == "")
                return 0;

            _Zapytanie = String.Format("SELECT id_dozymetru FROM Dozymetry WHERE typ = '{0}' AND nr_fabryczny = '{1}'", typDozymetru, nrDozymetru);
            int idDozymetru = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<int>(0);

            _Zapytanie = String.Format("SELECT MAX(id_karty) FROM Karta_przyjecia WHERE id_karty < {0} AND Id_dozymetru = {1}", IdKarty, idDozymetru);
            _OdpowiedzBazy = _BazaDanych.TworzTabeleDanych(_Zapytanie);

            if (null == _OdpowiedzBazy || null == _OdpowiedzBazy.Rows || null == _OdpowiedzBazy.Rows[0])
                return 0;

            try
            {
                return _OdpowiedzBazy.Rows[0].Field<int>(0);
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
   
        #endregion
    
}
