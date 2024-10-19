using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;

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
        public bool ZapiszDane(int idProtokolu, DateTime data, List<int> id, List<double> odleglosc, List<double> mocKermy, List<double> niepewnosc)
        //--------------------------------------------------------------------------------------
        {
            string nazwa = null;
            try
            {
                var tab = _BazaDanych.TworzTabeleDanych("SELECT Nazwa FROM Protokoly_Kalibracji_Lawy WHERE id_protokolu = ?", idProtokolu);
                var riw = tab.Rows[0];
                nazwa = tab.Rows[0].Field<string>(0);
            }
            catch (Exception) { }

            if (nazwa == null)
            {
                try
                {
                    var tab = _BazaDanych.TworzTabeleDanych("SELECT Nazwa FROM Protokoly_Kalibracji_Lawy ORDER BY id_protokolu DESC");
                    var riw = tab.Rows[0];
                    nazwa = tab.Rows[0].Field<string>(0);
                    var numText = nazwa.Substring(0, nazwa.Length - 4).Trim('/');
                    var number = Int32.Parse(numText) + 1;
                    nazwa = number.ToString("000") + data.Year;
                }
                catch (Exception) { }
            }

            if (nazwa == null)
            {
                nazwa = idProtokolu.ToString("000") + data.Year;
                MessageBox.Show("Nazwa protokołu nie może zostać automatycznie wyznaczona! Używam " + nazwa + ".");
            }

            /*_Zapytanie = String.Format("DELETE FROM Protokoly_Kalibracji_Lawy WHERE id_protokolu = {0}", idProtokolu);
            _BazaDanych.WykonajPolecenie(_Zapytanie);*/
            _BazaDanych.Protokoly_kalibracji_lawy
                .DELETE()
                .WHERE().ID_protokolu((short)idProtokolu)
                .EXECUTE();


            /*_Zapytanie = "INSERT INTO Protokoly_Kalibracji_Lawy (ID_protokolu, Data_kalibracji, Nazwa) VALUES (?, ?, ?)";
            _BazaDanych.WykonajPolecenie(_Zapytanie, idProtokolu, data, nazwa);*/
            _BazaDanych.Protokoly_kalibracji_lawy
                .INSERT()
                    .ID_protokolu((short)idProtokolu)
                    .Data_kalibracji(data)
                    .Nazwa(nazwa)
                .EXECUTE();

            /*_Zapytanie = String.Format("DELETE FROM Pomiary_wzorcowe WHERE id_protokolu = {0}", idProtokolu);
            _BazaDanych.WykonajPolecenie(_Zapytanie);*/
            _BazaDanych.Pomiary_wzorcowe
                .DELETE()
                .WHERE().ID_protokolu((short)idProtokolu)
                .EXECUTE();

            for(int i = 0; i < id.Count; ++i)
            {
                /*_Zapytanie = String.Format("INSERT INTO Pomiary_Wzorcowe VALUES ('{0}','{1}','{2}','{3}','{4}')", odleglosc[i], id[i], mocKermy[i].ToString(), idProtokolu, niepewnosc[i].ToString());
                _BazaDanych.WykonajPolecenie(_Zapytanie);*/
                _BazaDanych.Pomiary_wzorcowe
                    .INSERT()
                        .Odleglosc(odleglosc[i])
                        .ID_zrodla((short)id[i])
                        .Moc_kermy(mocKermy[i])
                        .ID_protokolu((short)idProtokolu)
                        .Niepewnosc(niepewnosc[i])
                    .EXECUTE();
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
            _Zapytanie = "SELECT P.ID_Protokolu, Data_kalibracji, ID_Zrodla, Odleglosc, Moc_Kermy, Niepewnosc FROM Protokoly_Kalibracji_Lawy AS K "
                       + String.Format("INNER JOIN Pomiary_Wzorcowe AS P ON K.id_protokolu=P.id_protokolu WHERE Data_kalibracji=#{0}#",
                                       data.ToShortDateString());
            
            return TworzOdpowiedz(_Zapytanie);
	    }

        //--------------------------------------------------------------------------------------
        public bool WyszukajProtokolPoId(int id)
        //--------------------------------------------------------------------------------------
        {
            _Zapytanie = "SELECT P.ID_Protokolu, Data_kalibracji, ID_Zrodla, Odleglosc, Moc_Kermy, Niepewnosc FROM Protokoly_Kalibracji_Lawy AS K "
                       + String.Format("INNER JOIN Pomiary_Wzorcowe AS P ON K.id_protokolu=P.id_protokolu WHERE P.ID_protokolu={0}", id);

            return TworzOdpowiedz(_Zapytanie);
        }

        //----------------------------------------------------
        public bool ZnajdzOstatni()
        //----------------------------------------------------
        {
            _Zapytanie = "SELECT P.ID_Protokolu, Data_kalibracji, ID_Zrodla, Odleglosc,Moc_Kermy, Niepewnosc FROM Protokoly_Kalibracji_Lawy AS K "
                       + "INNER JOIN Pomiary_Wzorcowe AS P ON K.id_protokolu=P.id_protokolu WHERE P.ID_protokolu IN (SELECT ID_Protokolu "
                       + "FROM Protokoly_kalibracji_lawy WHERE Data_kalibracji=(SELECT MAX(Data_kalibracji) FROM Protokoly_kalibracji_lawy))";

            return TworzOdpowiedz(_Zapytanie);
        }

    }
}
