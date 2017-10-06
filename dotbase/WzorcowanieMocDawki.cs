using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Narzedzia;
using DotBase;
using System.Windows.Forms;
using KlasyPomocniczeMocyDawki;

namespace WzorcowanieMocDawkiSpace
{
    class WzorcowanieMocDawki : DotBase.WzorcowanieCez
    {
        public MocDawkiWartosciWzorcowoPomiarowe Pomiary { get; protected set; }
        public MocDawkiWspolczynniki Wspolczynniki { get; protected set; }

        #region Dane będące jedynie zapisywanymi
        private MocDawkiWartosciWzorcowoPomiarowe _WartosciWzorcowoPomiaroweDoZapisu;
        private MocDawkiWspolczynniki _WspolczynnikiDoZapisu;
        #endregion

        //--------------------------------------------------------------------
        public WzorcowanieMocDawki(int idKarty, string rodzajWzorcowania)
            : base(idKarty, rodzajWzorcowania)
        //--------------------------------------------------------------------
        {
            Wspolczynniki = new MocDawkiWspolczynniki();
            Pomiary = new MocDawkiWartosciWzorcowoPomiarowe();
        }

        //---------------------------------------------------------------
        override public void CzyscStareDane()
        //---------------------------------------------------------------
        {
            base.CzyscStareDane();
            Pomiary.Dane.Clear();
            _WartosciWzorcowoPomiaroweDoZapisu.Dane.Clear();
            _WartosciWzorcowoPomiaroweDoZapisu.jednostka = _WartosciWzorcowoPomiaroweDoZapisu.protokol =
            _WartosciWzorcowoPomiaroweDoZapisu.tlo = _WartosciWzorcowoPomiaroweDoZapisu.WielkoscFizyczna = "";

            _WspolczynnikiDoZapisu.Dane.Clear();
            _WspolczynnikiDoZapisu.Sprawdzajacy = _WspolczynnikiDoZapisu.Wzorcujacy = "nie podano";
        }

        //---------------------------------------------------------------
        override public bool Inicjalizuj()
        //---------------------------------------------------------------
        {
            if (false == ZnajdzMinimalnyArkusz())
                StworzNowyArkusz();

            return PobierzIdWzorcowania();
        }

        //---------------------------------------------------------------
        public List<double> LiczWartoscWzorcowa(ref DataGridView tabela, string protokol, string jednostka, DateTime dataWzorcowania)
        //---------------------------------------------------------------
        {
            _Zapytanie = String.Format("SELECT Data_kalibracji, id_protokolu FROM Protokoly_kalibracji_lawy WHERE data_kalibracji=#{0}#", protokol);
            DateTime dataKalibracjiLawy = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<DateTime>(0);
            int idProtokolu = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<short>(1);
            
            _Zapytanie = String.Format("SELECT przelicznik FROM Jednostki WHERE jednostka='{0}'", jednostka);
            double przelicznik = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<float>(0);

            int roznicaDni = (dataWzorcowania - dataKalibracjiLawy).Days;

	        // 11050.0 - czas połowicznego rozpadu cezu w dniach
            Constants cons = Constants.getInstance();
            double korektaRozpad = Math.Exp(-Math.Log(2.0) * roznicaDni / cons.CS_HALF_TIME_VALUE);

            List<double> wartoscWzorcowa = new List<double>();

            double mocKermy;

            for(int i = 0; i < tabela.RowCount - 1 ; ++i)
            {
                try
                {
                    _Zapytanie = "SELECT moc_kermy FROM Pomiary_wzorcowe WHERE "
                               + String.Format("odleglosc={0} AND id_zrodla={1} AND id_protokolu={2}",
                                 tabela.Rows[i].Cells[0].Value, tabela.Rows[i].Cells[1].Value, idProtokolu);
                    mocKermy = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<double>(0);
                }
                catch (Exception)
                {
                    mocKermy = 0;
                }

                wartoscWzorcowa.Add(mocKermy / przelicznik * korektaRozpad);
            }

            return wartoscWzorcowa;
        }

        //---------------------------------------------------------------
        public bool LiczWspolczynnikINiepewnosc(ref DataGridView tabela, ref DataGridView tabela2, out List<double> zakresyPrzyrzadu, out List<double> wspolczynniki, out List<double> niepewnoscWspolczynnika)
        //---------------------------------------------------------------
        {
            zakresyPrzyrzadu = new List<double>();
            wspolczynniki = new List<double>();
            niepewnoscWspolczynnika = new List<double>();

            if (tabela.Rows == null)
                return false;

            List<string> wskazanie = new List<string>();
            List<string> wartosc = new List<string>();
            List<string> zakres = new List<string>();
            List<string> dolaczyc = new List<string>();
            List<string> niepewnosc = new List<string>();

            try
            {
                for (int i = 0; i < tabela.Rows.Count - 1; ++i)
                {
                    DataGridViewRow wiersz = tabela.Rows[i];

                    string t = wiersz.Cells[6].Value.ToString();
                    if ("true" == wiersz.Cells[6].Value.ToString().ToLower())
                    {
                        wskazanie.Add(wiersz.Cells[2].Value.ToString());
                        wartosc.Add(wiersz.Cells[5].Value.ToString());
                        zakres.Add(wiersz.Cells[4].Value.ToString());
                        dolaczyc.Add(wiersz.Cells[6].Value.ToString());
                        niepewnosc.Add(wiersz.Cells[3].Value.ToString());
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }

            if (zakres.Count <= 0)
                return true;

            zakresyPrzyrzadu.Add(double.Parse(zakres[0]));

            bool znaleziono;
            double temp;
            // znalezeienie wszystkich unikalnych zakresów
            for (int i = 1; i < zakres.Count; ++i)
            {
                znaleziono = false;
                temp = double.Parse(zakres[i]);

                for (int k = 0; k < zakresyPrzyrzadu.Count; ++k)
                {
                    if (temp == zakresyPrzyrzadu[k])
                    {
                        znaleziono = true;
                        break;
                    }
                }

                if (znaleziono == false)
                    zakresyPrzyrzadu.Add(temp);
            }


            double suma = 0;
            double wskazanie_temp;
            double wartosc_temp;
            double zakres_temp;
            int licznik, licznik2;
            licznik2 = 0;
            

            // posortowanie zakresów
            zakresyPrzyrzadu.Sort();

            try
            {
                // dla każdego z zakresów policz odpowiednie wartości
                for (int i = 0; i < zakresyPrzyrzadu.Count; ++i)
                {
                    licznik = 0;
                    suma = 0.0;
                    for (int k = 0; k < wskazanie.Count; ++k)
                    {
                        zakres_temp = double.Parse(zakres[k]);

                        // jeżeli użytkownik wybrał dołączenie
                        if (zakresyPrzyrzadu[i] == zakres_temp && "0" != dolaczyc[k])
                        {
                            wskazanie_temp = Double.Parse(wskazanie[k]);
                            wartosc_temp = Double.Parse(wartosc[k]);
                            suma += wartosc_temp / wskazanie_temp;
                            ++licznik;
                        }
                    }

                    if (licznik == 0)
                        continue;

                    suma /= licznik;
                    wspolczynniki.Add(suma);

/*                    tabela2.Rows.Add();
                    string s = zakresyPrzyrzadu[i].ToString("G");
                    tabela2.Rows[licznik2].Cells[0].Value = s;

                    

                    string ss = String.Format("{0}", suma);
                    tabela2.Rows[licznik2].Cells[1].Value = ss;*/

                    ++licznik2;
                }
            }
            catch (Exception)
            {
                return false;
            }

            double suma2 = 0.0;

            _Zapytanie = "SELECT wartosc FROM Budzetniepewnosci";

            foreach (DataRow wiersz in _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows)
            {
                suma2 += wiersz.Field<double>(0) * wiersz.Field<double>(0);
            }
            suma2 = Math.Sqrt(suma2);

            double max;
            double suma3 = 0.0;
            double suma4 = 0.0;

            for (int i = 0; i < zakresyPrzyrzadu.Count; ++i)
            {
                licznik = 0;
                max = Double.MinValue;

                for (int k = 0; k < wskazanie.Count; ++k)
                {
                    // jeżeli użytkownik wybrał dołączenie
                    if (zakresyPrzyrzadu[i].ToString() == zakres[k] && "false" != dolaczyc[k])
                    {
                        ++licznik;
                        suma3 += Double.Parse(niepewnosc[k]) / Double.Parse(wskazanie[k]) / Math.Sqrt(3.0);

                        if (Math.Abs(wspolczynniki[i] - Double.Parse(wartosc[k]) / Double.Parse(wskazanie[k])) > max)
                            max = Math.Abs(wspolczynniki[i] - Double.Parse(wartosc[k]) / Double.Parse(wskazanie[k]));
                    }
                }

                suma3 /= licznik;
                suma3 = Math.Sqrt(suma3 * suma3 + suma2 * suma2);
                suma4 = max / Math.Sqrt(3);

                //int precyzja = Narzedzia.Precyzja.Ustaw(suma);
                //zapytanie.Printf(wxT("%.*f"), Narzedzia.Precyzja(suma), suma3);

                suma3 = 2.0 * Math.Sqrt(suma3 * suma3 + suma4/wspolczynniki[i] * suma4/wspolczynniki[i] + suma2 * suma2) * wspolczynniki[i];

                niepewnoscWspolczynnika.Add(suma3);

                max = Double.MaxValue;
                suma3 = 0.0;
                suma4 = 0.0;
            }

            return true;
        }
        #region Pobieranie Danych

        //---------------------------------------------------------------
        public bool PobierzIdWzorcowania()
        //---------------------------------------------------------------
        {
            _Zapytanie = "SELECT Id_wzorcowania FROM Wzorcowanie_cezem WHERE "
                       + String.Format("Id_karty = {0} AND Id_arkusza = {1}", IdKarty, IdArkusza);

            try
            {
                IdWzorcowania = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<int>(0);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        //---------------------------------------------------------------
        public bool PobierzDaneWspolczynnikow1()
        //---------------------------------------------------------------
        {
            _Zapytanie = "SELECT osoba_wzorcujaca, osoba_sprawdzajaca, Dolacz FROM Wzorcowanie_cezem WHERE "
                       + String.Format("Id_wzorcowania = {0}", IdWzorcowania);
            _OdpowiedzBazy = _BazaDanych.TworzTabeleDanych(_Zapytanie);

            if (null == _OdpowiedzBazy || _OdpowiedzBazy.Rows.Count == 0)
                return false;

            Wspolczynniki.Wzorcujacy = _OdpowiedzBazy.Rows[0].Field<string>(0);
            Wspolczynniki.Sprawdzajacy = _OdpowiedzBazy.Rows[0].Field<string>(1);
            Wspolczynniki.Dolacz = _OdpowiedzBazy.Rows[0].Field<bool>(2);

            return true;
        }

        //---------------------------------------------------------------
        public bool PobierzDaneWspolczynnikow2()
        //---------------------------------------------------------------
        {
            _Zapytanie = "SELECT Zakres, Wspolczynnik, Niepewnosc  "
                       + String.Format("FROM Wyniki_moc_dawki WHERE Id_wzorcowania = {0}", IdWzorcowania);

            _OdpowiedzBazy = _BazaDanych.TworzTabeleDanych(_Zapytanie);

            if (null == _OdpowiedzBazy || _OdpowiedzBazy.Rows.Count == 0)
                return false;

            Wspolczynniki.Dane.Clear();

            foreach (DataRow wiersz in _OdpowiedzBazy.Rows)
            {
                Wspolczynniki.Dane.Add
                (
                    new MocDawkiWspolczynniki.Wspolczynnik(wiersz.Field<double>(1), wiersz.Field<double>(2), wiersz.Field<double>(0))
                );
            }

            return true;
        }

        //---------------------------------------------------------------
        public bool PobierzDaneWzorcoweIPomiarowe()
        //---------------------------------------------------------------
        {
            _Zapytanie = String.Format("SELECT tlo, wielkosc_fizyczna FROM Wzorcowanie_cezem WHERE id_wzorcowania = {0}", IdWzorcowania);

            try
            {
                _OdpowiedzBazy = _BazaDanych.TworzTabeleDanych(_Zapytanie);
                Pomiary.tlo = _OdpowiedzBazy.Rows[0].Field<string>(0);
                Pomiary.WielkoscFizyczna = _OdpowiedzBazy.Rows[0].Field<string>(1);
                /*Pomiary.jednostka = _OdpowiedzBazy.Rows[0].Field<string>(2);
                Pomiary.protokol = _OdpowiedzBazy.Rows[0].Field<string>(3);*/
            }
            catch (Exception)
            {
                Pomiary.tlo = "";
            }


            _Zapytanie = "SELECT odleglosc, id_zrodla, wskazanie, wahanie, zakres, dolaczyc FROM Pomiary_cez WHERE id_wzorcowania "
                       + String.Format("IN (SELECT id_wzorcowania FROM Wzorcowanie_cezem WHERE id_karty = {0} AND id_arkusza = {1})",
                       IdKarty, IdArkusza);

            _OdpowiedzBazy = _BazaDanych.TworzTabeleDanych(_Zapytanie);

            if (null == _OdpowiedzBazy || null == _OdpowiedzBazy.Rows)
                return false;

            foreach (DataRow wiersz in _OdpowiedzBazy.Rows)
            {
                Pomiary.Dane.Add(
                                    new MocDawkiWartosciWzorcowoPomiarowe.MocDawkiWartoscWzorcowoPomiarowa
                                    (
                                        wiersz.Field<short>("id_zrodla"),
                                        wiersz.Field<double>("odleglosc"),
                                        wiersz.Field<double>("wskazanie"),
                                        wiersz.Field<double>("wahanie"),
                                        wiersz.Field<string>("zakres"),
                                        wiersz.Field<bool>("dolaczyc")
                                    )
                               );
            }

            return true;
        }

        //---------------------------------------------------------------
        public bool PobierzKonkretnaJednostke()
        //---------------------------------------------------------------
        {
            _Zapytanie = String.Format("SELECT id_jednostki FROM Wzorcowanie_cezem WHERE id_wzorcowania={0}", IdWzorcowania);

            try
            {
                int idJednostki = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<int>(0);
                _Zapytanie = String.Format("SELECT Jednostka FROM Jednostki WHERE id_jednostki={0}", idJednostki);
                WybranaJednostka = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<string>(0);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        //---------------------------------------------------------------
        public bool PobierzKonkretnaWielkoscFizyczna(string jednostka)
        //---------------------------------------------------------------
        {
            _Zapytanie = String.Format("SELECT wielkosc_fizyczna FROM Jednostki WHERE jednostka='{0}'", jednostka);

            _OdpowiedzBazy = _BazaDanych.TworzTabeleDanych(_Zapytanie);

            if (_OdpowiedzBazy == null || _OdpowiedzBazy.Rows.Count == 0)
                return false;

            return true;
        }

        //---------------------------------------------------------------
        public bool PobierzSondyDlaDanegoWzorcowania()
        //---------------------------------------------------------------
        {
            _Zapytanie = String.Format("SELECT ID_sondy FROM Wzorcowanie_cezem WHERE ID_wzorcowania = {0}", IdWzorcowania);

            int id_sondy;

            try
            {
                id_sondy = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<int>(0);
            }
            catch (Exception)
            {
                return false;
            }

            _Zapytanie = String.Format("SELECT Typ, Nr_fabryczny FROM Sondy WHERE Id_sondy = {0}", id_sondy);
            _OdpowiedzBazy = _BazaDanych.TworzTabeleDanych(_Zapytanie);

            try
            {
                foreach (DataRow row in _OdpowiedzBazy.Rows)
                {
                    Sondy.Lista.Add(new Sonda(row.Field<string>(0), row.Field<string>(1)));
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        //---------------------------------------------------------------
        private bool PobierzObliczoneWspolczynniki()
        //---------------------------------------------------------------
        {
            _Zapytanie = String.Format("SELECT ID_sondy FROM Wzorcowanie_cezem WHERE ID_wzorcowania = {0}", IdWzorcowania);

            return true;
        }

        #endregion

        #region Przygotowywanie danych do zapisu oraz sprawdzanie ich poprawności

        //---------------------------------------------------------------
        public bool PrzygotujDaneWzorcowoPomiaroweDoZapisu(ref DataGridView tabela, string protokol, string jednostka, string tlo, string wielkoscFizyczna)
        //---------------------------------------------------------------
        {
            _WartosciWzorcowoPomiaroweDoZapisu = new MocDawkiWartosciWzorcowoPomiarowe();

            
            string sTemp;

            _WartosciWzorcowoPomiaroweDoZapisu.jednostka = jednostka;
            _WartosciWzorcowoPomiaroweDoZapisu.protokol = protokol;
            _WartosciWzorcowoPomiaroweDoZapisu.tlo = tlo;
            _WartosciWzorcowoPomiaroweDoZapisu.WielkoscFizyczna = wielkoscFizyczna;

            try
            {
                for (int i = 0; tabela.Rows[i].Cells[0].Value != null; ++i)
                {
                    MocDawkiWartosciWzorcowoPomiarowe.MocDawkiWartoscWzorcowoPomiarowa temp = new MocDawkiWartosciWzorcowoPomiarowe.MocDawkiWartoscWzorcowoPomiarowa();
                    // Odległość
                    sTemp = tabela.Rows[i].Cells[0].Value.ToString();

                    if (sTemp != "")
                        temp.Odleglosc = double.Parse(sTemp);
                    else
                        temp.Odleglosc = 0.0;

                    // id źródła
                    sTemp = tabela.Rows[i].Cells[1].Value.ToString();

                    if (sTemp != "")
                        temp.IdZrodla = short.Parse(sTemp);
                    else
                        temp.IdZrodla = 0;

                    // wskazanie
                    sTemp = tabela.Rows[i].Cells[2].Value.ToString();

                    if (sTemp != "")
                        temp.Wskazanie = double.Parse(sTemp);
                    else
                        temp.Wskazanie = 0.0;

                    // niepewność
                    sTemp = tabela.Rows[i].Cells[3].Value.ToString();

                    if (sTemp != "")
                        temp.Wahanie = double.Parse(sTemp);
                    else
                        temp.Wahanie = 0.0;
                    
                    // zakres 
                    temp.Zakres = tabela.Rows[i].Cells[4].Value.ToString();

                    if ( "true" == tabela.Rows[i].Cells[6].Value.ToString().ToLower())
                        temp.Dolaczyc = true;
                    else
                        temp.Dolaczyc = false;

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
        public bool PrzygotujDaneWspolczynnikowDoZapisu(ref DataGridView tabela, string wzorcujacy, string sprawdzajacy, bool dolacz)
        //---------------------------------------------------------------
        {
            _WspolczynnikiDoZapisu = new MocDawkiWspolczynniki();

            _WspolczynnikiDoZapisu.Wzorcujacy = wzorcujacy;
            _WspolczynnikiDoZapisu.Sprawdzajacy = sprawdzajacy;
            _WspolczynnikiDoZapisu.Dolacz = dolacz;

            try
            {
                for (int i = 0; i < tabela.RowCount; ++i)
                {
                    MocDawkiWspolczynniki.Wspolczynnik temp = new MocDawkiWspolczynniki.Wspolczynnik();

                    if ( tabela.Rows[i].Cells[0].Value.ToString() != "" )
                        temp.Zakres = double.Parse(tabela.Rows[i].Cells[0].Value.ToString());
                    else
                        temp.Zakres = 0.0;

                    if ( tabela.Rows[i].Cells[1].Value.ToString() != "" )
                        temp.Wartosc = double.Parse(tabela.Rows[i].Cells[1].Value.ToString());
                    else
                        temp.Wartosc = 0.0;

                    if ( tabela.Rows[i].Cells[2].Value.ToString() != "" )
                        temp.Niepewnosc = double.Parse(tabela.Rows[i].Cells[2].Value.ToString());
                    else
                        temp.Niepewnosc = 0.0;

                    _WspolczynnikiDoZapisu.Dane.Add(temp);   
                }

            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        #endregion

        #region Wyszukiwanie danych w bazie

        //---------------------------------------------------------------
        public bool ZnajdzWszystkieWielkosciFizyczne(string idJednostki)
        //---------------------------------------------------------------
        {
            _Zapytanie = String.Format("SELECT wielkosc_fizyczna FROM Jednostki WHERE jednostka='{0}'", idJednostki);

            _OdpowiedzBazy = _BazaDanych.TworzTabeleDanych(_Zapytanie);

            if (null == _OdpowiedzBazy || _OdpowiedzBazy.Rows.Count == 0)
                return false;

            return true;
        }

        //---------------------------------------------------------------
        public bool ZnajdzKonkretnaWielkoscFizyczna(string idJednostki)
        //---------------------------------------------------------------
        {
            _Zapytanie = String.Format("SELECT wielkosc_fizyczna FROM Jednostki WHERE jednostka='{0}'", idJednostki);

            _OdpowiedzBazy = _BazaDanych.TworzTabeleDanych(_Zapytanie);

            if (null == _OdpowiedzBazy || _OdpowiedzBazy.Rows.Count == 0)
                return false;

            return true;
        }
        
        #endregion

        #region Nadpisywanie Danych

        //---------------------------------------------------------------
        override public bool NadpiszDaneWzorcoweIPomiarowe()
        //---------------------------------------------------------------
        {
            _Zapytanie = String.Format("DELETE FROM Pomiary_cez WHERE id_wzorcowania = {0}", _DaneOgolneDoZapisu.IdWzorcowania);
	        _BazaDanych.WykonajPolecenie(_Zapytanie);

            for (int i = 0; i < _WartosciWzorcowoPomiaroweDoZapisu.Dane.Count; ++i)
            {
                _Zapytanie = String.Format("INSERT INTO Pomiary_cez VALUES ({0}, '{1}', '{2}', '{3}', '{4}', '{5}', {6})",
                             _DaneOgolneDoZapisu.IdWzorcowania, _WartosciWzorcowoPomiaroweDoZapisu.Dane[i].Odleglosc, _WartosciWzorcowoPomiaroweDoZapisu.Dane[i].IdZrodla,
                             _WartosciWzorcowoPomiaroweDoZapisu.Dane[i].Wskazanie, _WartosciWzorcowoPomiaroweDoZapisu.Dane[i].Wahanie, 
                             _WartosciWzorcowoPomiaroweDoZapisu.Dane[i].Zakres, _WartosciWzorcowoPomiaroweDoZapisu.Dane[i].Dolaczyc );

                _BazaDanych.WykonajPolecenie(_Zapytanie);
            }

            try
            {
                _Zapytanie = String.Format("SELECT id_protokolu FROM Protokoly_kalibracji_lawy WHERE data_kalibracji=#{0}#", _WartosciWzorcowoPomiaroweDoZapisu.protokol);
                short idProtokolu = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<short>(0);

                _Zapytanie = String.Format("SELECT id_jednostki FROM Jednostki WHERE jednostka='{0}'", _WartosciWzorcowoPomiaroweDoZapisu.jednostka);
                int idJednostki = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<int>(0);

                _Zapytanie = String.Format("UPDATE Wzorcowanie_cezem SET id_protokolu = {0}, id_jednostki = {1}, tlo = '{2}', wielkosc_fizyczna='{3}' WHERE id_wzorcowania = {4}",
                                           idProtokolu, idJednostki, _WartosciWzorcowoPomiaroweDoZapisu.tlo, _WartosciWzorcowoPomiaroweDoZapisu.WielkoscFizyczna, IdWzorcowania);
                _BazaDanych.WykonajPolecenie(_Zapytanie);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        //---------------------------------------------------------------
        override public bool NadpiszDaneObliczonychWspolczynnikow()
        //---------------------------------------------------------------
        {
            _Zapytanie = String.Format("DELETE FROM Wyniki_moc_dawki WHERE id_wzorcowania={0}", _DaneOgolneDoZapisu.IdWzorcowania);
            _BazaDanych.WykonajPolecenie(_Zapytanie);

            _Zapytanie = String.Format("UPDATE Wzorcowanie_cezem SET osoba_wzorcujaca='{0}', osoba_sprawdzajaca='{1}', Dolacz={2} WHERE id_wzorcowania={3}",
                                       _WspolczynnikiDoZapisu.Wzorcujacy, _WspolczynnikiDoZapisu.Sprawdzajacy, _WspolczynnikiDoZapisu.Dolacz, _DaneOgolneDoZapisu.IdWzorcowania);
            _BazaDanych.WykonajPolecenie(_Zapytanie);

            for (int i = 0; i < _WspolczynnikiDoZapisu.Dane.Count; ++i)
            {
                _Zapytanie = "INSERT INTO Wyniki_moc_dawki (Wspolczynnik, Niepewnosc, Zakres, id_wzorcowania) VALUES "
                           + String.Format("('{0}', '{1}', '{2}', {3})", _WspolczynnikiDoZapisu.Dane[i].Wartosc, _WspolczynnikiDoZapisu.Dane[i].Niepewnosc,
                                                                   _WspolczynnikiDoZapisu.Dane[i].Zakres, _DaneOgolneDoZapisu.IdWzorcowania);
                _BazaDanych.WykonajPolecenie(_Zapytanie);
            }

            return true;
        }

        #endregion

        #region Zapisywanie Danych

        //---------------------------------------------------------------
        override public bool ZapiszDaneWzorcoweIPomiarowe()
        //---------------------------------------------------------------
        {
            for (int i = 0; i < _WartosciWzorcowoPomiaroweDoZapisu.Dane.Count ; ++i)
            {
                _Zapytanie = String.Format("INSERT INTO Pomiary_cez VALUES ({0}, '{1}', '{2}', '{3}', '{4}', '{5}', {6})",
                             _DaneOgolneDoZapisu.IdWzorcowania, _WartosciWzorcowoPomiaroweDoZapisu.Dane[i].Odleglosc, _WartosciWzorcowoPomiaroweDoZapisu.Dane[i].IdZrodla, _WartosciWzorcowoPomiaroweDoZapisu.Dane[i].Wskazanie,
                             _WartosciWzorcowoPomiaroweDoZapisu.Dane[i].Wahanie, _WartosciWzorcowoPomiaroweDoZapisu.Dane[i].Zakres, _WartosciWzorcowoPomiaroweDoZapisu.Dane[i].Dolaczyc);
                _BazaDanych.WykonajPolecenie(_Zapytanie);
            }

            try
            {
                _Zapytanie = String.Format("SELECT id_protokolu FROM Protokoly_kalibracji_lawy WHERE data_kalibracji=#{0}#", _WartosciWzorcowoPomiaroweDoZapisu.protokol);
                short idProtokolu = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<short>(0);

                _Zapytanie = String.Format("SELECT id_jednostki FROM Jednostki WHERE jednostka='{0}'", _WartosciWzorcowoPomiaroweDoZapisu.jednostka);
                int idJednostki = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<int>(0);

                _Zapytanie = String.Format("UPDATE Wzorcowanie_cezem SET id_protokolu = {0}, id_jednostki = {1}, tlo = '{2}', wielkosc_fizyczna = '{3}'  WHERE id_wzorcowania = {4}",
                                           idProtokolu, idJednostki, _WartosciWzorcowoPomiaroweDoZapisu.tlo, _WartosciWzorcowoPomiaroweDoZapisu.WielkoscFizyczna, _DaneOgolneDoZapisu.IdWzorcowania);
                _BazaDanych.WykonajPolecenie(_Zapytanie);
            }
            catch (Exception)
            {
            }

            return true;
        }
        //---------------------------------------------------------------
        override public bool ZapiszDaneObliczonychWspolczynnikow()
        //---------------------------------------------------------------
        {
            /*_Zapytanie = "SELECT MAX(Id_wzorcowania) FROM Wzorcowanie_cezem";
            int idWzorcowania = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<int>(0);*/

            _Zapytanie = String.Format("UPDATE Wzorcowanie_cezem SET osoba_wzorcujaca='{0}', osoba_sprawdzajaca='{1}', Dolacz={2} WHERE id_wzorcowania={3}",
                                       _WspolczynnikiDoZapisu.Wzorcujacy, _WspolczynnikiDoZapisu.Sprawdzajacy, _WspolczynnikiDoZapisu.Dolacz, _DaneOgolneDoZapisu.IdWzorcowania);
            _BazaDanych.WykonajPolecenie(_Zapytanie);

            for (int i = 0; i < _WspolczynnikiDoZapisu.Dane.Count ; ++i)
            {
                _Zapytanie = "INSERT INTO Wyniki_moc_dawki (Wspolczynnik, Niepewnosc, Zakres, id_wzorcowania) VALUES "
                           + String.Format("('{0}', '{1}', '{2}', {3})", _WspolczynnikiDoZapisu.Dane[i].Wartosc , _WspolczynnikiDoZapisu.Dane[i].Niepewnosc,
                                                                         _WspolczynnikiDoZapisu.Dane[i].Zakres, _DaneOgolneDoZapisu.IdWzorcowania);
                _BazaDanych.WykonajPolecenie(_Zapytanie);
            }

            return true;
        }

        //---------------------------------------------------------------
        public void ZnajdzPoprzedniWspolczynnikINiepewnosc(ref DataGridView tabela, string idKarty, string typDozymetru, string nrDozymtru, string typSondy, string nrSondy)
        //---------------------------------------------------------------
        {
            _Zapytanie = "SELECT MAX(id_karty) FROM Karta_przyjecia WHERE id_dozymetru=(SELECT id_dozymetru FROM "
                        + String.Format("Karta_przyjecia WHERE id_karty = {0}) AND id_karty < {0}", idKarty);

            int maxIdKarty = 0;
            int idWzorcowania = 0;

            try
            {
                maxIdKarty = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<int>(0);

                _Zapytanie = String.Format("SELECT id_wzorcowania FROM Wzorcowanie_cezem WHERE id_karty = {0} AND id_sondy =", maxIdKarty)
                           + String.Format("(SELECT id_sondy FROM Sondy WHERE typ = '{0}' AND nr_fabryczny = '{1}' AND id_dozymetru =", typSondy, nrSondy)
                           + String.Format("(SELECT id_dozymetru FROM Dozymetry WHERE typ = '{0}' AND nr_fabryczny = '{1}'))", typDozymetru, nrDozymtru);
                idWzorcowania = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<int>(0);

                _Zapytanie = String.Format("SELECT zakres, wspolczynnik, niepewnosc FROM Wyniki_moc_dawki WHERE id_wzorcowania = {0}", idWzorcowania);

                string zakres = "", wspolczynnik, niepewnosc;

                foreach (DataRow wiersz in _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows)
                {
                    zakres = wiersz.Field<double>(0).ToString();
                    wspolczynnik = wiersz.Field<double>(1).ToString();
                    niepewnosc = wiersz.Field<double>(2).ToString();

                    foreach(DataGridViewRow wierszTabeli in tabela.Rows)
                    {
                        if (zakres == wierszTabeli.Cells[0].Value.ToString())
                        {
                            wierszTabeli.Cells[3].Value = wspolczynnik;
                            wierszTabeli.Cells[4].Value = niepewnosc;
                            break;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        #endregion
    }
}