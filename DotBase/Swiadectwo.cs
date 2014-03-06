using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;

namespace DotBase
{
    namespace Dokumenty
    {
        class Swiadectwo : Wydruki
        {
            StringBuilder _Tabela;
            StringBuilder _SzablonGlownyWzorcowania;
            StringBuilder _SzablonDrugiejStrony;

            enum stale
            {
                ADRES,
                CISNIENIE_MAX, CISNIENIE_MIN,
                DATA,
                EMISJA_POW,
                ID_WZORCOWANIA, INFORMACJA, INFORMACJA2, INFORMACJA3,
                JEDNOSTKA,
                METODA_WZORCOWANIA,
                NAPIECIE_ZAS_SONDY, NIEPEWNOSC, NAZWA, NR_FABRYCZNY, NR_KARTY,
                ODLEGLOSC_ZR_SONDA,
                PRODUCENT,
                ROK, ROK_PRODUKCJI,
                SKAZENIA_NIEPEWNOSC, SKAZENIA_RODZAJ_PROMIENIOWANIA, SKAZENIA_WSPOLCZYNNIK, SONDA_NR_FABRYCZNY, SONDA_TYP, SPRAWDZIL,
                TEMPERATURA_MIN, TEMPERATURA_MAX, TYP,
                UWAGI,
                WIELKOSC_FIZYCZNA, WILGOTNOSC_MAX, WILGOTNOSC_MIN, WSPOLCZYNNIK, WZ_DAWKI_ILE, WZ_MOC_DAWKI_ILE, WZ_SYG_DAWKI_ILE, WZ_ZR_POW_ILE, WZ_SYG_MOCY_DAWKI_ILE,
                ZAKRES, ZLECENIODAWCA, ZRODLO_CZAS_ROZPADU, ZRODLO_NAZWA,
                MAX_ELEMENTOW
            };

            enum staleZrodel { STRONT_SLABY = 2, WEGIEL_SLABY, AMERYK = 7, STRONT_SILNY, WEGIEL_SILNY, CHLOR, PLUTON = 17, STRONT_NAJSILNIEJSZY };

            enum szablon { DAWKA, MOC_DAWKI, SKAZENIA, SYG_DAWKI, SYG_MOCY_DAWKI };

            //********************************************************************************************
            public Swiadectwo(int nrKarty, DateTime data, String sprawdzil)
            //********************************************************************************************
            {
                InicjalizujListeDanychWypelniajacych((int)stale.MAX_ELEMENTOW);

                _BazaDanych = new BazaDanychWrapper();

                _DaneWypelniajace[(int)stale.NR_KARTY] = nrKarty.ToString();
                _DaneWypelniajace[(int)stale.DATA] = data.ToString("dd MMMM yyyy");
                _DaneWypelniajace[(int)stale.ROK] = data.Year.ToString();
                _DaneWypelniajace[(int)stale.SPRAWDZIL] = sprawdzil;
            }

            //********************************************************************************************
            private void PobierzDaneDawka(int ile, Char ch)
            //********************************************************************************************
            {
                if (false == SprawdzMozliwoscPobrania(ile, Narzedzia.StaleWzorcowan.stale.PLIK_POMOCNICZY_DAWKA))
                    return;

                _Zapytanie = String.Format("SELECT id_wzorcowania FROM Wzorcowanie_cezem WHERE id_karty = {0} ", _DaneWypelniajace[(int)stale.NR_KARTY])
                           + " AND rodzaj_wzorcowania='d' AND dolacz=true ORDER BY id_wzorcowania";

                PobierzIdWzorcowania(ile - 1);

                _Zapytanie = "SELECT typ, nr_fabryczny FROM Sondy WHERE id_sondy = (SELECT id_sondy FROM wzorcowanie_cezem WHERE "
                           + String.Format("id_wzorcowania=%d)", _DaneWypelniajace[(int)stale.ID_WZORCOWANIA]);

                PobierzDaneSondyDlaCezu();

                _DaneWypelniajace[(int)stale.JEDNOSTKA] = "mSv";

                PobierzDaneTabeloweDawka();

                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!c1>", ch.ToString());
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!sondaTyp>", _DaneWypelniajace[(int)stale.SONDA_TYP]);
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!sondaNrFab>", _DaneWypelniajace[(int)stale.SONDA_NR_FABRYCZNY]);
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!c5>", _DaneWypelniajace[(int)stale.JEDNOSTKA]);
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!tabela>", _Tabela.ToString());
            }

            //********************************************************************************************
            private bool PobierzDaneDrugaStrona()
            //********************************************************************************************
            {
                Char ch = 'a';
                _SzablonDrugiejStrony = new StringBuilder(200);

                try
                {
                    for (int i = int.Parse(_DaneWypelniajace[(int)stale.WZ_MOC_DAWKI_ILE]); i > 0; --i)
                    {
                        PobierzDaneMocDawki(i, ch);
                        _SzablonDrugiejStrony.Append(_SzablonPodstawowy);
                        ++ch;
                    }

                    for (int i = int.Parse(_DaneWypelniajace[(int)stale.WZ_DAWKI_ILE]); i > 0; --i)
                    {
                        PobierzDaneDawka(i, ch);
                        _SzablonDrugiejStrony.Append(_SzablonPodstawowy);
                        ++ch;
                    }

                    for (int i = int.Parse(_DaneWypelniajace[(int)stale.WZ_SYG_MOCY_DAWKI_ILE]); i > 0; --i)
                    {
                        PobierzDaneSygnalizacjaMocyDawki(i, ch);
                        _SzablonDrugiejStrony.Append(_SzablonPodstawowy);
                        ++ch;
                    }

                    for (int i = int.Parse(_DaneWypelniajace[(int)stale.WZ_SYG_DAWKI_ILE]); i > 0; --i)
                    {
                        PobierzDaneSygnalizacjaDawki(i, ch);
                        _SzablonDrugiejStrony.Append(_SzablonPodstawowy);
                        ++ch;
                    }

                    if (int.Parse(_DaneWypelniajace[(int)stale.WZ_ZR_POW_ILE]) > 0)
                    {
                        PobierzDaneSkazenia(1, ch);
                        _SzablonDrugiejStrony.Append(_SzablonPodstawowy);
                    }
                }
                catch (Exception)
                {
                    return false;
                }

                return true;
            }

            //********************************************************************************************
            private void PobierzDaneMocDawki(int ile, Char ch)
            //********************************************************************************************
            {
                if (ile <= 0 || false == WczytajSzablon(Narzedzia.StaleWzorcowan.stale.PLIK_POMOCNICZY_MOC_DAWKI))
                    return;

                _Zapytanie = String.Format("SELECT id_wzorcowania FROM Wzorcowanie_cezem WHERE id_karty = {0} AND rodzaj_wzorcowania = 'md' ",
               _DaneWypelniajace[(int)stale.NR_KARTY])
               + String.Format("AND dolacz = true ORDER BY id_wzorcowania");

                PobierzIdWzorcowania(ile - 1);

                _Zapytanie = "SELECT wielkosc_fizyczna FROM Wzorcowanie_cezem AS WC WHERE id_wzorcowania IN (SELECT id_wzorcowania FROM "
                           + String.Format("Wzorcowanie_cezem WHERE id_karty = {0} AND rodzaj_wzorcowania='md' AND dolacz=true) ORDER BY WC.id_wzorcowania",
                             _DaneWypelniajace[(int)stale.NR_KARTY]);

                PobierzWielkoscFizyczna();

                _Zapytanie = "SELECT typ, nr_fabryczny FROM Sondy WHERE id_sondy = (SELECT id_sondy FROM wzorcowanie_cezem WHERE "
                           + String.Format("id_wzorcowania = {0})", _DaneWypelniajace[(int)stale.ID_WZORCOWANIA]);

                PobierzDaneSondyDlaCezu();


                _Zapytanie = "SELECT jednostka FROM Jednostki WHERE id_jednostki=(SELECT id_jednostki FROM wzorcowanie_cezem WHERE "
                           + String.Format("id_wzorcowania = {0})", _DaneWypelniajace[(int)stale.ID_WZORCOWANIA]);

                PobierzJednostke();

                _Zapytanie = String.Format("SELECT zakres, wspolczynnik, niepewnosc FROM Wyniki_moc_dawki WHERE id_wzorcowania = {0}",
                           _DaneWypelniajace[(int)stale.ID_WZORCOWANIA]);

                PobierzDaneTabelowe();

                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!c1>", ch.ToString());
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!c2>", _DaneWypelniajace[(int)stale.WIELKOSC_FIZYCZNA]);
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!c3>", _DaneWypelniajace[(int)stale.SONDA_TYP]);
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!c4>", _DaneWypelniajace[(int)stale.SONDA_NR_FABRYCZNY]);
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!c5>", _DaneWypelniajace[(int)stale.JEDNOSTKA]);
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!c6>", _DaneWypelniajace[(int)stale.WIELKOSC_FIZYCZNA].Replace("moc", "mocy"));
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!tabela>", _Tabela.ToString());
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!wielkosc_fizyczna>", _DaneWypelniajace[(int)stale.WIELKOSC_FIZYCZNA].Replace("moc", "mocy"));

                if (_DaneWypelniajace[(int)stale.JEDNOSTKA].Contains("cps") || _DaneWypelniajace[(int)stale.JEDNOSTKA].Contains("cpm") ||
                    _DaneWypelniajace[(int)stale.JEDNOSTKA].Contains("1/s") || _DaneWypelniajace[(int)stale.JEDNOSTKA].Contains("1/min") ||
                    _DaneWypelniajace[(int)stale.JEDNOSTKA].Contains("imp/s") || _DaneWypelniajace[(int)stale.JEDNOSTKA].Contains("imp/min") ||
                    _DaneWypelniajace[(int)stale.JEDNOSTKA].Contains("s-1") || _DaneWypelniajace[(int)stale.JEDNOSTKA].Contains("Bq/cm2"))
                {
                    _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!korekcja_jednostki>", String.Format("(\u00B5Gy/h)/({0})", _DaneWypelniajace[(int)stale.JEDNOSTKA]));
                }
                else
                {
                    _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!korekcja_jednostki>", "-");
                }
            }

            //********************************************************************************************
            private bool PobierzDanePierwszaStrona()
            //********************************************************************************************
            {
                return PobierzDanePrzyrzadu() && PobierzDaneZleceniodawcy() && PobierzDaneWzorcowania();
            }

            //********************************************************************************************
            private bool PobierzDanePrzyrzadu()
            //********************************************************************************************
            {
                _Zapytanie = "SELECT nazwa, typ, nr_fabryczny, producent, rok_produkcji FROM Dozymetry WHERE id_dozymetru=(SELECT id_dozymetru "
                           + String.Format("FROM Karta_przyjecia WHERE id_karty = {0})", _DaneWypelniajace[(int)stale.NR_KARTY]);

                try
                {
                    DataRow wiersz = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0];

                    _DaneWypelniajace[(int)stale.NAZWA] = wiersz.Field<string>(0);
                    _DaneWypelniajace[(int)stale.TYP] = wiersz.Field<string>(1);
                    _DaneWypelniajace[(int)stale.NR_FABRYCZNY] = wiersz.Field<string>(2);
                    _DaneWypelniajace[(int)stale.PRODUCENT] = wiersz.Field<string>(3);
                    _DaneWypelniajace[(int)stale.ROK_PRODUKCJI] = wiersz.Field<string>(4);
                }
                catch (Exception)
                {
                    return false;
                }

                return true;
            }

            //********************************************************************************************
            private void PobierzDaneSkazenia(int ile, Char ch)
            //********************************************************************************************
            {
                string nrKarty = _DaneWypelniajace[(int)stale.NR_KARTY];

                if (false == SprawdzMozliwoscPobrania(ile, Narzedzia.StaleWzorcowan.stale.PLIK_POMOCNICZY_SKAZENIA))
                    return;

                _Tabela = new StringBuilder();

                _Zapytanie = String.Format("SELECT id_wzorcowania FROM Wzorcowanie_zrodlami_powierzchniowymi WHERE id_karty = {0} AND ", nrKarty)
                               + "dolacz=true ORDER BY id_wzorcowania";
                List<int> idWzorcowan = new List<int>();
                foreach (DataRow wiersz in _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows)
                {
                    idWzorcowan.Add(wiersz.Field<int>(0));
                }


                List<double> skazeniaWspolczynnik = new List<double>();
                List<double> skazeniaNiepewnosc = new List<double>();

                for (int i = 0; i < idWzorcowan.Count; ++i)
                {
                    _DaneWypelniajace[(int)stale.ID_WZORCOWANIA] = idWzorcowan[i].ToString();
                    string idWzorcowania = _DaneWypelniajace[(int)stale.ID_WZORCOWANIA];

                    PobierzDaneSondyDlaSkazen();

                    PobierzDaneSpecyficzneDlaSkazen(ref skazeniaWspolczynnik, ref skazeniaNiepewnosc);

                    PobierzDaneZrodla();

                    PobierzEmisjePowierzchniowa(idWzorcowan[i]);

                    _Tabela.Append(String.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td><td>{6}</td>",
                              i + 1, _DaneWypelniajace[(int)stale.SONDA_TYP], _DaneWypelniajace[(int)stale.SONDA_NR_FABRYCZNY],
                              _DaneWypelniajace[(int)stale.NAPIECIE_ZAS_SONDY], _DaneWypelniajace[(int)stale.ZRODLO_NAZWA].Replace("(słaby)", "").Replace("(silny)", ""),
                              _DaneWypelniajace[(int)stale.SKAZENIA_RODZAJ_PROMIENIOWANIA], _DaneWypelniajace[(int)stale.EMISJA_POW])
                              + String.Format("<td>{0}</td><td width=\"110\"><!wspol{1}> &plusmn; <!niep{1}></td></tr>",
                              _DaneWypelniajace[(int)stale.ODLEGLOSC_ZR_SONDA], i));
                }


                string format = "";

                for (int i = 0; i < skazeniaNiepewnosc.Count; ++i)
                {
                    string formatTemp = Narzedzia.Precyzja.Ustaw(skazeniaNiepewnosc[i]);
                    if (formatTemp.Length > format.Length)
                        format = formatTemp;
                }

                for (int i = 0; i < idWzorcowan.Count; ++i)
                {
                    string wspol = String.Format("<!wspol{0}>", i);
                    string niep = String.Format("<!niep{0}>", i);

                    _Tabela = _Tabela.Replace(wspol, skazeniaWspolczynnik[i].ToString(format));
                    _Tabela = _Tabela.Replace(niep, skazeniaNiepewnosc[i].ToString(format));
                }

                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!c1>", ch.ToString());
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!tabela>", _Tabela.ToString());
            }

            //********************************************************************************************
            private void PobierzEmisjePowierzchniowa(int idWzorcowania)
            //********************************************************************************************
            {
                _Zapytanie = String.Format("SELECT mnoznik_korekcyjny, data_wzorcowania FROM Wzorcowanie_zrodlami_powierzchniowymi WHERE id_wzorcowania = {0}", _DaneWypelniajace[(int)stale.ID_WZORCOWANIA]);
                DataRow wiersz = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0];

                double mnoznikKorekcyjny = wiersz.Field<double>(0);
                DateTime dataWzorcowaniaPrzyrzadu = wiersz.Field<DateTime>(1);

                _Zapytanie = "SELECT Emisja_powierzchniowa, data_wzorcowania FROM atesty_zrodel WHERE id_zrodla=(SELECT id_zrodla FROM "
                               + String.Format("Wzorcowanie_zrodlami_powierzchniowymi WHERE id_wzorcowania = {0} AND ", idWzorcowania)
                               + "id_atestu=(SELECT MAX(id_atestu) FROM atesty_zrodel WHERE id_zrodla=(SELECT id_zrodla FROM "
                               + String.Format("wzorcowanie_zrodlami_powierzchniowymi WHERE id_wzorcowania = {0})))", idWzorcowania);
                wiersz = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0];

                double emisja = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<double>(0);
                DateTime dataWzorcowaniaZrodla = wiersz.Field<DateTime>(1);
                double czasPolowicznegoRozpadu = double.Parse(_DaneWypelniajace[(int)stale.ZRODLO_CZAS_ROZPADU]);
                _DaneWypelniajace[(int)stale.EMISJA_POW] = (mnoznikKorekcyjny * emisja * Math.Exp(-Math.Log(2) / czasPolowicznegoRozpadu * (dataWzorcowaniaPrzyrzadu - dataWzorcowaniaZrodla).Days / 365.25)).ToString("0.00");
            }

            //********************************************************************************************
            private void PobierzDaneZrodla()
            //********************************************************************************************
            {
                _Zapytanie = "SELECT nazwa, czas_polowicznego_rozpadu FROM zrodla_powierzchniowe WHERE id_zrodla=(SELECT id_zrodla "
                               + String.Format("FROM Wzorcowanie_zrodlami_powierzchniowymi WHERE id_wzorcowania = {0})", _DaneWypelniajace[(int)stale.ID_WZORCOWANIA]);

                DataRow wiersz = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0];
                _DaneWypelniajace[(int)stale.ZRODLO_NAZWA] = wiersz.Field<string>(0);
                _DaneWypelniajace[(int)stale.ZRODLO_CZAS_ROZPADU] = wiersz.Field<float>(1).ToString();

                if (_DaneWypelniajace[(int)stale.ZRODLO_NAZWA] == "Am-241" || _DaneWypelniajace[(int)stale.ZRODLO_NAZWA] == "Pu-239")
                {
                    _DaneWypelniajace[(int)stale.SKAZENIA_RODZAJ_PROMIENIOWANIA] = "&alpha;";
                }
                else
                {
                    _DaneWypelniajace[(int)stale.SKAZENIA_RODZAJ_PROMIENIOWANIA] = "&beta;";
                }
            }

            //********************************************************************************************
            private void PobierzDaneSondyDlaSkazen()
            //********************************************************************************************
            {
                _Zapytanie = "SELECT typ, nr_fabryczny FROM Sondy WHERE id_sondy=(SELECT id_sondy FROM Wzorcowanie_zrodlami_powierzchniowymi "
                               + String.Format("WHERE id_wzorcowania = {0})", _DaneWypelniajace[(int)stale.ID_WZORCOWANIA]);

                DataRow wiersz = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0];
                _DaneWypelniajace[(int)stale.SONDA_TYP] = wiersz.Field<string>(0);
                _DaneWypelniajace[(int)stale.SONDA_NR_FABRYCZNY] = wiersz.Field<string>(1);
            }

            //********************************************************************************************
            private void PobierzDaneSondyDlaCezu()
            //********************************************************************************************
            {
                _Zapytanie = "SELECT typ, nr_fabryczny FROM Sondy WHERE id_sondy=(SELECT id_sondy FROM Wzorcowanie_cezem "
                               + String.Format("WHERE id_wzorcowania = {0})", _DaneWypelniajace[(int)stale.ID_WZORCOWANIA]);

                DataRow wiersz = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0];
                _DaneWypelniajace[(int)stale.SONDA_TYP] = wiersz.Field<string>(0);
                _DaneWypelniajace[(int)stale.SONDA_NR_FABRYCZNY] = wiersz.Field<string>(1);
            }

            //********************************************************************************************
            private void PobierzDaneSpecyficzneDlaSkazen(ref List<double> skazeniaWspolczynnik, ref List<double> skazeniaNiepewnosc)
            //********************************************************************************************
            {
                _Zapytanie = "SELECT napiecie_zasilania_sondy, odleglosc_zrodlo_sonda, wspolczynnik, niepewnosc FROM "
                           + String.Format("Wzorcowanie_zrodlami_powierzchniowymi WHERE id_wzorcowania = {0}", _DaneWypelniajace[(int)stale.ID_WZORCOWANIA]);

                DataRow wiersz = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0];
                _DaneWypelniajace[(int)stale.NAPIECIE_ZAS_SONDY] = wiersz.Field<string>(0);
                _DaneWypelniajace[(int)stale.ODLEGLOSC_ZR_SONDA] = wiersz.Field<double>(1).ToString();


                skazeniaWspolczynnik.Add(wiersz.Field<double>(2));
                skazeniaNiepewnosc.Add(wiersz.Field<double>(3));
                //_DaneWypelniajace[(int)stale.SKAZENIA_WSPOLCZYNNIK] = temp.ToString();
                //_DaneWypelniajace[(int)stale.SKAZENIA_NIEPEWNOSC] = temp.ToString();
            }

            //********************************************************************************************
            private void PobierzDaneSygnalizacjaDawki(int ile, Char ch)
            //********************************************************************************************
            {
                if (false == SprawdzMozliwoscPobrania(ile, Narzedzia.StaleWzorcowan.stale.PLIK_POMOCNICZY_SYGNALIZACJA_DAWKI))
                    return;

                _Zapytanie = String.Format("SELECT id_wzorcowania FROM Wzorcowanie_cezem WHERE id_karty = {0} ", _DaneWypelniajace[(int)stale.NR_KARTY])
                           + "AND rodzaj_wzorcowania = 'sd' AND dolacz = true ORDER BY id_wzorcowania";

                PobierzIdWzorcowania(ile - 1);

                _Zapytanie = "SELECT typ, nr_fabryczny FROM Sondy WHERE id_sondy = (SELECT id_sondy FROM wzorcowanie_cezem WHERE "
                           + String.Format("id_wzorcowania = {0})", _DaneWypelniajace[(int)stale.ID_WZORCOWANIA]);

                PobierzDaneSondyDlaCezu();

                _Zapytanie = "SELECT jednostka FROM Jednostki WHERE id_jednostki=(SELECT id_jednostki FROM wzorcowanie_cezem "
                           + String.Format("WHERE id_wzorcowania = {0})", _DaneWypelniajace[(int)stale.ID_WZORCOWANIA]);

                PobierzJednostke();

                _Zapytanie = "SELECT prog, wartosc_wzorcowa, wartosc_zmierzona FROM Sygnalizacja_dawka WHERE "
                           + String.Format("id_wzorcowania = {0}", _DaneWypelniajace[(int)stale.ID_WZORCOWANIA]);

                PobierzDaneTabeloweSygnalizacja();

                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!c1>", ch.ToString());
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!c2>", _DaneWypelniajace[(int)stale.SONDA_TYP]);
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!c3>", _DaneWypelniajace[(int)stale.SONDA_NR_FABRYCZNY]);
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!c4>", _DaneWypelniajace[(int)stale.JEDNOSTKA]);
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!tabela>", _Tabela.ToString());
            }

            //********************************************************************************************
            private void PobierzDaneSygnalizacjaMocyDawki(int ile, Char ch)
            //*******************************************************************************************
            {
                if (true == SprawdzCzyIstniejaUwagi())
                {
                    if (false == SprawdzMozliwoscPobrania(ile, Narzedzia.StaleWzorcowan.stale.PLIK_POMOCNICZY_SYGNALIZACJA_MOCY_DAWKI_OPIS))
                        return;

                    _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!c1>", ch.ToString());
                    _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!opis>", _DaneWypelniajace[(int)stale.UWAGI]);
                    return;
                }

                if (false == SprawdzMozliwoscPobrania(ile, Narzedzia.StaleWzorcowan.stale.PLIK_POMOCNICZY_SYGNALIZACJA_MOCY_DAWKI))
                    return;

                _Zapytanie = String.Format("SELECT id_wzorcowania FROM Wzorcowanie_cezem WHERE id_karty = {0} AND ", _DaneWypelniajace[(int)stale.NR_KARTY])
                           + "rodzaj_wzorcowania='sm' AND dolacz=true ORDER BY id_wzorcowania";

                PobierzIdWzorcowania(ile - 1);

                _Zapytanie = "SELECT typ, nr_fabryczny FROM Sondy WHERE id_sondy=(SELECT id_sondy FROM wzorcowanie_cezem WHERE "
                           + String.Format("id_wzorcowania = {0})", _DaneWypelniajace[(int)stale.ID_WZORCOWANIA]);
                PobierzDaneSondyDlaCezu();


                _Zapytanie = "SELECT jednostka FROM Jednostki WHERE id_jednostki=(SELECT id_jednostki FROM wzorcowanie_cezem WHERE "
                           + String.Format("id_wzorcowania = {0})", _DaneWypelniajace[(int)stale.ID_WZORCOWANIA]);
                PobierzJednostke();

                _Zapytanie = String.Format("SELECT prog, wartosc_zmierzona, niepewnosc FROM Sygnalizacja WHERE id_wzorcowania = {0}",
                                          _DaneWypelniajace[(int)stale.ID_WZORCOWANIA]);
                PobierzDaneTabeloweSygMocyDawki();

                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!c1>", ch.ToString());
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!c2>", _DaneWypelniajace[(int)stale.SONDA_TYP]);
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!c3>", _DaneWypelniajace[(int)stale.SONDA_NR_FABRYCZNY]);
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!c4>", _DaneWypelniajace[(int)stale.JEDNOSTKA]);
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!tabela>", _Tabela.ToString());
            }

            //********************************************************************************************
            private bool SprawdzCzyIstniejaUwagi()
            //********************************************************************************************
            {
                _Zapytanie = "SELECT Uwagi FROM Sygnalizacja WHERE id_wzorcowania IN (SELECT id_wzorcowania FROM wzorcowanie_cezem "
                           + String.Format("WHERE id_karty = {0})", _DaneWypelniajace[(int)stale.NR_KARTY]);

                try
                {
                    _DaneWypelniajace[(int)stale.UWAGI] = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<string>(0);
                }
                catch (Exception)
                {
                    return false;
                }

                if (_DaneWypelniajace[(int)stale.UWAGI] == "")
                    return false;
                else
                    return true;
            }

            //********************************************************************************************
            private void PobierzDaneTabeloweDawka()
            //********************************************************************************************
            {
                _Zapytanie = String.Format("SELECT zakres, wspolczynnik, niepewnosc, wielkosc_fizyczna FROM Wyniki_dawka WHERE id_wzorcowania = {0}",
                                          _DaneWypelniajace[(int)stale.ID_WZORCOWANIA]);

                List<double> zakres = new List<double>();
                List<double> wspolczynnik = new List<double>();
                List<double> niepewnosc = new List<double>();
                List<int> wielkosc_fizyczna = new List<int>();

                foreach (DataRow wiersz in _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows)
                {
                    zakres.Add(wiersz.Field<double>(0));
                    wspolczynnik.Add(wiersz.Field<double>(1));
                    niepewnosc.Add(wiersz.Field<double>(2));
                    wielkosc_fizyczna.Add(wiersz.Field<int>(3));
                }

                _Tabela = new StringBuilder();

                for (int i = 0; i < zakres.Count; ++i)
                {
                    _Tabela.AppendFormat("<tr><td>{0}</td><td>{1} &plusmn; {2}</td></tr>", zakres[i].ToString("G"), String.Format("{0:0.000}", wspolczynnik[i]), String.Format("{0:0.000}", niepewnosc[i]));
                }

                if (wielkosc_fizyczna.Count == 0)
                    return;

                if (wielkosc_fizyczna[0] == 0)
                {
                    _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!wielkosc>", "indywidualny równoważnik dawki Hp(10)");
                    _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!wielkosc2>", "indywidualnego równoważnika dawki Hp(10)");
                }
                else if (wielkosc_fizyczna[0] == 1)
                {
                    _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!wielkosc>", "indywidualny równoważnik dawki Hp(0,07)");
                    _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!wielkosc2>", "indywidualnego równoważnika dawki Hp(0,07)");
                }
                else
                {
                    _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!wielkosc>", "przestrzenny równoważnik dawki");
                    _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!wielkosc2>", "przestrzennego równoważnika dawki");
                }

            }

            //********************************************************************************************
            private void PobierzDaneTabeloweSygnalizacja()
            //********************************************************************************************
            {
                List<double> prog = new List<double>();
                List<double> wartosc_wzorcowa = new List<double>();
                List<double> wartosc_zmierzona = new List<double>();

                foreach (DataRow wiersz in _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows)
                {
                    prog.Add(wiersz.Field<double>(0));
                    wartosc_wzorcowa.Add(wiersz.Field<double>(1));
                    wartosc_zmierzona.Add(wiersz.Field<double>(2));
                }

                _Tabela = new StringBuilder();

                for (int i = 0; i < prog.Count; ++i)
                {
                    _Tabela.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>", prog[i].ToString("G"), wartosc_wzorcowa[i].ToString("G"), wartosc_zmierzona[i].ToString("G"));
                }
            }

            //********************************************************************************************
            private void PobierzDaneTabeloweSygMocyDawki()
            //********************************************************************************************
            {
                List<double> prog = new List<double>();
                List<double> wartosc_zmierzona = new List<double>();
                List<double> niepewnosc = new List<double>();

                foreach (DataRow wiersz in _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows)
                {
                    prog.Add(wiersz.Field<double>(0));
                    wartosc_zmierzona.Add(wiersz.Field<double>(1));
                    niepewnosc.Add(wiersz.Field<double>(2));
                }

                _Tabela = new StringBuilder();

                for (int i = 0; i < prog.Count; ++i)
                {
                    _Tabela.AppendFormat("<tr><td>{0}</td><td>{1} &plusmn; {2}</td></tr>", prog[i].ToString("G"), wartosc_zmierzona[i].ToString("G"), niepewnosc[i].ToString("G"));
                }
            }

            //********************************************************************************************
            private void PobierzDaneTabelowe()
            //********************************************************************************************
            {
                List<double> zakres = new List<double>();
                List<double> wspolczynnik = new List<double>();
                List<double> niepewnosc = new List<double>();

                bool wiecej_niz_jeden = false;

                foreach (DataRow wiersz in _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows)
                {
                    zakres.Add(wiersz.Field<double>(0));
                    wspolczynnik.Add(wiersz.Field<double>(1));
                    niepewnosc.Add(wiersz.Field<double>(2));
                }

                for (int i = 0; i < wspolczynnik.Count; ++i)
                {
                    if (wspolczynnik[i] >= 0.1)
                    {
                        wiecej_niz_jeden = true;
                        break;
                    }
                }

                _Tabela = new StringBuilder();

                if (wiecej_niz_jeden)
                {
                    for (int i = 0; i < zakres.Count; ++i)
                    {
                        _Tabela.AppendFormat("<tr><td>{0}</td><td>{1} &plusmn; {2}</td></tr>", zakres[i].ToString("G"), String.Format("{0:0.00}", wspolczynnik[i]), String.Format("{0:0.00}", niepewnosc[i]));
                    }
                }
                else
                {
                    for (int i = 0; i < zakres.Count; ++i)
                    {
                        _Tabela.AppendFormat("<tr><td>{0}</td><td>{1} &plusmn; {2}</td></tr>", zakres[i].ToString("G"), wspolczynnik[i].ToString("G"), niepewnosc[i].ToString("G"));
                    }
                }
            }

            //********************************************************************************************
            private bool PobierzDaneWzorcowania()
            //********************************************************************************************
            {
                try
                {
                    ZliczLiczbeWystapien();

                    bool bCez = false;
                    bool bDawka = false;
                    bool bSkazenia = false;

                    _Zapytanie = "Select Cisnienie, Temperatura, Wilgotnosc From ";

                    if (int.Parse(_DaneWypelniajace[(int)stale.WZ_MOC_DAWKI_ILE]) > 0)
                    {
                        _Zapytanie += String.Format("wzorcowanie_cezem where id_karty = {0} AND rodzaj_wzorcowania='md' AND dolacz=true", _DaneWypelniajace[(int)stale.NR_KARTY]);
                        bCez = true;
                    }
                    else if (int.Parse(_DaneWypelniajace[(int)stale.WZ_DAWKI_ILE]) > 0)
                    {
                        _Zapytanie += String.Format("wzorcowanie_cezem where id_karty = {0} AND rodzaj_wzorcowania='d' AND dolacz=true", _DaneWypelniajace[(int)stale.NR_KARTY]);
                        bDawka = true;
                    }
                    else if (int.Parse(_DaneWypelniajace[(int)stale.WZ_SYG_MOCY_DAWKI_ILE]) > 0)
                    {
                        _Zapytanie += String.Format("wzorcowanie_cezem where id_karty = {0} AND rodzaj_wzorcowania='sm' AND dolacz=true", _DaneWypelniajace[(int)stale.NR_KARTY]);
                        bCez = true;
                    }
                    else if (int.Parse(_DaneWypelniajace[(int)stale.WZ_SYG_DAWKI_ILE]) > 0)
                    {
                        _Zapytanie += String.Format("wzorcowanie_cezem where id_karty = {0} AND rodzaj_wzorcowania='sd' AND dolacz=true", _DaneWypelniajace[(int)stale.NR_KARTY]);
                        bCez = true;
                    }
                    else if (int.Parse(_DaneWypelniajace[(int)stale.WZ_ZR_POW_ILE]) > 0)
                    {
                        _Zapytanie += String.Format("wzorcowanie_zrodlami_powierzchniowymi where id_karty = {0} AND dolacz=true", _DaneWypelniajace[(int)stale.NR_KARTY]);
                        bSkazenia = true;
                    }

                    if (int.Parse(_DaneWypelniajace[(int)stale.WZ_ZR_POW_ILE]) > 0)
                    {
                        bSkazenia = true;
                    }
                    if (int.Parse(_DaneWypelniajace[(int)stale.WZ_DAWKI_ILE]) > 0)
                    {
                        bDawka = true;
                    }

                    DataRow odpowiedzBazy = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0];

                    double temp;

                    temp = odpowiedzBazy.Field<double>(0);
                    _DaneWypelniajace[(int)stale.CISNIENIE_MIN] = (temp - 0.5).ToString("0.0");
                    _DaneWypelniajace[(int)stale.CISNIENIE_MAX] = (temp + 0.5).ToString("0.0");
                    temp = odpowiedzBazy.Field<double>(1);
                    _DaneWypelniajace[(int)stale.TEMPERATURA_MIN] = (temp - 0.5).ToString("0.0");
                    _DaneWypelniajace[(int)stale.TEMPERATURA_MAX] = (temp + 0.5).ToString("0.0");
                    temp = odpowiedzBazy.Field<double>(2);
                    _DaneWypelniajace[(int)stale.WILGOTNOSC_MIN] = (temp - 0.5).ToString("0.0");
                    _DaneWypelniajace[(int)stale.WILGOTNOSC_MAX] = (temp + 0.5).ToString("0.0");

                    if (bDawka && bCez && bSkazenia)
                    {
                        _DaneWypelniajace[(int)stale.METODA_WZORCOWANIA] = "Procedura wzorcowania mierników mocy dawki promieniowaniem gamma ze źródła Cs-137 (WZOR-1<br>wyd. 3 z dn. 10.06.13) oraz procedura wzorcowania dawkomierzy promieniowania jonizującego (DAWKA<br>wyd. 3 z dn. 13.06.13) oraz procedura wzorcowania mierników skażeń powierzchni przy pomocy źródeł<br>powierzchniowych (WZOR-2 wyd. 3 z dn. 10.06.13)";
                        _DaneWypelniajace[(int)stale.INFORMACJA] =
                        "Wyniki wzorcowania zostały odniesione do państwowego wzorca pomiarowego mocy kermy promieniowania gamma " +
                        "w powietrzu utrzymywanego w GUM poprzez zastosowanie dawkomierza kontrolnego UNIDOS typ-ser. 10001-10551 z komorą jonizacyjną 30 cm<sup>3</sup> typ-ser. M23361-0337.<br><br>";
                    }
                    else if (bDawka && bSkazenia)
                    {
                        _DaneWypelniajace[(int)stale.METODA_WZORCOWANIA] = "Procedura wzorcowania dawkomierzy promieniowania jonizującego (DAWKA wyd. 3 z dn. 13.06.13) oraz procedura wzorcowania mierników skażeń powierzchni przy pomocy źródeł powierzchniowych (WZOR-2<br>wyd. 3 z dn. 13.06.13) ";
                        _DaneWypelniajace[(int)stale.INFORMACJA] =
                        "Wyniki wzorcowania zostały odniesione do państwowego wzorca pomiarowego mocy kermy promieniowania gamma " +
                        "w powietrzu utrzymywanego w GUM poprzez zastosowanie dawkomierza kontrolnego UNIDOS typ-ser. 10001-10551 z komorą jonizacyjną 30 cm<sup>3</sup> typ-ser. M23361-0337.<br><br>";
                    }
                    else if (bDawka && bCez)
                    {
                        _DaneWypelniajace[(int)stale.METODA_WZORCOWANIA] = "Procedura wzorcowania mierników mocy dawki promieniowaniem gamma ze źródła Cs-137 (WZOR-1<br>wyd. 3 z dn. 10.06.13) oraz procedura wzorcowania dawkomierzy promieniowania jonizującego (DAWKA<br>wyd. 3 z dn. 13.06.13)";
                        _DaneWypelniajace[(int)stale.INFORMACJA] =
                        "Wyniki wzorcowania zostały odniesione do państwowego wzorca pomiarowego mocy kermy promieniowania gamma " +
                        "w powietrzu utrzymywanego w GUM poprzez zastosowanie dawkomierza kontrolnego UNIDOS typ-ser. 10001-10551 z komorą jonizacyjną 30 cm<sup>3</sup> typ-ser. M23361-0337.<br><br>";
                    }
                    else if (bDawka)
                    {
                        _DaneWypelniajace[(int)stale.METODA_WZORCOWANIA] = "Procedura wzorcowania dawkomierzy promieniowania jonizującego (DAWKA<br>wyd. 3 z dn. 13.06.13)";
                        _DaneWypelniajace[(int)stale.INFORMACJA] =
                        "Wyniki wzorcowania zostały odniesione do państwowego wzorca pomiarowego mocy kermy promieniowania gamma " +
                        "w powietrzu utrzymywanego w GUM poprzez zastosowanie dawkomierza kontrolnego UNIDOS typ-ser. 10001-10551 z komorą jonizacyjną 30 cm<sup>3</sup> typ-ser. M23361-0337.<br><br>";
                    }
                    else if (bCez && bSkazenia)
                    {
                        _DaneWypelniajace[(int)stale.METODA_WZORCOWANIA] = "Procedura wzorcowania mierników mocy dawki promieniowaniem gamma ze źródła Cs-137 (WZOR-1<br>wyd. 3 z dn. 10.06.13) oraz procedura wzorcowania mierników skażeń powierzchni przy pomocy źródeł<br>powierzchniowych (WZOR-2 wyd. 3 z dn. 10.06.13)";
                        _DaneWypelniajace[(int)stale.INFORMACJA] =
                        "Wyniki wzorcowania zostały odniesione do państwowego wzorca pomiarowego mocy kermy promieniowania gamma " +
                        "w powietrzu utrzymywanego w GUM poprzez zastosowanie dawkomierza kontrolnego UNIDOS typ-ser. 10001-10551 z komorą jonizacyjną 30 cm<sup>3</sup> typ-ser. M23361-0337.<br><br>";
                    }
                    else if (bCez && !bSkazenia)
                    {
                        _DaneWypelniajace[(int)stale.METODA_WZORCOWANIA] = "Procedura wzorcowania mierników mocy dawki promieniowaniem gamma ze źródła Cs-137 (WZOR-1<br>wyd. 3 z dn. 10.06.13)";
                        _DaneWypelniajace[(int)stale.INFORMACJA] =
                        "Wyniki wzorcowania zostały odniesione do państwowego wzorca pomiarowego mocy kermy promieniowania gamma " +
                         "w powietrzu utrzymywanego w GUM poprzez zastosowanie dawkomierza kontrolnego UNIDOS typ-ser. 10001-10551 z komorą jonizacyjną 30 cm<sup>3</sup> typ-ser. M23361-0337.<br><br>";
                    }
                    else
                    {
                        _DaneWypelniajace[(int)stale.METODA_WZORCOWANIA] = "Procedura wzorcowania mierników skażeń powierzchni przy pomocy źródeł powierzchniowych (WZOR-2<br>wyd. 3 z dn. 10.06.13)";
                        _DaneWypelniajace[(int)stale.INFORMACJA] = "";
                    }

                    if (int.Parse(_DaneWypelniajace[(int)stale.WZ_ZR_POW_ILE]) > 0)
                    {
                        _DaneWypelniajace[(int)stale.INFORMACJA2] = "Wyniki wzorcowania w zakresie emisji powierzchniowej zostały odniesione do wzorca pomiarowego utrzymywanego w ";

                        bool polon = false;
                        bool bruschweig = false;

                        _Zapytanie = String.Format("SELECT id_zrodla FROM Wzorcowanie_zrodlami_powierzchniowymi WHERE id_karty = {0}", _DaneWypelniajace[(int)stale.NR_KARTY]);

                        foreach (DataRow row in _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows)
                        {
                            int idZrodla = row.Field<int>(0);

                            if ((int)staleZrodel.STRONT_SLABY == idZrodla || (int)staleZrodel.STRONT_SILNY == idZrodla || (int)staleZrodel.STRONT_NAJSILNIEJSZY == idZrodla || (int)staleZrodel.AMERYK == idZrodla)
                            {
                                bruschweig = true;
                            }
                            else if ((int)staleZrodel.WEGIEL_SLABY == idZrodla || (int)staleZrodel.CHLOR == idZrodla || (int)staleZrodel.PLUTON == idZrodla || (int)staleZrodel.WEGIEL_SILNY == idZrodla)
                            {
                                polon = true;
                            }
                        }

                        if (polon && bruschweig)
                        {
                            _DaneWypelniajace[(int)stale.INFORMACJA3] = "Laboratorium Wzorców Radioaktywności OBRI POLATOM i PTB Braunschweig, poprzez zastosowanie wzorcowego źródła powierzchniowego.";
                        }
                        else if (polon)
                        {
                            _DaneWypelniajace[(int)stale.INFORMACJA3] = "Laboratorium Wzorców Radioaktywności OBRI POLATOM, poprzez zastosowanie wzorcowego źródła powierzchniowego.";
                        }
                        else
                        {
                            _DaneWypelniajace[(int)stale.INFORMACJA3] = "PTB Braunschweig, poprzez zastosowanie wzorcowego źródła powierzchniowego.";
                        }
                    }
                    else
                    {
                        _DaneWypelniajace[(int)stale.INFORMACJA2] = "";
                        _DaneWypelniajace[(int)stale.INFORMACJA3] = "";
                    }
                }
                catch (Exception)
                {
                    return false;
                }

                return true;
            }

            //********************************************************************************************
            private bool PobierzDaneZleceniodawcy()
            //********************************************************************************************
            {
                _Zapytanie = "SELECT Zleceniodawca, adres FROM Zleceniodawca WHERE id_zleceniodawcy = (SELECT id_zleceniodawcy FROM Zlecenia WHERE id_zlecenia = (SELECT id_zlecenia FROM "
                           + String.Format("Karta_przyjecia WHERE id_karty = {0}))", _DaneWypelniajace[(int)stale.NR_KARTY]);

                try
                {
                    DataRow wiersz = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0];
                    _DaneWypelniajace[(int)stale.ZLECENIODAWCA] = wiersz.Field<string>(0);
                    _DaneWypelniajace[(int)stale.ADRES] = wiersz.Field<string>(1);
                }
                catch (Exception)
                {
                    _DaneWypelniajace[(int)stale.ZLECENIODAWCA] = "";
                    _DaneWypelniajace[(int)stale.ADRES] = "";
                    return false;
                }

                return true;
            }

            //********************************************************************************************
            private void PobierzJednostke()
            //********************************************************************************************
            {
                _DaneWypelniajace[(int)stale.JEDNOSTKA] = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<string>(0);
                _DaneWypelniajace[(int)stale.JEDNOSTKA] = _DaneWypelniajace[(int)stale.JEDNOSTKA].Replace("u", "&mu;");
            }

            //********************************************************************************************
            private void PobierzWielkoscFizyczna()
            //********************************************************************************************
            {
                _DaneWypelniajace[(int)stale.WIELKOSC_FIZYCZNA] = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<string>(0);
            }

            //********************************************************************************************
            private void PobierzIdWzorcowania(int ktoreWzorcowaniePobrac)
            //********************************************************************************************
            {
                _DaneWypelniajace[(int)stale.ID_WZORCOWANIA] = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[ktoreWzorcowaniePobrac].Field<int>(0).ToString();
            }

            //********************************************************************************************
            private bool SprawdzMozliwoscPobrania(int ilePobran, Narzedzia.StaleWzorcowan.stale typSzablonu)
            //********************************************************************************************
            {
                return 0 < ilePobran && true == WczytajSzablon(typSzablonu);
            }

            //********************************************************************************************
            private bool UtworzPierwszaStrone()
            //********************************************************************************************
            {
                if (false == PobierzSzablonGlowny())
                    return false;

                _SzablonGlownyWzorcowania = _SzablonGlownyWzorcowania.Replace("<!c1>", _DaneWypelniajace[(int)stale.DATA]);
                _SzablonGlownyWzorcowania = _SzablonGlownyWzorcowania.Replace("<!c2>", _DaneWypelniajace[(int)stale.NR_KARTY]);
                _SzablonGlownyWzorcowania = _SzablonGlownyWzorcowania.Replace("<!c3>", _DaneWypelniajace[(int)stale.ROK]);
                _SzablonGlownyWzorcowania = _SzablonGlownyWzorcowania.Replace("<!c4>", _DaneWypelniajace[(int)stale.NAZWA]);
                _SzablonGlownyWzorcowania = _SzablonGlownyWzorcowania.Replace("<!c5>", _DaneWypelniajace[(int)stale.TYP]);
                _SzablonGlownyWzorcowania = _SzablonGlownyWzorcowania.Replace("<!c6>", _DaneWypelniajace[(int)stale.NR_FABRYCZNY]);
                _SzablonGlownyWzorcowania = _SzablonGlownyWzorcowania.Replace("<!c7>", _DaneWypelniajace[(int)stale.PRODUCENT]);
                _SzablonGlownyWzorcowania = _SzablonGlownyWzorcowania.Replace("<!c8>", _DaneWypelniajace[(int)stale.ROK_PRODUKCJI]);
                _SzablonGlownyWzorcowania = _SzablonGlownyWzorcowania.Replace("<!c9>", _DaneWypelniajace[(int)stale.ZLECENIODAWCA]);
                _SzablonGlownyWzorcowania = _SzablonGlownyWzorcowania.Replace("<!c10>", _DaneWypelniajace[(int)stale.ADRES]);
                _SzablonGlownyWzorcowania = _SzablonGlownyWzorcowania.Replace("<!c11>", _DaneWypelniajace[(int)stale.CISNIENIE_MIN]);
                _SzablonGlownyWzorcowania = _SzablonGlownyWzorcowania.Replace("<!c12>", _DaneWypelniajace[(int)stale.CISNIENIE_MAX]);
                _SzablonGlownyWzorcowania = _SzablonGlownyWzorcowania.Replace("<!c13>", _DaneWypelniajace[(int)stale.TEMPERATURA_MIN]);
                _SzablonGlownyWzorcowania = _SzablonGlownyWzorcowania.Replace("<!c14>", _DaneWypelniajace[(int)stale.TEMPERATURA_MAX]);
                _SzablonGlownyWzorcowania = _SzablonGlownyWzorcowania.Replace("<!c15>", _DaneWypelniajace[(int)stale.WILGOTNOSC_MIN]);
                _SzablonGlownyWzorcowania = _SzablonGlownyWzorcowania.Replace("<!c16>", _DaneWypelniajace[(int)stale.WILGOTNOSC_MAX]);
                _SzablonGlownyWzorcowania = _SzablonGlownyWzorcowania.Replace("<!c17>", _DaneWypelniajace[(int)stale.INFORMACJA] + _DaneWypelniajace[(int)stale.INFORMACJA2] + _DaneWypelniajace[(int)stale.INFORMACJA3]);
                _SzablonGlownyWzorcowania = _SzablonGlownyWzorcowania.Replace("<!metoda_wzorcowania>", _DaneWypelniajace[(int)stale.METODA_WZORCOWANIA]);

                DateTime dataWzorcowaniaCez;
                try
                {
                    _Zapytanie = String.Format("SELECT MAX(data_wzorcowania) FROM wzorcowanie_cezem WHERE id_karty = {0} ", _DaneWypelniajace[(int)stale.NR_KARTY]);
                    dataWzorcowaniaCez = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<DateTime>(0);
                }
                catch (Exception)
                {
                    dataWzorcowaniaCez = DateTime.MinValue;
                }

                DateTime dataWzorcowaniaSkazenia;
                try
                {
                    _Zapytanie = String.Format("SELECT MAX(data_wzorcowania) FROM wzorcowanie_zrodlami_powierzchniowymi WHERE id_karty = {0} ", _DaneWypelniajace[(int)stale.NR_KARTY]);
                    dataWzorcowaniaSkazenia = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<DateTime>(0);
                }
                catch (Exception)
                {
                    dataWzorcowaniaSkazenia = DateTime.MinValue;
                }

                DateTime maxData = dataWzorcowaniaCez > dataWzorcowaniaSkazenia ? dataWzorcowaniaCez : dataWzorcowaniaSkazenia;

                _SzablonGlownyWzorcowania = _SzablonGlownyWzorcowania.Replace("<!dataWykonania>", maxData.ToString("d MMMM yyyy"));

                return true;
            }

            //********************************************************************************************
            private bool PobierzSzablonGlowny()
            //********************************************************************************************
            {
                return WczytajSzablon(Narzedzia.StaleWzorcowan.stale.SWIADECTWO, ref _SzablonGlownyWzorcowania);
            }

            //********************************************************************************************
            private bool UtworzDrugaStrone()
            //********************************************************************************************
            {
                _SzablonGlownyWzorcowania = _SzablonGlownyWzorcowania.Replace("<str2>", _SzablonDrugiejStrony.ToString());
                _SzablonGlownyWzorcowania = _SzablonGlownyWzorcowania.Replace("<!sprawdzil>", _DaneWypelniajace[(int)stale.SPRAWDZIL]);
                return true;
            }

            //********************************************************************************************
            override public bool UtworzDokument(string sciezka)
            //********************************************************************************************
            {
                return PobierzDanePierwszaStrona() &&
                       UtworzPierwszaStrone() &&
                       PobierzDaneDrugaStrona() &&
                       UtworzDrugaStrone() &&
                       ZapiszPlikWynikowy(sciezka);
            }

            //********************************************************************************************
            override protected bool ZapiszPlikWynikowy(string sciezka)
            //********************************************************************************************
            {
                _Fout = new StreamWriter(sciezka);
                _Fout.Write(_SzablonGlownyWzorcowania.ToString());
                _Fout.Close();
                return true;
            }

            //********************************************************************************************
            // w bazie danych dot. ilości typów poszczególnych wzorcowań
            private void ZliczLiczbeWystapien()
            //********************************************************************************************
            {
                // zliczenie liczby wpisów dot. źródeł powierzchniowych
                _Zapytanie = String.Format("SELECT COUNT(*) FROM Wzorcowanie_zrodlami_powierzchniowymi WHERE id_karty = {0} AND Dolacz=true",
                                           _DaneWypelniajace[(int)stale.NR_KARTY]);
                _DaneWypelniajace[(int)stale.WZ_ZR_POW_ILE] = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<int>(0).ToString();

                // zliczenie liczby wpisów dot. wzorcowania mocy dawki
                _Zapytanie = String.Format("SELECT COUNT(*) FROM Wzorcowanie_cezem WHERE id_karty = {0} AND rodzaj_wzorcowania = 'md' AND Dolacz=true",
                                           _DaneWypelniajace[(int)stale.NR_KARTY]);
                _DaneWypelniajace[(int)stale.WZ_MOC_DAWKI_ILE] = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<int>(0).ToString();

                // zliczenie liczby wpisów dot. wzorcowania dawki
                _Zapytanie = String.Format("SELECT COUNT(*) FROM Wzorcowanie_cezem WHERE id_karty = {0} AND rodzaj_wzorcowania = 'd' AND Dolacz=true",
                                           _DaneWypelniajace[(int)stale.NR_KARTY]);
                _DaneWypelniajace[(int)stale.WZ_DAWKI_ILE] = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<int>(0).ToString();

                // zliczenie liczby wpisów dot. syg. dawki
                _Zapytanie = String.Format("SELECT COUNT(*) FROM Wzorcowanie_cezem WHERE id_karty = {0} AND rodzaj_wzorcowania = 'sd' AND Dolacz=true",
                                           _DaneWypelniajace[(int)stale.NR_KARTY]);
                _DaneWypelniajace[(int)stale.WZ_SYG_DAWKI_ILE] = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<int>(0).ToString();

                // zliczenie liczby wpisów dot. syg. mocy dawki
                _Zapytanie = String.Format("SELECT COUNT(*) FROM Wzorcowanie_cezem WHERE id_karty = {0} AND rodzaj_wzorcowania = 'sm' AND Dolacz=true",
                                           _DaneWypelniajace[(int)stale.NR_KARTY]);
                _DaneWypelniajace[(int)stale.WZ_SYG_MOCY_DAWKI_ILE] = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<int>(0).ToString();
            }
        }
    }
}
