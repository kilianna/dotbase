using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Narzedzia;
using System.Windows.Forms;
using System.IO;

namespace DotBase
{
    namespace Dokumenty
    {
        class ProtokolDawka : WydrukiCez
        {
            ProtokolDawkaModel m_model;
            ProtokolDawkaData m_protokolDawkaData = new ProtokolDawkaData();

            //***************************************************
            public ProtokolDawka(ProtokolDawkaModel model)
                : base(DocumentsTemplatesFactory.TemplateType.SCIEZKA_PROTOKOL_DAWKA)
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
                           WypelnijDaneZleceniodawcy(m_templateToFill) &&
                           WypelnijDaneWzorcoweIPomiarowe();
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
                           PobierzDaneWzorcoweIPomiarowe(m_model.zrodlo, m_model.odleglosc, m_model.wspolczynnik, m_model.niepewnosc, m_model.tabela);
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
            public bool PobierzDaneWzorcoweIPomiarowe(string zrodlo, string odleglosc, string wspolczynnik, string niepewnosc, DataGridViewRowCollection tabela)
            //***************************************************
            {
                m_protokolDawkaData.setValue(ProtokolDawkaData.DataType.ZRODLO, zrodlo);
                m_protokolDawkaData.setValue(ProtokolDawkaData.DataType.ODLEGLOSC, odleglosc);

                
                StringBuilder tabelaDoWpisania = new StringBuilder();

                try
                {
                    for (int i = 0; tabela[i].Cells[0].Value != null; ++i)
                    {
                        tabelaDoWpisania.Append(String.Format("<tr align=\"center\" valign=\"middle\"><td><span>{0}</span></td>", i + 1))
                                        .Append(String.Format("<td><span>{0}</span></td> <td><span>{1}</span></td></tr>",
                                                        tabela[i].Cells["WartoscWzorcowa"].Value.ToString(),
                                                        tabela[i].Cells["Wskazanie"].Value.ToString()));
                    }
                }
                catch (Exception)
                {
                    return false;
                }


                double tempWspolczynnik, tempNiepewnosc;

                if (!Double.TryParse(wspolczynnik, out tempWspolczynnik) || !Double.TryParse(niepewnosc, out tempNiepewnosc))
                {
                    return false;
                }
                
                m_protokolDawkaData.setValue(ProtokolDawkaData.DataType.WSPOLCZYNNIK, String.Format("{0:0.000}", tempWspolczynnik));
                m_protokolDawkaData.setValue(ProtokolDawkaData.DataType.NIEPEWNOSC, String.Format("{0:0.000}", tempNiepewnosc));
                m_protokolDawkaData.setValue(ProtokolDawkaData.DataType.TABELA, tabelaDoWpisania.ToString());

                return true;
            }

            //***************************************************
            public bool WypelnijDaneWzorcoweIPomiarowe()
            //***************************************************
            {
                m_templateToFill.Replace("<!zrodlo>", m_protokolDawkaData.getValue(ProtokolDawkaData.DataType.ZRODLO))
                                .Replace("<!odleglosc>", m_protokolDawkaData.getValue(ProtokolDawkaData.DataType.ODLEGLOSC))
                                .Replace("<!wspolczynnik>", m_protokolDawkaData.getValue(ProtokolDawkaData.DataType.WSPOLCZYNNIK))
                                .Replace("<!niepewnosc>", m_protokolDawkaData.getValue(ProtokolDawkaData.DataType.NIEPEWNOSC))
                                .Replace("<!tabela>", m_protokolDawkaData.getValue(ProtokolDawkaData.DataType.TABELA));

                return true;
            }

            
        }
    }
}
