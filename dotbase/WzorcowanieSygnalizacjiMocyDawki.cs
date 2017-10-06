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
            }
            public DateTime m_Data;
            public String m_Protokol;
            public String m_Jednostka;
            public List<UInt16> m_Zrodlo1;
            public List<UInt16> m_Zrodlo2;
            public List<Double> m_Odleglosc1;
            public List<Double> m_Odleglosc2;
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
        public Narzedzia.Pair<List<double>, List<double>> LiczWartoscOrazNiepewnosc(DaneWzorcowoPomiarowe daneWejsciowe)
        //---------------------------------------------------------------
        {
            List<Double> wartosci = new List<double>();
            List<Double> niepewnosci = new List<double>();


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
                double temp1 = LiczWartoscOrazNiepewnoscDlaZrodla(wzorcowe[daneWejsciowe.m_Zrodlo1[i] - 1], daneWejsciowe.m_Odleglosc1[i]);
                double temp2 = LiczWartoscOrazNiepewnoscDlaZrodla(wzorcowe[daneWejsciowe.m_Zrodlo2[i] - 1], daneWejsciowe.m_Odleglosc2[i]);
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
            }

            return new Narzedzia.Pair<List<double>, List<double>>(wartosci, niepewnosci);
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


        private double LiczWartoscOrazNiepewnoscDlaZrodla(SortedDictionary<Double, Double> wzorcowe, double odleglosc)
        {
            KeyValuePair<double, double> point1 = wzorcowe.Last(pair => pair.Key <= odleglosc);

            double moc1 = point1.Value * Math.Pow(((point1.Key - 67) / (odleglosc - 67)), 2);

            KeyValuePair<double, double> point2 = wzorcowe.First(pair => pair.Key >= odleglosc);

            double moc2 = point2.Value * Math.Pow(((point2.Key - 67) / (odleglosc - 67)), 2);

            return (moc1 + moc2) / 2;
        }

        //---------------------------------------------------------------
        override public bool NadpiszDaneWzorcoweIPomiarowe()
        //---------------------------------------------------------------
        {
            _Zapytanie = String.Format("DELETE FROM Sygnalizacja WHERE id_wzorcowania = {0}", _DaneOgolneDoZapisu.IdWzorcowania);
            _BazaDanych.WykonajPolecenie(_Zapytanie);

            if ("" != Pomiary.Uwagi)
            {
                _Zapytanie = String.Format("INSERT INTO Sygnalizacja VALUES ({0}, 0.0, 0.0, 0.0, '{1}', 0.0, 0.0, 0.0, 0.0)",
                                    _DaneOgolneDoZapisu.IdWzorcowania, Pomiary.Uwagi.Replace(@"\", @"\\"));
                _BazaDanych.WykonajPolecenie(_Zapytanie);
            }
            else
            {
                for (UInt16 i = 0; i < Pomiary.Dane.Count; ++i)
                {
                    _Zapytanie = String.Format("INSERT INTO Sygnalizacja VALUES ({0}, '{1}', '{2}', '{3}', '', '{4}', '{5}', '{6}', '{7}')",
                                 _DaneOgolneDoZapisu.IdWzorcowania, Pomiary.Dane[i].Prog, Pomiary.Dane[i].Niepewnosc,
                                 Pomiary.Dane[i].WartoscZmierzona, Pomiary.Dane[i].Odleglosc1, Pomiary.Dane[i].Odleglosc2,
                                 Pomiary.Dane[i].Zrodlo1, Pomiary.Dane[i].Zrodlo2);

                    _BazaDanych.WykonajPolecenie(_Zapytanie);
            

                    /*_Zapytanie = String.Format("UPDATE Sygnalizacja SET Prog='{0}', niepewnosc='{1}', ", Pomiary.Dane[i].Prog, Pomiary.Dane[i].Niepewnosc)
                               + String.Format("Wartosc_zmierzona='{0}', Uwagi='', odleglosc1={1}, odleglosc2='{2}', ", Pomiary.Dane[i].WartoscZmierzona,
                                 Pomiary.Dane[i].Odleglosc1, Pomiary.Dane[i].Odleglosc2)
                               + String.Format("zrodlo1='{0}', zrodlo2={1} WHERE id_wzorcowania={2}",
                                 Pomiary.Dane[i].Zrodlo1, Pomiary.Dane[i].Zrodlo2, _DaneOgolneDoZapisu.IdWzorcowania);*/
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
            _Zapytanie = "SELECT prog, niepewnosc, wartosc_zmierzona, odleglosc1, odleglosc2, zrodlo1, zrodlo2 "
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
                                        wiersz.Field<double>("wartosc_zmierzona")
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
                                   tabela.Rows[i].Cells[6].Value != null;
                                   ++i)
                {
                    KlasyPomocniczeSygMocyDawki.DawkaWartosciWzorcowoPomiarowe.DawkaWartoscWzorcowoPomiarowa temp
                    = new KlasyPomocniczeSygMocyDawki.DawkaWartosciWzorcowoPomiarowe.DawkaWartoscWzorcowoPomiarowa();

                    // próg
                    sTemp = tabela.Rows[i].Cells["Prog"].Value.ToString();

                    if (sTemp != "")
                        temp.Prog = double.Parse(sTemp);
                    else
                        temp.Prog = 0.0;

                    // Odległość 1
                    sTemp = tabela.Rows[i].Cells["Odl1"].Value.ToString();

                    if (sTemp != "")
                        temp.Odleglosc1 = double.Parse(sTemp);
                    else
                        temp.Odleglosc1 = 0.0;

                    // Odległość 2
                    sTemp = tabela.Rows[i].Cells["Odl2"].Value.ToString();

                    if (sTemp != "")
                        temp.Odleglosc2 = double.Parse(sTemp);
                    else
                        temp.Odleglosc2 = 0.0;

                    // Źródło 1
                    sTemp = tabela.Rows[i].Cells["Zr1"].Value.ToString();

                    if (sTemp != "")
                        temp.Zrodlo1 = double.Parse(sTemp);
                    else
                        temp.Zrodlo1 = 0.0;

                    // Odległość 2
                    sTemp = tabela.Rows[i].Cells["Zr2"].Value.ToString();

                    if (sTemp != "")
                        temp.Zrodlo2 = double.Parse(sTemp);
                    else
                        temp.Zrodlo2 = 0.0;

                    // niepewność
                    sTemp = tabela.Rows[i].Cells["Niepewnosc"].Value.ToString();

                    if (sTemp != "")
                        temp.Niepewnosc = double.Parse(sTemp);
                    else
                        temp.Niepewnosc = 0.0;

                    // wartość zmierzona
                    sTemp = tabela.Rows[i].Cells["Wartosc"].Value.ToString();

                    if (sTemp != "")
                        temp.WartoscZmierzona = double.Parse(sTemp);
                    else
                        temp.WartoscZmierzona = 0.0;


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
