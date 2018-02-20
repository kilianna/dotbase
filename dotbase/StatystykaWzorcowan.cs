using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DotBase
{
    class StatystykaWzorcowan
    {
        public enum Stale
        {
            SKAZENIA, PROMIENIOWANIE_GAMMA, AMERYK, CHLOR, DAWKA, MOC_DAWKI, PLUTON, STRONT_SLABY, STRONT_SILNY, STRONT_NAJSILNIEJSZY, SYGNALIZACJA,
            WEGIEL_SLABY, WEGIEL_SILNY, LICZBA_ZLECEN, LICZBA_PRZYRZADOW, LICZBA_WZORCOWAN, LICZBA_WYSTAWIONYCH_SWIADECTW, LICZBA_ELEMENTOW
        };

        private BazaDanychWrapper _BazaDanych;
        private int[] _Wyniki;
        private String _Zapytanie;
        

        //---------------------------------------------------------
        public StatystykaWzorcowan()
        //---------------------------------------------------------
        {
            _BazaDanych = new BazaDanychWrapper();
            _Wyniki = new int[(int)Stale.LICZBA_ELEMENTOW];
        }

        //---------------------------------------------------------
        public int[] Wyniki
        //---------------------------------------------------------
        {
            get
            {
                return _Wyniki;
            }
        }

        //-------------------------------------------------------------
        public void ZbierzStatystyki(ref DateTime szukajOd, ref DateTime SzukajDo)
        //-------------------------------------------------------------
        {
            ZnajdzLiczbePrzyrzadow(ref szukajOd, ref SzukajDo);
            ZnajdzLiczbeSwiadectw(ref szukajOd, ref SzukajDo);
            ZnajdzLiczbeWzorcowanDawki(ref szukajOd, ref SzukajDo);
            ZnajdzLiczbeWzorcowanMocDawki(ref szukajOd, ref SzukajDo);
            ZnajdzLiczbeWzorcowanSygnalizacji(ref szukajOd, ref SzukajDo);
            ZnajdzLiczbeZlecen(ref szukajOd, ref SzukajDo);

            
            ZnajdzLiczbeWzorcowanNaSkazenia(ref szukajOd, ref SzukajDo);
            
            ObliczLiczbeWzorcowanGamma();
            ObliczLiczbeWzorcowan();
        }

        //-------------------------------------------------------------
        // znajdź liczbę przyrządów w danym okresie
        private void ZnajdzLiczbePrzyrzadow(ref DateTime szukajOd, ref DateTime SzukajDo)
        //-------------------------------------------------------------
        {
            _Zapytanie = "SELECT COUNT(*) FROM Karta_przyjecia AS K INNER JOIN Zlecenia AS Z ON K.id_zlecenia=Z.id_zlecenia WHERE "
                       + "data_przyjecia >= ? AND data_przyjecia <= ?";

            _Wyniki[(int)Stale.LICZBA_PRZYRZADOW] = _BazaDanych.TworzTabeleDanych(_Zapytanie, szukajOd, SzukajDo).Rows[0].Field<int>(0);
        }

        //-------------------------------------------------------------
        // znajdz liczbę wystawionych świadect w danym okresie
        private void ZnajdzLiczbeSwiadectw(ref DateTime szukajOd, ref DateTime SzukajDo)
        //-------------------------------------------------------------
        {
            _Zapytanie = "SELECT COUNT(*) FROM Swiadectwo WHERE data_wystawienia >= ? AND data_wystawienia <= ?";

            _Wyniki[(int)Stale.LICZBA_WYSTAWIONYCH_SWIADECTW] = _BazaDanych.TworzTabeleDanych(_Zapytanie, szukajOd, SzukajDo).Rows[0].Field<int>(0);
        }
        
        // znajdz liczbę wzorcowan w danym okresie
        //-------------------------------------------------------------
        private void ObliczLiczbeWzorcowan()
        //-------------------------------------------------------------
        {
            _Wyniki[(int)Stale.LICZBA_WZORCOWAN] = _Wyniki[(int)Stale.SKAZENIA] + _Wyniki[(int)Stale.PROMIENIOWANIE_GAMMA];
        }

        //-------------------------------------------------------------
        // znajdź liczbę wyorcowań na dawkę
        private void ZnajdzLiczbeWzorcowanDawki(ref DateTime szukajOd, ref DateTime SzukajDo)
        //-------------------------------------------------------------
        {
            _Zapytanie = "SELECT COUNT(*) FROM (SELECT DISTINCT id_wzorcowania FROM Wyniki_dawka) AS W INNER JOIN Wzorcowanie_cezem AS C "
                       + "ON W.id_wzorcowania=C.id_wzorcowania WHERE C.data_wzorcowania BETWEEN ? AND ?";

            _Wyniki[(int)Stale.DAWKA] = _BazaDanych.TworzTabeleDanych(_Zapytanie, szukajOd, SzukajDo).Rows[0].Field<int>(0);
        }
        
        //-------------------------------------------------------------
        // znajdź liczbę wzorcowań gamma
        private void ObliczLiczbeWzorcowanGamma()
        //-------------------------------------------------------------
        {
            _Wyniki[(int)Stale.PROMIENIOWANIE_GAMMA] = _Wyniki[(int)Stale.DAWKA]
                                                     + _Wyniki[(int)Stale.MOC_DAWKI]
                                                     + _Wyniki[(int)Stale.SYGNALIZACJA];
        }

        //-------------------------------------------------------------
        // znajdź liczbę wzorcowań na skażenia
        private void ZnajdzLiczbeWzorcowanNaSkazenia(ref DateTime szukajOd, ref DateTime SzukajDo)
        //-------------------------------------------------------------
        {

            _Zapytanie = "SELECT Z.id_zrodla, ( SELECT COUNT(*) FROM Wzorcowanie_zrodlami_powierzchniowymi AS W "
                       + "WHERE W.data_wzorcowania >= ? AND data_wzorcowania <= ? AND W.id_zrodla=Z.id_zrodla)"
                       + " FROM Zrodla_powierzchniowe AS Z "
                       + "GROUP BY Z.id_zrodla ORDER BY Z.id_zrodla";
            
            DataTable dane = _BazaDanych.TworzTabeleDanych(_Zapytanie, szukajOd, SzukajDo);

            if (dane.Rows.Count != 8)
                return;

            // wiersze wynikowe posortowane po id zrodla!!!

            // stront słaby
            _Wyniki[(int)Stale.STRONT_SLABY] = dane.Rows[0].Field<int>(1);
            // węgiel słaby
            _Wyniki[(int)Stale.WEGIEL_SLABY] = dane.Rows[1].Field<int>(1);
            // ameryk
            _Wyniki[(int)Stale.AMERYK]       = dane.Rows[2].Field<int>(1);
            // stront silny
            _Wyniki[(int)Stale.STRONT_SILNY] = dane.Rows[3].Field<int>(1);
            // węgiel silny
            _Wyniki[(int)Stale.WEGIEL_SILNY] = dane.Rows[4].Field<int>(1);
            // chlor
            _Wyniki[(int)Stale.CHLOR]        = dane.Rows[5].Field<int>(1);
            // pluton
            _Wyniki[(int)Stale.PLUTON]       = dane.Rows[6].Field<int>(1);

            _Wyniki[(int)Stale.STRONT_NAJSILNIEJSZY] = dane.Rows[7].Field<int>(1);
            

            _Wyniki[(int)Stale.SKAZENIA] = _Wyniki[(int)Stale.AMERYK]
                                         + _Wyniki[(int)Stale.CHLOR]
                                         + _Wyniki[(int)Stale.PLUTON]
                                         + _Wyniki[(int)Stale.STRONT_SILNY]
                                         + _Wyniki[(int)Stale.STRONT_SLABY]
                                         + _Wyniki[(int)Stale.WEGIEL_SILNY]
                                         + _Wyniki[(int)Stale.WEGIEL_SLABY]
                                         + _Wyniki[(int)Stale.STRONT_NAJSILNIEJSZY];
        }


        //-------------------------------------------------------------
        // znajdź liczbę wyorcowań na moc dawki
        private void ZnajdzLiczbeWzorcowanMocDawki(ref DateTime szukajOd, ref DateTime SzukajDo)
        //-------------------------------------------------------------
        {
            _Zapytanie = "SELECT COUNT(*) FROM (SELECT DISTINCT id_wzorcowania FROM Wyniki_moc_dawki) AS W INNER JOIN Wzorcowanie_cezem AS C "
                       +  "ON W.id_wzorcowania=C.id_wzorcowania WHERE C.data_wzorcowania BETWEEN ? AND ?";
            
            _Wyniki[(int)Stale.MOC_DAWKI] = _BazaDanych.TworzTabeleDanych(_Zapytanie, szukajOd, SzukajDo).Rows[0].Field<int>(0);    
        }

        // znajdź liczbę wzorcowań na sygnalizację
        //-------------------------------------------------------------
        private void ZnajdzLiczbeWzorcowanSygnalizacji(ref DateTime szukajOd, ref DateTime SzukajDo)
        //-------------------------------------------------------------
        {
            _Zapytanie = "SELECT COUNT(*) FROM (SELECT DISTINCT id_wzorcowania FROM Sygnalizacja) AS S INNER JOIN Wzorcowanie_cezem AS C "
                       + "ON S.id_wzorcowania=C.id_wzorcowania WHERE C.data_wzorcowania BETWEEN ? AND ?";

            _Wyniki[(int)Stale.SYGNALIZACJA] = _BazaDanych.TworzTabeleDanych(_Zapytanie, szukajOd, SzukajDo).Rows[0].Field<int>(0);
        }

        //-------------------------------------------------------------
        // znajdź liczbę zleceń w wybranym okresie
        private void ZnajdzLiczbeZlecen(ref DateTime szukajOd, ref DateTime SzukajDo)
        //-------------------------------------------------------------
        {
            _Zapytanie = "SELECT COUNT(*) FROM Zlecenia WHERE data_przyjecia >= ? AND data_przyjecia <= ?";

            _Wyniki[(int)Stale.LICZBA_ZLECEN] = _BazaDanych.TworzTabeleDanych(_Zapytanie, szukajOd, SzukajDo).Rows[0].Field<int>(0);
        }
    }
}