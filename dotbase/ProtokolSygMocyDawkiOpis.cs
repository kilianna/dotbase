using DotBase.Dokumenty;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DotBase
{
    namespace Dokumenty
    {
        class ProtokolSygMocyDawkiOpis : WydrukiCez
        {
            WzorcowanieSygnalizacjaMocyDawkiDataModel m_dataModel;

            public ProtokolSygMocyDawkiOpis(WzorcowanieSygnalizacjaMocyDawkiDataModel dataModel)
                : base(DocumentsTemplatesFactory.TemplateType.SCIEZKA_PROTOKOL_SYGNALIZACJA_MOCY_DAWKI_OPIS)
            {
                m_dataModel = dataModel;
            }

            public bool PobierzDaneWzorcoweIPomiarowe(string uwagi)
            {
                m_templateToFill.Replace("<!uwagiOpis>", uwagi);

                return true;
            }

            #region Document generation
            override protected bool fillDocument()
            {
                try
                {
                    return  WypelnijDanePodstawowe() &&
                            WypelnijDanePrzyrzadu() &&
                            WypelnijDaneWarunkow() &&
                            WypelnijDaneWspolczynnikow () &&
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
                           PobierzDaneWzorcoweIPomiarowe(m_dataModel.uwagiWzorcowe) &&
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
