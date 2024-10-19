using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DotBase
{    
    class WzorcowanieZrodel
    {
        BazaDanychWrapper _BazaDanych;
        string _Zapytanie;
        public DataTable OdpowiedzBazy { get; private set; }
        public Dictionary<string, int> Zrodla { get; private set; }

        //--------------------------------------------------------
        public WzorcowanieZrodel()
        //--------------------------------------------------------
        {
            _BazaDanych = new BazaDanychWrapper();
            Zrodla = new Dictionary<string, int>();
        }

        //-------------------------------------------------------------------
        public void AktualizujDane( List<short> zrodlo, List<string> dataWzorcowania, List<double> emisjaPowierzchniowa, List<short> idAtestu )
        //-------------------------------------------------------------------
        {
            for(int i = 0; i < zrodlo.Count; ++i)
            {
                /*_Zapytanie = String.Format("UPDATE Atesty_zrodel SET Data_wzorcowania='{0}', Emisja_powierzchniowa='{1}', id_zrodla={2} WHERE id_atestu={3}",
                                           dataWzorcowania[i], emisjaPowierzchniowa[i], zrodlo[i], idAtestu[i]);
                _BazaDanych.WykonajPolecenie(_Zapytanie);*/
                _BazaDanych.Atesty_zrodel
                    .UPDATE()
                        .Data_wzorcowania(DateTime.Parse(dataWzorcowania[i]))
                        .Emisja_powierzchniowa(emisjaPowierzchniowa[i])
                        .ID_zrodla(zrodlo[i])
                    .WHERE()
                        .ID_atestu(idAtestu[i])
                    .EXECUTE();
            }
        }

        //-------------------------------------------------------------------
        public void DodajDane(List<short> zrodlo, List<string> dataWzorcowania, List<double> emisjaPowierzchniowa)
        //-------------------------------------------------------------------
        {
            _Zapytanie = "SELECT MAX(ID_atestu) FROM Atesty_zrodel";
            short idAtestu = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<short>(0);
            ++idAtestu;
            for (int i = 0; i < zrodlo.Count; ++i, ++idAtestu)
            {
                /*_Zapytanie = "INSERT INTO Atesty_zrodel (data_wzorcowania, id_zrodla, Emisja_powierzchniowa, id_atestu) VALUES"
                           + String.Format("('{0}', {1}, '{2}', {3})", dataWzorcowania[i], zrodlo[i], emisjaPowierzchniowa[i], idAtestu);
                _BazaDanych.WykonajPolecenie(_Zapytanie);*/
                _BazaDanych.Atesty_zrodel
                    .INSERT()
                        .Data_wzorcowania(DateTime.Parse(dataWzorcowania[i]))
                        .ID_zrodla(zrodlo[i])
                        .Emisja_powierzchniowa(emisjaPowierzchniowa[i])
                        .ID_atestu(idAtestu)
                    .EXECUTE();
            }
        }

        //-------------------------------------------------------------------
        public bool TworzSlownik()
        //-------------------------------------------------------------------
        {
            Zrodla.Clear();

            if (TworzOdpowiedz("SELECT nazwa, id_zrodla FROM zrodla_powierzchniowe"))
            {
                foreach(DataRow row in OdpowiedzBazy.Rows)
                {
                    Zrodla.Add(row.Field<string>(0), row.Field<int>(1));
                }

                return true;
            }

            return false;
        }

        //--------------------------------------------------------
        public bool ZnajdzOstatnieWzorcowanie()
        //--------------------------------------------------------
        {
            _Zapytanie = "SELECT ID_Zrodla, Data_wzorcowania, Emisja_powierzchniowa, Id_Atestu FROM Atesty_zrodel WHERE data_wzorcowania "
                       + "IN (SELECT MAX(data_wzorcowania) FROM Atesty_zrodel GROUP BY Id_zrodla) ORDER BY id_zrodla";

            return TworzOdpowiedz(_Zapytanie);
        }

        //--------------------------------------------------------
        private bool TworzOdpowiedz(string zapytanie)
        //--------------------------------------------------------
        {
            OdpowiedzBazy = _BazaDanych.TworzTabeleDanych(zapytanie);

            if (OdpowiedzBazy == null || OdpowiedzBazy.Rows == null)
                return false;

            return true;
        }
    }
}
