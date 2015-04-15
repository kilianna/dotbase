using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotBase
{
    class ProtokolDawkaData
    {
        public enum DataType
        {
            ZRODLO, ODLEGLOSC, WARTOSC_WZROCOWA, WSKAZANIE, WSPOLCZYNNIK, NIEPEWNOSC, TABELA
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


