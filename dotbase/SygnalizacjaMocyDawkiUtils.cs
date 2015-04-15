using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotBase
{
    class SygnalizacjaMocyDawkiUtils
    {
        private static BazaDanychWrapper _BazaDanych = new BazaDanychWrapper();

        private static int retrieveIdWzorcowania(String idKarty)
        {
            return _BazaDanych.TworzTabeleDanych(
                    String.Format("SELECT id_wzorcowania FROM Wzorcowanie_cezem WHERE id_karty = {0} AND rodzaj_wzorcowania = 'sm'", idKarty))
                          .Rows[0].Field<int>("id_wzorcowania");
        }

        public static IList<double> computeFactors(String idKarty)
        {
            IList<double> computedFactors = new List<double>();

            foreach (DataRow row in _BazaDanych.TworzTabeleDanych(String.Format("SELECT prog, wartosc_zmierzona FROM Sygnalizacja WHERE id_wzorcowania = {0} ORDER BY prog", retrieveIdWzorcowania(idKarty))).Rows)
            {
                computedFactors.Add(row.Field<double>("prog") / row.Field<double>("wartosc_zmierzona"));
            }

            return computedFactors;
        }

        public static IList<double> computeUncertainity(String idKarty)
        {
            int idWzorcowania = retrieveIdWzorcowania(idKarty);
            IList<double> computedFactors = computeFactors(idKarty);
            IList<double> wahaniaWzgledne = retrieveWahanieWzgledne(idWzorcowania);
            IList<double> computedUncertainity = new List<double>();

            for (int i = 0; i < computedFactors.Count; ++i)
            {
                computedUncertainity.Add(Math.Sqrt(Math.Pow(wahaniaWzgledne[i], 2.0) + Math.Pow(retrieveSkladowaStalaNiepewnosci(), 2.0)) * computedFactors[i] * 2.0);
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

        private static IList<double> retrieveWahanieWzgledne(int idWzorcowania)
        {
            IList<double> computedFactors = new List<double>();

            foreach (DataRow row in _BazaDanych.TworzTabeleDanych(String.Format("SELECT niepewnosc, wartosc_zmierzona FROM Sygnalizacja WHERE id_wzorcowania = {0} ORDER BY prog", idWzorcowania)).Rows)
            {
                computedFactors.Add(row.Field<double>("niepewnosc") / row.Field<double>("wartosc_zmierzona"));
            }

            return computedFactors;
        }
    }
}
