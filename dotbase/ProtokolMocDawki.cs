using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
using Narzedzia;
using System.IO;

namespace DotBase
{
    namespace Dokumenty
    {
        class ProtokolMocDawkiData
        {
            public DaneZleceniodawcyData daneZleceniodawcyData = new DaneZleceniodawcyData();

            public enum DataType
            {
                JEDNOSTKA, TLO, WIELKOSC_FIZYCZNA, TABELA
            }

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

        class ProtokolMocDawkiModel
        {
            public DanePodstawoweModel modelDanePodstawowe;
            public DanePrzyrzaduModel modelDanePrzyrzadu;
            public DaneWarunkowModel modelDaneWarunkow;
            public DaneWspolczynnikowModel modelDaneWspolczynnikow;

            public string jednostka;
            public string wielkoscFizyczna;
            public string tlo;
            public DataGridViewRowCollection tabela;

            public ProtokolMocDawkiModel(DanePodstawoweModel modelDanePodstawowe, DanePrzyrzaduModel modelDanePrzyrzadu, DaneWarunkowModel modelDaneWarunkow, DaneWspolczynnikowModel modelDaneWspolczynnikow)
            {
                this.modelDanePodstawowe = modelDanePodstawowe;
                this.modelDanePrzyrzadu = modelDanePrzyrzadu;
                this.modelDaneWarunkow = modelDaneWarunkow;
                this.modelDaneWspolczynnikow = modelDaneWspolczynnikow;
            }
        }

        class ProtokolMocDawki : WydrukiCez
        {
            ProtokolMocDawkiData m_data = new ProtokolMocDawkiData();
            ProtokolMocDawkiModel m_model;

            //***************************************************
            public ProtokolMocDawki(ProtokolMocDawkiModel model)
                : base(DocumentsTemplatesFactory.TemplateType.SCIEZKA_PROTOKOL_MOC_DAWKI)
            //***************************************************
            {
                WczytajSzablon(StaleWzorcowan.stale.PROTOKOL_MOC_DAWKI);
                m_model = model;
            }


            #region Document generation
            override protected bool fillDocument()
            {
                try
                {
                    m_templateToFill.Replace("<!jednostka>", m_data.getValue(ProtokolMocDawkiData.DataType.JEDNOSTKA))
                                    .Replace("<!wielk_fiz>", m_data.getValue(ProtokolMocDawkiData.DataType.WIELKOSC_FIZYCZNA))
                                    .Replace("<!tlo>", m_data.getValue(ProtokolMocDawkiData.DataType.TLO))
                                    .Replace("<!tabela>", m_data.getValue(ProtokolMocDawkiData.DataType.TABELA));

                    return WypelnijDanePodstawowe() &&
                           WypelnijDanePrzyrzadu() &&
                           WypelnijDaneWarunkow() &&
                           WypelnijDaneZleceniodawcy(m_templateToFill) &&
                           WypelnijDaneWspolczynnikow();   
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
                           PobierzDaneZleceniodawcy(m_model.modelDanePodstawowe.nrKarty) &&
                           PobierzDaneWzorcoweIPomiarowe(m_model.jednostka, m_model.wielkoscFizyczna, m_model.tlo, m_model.tabela);
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
            public bool PobierzDaneWzorcoweIPomiarowe(string jednostka, string wielkoscFizyczna, string tlo,
                                                      DataGridViewRowCollection tabela)
            //***************************************************
            {

                m_data.setValue(ProtokolMocDawkiData.DataType.JEDNOSTKA, jednostka);
                m_data.setValue(ProtokolMocDawkiData.DataType.WIELKOSC_FIZYCZNA, wielkoscFizyczna);
                m_data.setValue(ProtokolMocDawkiData.DataType.TLO, tlo);
                
                
                String tabelaDoWpisania = "";

                int licznik = 1;

                double wartoscWzorcowa;

                try
                {
                    for (int i = 0; tabela[i].Cells[0].Value != null; ++i)
                    {
                        wartoscWzorcowa = ((double)tabela[i].Cells[5].Value);

                        tabelaDoWpisania
                        += String.Format("<tr align=\"center\" valign=\"middle\"><td><span>{0}</span></td>", licznik)
                        + String.Format("<td><span>{0}</span></td> <td><span>{1}</span></td> <td><span>{2}</span></td> <td><span>{3}</span></td>",
                                        tabela[i].Cells[0].Value.ToString(),
                                        tabela[i].Cells[1].Value.ToString(),
                                        wartoscWzorcowa.ToString(Precyzja.Ustaw(wartoscWzorcowa)),
                                        tabela[i].Cells[2].Value.ToString())
                        + String.Format("<td><span>{0}</span></td> <td><span>{1}</span></td> </tr>",
                                        tabela[i].Cells[3].Value.ToString(),
                                        tabela[i].Cells[4].Value.ToString());
                        ++licznik;
                    }
                }
                catch (Exception)
                {
                    return false;
                }

                m_data.setValue(ProtokolMocDawkiData.DataType.TABELA, tabelaDoWpisania);

                return true;
            }


        }
    }
}
