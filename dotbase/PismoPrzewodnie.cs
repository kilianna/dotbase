﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;

namespace DotBase
{
    namespace Dokumenty
    {

        class PismoPrzewodnie : Wydruki
        {

            class PismoPrzewodnieData
            {
                public enum DataType
                {
                    NR_KARTY, DATA_WYSTAWIENIA, DATA_WYKONANIA, DATA_MESIAC_SLOWNIE, DATA_PLUS_ROK, POPRAWA, ROK, UWAGA, NR_PISMA, ROK_PISMA, INFO_SONDY, TYP, NR_FABRYCZNY,
                    ZLECENIODAWCA, ADRES, INFO_WYKRES, INFO_WYKRES_KALIB
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

			private readonly string EVIDENCE_CORRECTION_MARKER = "P";
            PismoPrzewodnieData m_data = new PismoPrzewodnieData();
            StringBuilder m_templateToFill = DocumentsTemplatesFactory.getInstance().create(DocumentsTemplatesFactory.TemplateType.SCIEZKA_PISMO_PRZEWODNIE);
            private bool odlaczWykresMD;
            private bool odlaczWykresD;

            //****************************************************************************************
            public PismoPrzewodnie(int nrKarty, DateTime dataWystawienia, DateTime dataWykonania, string uwaga, string nrPisma, string rokPisma, bool przedluzonaWaznosc, bool poprawa, bool odlaczWykresMD, bool odlaczWykresD)
                : base(Jezyk.PL)
            //****************************************************************************************
            {
                this.odlaczWykresMD = odlaczWykresMD;
                this.odlaczWykresD = odlaczWykresD;
                _NrKarty = nrKarty.ToString();
                m_data.setValue(PismoPrzewodnieData.DataType.NR_KARTY, _NrKarty);
                m_data.setValue(PismoPrzewodnieData.DataType.DATA_WYSTAWIENIA, dataWystawienia.ToString("dd.MM.yyyy"));
				m_data.setValue(PismoPrzewodnieData.DataType.DATA_WYKONANIA, dataWykonania.ToString("dd.MM.yyyy"));
				m_data.setValue(PismoPrzewodnieData.DataType.DATA_MESIAC_SLOWNIE, dataWykonania.ToString("dd<!>MMMM yyyy").Replace("<!>", "&nbsp;"));

                if (przedluzonaWaznosc)
					m_data.setValue(PismoPrzewodnieData.DataType.DATA_PLUS_ROK, dataWykonania.AddYears(2).ToString("dd<!>MMMM yyyy").Replace("<!>", "&nbsp;"));
                else
                    m_data.setValue(PismoPrzewodnieData.DataType.DATA_PLUS_ROK, dataWykonania.AddYears(1).ToString("dd<!>MMMM yyyy").Replace("<!>", "&nbsp;"));

                m_data.setValue(PismoPrzewodnieData.DataType.ROK, dataWykonania.Year.ToString());
                m_data.setValue(PismoPrzewodnieData.DataType.UWAGA, uwaga);
                m_data.setValue(PismoPrzewodnieData.DataType.NR_PISMA, nrPisma);
                m_data.setValue(PismoPrzewodnieData.DataType.ROK_PISMA, rokPisma);
                m_data.setValue(PismoPrzewodnieData.DataType.POPRAWA, poprawa.ToString());
            }

            #region Document generation
            override protected bool fillDocument()
            {
                try
                {
					m_templateToFill.Replace("<!c1>", m_data.getValue(PismoPrzewodnieData.DataType.DATA_WYSTAWIENIA))
                        .Replace("<!data_slownie>", m_data.getValue(PismoPrzewodnieData.DataType.DATA_MESIAC_SLOWNIE))
                        .Replace("<!c2>", m_documentData.getValue(DocumentData.DataType.ZLECENIDOAWCA).Replace(";", "<br>"))
                        .Replace("<!c3>", m_documentData.getValue(DocumentData.DataType.ADRES).Replace(";", "<br>"))
                        .Replace("<!c4>", mapEvidenceIdToDisplayableForm(m_data.getValue(PismoPrzewodnieData.DataType.NR_PISMA)))
                        .Replace("<!c4rok>", m_data.getValue(PismoPrzewodnieData.DataType.ROK_PISMA))
                        .Replace("<!c5>", m_data.getValue(PismoPrzewodnieData.DataType.ROK))
                        .Replace("<!c6>", m_data.getValue(PismoPrzewodnieData.DataType.TYP))
                        .Replace("<!c7>", m_data.getValue(PismoPrzewodnieData.DataType.NR_FABRYCZNY))
						.Replace("<!c8>", mapEvidenceIdToDisplayableForm(m_data.getValue(PismoPrzewodnieData.DataType.NR_KARTY)))
                        .Replace("<!c9>", m_data.getValue(PismoPrzewodnieData.DataType.UWAGA))
                        .Replace("<!c10>", m_data.getValue(PismoPrzewodnieData.DataType.DATA_PLUS_ROK))
                        .Replace("<!sonda>", m_data.getValue(PismoPrzewodnieData.DataType.INFO_SONDY))
                        .Replace("<!c11>", m_data.getValue(PismoPrzewodnieData.DataType.INFO_WYKRES) + m_data.getValue(PismoPrzewodnieData.DataType.INFO_WYKRES_KALIB));

					return true;
                }
                catch (Exception)
                {
                    return false;
                }

            }

            override protected bool retrieveAllData()
            {
                try
                {
                    PobierzDaneZleceniodawcy(m_data.getValue(PismoPrzewodnieData.DataType.NR_KARTY));
                    PobierzDaneDozymetru();
                    PobierzDaneSond();
                    UstawUwage();
                    PobierzDodatkowaInformacjeOWykresie();

                    _Zapytanie = String.Format("SELECT COUNT(*) FROM Wzorcowanie_cezem WHERE id_karty = {0} AND rodzaj_wzorcowania = 'd'", _NrKarty);

                    m_data.setValue(PismoPrzewodnieData.DataType.INFO_WYKRES_KALIB, "");
                    if (0 != _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<int>(0))
                    {
                        UsunKomentarzeDotWzorcowaniaCezem();
                        if (!odlaczWykresD)
                        {
                            m_data.setValue(PismoPrzewodnieData.DataType.INFO_WYKRES_KALIB, "<li>Wykres kalibracyjny w zakresie dawki</li>");
                        }
                    }
                }
                catch (Exception)
                {
                    return false;
                }

                return true;
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


            //********************************************************************************************
            private void PobierzDaneDozymetru()
            //********************************************************************************************
            {
                _Zapytanie = "SELECT typ, nr_fabryczny FROM Dozymetry WHERE id_dozymetru=(SELECT id_dozymetru FROM Karta_przyjecia WHERE "
                           + String.Format("id_karty = {0})", _NrKarty);

                DataRow wiersz = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0];
                m_data.setValue(PismoPrzewodnieData.DataType.TYP, wiersz.Field<string>(0));
                m_data.setValue(PismoPrzewodnieData.DataType.NR_FABRYCZNY, wiersz.Field<string>(1));
            }

			//********************************************************************************************
			private String mapEvidenceIdToDisplayableForm(String evidenceId)
			//********************************************************************************************
			{
				if ( Boolean.Parse(m_data.getValue(PismoPrzewodnieData.DataType.POPRAWA)) )
				{
					return evidenceId + EVIDENCE_CORRECTION_MARKER; 
				}

				return evidenceId;
			}

            //********************************************************************************************
            private void PobierzDaneSond()
            //********************************************************************************************
            {
                List<KeyValuePair<string, string>> sondy = new List<KeyValuePair<string, string>>();

                var table = _BazaDanych.TworzTabeleDanych(@"
                    SELECT typ, nr_fabryczny
                    FROM Sondy
                    WHERE ID_sondy IN (SELECT ID_sondy FROM wzorcowanie_cezem WHERE ID_karty=?)
                        OR ID_sondy IN (SELECT ID_sondy FROM Wzorcowanie_zrodlami_powierzchniowymi WHERE ID_karty=?)", _NrKarty, _NrKarty);

                int licznik = 0;

                foreach (DataRow wiersz in table.Rows)
                {
                    ++licznik;
                    sondy.Add(new KeyValuePair<string, string>(wiersz.Field<string>(0), wiersz.Field<string>(1)));
                }

                String NIE_DOTYCZY = "nie dotyczy";
                String infoSondy = "";

                if (1 == licznik && NIE_DOTYCZY != sondy[0].Value)
                {
                    infoSondy = String.Format("wraz z sondą <b>{0}</b> nr&nbsp;fab. <b>{1}</b> ", sondy[0].Key.Replace(" ", "&nbsp;"), sondy[0].Value.Replace(" ", "&nbsp;"));
                }
                else if (1 == licznik && NIE_DOTYCZY == sondy[0].Value)
                {
                    infoSondy = "";
                }
                else if ((NIE_DOTYCZY == sondy[0].Value ^ NIE_DOTYCZY == sondy[1].Value) && licznik == 2)
                {
                    if (sondy[0].Value != NIE_DOTYCZY)
                        infoSondy = String.Format("wraz z sondą <b>{0}</b> nr&nbsp;fab. <b>{1}</b> ", sondy[0].Key.Replace(" ", "&nbsp;"), sondy[0].Value.Replace(" ", "&nbsp;"));
                    else
                        infoSondy = String.Format("wraz z sondą <b>{0}</b> nr&nbsp;fab. <b>{1}</b> ", sondy[1].Key.Replace(" ", "&nbsp;"), sondy[1].Value.Replace(" ", "&nbsp;"));
                }
                else if (2 == licznik && NIE_DOTYCZY != sondy[0].Value && NIE_DOTYCZY != sondy[1].Value)
                {
                    infoSondy = String.Format("wraz z sondami <b>{0}</b> nr&nbsp;fab. <b>{1}</b> i <b>{2}</b> nr&nbsp;fab. <b>{3}</b> ",
                                sondy[0].Key.Replace(" ", "&nbsp;"), sondy[0].Value.Replace(" ", "&nbsp;"), sondy[1].Key.Replace(" ", "&nbsp;"), sondy[1].Value.Replace(" ", "&nbsp;"));
                }
                else if (2 < licznik)
                {
                    int k;
                    for (k = 0; k < licznik; ++k)
                    {
                        if (NIE_DOTYCZY == sondy[k].Value)
                            break;
                    }

                    List<String> tekstySond = new List<String>();

                    for (int p = 0; p < licznik; ++p)
                    {
                        if (p != k)
                        {
                            tekstySond.Add(String.Format("<b>{0}</b> nr&nbsp;fab. <b>{1}</b>", sondy[p].Key.Replace(" ", "&nbsp;"), sondy[p].Value.Replace(" ", "&nbsp;")));
                        }
                    }
                    
                    infoSondy += "wraz z sondami " + String.Join(", ", tekstySond);
                }

                m_data.setValue(PismoPrzewodnieData.DataType.INFO_SONDY, infoSondy);
            }

            //********************************************************************************************
            private void PobierzDaneZleceniodawcy()
            //********************************************************************************************
            {
                _Zapytanie = "SELECT Zleceniodawca, Adres FROM Zleceniodawca WHERE id_zleceniodawcy=(SELECT id_zleceniodawcy FROM Zlecenia WHERE "
                           + String.Format("id_zlecenia=(SELECT id_zlecenia FROM Karta_przyjecia WHERE id_karty = {0}))",
                           m_data.getValue(PismoPrzewodnieData.DataType.NR_KARTY));

                DataRow wiersz = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0];

                m_data.setValue(PismoPrzewodnieData.DataType.ZLECENIODAWCA, wiersz.Field<string>(0).Replace(";", "<br>"));
                m_data.setValue(PismoPrzewodnieData.DataType.ADRES, wiersz.Field<string>(1).Replace(";", "<br>"));
            }

            //********************************************************************************************
            private void PobierzDodatkowaInformacjeOWykresie()
            //********************************************************************************************
            {
                _Zapytanie = String.Format("SELECT COUNT(*) FROM Wzorcowanie_cezem WHERE rodzaj_wzorcowania='md' AND id_karty = {0}", _NrKarty);

                m_data.setValue(PismoPrzewodnieData.DataType.INFO_WYKRES, "");
                if (0 != _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<int>(0))
                {
                    UsunKomentarzeDotWzorcowaniaCezem();
                    if (!odlaczWykresMD)
                    {
                        m_data.setValue(PismoPrzewodnieData.DataType.INFO_WYKRES, "<li>Wykres kalibracyjny w zakresie mocy dawki</li>");
                    }
                }

                _Zapytanie = String.Format("SELECT COUNT(*) FROM Wzorcowanie_cezem WHERE rodzaj_wzorcowania IN('sd', 'sm') AND id_karty = {0}", _NrKarty);

                if (0 != _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<int>(0))
                {
                    UsunKomentarzeDotWzorcowaniaCezem();
                }
            }

            //********************************************************************************************
            private void UstawUwage()
            //********************************************************************************************
            {
                if ("" != m_data.getValue(PismoPrzewodnieData.DataType.UWAGA))
                    m_data.setValue(PismoPrzewodnieData.DataType.UWAGA, "Uwagi: " + m_data.getValue(PismoPrzewodnieData.DataType.UWAGA));
            }

            //********************************************************************************************
            void UsunKomentarzeDotWzorcowaniaCezem()
            //********************************************************************************************
            {
                m_templateToFill.Replace("<!--", "").Replace("-->", "");
            }
        }
    }
}
