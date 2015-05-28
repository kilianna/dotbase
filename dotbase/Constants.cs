using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DotBase
{
    class Constants
    {
        private static readonly String UNCERTAINITY_PRESSURE = "up";
        private static readonly String UNCERTAINITY_TEMPERATURE = "uT";
        private static readonly String UNCERTAINITY_HUMIDITY = "uw";
        private static readonly String CS_HALF_TIME = "T1/2 Cs";

        public readonly float UNCERTAINITY_PRESSURE_VALUE;
        public readonly float UNCERTAINITY_TEMPERATURE_VALUE;
        public readonly float UNCERTAINITY_HUMIDITY_VALUE;
        public readonly float CS_HALF_TIME_VALUE;

        private static Constants instance = new Constants();

        private Constants() {
            BazaDanychWrapper dbConnector = new BazaDanychWrapper();
            UNCERTAINITY_PRESSURE_VALUE = dbConnector.TworzTabeleDanych(String.Format("SELECT wartosc FROM Stale WHERE nazwa='{0}'", Constants.UNCERTAINITY_PRESSURE)).Rows[0].Field<float>(0);
            UNCERTAINITY_TEMPERATURE_VALUE = dbConnector.TworzTabeleDanych(String.Format("SELECT wartosc FROM Stale WHERE nazwa='{0}'", Constants.UNCERTAINITY_TEMPERATURE)).Rows[0].Field<float>(0);
            UNCERTAINITY_HUMIDITY_VALUE = dbConnector.TworzTabeleDanych(String.Format("SELECT wartosc FROM Stale WHERE nazwa='{0}'", Constants.UNCERTAINITY_HUMIDITY)).Rows[0].Field<float>(0);
            CS_HALF_TIME_VALUE = dbConnector.TworzTabeleDanych(String.Format("SELECT wartosc FROM Stale WHERE nazwa='{0}'", Constants.CS_HALF_TIME)).Rows[0].Field<float>(0);
        }

        public static Constants getInstance()
        {
            return instance;
        }
    }
}
