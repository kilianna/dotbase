using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DotBase
{
    class WzorcowanieSygDawki : WzorcowanieCez
    {
        public KlasyPomocniczeCez.Protokol Protokol { get; private set; }
        public KlasyPomocniczeSygDawki.DawkaWartosciWzorcowoPomiarowe Pomiary { get; private set; }

        public WzorcowanieSygDawki(int idKarty, string rodzajWzorcowania)
            : base(idKarty, rodzajWzorcowania)
        {
            Protokol = new KlasyPomocniczeCez.Protokol();
            Pomiary = new KlasyPomocniczeSygDawki.DawkaWartosciWzorcowoPomiarowe();
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
        override public void CzyscStareDane()
        //---------------------------------------------------------------
        {
            base.CzyscStareDane();
        }

        //---------------------------------------------------------------
        public List<double> LiczWartoscRzecyzwista(double odleglosc, int zrodlo, string protokol, string jednostka, List<double> czasZmierzony, DateTime dataWzorcowania)
        //---------------------------------------------------------------
        {
            _Zapytanie = String.Format("SELECT Data_kalibracji, id_protokolu FROM Protokoly_kalibracji_lawy WHERE data_kalibracji=#{0}#", protokol);
            DateTime dataKalibracjiLawy = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<DateTime>(0);
            int idProtokolu = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<short>(1);
            
            int roznicaDni = (dataWzorcowania - dataKalibracjiLawy).Days;

            // 11050.0 - czas połowicznego rozpadu cezu w dniach
            double korektaRozpad = Math.Exp(-Math.Log(2.0) * roznicaDni / Constants.getInstance().CS_HALF_TIME_VALUE);

            _Zapytanie = String.Format("SELECT przelicznik FROM Jednostki WHERE jednostka='{0}'", jednostka);
            double przelicznik = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<float>(0);

            List<double> listaWartRzeczywiste = new List<double>();
            double mocKermy;

            for (UInt16 i = 0; i < czasZmierzony.Count; ++i)
            {
                try
                {
                    _Zapytanie = String.Format("SELECT moc_kermy FROM Pomiary_wzorcowe WHERE odleglosc={0} AND id_zrodla={1} AND id_protokolu={2}",
                                 odleglosc.ToString().Replace(",", "."), zrodlo, idProtokolu);
                    mocKermy = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<double>(0);
                }
                catch (Exception)
                {
                    listaWartRzeczywiste.Add(0);
                    continue;
                }

                listaWartRzeczywiste.Add(czasZmierzony[i] * mocKermy / przelicznik * korektaRozpad / 3600);
            }

            return listaWartRzeczywiste;
        }

        //---------------------------------------------------------------
        public List<double> LiczCzasWzorcowy(double odleglosc, int zrodlo, string protokol, string jednostka, List<double> progi, DateTime dataWzorcowania)
        //---------------------------------------------------------------
        {
            _Zapytanie = String.Format("SELECT Data_kalibracji, id_protokolu FROM Protokoly_kalibracji_lawy WHERE data_kalibracji=#{0}#", protokol);
             DateTime dataKalibracjiLawy = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<DateTime>(0);
             int idProtokolu = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<short>(1);

             int roznicaDni = (dataWzorcowania - dataKalibracjiLawy).Days;

             // 11050.0 - czas połowicznego rozpadu cezu w dniach
             double korektaRozpad = Math.Exp(-Math.Log(2.0) * roznicaDni / Constants.getInstance().CS_HALF_TIME_VALUE);

             List<double> listaCzasWzorcowy = new List<double>();
             

             _Zapytanie = String.Format("SELECT przelicznik FROM Jednostki WHERE jednostka='{0}'", jednostka);
             double przelicznik = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<float>(0);

             double mocKermy;

             for (UInt16 i = 0; i < progi.Count; ++i)
             {
                 try
                 {
                     _Zapytanie = String.Format("SELECT moc_kermy FROM Pomiary_wzorcowe WHERE odleglosc={0} AND id_zrodla={1} AND id_protokolu={2}", 
                                  odleglosc.ToString().Replace(",", "."), zrodlo, idProtokolu);
                     mocKermy = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<double>(0);
                 }
                 catch (Exception)
                 {
                     listaCzasWzorcowy.Add(0);
                     
                     continue;
                 }

                 listaCzasWzorcowy.Add( progi[i] * 3600 / (1.0/przelicznik * mocKermy * korektaRozpad));
             }

             return listaCzasWzorcowy;
        }

        //---------------------------------------------------------------
        override public bool NadpiszDaneWzorcoweIPomiarowe()
        //---------------------------------------------------------------
        {
            _Zapytanie = String.Format("DELETE FROM Sygnalizacja_dawka WHERE id_wzorcowania = {0}", _DaneOgolneDoZapisu.IdWzorcowania);
            _BazaDanych.WykonajPolecenie(_Zapytanie);

            for (int i = 0; i < Pomiary.Dane.Count; ++i)
            {
                _Zapytanie = String.Format("INSERT INTO Sygnalizacja_dawka VALUES ({0}, '{1}', '{2}', '{3}', '{4}', {5}, '{6}')",
                             _DaneOgolneDoZapisu.IdWzorcowania, Pomiary.Dane[i].Prog, Pomiary.Dane[i].WartRzeczywista,
                             Pomiary.Dane[i].WartZmierzona, Pomiary.odleglosc, Pomiary.zrodlo, Pomiary.Dane[i].Tzmierzony);
                _BazaDanych.WykonajPolecenie(_Zapytanie);
            }

            if (Pomiary.Dane.Count == 0)
            {
                _Zapytanie = String.Format("INSERT INTO Sygnalizacja_dawka VALUES ({0}, 0, 0, 0, '{1}', {2}, 0)",
                             _DaneOgolneDoZapisu.IdWzorcowania, Pomiary.odleglosc, Pomiary.zrodlo);
                _BazaDanych.WykonajPolecenie(_Zapytanie);
            }

            try
            {
                _Zapytanie = String.Format("SELECT id_protokolu FROM Protokoly_kalibracji_lawy WHERE data_kalibracji=#{0}#", Pomiary.protokol);
                short idProtokolu = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<short>(0);

                _Zapytanie = String.Format("SELECT id_jednostki FROM Jednostki WHERE jednostka='{0}'", Pomiary.jednostka);
                int idJednostki = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<int>(0);

                _Zapytanie = String.Format("UPDATE Wzorcowanie_cezem SET id_protokolu = {0}, id_jednostki = {1}, tlo = 0.0, wielkosc_fizyczna = 'nie dotyczy'  WHERE id_wzorcowania = {2}",
                                           idProtokolu, idJednostki, _DaneOgolneDoZapisu.IdWzorcowania);
                _BazaDanych.WykonajPolecenie(_Zapytanie);
            }
            catch (Exception)
            {
            }

            return true;
        }

        //---------------------------------------------------------------
        override public bool NadpiszDaneObliczonychWspolczynnikow()
        //---------------------------------------------------------------
        {
            return ZapiszDaneObliczonychWspolczynnikow();
        }

        //---------------------------------------------------------------
        public bool PobierzDaneProtokolow()
        //---------------------------------------------------------------
        {
            _Zapytanie = "SELECT osoba_wzorcujaca, osoba_sprawdzajaca, Dolacz FROM Wzorcowanie_cezem WHERE "
                       + String.Format("Id_wzorcowania = {0}", IdWzorcowania);
            _OdpowiedzBazy = _BazaDanych.TworzTabeleDanych(_Zapytanie);

            if (null == _OdpowiedzBazy || _OdpowiedzBazy.Rows.Count == 0)
                return false;

            Protokol.Wzorcujacy = _OdpowiedzBazy.Rows[0].Field<string>(0);
            Protokol.Sprawdzajacy = _OdpowiedzBazy.Rows[0].Field<string>(1);
            Protokol.Dolacz = _OdpowiedzBazy.Rows[0].Field<bool>(2);

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
        private bool PobierzIdWzorcowania()
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
            _Zapytanie = "SELECT prog, wartosc_wzorcowa, wartosc_zmierzona, czas_zmierzony FROM Sygnalizacja_dawka WHERE id_wzorcowania "
                       + String.Format("IN (SELECT id_wzorcowania FROM Wzorcowanie_cezem WHERE id_karty = {0} AND id_arkusza = {1})",
                       IdKarty, IdArkusza);

            _OdpowiedzBazy = _BazaDanych.TworzTabeleDanych(_Zapytanie);

            if (null == _OdpowiedzBazy || null == _OdpowiedzBazy.Rows)
                return false;

            foreach (DataRow wiersz in _OdpowiedzBazy.Rows)
            {
                Pomiary.Dane.Add(
                                    new KlasyPomocniczeSygDawki.DawkaWartosciWzorcowoPomiarowe.DawkaWartoscWzorcowoPomiarowa
                                    (
                                        wiersz.Field<double>("prog"),
                                        0.0,
                                        wiersz.Field<double>("czas_zmierzony"),
                                        wiersz.Field<double>("wartosc_wzorcowa"),
                                        wiersz.Field<double>("wartosc_zmierzona")
                                    )
                               );
            }

            _Zapytanie = String.Format("SELECT odleglosc, id_zrodla FROM Sygnalizacja_dawka WHERE id_wzorcowania = {0}", IdWzorcowania);
            _OdpowiedzBazy = _BazaDanych.TworzTabeleDanych(_Zapytanie);

            try
            {
                Pomiary.odleglosc = _OdpowiedzBazy.Rows[0].Field<double>(0).ToString();
                Pomiary.zrodlo = _OdpowiedzBazy.Rows[0].Field<int>(1).ToString();
            }
            catch (Exception)
            {
                Pomiary.odleglosc = "0";
                Pomiary.zrodlo = "0";
            }

            return true;
        }

        //---------------------------------------------------------------
        public bool PrzygotujDaneWzorcowoPomiaroweDoZapisu(ref System.Windows.Forms.DataGridView tabela, string protokol, string zrodlo, string odleglosc, string jednostka)
        //---------------------------------------------------------------
        {
            Pomiary.Dane.Clear();

            if("" != jednostka || "" != protokol || "" != zrodlo)
            {
                Pomiary.jednostka = jednostka;
                Pomiary.protokol = protokol;
                Pomiary.zrodlo = zrodlo;
            }
            else
                return false;

            if("" != odleglosc)
                Pomiary.odleglosc = odleglosc;
            else
                Pomiary.odleglosc = "0";

            string sTemp;

            try
            {
                for (UInt16 i = 0; tabela.Rows[i].Cells[0].Value != null &&
                                   tabela.Rows[i].Cells[3].Value != null &&
                                   tabela.Rows[i].Cells[4].Value != null && i < tabela.Rows.Count - 1; ++i)
                {
                    KlasyPomocniczeSygDawki.DawkaWartosciWzorcowoPomiarowe.DawkaWartoscWzorcowoPomiarowa temp
                    = new KlasyPomocniczeSygDawki.DawkaWartosciWzorcowoPomiarowe.DawkaWartoscWzorcowoPomiarowa();

                    // próg
                    sTemp = tabela.Rows[i].Cells["Prog"].Value.ToString();

                    if (sTemp != "")
                        temp.Prog = double.Parse(sTemp);
                    else
                        temp.Prog = 0.0;

                    // wartość rzeczywista
                    sTemp = tabela.Rows[i].Cells["wartRzeczywista"].Value.ToString();

                    if (sTemp != "")
                        temp.WartRzeczywista = double.Parse(sTemp);
                    else
                        temp.WartRzeczywista = 0.0;

                    // wskazanie
                    sTemp = tabela.Rows[i].Cells["Wskazanie"].Value.ToString();

                    if (sTemp != "")
                        temp.WartZmierzona = double.Parse(sTemp);
                    else
                        temp.WartZmierzona = 0.0;

                    // czas zmierzony
                    sTemp = tabela.Rows[i].Cells["tZmierzony"].Value.ToString();

                    if (sTemp != "")
                        temp.Tzmierzony = double.Parse(sTemp);
                    else
                        temp.Tzmierzony = 0.0;

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
        public bool PrzygotujDaneProtokolowDoZapisu(string wzorcujacy, string sprawdzajacy, bool dolacz)
        //---------------------------------------------------------------
        {
            if ("" != wzorcujacy)
                Protokol.Wzorcujacy = wzorcujacy;
            else
                Protokol.Wzorcujacy = "Nie podano.";

            if ("" != sprawdzajacy)
                Protokol.Sprawdzajacy = sprawdzajacy;
            else
                Protokol.Sprawdzajacy = "Nie podano.";

            Protokol.Dolacz = dolacz;
            
            return true;
        }

        //---------------------------------------------------------------
        override public bool ZapiszDaneWzorcoweIPomiarowe()
        //---------------------------------------------------------------
        {
            for (int i = 0; i < Pomiary.Dane.Count; ++i)
            {
                _Zapytanie = String.Format("INSERT INTO Sygnalizacja_dawka VALUES ({0}, '{1}', '{2}', '{3}', '{4}', {5}, '{6}')",
                             _DaneOgolneDoZapisu.IdWzorcowania, Pomiary.Dane[i].Prog, Pomiary.Dane[i].WartRzeczywista,
                             Pomiary.Dane[i].WartZmierzona, Pomiary.odleglosc, Pomiary.zrodlo, Pomiary.Dane[i].Tzmierzony);
                _BazaDanych.WykonajPolecenie(_Zapytanie);
            }

            if (Pomiary.Dane.Count == 0)
            {
                _Zapytanie = String.Format("INSERT INTO Sygnalizacja_dawka VALUES ({0}, 0, 0, 0, '{1}', {2}, 0)",
                             _DaneOgolneDoZapisu.IdWzorcowania, Pomiary.odleglosc, Pomiary.zrodlo);
                _BazaDanych.WykonajPolecenie(_Zapytanie);
            }

            try
            {
                _Zapytanie = String.Format("SELECT id_protokolu FROM Protokoly_kalibracji_lawy WHERE data_kalibracji=#{0}#", Pomiary.protokol);
                short idProtokolu = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<short>(0);

                _Zapytanie = String.Format("SELECT id_jednostki FROM Jednostki WHERE jednostka='{0}'", Pomiary.jednostka);
                int idJednostki = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<int>(0);

                _Zapytanie = String.Format("UPDATE Wzorcowanie_cezem SET id_protokolu = {0}, id_jednostki = {1}, tlo = 0.0, wielkosc_fizyczna = 'nie dotyczy'  WHERE id_wzorcowania = {2}",
                                           idProtokolu, idJednostki, _DaneOgolneDoZapisu.IdWzorcowania);
                _BazaDanych.WykonajPolecenie(_Zapytanie);
            }
            catch (Exception)
            {
            }

            return true;
        }

        //---------------------------------------------------------------
        // w zasadzie dane protokołu
        override public bool ZapiszDaneObliczonychWspolczynnikow()
        //---------------------------------------------------------------
        {
            _Zapytanie = String.Format("UPDATE Wzorcowanie_cezem SET osoba_wzorcujaca='{0}', osoba_sprawdzajaca='{1}', Dolacz={2} WHERE id_wzorcowania={3}",
                                       Protokol.Wzorcujacy, Protokol.Sprawdzajacy, Protokol.Dolacz, _DaneOgolneDoZapisu.IdWzorcowania);
            _BazaDanych.WykonajPolecenie(_Zapytanie);

            return true;
        }

    }
}
