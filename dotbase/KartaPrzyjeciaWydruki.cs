using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Narzedzia;
using System.IO;

namespace DotBase
{
    namespace Dokumenty
    {
        class KartaPrzyjecia : Wydruki
        {
            StringBuilder templateToFill;

            public KartaPrzyjecia(string idKarty) : base(idKarty)
            {
                templateToFill = DocumentsTemplatesFactory.getInstance().create(DocumentsTemplatesFactory.TemplateType.SCIEZKA_KARTA_PRZYJECIA);
            }

            #region Document generation
            override protected bool fillDocument()
            {
                try
                {
                    templateToFill = templateToFill.Replace("<!c1>", m_documentData.getValue(DocumentData.DataType.ID_KARTY));
                    templateToFill = templateToFill.Replace("<!c2>", m_documentData.getValue(DocumentData.DataType.ROK));
                    templateToFill = templateToFill.Replace("<!c3>", m_documentData.getValue(DocumentData.DataType.ZLECENIDOAWCA).Replace(";", "<br>"));
                    templateToFill = templateToFill.Replace("<!c4>", m_documentData.getValue(DocumentData.DataType.ADRES).Replace(";", "<br>"));
                    templateToFill = templateToFill.Replace("<!c5>", m_documentData.getValue(DocumentData.DataType.OSOBA_KONTAKTOWA));
                    templateToFill = templateToFill.Replace("<!c6>", m_documentData.getValue(DocumentData.DataType.ID_ZLECENIODAWCY));
                    templateToFill = templateToFill.Replace("<!c7>", m_documentData.getValue(DocumentData.DataType.TELEFON));
                    templateToFill = templateToFill.Replace("<!c8>", m_documentData.getValue(DocumentData.DataType.FAKS));
                    templateToFill = templateToFill.Replace("<!c9>", m_documentData.getValue(DocumentData.DataType.EMAIL));
                    templateToFill = templateToFill.Replace("<!c10>", m_documentData.getValue(DocumentData.DataType.DOZYMETR_TYP));
                    templateToFill = templateToFill.Replace("<!c11>", m_documentData.getValue(DocumentData.DataType.DOZYMETR_NR_FABRYCZNY));
                    templateToFill = templateToFill.Replace("<!c12>", m_documentData.getValue(DocumentData.DataType.DOZYMETR_ID));
                    templateToFill = templateToFill.Replace("<!tabela>", m_documentData.getValue(DocumentData.DataType.TABELA));
                    templateToFill = templateToFill.Replace("<!c13>", m_documentData.getValue(DocumentData.DataType.AKCESORIA));

                    /* Usuwamy ostatnią spację i przecinek z listy wymagań*/
                    String temp = m_documentData.getValue(DocumentData.DataType.WYMAGANIA);
                    temp = temp.Remove(temp.Length - 2);
                    templateToFill = templateToFill.Replace("<!c14>", temp);

                    templateToFill = templateToFill.Replace("<!c15>", m_documentData.getValue(DocumentData.DataType.DATA_PRZYJECIA));
                    templateToFill = templateToFill.Replace("<!c16>", m_documentData.getValue(DocumentData.DataType.DATA_ZWROTU));
                    templateToFill = templateToFill.Replace("<!c17>", m_documentData.getValue(DocumentData.DataType.TEST_NA_SKAZENIA));
                    templateToFill = templateToFill.Replace("<!c18>", m_documentData.getValue(DocumentData.DataType.OSOBA_PRZYJMUJACA));
                    templateToFill = templateToFill.Replace("<!c19>", m_documentData.getValue(DocumentData.DataType.UWAGI));
                }
                catch (Exception)
                {
                    return false;
                }

                return true;
            }

            override protected bool retrieveAllData()
            {
                try
                {
                    PobierzRok();
                    PobierzDaneZleceniodawcy(_NrKarty);
                    PobierzDaneDozymetru();
                    PobierzDaneSondy();
                    PobierzDaneDotyczaceWymagan();
                    PobierzEkspres();
                    PobierzDatyZlecenia();
                    PobierzTestyNaSkazenia();
                    PobierzUwagiOrazOsobe();
                    PobierzDaneAkcesoriow();
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
                        streamWriter.Write(templateToFill.ToString());
                    }
                }
                catch (Exception)
                {
                    return false;
                }

                return true;
            }
            #endregion

            #region PobieranieDanych

            //****************************************************************************************
            private void PobierzDaneAkcesoriow()
            //****************************************************************************************
            {
                // pobranie informacji na temat akcesoriów
                _Zapytanie = String.Format("SELECT Akcesoria FROM Karta_przyjecia WHERE id_karty={0}", _NrKarty);
                m_documentData.setValue(DocumentData.DataType.AKCESORIA, _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<string>(0));
            }

            //****************************************************************************************
            private void PobierzUwagiOrazOsobe()
            //****************************************************************************************
            {
                _Zapytanie = String.Format("SELECT osoba_przyjmujaca FROM Zlecenia WHERE id_zlecenia = {0}", m_documentData.getValue(DocumentData.DataType.ID_ZLECENIA));
                DataRow wiersz = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0];
                m_documentData.setValue(DocumentData.DataType.OSOBA_PRZYJMUJACA, wiersz.Field<string>(0));

                _Zapytanie = String.Format("SELECT uwagi FROM Karta_Przyjecia WHERE id_karty = {0}", m_documentData.getValue(DocumentData.DataType.ID_KARTY));
                wiersz = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0];
                m_documentData.setValue(DocumentData.DataType.UWAGI, wiersz.Field<string>(0));
            }

            //****************************************************************************************
            private void PobierzTestyNaSkazenia()
            //****************************************************************************************
            {
                _Zapytanie = String.Format("SELECT test_na_skazenia FROM Karta_przyjecia WHERE id_karty = {0}", _NrKarty);
                m_documentData.setValue(DocumentData.DataType.TEST_NA_SKAZENIA, _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<string>(0).ToString());
            }

            //****************************************************************************************
            private void PobierzDatyZlecenia()
            //****************************************************************************************
            {
                _Zapytanie = String.Format("SELECT data_przyjecia, data_zwrotu FROM Zlecenia WHERE id_zlecenia={0}", m_documentData.getValue(DocumentData.DataType.ID_ZLECENIA));
                DataRow wiersz = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0];

                m_documentData.setValue(DocumentData.DataType.DATA_PRZYJECIA, wiersz.Field<DateTime>(0).ToShortDateString());
                m_documentData.setValue(DocumentData.DataType.DATA_ZWROTU, wiersz.Field<DateTime>(1).ToShortDateString());
            }

            //****************************************************************************************
            private void PobierzEkspres()
            //****************************************************************************************
            {
                _Zapytanie = String.Format("SELECT ekspres FROM Zlecenia WHERE id_zlecenia={0}", m_documentData.getValue(DocumentData.DataType.ID_ZLECENIA));
                m_documentData.setValue(DocumentData.DataType.EKSPRES, _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<bool>(0).ToString());
            }

            //****************************************************************************************
            private void PobierzDaneDotyczaceWymagan()
            //****************************************************************************************
            {
                // pobranie inf. na temat wymagań
                _Zapytanie = "SELECT moc_dawki, dawka, syg_dawki, syg_mocy_dawki, stront_slaby, stront_silny, wegiel_slaby, wegiel_silny, "
                           + String.Format("ameryk, pluton, chlor FROM Karta_przyjecia WHERE id_karty = {0}", _NrKarty);
                DataRow wiersz = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0];

                StringBuilder temp = new StringBuilder(256);

                string[] nazwy = new string[11]{"moc dawki Cs-137", "dawka", "sygnalizacja dawki", "sygnalizacja mocy dawki", "Sr-90/Y-90",
                                            "Sr-90/Y-90 (silny)","C-14", "C-14 (silny)", "Am-241", "Pu-239", "Cl-36"};
                for (int i = 1; i <= 11; ++i)
                {
                    if (false != wiersz.Field<bool>(i - 1))
                    {
                        temp.Append(String.Format("{0}, ", nazwy[i - 1]));
                    }
                }

                m_documentData.setValue(DocumentData.DataType.WYMAGANIA, temp.ToString());
            }

            //****************************************************************************************
            private void PobierzDaneSondy()
            //****************************************************************************************
            {
                _Zapytanie = "SELECT typ, nr_fabryczny, S.id_sondy FROM Sondy AS S INNER JOIN Karta_przyjecia AS K ON S.id_dozymetru="
                           + String.Format("K.id_dozymetru WHERE K.id_karty = {0}", _NrKarty);


                StringBuilder tableBuilder = new StringBuilder(256);
                char licznik = '1';

                foreach (DataRow row in _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows)
                {
                    tableBuilder.Append(String.Format("<tr align=\"center\" valign=\"middle\"><td><span>{0}</span></td><td><span>{1}</span></td>", licznik.ToString(), row.Field<string>(0)));
                    tableBuilder.Append(String.Format("<td><span>{0}</span></td><td><span>{1}</span></td></tr>", row.Field<string>(1), row.Field<int>(2).ToString()));

                    ++licznik;
                }

                m_documentData.setValue(DocumentData.DataType.TABELA, tableBuilder.ToString());
            }

            //****************************************************************************************
            private void PobierzDaneDozymetru()
            //****************************************************************************************
            {
                _Zapytanie = "SELECT typ, nr_fabryczny, K.id_dozymetru FROM Dozymetry AS D INNER JOIN Karta_przyjecia AS K ON "
                           + String.Format("D.id_dozymetru=K.id_dozymetru WHERE K.id_karty = {0}", _NrKarty);
                DataRow wiersz = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0];

                // dodanie typu
                m_documentData.setValue(DocumentData.DataType.DOZYMETR_TYP, wiersz.Field<string>("typ"));
                // dodanie typu
                m_documentData.setValue(DocumentData.DataType.DOZYMETR_NR_FABRYCZNY, wiersz.Field<string>("nr_fabryczny"));
                // dodanie id dozymetru
                m_documentData.setValue(DocumentData.DataType.DOZYMETR_ID, wiersz.Field<int>("id_dozymetru").ToString());
            }

            //****************************************************************************************
            private void PobierzRok()
            //****************************************************************************************
            {
                // dodanie roku
                _Zapytanie = String.Format("SELECT rok FROM Karta_przyjecia WHERE id_karty = {0}", _NrKarty);
                m_documentData.setValue(DocumentData.DataType.ROK, _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<int>(0).ToString());
            }

            #endregion
        }
    }
}
