using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DotBase
{
    class MenuWzorcowanie
    {
        private BazaDanychWrapper _BazaDanych;
        private String _Zapytanie;
        private DataTable _OdpowiedzBazy;

        //--------------------------------------------------------------
        public MenuWzorcowanie()
        //--------------------------------------------------------------
        {
            _BazaDanych = new BazaDanychWrapper();
        }

        //--------------------------------------------------------------
        public DataTable Dane
        //--------------------------------------------------------------
        {
            get
            {
                return _OdpowiedzBazy;
            }
        }

        //--------------------------------------------------------------
        public bool Inicjalizuj()
        //--------------------------------------------------------------
        {
            if (false == ZnajdzOstatniaKarte())
                return false;
            
            return true;
        }

        //--------------------------------------------------------------
        public bool ZnajdzKartePoNumerze(int numer)
        //--------------------------------------------------------------
        {
            _Zapytanie = "SELECT K.Id_karty, ZA.Zleceniodawca, D.Typ, Nr_fabryczny, K.Id_zlecenia " +
                         "FROM (((Karta_przyjecia AS K INNER JOIN Dozymetry AS D ON K.id_dozymetru=D.id_dozymetru) INNER JOIN Zlecenia " +
                         "AS Z ON Z.id_zlecenia=K.id_zlecenia) INNER JOIN Zleceniodawca AS ZA ON " +
                         String.Format("Z.id_zleceniodawcy=ZA.id_zleceniodawcy) WHERE id_karty = {0}", numer);

            _OdpowiedzBazy = _BazaDanych.TworzTabeleDanych(_Zapytanie);

            if (null == _OdpowiedzBazy)
                return false;

            return true;
        }

        //--------------------------------------------------------------
        public bool ZnajdzOstatniaKarte()
        //--------------------------------------------------------------
        {
            _Zapytanie = "SELECT K.Id_karty, ZA.Zleceniodawca, D.Typ, Nr_fabryczny, K.id_zlecenia " +
                         "FROM (((Karta_przyjecia AS K INNER JOIN Dozymetry AS D ON K.id_dozymetru=D.id_dozymetru) INNER JOIN Zlecenia " +
                         "AS Z ON Z.id_zlecenia=K.id_zlecenia) INNER JOIN Zleceniodawca AS ZA ON " +
                         "Z.id_zleceniodawcy=ZA.id_zleceniodawcy) WHERE id_karty=(SELECT MAX(id_karty) FROM Karta_przyjecia)";

            _OdpowiedzBazy = _BazaDanych.TworzTabeleDanych(_Zapytanie);

            if (null == _OdpowiedzBazy)
                return false;

            return true;
        }

        //--------------------------------------------------------------
        public bool ZnajdzWzorcowaniaDlaDanejKarty(int numer)
        //--------------------------------------------------------------
        {
            _Zapytanie = String.Format("SELECT Stront_slaby, Stront_silny, Wegiel_silny, Wegiel_slaby, Pluton, Chlor, Ameryk, Moc_dawki, Dawka, Syg_mocy_dawki, Syg_dawki, " +
                                       "Stront_najsilniejszy FROM Karta_przyjecia WHERE id_karty = {0}", numer);

            _OdpowiedzBazy = _BazaDanych.TworzTabeleDanych(_Zapytanie);

            if (null == _OdpowiedzBazy)
                return false;

            return true;
        }

        //--------------------------------------------------------------
        public bool ZnajdzPoprzednieWzorcowanieDlaDanegoPrzyrzadu(int PoprzednieId)
        //--------------------------------------------------------------
        {
             _Zapytanie = "SELECT MAX(id_karty) FROM Karta_przyjecia WHERE id_dozymetru=(SELECT id_dozymetru FROM "
	                    + String.Format("Karta_przyjecia WHERE id_karty = {0}) AND id_karty < {0}", PoprzednieId);
	
            int idKarty = 0;

            try
            {
        	    idKarty = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<int>(0);
            }
            catch(Exception)
            {
                return false;
            }

	        ZnajdzKartePoNumerze(idKarty);

            return true;
        }
        
        //--------------------------------------------------------------
        public bool ZnajdzNastepneWzorcowanieDlaDanegoPrzyrzadu(int PoprzednieId)
        //--------------------------------------------------------------
        {
            _Zapytanie = "SELECT MIN(id_karty) FROM Karta_przyjecia WHERE id_dozymetru=(SELECT id_dozymetru FROM "
                        + String.Format("Karta_przyjecia WHERE id_karty = {0}) AND id_karty > {0}", PoprzednieId);

            int idKarty = 0;

            try
            {
                idKarty = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<int>(0);
            }
            catch (Exception)
            {
                return false;
            }

            ZnajdzKartePoNumerze(idKarty);

            return true;
        }
    }
}
