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

        //---------------------------------------------------------
        public StaleForm()
        //---------------------------------------------------------
        {
            InitializeComponent();

            _BazaDanych = new BazaDanychWrapper();
        }

        //---------------------------------------------------------
        private void button1_Click(object sender, EventArgs e)
        //---------------------------------------------------------
        {
            if (false == TestujMozliwoscZapisu())
            {
                MyMessageBox.Show("Część danych jest niepoprawnych. Zapis nie jest możliwy.");
                return;
            }

            ZapiszDane();
        }

        //---------------------------------------------------------
        private void button2_Click(object sender, EventArgs e)
        //---------------------------------------------------------
        {
            if (DialogResult.Yes == MyMessageBox.Show("Czy na pewno wyjść? Jeśli wprowadziłeś zmiany bez zatwierdzenia nie zostaną zapamiętane.", "Kończenie pracy", MessageBoxButtons.YesNo))
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
                dataGridView1.Rows.Add(wiersz.Field<string>("Nazwa"), wiersz.Field<double>("Wartosc"));
        }

        //----------------------------------------------------------
        private bool TestujMozliwoscZapisu()
        //----------------------------------------------------------
        {
            double temp;

            foreach(DataGridViewRow wiersz in dataGridView1.Rows)
            {
                if ("" == wiersz.Cells["Parametr"].Value.ToString() || false == N.doubleTryParse(dataGridView1.Rows[1].Cells["Wartosc"].Value.ToString(), out temp))
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
            /*_BazaDanych.WykonajPolecenie("DELETE FROM Stale");*/
            _BazaDanych.Stale
                .DELETE()
                .INFO("Usuń wszystkie stałe przed ponownym zapisaniem.")
                .EXECUTE();

            foreach (DataGridViewRow wiersz in dataGridView1.Rows)
            {
                // TODO: Sprawdzić, czy po zmianie stałych nie trzeba przeładować jakiś globalnych danych
                String parametr = wiersz.Cells["Parametr"].Value.ToString();
                var wartosc = N.doubleParse(wiersz.Cells["Wartosc"].Value.ToString());

                /*_Zapytanie = String.Format("INSERT INTO Stale VALUES('{0}', {1}, '')", parametr, wartosc);
                _BazaDanych.WykonajPolecenie(_Zapytanie);*/
                _BazaDanych.Stale
                    .INSERT()
                        .Nazwa(parametr)
                        .Wartosc(wartosc)
                        .Uwagi("")
                    .INFO("Zapis stałych.")
                    .EXECUTE();
            }
        }
    }
}

