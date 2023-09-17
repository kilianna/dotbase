using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using Narzedzia;
using System.IO;

namespace DotBase
{
    namespace Dokumenty
    {
        class ProtokolSygnalizacjaDawkiModel
        {
            public DanePodstawoweModel modelDanePodstawowe;
            public DanePrzyrzaduModel modelDanePrzyrzadu;
            public DaneWarunkowModel modelDaneWarunkow;
            public DaneWspolczynnikowModel modelDaneWspolczynnikow;

            public string odleglosc;
            public string zrodlo;
            public string jednostka;
            public DataGridView tabela;

            public ProtokolSygnalizacjaDawkiModel(DanePodstawoweModel modelDanePodstawowe, DanePrzyrzaduModel modelDanePrzyrzadu, DaneWarunkowModel modelDaneWarunkow, DaneWspolczynnikowModel modelDaneWspolczynnikow)
            {
                this.modelDanePodstawowe = modelDanePodstawowe;
                this.modelDanePrzyrzadu = modelDanePrzyrzadu;
                this.modelDaneWarunkow = modelDaneWarunkow;
                this.modelDaneWspolczynnikow = modelDaneWspolczynnikow;
            }
        }

        class ProtokolSygnalizacjaDawkiData
        {
            public enum DataType
            {
                ZRODLO, ODLEGLOSC, JEDNOSTKA, TABELA
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

        class ProtokolSygnalizacjaDawki : WydrukiCez
        {
            ProtokolSygnalizacjaDawkiData m_data = new ProtokolSygnalizacjaDawkiData();
            ProtokolSygnalizacjaDawkiModel m_model;

            //***************************************************
            public ProtokolSygnalizacjaDawki(ProtokolSygnalizacjaDawkiModel model)
                : base(DocumentsTemplatesFactory.TemplateType.SCIEZKA_PROTOKOL_SYGNALIZACJA_DAWKI)
            //***************************************************
            {
                m_model = model;
            }

            #region Document generation
            override protected bool fillDocument()
            {
                try
                {
                    return WypelnijDanePodstawowe() &&
                           WypelnijDanePrzyrzadu() &&
                           WypelnijDaneWarunkow() &&
                           WypelnijDaneWspolczynnikow() &&
                           WyplenijDaneWzorcoweIPomiarowe() &&
                           WypelnijDaneZleceniodawcy(m_templateToFill);
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
                    return PobierzDanePodstawowe(m_model.modelDanePodstawowe) &&
                           PobierzDanePrzyrzadu(m_model.modelDanePrzyrzadu) &&
                           PobierzDaneWarunkow(m_model.modelDaneWarunkow) &&
                           PobierzDaneWspolczynnikow(m_model.modelDaneWspolczynnikow) &&
                           PobierzDaneWzorcoweIPomiarowe(m_model.odleglosc, m_model.zrodlo, m_model.jednostka, ref m_model.tabela) &&
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

            //***************************************************
            public bool PobierzDaneWzorcoweIPomiarowe(string odleglosc, string zrodlo, string jednostka, ref DataGridView tabela)
            //***************************************************
            {
                m_data.setValue(ProtokolSygnalizacjaDawkiData.DataType.ODLEGLOSC, odleglosc);
                m_data.setValue(ProtokolSygnalizacjaDawkiData.DataType.ZRODLO, zrodlo);
                m_data.setValue(ProtokolSygnalizacjaDawkiData.DataType.JEDNOSTKA, jednostka.Replace("/h", "").Replace("/s", "").Replace("u", "&mu;"));

                StringBuilder tabelaDoWpisania = new StringBuilder();

                IList<double> computedFactors = null;
                IList<double> computedUncertainity = null;

                if (!N.proceduraOd20230915(m_model.modelDanePodstawowe.data))
                {
                    computedFactors = SygnalizacjaDawkiUtils.computeFactors(m_model.modelDanePodstawowe.nrKarty);
                    computedUncertainity = SygnalizacjaDawkiUtils.computeUncertainity(m_model.modelDanePodstawowe.nrKarty);
                }

                for (int i = 0; i < tabela.Rows.Count - 1; ++i)
                {
                    tabelaDoWpisania.Append(
                    String.Format("</tr><tr align=\"center\" valign=\"middle\"><td><span>{0}</span></td><td><span>{1} &plusmn; {2}</span></td><td><span>{3} &plusmn; {4} </span></td></tr>",
                                  tabela.Rows[i].Cells[0].Value.ToString(),
                                  tabela.Rows[i].Cells[3].Value.ToString(),
                                  tabela.Rows[i].Cells[4].Value.ToString(),
                                  computedFactors != null ? computedFactors[i].ToString("0.00") : tabela.Rows[i].Cells[6].Value.ToString(),
                                  computedFactors != null ? computedUncertainity[i].ToString("0.00") : tabela.Rows[i].Cells[7].Value.ToString()));
                }

                m_data.setValue(ProtokolSygnalizacjaDawkiData.DataType.TABELA, tabelaDoWpisania.ToString());

                return true;
            }

            public bool WyplenijDaneWzorcoweIPomiarowe()
            {
                m_templateToFill.Replace("<!zrodlo>", m_data.getValue(ProtokolSygnalizacjaDawkiData.DataType.ZRODLO))
                                  .Replace("<!jednostka>", m_data.getValue(ProtokolSygnalizacjaDawkiData.DataType.JEDNOSTKA))
                                  .Replace("<!odleglosc>", m_data.getValue(ProtokolSygnalizacjaDawkiData.DataType.ODLEGLOSC))
                                  .Replace("<!tabela>", m_data.getValue(ProtokolSygnalizacjaDawkiData.DataType.TABELA));

                return true;
            }
        }
    }
}
