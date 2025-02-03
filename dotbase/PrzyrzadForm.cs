using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PrzestrzenSody;

namespace DotBase
{
    public partial class PrzyrzadForm : Form
    {
        bool _TrybDodawania;

        Przyrzad _Przyrzad;
        SondaForm _Sonda;

        //*************************************************
        private PrzyrzadForm()
        //*************************************************
        {
            InitializeComponent();
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(Zamknij);
          //dataGridView1.Rows[0].Cells[2].Value = "Edytruj";
            _TrybDodawania = false;
        }

        //*************************************************
        public PrzyrzadForm(int idKarty)
            : this()
        //*************************************************
        {      
            _Przyrzad = new Przyrzad(idKarty);

            button2_Click();
        }

        //*************************************************
        public PrzyrzadForm(int idKarty, string typ, string nrFabryczny)
            : this()
        //*************************************************
        {
            _Przyrzad = new Przyrzad(typ, nrFabryczny);
            WyswietlDane();
        }

        //*************************************************
        int ZnajdzNowyNumerPrzyrzadu()
        //*************************************************
        {
            return _Przyrzad.StorzNowyNumerPrzyrzadu();
        }

        //*************************************************
        // Dodaj sondę
        private void button1_Click(object sender, EventArgs e)
        //*************************************************
        {
            try
            {
                bool istnieje = _Przyrzad.SprawdzCzyPrzyrzadIsnieje(Int32.Parse(textBox6.Text));
                if(istnieje==false)
                {
                    MyMessageBox.Show(String.Format("Przyrząd o danym numerze: {0} - nie istnieje. Nie można więc dodać do niego sondy", textBox6.Text));
                    return;
                }
            }
            catch (Exception)
            {
                MyMessageBox.Show(String.Format("Przyrząd o danym numerze: {0} - nie istnieje. Nie można więc dodać do niego sondy", textBox6.Text));
                return;
            }

            _Sonda = new SondaForm(Int32.Parse(textBox6.Text));
            _Sonda.ShowDialog();
            
            _Przyrzad = new Przyrzad(textBox1.Text, textBox2.Text);

            WyswietlDane();
        }

        //*************************************************
        // zapisz przyrząd
        private bool DodajPrzyrzad()
        //*************************************************
        {
            if (false == _Przyrzad.SprawdzCzyPrzyrzadNieIsnieje(textBox1.Text, textBox2.Text))
            {
                MyMessageBox.Show("Podany przyrząd istnieje już w bazie.", "Uwaga");
                return false;
            }

            if (SprawdzPoprawnoscDanych())
            {
                _Przyrzad.Zapisz(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text);
                return true;
            }
            else
            {
                return false;
            }
        }

        //*************************************************
        private bool SprawdzMozliwoscZapisu(string typ, string nrFabryczny)
        //*************************************************
        {
            return dataGridView1.Rows.Count > 1 && SprawdzPoprawnoscDanych() && _Przyrzad.SprawdzCzyPrzyrzadNieIsnieje(typ, nrFabryczny);
        }

        //*************************************************
        private bool SprawdzPoprawnoscDanych()
        //*************************************************
        {
            return textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "" && textBox4.Text != "" && textBox5.Text != "";
        }

        //*************************************************
        private void WyswietlDane()
        //*************************************************
        {
            textBox1.Text = _Przyrzad.Typ;
            textBox2.Text = _Przyrzad.NumerFabryczny;
            textBox3.Text = _Przyrzad.RokProdukcji;
            textBox4.Text = _Przyrzad.Producent;
            textBox5.Text = _Przyrzad.Nazwa;
            textBox6.Text = _Przyrzad.NumerPrzyrzadu.ToString();

            dataGridView1.Rows.Clear();

            _Przyrzad.PobierzSondy();

            foreach (Narzedzia.Sonda sonda in _Przyrzad.Sondy.Lista)
                dataGridView1.Rows.Add(sonda.Typ, sonda.NrFabryczny);
        }

        //*************************************************
        private void Zamknij(object sender, EventArgs e)
        //*************************************************
        {
         /*   if (button1.Enabled != false || false == SprawdzMozliwoscZapisu(textBox1.Text, textBox2.Text))
            {
                if( false == _bPrzyrzadIstnieje )
                    MyMessageBox.Show("Proces dodawania nie został ukończony pomyślnie.", "Informacja");

                return;
            }

            if (_bPrzyrzadIstnieje)
                _Przyrzad.NapiszDane(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text);
            else
                _Przyrzad.Zapisz(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text);*/
        }

        //*************************************************
        private void button3_Click_1(object sender, EventArgs e)
        //*************************************************
        {
            int numer;

            try
            {
                numer = _Przyrzad.ZnajdzPoprzedni(Int32.Parse(textBox6.Text));
            }
            catch (Exception)
            {
                return;
            }

            if (-1 != numer)
            {
                if (_Przyrzad.PobierzDane(numer))
                    WyswietlDane();
            }
            else
            {
                MyMessageBox.Show("Nie istnieje przyrząd o mniejszym numerze.");
            }
        }

        //*************************************************
        private void button4_Click(object sender, EventArgs e)
        //*************************************************
        {
            int numer;

            try
            {
                numer = _Przyrzad.ZnajdzNastepny(Int32.Parse(textBox6.Text));
            }
            catch (Exception)
            {
                return;
            }

            if (-1 != numer)
            {
                if( _Przyrzad.PobierzDane(numer) )
                    WyswietlDane();
            }
            else
            {
                MyMessageBox.Show("Nie istnieje przyrząd o większym numerze.");
            }
        }

        //*************************************************
        private void button2_Click(object sender = null, EventArgs e = null)
        //*************************************************
        {
            if (_TrybDodawania)
            {
                if (true == DodajPrzyrzad())
                {
                    button2.Text = "Dodaj nowy";
                    dataGridView1.Rows.Clear();
                    textBox6.Enabled = true;
                    button1.Enabled = true;
                    button3.Enabled = true;
                    button4.Enabled = true;
                    button5.Enabled = true;

                    _TrybDodawania = false;
                }
                else
                    MyMessageBox.Show("Przyrząd nie został dodany. Błędne dane lub część danych nie została podanych.", "Uwaga");
            }
            else
            {
                textBox6.Text = ZnajdzNowyNumerPrzyrzadu().ToString();
                textBox1.Text = textBox2.Text = textBox3.Text = textBox4.Text = textBox5.Text = "";

                button2.Text = "Zatwierdź dodanie";
                dataGridView1.Rows.Clear();
                textBox6.Enabled = false;
                button1.Enabled = false;
                button3.Enabled = false;
                button4.Enabled = false;
                button5.Enabled = false;

                _TrybDodawania = true;
            }
        }

        //*************************************************
        private void button5_Click(object sender, EventArgs e)
        //*************************************************
        {
            if (SprawdzPoprawnoscDanych())
                _Przyrzad.NapiszDane(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text);
            else
                MyMessageBox.Show("Dane są niepoprawne", "Uwaga");
        }

        //*************************************************
        private void EdytujSonde(object sender, DataGridViewCellEventArgs e)
        //*************************************************
        {
            int i = e.RowIndex;
            string typSondy = dataGridView1.Rows[i].Cells[0].Value.ToString();
            string nrFabrycznySondy = dataGridView1.Rows[i].Cells[1].Value.ToString();

            _Przyrzad.ZnajdzIdDozymetru(textBox1.Text, textBox2.Text);
            
            new SondaForm(_Przyrzad.NumerPrzyrzadu, typSondy, nrFabrycznySondy).ShowDialog();

            _Przyrzad = new Przyrzad(textBox1.Text, textBox2.Text);

            WyswietlDane();
        }

    }
}
