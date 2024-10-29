using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using WzorcowanieMocDawkiSpace;

namespace DotBase
{
    namespace Dokumenty
    {
        class SwiadectwoData
        {
            public enum DataType
            {
                ADRES,
                CISNIENIE_MAX, CISNIENIE_MIN,
                DATA_WYDANIA, DATA_WYKONANIA, DATA_PRZYJECIA,
                EMISJA_POW,
                ID_WZORCOWANIA, INFORMACJA,
                JEDNOSTKA,
                METODA_WZORCOWANIA,
                NAPIECIE_ZAS_SONDY, NIEPEWNOSC, NAZWA, NR_FABRYCZNY, NR_KARTY,
                JEDNOSTKA_PRZYRZADU,
                POPRAWA, PRODUCENT,
                ROK, ROK_PRODUKCJI,
                SKAZENIA_NIEPEWNOSC, SKAZENIA_RODZAJ_PROMIENIOWANIA, SKAZENIA_WSPOLCZYNNIK, SONDA_NR_FABRYCZNY, SONDA_TYP, SPRAWDZIL,
                TEMPERATURA_MIN, TEMPERATURA_MAX, TYP,
                UWAGI,
                WIELKOSC_FIZYCZNA, WILGOTNOSC_MAX, WILGOTNOSC_MIN, WSPOLCZYNNIK, WZ_DAWKI_ILE, WZ_MOC_DAWKI_ILE, WZ_SYG_DAWKI_ILE, WZ_ZR_POW_ILE, WZ_SYG_MOCY_DAWKI_ILE,
                ZAKRES, ZLECENIODAWCA, ZRODLO_CZAS_ROZPADU, ZRODLO_NAZWA,
                UWAGA_MD, UWAGA_D, UWAGA_S, UWAGA_SMD, UWAGA_SD,
                MAX_ELEMENTOW
            };

            private readonly IDictionary<DataType, String> m_documentData = new Dictionary<DataType, String>();

            public string getValue(DataType dataType)
            {
                return m_documentData[dataType];
            }

            public void setValue(DataType dataType, string value)
            {
                m_documentData[dataType] = value;
            }
        }

        class Swiadectwo : Wydruki
        {
            StringBuilder _Tabela;
            StringBuilder _SzablonGlownyWzorcowania;
            StringBuilder _SzablonDrugiejStrony;
            SwiadectwoData m_data = new SwiadectwoData();
            DateTime dataWykonania;
            private readonly string EVIDENCE_CORRECTION_MARKER = "P";

            enum staleZrodel { STRONT_SLABY = 2, WEGIEL_SLABY, AMERYK = 7, STRONT_SILNY, WEGIEL_SILNY, CHLOR, PLUTON = 17, STRONT_NAJSILNIEJSZY };

            enum szablon { DAWKA, MOC_DAWKI, SKAZENIA, SYG_DAWKI, SYG_MOCY_DAWKI };


            override protected bool fillDocument() { return true; }
            override protected bool retrieveAllData() { return true; }
			override protected bool saveDocument(string path) { return true; }

            //********************************************************************************************
            public Swiadectwo(int nrKarty, DateTime dataWydania, DateTime dataWykonania, DateTime dataPrzyjecia, String sprawdzil, string poprawa, string uwMD, string uwD, string uwS, string uwSMD, string uwSD, Jezyk jezykSwiadectwa) : base(jezykSwiadectwa)
            //********************************************************************************************
            {
                m_data.setValue(SwiadectwoData.DataType.NR_KARTY, nrKarty.ToString());
                m_data.setValue(SwiadectwoData.DataType.DATA_WYDANIA, formatujDate(dataWydania));
                m_data.setValue(SwiadectwoData.DataType.ROK, dataWydania.Year.ToString());
                m_data.setValue(SwiadectwoData.DataType.DATA_WYKONANIA, formatujDate(dataWykonania));
                m_data.setValue(SwiadectwoData.DataType.DATA_PRZYJECIA, formatujDate(dataPrzyjecia));
                m_data.setValue(SwiadectwoData.DataType.SPRAWDZIL, sprawdzil);
                m_data.setValue(SwiadectwoData.DataType.POPRAWA, poprawa);
                m_data.setValue(SwiadectwoData.DataType.UWAGA_MD, uwMD);
                m_data.setValue(SwiadectwoData.DataType.UWAGA_D, uwD);
                m_data.setValue(SwiadectwoData.DataType.UWAGA_S, uwS);
                m_data.setValue(SwiadectwoData.DataType.UWAGA_SMD, uwSMD);
                m_data.setValue(SwiadectwoData.DataType.UWAGA_SD, uwSD);
                this.dataWykonania = dataWykonania;
            }

            //********************************************************************************************
            private void PobierzDaneDawka(int ile, Char ch)
            //********************************************************************************************
            {
                if (false == SprawdzMozliwoscPobrania(ile, Narzedzia.StaleWzorcowan.stale.PLIK_POMOCNICZY_DAWKA))
                    return;

                m_data.setValue(SwiadectwoData.DataType.JEDNOSTKA, "mSv");
                PobierzIdWzorcowania(ile - 1, "d");
                PobierzDaneSondyDlaCezu();
                PobierzDaneTabeloweDawka();

                _SzablonPodstawowy.Replace("<!c1>", ch.ToString())
                                  .Replace("<!sondaTyp>", TranslacjaForm.Tlumacz(m_data.getValue(SwiadectwoData.DataType.SONDA_TYP), jezyk))
                                  .Replace("<!sondaNrFab>", TranslacjaForm.Tlumacz(m_data.getValue(SwiadectwoData.DataType.SONDA_NR_FABRYCZNY), jezyk))
                                  .Replace("<!c5>", m_data.getValue(SwiadectwoData.DataType.JEDNOSTKA))
                                  .Replace("<!tabela>", _Tabela.ToString())
                                  .Replace("<!uwaga>", m_data.getValue(SwiadectwoData.DataType.UWAGA_D));
            }

            //********************************************************************************************
            private bool PobierzDaneDrugaStrona(bool dolaczTabelePunkty)
            //********************************************************************************************
            {
                Char ch = 'a';
                _SzablonDrugiejStrony = new StringBuilder(200);

                try
                {
                    for (int i = int.Parse(m_data.getValue(SwiadectwoData.DataType.WZ_MOC_DAWKI_ILE)); i > 0; --i)
                    {
                        PobierzDaneMocDawki(i, ch, dolaczTabelePunkty);
                        _SzablonDrugiejStrony.Append(_SzablonPodstawowy);
                        ++ch;
                    }

                    for (int i = int.Parse(m_data.getValue(SwiadectwoData.DataType.WZ_DAWKI_ILE)); i > 0; --i)
                    {
                        PobierzDaneDawka(i, ch);
                        _SzablonDrugiejStrony.Append(_SzablonPodstawowy);
                        ++ch;
                    }

                    for (int i = int.Parse(m_data.getValue(SwiadectwoData.DataType.WZ_SYG_MOCY_DAWKI_ILE)); i > 0; --i)
                    {
                        PobierzDaneSygnalizacjaMocyDawki(i, ch);
                        _SzablonDrugiejStrony.Append(_SzablonPodstawowy);
                        ++ch;
                    }

                    for (int i = int.Parse(m_data.getValue(SwiadectwoData.DataType.WZ_SYG_DAWKI_ILE)); i > 0; --i)
                    {
                        PobierzDaneSygnalizacjaDawki(i, ch);
                        _SzablonDrugiejStrony.Append(_SzablonPodstawowy);
                        ++ch;
                    }

                    if (int.Parse(m_data.getValue(SwiadectwoData.DataType.WZ_ZR_POW_ILE)) > 0)
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
            private void PobierzDaneMocDawki(int ile, Char ch, bool dolaczTabelePunkty)
            //********************************************************************************************
            {
                string tabpunkt = null;

                if (ile <= 0 || false == WczytajSzablon(Narzedzia.StaleWzorcowan.stale.PLIK_POMOCNICZY_MOC_DAWKI))
                    return;

                _Zapytanie = String.Format("SELECT id_wzorcowania FROM Wzorcowanie_cezem WHERE id_karty = {0} AND rodzaj_wzorcowania = 'md' ",
                               m_data.getValue(SwiadectwoData.DataType.NR_KARTY)) + String.Format("AND dolacz = true ORDER BY id_wzorcowania");

                PobierzIdWzorcowania(ile - 1, "md");

                _Zapytanie = "SELECT wielkosc_fizyczna, ID_protokolu, Data_wzorcowania FROM Wzorcowanie_cezem AS WC WHERE id_wzorcowania IN (SELECT id_wzorcowania FROM "
                           + String.Format("Wzorcowanie_cezem WHERE id_karty = {0} AND rodzaj_wzorcowania='md' AND dolacz=true) ORDER BY WC.id_wzorcowania",
                             m_data.getValue(SwiadectwoData.DataType.NR_KARTY));

                var wc = PobierzWielkoscFizyczna();

                _Zapytanie = "SELECT typ, nr_fabryczny FROM Sondy WHERE id_sondy = (SELECT id_sondy FROM wzorcowanie_cezem WHERE "
                           + String.Format("id_wzorcowania = {0})", m_data.getValue(SwiadectwoData.DataType.ID_WZORCOWANIA));

                PobierzDaneSondyDlaCezu();


                _Zapytanie = "SELECT jednostka FROM Jednostki WHERE id_jednostki=(SELECT id_jednostki FROM wzorcowanie_cezem WHERE "
                           + String.Format("id_wzorcowania = {0})", m_data.getValue(SwiadectwoData.DataType.ID_WZORCOWANIA));

                PobierzJednostke();

                _Zapytanie = String.Format("SELECT zakres, wspolczynnik, niepewnosc FROM Wyniki_moc_dawki WHERE id_wzorcowania = {0}",
                           m_data.getValue(SwiadectwoData.DataType.ID_WZORCOWANIA));

                PobierzDaneTabelowe();

                if (dolaczTabelePunkty)
                {
                    var ID_protokolu = wc.Item1;
                    var Data_wzorcowania = wc.Item2;
                    var Data_kalibracji = _BazaDanych.TworzTabeleDanych(
                        "SELECT Data_kalibracji FROM Protokoly_kalibracji_lawy WHERE ID_protokolu=?", ID_protokolu)
                        .Rows[0].Field<DateTime>(0);
                    tabpunkt = TworzTabelePunktow(Data_kalibracji, Data_wzorcowania);
                }

                if (tabpunkt != null)
                {
                    _SzablonPodstawowy = _SzablonPodstawowy
                        .Replace("<!tabpunkt>", tabpunkt)
                        .Replace("<!tabpunkt_begin>", "")
                        .Replace("<!tabpunkt_end>", "");
                }
                else
                {
                    _SzablonPodstawowy = _SzablonPodstawowy
                        .Replace("<!tabpunkt>", "")
                        .Replace("<!tabpunkt_begin>", "<!--")
                        .Replace("<!tabpunkt_end>", "-->");
                }

                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!c1>", ch.ToString());
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!c2>", TranslacjaForm.Tlumacz(m_data.getValue(SwiadectwoData.DataType.WIELKOSC_FIZYCZNA), jezyk));
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!c3>", TranslacjaForm.Tlumacz(m_data.getValue(SwiadectwoData.DataType.SONDA_TYP), jezyk));
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!c4>", TranslacjaForm.Tlumacz(m_data.getValue(SwiadectwoData.DataType.SONDA_NR_FABRYCZNY), jezyk));
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!c5>", m_data.getValue(SwiadectwoData.DataType.JEDNOSTKA));
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!c6>", TranslacjaForm.Tlumacz(m_data.getValue(SwiadectwoData.DataType.WIELKOSC_FIZYCZNA).Replace("moc", "mocy"), jezyk));
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!uwaga>", m_data.getValue(SwiadectwoData.DataType.UWAGA_MD));
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!tabela>", _Tabela.ToString());
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!wielkosc_fizyczna>", TranslacjaForm.Tlumacz(m_data.getValue(SwiadectwoData.DataType.WIELKOSC_FIZYCZNA).Replace("moc", "mocy"), jezyk));

                if (m_data.getValue(SwiadectwoData.DataType.JEDNOSTKA).Contains("cps") || m_data.getValue(SwiadectwoData.DataType.JEDNOSTKA).Contains("cpm") ||
                    m_data.getValue(SwiadectwoData.DataType.JEDNOSTKA).Contains("1/s") || m_data.getValue(SwiadectwoData.DataType.JEDNOSTKA).Contains("1/min") ||
                    m_data.getValue(SwiadectwoData.DataType.JEDNOSTKA).Contains("imp/s") || m_data.getValue(SwiadectwoData.DataType.JEDNOSTKA).Contains("imp/min") ||
                    m_data.getValue(SwiadectwoData.DataType.JEDNOSTKA).Contains("s-1") || m_data.getValue(SwiadectwoData.DataType.JEDNOSTKA).Contains("Bq/cm2"))
                {
                    _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!korekcja_jednostki>", String.Format("(\u00B5Gy/h)/({0})", m_data.getValue(SwiadectwoData.DataType.JEDNOSTKA)));
                }
                else
                {
                    _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!korekcja_jednostki>", "-");
                }
            }

            private string TworzTabelePunktow(DateTime Data_kalibracji, DateTime Data_wzorcowania)
            {
                _Zapytanie = "SELECT odleglosc, id_zrodla, wskazanie, wahanie, zakres, dolaczyc FROM Pomiary_cez WHERE id_wzorcowania IN (SELECT id_wzorcowania FROM "
                           + String.Format("Wzorcowanie_cezem WHERE id_karty = {0} AND rodzaj_wzorcowania='md' AND dolacz=true)",
                             m_data.getValue(SwiadectwoData.DataType.NR_KARTY));

                var wzorcowanieMocDawki = new WzorcowanieMocDawki(Int32.Parse(m_data.getValue(SwiadectwoData.DataType.NR_KARTY)), "md");

                var _OdpowiedzBazy = _BazaDanych.TworzTabeleDanych(_Zapytanie);

                if (null == _OdpowiedzBazy || null == _OdpowiedzBazy.Rows || 0 == _OdpowiedzBazy.Rows.Count)
                    return null;

                var res = new StringBuilder();
                var tabPunkt = new List<Tuple<double, double>>();
                var tabDaneDlaWartWzor = new List<Tuple<double, int>>();

                foreach (DataRow wiersz in _OdpowiedzBazy.Rows)
                {
                    tabPunkt.Add(new Tuple<double, double>(
                        wiersz.Field<double>("wskazanie"),
                        wiersz.Field<double>("wahanie")
                        ));
                    tabDaneDlaWartWzor.Add(new Tuple<double, int>(
                        wiersz.Field<double>("odleglosc"),
                        wiersz.Field<short>("id_zrodla")
                        ));
                }

                var tabWartWzor = wzorcowanieMocDawki.LiczWartoscWzorcowa(
                    tabDaneDlaWartWzor.ToArray(),
                    Data_kalibracji.ToString("yyyy-MM-dd"),
                    m_data.getValue(SwiadectwoData.DataType.JEDNOSTKA).Replace("&mu;", "u"),
                    Data_wzorcowania);

                for (int i = 0; i < tabPunkt.Count; i++)
                {
                    res.AppendFormat("<tr><td>{0}</td><td>{1}&nbsp;±&nbsp;{2}</td><td>{3}&nbsp;±&nbsp;{4}</td></tr>",
                        i + 1,
                        tabWartWzor[i].ToString("0.00"),
                        (tabWartWzor[i] * 0.02).ToString("0.00"),
                        tabPunkt[i].Item1,
                        tabPunkt[i].Item2);
                }

                return res.ToString();
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
                           + String.Format("FROM Karta_przyjecia WHERE id_karty = {0})", m_data.getValue(SwiadectwoData.DataType.NR_KARTY));

                try
                {
                    DataRow wiersz = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0];

                    m_data.setValue(SwiadectwoData.DataType.NAZWA, wiersz.Field<string>(0));
                    m_data.setValue(SwiadectwoData.DataType.TYP, wiersz.Field<string>(1));
                    m_data.setValue(SwiadectwoData.DataType.NR_FABRYCZNY, wiersz.Field<string>(2));
                    m_data.setValue(SwiadectwoData.DataType.PRODUCENT, wiersz.Field<string>(3));
                    m_data.setValue(SwiadectwoData.DataType.ROK_PRODUKCJI, wiersz.Field<string>(4));
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
                string nrKarty = m_data.getValue(SwiadectwoData.DataType.NR_KARTY);

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
                    m_data.setValue(SwiadectwoData.DataType.ID_WZORCOWANIA, idWzorcowan[i].ToString());
                    string idWzorcowania = m_data.getValue(SwiadectwoData.DataType.ID_WZORCOWANIA);
                    
                    PobierzDaneSondyDlaSkazen();
                    PobierzDaneSpecyficzneDlaSkazen(ref skazeniaWspolczynnik, ref skazeniaNiepewnosc);
                    PobierzDaneZrodla();
                    PobierzEmisjePowierzchniowa(idWzorcowan[i]);

                    _Tabela.Append(String.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td><td>{6}</td>",
                              i + 1,TranslacjaForm.Tlumacz(m_data.getValue(SwiadectwoData.DataType.SONDA_TYP), jezyk), TranslacjaForm.Tlumacz(m_data.getValue(SwiadectwoData.DataType.SONDA_NR_FABRYCZNY), jezyk),
                              m_data.getValue(SwiadectwoData.DataType.NAPIECIE_ZAS_SONDY), m_data.getValue(SwiadectwoData.DataType.ZRODLO_NAZWA).Replace("(słaby)", "").Replace("(silny)", "").Replace("(najsilniejszy)", ""),
                              m_data.getValue(SwiadectwoData.DataType.SKAZENIA_RODZAJ_PROMIENIOWANIA), m_data.getValue(SwiadectwoData.DataType.EMISJA_POW))
                              + String.Format("<td>{0}</td><td width=\"110\"><!wspol{1}> &plusmn; <!niep{1}></td></tr>",
                              m_data.getValue(SwiadectwoData.DataType.JEDNOSTKA_PRZYRZADU), i));
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
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!uwaga>", m_data.getValue(SwiadectwoData.DataType.UWAGA_S));
            }

            //********************************************************************************************
            private void PobierzEmisjePowierzchniowa(int idWzorcowania)
            //********************************************************************************************
            {
                _Zapytanie = String.Format("SELECT mnoznik_korekcyjny, data_wzorcowania FROM Wzorcowanie_zrodlami_powierzchniowymi WHERE id_wzorcowania = {0}", m_data.getValue(SwiadectwoData.DataType.ID_WZORCOWANIA));
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
                double czasPolowicznegoRozpadu = N.doubleParse(m_data.getValue(SwiadectwoData.DataType.ZRODLO_CZAS_ROZPADU));
                m_data.setValue(SwiadectwoData.DataType.EMISJA_POW, (mnoznikKorekcyjny * emisja * Math.Exp(-Math.Log(2) / czasPolowicznegoRozpadu * (dataWzorcowaniaPrzyrzadu - dataWzorcowaniaZrodla).Days / 365.25)).ToString("0.00"));
            }

            //********************************************************************************************
            private void PobierzDaneZrodla()
            //********************************************************************************************
            {
                _Zapytanie = "SELECT nazwa, czas_polowicznego_rozpadu FROM zrodla_powierzchniowe WHERE id_zrodla=(SELECT id_zrodla "
                               + String.Format("FROM Wzorcowanie_zrodlami_powierzchniowymi WHERE id_wzorcowania = {0})", m_data.getValue(SwiadectwoData.DataType.ID_WZORCOWANIA));

                DataRow wiersz = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0];
                m_data.setValue(SwiadectwoData.DataType.ZRODLO_NAZWA, wiersz.Field<string>(0));
                m_data.setValue(SwiadectwoData.DataType.ZRODLO_CZAS_ROZPADU, wiersz.Field<float>(1).ToString());

                if (m_data.getValue(SwiadectwoData.DataType.ZRODLO_NAZWA) == "Am-241" || m_data.getValue(SwiadectwoData.DataType.ZRODLO_NAZWA) == "Pu-239")
                {
                    m_data.setValue(SwiadectwoData.DataType.SKAZENIA_RODZAJ_PROMIENIOWANIA, "&alpha;");
                }
                else
                {
                    m_data.setValue(SwiadectwoData.DataType.SKAZENIA_RODZAJ_PROMIENIOWANIA, "&beta;");
                }
            }

            //********************************************************************************************
            private void PobierzDaneSondyDlaSkazen()
            //********************************************************************************************
            {
                _Zapytanie = "SELECT typ, nr_fabryczny FROM Sondy WHERE id_sondy=(SELECT id_sondy FROM Wzorcowanie_zrodlami_powierzchniowymi "
                               + String.Format("WHERE id_wzorcowania = {0})", m_data.getValue(SwiadectwoData.DataType.ID_WZORCOWANIA));

                DataRow wiersz = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0];
                m_data.setValue(SwiadectwoData.DataType.SONDA_TYP, wiersz.Field<string>(0));
                m_data.setValue(SwiadectwoData.DataType.SONDA_NR_FABRYCZNY, wiersz.Field<string>(1));
            }

            //********************************************************************************************
            private void PobierzDaneSondyDlaCezu()
            //********************************************************************************************
            {
                _Zapytanie = "SELECT typ, nr_fabryczny FROM Sondy WHERE id_sondy=(SELECT id_sondy FROM Wzorcowanie_cezem "
                               + String.Format("WHERE id_wzorcowania = {0})", m_data.getValue(SwiadectwoData.DataType.ID_WZORCOWANIA));

                DataRow wiersz = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0];
                m_data.setValue(SwiadectwoData.DataType.SONDA_TYP, wiersz.Field<string>(0));
                m_data.setValue(SwiadectwoData.DataType.SONDA_NR_FABRYCZNY, wiersz.Field<string>(1));
            }

            //********************************************************************************************
            private void PobierzDaneSpecyficzneDlaSkazen(ref List<double> skazeniaWspolczynnik, ref List<double> skazeniaNiepewnosc)
            //********************************************************************************************
            {
                _Zapytanie = "SELECT napiecie_zasilania_sondy, odleglosc_zrodlo_sonda, wspolczynnik, niepewnosc, ID_jednostki FROM "
                           + String.Format("Wzorcowanie_zrodlami_powierzchniowymi WHERE id_wzorcowania = {0}", m_data.getValue(SwiadectwoData.DataType.ID_WZORCOWANIA));

                DataRow wiersz = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0];

                _Zapytanie = String.Format("SELECT Jednostka FROM Jednostki WHERE ID_jednostki = {0}", wiersz.Field<int>(4));

                DataRow wierszJedn = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0];

                m_data.setValue(SwiadectwoData.DataType.NAPIECIE_ZAS_SONDY, wiersz.Field<string>(0));
                m_data.setValue(SwiadectwoData.DataType.JEDNOSTKA_PRZYRZADU, wierszJedn.Field<string>(0));


                skazeniaWspolczynnik.Add(wiersz.Field<double>(2));
                skazeniaNiepewnosc.Add(wiersz.Field<double>(3));
                //m_data.setValue(SwiadectwoData.DataType.SKAZENIA_WSPOLCZYNNIK, temp.ToString();
                //m_data.setValue(SwiadectwoData.DataType.SKAZENIA_NIEPEWNOSC, temp.ToString();
            }

            //********************************************************************************************
            private void PobierzDaneSygnalizacjaDawki(int ile, Char ch)
            //********************************************************************************************
            {
                if (false == SprawdzMozliwoscPobrania(ile, Narzedzia.StaleWzorcowan.stale.PLIK_POMOCNICZY_SYGNALIZACJA_DAWKI))
                    return;

                _Zapytanie = String.Format("SELECT id_wzorcowania FROM Wzorcowanie_cezem WHERE id_karty = {0} ", m_data.getValue(SwiadectwoData.DataType.NR_KARTY))
                           + "AND rodzaj_wzorcowania = 'sd' AND dolacz = true ORDER BY id_wzorcowania";

                PobierzIdWzorcowania(ile - 1, "sd");

                _Zapytanie = "SELECT typ, nr_fabryczny FROM Sondy WHERE id_sondy = (SELECT id_sondy FROM wzorcowanie_cezem WHERE "
                           + String.Format("id_wzorcowania = {0})", m_data.getValue(SwiadectwoData.DataType.ID_WZORCOWANIA));

                PobierzDaneSondyDlaCezu();

                _Zapytanie = "SELECT jednostka FROM Jednostki WHERE id_jednostki=(SELECT id_jednostki FROM wzorcowanie_cezem "
                           + String.Format("WHERE id_wzorcowania = {0})", m_data.getValue(SwiadectwoData.DataType.ID_WZORCOWANIA));

                PobierzJednostke();

                _Zapytanie = "SELECT prog, wartosc_wzorcowa, wartosc_zmierzona, Niepewnosc, Wspolczynnik, Niepewnosc_wsp FROM Sygnalizacja_dawka WHERE "
                           + String.Format("id_wzorcowania = {0}", m_data.getValue(SwiadectwoData.DataType.ID_WZORCOWANIA));

                PobierzDaneTabeloweSygnalizacja();

                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!c1>", ch.ToString());
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!c2>", TranslacjaForm.Tlumacz(m_data.getValue(SwiadectwoData.DataType.SONDA_TYP), jezyk));
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!c3>", TranslacjaForm.Tlumacz(m_data.getValue(SwiadectwoData.DataType.SONDA_NR_FABRYCZNY), jezyk));
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!c4>", m_data.getValue(SwiadectwoData.DataType.JEDNOSTKA));
                var jedn = m_data.getValue(SwiadectwoData.DataType.JEDNOSTKA);
                var parts = jedn.Split('/');
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!jednDawka>", parts[0]);
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!tabela>", _Tabela.ToString());
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!uwaga>", m_data.getValue(SwiadectwoData.DataType.UWAGA_SD));
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
                    _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!opis>", m_data.getValue(SwiadectwoData.DataType.UWAGI));
                    _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!uwaga>", m_data.getValue(SwiadectwoData.DataType.UWAGA_SMD));
                    return;
                }

                if (false == SprawdzMozliwoscPobrania(ile, Narzedzia.StaleWzorcowan.stale.PLIK_POMOCNICZY_SYGNALIZACJA_MOCY_DAWKI))
                    return;

                _Zapytanie = String.Format("SELECT id_wzorcowania FROM Wzorcowanie_cezem WHERE id_karty = {0} AND ", m_data.getValue(SwiadectwoData.DataType.NR_KARTY))
                           + "rodzaj_wzorcowania='sm' AND dolacz=true ORDER BY id_wzorcowania";

                PobierzIdWzorcowania(ile - 1, "sm");

                _Zapytanie = "SELECT typ, nr_fabryczny FROM Sondy WHERE id_sondy=(SELECT id_sondy FROM wzorcowanie_cezem WHERE "
                           + String.Format("id_wzorcowania = {0})", m_data.getValue(SwiadectwoData.DataType.ID_WZORCOWANIA));
                PobierzDaneSondyDlaCezu();


                _Zapytanie = "SELECT jednostka FROM Jednostki WHERE id_jednostki=(SELECT id_jednostki FROM wzorcowanie_cezem WHERE "
                           + String.Format("id_wzorcowania = {0})", m_data.getValue(SwiadectwoData.DataType.ID_WZORCOWANIA));
                PobierzJednostke();

                _Zapytanie = String.Format("SELECT prog, wartosc_zmierzona, niepewnosc, Wspolczynnik, Niepewnosc_Wspolczynnika FROM Sygnalizacja WHERE id_wzorcowania = {0}",
                                          m_data.getValue(SwiadectwoData.DataType.ID_WZORCOWANIA));
                PobierzDaneTabeloweSygMocyDawki();

                _SzablonPodstawowy.Replace("<!c1>", ch.ToString())
                                  .Replace("<!c2>", TranslacjaForm.Tlumacz(m_data.getValue(SwiadectwoData.DataType.SONDA_TYP), jezyk))
                                  .Replace("<!c3>", TranslacjaForm.Tlumacz(m_data.getValue(SwiadectwoData.DataType.SONDA_NR_FABRYCZNY), jezyk))
                                  .Replace("<!c4>", m_data.getValue(SwiadectwoData.DataType.JEDNOSTKA))
                                  .Replace("<!tabela>", _Tabela.ToString())
                                  .Replace("<!uwaga>", m_data.getValue(SwiadectwoData.DataType.UWAGA_SMD));

            }

            //********************************************************************************************
            private bool SprawdzCzyIstniejaUwagi()
            //********************************************************************************************
            {
                _Zapytanie = "SELECT Uwagi FROM Sygnalizacja WHERE id_wzorcowania IN (SELECT id_wzorcowania FROM wzorcowanie_cezem "
                           + String.Format("WHERE id_karty = {0})", m_data.getValue(SwiadectwoData.DataType.NR_KARTY));

                try
                {
                    m_data.setValue(SwiadectwoData.DataType.UWAGI, _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<string>(0));
                }
                catch (Exception)
                {
                    return false;
                }

                if (m_data.getValue(SwiadectwoData.DataType.UWAGI) == "")
                    return false;
                else
                    return true;
            }

            //********************************************************************************************
            private void PobierzDaneTabeloweDawka()
            //********************************************************************************************
            {
                _Zapytanie = String.Format("SELECT zakres, wspolczynnik, niepewnosc, wielkosc_fizyczna FROM Wyniki_dawka WHERE id_wzorcowania = {0}",
                                          m_data.getValue(SwiadectwoData.DataType.ID_WZORCOWANIA));

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
                    _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!wielkosc>", "indywidualny równoważnik dawki Hₚ(10)");
                    _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!wielkosc2>", "indywidualnego równoważnika dawki Hₚ(10)");
                }
                else if (wielkosc_fizyczna[0] == 1)
                {
                    _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!wielkosc>", "indywidualny równoważnik dawki Hₚ(0,07)");
                    _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!wielkosc2>", "indywidualnego równoważnika dawki Hₚ(0,07)");
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
                List<double> niepewnosc = new List<double>();
                List<double> wspolczynnik = new List<double>();
                List<double> niepewnosc_wsp = new List<double>();

                foreach (DataRow wiersz in _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows)
                {
                    prog.Add(wiersz.Field<double>(0));
                    wartosc_wzorcowa.Add(wiersz.Field<double>(1));
                    wartosc_zmierzona.Add(wiersz.Field<double>(2));
                    niepewnosc.Add(wiersz.Field<double>(3));
                    wspolczynnik.Add(wiersz.Field<double>(4));
                    niepewnosc_wsp.Add(wiersz.Field<double>(5));
                }

                IList<double> computedFactors;
                IList<double> computedUncertainity;

                if (N.proceduraOd20230915(DateTime.Parse(m_data.getValue(SwiadectwoData.DataType.DATA_WYKONANIA))))
                {
                    computedFactors = wspolczynnik;
                    computedUncertainity = niepewnosc_wsp;
                }
                else
                {
                    computedFactors = SygnalizacjaDawkiUtils.computeFactors(m_data.getValue(SwiadectwoData.DataType.NR_KARTY));
                    computedUncertainity = SygnalizacjaDawkiUtils.computeUncertainity(m_data.getValue(SwiadectwoData.DataType.NR_KARTY));
                }

                _Tabela = new StringBuilder();

                for (int i = 0; i < prog.Count; ++i)
                {
                    _Tabela.AppendFormat("<tr><td>{0}</td><td>{1} &plusmn; {2} </td><td>{3} &plusmn; {4} </td></tr>",
                        prog[i].ToString("G"),
                        wartosc_wzorcowa[i].ToString("0.00"),
                        niepewnosc[i].ToString("0.00"),
                        computedFactors[i].ToString("0.00"),
                        computedUncertainity[i].ToString("0.00"));
                }
            }

            //********************************************************************************************
            private void PobierzDaneTabeloweSygMocyDawki()
            //********************************************************************************************
            {
                List<double> prog = new List<double>();
                List<double> wartosc_zmierzona = new List<double>();
                List<double> niepewnosc = new List<double>();
                List<double> wspolczynnik = new List<double>();
                List<double> niepewnosc_wspolczynnika = new List<double>();

                foreach (DataRow wiersz in _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows)
                {
                    prog.Add(wiersz.Field<double>(0));
                    wartosc_zmierzona.Add(wiersz.Field<double>(1));
                    niepewnosc.Add(wiersz.Field<double>(2));
                    wspolczynnik.Add(wiersz.Field<double>(3));
                    niepewnosc_wspolczynnika.Add(wiersz.Field<double>(4));
                }

                IList<double> computedFactors;
                IList<double> computedUncertainity;

                if (N.proceduraOd20230915(dataWykonania))
                {
                    computedFactors = wspolczynnik;
                    computedUncertainity = niepewnosc_wspolczynnika;
                }
                else
                {
                    computedFactors = SygnalizacjaMocyDawkiUtils.computeFactors(m_data.getValue(SwiadectwoData.DataType.NR_KARTY));
                    computedUncertainity = SygnalizacjaMocyDawkiUtils.computeUncertainity(m_data.getValue(SwiadectwoData.DataType.NR_KARTY));
                }

                _Tabela = new StringBuilder();

                for (int i = 0; i < prog.Count; ++i)
                {
                    _Tabela.AppendFormat("<tr><td>{0}</td><td>{1} &plusmn; {2}</td><td>{3} &plusmn; {4}</td></tr>",
                        prog[i].ToString("G"),
                        wartosc_zmierzona[i].ToString("0.00"),
                        niepewnosc[i].ToString("0.00"),
                        computedFactors[i].ToString("0.00"),
                        computedUncertainity[i].ToString("0.00"));
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
                    bool bSI = true;

                    _Zapytanie = "Select Cisnienie, Temperatura, Wilgotnosc From ";

                    if (int.Parse(m_data.getValue(SwiadectwoData.DataType.WZ_MOC_DAWKI_ILE)) > 0)
                    {
                        _Zapytanie += String.Format("wzorcowanie_cezem where id_karty = {0} AND rodzaj_wzorcowania='md' AND dolacz=true", m_data.getValue(SwiadectwoData.DataType.NR_KARTY));
                        bCez = true;
                    }
                    else if (int.Parse(m_data.getValue(SwiadectwoData.DataType.WZ_DAWKI_ILE)) > 0)
                    {
                        _Zapytanie += String.Format("wzorcowanie_cezem where id_karty = {0} AND rodzaj_wzorcowania='d' AND dolacz=true", m_data.getValue(SwiadectwoData.DataType.NR_KARTY));
                        bDawka = true;
                    }
                    else if (int.Parse(m_data.getValue(SwiadectwoData.DataType.WZ_SYG_MOCY_DAWKI_ILE)) > 0)
                    {
                        _Zapytanie += String.Format("wzorcowanie_cezem where id_karty = {0} AND rodzaj_wzorcowania='sm' AND dolacz=true", m_data.getValue(SwiadectwoData.DataType.NR_KARTY));
                        bCez = true;
                    }
                    else if (int.Parse(m_data.getValue(SwiadectwoData.DataType.WZ_SYG_DAWKI_ILE)) > 0)
                    {
                        _Zapytanie += String.Format("wzorcowanie_cezem where id_karty = {0} AND rodzaj_wzorcowania='sd' AND dolacz=true", m_data.getValue(SwiadectwoData.DataType.NR_KARTY));
                        bCez = true;
                    }
                    else if (int.Parse(m_data.getValue(SwiadectwoData.DataType.WZ_ZR_POW_ILE)) > 0)
                    {
                        _Zapytanie += String.Format("wzorcowanie_zrodlami_powierzchniowymi where id_karty = {0} AND dolacz=true", m_data.getValue(SwiadectwoData.DataType.NR_KARTY));
                        bSkazenia = true;
                    }

                    if (int.Parse(m_data.getValue(SwiadectwoData.DataType.WZ_ZR_POW_ILE)) > 0)
                    {
                        bSkazenia = true;
                    }
                    if (int.Parse(m_data.getValue(SwiadectwoData.DataType.WZ_DAWKI_ILE)) > 0)
                    {
                        bDawka = true;
                    }
                    if (int.Parse(m_data.getValue(SwiadectwoData.DataType.WZ_SYG_MOCY_DAWKI_ILE)) > 0 ||
                        int.Parse(m_data.getValue(SwiadectwoData.DataType.WZ_SYG_DAWKI_ILE)) > 0      ||
                        int.Parse(m_data.getValue(SwiadectwoData.DataType.WZ_MOC_DAWKI_ILE)) > 0)
                    {
                        bCez = true;
                    }

                    if (int.Parse(m_data.getValue(SwiadectwoData.DataType.WZ_MOC_DAWKI_ILE)) > 0)
                    {
                        DataRow odpowiedzJednostki = _BazaDanych.TworzTabeleDanych("SELECT SI FROM Jednostki WHERE ID_jednostki=(SELECT TOP 1 ID_jednostki FROM wzorcowanie_cezem WHERE ID_karty=" + m_data.getValue(SwiadectwoData.DataType.NR_KARTY) + " AND rodzaj_wzorcowania=\"md\")").Rows[0];
                        bSI = odpowiedzJednostki.Field<bool>(0);
                    }
                    else
                    {
                        bSI = true;
                    }

                    DataRow odpowiedzBazy = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0];

                    double temp = odpowiedzBazy.Field<double>(0);
                    m_data.setValue(SwiadectwoData.DataType.CISNIENIE_MIN, (temp - Constants.getInstance().UNCERTAINITY_PRESSURE_VALUE).ToString("0.0"));
                    m_data.setValue(SwiadectwoData.DataType.CISNIENIE_MAX, (temp + Constants.getInstance().UNCERTAINITY_PRESSURE_VALUE).ToString("0.0"));
                    temp = odpowiedzBazy.Field<double>(1);
                    m_data.setValue(SwiadectwoData.DataType.TEMPERATURA_MIN, (temp - Constants.getInstance().UNCERTAINITY_TEMPERATURE_VALUE).ToString("0.0"));
                    m_data.setValue(SwiadectwoData.DataType.TEMPERATURA_MAX, (temp + Constants.getInstance().UNCERTAINITY_TEMPERATURE_VALUE).ToString("0.0"));
                    temp = odpowiedzBazy.Field<double>(2);
                    m_data.setValue(SwiadectwoData.DataType.WILGOTNOSC_MIN, (temp - Constants.getInstance().UNCERTAINITY_HUMIDITY_VALUE).ToString("0.0"));
                    m_data.setValue(SwiadectwoData.DataType.WILGOTNOSC_MAX, (temp + Constants.getInstance().UNCERTAINITY_HUMIDITY_VALUE).ToString("0.0"));

                    SwiadectwoTextsLoader stl = new SwiadectwoTextsLoader(jezyk);
                    if (bDawka && bCez && bSkazenia)
                    {
                        m_data.setValue(SwiadectwoData.DataType.METODA_WZORCOWANIA, stl.GetText("metodaWzorcowania", true, true, true));
                    }
                    else if (bDawka && bSkazenia)
                    {
                        m_data.setValue(SwiadectwoData.DataType.METODA_WZORCOWANIA, stl.GetText("metodaWzorcowania", true, false, true));
                    }
                    else if (bDawka && bCez)
                    {
                        m_data.setValue(SwiadectwoData.DataType.METODA_WZORCOWANIA, stl.GetText("metodaWzorcowania", true, true, false));
                    }
                    else if (bDawka)
                    {
                        m_data.setValue(SwiadectwoData.DataType.METODA_WZORCOWANIA, stl.GetText("metodaWzorcowania", true, false, false));
                    }
                    else if (bCez && bSkazenia)
                    {
                        m_data.setValue(SwiadectwoData.DataType.METODA_WZORCOWANIA, stl.GetText("metodaWzorcowania", false, true, true));
                    }
                    else if (bCez && !bSkazenia)
                    {
                        m_data.setValue(SwiadectwoData.DataType.METODA_WZORCOWANIA, stl.GetText("metodaWzorcowania", false, true, false));
                    }
                    else
                    {
                        m_data.setValue(SwiadectwoData.DataType.METODA_WZORCOWANIA, stl.GetText("metodaWzorcowania", false, false, true));
                    }

                    if (bSI)
                    {
                        m_data.setValue(SwiadectwoData.DataType.INFORMACJA, stl.GetTextInfo("tekst", bSI));
                    }
                    else if (int.Parse(m_data.getValue(SwiadectwoData.DataType.WZ_ZR_POW_ILE)) > 0)
                    {

                        bool polon = false;
                        bool bruschweig = false;

                        _Zapytanie = String.Format("SELECT id_zrodla FROM Wzorcowanie_zrodlami_powierzchniowymi WHERE id_karty = {0}", m_data.getValue(SwiadectwoData.DataType.NR_KARTY));

                        foreach (DataRow row in _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows)
                        {
                            int idZrodla = row.Field<int>(0);

                            if ((int)staleZrodel.STRONT_NAJSILNIEJSZY == idZrodla)
                            {
                                bruschweig = true;
                            }
                            else if ((int)staleZrodel.WEGIEL_SLABY == idZrodla || (int)staleZrodel.CHLOR == idZrodla || (int)staleZrodel.PLUTON == idZrodla ||
                                (int)staleZrodel.WEGIEL_SILNY == idZrodla || (int)staleZrodel.STRONT_SLABY == idZrodla || (int)staleZrodel.STRONT_SILNY == idZrodla || (int)staleZrodel.AMERYK == idZrodla)
                            {
                                polon = true;
                            }
                        }


                        m_data.setValue(SwiadectwoData.DataType.INFORMACJA, stl.GetTextInfo("tekst", false, polon, bruschweig));
                    }
                    else 
                    {
                        m_data.setValue(SwiadectwoData.DataType.INFORMACJA, stl.GetTextInfo("tekst", false, false, false));
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
                           + String.Format("Karta_przyjecia WHERE id_karty = {0}))", m_data.getValue(SwiadectwoData.DataType.NR_KARTY));

                try
                {
                    DataRow wiersz = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0];
                    m_data.setValue(SwiadectwoData.DataType.ZLECENIODAWCA, wiersz.Field<string>(0));
                    m_data.setValue(SwiadectwoData.DataType.ADRES, wiersz.Field<string>(1));
                }
                catch (Exception)
                {
                    m_data.setValue(SwiadectwoData.DataType.ZLECENIODAWCA, "");
                    m_data.setValue(SwiadectwoData.DataType.ADRES, "");
                    return false;
                }

                return true;
            }

            //********************************************************************************************
            private void PobierzJednostke()
            //********************************************************************************************
            {
                m_data.setValue(SwiadectwoData.DataType.JEDNOSTKA, _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<string>(0));
                m_data.setValue(SwiadectwoData.DataType.JEDNOSTKA, m_data.getValue(SwiadectwoData.DataType.JEDNOSTKA).Replace("u", "&mu;"));
            }

            //********************************************************************************************
            private Tuple<int, DateTime> PobierzWielkoscFizyczna()
            //********************************************************************************************
            {
                var row = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0];
                m_data.setValue(SwiadectwoData.DataType.WIELKOSC_FIZYCZNA, row.Field<string>(0));
                return new Tuple<int,DateTime>(row.Field<short>(1), row.Field<DateTime>(2));
            }

            //********************************************************************************************
            private void PobierzIdWzorcowania(int ktoreWzorcowaniePobrac, String rodzajWzorcowania)
            //********************************************************************************************
            {
                _Zapytanie = String.Format("SELECT id_wzorcowania FROM Wzorcowanie_cezem WHERE id_karty = {0} ", m_data.getValue(SwiadectwoData.DataType.NR_KARTY))
                           + String.Format(" AND rodzaj_wzorcowania='{0}' AND dolacz=true ORDER BY id_wzorcowania", rodzajWzorcowania);
                m_data.setValue(SwiadectwoData.DataType.ID_WZORCOWANIA, _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[ktoreWzorcowaniePobrac].Field<int>(0).ToString());
            }

            //********************************************************************************************
            private bool SprawdzMozliwoscPobrania(int ilePobran, Narzedzia.StaleWzorcowan.stale typSzablonu)
            //********************************************************************************************
            {
                return 0 < ilePobran && true == WczytajSzablon(typSzablonu);
            }

            private String mapEvidenceIdToDisplayableForm(String evidenceId)
            {
                if ( Boolean.Parse(m_data.getValue(SwiadectwoData.DataType.POPRAWA)) )
                {
                    return evidenceId + EVIDENCE_CORRECTION_MARKER; 
                }

                return evidenceId;
            }

            //********************************************************************************************
            private bool UtworzPierwszaStrone()
            //********************************************************************************************
            {
                if (false == PobierzSzablonGlowny())
                    return false;

                _SzablonGlownyWzorcowania = _SzablonGlownyWzorcowania.Replace("<!c1>", m_data.getValue(SwiadectwoData.DataType.DATA_WYDANIA));
                _SzablonGlownyWzorcowania = _SzablonGlownyWzorcowania.Replace("<!dataWykonania>", m_data.getValue(SwiadectwoData.DataType.DATA_WYKONANIA));
                _SzablonGlownyWzorcowania = _SzablonGlownyWzorcowania.Replace("<!dataPrzyjecia>", m_data.getValue(SwiadectwoData.DataType.DATA_PRZYJECIA));
                _SzablonGlownyWzorcowania = _SzablonGlownyWzorcowania.Replace("<!c2>", mapEvidenceIdToDisplayableForm(m_data.getValue(SwiadectwoData.DataType.NR_KARTY)));
                _SzablonGlownyWzorcowania = _SzablonGlownyWzorcowania.Replace("<!c3>", m_data.getValue(SwiadectwoData.DataType.ROK));
                _SzablonGlownyWzorcowania = _SzablonGlownyWzorcowania.Replace("<!c4>", TranslacjaForm.Tlumacz(m_data.getValue(SwiadectwoData.DataType.NAZWA), jezyk));
                _SzablonGlownyWzorcowania = _SzablonGlownyWzorcowania.Replace("<!c5>", m_data.getValue(SwiadectwoData.DataType.TYP));
                _SzablonGlownyWzorcowania = _SzablonGlownyWzorcowania.Replace("<!c6>", m_data.getValue(SwiadectwoData.DataType.NR_FABRYCZNY));
                _SzablonGlownyWzorcowania = _SzablonGlownyWzorcowania.Replace("<!c7>", m_data.getValue(SwiadectwoData.DataType.PRODUCENT));
                _SzablonGlownyWzorcowania = _SzablonGlownyWzorcowania.Replace("<!c8>", TranslacjaForm.Tlumacz(m_data.getValue(SwiadectwoData.DataType.ROK_PRODUKCJI), jezyk));
                _SzablonGlownyWzorcowania = _SzablonGlownyWzorcowania.Replace("<!c9>", m_data.getValue(SwiadectwoData.DataType.ZLECENIODAWCA));
                _SzablonGlownyWzorcowania = _SzablonGlownyWzorcowania.Replace("<!c10>", m_data.getValue(SwiadectwoData.DataType.ADRES));
                _SzablonGlownyWzorcowania = _SzablonGlownyWzorcowania.Replace("<!c11>", m_data.getValue(SwiadectwoData.DataType.CISNIENIE_MIN));
                _SzablonGlownyWzorcowania = _SzablonGlownyWzorcowania.Replace("<!c12>", m_data.getValue(SwiadectwoData.DataType.CISNIENIE_MAX));
                _SzablonGlownyWzorcowania = _SzablonGlownyWzorcowania.Replace("<!c13>", m_data.getValue(SwiadectwoData.DataType.TEMPERATURA_MIN));
                _SzablonGlownyWzorcowania = _SzablonGlownyWzorcowania.Replace("<!c14>", m_data.getValue(SwiadectwoData.DataType.TEMPERATURA_MAX));
                _SzablonGlownyWzorcowania = _SzablonGlownyWzorcowania.Replace("<!c15>", m_data.getValue(SwiadectwoData.DataType.WILGOTNOSC_MIN));
                _SzablonGlownyWzorcowania = _SzablonGlownyWzorcowania.Replace("<!c16>", m_data.getValue(SwiadectwoData.DataType.WILGOTNOSC_MAX));
                _SzablonGlownyWzorcowania = _SzablonGlownyWzorcowania.Replace("<!c17>", m_data.getValue(SwiadectwoData.DataType.INFORMACJA));
                _SzablonGlownyWzorcowania = _SzablonGlownyWzorcowania.Replace("<!metoda_wzorcowania>", m_data.getValue(SwiadectwoData.DataType.METODA_WZORCOWANIA));

                DateTime dataWzorcowaniaCez;
                try
                {
                    _Zapytanie = String.Format("SELECT MAX(data_wzorcowania) FROM wzorcowanie_cezem WHERE id_karty = {0} ", m_data.getValue(SwiadectwoData.DataType.NR_KARTY));
                    dataWzorcowaniaCez = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<DateTime>(0);
                }
                catch (Exception)
                {
                    dataWzorcowaniaCez = DateTime.MinValue;
                }

                DateTime dataWzorcowaniaSkazenia;
                try
                {
                    _Zapytanie = String.Format("SELECT MAX(data_wzorcowania) FROM wzorcowanie_zrodlami_powierzchniowymi WHERE id_karty = {0} ", m_data.getValue(SwiadectwoData.DataType.NR_KARTY));
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
                _SzablonGlownyWzorcowania.Replace("<str2>", _SzablonDrugiejStrony.ToString())
                                         .Replace("<!sprawdzil>", m_data.getValue(SwiadectwoData.DataType.SPRAWDZIL));
                return true;
            }

            //********************************************************************************************
            public bool UtworzDokument(string sciezka, bool dolaczTabelePunkty)
            //********************************************************************************************
            {
                bool ok = PobierzDanePierwszaStrona();
                ok = ok && UtworzPierwszaStrone();
                ok = ok && PobierzDaneDrugaStrona(dolaczTabelePunkty);
                ok = ok && UtworzDrugaStrone();
                ok = ok && ZapiszPlikWynikowy(sciezka);
                return ok;
            }

            //********************************************************************************************
            override protected bool ZapiszPlikWynikowy(string sciezka)
            //********************************************************************************************
            {
                using (StreamWriter _Fout = new StreamWriter(sciezka))
                {
                    _Fout.Write(_SzablonGlownyWzorcowania.ToString());
                    _Fout.Close();
                }
                return true;
            }

            //********************************************************************************************
            // w bazie danych dot. ilości typów poszczególnych wzorcowań
            private void ZliczLiczbeWystapien()
            //********************************************************************************************
            {
                // zliczenie liczby wpisów dot. źródeł powierzchniowych
                _Zapytanie = String.Format("SELECT COUNT(*) FROM Wzorcowanie_zrodlami_powierzchniowymi WHERE id_karty = {0} AND Dolacz=true",
                                           m_data.getValue(SwiadectwoData.DataType.NR_KARTY));
                m_data.setValue(SwiadectwoData.DataType.WZ_ZR_POW_ILE, _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<int>(0).ToString());

                // zliczenie liczby wpisów dot. wzorcowania mocy dawki
                _Zapytanie = String.Format("SELECT COUNT(*) FROM Wzorcowanie_cezem WHERE id_karty = {0} AND rodzaj_wzorcowania = 'md' AND Dolacz=true",
                                           m_data.getValue(SwiadectwoData.DataType.NR_KARTY));
                m_data.setValue(SwiadectwoData.DataType.WZ_MOC_DAWKI_ILE, _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<int>(0).ToString());

                // zliczenie liczby wpisów dot. wzorcowania dawki
                _Zapytanie = String.Format("SELECT COUNT(*) FROM Wzorcowanie_cezem WHERE id_karty = {0} AND rodzaj_wzorcowania = 'd' AND Dolacz=true",
                                           m_data.getValue(SwiadectwoData.DataType.NR_KARTY));
                m_data.setValue(SwiadectwoData.DataType.WZ_DAWKI_ILE, _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<int>(0).ToString());

                // zliczenie liczby wpisów dot. syg. dawki
                _Zapytanie = String.Format("SELECT COUNT(*) FROM Wzorcowanie_cezem WHERE id_karty = {0} AND rodzaj_wzorcowania = 'sd' AND Dolacz=true",
                                           m_data.getValue(SwiadectwoData.DataType.NR_KARTY));
                m_data.setValue(SwiadectwoData.DataType.WZ_SYG_DAWKI_ILE, _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<int>(0).ToString());

                // zliczenie liczby wpisów dot. syg. mocy dawki
                _Zapytanie = String.Format("SELECT COUNT(*) FROM Wzorcowanie_cezem WHERE id_karty = {0} AND rodzaj_wzorcowania = 'sm' AND Dolacz=true",
                                           m_data.getValue(SwiadectwoData.DataType.NR_KARTY));
                m_data.setValue(SwiadectwoData.DataType.WZ_SYG_MOCY_DAWKI_ILE, _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<int>(0).ToString());
            }
        }
    }
}
