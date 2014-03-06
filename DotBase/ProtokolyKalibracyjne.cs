using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DotBase
{
    class ProtokolyKalibracyjne
    {
        BazaDanychWrapper _BazaDanych;
        String _Zapytanie;
        public DataTable OdpowiedzBazy { get; private set; }

        //----------------------------------------------------
        public ProtokolyKalibracyjne()
        //----------------------------------------------------
        {
            _BazaDanych = new BazaDanychWrapper();
        }

        //--------------------------------------------------------------------------------------
        public bool ZapiszDane(int idProtokolu, DateTime data, List<int> id, List<double> odleglosc, List<double> mocKermy)
        //--------------------------------------------------------------------------------------
        {
            _Zapytanie = String.Format("DELETE FROM Protokoly_Kalibracji_Lawy WHERE id_protokolu = {0}", idProtokolu);
            _BazaDanych.WykonajPolecenie(_Zapytanie);

            _Zapytanie = String.Format("INSERT INTO Protokoly_Kalibracji_Lawy (ID_protokolu, Data_kalibracji) VALUES ({0}, #{1}#)",
                                       idProtokolu, data.ToShortDateString());
            _BazaDanych.WykonajPolecenie(_Zapytanie);


            _Zapytanie = String.Format("DELETE FROM Pomiary_wzorcowe WHERE id_protokolu = {0}", idProtokolu);
            _BazaDanych.WykonajPolecenie(_Zapytanie);
                
            for(int i = 0; i < id.Count; ++i)
            {
                _Zapytanie = String.Format("INSERT INTO Pomiary_Wzorcowe VALUES ('{0}','{1}','{2}','{3}')", odleglosc[i], id[i], mocKermy[i].ToString(), idProtokolu);
             
                _BazaDanych.WykonajPolecenie(_Zapytanie);
            }

            return true;
        }

        //--------------------------------------------------------------------------------------
        private bool TworzOdpowiedz(string _Zapytanie)
        //--------------------------------------------------------------------------------------
        {
            OdpowiedzBazy = _BazaDanych.TworzTabeleDanych(_Zapytanie);

            if (null == OdpowiedzBazy || null == OdpowiedzBazy.Rows || 0 == OdpowiedzBazy.Rows.Count)
                return false;

            return true;
        }

        //--------------------------------------------------------------------------------------
        public bool WyszukajProtokolPoDacie(DateTime data)
        //--------------------------------------------------------------------------------------
        {
            _Zapytanie = "SELECT P.ID_Protokolu, Data_kalibracji, ID_Zrodla, Odleglosc, Moc_Kermy FROM Protokoly_Kalibracji_Lawy AS K "
                       + String.Format("INNER JOIN Pomiary_Wzorcowe AS P ON K.id_protokolu=P.id_protokolu WHERE Data_kalibracji=#{0}#",
                                       data.ToShortDateString());
            
            return TworzOdpowiedz(_Zapytanie);
	    }

        //--------------------------------------------------------------------------------------
        public bool WyszukajProtokolPoId(int id)
        //--------------------------------------------------------------------------------------
        {
            _Zapytanie = "SELECT P.ID_Protokolu, Data_kalibracji, ID_Zrodla, Odleglosc, Moc_Kermy FROM Protokoly_Kalibracji_Lawy AS K "
                       + String.Format("INNER JOIN Pomiary_Wzorcowe AS P ON K.id_protokolu=P.id_protokolu WHERE P.ID_protokolu={0}", id);

            return TworzOdpowiedz(_Zapytanie);
        }

        //----------------------------------------------------
        public bool ZnajdzOstatni()
        //----------------------------------------------------
        {
            _Zapytanie = "SELECT P.ID_Protokolu, Data_kalibracji, ID_Zrodla, Odleglosc,Moc_Kermy FROM Protokoly_Kalibracji_Lawy AS K "
                       + "INNER JOIN Pomiary_Wzorcowe AS P ON K.id_protokolu=P.id_protokolu WHERE P.ID_protokolu IN (SELECT ID_Protokolu "
                       + "FROM Protokoly_kalibracji_lawy WHERE Data_kalibracji=(SELECT MAX(Data_kalibracji) FROM Protokoly_kalibracji_lawy))";

            return TworzOdpowiedz(_Zapytanie);
        }

    }
}
