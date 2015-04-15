using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotBase
{
    class WydrukiCezData
    {
        public enum DataType
        {
            NR_KARTY, NR_ARKUSZA, DATA, ROK,
            DOZYMETR_ID, SONDA_ID, DOZYMETR_TYP, DOZYMETR_NR_FAB,
            INNE_NASTAWY, SONDA_TYP, SONDA_NR_FAB, NAP_ZAS_SONDY,
            CISNIENIE, TEMPERATURA, WILGOTNOSC, UWAGI, WYKONAL, SPRAWDZIL
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
}