using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Narzedzia;

namespace DotBase
{
    public partial class WzorcowanieZrodlamiPowForm : Form
    {
        private WzorcowanieZrodlamiPow _WzorcowanieZrodlamiPow;
        DocumentationPathsLoader _DocumentationPathsLoader = new DocumentationPathsLoader();

        //---------------------------------------------------------------
        public WzorcowanieZrodlamiPowForm(int idKarty, int idZrodla)
        //---------------------------------------------------------------
        {
            InitializeComponent();
         
            _WzorcowanieZrodlamiPow = new WzorcowanieZrodlamiPow(idKarty, idZrodla);

            textBoxArkusz.Text = _WzorcowanieZrodlamiPow.IdArkusza.ToString();
            textBox1.Text = idKarty.ToString();
            textBox2.Text = idZrodla.ToString();

            UstawDzisiejszaDate();

            _WzorcowanieZrodlamiPow.Inicjalizuj();


            dataGridView1.CellValueChanged += new DataGridViewCellEventHandler(LiczWskazanieMinMax);

            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(ZamykanieOkna);

            if (_WzorcowanieZrodlamiPow.SprawdzCzyArkuszJestJuzZapisany(idZrodla))
                WyswietlDane();
            else
                WyswietlDanePodstawowe();
        }
        //---------------------------------------------------------------
        private void BlokujPrzyciski()
        //---------------------------------------------------------------
        {
            MessageBox.Show("Blokuj");
        }

        //---------------------------------------------------------------
        // Szukanie poprzedniego arkusza dla danego źródła i karty
        private void button3_Click(object sender, EventArgs e)
        //---------------------------------------------------------------
        {
            if ( false == ZapiszDane() )
                return;

            if (false == _WzorcowanieZrodlamiPow.ZnajdzMniejszyArkusz())
            {
                MessageBox.Show("Nie istnieje mniejszy arkusz dla danego źródła.");
                return;
            }

            CzyscOkna();
            WyswietlDane();
        }


        //---------------------------------------------------------------
        // Szukanie następnego arkusza dla danego źródła i karty
        private void button4_Click(object sender, EventArgs e)
        //---------------------------------------------------------------
        {
            if ( false == ZapiszDane() )
                return;

            CzyscOkna();

            if (false == _WzorcowanieZrodlamiPow.ZnajdzWiekszyArkusz())
            {
                _WzorcowanieZrodlamiPow.StworzNowyArkusz();
                WyswietlDanePodstawowe();
                MessageBox.Show("To nowy arkusz. Zostanie on zapisany automatycznie po wprowadzeniu danych.");
            }
            else
            {
                WyswietlDanePodstawowe();
                WyswietlDane();
            }
        }

        //---------------------------------------------------------------
        private void button5_Click(object sender, EventArgs e)
        //---------------------------------------------------------------
        {
            comboBox2.Items.Clear();
            comboBox2.Text = "";
            comboBox3.Items.Clear();
            comboBox3.Text = "";

            if (0 == textBox3.Text.Length || 0 == textBox4.Text.Length)
            {
                MessageBox.Show("Brak odpowiednich danych do wyszukania sond.");
                return;
            }

            DataTable dane = _WzorcowanieZrodlamiPow.ZnajdzSondy(textBox3.Text, textBox4.Text);

            if (null == dane)
            {
                return;
            }

            foreach (DataRow row in dane.Rows)
            {
                // typ
                comboBox2.Items.Add(row[0]);
                // nr fabryczny
                comboBox3.Items.Add(row[1]);
            }
        }
        
        //---------------------------------------------------------------
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        //---------------------------------------------------------------
        {
            comboBox3.SelectedIndex = comboBox2.SelectedIndex;
        }

        //---------------------------------------------------------------
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        //---------------------------------------------------------------
        {
            comboBox2.SelectedIndex = comboBox3.SelectedIndex;
        }

        //---------------------------------------------------------------
        protected void CzyscOkna()
        //---------------------------------------------------------------
        {
            textBox7.Text = textBox8.Text = "nie dotyczy";
            textBox3.Text = textBox4.Text = comboBox2.Text = comboBox3.Text = textBox9.Text = 
            textBox10.Text = textBox11.Text = textBox12.Text = textBox13.Text =
            textBox14.Text = textBox15.Text = textBox16.Text = textBox17.Text = textBox18.Text =
            textBox19.Text = textBox20.Text = textBox21.Text = textBox22.Text = "";
            comboBox2.Items.Clear();
            comboBox3.Items.Clear();
            dataGridView1.Rows.Clear();
        }

        //---------------------------------------------------------------
        private void LiczWskazanieMinMax(object sender, EventArgs e)
        //---------------------------------------------------------------
        {
            double min, max;

            foreach( DataGridViewRow row in dataGridView1.Rows )
            {
                try
                {
                    if (true == double.TryParse(row.Cells["Min"].Value.ToString(), out min) && true == double.TryParse(row.Cells["Max"].Value.ToString(), out max))
                    {
                        row.Cells["Wskazanie"].Value = (min + max) / 2;
                    }
                }
                catch (Exception)
                {
                    // część danych w wierszach może być niewypełnionych bądź wypełnionych błędnie. Tutaj tłumimy błędy tym spowodowane.
                }
            }
        }

        #region Nadpisywanie Danych

        //---------------------------------------------------------------
        private void NadpiszDaneOgolne()
        //---------------------------------------------------------------
        {
            _WzorcowanieZrodlamiPow.NadpiszDaneOgolne();
        }

        //---------------------------------------------------------------
        private void NadpiszDaneWzorcoweIPomiarowe()
        //---------------------------------------------------------------
        {
            _WzorcowanieZrodlamiPow.NadpiszDaneWzorcoweIPomiarowe();
        }

        //---------------------------------------------------------------
        private void NadpiszDaneObliczonychWspolczynnikow()
        //---------------------------------------------------------------
        {
            _WzorcowanieZrodlamiPow.NadpiszDaneObliczonychWspolczynnikow();
        }

        //---------------------------------------------------------------
        private void NadpiszDanePrzyrzadu()
        //---------------------------------------------------------------
        {
            _WzorcowanieZrodlamiPow.NadpiszDanePrzyrzadu();
        }

        //---------------------------------------------------------------
        private void NadpiszDaneWarunkow()
        //---------------------------------------------------------------
        {
            _WzorcowanieZrodlamiPow.NadpiszDaneWarunkow();
        }

        #endregion

        //---------------------------------------------------------------
        private void OdblokujPrzyciski()
        //---------------------------------------------------------------
        {

        }

        //---------------------------------------------------------------
        private bool PrzygotujDaneDoZapisu(bool NadpisywanieStarychDanych)
        //---------------------------------------------------------------
        {
            if (false == _WzorcowanieZrodlamiPow.PrzygotujDaneOgolneDoZapisu(textBox1.Text, textBoxArkusz.Text, dateTimePicker1.Value, NadpisywanieStarychDanych, textBox2.Text))
            {
                MessageBox.Show("Błędne dane ogólne.", "Uwaga");
                return false;
            }

            if (false == _WzorcowanieZrodlamiPow.PrzygotujDanePrzyrzaduDoZapisu(textBox3.Text, textBox4.Text, textBox7.Text, textBox9.Text, comboBox2.Text, comboBox3.Text, textBox8.Text))
            {
                MessageBox.Show("Błędne dane przyrządu.", "Uwaga");
                return false;
            }

            if (false == _WzorcowanieZrodlamiPow.PrzygotujDaneWarunkowDoZapisu(textBox14.Text, textBox15.Text, textBox16.Text, textBox12.Text, textBox11.Text, textBox10.Text))
            {
                MessageBox.Show("Błędne dane warunków.", "Uwaga");
                return false;
            }

            if (false == _WzorcowanieZrodlamiPow.PrzygotujDaneWzorcowoPomiaroweDoZapisu(ref dataGridView1, comboBox1.Text, textBox13.Text))
            {
                MessageBox.Show("Błędne dane wzorcowo-pomiarowe.", "Uwaga");
                return false;
            }

            if (false == _WzorcowanieZrodlamiPow.PrzygotujDaneWspolczynnikowDoZapisu(textBox21.Text, textBox22.Text, textBox17.Text, textBox18.Text, checkBox1.Checked))
            {
                MessageBox.Show("Błędne dane współczynników.", "Uwaga");
                return false;
            }
            
            return true;
        }

        //---------------------------------------------------------------
        private bool SprawdzPoprawnoscDanych()
        //---------------------------------------------------------------
        {
            if (textBox3.Text == "" || textBox4.Text == "" || comboBox2.SelectedIndex < 0 || comboBox3.SelectedIndex < 0)
            {
                MessageBox.Show("Dane przyrządu są niepoprawne.","Uwaga");
                return false;
            }

            try
            {
                int.Parse(textBox1.Text);
                int.Parse(textBox2.Text);
                int.Parse(textBoxArkusz.Text);
            }
            catch(Exception)
            {
                return false;
            }

            if (textBox7.Text == "")
                textBox7.Text = "nie dotyczy";

            if (textBox8.Text == "")
                textBox8.Text = "nie dotyczy";

            if (textBox9.Text == "")
                textBox9.Text = "Brak";

            if (textBox10.Text == "")
                textBox10.Text = "1";

            if (textBox11.Text == "")
                textBox11.Text = "2,6";

            if (textBox12.Text == "")
                textBox12.Text = "-";

            if (textBox13.Text == "")
                textBox13.Text = "Brak";

            if (textBox14.Text == "")
                textBox14.Text = "0";

            if (textBox15.Text == "")
                textBox15.Text = "0";

            if (textBox16.Text == "")
                textBox16.Text = "0";

            if (textBox17.Text == "")
                textBox17.Text = "0";

            if (textBox18.Text == "")
                textBox18.Text = "0";

            if (comboBox1.Text == "")
                comboBox1.SelectedIndex = comboBox1.Items.IndexOf("s-1");

            if (textBox21.Text == "")
                textBox21.Text = "B. Winiarska";

            if (textBox22.Text == "")
                textBox22.Text = "P. Bilski";

            return true;
        }

        #region Wyswietlanie Danych

        //---------------------------------------------------------------
        public void WyswietlDane()
        //---------------------------------------------------------------
        {
            ZnajdzJednostki();

            if (false == _WzorcowanieZrodlamiPow.Inicjalizuj())
            {
                MessageBox.Show("Brak odpowiednich danych do wyswietlenia lub połączenie z bazą zostało przerwane.");
                return;
            }

            textBoxArkusz.Text = _WzorcowanieZrodlamiPow.IdArkusza.ToString();

            if (WyswietlDanePrzyrzadow() && WyswietlDanePomiarow() && WyswietlDaneWarunkow() && WyswietlDaneWspolczynnikow())
            {
                dateTimePicker1.Value = _WzorcowanieZrodlamiPow.DanePrzyrzadow._Data;

                if (true == _WzorcowanieZrodlamiPow.PobierzIdPoprzedniegoWzorcowania())
                {
                    textBox19.Text = _WzorcowanieZrodlamiPow.DaneWspolczynnikow.wspolczynnikPoprzedni.ToString();
                    textBox20.Text = _WzorcowanieZrodlamiPow.DaneWspolczynnikow.niepewnoscPoprzednia.ToString();
                }

                ZnajdzPoprzedniWspolczynnikOrazNiepewnosc();
            }
            else
            {
                MessageBox.Show("Brak odpowiednich danych do wyswietlenia lub połączenie z bazą zostało przerwane.");
            }
        }


        //---------------------------------------------------------------
        private bool WyswietlDanePodstawowe()
        //---------------------------------------------------------------
        {
            if (false == _WzorcowanieZrodlamiPow.PobierzPodstawoweDanePrzyrzadu())
                return false;

            comboBox2.Items.Clear();
            comboBox3.Items.Clear();

            textBox3.Text = _WzorcowanieZrodlamiPow.DanePrzyrzadow.typDozymetru;
            textBox4.Text = _WzorcowanieZrodlamiPow.DanePrzyrzadow.nrFabrycznyDozymetru;
            textBox9.Text = "nie dotyczy";

            dateTimePicker1.Value = DateTime.Today;
            textBoxArkusz.Text = _WzorcowanieZrodlamiPow.IdArkusza.ToString();

            ZnajdzJednostki();
            _WzorcowanieZrodlamiPow.PobierzMozliweSondy();

            checkBox1.Checked = true;

            foreach (Narzedzia.Sonda sonda in _WzorcowanieZrodlamiPow.DanePrzyrzadow.sondy.Lista)
            {
                comboBox2.Items.Add(sonda.Typ);
                comboBox3.Items.Add(sonda.NrFabryczny);
            }

            int nrPoprzedniejKalibracji = _WzorcowanieZrodlamiPow.ZnajdzNrPoprzedniejKalibracji(textBox3.Text, textBox4.Text);

            if (0 < nrPoprzedniejKalibracji)
            {
                textBox13.Clear();
                textBox13.Text = "Numer poprzedniego świadectwa: " + nrPoprzedniejKalibracji + "/" + _WzorcowanieZrodlamiPow.PobierzRok(nrPoprzedniejKalibracji);
            }

            return true;
        }

        //---------------------------------------------------------------
        private bool WyswietlDanePomiarow()
        //---------------------------------------------------------------
        {
            if (false == _WzorcowanieZrodlamiPow.PobierzDanePomiarow())
                return false;

            dataGridView1.Rows.Clear();

            comboBox1.Text = _WzorcowanieZrodlamiPow.DanePomiary.jednostka;
            textBox13.Text = _WzorcowanieZrodlamiPow.DanePomiary.uwagi;

            for (int i = 0; i < _WzorcowanieZrodlamiPow.DanePomiary.pomiar.Count; ++i)
            {
                dataGridView1.Rows.Add(_WzorcowanieZrodlamiPow.DanePomiary.pomiar[i].ToString(), _WzorcowanieZrodlamiPow.DanePomiary.tlo[i].ToString(), "", "");
            }

            return true;
        }

        //---------------------------------------------------------------
        private bool WyswietlDanePrzyrzadow()
        //---------------------------------------------------------------
        {
            if (false == _WzorcowanieZrodlamiPow.PobierzDanePrzyrzadow())
                return false;

            comboBox2.Items.Clear();
            comboBox3.Items.Clear();

            textBox3.Text = _WzorcowanieZrodlamiPow.DanePrzyrzadow.typDozymetru;
            textBox4.Text = _WzorcowanieZrodlamiPow.DanePrzyrzadow.nrFabrycznyDozymetru;
            
            foreach(Narzedzia.Sonda sonda in _WzorcowanieZrodlamiPow.DanePrzyrzadow.sondy.Lista)
            {
                comboBox2.Items.Add( sonda.Typ );
                comboBox3.Items.Add( sonda.NrFabryczny );
            }

            comboBox2.Text = _WzorcowanieZrodlamiPow.DanePrzyrzadow.typSondy;
            comboBox3.Text = _WzorcowanieZrodlamiPow.DanePrzyrzadow.nrFabrycznySondy;

            textBox7.Text = _WzorcowanieZrodlamiPow.DanePrzyrzadow.napiecieZasilaniaSondy;
            textBox8.Text = _WzorcowanieZrodlamiPow.DanePrzyrzadow.zakres;
            textBox9.Text = _WzorcowanieZrodlamiPow.DanePrzyrzadow.inneNastawy;

            return true;
        }

        //---------------------------------------------------------------
        private bool WyswietlDaneWarunkow()
        //---------------------------------------------------------------
        {
            if (false == _WzorcowanieZrodlamiPow.PobierzDaneWarunkow())
                return false;

            textBox14.Text = _WzorcowanieZrodlamiPow.DaneWarunki.cisnienie.ToString("F1");
            textBox15.Text = _WzorcowanieZrodlamiPow.DaneWarunki.temperatura.ToString("F1");
            textBox16.Text = _WzorcowanieZrodlamiPow.DaneWarunki.wilgotnosc.ToString("F1");

            textBox12.Text = _WzorcowanieZrodlamiPow.DaneWarunki.podstawka.ToString();
            textBox11.Text = _WzorcowanieZrodlamiPow.DaneWarunki.odleglosc.ToString("G");
            textBox10.Text = _WzorcowanieZrodlamiPow.DaneWarunki.mnoznikKorekcyjny.ToString("G");

            return true;
        }

        //---------------------------------------------------------------
        private bool WyswietlDaneWspolczynnikow()
        //---------------------------------------------------------------
        {
            if (false == _WzorcowanieZrodlamiPow.PobierzDaneWspolczynnikow())
                return false;

            textBox17.Text = _WzorcowanieZrodlamiPow.DaneWspolczynnikow.wspolczynnik.ToString();
            textBox18.Text = _WzorcowanieZrodlamiPow.DaneWspolczynnikow.niepewnnosc.ToString();
            textBox21.Text = _WzorcowanieZrodlamiPow.DaneWspolczynnikow.osobaWzorcujaca;
            textBox22.Text = _WzorcowanieZrodlamiPow.DaneWspolczynnikow.osobaSprawdzajaca;
            checkBox1.Checked = _WzorcowanieZrodlamiPow.DaneWspolczynnikow.dolaczyc;

            return true;
        }

        #endregion

        //---------------------------------------------------------------
        private void UstawDzisiejszaDate()
        //---------------------------------------------------------------
        {
            dateTimePicker1.Value = DateTime.Today;
        }

        //---------------------------------------------------------------
        // Sprawdza czy dane zostały już wcześniej zapisane.
        // Jeśli tak - zostanie wywołana grupa metod nadpisujących dane
        // Jeśli nie - zostanie wywołana grupa metod zapisujących dane
        public bool ZapiszDane()
        //---------------------------------------------------------------
        {
            if (false == SprawdzPoprawnoscDanych())
                return false;

            if (true == _WzorcowanieZrodlamiPow.SprawdzCzyArkuszJestJuzZapisany())
            {
                if (true == PrzygotujDaneDoZapisu(true))
                {
                    NadpiszDaneOgolne();
                    NadpiszDanePrzyrzadu();
                    NadpiszDaneWarunkow();
                    NadpiszDaneWzorcoweIPomiarowe();
                    NadpiszDaneObliczonychWspolczynnikow();
                }
                else
                {
                    MessageBox.Show("Część danych jest niepoprawnych lub niepodanych. Nadpisanie danych niemożliwe", "Uwaga");
                    return false;
                }
            }
            else
            {
                if (true == PrzygotujDaneDoZapisu(false))
                {
                    ZapiszDaneOgolne();
                    ZapiszDanePrzyrzadu();
                    ZapiszDaneWarunkow();
                    ZapiszDaneWzorcoweIPomiarowe();
                    ZapiszDaneObliczonychWspolczynnikow();
                }
                else
                {
                    MessageBox.Show("Część danych jest niepoprawnych lub niepodanych. Nadpisanie danych niemożliwe", "Uwaga");
                    return false;
                }
            }

            return true;
        }

        #region Zapisywanie Danych

        //---------------------------------------------------------------
        private void ZapiszDaneOgolne()
        //---------------------------------------------------------------
        {
            _WzorcowanieZrodlamiPow.ZapiszDaneOgolne();
        }

        //---------------------------------------------------------------
        private void ZapiszDanePrzyrzadu()
        //---------------------------------------------------------------
        {
            _WzorcowanieZrodlamiPow.ZapiszDanePrzyrzadu();
        }

        //---------------------------------------------------------------
        private void ZapiszDaneWarunkow()
        //---------------------------------------------------------------
        {
            _WzorcowanieZrodlamiPow.ZapiszDaneWarunkow();
        }

        //---------------------------------------------------------------
        private void ZapiszDaneWzorcoweIPomiarowe()
        //---------------------------------------------------------------
        {
            _WzorcowanieZrodlamiPow.ZapiszWartosciWzorcowoPomiarowe();
        }

        //---------------------------------------------------------------
        private void ZapiszDaneObliczonychWspolczynnikow()
        //---------------------------------------------------------------
        {
            _WzorcowanieZrodlamiPow.ZapiszDaneObliczonychWspolczynnikow();
        }

        #endregion

        //---------------------------------------------------------------
        public void ZnajdzJednostki()
        //---------------------------------------------------------------
        {
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(_WzorcowanieZrodlamiPow.ZnajdzJednostki().ToArray());
        }

        //---------------------------------------------------------------
        public void ZnajdzPoprzedniWspolczynnikOrazNiepewnosc()
        //---------------------------------------------------------------
        {
            double wspolczynnik, niepewnosc;

            if ( true == _WzorcowanieZrodlamiPow.ZnajdzPoprzedniWspolczynnikOrazNiepewnosc(out wspolczynnik, out niepewnosc, dateTimePicker1.Value) )
            {
                textBox19.Text = wspolczynnik.ToString();
                textBox20.Text = niepewnosc.ToString();
            }
        }

        //---------------------------------------------------------------
        // obliczanie współczynników
        private void button1_Click(object sender, EventArgs e)
        //---------------------------------------------------------------
        {
            double wspolczynnik_kalibracyjny;
            double niepewnosc;
            int precyzja;

            _WzorcowanieZrodlamiPow.LiczWspolczynnikiOrazNiepewnosc(out wspolczynnik_kalibracyjny, out niepewnosc, out precyzja, ref dataGridView1, dateTimePicker1.Value, double.Parse(textBox10.Text));

            if (Double.IsInfinity(niepewnosc) || Double.IsNaN(niepewnosc))
                textBox18.Text = "-1";
            else
                textBox18.Text = niepewnosc.ToString();

            if (Double.IsInfinity(wspolczynnik_kalibracyjny) || Double.IsNaN(wspolczynnik_kalibracyjny))
                textBox17.Text = "-1";
            else
                textBox17.Text = wspolczynnik_kalibracyjny.ToString();

            _WzorcowanieZrodlamiPow.ZnajdzPoprzedniWspolczynnikINiepewnosc(ref textBox19, ref textBox20, textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, comboBox2.Text, comboBox3.Text);
        }

        //---------------------------------------------------------------
        void ZamykanieOkna(object sender, FormClosingEventArgs args)
        //---------------------------------------------------------------
        {
            if (false == ZapiszDane())
            {
                if (MessageBox.Show("Czy na pewno chcesz zamknąć okno? Dane nie zostaną zapisany z uwagi na błędy wśród wpisanych danych.", "Uwaga", MessageBoxButtons.YesNo) == DialogResult.No)
                    args.Cancel = true;
            }
        }

        //---------------------------------------------------------------
        private void button2_Click(object sender, EventArgs e)
        //---------------------------------------------------------------
        {
            string sciezka = _DocumentationPathsLoader.GetPath("ProtokolySkazeniaWynik") + textBox1.Text + "WynikSkazenia.html";

            Dokumenty.ProtokolZrodlaPowierzchniowe dokument = new Dokumenty.ProtokolZrodlaPowierzchniowe();
            dokument.PobierzDanePodstawowe(textBox1.Text, textBoxArkusz.Text, dateTimePicker1.Value, textBox2.Text);
            dokument.PobierzDanePomiarow(comboBox1.Text, textBox13.Text, ref dataGridView1);
            dokument.PobierzDanePrzyrzadu(textBox3.Text, textBox4.Text, textBox8.Text, textBox9.Text, comboBox2.Text, comboBox3.Text, textBox7.Text);
            dokument.PobierzDaneWarunkow(textBox14.Text, textBox15.Text, textBox16.Text, textBox13.Text, textBox12.Text, textBox11.Text, textBox10.Text);
            dokument.PobierzDaneWspolczynnikow(textBox21.Text, textBox22.Text, textBox17.Text, textBox18.Text, textBox19.Text);
            dokument.UtworzDokument(sciezka);
            System.Diagnostics.Process.Start(sciezka);

        }

        private void PrzejdzDoKolejnegoPola(object sender, KeyPressEventArgs e)
        {
            // if enter
            if (e.KeyChar == 13)
                SelectNextControl((Control)sender, true, false, true, true);
        }

        private void textBox14_Leave(object sender, EventArgs e)
        {
            textBox14.Text = Narzedzia.Format.PoprawFormat(textBox14.Text, 1);
        }

        private void textBox15_Leave(object sender, EventArgs e)
        {
            textBox15.Text = Narzedzia.Format.PoprawFormat(textBox15.Text, 1);
        }

        private void textBox16_Leave(object sender, EventArgs e)
        {
            textBox16.Text = Narzedzia.Format.PoprawFormat(textBox16.Text, 1);
        }
    }
}
