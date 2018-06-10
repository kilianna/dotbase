using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PrzestrzenSody
{
    public partial class SondaForm : Form
    {
        bool _TrybDodawania;
        Sonda _Sonda;

        //******************************************************
        public SondaForm(int idPrzyrzadu) 
        //******************************************************
        {
            InitializeComponent();
            _TrybDodawania = false;
            _Sonda = new Sonda(idPrzyrzadu);
            label4.Text = "Sonda zostanie dodana dla dozymetru o numerze: " + idPrzyrzadu;

            int numerSondy = _Sonda.ZnajdzOstatniaSonde();

            if (-1 != numerSondy && _Sonda.PobierzDane(numerSondy))
            {
                button1_Click();
            }
        }

        //******************************************************
        public SondaForm(int idPrzyrzadu, string typ, string nrFabryczny)
        //******************************************************
        {
            InitializeComponent();
            
            _Sonda = new Sonda(idPrzyrzadu);
            textBox1.Text = typ;
            textBox2.Text = nrFabryczny;
            textBox3.Text = _Sonda.ZnajdzNumerSondy(idPrzyrzadu, typ, nrFabryczny).ToString();

            label4.Text = "Sonda zostanie dodana dla dozymetru o numerze: " + idPrzyrzadu;

            WlaczTrybEdytowania();
        }


        //******************************************************
        public void WlaczTrybDodawania()
        //******************************************************
        {
            button2.Enabled = textBox3.Enabled = false;
            textBox3.Text = _Sonda.UtworzNowyNumerSondy().ToString();
        }

        //******************************************************
        public void WlaczTrybEdytowania()
        //******************************************************
        {
            button1.Enabled = button3.Enabled = button4.Enabled = textBox3.Enabled = false;
        }

        //******************************************************
        // Dodaj sondę
        private void button1_Click(object sender = null, EventArgs e = null)
        //******************************************************
        {
            if (_TrybDodawania)
            {
                try
                {
                    if (SprawdzPoprawnoscDanych() && _Sonda.DodajSonde(_Sonda.UtworzNowyNumerSondy(), textBox1.Text, textBox2.Text))
                    {
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("", "Uwaga");
                        return;
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Błędne dane.", "Uwaga");
                    return;
                }
            }
            else
            {
                _TrybDodawania = true;
                button1.Text = "Zatwierdź dodanie";
                button2.Enabled = button3.Enabled = button4.Enabled = false;
                textBox3.Text = _Sonda.UtworzNowyNumerSondy().ToString();
                textBox1.Text = "detektor własny";
                textBox2.Text = "nie dotyczy";
            }
        }

        //******************************************************
        // Aktualizuj dane sondy
        private void button2_Click(object sender, EventArgs e)
        //******************************************************
        {
            if (false == SprawdzPoprawnoscDanych())
            {
                MessageBox.Show("Dane są niepoprawne.");
                return;
            }

            _Sonda.AktualizujDaneSondy(Int32.Parse(textBox3.Text), textBox1.Text, textBox2.Text);
        }

        //******************************************************
        private bool SprawdzPoprawnoscDanych()
        //******************************************************
        {
            return textBox1.Text != "" && textBox2.Text != "";
        }

        //******************************************************
        private void button3_Click(object sender, EventArgs e)
        //******************************************************
        {
            int numer;

            try
            {
                numer = _Sonda.ZnajdzPoprzednia(Int32.Parse(textBox3.Text));
            }
            catch (Exception)
            {
                return;
            }

            if (-1 != numer)
            {
                if (_Sonda.PobierzDane(numer))
                    WyswietlDane();
            }
            else
            {
                MessageBox.Show("Nie istnieje przyrząd o mniejszym numerze.");
            }
        }

        //*************************************************
        private void button4_Click(object sender, EventArgs e)
        //*************************************************
        {
            int numer;

            try
            {
                numer = _Sonda.ZnajdzNastepna(Int32.Parse(textBox3.Text));
            }
            catch (Exception)
            {
                return;
            }

            if (-1 != numer)
            {
                if (_Sonda.PobierzDane(numer))
                    WyswietlDane();
            }
            else
            {
                MessageBox.Show("Nie istnieje przyrząd o większym numerze.");
            }
        }

        //*************************************************
        private void WyswietlDane()
        //*************************************************
        {
            textBox3.Text = _Sonda.IdSondy.ToString();
            textBox1.Text = _Sonda.Typ;
            textBox2.Text = _Sonda.NrFabryczny;
        }
    }
}
