using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotBase
{
    class SwiadectwoData
    {
        public enum DataType
        {
            ADRES,
            DATA, DATA_PLUS_ROK, DATA_MESIAC_SLOWNIE,
            INFO_SONDY, INFO_WYKRES, INFO_WYKRES_KALIB,
            NR_FABRYCZNY, NR_KARTY,
            ZLECENIODAWCA,
            NR_PISMA,
            ROK,
            TYP,
            UWAGA
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
