﻿using System;
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
        public List<double>[] LiczWartoscRzecyzwistaOld(double odleglosc, int zrodlo, string protokol, string jednostka, List<double> czasZmierzony, DateTime dataWzorcowania)
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
            List<double> empty = new List<double>();
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
                    empty.Add(0);
                    continue;
                }

                listaWartRzeczywiste.Add(czasZmierzony[i] * mocKermy / przelicznik * korektaRozpad / 3600);
                empty.Add(0);
            }

            return new List<double>[] { listaWartRzeczywiste, empty, empty, empty };
        }
        
        //---------------------------------------------------------------
        public List<double>[] LiczWartoscRzecyzwistaOd20230915(double odleglosc, int zrodlo, string protokol, string jednostka, List<double> czasZmierzony, List<double> progi, DateTime dataWzorcowania)
        //---------------------------------------------------------------
        {
            _Zapytanie = String.Format("SELECT Data_kalibracji, id_protokolu FROM Protokoly_kalibracji_lawy WHERE data_kalibracji=#{0}#", protokol);
            DateTime dataKalibracjiLawy = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<DateTime>(0);
            int idProtokolu = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<short>(1);
            var stale = Constants.getInstance();
            
            int roznicaDni = (dataWzorcowania - dataKalibracjiLawy).Days;

            // 11050.0 - czas połowicznego rozpadu cezu w dniach
            double korektaRozpad = Math.Exp(-Math.Log(2.0) * roznicaDni / stale.CS_HALF_TIME_VALUE);

            _Zapytanie = String.Format("SELECT przelicznik FROM Jednostki WHERE jednostka='{0}'", jednostka);
            double przelicznik = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<float>(0);

            List<double> listaWartRzeczywiste = new List<double>();
            List<double> niepewnosci = new List<double>();
            List<double> wspolczynniki = new List<double>();
            List<double> niepewnosci_wsp = new List<double>();
            double mocKermy;
            double niepewnoscWzorcowa;

            for (UInt16 i = 0; i < czasZmierzony.Count; ++i)
            {
                try
                {
                    _Zapytanie = String.Format("SELECT moc_kermy, Niepewnosc FROM Pomiary_wzorcowe WHERE odleglosc={0} AND id_zrodla={1} AND id_protokolu={2}",
                                 odleglosc.ToString().Replace(",", "."), zrodlo, idProtokolu);
                    var row = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0];
                    mocKermy = row.Field<double>(0);
                    niepewnoscWzorcowa = row.Field<double>(1);
                }
                catch (Exception)
                {
                    listaWartRzeczywiste.Add(0);
                    niepewnosci.Add(0);
                    wspolczynniki.Add(0);
                    niepewnosci_wsp.Add(0);
                    continue;
                }

                double wkt = (Math.Log(2) * roznicaDni / stale.T12Cs) * Math.Sqrt((stale.ut / roznicaDni) * (stale.ut / roznicaDni) + (stale.ukT12Cs / stale.T12Cs) * (stale.ukT12Cs / stale.T12Cs));
                double wkd = 2 * stale.DELTA_L_D / 2 / Math.Sqrt(3) / 10 / odleglosc;
                double ut = stale.UT_SD / 2 / Math.Sqrt(3);
                double wt = ut / czasZmierzony[i];

                double wartRzeczywista = czasZmierzony[i] * mocKermy / przelicznik * korektaRozpad / 3600;

                double ukjed = stale.UKJED;

                if (jednostka.ToLower().IndexOf("sv") < 0)
                {
                    ukjed = 0;
                }

                double niepewnosc = wartRzeczywista * Math.Sqrt(Math.Pow(wkt, 2) + Math.Pow(wkd, 2) + Math.Pow(ukjed, 2) + Math.Pow(wt, 2) + Math.Pow(niepewnoscWzorcowa, 2));

                double wspolczynnik = wartRzeczywista / progi[i];

                double niepewnosc_wsp = wspolczynnik * 2 * Math.Sqrt(Math.Pow(wkt, 2) + Math.Pow(wkd, 2) + Math.Pow(ukjed, 2) + Math.Pow(wt, 2) + Math.Pow(niepewnoscWzorcowa, 2));

                listaWartRzeczywiste.Add(wartRzeczywista);
                niepewnosci.Add(niepewnosc);
                wspolczynniki.Add(wspolczynnik);
                niepewnosci_wsp.Add(niepewnosc_wsp);
            }

            return new List<double>[] { listaWartRzeczywiste, niepewnosci, wspolczynniki, niepewnosci_wsp };
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
            /*_Zapytanie = String.Format("DELETE FROM Sygnalizacja_dawka WHERE id_wzorcowania = {0}", _DaneOgolneDoZapisu.IdWzorcowania);
            _BazaDanych.WykonajPolecenie(_Zapytanie);*/
            _BazaDanych.Sygnalizacja_dawka
                .DELETE()
                .WHERE().ID_wzorcowania(_DaneOgolneDoZapisu.IdWzorcowania)
                .INFO("Wyczyszczenie przed zapisem danych wzorcowych i pomiarowych")
                .EXECUTE();

            for (int i = 0; i < Pomiary.Dane.Count; ++i)
            {
                /*_Zapytanie = String.Format("INSERT INTO Sygnalizacja_dawka VALUES ({0}, '{1}', '{2}', '{3}', '{4}', {5}, '{6}', '{7}', '{8}', '{9}')",
                             _DaneOgolneDoZapisu.IdWzorcowania, Pomiary.Dane[i].Prog, Pomiary.Dane[i].WartRzeczywista,
                             Pomiary.Dane[i].WartZmierzona, Pomiary.odleglosc, Pomiary.zrodlo, Pomiary.Dane[i].Tzmierzony,
                             Pomiary.Dane[i].Niepewnosc, Pomiary.Dane[i].Wspolczynnik, Pomiary.Dane[i].Niepewnosc_wsp);
                _BazaDanych.WykonajPolecenie(_Zapytanie);*/
                _BazaDanych.Sygnalizacja_dawka
                    .INSERT()
                        .ID_wzorcowania(_DaneOgolneDoZapisu.IdWzorcowania)
                        .Prog(Pomiary.Dane[i].Prog)
                        .Wartosc_wzorcowa(Pomiary.Dane[i].WartRzeczywista)
                        .Wartosc_zmierzona(Pomiary.Dane[i].WartZmierzona)
                        .Odleglosc(Pomiary.odleglosc)
                        .ID_zrodla(Pomiary.zrodlo)
                        .Czas_zmierzony(Pomiary.Dane[i].Tzmierzony)
                        .Niepewnosc(Pomiary.Dane[i].Niepewnosc)
                        .Wspolczynnik(Pomiary.Dane[i].Wspolczynnik)
                        .Niepewnosc_wsp(Pomiary.Dane[i].Niepewnosc_wsp)
                    .EXECUTE();
            }

            if (Pomiary.Dane.Count == 0)
            {
                /*_Zapytanie = String.Format("INSERT INTO Sygnalizacja_dawka VALUES ({0}, 0, 0, 0, '{1}', {2}, 0, 0, 0, 0)",
                             _DaneOgolneDoZapisu.IdWzorcowania, Pomiary.odleglosc, Pomiary.zrodlo);
                _BazaDanych.WykonajPolecenie(_Zapytanie);*/
                _BazaDanych.Sygnalizacja_dawka
                    .INSERT()
                        .ID_wzorcowania(_DaneOgolneDoZapisu.IdWzorcowania)
                        .Prog(0)
                        .Wartosc_wzorcowa(0)
                        .Wartosc_zmierzona(0)
                        .Odleglosc(Pomiary.odleglosc)
                        .ID_zrodla(Pomiary.zrodlo)
                        .Czas_zmierzony(0)
                        .Niepewnosc(0)
                        .Wspolczynnik(0)
                        .Niepewnosc_wsp(0)
                    .INSERT()
                    .EXECUTE();
            }

            try
            {
                _Zapytanie = String.Format("SELECT id_protokolu FROM Protokoly_kalibracji_lawy WHERE data_kalibracji=#{0}#", Pomiary.protokol);
                short idProtokolu = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<short>(0);

                _Zapytanie = String.Format("SELECT id_jednostki FROM Jednostki WHERE jednostka='{0}'", Pomiary.jednostka);
                int idJednostki = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<int>(0);

                /*_Zapytanie = String.Format("UPDATE Wzorcowanie_cezem SET id_protokolu = {0}, id_jednostki = {1}, tlo = 0.0, wielkosc_fizyczna = 'nie dotyczy'  WHERE id_wzorcowania = {2}",
                                           idProtokolu, idJednostki, _DaneOgolneDoZapisu.IdWzorcowania);
                _BazaDanych.WykonajPolecenie(_Zapytanie);*/
                _BazaDanych.wzorcowanie_cezem
                    .UPDATE()
                        .ID_protokolu(idProtokolu)
                        .ID_jednostki(idJednostki)
                        .Tlo("0.0")
                        .Wielkosc_fizyczna("nie dotyczy")
                    .WHERE()
                        .ID_wzorcowania(_DaneOgolneDoZapisu.IdWzorcowania)
                    .EXECUTE();
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
            _Zapytanie = "SELECT prog, wartosc_wzorcowa, wartosc_zmierzona, czas_zmierzony, Niepewnosc, Wspolczynnik, Niepewnosc_wsp FROM Sygnalizacja_dawka WHERE id_wzorcowania "
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
                                        wiersz.Field<double>("Niepewnosc"),
                                        wiersz.Field<double>("wartosc_zmierzona"),
                                        wiersz.Field<double>("Wspolczynnik"),
                                        wiersz.Field<double>("Niepewnosc_wsp")
                                    )
                               );
            }

            _Zapytanie = String.Format("SELECT odleglosc, id_zrodla FROM Sygnalizacja_dawka WHERE id_wzorcowania = {0}", IdWzorcowania);
            _OdpowiedzBazy = _BazaDanych.TworzTabeleDanych(_Zapytanie);

            try
            {
                Pomiary.odleglosc = _OdpowiedzBazy.Rows[0].Field<double>(0);
                Pomiary.zrodlo = _OdpowiedzBazy.Rows[0].Field<int>(1);
            }
            catch (Exception)
            {
                Pomiary.odleglosc = 0.0;
                Pomiary.zrodlo = 0;
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
                Pomiary.zrodlo = N.intParse(zrodlo);
            }
            else
                return false;

            if("" != odleglosc)
                Pomiary.odleglosc = N.doubleParse(odleglosc);
            else
                Pomiary.odleglosc = 0.0;

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
                        temp.Prog = N.doubleParse(sTemp);
                    else
                        temp.Prog = 0.0;

                    // wartość rzeczywista
                    sTemp = tabela.Rows[i].Cells["wartRzeczywista"].Value.ToString();

                    if (sTemp != "")
                        temp.WartRzeczywista = N.doubleParse(sTemp);
                    else
                        temp.WartRzeczywista = 0.0;

                    // wskazanie
                    sTemp = tabela.Rows[i].Cells["Wskazanie"].Value.ToString();

                    if (sTemp != "")
                        temp.WartZmierzona = N.doubleParse(sTemp);
                    else
                        temp.WartZmierzona = 0.0;

                    // czas zmierzony
                    sTemp = tabela.Rows[i].Cells["tZmierzony"].Value.ToString();

                    if (sTemp != "")
                        temp.Tzmierzony = N.doubleParse(sTemp);
                    else
                        temp.Tzmierzony = 0.0;

                    // Niepewnosc
                    sTemp = tabela.Rows[i].Cells["Niepewnosc"].Value.ToString();

                    if (sTemp != "")
                        temp.Niepewnosc = N.doubleParse(sTemp);
                    else
                        temp.Niepewnosc = 0.0;

                    // czas zmierzony
                    sTemp = tabela.Rows[i].Cells["Wspolczynnik"].Value.ToString();

                    if (sTemp != "")
                        temp.Wspolczynnik = N.doubleParse(sTemp);
                    else
                        temp.Wspolczynnik = 0.0;

                    // czas zmierzony
                    sTemp = tabela.Rows[i].Cells["Niepewnosc_wsp"].Value.ToString();

                    if (sTemp != "")
                        temp.Niepewnosc_wsp = N.doubleParse(sTemp);
                    else
                        temp.Niepewnosc_wsp = 0.0;

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
                /*_Zapytanie = String.Format("INSERT INTO Sygnalizacja_dawka VALUES ({0}, '{1}', '{2}', '{3}', '{4}', {5}, '{6}', '{7}', '{8}', '{9}')",
                             _DaneOgolneDoZapisu.IdWzorcowania, Pomiary.Dane[i].Prog, Pomiary.Dane[i].WartRzeczywista,
                             Pomiary.Dane[i].WartZmierzona, Pomiary.odleglosc, Pomiary.zrodlo, Pomiary.Dane[i].Tzmierzony,
                             Pomiary.Dane[i].Niepewnosc, Pomiary.Dane[i].Wspolczynnik, Pomiary.Dane[i].Niepewnosc_wsp);
                _BazaDanych.WykonajPolecenie(_Zapytanie);*/
                _BazaDanych.Sygnalizacja_dawka
                    .INSERT()
                        .ID_wzorcowania(_DaneOgolneDoZapisu.IdWzorcowania)
                        .Prog(Pomiary.Dane[i].Prog)
                        .Wartosc_wzorcowa(Pomiary.Dane[i].WartRzeczywista)
                        .Wartosc_zmierzona(Pomiary.Dane[i].WartZmierzona)
                        .Odleglosc(Pomiary.odleglosc)
                        .ID_zrodla(Pomiary.zrodlo)
                        .Czas_zmierzony(Pomiary.Dane[i].Tzmierzony)
                        .Niepewnosc(Pomiary.Dane[i].Niepewnosc)
                        .Wspolczynnik(Pomiary.Dane[i].Wspolczynnik)
                        .Niepewnosc_wsp(Pomiary.Dane[i].Niepewnosc_wsp)
                    .EXECUTE();
            }

            if (Pomiary.Dane.Count == 0)
            {
                /*_Zapytanie = String.Format("INSERT INTO Sygnalizacja_dawka VALUES ({0}, 0, 0, 0, '{1}', {2}, 0, 0, 0, 0)",
                             _DaneOgolneDoZapisu.IdWzorcowania, Pomiary.odleglosc, Pomiary.zrodlo);
                _BazaDanych.WykonajPolecenie(_Zapytanie);*/
                _BazaDanych.Sygnalizacja_dawka
                    .INSERT()
                        .ID_wzorcowania(_DaneOgolneDoZapisu.IdWzorcowania)
                        .Prog(0)
                        .Wartosc_wzorcowa(0)
                        .Wartosc_zmierzona(0)
                        .Odleglosc(Pomiary.odleglosc)
                        .ID_zrodla(Pomiary.zrodlo)
                        .Czas_zmierzony(0)
                        .Niepewnosc(0)
                        .Wspolczynnik(0)
                        .Niepewnosc_wsp(0)
                    .EXECUTE();
            }

            try
            {
                _Zapytanie = String.Format("SELECT id_protokolu FROM Protokoly_kalibracji_lawy WHERE data_kalibracji=#{0}#", Pomiary.protokol);
                short idProtokolu = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<short>(0);

                _Zapytanie = String.Format("SELECT id_jednostki FROM Jednostki WHERE jednostka='{0}'", Pomiary.jednostka);
                int idJednostki = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<int>(0);

                /*_Zapytanie = String.Format("UPDATE Wzorcowanie_cezem SET id_protokolu = {0}, id_jednostki = {1}, tlo = 0.0, wielkosc_fizyczna = 'nie dotyczy'  WHERE id_wzorcowania = {2}",
                                           idProtokolu, idJednostki, _DaneOgolneDoZapisu.IdWzorcowania);
                _BazaDanych.WykonajPolecenie(_Zapytanie);*/
                _BazaDanych.wzorcowanie_cezem
                    .UPDATE()
                        .ID_protokolu(idProtokolu)
                        .ID_jednostki(idJednostki)
                        .Tlo("0.0")
                        .Wielkosc_fizyczna("nie dotyczy")
                    .WHERE()
                        .ID_wzorcowania(_DaneOgolneDoZapisu.IdWzorcowania)
                    .EXECUTE();
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
            /*_Zapytanie = String.Format("UPDATE Wzorcowanie_cezem SET osoba_wzorcujaca='{0}', osoba_sprawdzajaca='{1}', Dolacz={2} WHERE id_wzorcowania={3}",
                                       Protokol.Wzorcujacy, Protokol.Sprawdzajacy, Protokol.Dolacz, _DaneOgolneDoZapisu.IdWzorcowania);
            _BazaDanych.WykonajPolecenie(_Zapytanie);*/
            _BazaDanych.wzorcowanie_cezem
                .UPDATE()
                    .Osoba_wzorcujaca(Protokol.Wzorcujacy)
                    .Osoba_sprawdzajaca(Protokol.Sprawdzajacy)
                    .Dolacz(Protokol.Dolacz)
                .WHERE()
                    .ID_wzorcowania(_DaneOgolneDoZapisu.IdWzorcowania)
                .EXECUTE();
            return true;
        }

    }
}
