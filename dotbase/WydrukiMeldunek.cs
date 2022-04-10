using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Data;

namespace DotBase
{
    class WydrukiMeldunek : Dokumenty.Wydruki
    {
        class WydrukiMeldunekData
        {
            public enum DataType
            {
                DATA, NR_SWIADECTWA, DATA_SPRZEDAZY, TYP, NUMERY, CENA, LICZBA_SZTUK, NIP, KOSZT, ZLECENIODAWCA, ADRES_ZLECENIODAWCY, LISTA, NAZWA_PLATNIKA, ADRES_PLATNIKA, NR_ZLECENIA_PLATNIKA
            };

            private readonly IDictionary<DataType, String> m_documentData = new Dictionary<DataType, String>();

            public string getValue(DataType dataType)
            {
                return m_documentData[dataType];
            }

            public void setValue(DataType dataType, string value)
            {
                m_documentData.Add(dataType, value);
            }
        }

        MeldunekModel m_meldunekModel;
        WydrukiMeldunekData m_wydrukiMeldunekData = new WydrukiMeldunekData();

        public WydrukiMeldunek(MeldunekModel meldunekModel)
        {
            m_meldunekModel = meldunekModel;
            WczytajSzablon(Narzedzia.StaleWzorcowan.stale.MELDUNEK);
        }

        override protected bool fillDocument()
        {
            _SzablonPodstawowy.Replace("<!data>", m_wydrukiMeldunekData.getValue(WydrukiMeldunekData.DataType.DATA));
            _SzablonPodstawowy.Replace("<!numer_swiadectwa>", m_wydrukiMeldunekData.getValue(WydrukiMeldunekData.DataType.NR_SWIADECTWA));
            _SzablonPodstawowy.Replace("<!data_sprzedazy>", m_wydrukiMeldunekData.getValue(WydrukiMeldunekData.DataType.DATA_SPRZEDAZY));
            _SzablonPodstawowy.Replace("<!lista>", m_wydrukiMeldunekData.getValue(WydrukiMeldunekData.DataType.LISTA));

            _SzablonPodstawowy.Replace("<!zleceniodawca>", m_wydrukiMeldunekData.getValue(WydrukiMeldunekData.DataType.ZLECENIODAWCA));
            _SzablonPodstawowy.Replace("<!nip>", m_wydrukiMeldunekData.getValue(WydrukiMeldunekData.DataType.NIP));
            _SzablonPodstawowy.Replace("<!adres>", m_wydrukiMeldunekData.getValue(WydrukiMeldunekData.DataType.ADRES_ZLECENIODAWCY));
            _SzablonPodstawowy.Replace("<!cena>", m_wydrukiMeldunekData.getValue(WydrukiMeldunekData.DataType.CENA));

            _SzablonPodstawowy.Replace("<!nazwaPlatnika>", m_wydrukiMeldunekData.getValue(WydrukiMeldunekData.DataType.NAZWA_PLATNIKA));
            _SzablonPodstawowy.Replace("<!adresPlatnika>", m_wydrukiMeldunekData.getValue(WydrukiMeldunekData.DataType.ADRES_PLATNIKA));
            _SzablonPodstawowy.Replace("<!nrZleceniaKlienta>", m_wydrukiMeldunekData.getValue(WydrukiMeldunekData.DataType.NR_ZLECENIA_PLATNIKA));

            if (m_meldunekModel.innyPlatnik)
            {
                _SzablonPodstawowy.Replace("<!platnikBegin>", "");
                _SzablonPodstawowy.Replace("<!platnikEnd>", "");
            }
            else
            {
                _SzablonPodstawowy.Replace("<!platnikBegin>", "<!--");
                _SzablonPodstawowy.Replace("<!platnikEnd>", "-->");
            }

            return true;
        }

        override protected bool retrieveAllData()
        {
            IList<DateTime> daty = new List<DateTime>();
            foreach (String nrKarty in m_meldunekModel.nrKart) 
            {
                _Zapytanie = "SELECT data_wystawienia FROM Swiadectwo WHERE id_karty=" + nrKarty;
                daty.Add(_BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<DateTime>(0));
            }

            DateTime maxDate = (from data in daty select data).Max();
            m_wydrukiMeldunekData.setValue(WydrukiMeldunekData.DataType.DATA_SPRZEDAZY, maxDate.ToString("dd\\/MM\\/yyyy"));

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < m_meldunekModel.nrKart.Count; ++i)
            {
                sb.Append(String.Format("{0}/{1}", m_meldunekModel.nrKart[i], maxDate.Year));
                if (i + 1 < m_meldunekModel.nrKart.Count)
                {
                    sb.Append(", ");
                }
            }

            m_wydrukiMeldunekData.setValue(WydrukiMeldunekData.DataType.DATA, System.DateTime.Today.ToString("dd\\/MM\\/yyyy"));
            m_wydrukiMeldunekData.setValue(WydrukiMeldunekData.DataType.NIP, m_meldunekModel.nip);
            m_wydrukiMeldunekData.setValue(WydrukiMeldunekData.DataType.ZLECENIODAWCA, m_meldunekModel.zleceniodawca.Replace(";", "<br>"));
            m_wydrukiMeldunekData.setValue(WydrukiMeldunekData.DataType.ADRES_ZLECENIODAWCY, m_meldunekModel.adresZleceniodawcy.Replace(";", "<br>"));
            m_wydrukiMeldunekData.setValue(WydrukiMeldunekData.DataType.NAZWA_PLATNIKA, m_meldunekModel.nazwaPlatnika.Replace(";", "<br>"));
            m_wydrukiMeldunekData.setValue(WydrukiMeldunekData.DataType.ADRES_PLATNIKA, m_meldunekModel.adresPlatnika.Replace(";", "<br>"));
            m_wydrukiMeldunekData.setValue(WydrukiMeldunekData.DataType.NR_ZLECENIA_PLATNIKA, m_meldunekModel.nrZleceniaKlienta.Replace(";", "<br>"));
            StringBuilder sb2 = new StringBuilder();
            foreach (String nrKarty in m_meldunekModel.nrKart)
            {
                sb2.Append(String.Format("{0}/{1}, ", nrKarty, maxDate.Year));
            }
            sb2.Remove(sb2.Length - 2, 2);

            m_wydrukiMeldunekData.setValue(WydrukiMeldunekData.DataType.NR_SWIADECTWA, sb2.ToString());

            FillPrices();


            return true;
        }

        private void FillPrices()
        {
            MultiMap<String, KeyValuePair<String, Double>> typNumerFabryczny = new MultiMap<String, KeyValuePair<String, Double>>();
            foreach (String nrKarty in m_meldunekModel.nrKart)
            {
                // TODO - warunek
                _Zapytanie = String.Format("SELECT DISTINCT typ, nr_fabryczny, cena FROM Dozymetry " +
                                           "INNER JOIN Karta_przyjecia ON Dozymetry.id_dozymetru = Karta_przyjecia.id_dozymetru " +
                                           "WHERE Karta_przyjecia.id_dozymetru = (SELECT id_dozymetru FROM Karta_przyjecia WHERE id_karty = {0}) " +
                                           "AND id_karty = {0}", nrKarty);

                System.Data.DataTable results = _BazaDanych.TworzTabeleDanych(_Zapytanie);

                KeyValuePair<String, Double> temp = new KeyValuePair<String, Double>(results.Rows[0].Field<String>(1), results.Rows[0].Field<Double>(2));
                typNumerFabryczny.Add(results.Rows[0].Field<String>(0), temp);
            }
            
			
			//po <!cena>, <!liczba_sztuk> szt.</div>

            StringBuilder sb = new StringBuilder();
            int counter = 1;
            foreach (String typ in typNumerFabryczny.Keys)
            {
                sb.Append(String.Format("<div class = \"kolumna1\">{0}). Radiometr typu {1} nr ", counter, typ));

                IList<String> types = new List<String>();
                IDictionary<double, int> ceny = new Dictionary<double, int>();
                foreach (KeyValuePair<String, Double> data in typNumerFabryczny[typ])
                {
                    types.Add(data.Key);
                    if (!ceny.ContainsKey(data.Value))
                        ceny.Add(data.Value, 1);
                    else
                        ceny[data.Value] += 1;
                }
                sb.Append(String.Join(", ", types.ToArray())).Append("</div>\n");

                sb.Append("<div class = \"kolumna2\">");
                foreach (KeyValuePair<double, int> sztukiCena in ceny)
                {
                    sb.Append(String.Format("po {0} - {1} szt.<br>", sztukiCena.Key, sztukiCena.Value));
                }
                sb.Remove(sb.Length - 4, 4);
                sb.Append("</div>\n");

                ++counter;
            }

            double sum = 0;
            foreach (String typ in typNumerFabryczny.Keys)
            {
                foreach (KeyValuePair<string, double> value in typNumerFabryczny[typ])
                    sum += value.Value;
            }

            m_wydrukiMeldunekData.setValue(WydrukiMeldunekData.DataType.CENA, sum.ToString("0.00"));
            m_wydrukiMeldunekData.setValue(WydrukiMeldunekData.DataType.LISTA, sb.ToString());
        }

        override protected bool saveDocument(string path)
        {
            try
            {
                using (StreamWriter streamWriter = new StreamWriter(path, false, Encoding.UTF8))
                {
                    streamWriter.Write(_SzablonPodstawowy.ToString());
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
