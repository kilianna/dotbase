using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DotBase
{
    class WzorcowanieSygMocyDawki : WzorcowanieCez
    {
        public class DaneWzorcowoPomiarowe
        {
            public DaneWzorcowoPomiarowe(String protokol, String jednostka, DateTime data)
            {
                m_Data = data;
                m_Protokol = protokol;
                m_Jednostka = jednostka;
                m_Zrodlo1 = new List<UInt16>();
                m_Zrodlo2 = new List<UInt16>();
                m_Odleglosc1 = new List<double>();
                m_Odleglosc2 = new List<double>();
                m_Prog = new List<double>();
            }
            public DateTime m_Data;
            public String m_Protokol;
            public String m_Jednostka;
            public List<UInt16> m_Zrodlo1;
            public List<UInt16> m_Zrodlo2;
            public List<Double> m_Odleglosc1;
            public List<Double> m_Odleglosc2;
            public List<double> m_Prog;
        };

        public KlasyPomocniczeCez.Protokol Protokol { get; private set; }
        public KlasyPomocniczeSygMocyDawki.DawkaWartosciWzorcowoPomiarowe Pomiary { get; private set; }

        //---------------------------------------------------------------
        public WzorcowanieSygMocyDawki(int idKarty, string rodzajWzorcowania)
            : base(idKarty, rodzajWzorcowania)
        //---------------------------------------------------------------
        {
            Protokol = new KlasyPomocniczeCez.Protokol();
            Pomiary = new KlasyPomocniczeSygMocyDawki.DawkaWartosciWzorcowoPomiarowe();
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
        public List<double>[] LiczWartoscOrazNiepewnoscOld(DaneWzorcowoPomiarowe daneWejsciowe)
        //---------------------------------------------------------------
        {
            List<Double> wartosci = new List<double>();
            List<Double> niepewnosci = new List<double>();
            List<Double> empty1 = new List<double>();
            List<Double> empty2 = new List<double>();

            SortedDictionary<Double, Double>[] wzorcowe = new SortedDictionary<Double, Double>[] { new SortedDictionary<Double, Double>(), new SortedDictionary<Double, Double>(), new SortedDictionary<Double, Double>() };

            _Zapytanie = String.Format("SELECT id_protokolu FROM Protokoly_kalibracji_lawy WHERE data_kalibracji=#{0}#", daneWejsciowe.m_Protokol);
            short idProtokolu = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<short>(0);

            for (int i = 1; i < 4; i++)
            {
                _Zapytanie = "SELECT odleglosc, moc_kermy FROM pomiary_wzorcowe WHERE id_protokolu=" + idProtokolu + " AND id_zrodla=" + i;
                foreach (DataRow wiersz in _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows)
                {
                    wzorcowe[i - 1].Add(wiersz.Field<double>(0), wiersz.Field<double>(1));
                }
            }

            for (UInt16 i = 0; i < daneWejsciowe.m_Zrodlo1.Count; ++i)
            {
                double temp1 = LiczWartoscOrazNiepewnoscDlaZrodlaOld(wzorcowe[daneWejsciowe.m_Zrodlo1[i] - 1], daneWejsciowe.m_Odleglosc1[i]);
                double temp2 = LiczWartoscOrazNiepewnoscDlaZrodlaOld(wzorcowe[daneWejsciowe.m_Zrodlo2[i] - 1], daneWejsciowe.m_Odleglosc2[i]);
                double roznicaPomiarowDni = (daneWejsciowe.m_Data - DateTime.Parse(daneWejsciowe.m_Protokol)).Days;
                double korektaNaRozpad = Math.Exp(-Math.Log(2) / Constants.getInstance().CS_HALF_TIME_VALUE * roznicaPomiarowDni);
                temp1 *= korektaNaRozpad;
                temp2 *= korektaNaRozpad;

                switch(daneWejsciowe.m_Jednostka)
                {
                    case "mSv/h":
                        temp1 *= 0.0012;
                        temp2 *= 0.0012;
                        break;
                    case "uSv/h":
                        temp1 *= 1.2;
                        temp2 *= 1.2;
                        break;
                    case "uGy/h":
                        temp1 *= 1.0;
                        temp2 *= 1.0;
                        break;
                    case "nA/kg":
                        temp1 *= 1.0 / 121.9;
                        temp2 *= 1.0 / 121.9;
                        break;
                    case "mR/h":
                        temp1 *= 1.00 / 8.74;
                        temp2 *= 1.00 / 8.74;
                        break;
                    default:
                        return null;
                }

                wartosci.Add((temp1 + temp2) / 2);
                niepewnosci.Add(Math.Abs((temp1 + temp2) / 2 - temp1));
                empty1.Add(0);
                empty2.Add(0);
            }

            return new List<double>[] { wartosci, niepewnosci, empty1, empty2 };
        }

        //---------------------------------------------------------------
        public List<double>[] LiczWartoscOrazNiepewnosc20230915(DaneWzorcowoPomiarowe daneWejsciowe)
        //---------------------------------------------------------------
        {
            List<Double> wartosci = new List<double>();
            List<Double> niepewnosci = new List<double>();
            List<Double> wspolczynniki = new List<double>();
            List<Double> niepewnosci_wsp = new List<double>();
            var stale = Constants.getInstance();

            SortedDictionary<Double, Double>[] wzorcowe = new SortedDictionary<Double, Double>[] { new SortedDictionary<Double, Double>(), new SortedDictionary<Double, Double>(), new SortedDictionary<Double, Double>() };

            _Zapytanie = String.Format("SELECT id_protokolu FROM Protokoly_kalibracji_lawy WHERE data_kalibracji=#{0}#", daneWejsciowe.m_Protokol);
            short idProtokolu = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<short>(0);

            for (int i = 1; i < 4; i++)
            {
                _Zapytanie = "SELECT odleglosc, Niepewnosc FROM pomiary_wzorcowe WHERE id_protokolu=" + idProtokolu + " AND id_zrodla=" + i;
                foreach (DataRow wiersz in _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows)
                {
                    wzorcowe[i - 1].Add(wiersz.Field<double>(0), wiersz.Field<double>(1));
                }
            }

            for (UInt16 i = 0; i < daneWejsciowe.m_Zrodlo1.Count; ++i)
            {
                double wartosc_wzorcowa_1, wartosc_wzorcowa_2;
                double niepewnosc_wart_wzorcowej_1, niepewnosc_wart_wzorcowej_2;
                LiczWartoscOrazNiepewnoscDlaZrodla20230915(daneWejsciowe.m_Zrodlo1[i], wzorcowe[daneWejsciowe.m_Zrodlo1[i] - 1], daneWejsciowe.m_Odleglosc1[i], out wartosc_wzorcowa_1, out niepewnosc_wart_wzorcowej_1);
                LiczWartoscOrazNiepewnoscDlaZrodla20230915(daneWejsciowe.m_Zrodlo2[i], wzorcowe[daneWejsciowe.m_Zrodlo2[i] - 1], daneWejsciowe.m_Odleglosc2[i], out wartosc_wzorcowa_2, out niepewnosc_wart_wzorcowej_2);
                double roznicaPomiarowDni = (daneWejsciowe.m_Data - DateTime.Parse(daneWejsciowe.m_Protokol)).Days;
                double korektaNaRozpad = Math.Exp(-Math.Log(2) / Constants.getInstance().CS_HALF_TIME_VALUE * roznicaPomiarowDni);
                wartosc_wzorcowa_1 *= korektaNaRozpad;
                wartosc_wzorcowa_2 *= korektaNaRozpad;

                switch (daneWejsciowe.m_Jednostka)
                {
                    case "mSv/h":
                        wartosc_wzorcowa_1 *= 1.21;
                        wartosc_wzorcowa_2 *= 1.21;
                        break;
                    case "uSv/h":
                        wartosc_wzorcowa_1 *= 1210;
                        wartosc_wzorcowa_2 *= 1210;
                        break;
                    case "uGy/h":
                        wartosc_wzorcowa_1 *= 1000;
                        wartosc_wzorcowa_2 *= 1000;
                        break;
                    case "nA/kg":
                        wartosc_wzorcowa_1 *= 1000 / 121.9;
                        wartosc_wzorcowa_2 *= 1000 / 121.9;
                        break;
                    case "mR/h":
                        wartosc_wzorcowa_1 *= 1000 / 8.74;
                        wartosc_wzorcowa_2 *= 1000 / 8.74;
                        break;
                    default:
                        return null;
                }

                double wartosc_wzorcowa = (wartosc_wzorcowa_1 + wartosc_wzorcowa_2) / 2;
                double niepewnosc = wartosc_wzorcowa * Math.Sqrt(Math.Pow(niepewnosc_wart_wzorcowej_1, 2) + Math.Pow(niepewnosc_wart_wzorcowej_2, 2));

                double K1 = wartosc_wzorcowa_1 / daneWejsciowe.m_Prog[i];
                double K2 = wartosc_wzorcowa_2 / daneWejsciowe.m_Prog[i];
                double wspolczynnik = (K1 + K2) / 2;

                double ukjed = stale.UKJED;

                if (daneWejsciowe.m_Jednostka.ToLower().IndexOf("sv") < 0)
                {
                    ukjed = 0;
                }

                double wd1 = 2 * stale.DELTA_L_MD / 2 / Math.Sqrt(3) / 10 / daneWejsciowe.m_Odleglosc1[i];
                double wd2 = 2 * stale.DELTA_L_MD / 2 / Math.Sqrt(3) / 10 / daneWejsciowe.m_Odleglosc2[i];
                double wkt = (Math.Log(2) * roznicaPomiarowDni / stale.T12Cs) * Math.Sqrt((stale.ut / roznicaPomiarowDni) * (stale.ut / roznicaPomiarowDni) + (stale.ukT12Cs / stale.T12Cs) * (stale.ukT12Cs / stale.T12Cs));
                double wk1 = Math.Sqrt(Math.Pow(niepewnosc_wart_wzorcowej_1, 2) + Math.Pow(ukjed, 2) + Math.Pow(wkt, 2) + Math.Pow(wd1, 2));
                double wk2 = Math.Sqrt(Math.Pow(niepewnosc_wart_wzorcowej_2, 2) + Math.Pow(ukjed, 2) + Math.Pow(wkt, 2) + Math.Pow(wd2, 2));
                double wk = (wk1 + wk2) / 2;
                double wkp = Math.Abs(wspolczynnik - K1) / Math.Sqrt(3) / wspolczynnik;
                double uk = wspolczynnik * Math.Sqrt(wk * wk + wkp * wkp);
                double niepewnosc_wspolczynnika = 2 * uk;

                wartosci.Add(wartosc_wzorcowa);
                niepewnosci.Add(niepewnosc);
                wspolczynniki.Add(wspolczynnik);
                niepewnosci_wsp.Add(niepewnosc_wspolczynnika);
            }

            return new List<double>[] { wartosci, niepewnosci, wspolczynniki, niepewnosci_wsp };
        }
        


        /*
        * The old way:
        //---------------------------------------------------------------
        private double LiczWartoscOrazNiepewnoscDlaZrodla1(Dictionary<String, Double> stale, double odleglosc)
        //---------------------------------------------------------------
        {
            double returnValue;

            if (odleglosc > stale["smdx1"])
            {
                returnValue = (stale["smdG11"] / Math.Pow(odleglosc + stale["smdE1"], 2.0)) / stale["smdG1"] * Math.Exp(stale["smdH4"] * (odleglosc + stale["smdE1"])) * (1 + (stale["smdH1"] * Math.Pow(odleglosc + stale["smdE1"], 3.0) + stale["smdH2"] * Math.Pow(odleglosc + stale["smdE1"], 2) + stale["smdH3"] * (odleglosc + stale["smdE1"]) - 0.02));
            }
            else
            {
                returnValue = (stale["smdG11"] / Math.Pow(odleglosc + stale["smdE1"], 2.0)) / stale["smdG1"] * Math.Exp(stale["smdH4"] * (odleglosc + stale["smdE1"]));
            }

            return returnValue;
        }

        //---------------------------------------------------------------
        private double LiczWartoscOrazNiepewnoscDlaZrodla2(Dictionary<String, Double> stale, double odleglosc)
        //---------------------------------------------------------------
        {
            double returnValue;

            if (odleglosc > stale["smdx2"])
            {
                returnValue = (stale["smdG11"] / Math.Pow(odleglosc + stale["smdE1"], 2.0)) / stale["smdG6"] * Math.Exp(stale["smdH9"] * (odleglosc + stale["smdE1"])) * (1 + (stale["smdH6"] * Math.Pow(odleglosc + stale["smdE1"], 3.0) + stale["smdH7"] * Math.Pow(odleglosc + stale["smdE1"], 2) + stale["smdH8"] * (odleglosc + stale["smdE1"]) - 0.02));
            }
            else
            {
                returnValue = (stale["smdG11"] / Math.Pow(odleglosc + stale["smdE1"], 2.0)) / stale["smdG6"] * Math.Exp(stale["smdH4"] * (odleglosc + stale["smdE1"]));
            }

            return returnValue;
        }

        //---------------------------------------------------------------
        private double LiczWartoscOrazNiepewnoscDlaZrodla3(Dictionary<String, Double> stale, double odleglosc)
        //---------------------------------------------------------------
        {
            double returnValue;

            if (odleglosc > stale["smdx3"])
            {
                returnValue = (stale["smdG11"] / Math.Pow(odxleglosc + stale["smdE1"], 2.0)) * Math.Exp(stale["smdH14"] * (odleglosc + stale["smdE1"])) * (1 + (stale["smdH11"] * Math.Pow(odleglosc + stale["smdE1"], 3.0) + stale["smdH12"] * Math.Pow(odleglosc + stale["smdE1"], 2) + stale["smdH13"] * (odleglosc + stale["smdE1"]) - 0.02));
            }
            else
            {
                returnValue = (stale["smdG11"] / Math.Pow(odleglosc + stale["smdE1"], 2.0)) * Math.Exp(stale["smdH14"] * (odleglosc + stale["smdE1"]));
            }

            return returnValue;
        }*
         */


        private double LiczWartoscOrazNiepewnoscDlaZrodlaOld(SortedDictionary<Double, Double> wzorcowe, double odleglosc)
        {
            KeyValuePair<double, double> point1 = wzorcowe.Last(pair => pair.Key <= odleglosc);

            double moc1 = point1.Value * Math.Pow(((point1.Key - 67) / (odleglosc - 67)), 2);

            KeyValuePair<double, double> point2 = wzorcowe.First(pair => pair.Key >= odleglosc);

            double moc2 = point2.Value * Math.Pow(((point2.Key - 67) / (odleglosc - 67)), 2);

            return (moc1 + moc2) / 2;
        }

        private void LiczWartoscOrazNiepewnoscDlaZrodla20230915(int zrodlo, SortedDictionary<Double, Double> wzorcowe, double odleglosc, out double wartosc_wzorcowa, out double niepewnosc)
        {
            double d = 65.96;
            double c = 0.00015;
            double A3 = 961500;
            double A2 = 14390;
            double A1 = 154.5;
            double mu = 0.000093415956075;
            double interpolacja = 40;

            switch (zrodlo)
            {
                case 1:
                    wartosc_wzorcowa = A1 / Math.Pow(odleglosc - d, 2) / Math.Exp(mu * (odleglosc - d)) + c;
                    break;
                case 2:
                    if (odleglosc < 600)
                    {
                        wartosc_wzorcowa = A2 / Math.Pow(odleglosc - d, 2) / Math.Exp(mu * (odleglosc - d)) + c;
                    }
                    else
                    {
                        wartosc_wzorcowa = A2 / Math.Pow(odleglosc - d, 2) / Math.Exp(mu * (odleglosc - d)) * Math.Pow(1 + interpolacja / (750 - odleglosc), 2) + c;
                    }
                    break;
                case 3:
                    if (odleglosc < 600)
                    {
                        wartosc_wzorcowa = A3 / Math.Pow(odleglosc - d, 2) / Math.Exp(mu * (odleglosc - d)) + c;
                    }
                    else
                    {
                        wartosc_wzorcowa = A3 / Math.Pow(odleglosc - d, 2) / Math.Exp(mu * (odleglosc - d)) * Math.Pow(1 + interpolacja / (750 - odleglosc), 2) + c;
                    }
                    break;
                default:
                    throw new ApplicationException("Nieznany nr źródła " + zrodlo);
            }

            var below = wzorcowe.First();
            var above = wzorcowe.Last();

            foreach (var item in wzorcowe)
            {
                if (item.Key > below.Key && item.Key < odleglosc) below = item;
                if (item.Key < above.Key && item.Key >= odleglosc) above = item;
            }

            niepewnosc = ((above.Key - odleglosc) * below.Value + (odleglosc - below.Key) * above.Value) / (above.Key - below.Key);
        }

        //---------------------------------------------------------------
        override public bool NadpiszDaneWzorcoweIPomiarowe()
        //---------------------------------------------------------------
        {
            _BazaDanych.Sygnalizacja
                .DELETE()
                .WHERE()
                    .ID_wzorcowania(Int32.Parse(_DaneOgolneDoZapisu.IdWzorcowania))
                .INFO("Czyszczenie przed nadpisaniem danych wzorcowych i pomiarowych.")
                .EXECUTE();

            if ("" != Pomiary.Uwagi)
            {
                _BazaDanych.Sygnalizacja
                    .INSERT()
                        .ID_wzorcowania(Int32.Parse(_DaneOgolneDoZapisu.IdWzorcowania))
                        .Prog(0)
                        .Niepewnosc(0)
                        .Wartosc_zmierzona(0)
                        .Uwagi(Pomiary.Uwagi)
                        .odleglosc1(0)
                        .odleglosc2(0)
                        .zrodlo1(0)
                        .zrodlo2(0)
                        .Wspolczynnik(0)
                        .Niepewnosc_Wspolczynnika(0)
                    .INFO("Nadpisywanie danych wzorcowych i pomiarowych z uwagami.")
                    .EXECUTE();
            }
            else
            {
                for (UInt16 i = 0; i < Pomiary.Dane.Count; ++i)
                {
                    _BazaDanych.Sygnalizacja
                        .INSERT()
                            .ID_wzorcowania(Int32.Parse(_DaneOgolneDoZapisu.IdWzorcowania))
                            .Prog(Pomiary.Dane[i].Prog)
                            .Niepewnosc(Pomiary.Dane[i].Niepewnosc)
                            .Wartosc_zmierzona(Pomiary.Dane[i].WartoscZmierzona)
                            .Uwagi("")
                            .odleglosc1(Pomiary.Dane[i].Odleglosc1)
                            .odleglosc2(Pomiary.Dane[i].Odleglosc2)
                            .zrodlo1(Pomiary.Dane[i].Zrodlo1)
                            .zrodlo2(Pomiary.Dane[i].Zrodlo2)
                            .Wspolczynnik(Pomiary.Dane[i].Wspolczynnik)
                            .Niepewnosc_Wspolczynnika(Pomiary.Dane[i].NiepewnoscWspolczynnika)
                        .INFO("Nadpisywanie danych wzorcowych i pomiarowych bez uwag.")
                        .EXECUTE();
                }

                try
                {
                    _Zapytanie = String.Format("SELECT id_protokolu FROM Protokoly_kalibracji_lawy WHERE data_kalibracji=#{0}#", Pomiary.Protokol);
                    short idProtokolu = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<short>(0);

                    _Zapytanie = String.Format("SELECT id_jednostki FROM Jednostki WHERE jednostka='{0}'", Pomiary.Jednostka);
                    int idJednostki = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<int>(0);

                    _Zapytanie = String.Format("UPDATE Wzorcowanie_cezem SET id_protokolu = {0}, id_jednostki = {1}, tlo = 0.0, wielkosc_fizyczna = 'nie dotyczy'  WHERE id_wzorcowania = {2}",
                                               idProtokolu, idJednostki, _DaneOgolneDoZapisu.IdWzorcowania);
                    _BazaDanych.WykonajPolecenie(_Zapytanie);
                }
                catch (Exception)
                {
                }
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
        public bool PobierzDaneWzorcoweIPomiaroweFormaTabelowa()
        //---------------------------------------------------------------
        {
            _Zapytanie = "SELECT prog, niepewnosc, wartosc_zmierzona, odleglosc1, odleglosc2, zrodlo1, zrodlo2, Wspolczynnik, Niepewnosc_Wspolczynnika "
                       + "FROM Sygnalizacja WHERE id_wzorcowania IN (SELECT id_wzorcowania FROM "
                       + String.Format("Wzorcowanie_cezem WHERE id_karty = {0} AND id_arkusza = {1})", IdKarty, IdArkusza);

            _OdpowiedzBazy = _BazaDanych.TworzTabeleDanych(_Zapytanie);

            if (null == _OdpowiedzBazy || null == _OdpowiedzBazy.Rows)
                return false;

            foreach (DataRow wiersz in _OdpowiedzBazy.Rows)
            {
                Pomiary.Dane.Add(
                                    new KlasyPomocniczeSygMocyDawki.DawkaWartosciWzorcowoPomiarowe.DawkaWartoscWzorcowoPomiarowa
                                    (
                                        wiersz.Field<double>("prog"),
                                        wiersz.Field<double>("niepewnosc"),
                                        wiersz.Field<double>("odleglosc1"),
                                        wiersz.Field<double>("odleglosc2"),
                                        wiersz.Field<double>("zrodlo1"),
                                        wiersz.Field<double>("zrodlo2"),
                                        wiersz.Field<double>("wartosc_zmierzona"),
                                        wiersz.Field<double>("Wspolczynnik"),
                                        wiersz.Field<double>("Niepewnosc_Wspolczynnika")
                                    )
                               );
            }

            return true;
        }

        //---------------------------------------------------------------
        public bool PobierzDaneWzorcoweIPomiaroweFormaUwag()
        //---------------------------------------------------------------
        {
            _Zapytanie = "SELECT Uwagi FROM Sygnalizacja WHERE id_wzorcowania IN (SELECT id_wzorcowania FROM "
                       + String.Format("Wzorcowanie_cezem WHERE id_karty = {0} AND id_arkusza = {1})", IdKarty, IdArkusza);

            _OdpowiedzBazy = _BazaDanych.TworzTabeleDanych(_Zapytanie);

            if (null == _OdpowiedzBazy || null == _OdpowiedzBazy.Rows || _OdpowiedzBazy.Rows.Count <= 0)
                return false;

            Pomiary.Uwagi = _OdpowiedzBazy.Rows[0].Field<string>(0);

            if ("" == Pomiary.Uwagi)
                return false;

            return true;
        }

        //---------------------------------------------------------------
        public bool PrzygotujDaneWzorcowoPomiaroweDoZapisu(ref System.Windows.Forms.DataGridView tabela, string protokol, string jednostka, string uwagi, bool zapisTabeli)
        //---------------------------------------------------------------
        {
            Pomiary.Dane.Clear();

            if (zapisTabeli)
            {
                return PrzygotujDaneWzorcowoPomiaroweDoZapisu_Tabela(ref tabela, protokol, jednostka);
            }
            else
            {
                return PrzygotujDaneWzorcowoPomiaroweDoZapisu_Uwagi(uwagi);
            }
        }

        //---------------------------------------------------------------
        public bool PrzygotujDaneWzorcowoPomiaroweDoZapisu_Tabela(ref System.Windows.Forms.DataGridView tabela, string protokol, string jednostka)
        //---------------------------------------------------------------
        {
            if ("" == jednostka || "" == protokol)
                return false;

            Pomiary.Jednostka = jednostka;
            Pomiary.Protokol = protokol;
            Pomiary.Uwagi = "";

            string sTemp;

            try
            {
                for (UInt16 i = 0; i < tabela.Rows.Count - 1 &&
                                   tabela.Rows[i].Cells[0].Value != null &&
                                   tabela.Rows[i].Cells[1].Value != null &&
                                   tabela.Rows[i].Cells[2].Value != null &&
                                   tabela.Rows[i].Cells[3].Value != null &&
                                   tabela.Rows[i].Cells[4].Value != null &&
                                   tabela.Rows[i].Cells[5].Value != null &&
                                   tabela.Rows[i].Cells[6].Value != null &&
                                   tabela.Rows[i].Cells[7].Value != null &&
                                   tabela.Rows[i].Cells[8].Value != null;
                                   ++i)
                {
                    KlasyPomocniczeSygMocyDawki.DawkaWartosciWzorcowoPomiarowe.DawkaWartoscWzorcowoPomiarowa temp
                    = new KlasyPomocniczeSygMocyDawki.DawkaWartosciWzorcowoPomiarowe.DawkaWartoscWzorcowoPomiarowa();

                    // próg
                    sTemp = tabela.Rows[i].Cells["Prog"].Value.ToString();

                    if (sTemp != "")
                        temp.Prog = N.doubleParse(sTemp);
                    else
                        temp.Prog = 0.0;

                    // Odległość 1
                    sTemp = tabela.Rows[i].Cells["Odl1"].Value.ToString();

                    if (sTemp != "")
                        temp.Odleglosc1 = N.doubleParse(sTemp);
                    else
                        temp.Odleglosc1 = 0.0;

                    // Odległość 2
                    sTemp = tabela.Rows[i].Cells["Odl2"].Value.ToString();

                    if (sTemp != "")
                        temp.Odleglosc2 = N.doubleParse(sTemp);
                    else
                        temp.Odleglosc2 = 0.0;

                    // Źródło 1
                    sTemp = tabela.Rows[i].Cells["Zr1"].Value.ToString();

                    if (sTemp != "")
                        temp.Zrodlo1 = N.doubleParse(sTemp);
                    else
                        temp.Zrodlo1 = 0.0;

                    // Odległość 2
                    sTemp = tabela.Rows[i].Cells["Zr2"].Value.ToString();

                    if (sTemp != "")
                        temp.Zrodlo2 = N.doubleParse(sTemp);
                    else
                        temp.Zrodlo2 = 0.0;

                    // niepewność
                    sTemp = tabela.Rows[i].Cells["Niepewnosc"].Value.ToString();

                    if (sTemp != "")
                        temp.Niepewnosc = N.doubleParse(sTemp);
                    else
                        temp.Niepewnosc = 0.0;

                    // wartość zmierzona
                    sTemp = tabela.Rows[i].Cells["Wartosc"].Value.ToString();

                    if (sTemp != "")
                        temp.WartoscZmierzona = N.doubleParse(sTemp);
                    else
                        temp.WartoscZmierzona = 0.0;

                    // Wspolczynnik
                    sTemp = tabela.Rows[i].Cells["Wspolczynnik"].Value.ToString();

                    if (sTemp != "")
                        temp.Wspolczynnik = N.doubleParse(sTemp);
                    else
                        temp.Wspolczynnik = 0.0;

                    // Niep. wspołczynnika
                    sTemp = tabela.Rows[i].Cells["NiepWsp"].Value.ToString();

                    if (sTemp != "")
                        temp.NiepewnoscWspolczynnika = N.doubleParse(sTemp);
                    else
                        temp.NiepewnoscWspolczynnika = 0.0;


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
        public bool PrzygotujDaneWzorcowoPomiaroweDoZapisu_Uwagi(string uwagi)
        //---------------------------------------------------------------
        {   
            if ("" != uwagi)
                Pomiary.Uwagi = uwagi;
            else
                Pomiary.Uwagi = "brak uwag";

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
            if ("" != Pomiary.Uwagi)
            {
                _Zapytanie = String.Format("INSERT INTO Sygnalizacja VALUES ({0}, 0.0, 0.0, 0.0, '{1}', 0.0, 0.0, 0.0, 0.0)",
                                    _DaneOgolneDoZapisu.IdWzorcowania, Pomiary.Uwagi);
                _BazaDanych.WykonajPolecenie(_Zapytanie);
            }
            else
            {
                for (int i = 0; i < Pomiary.Dane.Count; ++i)
                {
                    _Zapytanie = String.Format("INSERT INTO Sygnalizacja VALUES ({0}, '{1}', '{2}', '{3}', '', '{4}', '{5}', '{6}', '{7}')",
                                 _DaneOgolneDoZapisu.IdWzorcowania, Pomiary.Dane[i].Prog, Pomiary.Dane[i].Niepewnosc,
                                 Pomiary.Dane[i].WartoscZmierzona, Pomiary.Dane[i].Odleglosc1, Pomiary.Dane[i].Odleglosc2,
                                 Pomiary.Dane[i].Zrodlo1, Pomiary.Dane[i].Zrodlo2);
                    _BazaDanych.WykonajPolecenie(_Zapytanie);
                }

                try
                {
                    _Zapytanie = String.Format("SELECT id_protokolu FROM Protokoly_kalibracji_lawy WHERE data_kalibracji=#{0}#", Pomiary.Protokol);
                    short idProtokolu = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<short>(0);

                    _Zapytanie = String.Format("SELECT id_jednostki FROM Jednostki WHERE jednostka='{0}'", Pomiary.Jednostka);
                    int idJednostki = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<int>(0);

                    _Zapytanie = String.Format("UPDATE Wzorcowanie_cezem SET id_protokolu = {0}, id_jednostki = {1}, tlo = 0.0, wielkosc_fizyczna = 'nie dotyczy'  WHERE id_wzorcowania = {2}",
                                               idProtokolu, idJednostki, _DaneOgolneDoZapisu.IdWzorcowania);
                    _BazaDanych.WykonajPolecenie(_Zapytanie);
                }
                catch (Exception)
                {
                }
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
