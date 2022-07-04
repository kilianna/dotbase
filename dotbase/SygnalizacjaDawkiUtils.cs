using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotBase
{
    class SygnalizacjaDawkiUtils
    {
        private static BazaDanychWrapper _BazaDanych = new BazaDanychWrapper();

        private static int retrieveIdWzorcowania(String idKarty)
        {
            return _BazaDanych.TworzTabeleDanych(
                    String.Format("SELECT id_wzorcowania FROM Wzorcowanie_cezem WHERE id_karty = {0} AND rodzaj_wzorcowania = 'sd'", idKarty))
                          .Rows[0].Field<int>("id_wzorcowania");
        }

        public static IList<double> computeFactors(String idKarty)
        {
            IList<double> computedFactors = new List<double>();

            foreach (DataRow row in _BazaDanych.TworzTabeleDanych(String.Format("SELECT prog, Wartosc_wzorcowa FROM Sygnalizacja_dawka WHERE id_wzorcowania = {0} ORDER BY prog", retrieveIdWzorcowania(idKarty))).Rows)
            {
                computedFactors.Add(row.Field<double>("Wartosc_wzorcowa") / row.Field<double>("prog"));
            }

            return computedFactors;
        }

        public static IList<double> computeUncertainity(String idKarty)
        {
            int idWzorcowania = retrieveIdWzorcowania(idKarty);
            IList<double> computedFactors = computeFactors(idKarty);
            IList<double> computedUncertainity = new List<double>();

            for (int i = 0; i < computedFactors.Count; ++i)
            {
                computedUncertainity.Add(retrieveSkladowaStalaNiepewnosci() * computedFactors[i] * 2.0);
            }

            return computedUncertainity;
        }

        public static double retrieveSkladowaStalaNiepewnosci()
        {
            double sum = 0.0;

            foreach (DataRow wiersz in _BazaDanych.TworzTabeleDanych("SELECT wartosc * wartosc FROM Budzetniepewnosci").Rows)
            {
                sum += wiersz.Field<double>(0);
            }

            return Math.Sqrt(sum);
        }
    }
}
