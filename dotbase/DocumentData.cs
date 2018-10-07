using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotBase
{
    class DocumentData
    {
        public enum DataType
        {
            ADRES, AKCESORIA,
            DATA_PRZYJECIA, DATA_ZWROTU, DOZYMETR_ID, DOZYMETR_NR_FABRYCZNY, DOZYMETR_TYP,
            EKSPRES, EMAIL,
            FAKS,
            ID_KARTY, ID_ZLECENIA, ID_ZLECENIODAWCY,
            OSOBA_KONTAKTOWA, OSOBA_PRZYJMUJACA,
            ROK,
            SONDA_ID, SONDA_NR_FABRYCZNY, SONDA_TYP,
            TABELA, TELEFON, TEST_NA_SKAZENIA,
            UWAGI,
            WYMAGANIA,
            ZLECENIDOAWCA,
            NR_REJESTRU,
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
