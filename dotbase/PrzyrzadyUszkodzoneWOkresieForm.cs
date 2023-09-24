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
    public partial class PrzyrzadyUszkodzoneWOkresieForm : Form
    {
        string _Zapytanie;
        BazaDanychWrapper _BazaDanych;
        DataTable _Dane;

        //----------------------------------------------------------
        public PrzyrzadyUszkodzoneWOkresieForm()
        //----------------------------------------------------------
        {
            InitializeComponent();
            _BazaDanych = new BazaDanychWrapper();
            dataGridView1.Columns[2].ValueType = typeof(int);
        }

        //----------------------------------------------------------
        private void CzyscStareDane()
        //----------------------------------------------------------
        {
            dataGridView1.Rows.Clear();
        }

        //----------------------------------------------------------
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        //----------------------------------------------------------
        {
            Wyszukaj();
        }

        //----------------------------------------------------------
        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        //----------------------------------------------------------
        {
            Wyszukaj();
        }

        //----------------------------------------------------------
        private void Wyszukaj()
        //----------------------------------------------------------
        {
            _Zapytanie = "SELECT D.Typ, D.Nr_fabryczny, K.Id_karty, K.Uwagi FROM (Karta_przyjecia AS K INNER JOIN Dozymetry AS D ON "
                       + "K.Id_dozymetru=D.Id_dozymetru) INNER JOIN Zlecenia AS Z ON Z.id_zlecenia=K.id_zlecenia WHERE K.Uszkodzony=true "
                       + String.Format("AND Z.data_przyjecia BETWEEN #{0}# AND #{1}# ORDER BY K.Id_karty",
                       dateTimePicker1.Value.ToShortDateString(), dateTimePicker2.Value.ToShortDateString());
            
            _Dane = _BazaDanych.TworzTabeleDanych(_Zapytanie);

            if (null != _Dane)
                Wyswietl();
        }

        //----------------------------------------------------------
        private void Wyswietl()
        //----------------------------------------------------------
        {
            // Liczba wierszy wynikowych jest jednocześnie sumą wszystkich zepsutych
            textBox1.Text = _Dane.Rows.Count.ToString();

            dataGridView1.Rows.Clear();

            foreach (DataRow row in _Dane.Rows)
            {
                dataGridView1.Rows.Add(1);

                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells["Typ"].Value = row.Field<string>(0);
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells["NrFabryczny"].Value = row.Field<string>(1);
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells["NrKarty"].Value = row.Field<int>(2);
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells["Uwagi"].Value = row.Field<string>(3);
            }
        }
    }
}
