using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DotBase
{
    class WzorcowanieDawka : WzorcowanieCez
    {
        public enum ROWNOWAZNIKI_DAWKI { Indywidualny_HP07, Indywidualny_HP10, Przestrzenny};

        public KlasyPomocniczeDawka.DawkaWartosciWzorcowoPomiarowe Pomiary { get; protected set; }
        public KlasyPomocniczeDawka.DawkaWspolczynniki Wspolczynniki { get; protected set; }

        //--------------------------------------------------------------------
        public WzorcowanieDawka(int idKarty, string rodzajWzorcowania)
            : base(idKarty, rodzajWzorcowania)
        //--------------------------------------------------------------------
        {
            Przyrzad = new KlasyPomocniczeCez.Przyrzad();
            Wspolczynniki = new KlasyPomocniczeDawka.DawkaWspolczynniki();
            Pomiary = new KlasyPomocniczeDawka.DawkaWartosciWzorcowoPomiarowe();
        }

        //---------------------------------------------------------------
        override public bool Inicjalizuj()
        //---------------------------------------------------------------
        {
            if( false == ZnajdzMinimalnyArkusz())
                StworzNowyArkusz();
            
            return PobierzIdWzorcowania();
        }

        //---------------------------------------------------------------
        override public void CzyscStareDane()
        //---------------------------------------------------------------
        {
            base.CzyscStareDane();
        }

        //---------------------------------------------------------------
        override public bool NadpiszDaneWzorcoweIPomiarowe()
        //---------------------------------------------------------------
        {
            _Zapytanie = String.Format("DELETE FROM Pomiary_dawka WHERE id_wzorcowania = {0}", _DaneOgolneDoZapisu.IdWzorcowania);
            _BazaDanych.WykonajPolecenie(_Zapytanie);

            for (int i = 0; i < Pomiary.Dane.Count; ++i)
            {
                _Zapytanie = String.Format("INSERT INTO Pomiary_dawka VALUES ({0}, '{1}', '{2}', {3}, '{4}')",
                             _DaneOgolneDoZapisu.IdWzorcowania, Pomiary.Dane[i].Czas, Pomiary.Dane[i].Wskazanie,
                             Pomiary.Dane[i].Dolaczyc, Pomiary.Dane[i].WartoscWzorcowa);

                _BazaDanych.WykonajPolecenie(_Zapytanie);
            }

            try
            {
                _Zapytanie = String.Format("SELECT id_protokolu FROM Protokoly_kalibracji_lawy WHERE data_kalibracji=#{0}#", Pomiary.protokol);
                short idProtokolu = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<short>(0);

                _Zapytanie = String.Format("UPDATE Wzorcowanie_cezem SET id_protokolu = {0}, id_jednostki = 0, tlo = 0.0, wielkosc_fizyczna='nie dotyczy' WHERE id_wzorcowania = {1}",
                                           idProtokolu, IdWzorcowania);
                _BazaDanych.WykonajPolecenie(_Zapytanie);

                _Zapytanie = String.Format("UPDATE Wyniki_dawka SET id_zrodla = {0}, odleglosc = '{1}' WHERE id_wzorcowania = {2}", Pomiary.zrodlo, Pomiary.odleglosc, _DaneOgolneDoZapisu.IdWzorcowania);
                _BazaDanych.WykonajPolecenie(_Zapytanie);
            }
            catch (Exception)
            {
                return false;
            }
            
            return true;
        }

        //---------------------------------------------------------------
        public Narzedzia.Pair<double,double> LiczWspolczynnikOrazNiepewnosc(List<Narzedzia.Pair<double, double>> inputList)
        //---------------------------------------------------------------
        {
            List<double> temporaryValues = new List<double>();

            if (inputList.Count == 0)
                return new Narzedzia.Pair<double, double>(0, 0);

            foreach (Narzedzia.Pair<double, double> dataPair in inputList)
            {
                temporaryValues.Add(dataPair.First / dataPair.Second);
            }

            double wspolczynnik = temporaryValues.Average();
            double odchylenieStandardowe = Math.Sqrt(temporaryValues.Average(v => Math.Pow(v - wspolczynnik, 2)));
            odchylenieStandardowe /= Math.Sqrt(temporaryValues.Count-1);
            odchylenieStandardowe /= wspolczynnik;
            odchylenieStandardowe = Math.Sqrt(odchylenieStandardowe * odchylenieStandardowe + Math.Pow(SygnalizacjaMocyDawkiUtils.retrieveSkladowaStalaNiepewnosci(), 2.0)); // TODO - wynieś to (retrieveSkladowaStalaNiepewnosci) do konfiguracji pliku w xml
            odchylenieStandardowe *= 2.0;

            return new Narzedzia.Pair<double, double>(wspolczynnik, odchylenieStandardowe);
        }

        //---------------------------------------------------------------
        public List<double> LiczCzas(ref System.Windows.Forms.DataGridView tabela, string protokol, DateTime dataWzorcowania, string odleglosc, string id_zrodla, int rownowaznikDawki)
        //---------------------------------------------------------------
        {
            double dzielnik = 0.0;

            if (rownowaznikDawki == 0)
                dzielnik = 1.21;
            else if (rownowaznikDawki == 1)
                dzielnik = 1.12;
            else if (rownowaznikDawki == 2)
                dzielnik = 1.20;
            else
                dzielnik = 1.00;

            _Zapytanie = String.Format("SELECT Data_kalibracji, id_protokolu FROM Protokoly_kalibracji_lawy WHERE data_kalibracji=#{0}#", protokol);
            DateTime dataKalibracjiLawy = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<DateTime>(0);
            int idProtokolu = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<short>(1);

            int roznicaDni = (dataWzorcowania - dataKalibracjiLawy).Days;

            // 11050.0 - czas połowicznego rozpadu cezu w dniach
            double korektaRozpad = Math.Exp(-Math.Log(2.0) * roznicaDni / 11050.0);

            List<double> listaCzas = new List<double>();

            double mocKermy, czas;

            double sumaCzasowPoprzednich = 0;

            for (int i = 0; i < tabela.RowCount - 1; ++i)
            {
                try
                {
                    _Zapytanie = "SELECT moc_kermy FROM Pomiary_wzorcowe WHERE "
                               + String.Format("odleglosc={0} AND id_zrodla={1} AND id_protokolu={2}",
                                 odleglosc.Replace(",", "."), id_zrodla, idProtokolu);
                    mocKermy = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<double>(0);
                }
                catch (Exception)
                {
                    listaCzas.Add(0);
                    continue;
                }

                if (tabela.Rows[i].Cells["WartoscWzorcowa"].Value != null && Double.TryParse(tabela.Rows[i].Cells["WartoscWzorcowa"].Value.ToString(), out czas))
                {
                    czas = czas * (1000 * 3600 / (dzielnik * mocKermy * korektaRozpad)) - sumaCzasowPoprzednich;
                    sumaCzasowPoprzednich += czas;
                }
                else
                    czas = 0.0;

                listaCzas.Add(czas);
            }

            return listaCzas;
        }

        //---------------------------------------------------------------
        override public bool NadpiszDaneObliczonychWspolczynnikow()
        //---------------------------------------------------------------
        {
            _Zapytanie = String.Format("UPDATE Wzorcowanie_cezem SET osoba_wzorcujaca='{0}', osoba_sprawdzajaca='{1}', Dolacz={2} WHERE id_wzorcowania={3}",
                                       Wspolczynniki.Wzorcujacy, Wspolczynniki.Sprawdzajacy, Wspolczynniki.Dolacz, _DaneOgolneDoZapisu.IdWzorcowania);
            _BazaDanych.WykonajPolecenie(_Zapytanie);

            _Zapytanie = String.Format("UPDATE Wyniki_dawka SET id_zrodla={0}, odleglosc='{1}', wspolczynnik='{2}', ", Pomiary.zrodlo, Wspolczynniki.Odleglosc, Wspolczynniki.Wspolczynnik)
                       + String.Format("niepewnosc='{0}', zakres='{1}', wielkosc_fizyczna='{2}' WHERE id_wzorcowania={3}", Wspolczynniki.Niepewnosc, Wspolczynniki.Zakres, Wspolczynniki.RownowaznikDawki, _DaneOgolneDoZapisu.IdWzorcowania);
            _BazaDanych.WykonajPolecenie(_Zapytanie);

            return true;
        }

        //---------------------------------------------------------------
        public bool PobierzDaneWspolczynnikow()
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

            _Zapytanie = String.Format("SELECT Wspolczynnik, Niepewnosc, Zakres, wielkosc_fizyczna FROM Wyniki_dawka WHERE Id_wzorcowania = {0}", IdWzorcowania);
            _OdpowiedzBazy = _BazaDanych.TworzTabeleDanych(_Zapytanie);

            if (null == _OdpowiedzBazy || _OdpowiedzBazy.Rows.Count == 0)
            {
                Wspolczynniki.Wspolczynnik = Wspolczynniki.Niepewnosc = Wspolczynniki.Zakres = 0.0;
                Wspolczynniki.RownowaznikDawki = (int)ROWNOWAZNIKI_DAWKI.Indywidualny_HP07;
            }
            else
            {
                Wspolczynniki.Wspolczynnik = _OdpowiedzBazy.Rows[0].Field<double>(0);
                Wspolczynniki.Niepewnosc = _OdpowiedzBazy.Rows[0].Field<double>(1);
                Wspolczynniki.Zakres = _OdpowiedzBazy.Rows[0].Field<double>(2);
                Wspolczynniki.RownowaznikDawki = _OdpowiedzBazy.Rows[0].Field<int>(3);
            }

            return true;
        }

        //---------------------------------------------------------------
        public bool PobierzKonkretnaJednostke()
        //---------------------------------------------------------------
        {
            WybranaJednostka = "mSv";
            
            return true;
        }

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
        public bool PobierzDaneWzorcoweIPomiarowe()
        //---------------------------------------------------------------
        {
            _Zapytanie = "SELECT czas, wskazanie, dolaczyc, wartosc_wzorcowa FROM Pomiary_dawka WHERE id_wzorcowania "
                       + String.Format("IN (SELECT id_wzorcowania FROM Wzorcowanie_cezem WHERE id_karty = {0} AND id_arkusza = {1})",
                       IdKarty, IdArkusza);

            _OdpowiedzBazy = _BazaDanych.TworzTabeleDanych(_Zapytanie);

            if (null == _OdpowiedzBazy || null == _OdpowiedzBazy.Rows)
                return false;

            Pomiary.Dane.Clear();

            foreach (DataRow wiersz in _OdpowiedzBazy.Rows)
            {
                Pomiary.Dane.Add(
                                    new KlasyPomocniczeDawka.DawkaWartosciWzorcowoPomiarowe.DawkaWartoscWzorcowoPomiarowa
                                    (
                                        wiersz.Field<double>("wartosc_wzorcowa"),
                                        wiersz.Field<double>("czas"),
                                        wiersz.Field<double>("wskazanie"),
                                        wiersz.Field<bool>("dolaczyc")
                                    )
                               );
            }

            _Zapytanie = String.Format("SELECT odleglosc, id_zrodla FROM Wyniki_dawka WHERE id_wzorcowania = {0}", IdWzorcowania);
            _OdpowiedzBazy = _BazaDanych.TworzTabeleDanych(_Zapytanie);

            try
            {
                Pomiary.odleglosc = _OdpowiedzBazy.Rows[0].Field<double>(0).ToString();
                Pomiary.zrodlo = _OdpowiedzBazy.Rows[0].Field<int>(1).ToString();
            }
            catch(Exception)
            {
                Pomiary.odleglosc = "0";
                Pomiary.zrodlo = "0";
            }

            return true;
        }

        //---------------------------------------------------------------
        public bool PrzygotujDaneWzorcowoPomiaroweDoZapisu(ref System.Windows.Forms.DataGridView tabela, string protokol, string zrodlo, string odleglosc)
        //---------------------------------------------------------------
        {
            Pomiary.Dane.Clear();

            string sTemp;

            Pomiary.jednostka = "mSv";
            Pomiary.protokol  = protokol;
            Pomiary.odleglosc = odleglosc;
            Pomiary.zrodlo    = zrodlo;

            try
            {
                for (int i = 0; tabela.Rows[i].Cells[0].Value != null || i < tabela.Rows.Count - 1; ++i)
                {
                    KlasyPomocniczeDawka.DawkaWartosciWzorcowoPomiarowe.DawkaWartoscWzorcowoPomiarowa temp 
                    = new KlasyPomocniczeDawka.DawkaWartosciWzorcowoPomiarowe.DawkaWartoscWzorcowoPomiarowa();
                    
                    // wartość wzorcowa
                    sTemp = tabela.Rows[i].Cells[0].Value.ToString();

                    if (sTemp != "")
                        temp.WartoscWzorcowa = double.Parse(sTemp);
                    else
                        temp.WartoscWzorcowa = 0.0;

                    // czas
                    sTemp = tabela.Rows[i].Cells[1].Value.ToString();

                    if (sTemp != "")
                        temp.Czas = double.Parse(sTemp);
                    else
                        temp.Czas = 0.0;

                    // wskazanie
                    sTemp = tabela.Rows[i].Cells[2].Value.ToString();

                    if (sTemp != "")
                        temp.Wskazanie = double.Parse(sTemp);
                    else
                        temp.Wskazanie = 0.0;

                    // dołączyć
                    sTemp = tabela.Rows[i].Cells[3].Value.ToString();

                    if (sTemp != "")
                        temp.Dolaczyc = bool.Parse(sTemp);
                    else
                        temp.Dolaczyc = false;

                    Pomiary.Dane.Add(temp);
                }
            }
            catch (Exception)
            {
                return false;
            }
            
            return true;
        }

        //---------------------------------------------------------------
        public bool PrzygotujDaneWspolczynnikowDoZapisu(string wzorcujacy, string sprawdzajacy, bool dolacz, string wspolczynnik, string niepewnosc, string odleglosc, string zakres, int rownowaznikDawki)
        //---------------------------------------------------------------
        {
            Wspolczynniki.Wzorcujacy = wzorcujacy;
            Wspolczynniki.Sprawdzajacy = sprawdzajacy;
            Wspolczynniki.Dolacz = dolacz;
            Wspolczynniki.RownowaznikDawki = rownowaznikDawki;

            double temp;

            if (Double.TryParse(wspolczynnik, out temp))
                Wspolczynniki.Wspolczynnik = temp;
            else
                Wspolczynniki.Wspolczynnik = 0.0;

            if (Double.TryParse(niepewnosc, out temp))
                Wspolczynniki.Niepewnosc = temp;
            else
                Wspolczynniki.Niepewnosc = 0.0;

            if (Double.TryParse(odleglosc, out temp))
                Wspolczynniki.Odleglosc = temp;
            else
                Wspolczynniki.Odleglosc = 0.0;

            if (Double.TryParse(zakres, out temp))
                Wspolczynniki.Zakres = temp;
            else
                Wspolczynniki.Zakres = 0.0;

            return true;
        }

        //---------------------------------------------------------------
        override public bool ZapiszDaneWzorcoweIPomiarowe()
        //---------------------------------------------------------------
        {
            for (int i = 0; i < Pomiary.Dane.Count; ++i)
            {
                _Zapytanie = String.Format("INSERT INTO Pomiary_dawka VALUES ({0}, '{1}', '{2}', {3}, '{4}')",
                             _DaneOgolneDoZapisu.IdWzorcowania, Pomiary.Dane[i].Czas, Pomiary.Dane[i].Wskazanie,
                             Pomiary.Dane[i].Dolaczyc, Pomiary.Dane[i].WartoscWzorcowa);
                _BazaDanych.WykonajPolecenie(_Zapytanie);
            }

            try
            {
                _Zapytanie = String.Format("SELECT id_protokolu FROM Protokoly_kalibracji_lawy WHERE data_kalibracji=#{0}#", Pomiary.protokol);
                short idProtokolu = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<short>(0);

                _Zapytanie = String.Format("UPDATE Wzorcowanie_cezem SET id_protokolu = {0}, id_jednostki = 0, tlo = 0.0, wielkosc_fizyczna = 'nie dotyczy'  WHERE id_wzorcowania = {1}",
                                           idProtokolu, _DaneOgolneDoZapisu.IdWzorcowania);
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
            _Zapytanie = String.Format("UPDATE Wzorcowanie_cezem SET osoba_wzorcujaca='{0}', osoba_sprawdzajaca='{1}', Dolacz={2} WHERE id_wzorcowania={3}",
                                       Wspolczynniki.Wzorcujacy, Wspolczynniki.Sprawdzajacy, Wspolczynniki.Dolacz, _DaneOgolneDoZapisu.IdWzorcowania);
            _BazaDanych.WykonajPolecenie(_Zapytanie);

            _Zapytanie = "INSERT INTO Wyniki_dawka (id_wzorcowania, id_zrodla, odleglosc, wspolczynnik, niepewnosc, zakres, wielkosc_fizyczna) VALUES "
                       + String.Format("({0}, {1}, '{2}', '{3}', '{4}', '{5}', {6})", 
                         _DaneOgolneDoZapisu.IdWzorcowania,
                         Pomiary.zrodlo,
                         Wspolczynniki.Odleglosc,
                         Wspolczynniki.Wspolczynnik,
                         Wspolczynniki.Niepewnosc,
                         Wspolczynniki.Zakres,
                         Wspolczynniki.RownowaznikDawki);

            _BazaDanych.WykonajPolecenie(_Zapytanie);

            return true;
        }

        //---------------------------------------------------------------
        public Narzedzia.Pair<double, double> ZnajdzPoprzedniWspolczynnikOrazNiepewnosc()
        //---------------------------------------------------------------
        {
            _Zapytanie = "SELECT MAX(id_karty) FROM Karta_przyjecia WHERE id_dozymetru=(SELECT id_dozymetru FROM "
                        + String.Format("Karta_przyjecia WHERE id_karty = {0}) AND id_karty < {0}", IdKarty);

            int maxIdKarty = 0;
            int idWzorcowania = 0;

            try
            {
                maxIdKarty = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<int>(0);

                _Zapytanie = String.Format("SELECT id_wzorcowania FROM Wzorcowanie_cezem WHERE id_karty = {0} AND id_sondy =", maxIdKarty)
                           + String.Format("(SELECT id_sondy FROM Sondy WHERE typ = '{0}' AND nr_fabryczny = '{1}' AND id_dozymetru =", Sondy.Lista[0].Typ, Sondy.Lista[0].NrFabryczny)
                           + String.Format("(SELECT id_dozymetru FROM Dozymetry WHERE typ = '{0}' AND nr_fabryczny = '{1}'))", Przyrzad.TypDozymetru, Przyrzad.NrFabrycznyDozymetru);
                idWzorcowania = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<int>(0);

                _Zapytanie = String.Format("SELECT wspolczynnik, niepewnosc FROM Wyniki_dawka WHERE id_wzorcowania = {0}", idWzorcowania);

                DataRow wiersz = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0];
                
                return new Narzedzia.Pair<double,double>(wiersz.Field<double>(0), wiersz.Field<double>(1));
            }
            catch (Exception)
            {
                return new Narzedzia.Pair<double, double>(0, 0);
            }
        }
    }
}
