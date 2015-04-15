using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotBase
{
    class DaneZleceniodawcyData
    {
        public enum DataType
        {
            ZLECENIDOAWCA, ADRES, OSOBA_KONTAKTOWA, ID_ZLECENIODAWCY, TELEFON, FAKS, EMAIL
        };

        readonly private IDictionary<DataType, String> m_documentData = new Dictionary<DataType, String>();

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
