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
    public partial class StaleForm : Form
    {
        BazaDanychWrapper _BazaDanych;
        string _Zapytanie;

        //---------------------------------------------------------
        public StaleForm()
        //---------------------------------------------------------
        {
            InitializeComponent();

            _BazaDanych = new BazaDanychWrapper();
        }

        //---------------------------------------------------------
        private void button2_Click(object sender, EventArgs e)
        //---------------------------------------------------------
        {
            if (DialogResult.Yes == MessageBox.Show("Czy na pewno wyjść? Jeśli wprowadziłeś zmiany bez zatwierdzenia nie zostaną zapamiętane.", "Kończenie pracy", MessageBoxButtons.YesNo))
                Dispose();
        }

        //---------------------------------------------------------
        public bool Inicjalizuj()
        //---------------------------------------------------------
        {
            DataTable dane = _BazaDanych.TworzTabeleDanych("SELECT * FROM Stale ORDER BY Nazwa");

            try
            {
                Wyswietl(ref dane);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        //---------------------------------------------------------
        public void Wyswietl(ref DataTable dane)
        //---------------------------------------------------------
        {
            foreach(DataRow wiersz in dane.Rows)
                dataGridView1.Rows.Add(wiersz.Field<string>("Nazwa"), wiersz.Field<double>("Wartosc").ToString());
        }

        //----------------------------------------------------------
        private bool TestujMozliwoscZapisu()
        //----------------------------------------------------------
        {
            double temp;

            foreach(DataGridViewRow wiersz in dataGridView1.Rows)
            {
                if ("" == wiersz.Cells["Parametr"].Value.ToString() || false == double.TryParse(dataGridView1.Rows[1].Cells["Wartosc"].Value.ToString(), out temp))
                {
                    return false;
                }
            }

            return true;
        }

        //----------------------------------------------------------
        private void ZapiszDane()
        //----------------------------------------------------------
        {
            if (false == TestujMozliwoscZapisu())
            {
                MessageBox.Show("Część danych jest niepoprawnych. Zapis nie jest możliwy.");
            }

            _BazaDanych.WykonajPolecenie("DELETE FROM Stale");

            foreach (DataGridViewRow wiersz in dataGridView1.Rows)
            {
                _Zapytanie = String.Format("INSERT INTO Stale VALUE('{0}', {1})", wiersz.Cells["Parametr"].Value, wiersz.Cells["Wartosc"].Value);

                _BazaDanych.WykonajPolecenie(_Zapytanie);
            }
        }
    }
}
