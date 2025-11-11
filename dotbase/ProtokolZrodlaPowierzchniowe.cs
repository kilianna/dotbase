using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.IO;

namespace DotBase
{
    namespace Dokumenty
    {
        class ProtokolZrodlaPowierzchnioweData
        {
            public enum DataType
            {
                NR_KART, NR_ARKUSZA, DATA, ROK, ZRODLO, NR_ZRODLA, EMISJA_POW,
                DOZYMETR_ID, SONDA_ID, DOZYMETR_TYP, DOZYMETR_NR_FAB, INNE_NASTAWY, SONDA_TYP, SONDA_NR_FAB,
                NAPIECIE_ZAS_SONDY, ZAKRES_POMIAROWY, CISNIENIE, TEMPERATURA, WILGOTNOSC,
                UWAGI, PODSAWKA, ODL_ZRODLA_SONDA, MNOZNIK_KOREKCYJNY, JEDNOSTKA, TABELA,
                WZORCOWAL, SPRAWDZIL, NIEPEWNOSC, WSPOL_KALIBRACYJNY, PO_WSPOL_KALIBRACYJNY,
                NIEPEWNOSC_WZGLEDNA
            }
            
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

        class ProtokolZrodlaPowierzchnioweModel
        {
            public DanePodstawoweModel modelDanePodstawowe;
            public DanePrzyrzaduModel modelDanePrzyrzadu;
            public DaneWarunkowModel modelDaneWarunkow;
            public DaneWspolczynnikowModel modelDaneWspolczynnikow;


            public ProtokolZrodlaPowierzchnioweModel(DanePodstawoweModel modelDanePodstawowe, DanePrzyrzaduModel modelDanePrzyrzadu, DaneWarunkowModel modelDaneWarunkow, DaneWspolczynnikowModel modelDaneWspolczynnikow)
            {
                this.modelDanePodstawowe = modelDanePodstawowe;
                this.modelDanePrzyrzadu = modelDanePrzyrzadu;
                this.modelDaneWarunkow = modelDaneWarunkow;
                this.modelDaneWspolczynnikow = modelDaneWspolczynnikow;
            }

            public string id_zdrodla;
            public string jednostka;
            public string uwagi;
            public DataGridView tabela;
            public string zakres;
            public string podstawka;
            public string odlegl_zr_sonda;
            public string wsp_korekcyjny;
            public string wspol_kalibracyjny;
            public string niep_wspol_kalibracyjnego;
            public string pop_wspol_kalibracyjny;
        }

        class ProtokolZrodlaPowierzchniowe : Wydruki
        {
            ProtokolZrodlaPowierzchnioweData m_data = new ProtokolZrodlaPowierzchnioweData();
            StringBuilder m_templateToFill = DocumentsTemplatesFactory.getInstance().create(DocumentsTemplatesFactory.TemplateType.SCIEZKA_PROTOKOL_SKAZENIA);
            ProtokolZrodlaPowierzchnioweModel m_model;

            //**************************************************************************
            public ProtokolZrodlaPowierzchniowe(ProtokolZrodlaPowierzchnioweModel model)
            //**************************************************************************
            {
                WczytajSzablon(Narzedzia.StaleWzorcowan.stale.PROTOKOL_SKAZENIA);
                m_model = model;
            }

            #region Document generation
            override protected bool fillDocument()
            {
                try
                {
                    WypelnijZnakSprawy(m_templateToFill);
                    m_templateToFill.Replace("<!nrKarty>", m_data.getValue(ProtokolZrodlaPowierzchnioweData.DataType.NR_KART))
                                        .Replace("<!nrArkusza>", m_data.getValue(ProtokolZrodlaPowierzchnioweData.DataType.NR_ARKUSZA))
                                        .Replace("<!data>", m_data.getValue(ProtokolZrodlaPowierzchnioweData.DataType.DATA))
                                        .Replace("<!rok>", m_data.getValue(ProtokolZrodlaPowierzchnioweData.DataType.ROK))
                                        .Replace("<!zrodlo>", m_data.getValue(ProtokolZrodlaPowierzchnioweData.DataType.ZRODLO))
                                        .Replace("<!nr_zrodla>", m_data.getValue(ProtokolZrodlaPowierzchnioweData.DataType.NR_ZRODLA))
                                        .Replace("<!emisja_pow>", m_data.getValue(ProtokolZrodlaPowierzchnioweData.DataType.EMISJA_POW))
                                        .Replace("<!cisnienie>", m_data.getValue(ProtokolZrodlaPowierzchnioweData.DataType.CISNIENIE))
                                        .Replace("<!temperatura>", m_data.getValue(ProtokolZrodlaPowierzchnioweData.DataType.TEMPERATURA))
                                        .Replace("<!wilgotnosc>", m_data.getValue(ProtokolZrodlaPowierzchnioweData.DataType.WILGOTNOSC))
                                        .Replace("<!uwagi>", m_data.getValue(ProtokolZrodlaPowierzchnioweData.DataType.UWAGI))
                                        .Replace("<!podstawka>", m_data.getValue(ProtokolZrodlaPowierzchnioweData.DataType.PODSAWKA))
                                        .Replace("<!odl_zrodlo_sonda>", m_data.getValue(ProtokolZrodlaPowierzchnioweData.DataType.ODL_ZRODLA_SONDA))
                                        .Replace("<!mnoznik_korekcyjny>", m_data.getValue(ProtokolZrodlaPowierzchnioweData.DataType.MNOZNIK_KOREKCYJNY))
                                        .Replace("<!dozymetr_id>", m_data.getValue(ProtokolZrodlaPowierzchnioweData.DataType.DOZYMETR_ID))
                                        .Replace("<!sonda_id>", m_data.getValue(ProtokolZrodlaPowierzchnioweData.DataType.SONDA_ID))
                                        .Replace("<!dozymetr_typ>", m_data.getValue(ProtokolZrodlaPowierzchnioweData.DataType.DOZYMETR_TYP))
                                        .Replace("<!dozymetr_nrFab>", m_data.getValue(ProtokolZrodlaPowierzchnioweData.DataType.DOZYMETR_NR_FAB))
                                        .Replace("<!inne_nastawy>", m_data.getValue(ProtokolZrodlaPowierzchnioweData.DataType.INNE_NASTAWY))
                                        .Replace("<!sonda_typ>", m_data.getValue(ProtokolZrodlaPowierzchnioweData.DataType.SONDA_TYP))
                                        .Replace("<!sonda_nrFab>", m_data.getValue(ProtokolZrodlaPowierzchnioweData.DataType.SONDA_NR_FAB))
                                        .Replace("<!napiecie_zas_sondy>", m_data.getValue(ProtokolZrodlaPowierzchnioweData.DataType.NAPIECIE_ZAS_SONDY))
                                        .Replace("<!zakres_pomiarowy>", m_data.getValue(ProtokolZrodlaPowierzchnioweData.DataType.ZAKRES_POMIAROWY))
                                        .Replace("<!jednostka>", m_data.getValue(ProtokolZrodlaPowierzchnioweData.DataType.JEDNOSTKA))
                                        .Replace("<!tabela>", m_data.getValue(ProtokolZrodlaPowierzchnioweData.DataType.TABELA))
                                        .Replace("<!wzorcowal>", m_data.getValue(ProtokolZrodlaPowierzchnioweData.DataType.WZORCOWAL))
                                        .Replace("<!sprawdzil>", m_data.getValue(ProtokolZrodlaPowierzchnioweData.DataType.SPRAWDZIL))
                                        .Replace("<!niepewnosc>", m_data.getValue(ProtokolZrodlaPowierzchnioweData.DataType.NIEPEWNOSC))
                                        .Replace("<!wspol_kalibracyjny>", m_data.getValue(ProtokolZrodlaPowierzchnioweData.DataType.WSPOL_KALIBRACYJNY))
                                        .Replace("<!pop_wspol_kalibracyjny>", m_data.getValue(ProtokolZrodlaPowierzchnioweData.DataType.PO_WSPOL_KALIBRACYJNY))
                                        .Replace("<!niepewnosc_wzgledna>", m_data.getValue(ProtokolZrodlaPowierzchnioweData.DataType.NIEPEWNOSC_WZGLEDNA));

                    return WypelnijDaneZleceniodawcy(m_templateToFill);
                }
                catch (Exception) { }

                return false;
            }

            override protected bool retrieveAllData()
            {
                try
                {

                    return PobierzDanePodstawowe(m_model.modelDanePodstawowe, m_model.id_zdrodla) &&
                           PobierzDanePomiarow(m_model.jednostka, m_model.uwagi, ref m_model.tabela) &&
                           PobierzDanePrzyrzadu(m_model.modelDanePrzyrzadu, m_model.zakres) &&
                           PobierzDaneWarunkow(m_model.modelDaneWarunkow, m_model.podstawka, m_model.odlegl_zr_sonda, m_model.wsp_korekcyjny) &&
                           PobierzDaneWspolczynnikow(m_model.modelDaneWspolczynnikow, m_model.wspol_kalibracyjny, m_model.niep_wspol_kalibracyjnego, m_model.pop_wspol_kalibracyjny) &&
                           PobierzDaneZleceniodawcy(m_model.modelDanePodstawowe.nrKarty);
                }
                catch (Exception)
                {
                    return false;
                }
            }

            override protected bool saveDocument(string path)
            {
                try
                {
                    using (StreamWriter streamWriter = new StreamWriter(path, false, Encoding.UTF8))
                    {
                        streamWriter.Write(m_templateToFill.ToString());
                    }
                }
                catch (Exception)
                {
                    return false;
                }

                return true;
            }
            #endregion

            //**************************************************************************
            public bool PobierzDanePodstawowe(DanePodstawoweModel model, String idZrodla)
            //**************************************************************************
            {
                m_data.setValue(ProtokolZrodlaPowierzchnioweData.DataType.NR_KART, model.nrKarty);
                m_data.setValue(ProtokolZrodlaPowierzchnioweData.DataType.NR_ARKUSZA, model.nrArkusza);
                m_data.setValue(ProtokolZrodlaPowierzchnioweData.DataType.DATA, model.data.ToShortDateString());
                m_data.setValue(ProtokolZrodlaPowierzchnioweData.DataType.ROK, model.data.Year.ToString());

                string zapytanie;
                zapytanie = String.Format("SELECT nazwa, numer FROM Zrodla_powierzchniowe WHERE id_zrodla = {0}", idZrodla);
                m_data.setValue(ProtokolZrodlaPowierzchnioweData.DataType.ZRODLO, _BazaDanych.TworzTabeleDanych(zapytanie).Rows[0].Field<string>(0));
                m_data.setValue(ProtokolZrodlaPowierzchnioweData.DataType.NR_ZRODLA, _BazaDanych.TworzTabeleDanych(zapytanie).Rows[0].Field<int>(1).ToString());

                _Zapytanie = String.Format("SELECT emisja_powierzchniowa, data_wzorcowania FROM Atesty_zrodel WHERE ", model.data.ToShortDateString())
                + String.Format("id_zrodla = {0} AND data_wzorcowania=(SELECT MAX(data_wzorcowania) FROM Atesty_zrodel WHERE id_zrodla = {0})", idZrodla);

                double emisjaPowierzchniowa = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<double>(0);
                double czas = N.doubleParse((model.data - _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<DateTime>(1)).Days.ToString());

                _Zapytanie = String.Format("SELECT czas_polowicznego_rozpadu FROM Zrodla_powierzchniowe WHERE id_zrodla = {0}", idZrodla);
                double czas_polowicznego_rozpadu = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<float>(0);

                emisjaPowierzchniowa *= Math.Exp(-czas / 365.25 * Math.Log(2.0) / czas_polowicznego_rozpadu);
                m_data.setValue(ProtokolZrodlaPowierzchnioweData.DataType.EMISJA_POW, emisjaPowierzchniowa.ToString("0.00"));

                return true;
            }

            //**************************************************************************
            public bool PobierzDanePrzyrzadu(DanePrzyrzaduModel model, String zakres)
            //**************************************************************************
            {
                string zapytanie;
                zapytanie = String.Format("SELECT id_dozymetru FROM Dozymetry WHERE typ = '{0}' AND nr_fabryczny = '{1}'",
                                          model.typ, model.nrFabryczny);
                string id_dozymetru = _BazaDanych.TworzTabeleDanych(zapytanie).Rows[0].Field<int>(0).ToString();
                zapytanie = String.Format("SELECT id_sondy FROM Sondy WHERE typ = '{0}' AND nr_fabryczny = '{1}' AND id_dozymetru = {2}",
                                          model.sondaTyp, model.sondaNrFabryczny, id_dozymetru);
                string id_sondy = _BazaDanych.TworzTabeleDanych(zapytanie).Rows[0].Field<int>(0).ToString();

                m_data.setValue(ProtokolZrodlaPowierzchnioweData.DataType.DOZYMETR_ID, id_dozymetru);
                m_data.setValue(ProtokolZrodlaPowierzchnioweData.DataType.SONDA_ID, id_sondy);
                m_data.setValue(ProtokolZrodlaPowierzchnioweData.DataType.DOZYMETR_TYP, model.typ);
                m_data.setValue(ProtokolZrodlaPowierzchnioweData.DataType.DOZYMETR_NR_FAB, model.nrFabryczny);
                m_data.setValue(ProtokolZrodlaPowierzchnioweData.DataType.INNE_NASTAWY, model.inneNastawy);
                m_data.setValue(ProtokolZrodlaPowierzchnioweData.DataType.SONDA_TYP, model.sondaTyp);
                m_data.setValue(ProtokolZrodlaPowierzchnioweData.DataType.SONDA_NR_FAB, model.sondaNrFabryczny);
                m_data.setValue(ProtokolZrodlaPowierzchnioweData.DataType.NAPIECIE_ZAS_SONDY, model.napiecieZasilaniaSondy);
                m_data.setValue(ProtokolZrodlaPowierzchnioweData.DataType.ZAKRES_POMIAROWY, zakres);
                return true;
            }



            //**************************************************************************
            public bool PobierzDaneWarunkow(DaneWarunkowModel model, String podstawka, String odlegl_zr_sonda, String wsp_korekcyjny)
            //**************************************************************************
            {
                m_data.setValue(ProtokolZrodlaPowierzchnioweData.DataType.CISNIENIE, model.cisnienie);
                m_data.setValue(ProtokolZrodlaPowierzchnioweData.DataType.TEMPERATURA, model.temperatura);
                m_data.setValue(ProtokolZrodlaPowierzchnioweData.DataType.WILGOTNOSC, model.wilgotnosc);
                m_data.setValue(ProtokolZrodlaPowierzchnioweData.DataType.UWAGI, model.uwagi);
                m_data.setValue(ProtokolZrodlaPowierzchnioweData.DataType.PODSAWKA, podstawka);
                m_data.setValue(ProtokolZrodlaPowierzchnioweData.DataType.ODL_ZRODLA_SONDA, odlegl_zr_sonda);
                m_data.setValue(ProtokolZrodlaPowierzchnioweData.DataType.MNOZNIK_KOREKCYJNY, wsp_korekcyjny);
                return true;
            }


            //**************************************************************************
            public bool PobierzDanePomiarow(string jednostka, string uwagi, ref DataGridView tabela)
            //**************************************************************************
            {
                m_data.setValue(ProtokolZrodlaPowierzchnioweData.DataType.JEDNOSTKA, jednostka);
                
                String tabelaDoWpisania = "";

                for (int i = 0; i < tabela.Rows.Count - 1; ++i)
                {
                    tabelaDoWpisania +=
                    String.Format("<tr align=\"center\" valign=\"middle\">\n<td><span>{0}</span></td>\n<td><span>{1}</span></td></tr>",
                                   tabela.Rows[i].Cells[0].Value.ToString(), tabela.Rows[i].Cells[1].Value.ToString());
                }

                m_data.setValue(ProtokolZrodlaPowierzchnioweData.DataType.TABELA, tabelaDoWpisania);

                return true;
            }


            //**************************************************************************
            public bool PobierzDaneWspolczynnikow(DaneWspolczynnikowModel model, string wspol_kalibracyjny,
                                                  string niepewnosc_wspol_kalibracyjnego, string poprz_wspol_kalibracyjny)
            //**************************************************************************
            {
                m_data.setValue(ProtokolZrodlaPowierzchnioweData.DataType.WZORCOWAL, model.wykonal);
                m_data.setValue(ProtokolZrodlaPowierzchnioweData.DataType.SPRAWDZIL, model.sprawdzil);

                double dNiepewnosc;
                double dWspolczynnik;
                double dPopWspolczynnik;
                string format = "G";

                m_data.setValue(ProtokolZrodlaPowierzchnioweData.DataType.NIEPEWNOSC, "");
                if (N.doubleTryParse(niepewnosc_wspol_kalibracyjnego, out dNiepewnosc))
                {
                    format = Narzedzia.Precyzja.Ustaw(dNiepewnosc);
                    m_data.setValue(ProtokolZrodlaPowierzchnioweData.DataType.NIEPEWNOSC, dNiepewnosc.ToString(format));
                }

                m_data.setValue(ProtokolZrodlaPowierzchnioweData.DataType.WSPOL_KALIBRACYJNY, "");
                if (N.doubleTryParse(wspol_kalibracyjny, out dWspolczynnik))
                {
                    m_data.setValue(ProtokolZrodlaPowierzchnioweData.DataType.WSPOL_KALIBRACYJNY, dWspolczynnik.ToString(format));
                }

                m_data.setValue(ProtokolZrodlaPowierzchnioweData.DataType.NIEPEWNOSC_WZGLEDNA, "");
                if (N.doubleTryParse(niepewnosc_wspol_kalibracyjnego, out dNiepewnosc) && N.doubleTryParse(wspol_kalibracyjny, out dWspolczynnik) && dWspolczynnik != 0.0)
                {
                    double wzgledna = dNiepewnosc / dWspolczynnik * 100.0;
                    m_data.setValue(ProtokolZrodlaPowierzchnioweData.DataType.NIEPEWNOSC_WZGLEDNA, wzgledna.ToString("0.00"));
                }

                if (N.doubleTryParse(poprz_wspol_kalibracyjny, out dPopWspolczynnik))
                {
                    m_data.setValue(ProtokolZrodlaPowierzchnioweData.DataType.PO_WSPOL_KALIBRACYJNY, dPopWspolczynnik.ToString(format));
                }
                else
                {
                    m_data.setValue(ProtokolZrodlaPowierzchnioweData.DataType.PO_WSPOL_KALIBRACYJNY, poprz_wspol_kalibracyjny);
                }

                return true;
            }
        }
    }
}
