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

        public readonly double UNCERTAINITY_PRESSURE_VALUE;
        public readonly double UNCERTAINITY_TEMPERATURE_VALUE;
        public readonly double UNCERTAINITY_HUMIDITY_VALUE;
        public readonly double CS_HALF_TIME_VALUE;
        public readonly double UT_D;
        public readonly double UT_SD;
        public readonly double UKJED;
        public readonly double DELTA_L_D;
        public readonly double DELTA_L_MD;
        public readonly double ut;
        public readonly double ukT12Cs;
        public readonly double T12Cs;


        private static Constants instance = new Constants();

        private Constants() {
            BazaDanychWrapper dbConnector = new BazaDanychWrapper();
            UNCERTAINITY_PRESSURE_VALUE = dbConnector.TworzTabeleDanych(String.Format("SELECT wartosc FROM Stale WHERE nazwa='{0}'", Constants.UNCERTAINITY_PRESSURE)).Rows[0].Field<double>(0);
            UNCERTAINITY_TEMPERATURE_VALUE = dbConnector.TworzTabeleDanych(String.Format("SELECT wartosc FROM Stale WHERE StrComp(nazwa, '{0}', 0)=0", Constants.UNCERTAINITY_TEMPERATURE)).Rows[0].Field<double>(0);
            UNCERTAINITY_HUMIDITY_VALUE = dbConnector.TworzTabeleDanych(String.Format("SELECT wartosc FROM Stale WHERE nazwa='{0}'", Constants.UNCERTAINITY_HUMIDITY)).Rows[0].Field<double>(0);
            CS_HALF_TIME_VALUE = dbConnector.TworzTabeleDanych(String.Format("SELECT wartosc FROM Stale WHERE nazwa='{0}'", Constants.CS_HALF_TIME)).Rows[0].Field<double>(0);
            UT_D = dbConnector.TworzTabeleDanych("SELECT wartosc FROM Stale WHERE nazwa='ut_d'").Rows[0].Field<double>(0);
            UT_SD = dbConnector.TworzTabeleDanych("SELECT wartosc FROM Stale WHERE nazwa='ut_sd'").Rows[0].Field<double>(0);
            UKJED = dbConnector.TworzTabeleDanych("SELECT wartosc FROM Stale WHERE nazwa='ukjed'").Rows[0].Field<double>(0);
            DELTA_L_D = dbConnector.TworzTabeleDanych("SELECT wartosc FROM Stale WHERE nazwa='delta_l_d'").Rows[0].Field<double>(0);
            DELTA_L_MD = dbConnector.TworzTabeleDanych("SELECT wartosc FROM Stale WHERE nazwa='delta_l_md'").Rows[0].Field<double>(0);
            ut = dbConnector.TworzTabeleDanych("SELECT wartosc FROM Stale WHERE StrComp(nazwa, 'ut', 0)=0").Rows[0].Field<double>(0);
            ukT12Cs = dbConnector.TworzTabeleDanych("SELECT wartosc FROM Stale WHERE nazwa='ukT1/2 Cs'").Rows[0].Field<double>(0);
            T12Cs = dbConnector.TworzTabeleDanych("SELECT wartosc FROM Stale WHERE nazwa='T1/2 Cs'").Rows[0].Field<double>(0);
        }

        public static Constants getInstance()
        {
            return instance;
        }
    }
}
