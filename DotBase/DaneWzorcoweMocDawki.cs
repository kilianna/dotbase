using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DotBase
{
    public partial class DaneWzorcoweMocDawki : Form
    {
        BazaDanychWrapper _BazaDanych;
        String _Zapytanie;

        //**********************************************************
        public DaneWzorcoweMocDawki()
        //**********************************************************
        {
            _BazaDanych = new BazaDanychWrapper();
            InitializeComponent();
            dataGridView1.Columns[0].ValueType = typeof(double);
            dataGridView1.Columns[1].ValueType = typeof(double);
            dataGridView1.Columns[2].ValueType = typeof(double);
            dataGridView1.Columns[3].ValueType = typeof(double);
        }

        //**********************************************************
        private void LiczWartoscDlaZrodla(int id_zrodla, DateTime protokol, double przelicznik, int kolumna, double korekta, int ile)
        //**********************************************************
        {
            double mocKermy;

            string drugaCzescZapytanie = String.Format("(SELECT id_protokolu FROM Protokoly_kalibracji_lawy WHERE data_kalibracji = #{0}#) ", protokol.ToShortDateString());

	        for(int k = 0; k < ile; ++k)
	        {
                _Zapytanie = String.Format("SELECT moc_kermy FROM Pomiary_wzorcowe WHERE id_zrodla={0} AND odleglosc = {1} AND id_protokolu =", id_zrodla, dataGridView1.Rows[k].Cells[0].Value.ToString().Replace(",", ".") ) 
                           + drugaCzescZapytanie;

                try
                {
                     mocKermy = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<double>(0);
                }
                catch(Exception)
                {
                    mocKermy = -1.0;
                    dataGridView1.Rows[k].Cells[kolumna].Value = "";
                    continue;
                }

		        dataGridView1.Rows[k].Cells[kolumna].Value = Przelicz(mocKermy, przelicznik, korekta);
	        }
        }

        //**********************************************************
        double Przelicz(double liczba, double przelicznik, double korekta)
        //**********************************************************
        {
            return liczba / przelicznik * korekta;
        }

        //**********************************************************
        private double ZnajdzKorekte(DateTime data1, DateTime data2)
        //**********************************************************
        {
            int ile = (data1 - data2).Days;

            // 11050.0 - czas połowicznego rozpadu cezu w dniach
            return Math.Exp(-Math.Log(2.0) * ile / 11050.0);
        }

        //**********************************************************
        private double ZnajdzPrzelicznik(String jednostka)
        //**********************************************************
        {
            _Zapytanie = String.Format("SELECT przelicznik FROM Jednostki WHERE jednostka='{0}'", jednostka);

            return _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<float>(0);
        }

        //**********************************************************
        public void WyswietlDane(DateTime data, DateTime protokol, String jednostka)
        //**********************************************************
        {
            double korekta = ZnajdzKorekte(data, protokol);
            double przelicznik = ZnajdzPrzelicznik(jednostka);

            _Zapytanie = "SELECT DISTINCT odleglosc FROM Pomiary_wzorcowe WHERE id_protokolu=(SELECT id_protokolu FROM "
                       + String.Format("Protokoly_kalibracji_lawy WHERE data_kalibracji=#{0}#)", protokol.ToShortDateString());

            DataTable dane = _BazaDanych.TworzTabeleDanych(_Zapytanie);

            int i;
            for (i = 0; i < dane.Rows.Count; ++i)
            {
                dataGridView1.Rows.Add(dane.Rows[i].Field<double>(0));
            }

            LiczWartoscDlaZrodla(3, protokol, przelicznik, 1, korekta, i);
            LiczWartoscDlaZrodla(2, protokol, przelicznik, 2, korekta, i);
            LiczWartoscDlaZrodla(1, protokol, przelicznik, 3, korekta, i);
        }
    }
}

