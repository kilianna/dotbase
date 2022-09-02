using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Narzedzia;
using System.Data;
using System.Windows.Forms;
using System.IO;

namespace DotBase
{
    namespace Dokumenty
    {
        abstract class WydrukiCez : Wydruki
        {
            protected StringBuilder m_templateToFill;
            private WydrukiCezData m_data;

            //***************************************************
            public WydrukiCez(DocumentsTemplatesFactory.TemplateType type) : base(Jezyk.PL)
            //***************************************************
            {
                _BazaDanych = new BazaDanychWrapper();
                m_templateToFill = DocumentsTemplatesFactory.getInstance().create(type);
                m_data = new WydrukiCezData();
            }

            //***************************************************
            public bool PobierzDanePodstawowe(DanePodstawoweModel model)
            //***************************************************
            {
                m_data.setValue(WydrukiCezData.DataType.NR_KARTY, model.nrKarty);
                m_data.setValue(WydrukiCezData.DataType.NR_ARKUSZA, model.nrArkusza);
                m_data.setValue(WydrukiCezData.DataType.DATA, model.data.ToShortDateString());
                m_data.setValue(WydrukiCezData.DataType.ROK, model.data.Year.ToString());
                return true;
            }

            //***************************************************
            public bool WypelnijDanePodstawowe()
            //***************************************************
            {
                m_templateToFill.Replace("<!nrKarty>", m_data.getValue(WydrukiCezData.DataType.NR_KARTY))
                                .Replace("<!nrArkusza>", m_data.getValue(WydrukiCezData.DataType.NR_ARKUSZA))
                                .Replace("<!data>", m_data.getValue(WydrukiCezData.DataType.DATA))
                                .Replace("<!rok>", m_data.getValue(WydrukiCezData.DataType.ROK));
                return true;
            }

            //***************************************************
            public bool PobierzDanePrzyrzadu(DanePrzyrzaduModel model)
            //***************************************************
            {
                string zapytanie;
                string id_dozymetru;
                string id_sondy;

                try
                {
                    zapytanie = String.Format("SELECT id_dozymetru FROM Dozymetry WHERE typ = '{0}' AND nr_fabryczny = '{1}'",
                                              model.typ, model.nrFabryczny);
                    id_dozymetru = _BazaDanych.TworzTabeleDanych(zapytanie).Rows[0].Field<int>(0).ToString();
                    zapytanie = String.Format("SELECT id_sondy FROM Sondy WHERE typ = '{0}' AND nr_fabryczny = '{1}' AND id_dozymetru = {2}",
                                              model.sondaTyp, model.sondaNrFabryczny, id_dozymetru);
                    id_sondy = _BazaDanych.TworzTabeleDanych(zapytanie).Rows[0].Field<int>(0).ToString();
                }
                catch (Exception)
                {
                    return false;
                }

                m_data.setValue(WydrukiCezData.DataType.DOZYMETR_ID, id_dozymetru);
                m_data.setValue(WydrukiCezData.DataType.SONDA_ID, id_sondy);
                m_data.setValue(WydrukiCezData.DataType.DOZYMETR_TYP, model.typ);
                m_data.setValue(WydrukiCezData.DataType.DOZYMETR_NR_FAB, model.nrFabryczny);
                m_data.setValue(WydrukiCezData.DataType.INNE_NASTAWY, model.inneNastawy);
                m_data.setValue(WydrukiCezData.DataType.SONDA_TYP, model.sondaTyp);
                m_data.setValue(WydrukiCezData.DataType.SONDA_NR_FAB, model.sondaNrFabryczny);
                m_data.setValue(WydrukiCezData.DataType.NAP_ZAS_SONDY, model.napiecieZasilaniaSondy);
                
                return true;
            }

            public bool WypelnijDanePrzyrzadu()
            {
                m_templateToFill.Replace("<!dozymetr_id>", m_data.getValue(WydrukiCezData.DataType.DOZYMETR_ID))
                                .Replace("<!sonda_id>", m_data.getValue(WydrukiCezData.DataType.SONDA_ID))
                                .Replace("<!dozymetr_typ>", m_data.getValue(WydrukiCezData.DataType.DOZYMETR_TYP))
                                .Replace("<!dozymetr_NrFab>", m_data.getValue(WydrukiCezData.DataType.DOZYMETR_NR_FAB))
                                .Replace("<!inne_nastawy>", m_data.getValue(WydrukiCezData.DataType.INNE_NASTAWY))
                                .Replace("<!sonda_typ>", m_data.getValue(WydrukiCezData.DataType.SONDA_TYP))
                                .Replace("<!sonda_NrFab>", m_data.getValue(WydrukiCezData.DataType.SONDA_NR_FAB))
                                .Replace("<!nap_zas_sondy>", m_data.getValue(WydrukiCezData.DataType.NAP_ZAS_SONDY));
                return true;
            }

            //***************************************************
            public bool PobierzDaneWarunkow(DaneWarunkowModel model)
            //***************************************************
            {
                m_data.setValue(WydrukiCezData.DataType.CISNIENIE, model.cisnienie);
                m_data.setValue(WydrukiCezData.DataType.TEMPERATURA, model.temperatura);
                m_data.setValue(WydrukiCezData.DataType.WILGOTNOSC, model.wilgotnosc);
                m_data.setValue(WydrukiCezData.DataType.UWAGI, model.uwagi);

                return true;
            }

            //***************************************************
            public bool WypelnijDaneWarunkow()
            //***************************************************
            {
                m_templateToFill.Replace("<!cisnienie>", m_data.getValue(WydrukiCezData.DataType.CISNIENIE))
                                .Replace("<!temperatura>", m_data.getValue(WydrukiCezData.DataType.TEMPERATURA))
                                .Replace("<!wilgotnosc>", m_data.getValue(WydrukiCezData.DataType.WILGOTNOSC))
                                .Replace("<!uwagi>", m_data.getValue(WydrukiCezData.DataType.UWAGI));

                return true;
            }
            

            //***************************************************
            public bool PobierzDaneWspolczynnikow(DaneWspolczynnikowModel model)
            //***************************************************
            {
                m_data.setValue(WydrukiCezData.DataType.WYKONAL, model.wykonal);
                m_data.setValue(WydrukiCezData.DataType.SPRAWDZIL, model.sprawdzil);
                return true;
            }

            //***************************************************
            public bool WypelnijDaneWspolczynnikow()
            //***************************************************
            {
                m_templateToFill.Replace("<!wykonal>", m_data.getValue(WydrukiCezData.DataType.WYKONAL))
                                .Replace("<!sprawdzil>", m_data.getValue(WydrukiCezData.DataType.SPRAWDZIL));
                return true;
            }
        }
    }
}
