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
    // Klasa ma tak niewielką funkcjonalność, że nie została rozbita
    // Na część GUI i przetwarzającą
    public partial class BudzetNiepewnosciForm : Form
    {
        private const int MAX_WIERSZY = 7;

        private string _Zapytanie;
        private BazaDanychWrapper _BazaDanych;
        private string[] wielkosci;
        private double[] wartosci;

        //----------------------------------------------------------
        public BudzetNiepewnosciForm()
        //----------------------------------------------------------
        {
            InitializeComponent();
            wielkosci = new string[MAX_WIERSZY];
            wartosci = new double[MAX_WIERSZY];
            _BazaDanych = new BazaDanychWrapper();
        }

        //----------------------------------------------------------
        private void button1_Click(object sender, EventArgs e)
        //----------------------------------------------------------
        {
            if (false == TestujMozliwoscZapisu())
                return;
            
            ZapiszDane();
        }

        //----------------------------------------------------------
        private void button2_Click(object sender, EventArgs e)
        //----------------------------------------------------------
        {
            if (DialogResult.Yes == MessageBox.Show("Czy na pewno wyjść? Jeśli wprowadziłeś zmiany bez zatwierdzenia nie zostaną zapamiętane.", "Kończenie pracy", MessageBoxButtons.YesNo))
                Dispose();
        }

        //----------------------------------------------------------
        private void CzyscOkno()
        //----------------------------------------------------------
        {
            textBox1.Text = textBox1.Text = textBox2.Text = textBox1.Text = textBox3.Text =
            textBox4.Text = textBox1.Text = textBox5.Text = textBox1.Text = textBox6.Text =
            textBox7.Text = textBox1.Text = textBox8.Text = textBox1.Text = textBox9.Text =
            textBox10.Text = textBox11.Text = textBox12.Text = textBox13.Text = textBox14.Text =
            textBox7.Text = "";
        }

        //----------------------------------------------------------
        public bool Inicjalizuj()
        //----------------------------------------------------------
        {
            _Zapytanie = "SELECT wielkosc, wartosc FROM BudzetNiepewnosci";

            try
            {
                DataTable dane = _BazaDanych.TworzTabeleDanych(_Zapytanie);

                PobierzDane(dane);
                WyswietlDane();
            }
            catch (Exception)
            {
                CzyscOkno();
                return false;
            }

            LiczNiepewnosc();

            return true;
        }

        //----------------------------------------------------------
        void LiczNiepewnosc()
        //----------------------------------------------------------
        {
            double suma = 0;

            for (int i = 0; i < MAX_WIERSZY; ++i)
            {
                suma += wartosci[i] * wartosci[i];
            }

            textBox15.Text = suma.ToString();
        }

        //----------------------------------------------------------
        void PobierzDane(DataTable dane)
        //----------------------------------------------------------
        {
            // wierszy danych zawsze będzie 7
            for (int i = 0; i < MAX_WIERSZY; ++i)
            {
                wielkosci[i] = dane.Rows[i].Field<string>("Wielkosc");
                wartosci[i] = dane.Rows[i].Field<double>("Wartosc");
            }
        }

        //----------------------------------------------------------
        private bool TestujMozliwoscZapisu()
        //----------------------------------------------------------
        {
            if ( "" == (wielkosci[0] = textBox1.Text) ||
                 "" == (wielkosci[1] = textBox2.Text) ||
                 "" == (wielkosci[2] = textBox3.Text) ||
                 "" == (wielkosci[3] = textBox4.Text) ||
                 "" == (wielkosci[4] = textBox5.Text) ||
                 "" == (wielkosci[5] = textBox6.Text) ||
                 "" == (wielkosci[6] = textBox7.Text) )
            {
                MessageBox.Show("Nie podano wszystkich wielkości! Zapis niemożliwy.");
                return false;
            }

            try
            {
                wartosci[0] = N.doubleParse(textBox8.Text);
                wartosci[1] = N.doubleParse(textBox9.Text);
                wartosci[2] = N.doubleParse(textBox10.Text);
                wartosci[3] = N.doubleParse(textBox11.Text);
                wartosci[4] = N.doubleParse(textBox12.Text);
                wartosci[5] = N.doubleParse(textBox13.Text);
                wartosci[6] = N.doubleParse(textBox14.Text);
            }
            catch(Exception)
            {
                MessageBox.Show("Podane wartości są nieodpowiednie! Zapis niemożliwy.");
                return false;
            }

            return true;
        }

        //----------------------------------------------------------
        private void WyswietlDane()
        //----------------------------------------------------------
        {
            textBox1.Text = wielkosci[0];
            textBox8.Text = wartosci[0].ToString();
            textBox2.Text = wielkosci[1];
            textBox9.Text = wartosci[1].ToString();
            textBox3.Text = wielkosci[2];
            textBox10.Text = wartosci[2].ToString();
            textBox4.Text = wielkosci[3];
            textBox11.Text = wartosci[3].ToString();
            textBox5.Text = wielkosci[4];
            textBox12.Text = wartosci[4].ToString();
            textBox6.Text = wielkosci[5];
            textBox13.Text = wartosci[5].ToString();
            textBox7.Text = wielkosci[6];
            textBox14.Text = wartosci[6].ToString();
        }

        //----------------------------------------------------------
        private void ZapiszDane()
        //----------------------------------------------------------
        {
            _BazaDanych.WykonajPolecenie("DELETE FROM BudzetNiepewnosci");

            for (int i = 0; i < MAX_WIERSZY; ++i)
            {
                _Zapytanie = String.Format("INSERT INTO BudzetNiepewnosci VALUES ('{0}', '{1}')", wielkosci[i], wartosci[i]);
                _BazaDanych.WykonajPolecenie(_Zapytanie);
            }

            LiczNiepewnosc();
        }
    }

}
