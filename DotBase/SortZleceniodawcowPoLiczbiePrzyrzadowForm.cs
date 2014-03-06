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
    // klasa z uwagi na swą banalność nie ma rozdziału na część Frontendową i Backendową
    public partial class SortZleceniodawcowPoLiczbiePrzyrzadow : Form
    {
        BazaDanychWrapper _BazaDanych;
        string _Zapytanie;
        
        //-------------------------------------------------------------
        public SortZleceniodawcowPoLiczbiePrzyrzadow()
        //-------------------------------------------------------------
        {
            InitializeComponent();
            _BazaDanych = new BazaDanychWrapper();
        }

        //-------------------------------------------------------------
        private void FiltrujDane(object sender, EventArgs e)
        //-------------------------------------------------------------
        {
            dataGridView1.Rows.Clear();

            _Zapytanie = "SELECT Z.id_zleceniodawcy, Z.Zleceniodawca, COUNT(*) FROM ((Zleceniodawca AS Z INNER JOIN Zlecenia AS ZL ON "
                       + "Z.id_zleceniodawcy=ZL.id_zleceniodawcy) INNER JOIN Karta_przyjecia AS K ON ZL.id_zlecenia=K.id_zlecenia) "
                       + String.Format("WHERE data_przyjecia BETWEEN #{0}# AND #{1}# GROUP BY ", dateTimePicker1.Value.ToShortDateString(), dateTimePicker2.Value.ToShortDateString())
                       + "Z.id_zleceniodawcy, Z.Zleceniodawca HAVING COUNT(*) > 0";
            
            DataTable dane = _BazaDanych.TworzTabeleDanych(_Zapytanie);

            try
            {
                foreach (DataRow row in dane.Rows)
                {
                    dataGridView1.Rows.Add(row.Field<int>(0).ToString(), row.Field<string>(1), row.Field<int>(2).ToString());
                }
            }
            catch (Exception)
            {
                return;
            }
        }
    }
}
