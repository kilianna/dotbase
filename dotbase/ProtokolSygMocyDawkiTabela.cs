using DotBase.Dokumenty;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DotBase
{
    namespace Dokumenty
    {
        class ProtokolSygMocyDawkiTabela : WydrukiCez
        {
            WzorcowanieSygnalizacjaMocyDawkiDataModel m_dataModel;

            public ProtokolSygMocyDawkiTabela(WzorcowanieSygnalizacjaMocyDawkiDataModel dataModel)
                : base(DocumentsTemplatesFactory.TemplateType.SCIEZKA_PROTOKOL_SYGNALIZACJA_MOCY_DAWKI_TABELA)
            {
                m_dataModel = dataModel;
            }

            //***************************************************
            public bool PobierzDaneWzorcoweIPomiarowe(string jednostka, ref DataGridView tabela)
            //***************************************************
            {
                m_templateToFill.Replace("<!jednostka>", jednostka);

                String tabelaDoWpisania = "";

                IList<double> computedFactors = SygnalizacjaMocyDawkiUtils.computeFactors(m_dataModel.modelDanePodstawowe.nrKarty);
                IList<double> computedUncertainity = SygnalizacjaMocyDawkiUtils.computeUncertainity(m_dataModel.modelDanePodstawowe.nrKarty);

                for (int i = 0; i < tabela.Rows.Count - 1; ++i)
                {
                    tabelaDoWpisania +=
                    String.Format("</tr><tr align=\"center\" valign=\"middle\"><td><span>{0}</span></td><td><span>{1} &plusmn; {2}</span></td><td><span>{3} &plusmn; {4}</span></td></tr>",
                                 tabela.Rows[i].Cells[0].Value.ToString(),
                                 tabela.Rows[i].Cells[5].Value.ToString(),
                                 tabela.Rows[i].Cells[6].Value.ToString(),
                                 computedFactors[i].ToString("0.00"),
                                 computedUncertainity[i].ToString("0.00"));
                }

                m_templateToFill.Replace("<!tabela>", tabelaDoWpisania);

                return true;
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
                    return PobierzDanePodstawowe(m_dataModel.modelDanePodstawowe) &&
                           PobierzDanePrzyrzadu(m_dataModel.modelDanePrzyrzadu) &&
                           PobierzDaneWarunkow(m_dataModel.modelDaneWarunkow) &&
                           PobierzDaneWspolczynnikow(m_dataModel.modelDaneWspolczynnikow) &&
                           PobierzDaneWzorcoweIPomiarowe(m_dataModel.jednostka, ref m_dataModel.dataGridView) &&
                           PobierzDaneZleceniodawcy(m_dataModel.modelDanePodstawowe.nrKarty);
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
        }
    }
}
