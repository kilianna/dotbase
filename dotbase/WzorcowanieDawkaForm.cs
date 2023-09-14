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
    public partial class WzorcowanieDawkaForm : Form
    {
        WzorcowanieDawka _WzorcowanieDawka;
        DocumentationPathsLoader _DocumentationPathsLoader = new DocumentationPathsLoader();

        //-------------------------------------------------------------------
        public WzorcowanieDawkaForm(int nrKarty)
        //-------------------------------------------------------------------
        {
            InitializeComponent();
            _WzorcowanieDawka = new WzorcowanieDawka(nrKarty, "d");
        
            textBox1.Text = nrKarty.ToString();
            comboBox1.SelectedIndexChanged += new System.EventHandler(comboBox1_SelectedIndexChanged);
            comboBox2.SelectedIndexChanged += new System.EventHandler(comboBox2_SelectedIndexChanged);
            //comboBox4.SelectedIndexChanged += new System.EventHandler(LiczWartoscWzorcowa);

            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(ZamykanieOkna);

            textBox15.Text = Properties.Settings.Default.Wykonal;
            textBox16.Text = Properties.Settings.Default.Sprawdzil;
        }

        //---------------------------------------------------------------
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs args)
        //---------------------------------------------------------------
        {
            comboBox2.SelectedIndex = comboBox1.SelectedIndex;
        }

        //---------------------------------------------------------------
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs args)
        //---------------------------------------------------------------
        {
            comboBox1.SelectedIndex = comboBox2.SelectedIndex;
        }

        //---------------------------------------------------------------
        private int getRownowaznikDawki()
        //---------------------------------------------------------------
        {
            if (radioButton1.Checked)
                return 0;
            else if (radioButton2.Checked)
                return 1;
            else if (radioButton3.Checked)
                return 2;
            else
                return 3;
        }

        //---------------------------------------------------------------
        private void CzyscOkno()
        //---------------------------------------------------------------
        {
            comboBox1.Text = comboBox2.Text = comboBox3.Text = "";
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            comboBox3.Items.Clear();

            textBox5.Text = "nie dotyczy";
            textBox9.Text = "nie dotyczy";
            
            textBox7.Text = textBox8.Text = textBox9.Text = textBox11.Text = textBox12.Text = textBox13.Text =
            textBox14.Text = textBox15.Text = textBox17.Text = textBox18.Text = "";
            textBox10.Text = "brak";
            textBox16.Text = "P. Bilski";
        
            dataGridView1.Rows.Clear();

            checkBox1.Checked = true;

            _WzorcowanieDawka.CzyscStareDane();

            textBox15.Text = Properties.Settings.Default.Wykonal;
            textBox16.Text = Properties.Settings.Default.Sprawdzil;
        }

        //---------------------------------------------------------------
        public bool Inicjalizuj()
        //---------------------------------------------------------------
        {
            if (_WzorcowanieDawka.Inicjalizuj())
            {
                textBox2.Text = _WzorcowanieDawka.IdArkusza.ToString();
                UstawDzisiejszaDate();
                WyswietlDane();
                return true;
            }
            else
            {
                textBox2.Text = _WzorcowanieDawka.IdArkusza.ToString();
                UstawDzisiejszaDate();
                WyswietlDanePodstawowe();
                return true;
            }
        }

        //---------------------------------------------------------------
        private bool PrzygotujDaneDoZapisu(bool NadpisywanieStarychDanych)
        //---------------------------------------------------------------
        {
            if (false == SprawdzPoprawnoscDanych())
                return false;

            if (false == _WzorcowanieDawka.PrzygotujDaneOgolneDoZapisu(textBox1.Text, textBox2.Text, dateTimePicker1.Value, NadpisywanieStarychDanych))
            {
                MessageBox.Show("Dane ogólne są błędne.", "Uwaga");
                return false;
            }

            if (false == _WzorcowanieDawka.PrzygotujDanePrzyrzaduDoZapisu(textBox3.Text, textBox4.Text, textBox6.Text, textBox5.Text, comboBox1.Text, comboBox2.Text))
            {
                MessageBox.Show("Dane przyrządu są błędne.", "Uwaga");
                return false;
            }

            if (false == _WzorcowanieDawka.PrzygotujDaneWarunkowDoZapisu(textBox7.Text, textBox8.Text, textBox9.Text, textBox10.Text))
            {
                MessageBox.Show("Dane warunków są błędne.", "Uwaga");
                return false;
            }

            if (false == _WzorcowanieDawka.PrzygotujDaneWzorcowoPomiaroweDoZapisu(ref dataGridView1, comboBox3.Text, textBox17.Text, textBox18.Text))
            {
                MessageBox.Show("Dane wzorcowo-pomiarowe są błędne.", "Uwaga");
                return false;
            }

            if (false == _WzorcowanieDawka.PrzygotujDaneWspolczynnikowDoZapisu(textBox15.Text, textBox16.Text, checkBox1.Checked, textBox11.Text, textBox12.Text, textBox18.Text, textBox19.Text, getRownowaznikDawki()))
            {
                MessageBox.Show("Dane współczynników są błędne.", "Uwaga");
                return false;
            }
            
            return true;
        }

        //---------------------------------------------------------------
        private bool SprawdzPoprawnoscDanych()
        //---------------------------------------------------------------
        {
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "" ||
                comboBox1.SelectedIndex < 0 || comboBox2.SelectedIndex < 0)
            {
                MessageBox.Show("Błędne dane przyrządu.", "Uwaga");
                return false;
            }

            // napięcie zasilania
            if (textBox5.Text == "")
                textBox5.Text = "nie dotyczy";

            // inne nastawy
            if (textBox6.Text == "")
                textBox6.Text = "Brak";

            // ciśnienie
            if (textBox7.Text == "")
                textBox7.Text = "0";

            // temperatura
            if (textBox8.Text == "")
                textBox8.Text = "0";

            // wilgotność
            if (textBox9.Text == "")
                textBox9.Text = "0";

            // Uwagi
            if (textBox10.Text == "")
                textBox10.Text = "Brak";

            // jeśli nie podano wybierz ostatni protokół
            if (comboBox3.Text == "")
                comboBox3.SelectedItem = comboBox3.Items[comboBox3.Items.Count - 1];
            
            // źródło
            if (textBox17.Text == "")
                textBox17.Text = "0";

            // odległość
            if (textBox18.Text == "")
                textBox18.Text = "0";

            // współczynnik
            if (textBox11.Text == "")
                textBox11.Text = "0";

            // niepewność
            if (textBox12.Text == "")
                textBox12.Text = "0";

            // wykonał
            if (textBox15.Text == "")
                textBox15.Text = " ";

            // sprawdził
            if (textBox16.Text == "")
                textBox16.Text = " ";

            return true;
        }

        //---------------------------------------------------------------
        private void UstawDzisiejszaDate()
        //---------------------------------------------------------------
        {
            dateTimePicker1.Value = DateTime.Today;
        }

        #region Wyswietlanie

        //---------------------------------------------------------------
        private void WyswietlDane()
        //---------------------------------------------------------------
        {
            CzyscOkno();
            _WzorcowanieDawka.PobierzIdWzorcowania();
            textBox2.Text = _WzorcowanieDawka.IdArkusza.ToString();
            WyswietlDanePrzyrzadu();
            WyswietlDateWzorcowania();
            //    WyswietlSondyDlaDanegoWzorcowania();
            WyswietlDaneWarunkow();
            WyswietlWszystkieProtokoly();
            WyswietlKonkretnyProtokol();
            WyswietlDaneWzorcoweIPomiarowe();
            WyswietlDaneWspolczynnikow();
            WyswietlWielkoscFizyczna();
        }

        //---------------------------------------------------------------
        private void WyswietlDanePodstawowe()
        //---------------------------------------------------------------
        {
            CzyscOkno();

            textBox2.Text = _WzorcowanieDawka.IdArkusza.ToString();
            dateTimePicker1.Value = DateTime.Today;

            WyswietlPodstawoweDanePrzyrzadu();
            WyswietlWszystkieProtokoly();
            WyswietlWszystkieSondy();
            WyswietlPoprzedniWspolczynnikOrazNiepewnosc();
            radioButton1.Checked = true;

            int nrPoprzedniejKalibracji = _WzorcowanieDawka.ZnajdzNrPoprzedniejKalibracji(textBox3.Text, textBox4.Text);

            if (0 < nrPoprzedniejKalibracji)
            {
                textBox10.Text = "Numer poprzedniego świadectwa " + nrPoprzedniejKalibracji + "/" + _WzorcowanieDawka.PobierzRok(nrPoprzedniejKalibracji);
            }
        }

        //---------------------------------------------------------------
        private void WyswietlDanePrzyrzadu()
        //---------------------------------------------------------------
        {
            if (_WzorcowanieDawka.PobierzDanePrzyrzadu())
            {
                KlasyPomocniczeCez.Przyrzad przyrzad = _WzorcowanieDawka.Przyrzad;

                textBox3.Text = przyrzad.TypDozymetru;
                textBox4.Text = przyrzad.NrFabrycznyDozymetru;
                textBox5.Text = przyrzad.InneNastawy;
                textBox6.Text = przyrzad.NapiecieZasilaniaSondy;

                comboBox1.Items.Clear();
                comboBox2.Items.Clear();

                for (int i = 0; i < _WzorcowanieDawka.Sondy.Lista.Count; ++i)
                {
                    comboBox1.Items.Add(_WzorcowanieDawka.Sondy.Lista[i].Typ);
                    comboBox2.Items.Add(_WzorcowanieDawka.Sondy.Lista[i].NrFabryczny);
                }

                Sonda s = _WzorcowanieDawka.ZnajdzSondeWybranaPrzezUzytkwonika();

                comboBox2.SelectedIndex = comboBox2.Items.IndexOf(s.NrFabryczny);
            }
        }

        //---------------------------------------------------------------
        private void WyswietlDateWzorcowania()
        //---------------------------------------------------------------
        {
            if (_WzorcowanieDawka.PobierzDateWzorcowania())
            {
                dateTimePicker1.Value = _WzorcowanieDawka.OdpowiedzBazy.Rows[0].Field<DateTime>(0);
            }
        }

        //---------------------------------------------------------------
        private void WyswietlDaneWarunkow()
        //---------------------------------------------------------------
        {
            if (_WzorcowanieDawka.PobierzDaneWarunkow())
            {
                KlasyPomocniczeCez.Warunki warunki = _WzorcowanieDawka.Warunki;

                textBox7.Text = warunki.Cisnienie.ToString("0.0");
                textBox8.Text = warunki.Temperatura.ToString("0.0");
                textBox9.Text = warunki.Wilgotnosc.ToString("0.0");
                textBox10.Text = warunki.Uwagi;
            }
        }

        //---------------------------------------------------------------
        private void WyswietlKonkretnyProtokol()
        //---------------------------------------------------------------
        {
            if (_WzorcowanieDawka.PobierzKonkretnyProtokol())
            {
                int index = comboBox3.Items.IndexOf(_WzorcowanieDawka.WybranyProtokol);

                if (index >= 0)
                    comboBox3.SelectedIndex = index;
            }
        }

        //---------------------------------------------------------------
        private void WyswietlPodstawoweDanePrzyrzadu()
        //---------------------------------------------------------------
        {
            if (_WzorcowanieDawka.PobierzPodstawoweDanePrzyrzadu())
            {
                textBox3.Text = _WzorcowanieDawka.Przyrzad.TypDozymetru;
                textBox4.Text = _WzorcowanieDawka.Przyrzad.NrFabrycznyDozymetru;
            }

            textBox5.Text = "nie dotyczy";
        }

        //---------------------------------------------------------------
        private void WyswietlWszystkieProtokoly()
        //---------------------------------------------------------------
        {
            if (_WzorcowanieDawka.PobierzWszystkieProtokoly())
            {
                foreach (DateTime data in _WzorcowanieDawka.Protokoly.Daty)
                {
                    comboBox3.Items.Add(data.ToShortDateString());
                }
            }
        }

        //---------------------------------------------------------------
        private void WyswietlWszystkieSondy()
        //---------------------------------------------------------------
        {
            if (_WzorcowanieDawka.PobierzWszystkieMozliweSondy())
            {
                foreach (Sonda sonda in _WzorcowanieDawka.Sondy.Lista)
                {
                    comboBox1.Items.Add(sonda.Typ);
                    comboBox2.Items.Add(sonda.NrFabryczny);
                }
            }
        }

        //---------------------------------------------------------------
        private void WyswietlDaneWzorcoweIPomiarowe()
        //---------------------------------------------------------------
        {
            if (_WzorcowanieDawka.PobierzDaneWzorcoweIPomiarowe())
            {
                foreach (KlasyPomocniczeDawka.DawkaWartosciWzorcowoPomiarowe.DawkaWartoscWzorcowoPomiarowa dane in _WzorcowanieDawka.Pomiary.Dane)
                {
                    dataGridView1.Rows.Add(dane.WartoscWzorcowa, dane.Czas.ToString("0.0"), dane.Wskazanie, dane.Dolaczyc);
                }
            }

            WyswietlKonkretnaJednostke();
            WyswietlKonkretnyProtokol();

            textBox17.Text = _WzorcowanieDawka.Pomiary.zrodlo;
            textBox18.Text = _WzorcowanieDawka.Pomiary.odleglosc;
        }

        //---------------------------------------------------------------
        private void WyswietlDaneWspolczynnikow()
        //---------------------------------------------------------------
        {
            if (_WzorcowanieDawka.PobierzDaneWspolczynnikow())
            {
                textBox15.Text = _WzorcowanieDawka.Wspolczynniki.Wzorcujacy;
                textBox16.Text = _WzorcowanieDawka.Wspolczynniki.Sprawdzajacy;
                checkBox1.Checked = _WzorcowanieDawka.Wspolczynniki.Dolacz;
                textBox11.Text = _WzorcowanieDawka.Wspolczynniki.Wspolczynnik.ToString("0.000");
                textBox12.Text = _WzorcowanieDawka.Wspolczynniki.Niepewnosc.ToString("0.000");
                textBox19.Text = _WzorcowanieDawka.Wspolczynniki.Zakres.ToString();
                if (_WzorcowanieDawka.Wspolczynniki.RownowaznikDawki == 0)
                    radioButton1.Checked = true;
                else if (_WzorcowanieDawka.Wspolczynniki.RownowaznikDawki == 1)
                    radioButton2.Checked = true;
                else if (_WzorcowanieDawka.Wspolczynniki.RownowaznikDawki == 2)
                    radioButton3.Checked = true;
                else
                    radioButton4.Checked = true;
            }
            else
            {
                radioButton1.Checked = true;
            }

            WyswietlPoprzedniWspolczynnikOrazNiepewnosc();
        }

        //---------------------------------------------------------------
        private void WyswietlPoprzedniWspolczynnikOrazNiepewnosc()
        //---------------------------------------------------------------
        {
            Narzedzia.Pair<double, double> poprzedni_wzposlczynnik_niepewnosc = _WzorcowanieDawka.ZnajdzPoprzedniWspolczynnikOrazNiepewnosc();
            textBox13.Text = poprzedni_wzposlczynnik_niepewnosc.First.ToString();
            textBox14.Text = poprzedni_wzposlczynnik_niepewnosc.Second.ToString();
        }

        //---------------------------------------------------------------
        private void WyswietlKonkretnaJednostke()
        //---------------------------------------------------------------
        {
            _WzorcowanieDawka.PobierzKonkretnaJednostke();
        }

        //---------------------------------------------------------------
        private void WyswietlWielkoscFizyczna()
        //---------------------------------------------------------------
        {
       /*     if (_WzorcowanieMocDawki.PobierzKonkretnaWielkoscFizyczna(comboBox4.Text))
            {
                comboBox5.Text = _WzorcowanieMocDawki.OdpowiedzBazy.Rows[0].Field<string>(0);
            }*/
        }

        #endregion

        //---------------------------------------------------------------
        private void ZamykanieOkna(object sender, FormClosingEventArgs e)
        //---------------------------------------------------------------
        {
            e.Cancel = !N.PotwierdzenieZapisz(this, ZapiszDane, true, false);
        }
                
        //---------------------------------------------------------------
        private bool ZapiszDane()
        //---------------------------------------------------------------
        {
            if (true == _WzorcowanieDawka.SprawdzCzyArkuszJestJuzZapisany())
            {
                if (false == PrzygotujDaneDoZapisu(true))
                    return false;

                // Nadpisywanie danych
                _WzorcowanieDawka.NadpiszDane();
            }
            else
            {
                if (false == PrzygotujDaneDoZapisu(false))
                    return false;

                Properties.Settings.Default.Wykonal = textBox15.Text;
                Properties.Settings.Default.Sprawdzil = textBox16.Text;
                Properties.Settings.Default.Save();

                // Zapisywanie danych
                _WzorcowanieDawka.ZapiszDane();
            }

            return true;
        }

        //---------------------------------------------------------------
        private void PoprzedniArkusz(object sender, EventArgs e)
        //---------------------------------------------------------------
        {
            if (!N.PotwierdzenieZapisz(this, ZapiszDane, true, false))
                return;

            if (true == _WzorcowanieDawka.ZnajdzMniejszyArkusz())
            {
                WyswietlDane();
            }
            else
            {
                MessageBox.Show("Nie istnieje mniejszy arkusz dla danego wzorcowania.");
            }
        }

        //---------------------------------------------------------------
        private void NastepnyArkusz(object sender, EventArgs e)
        //---------------------------------------------------------------
        {
            if (!N.PotwierdzenieZapisz(this, ZapiszDane, true, false))
                return;

            if (true == _WzorcowanieDawka.ZnajdzWiekszyArkusz())
            {
                WyswietlDane();
            }
            else
            {
                _WzorcowanieDawka.StworzNowyArkusz();
                WyswietlDanePodstawowe();
                MessageBox.Show("To nowy arkusz który nie istnieje jeszcze w bazie. Zostanie on zapisany automatycznie.", "Uwaga");
            }
        }

        // poprzedni arkusz
        //---------------------------------------------------------------
        private void button1_Click(object sender, EventArgs e)
        //---------------------------------------------------------------
        {
            if (!N.PotwierdzenieZapisz(this, ZapiszDane, true, false))
                return;

            if (true == _WzorcowanieDawka.ZnajdzMniejszyArkusz())
            {
                WyswietlDane();
            }
            else
            {
                MessageBox.Show("Nie istnieje mniejszy arkusz dla danego wzorcowania.");
            }
        }

        // następny arkusz
        //---------------------------------------------------------------
        private void button2_Click(object sender, EventArgs e)
        //---------------------------------------------------------------
        {
            if (!N.PotwierdzenieZapisz(this, ZapiszDane, true, false))
                return;

            if (true == _WzorcowanieDawka.ZnajdzWiekszyArkusz())
            {
                WyswietlDane();
            }
            else
            {
                _WzorcowanieDawka.StworzNowyArkusz();
                WyswietlDanePodstawowe();
                MessageBox.Show("To nowy arkusz który nie istnieje jeszcze w bazie. Zostanie on zapisany automatycznie.", "Uwaga");
            }
        }

        // licz
        //---------------------------------------------------------------
        private void button5_Click(object sender, EventArgs e)
        //---------------------------------------------------------------
        {
            List<double> dane = _WzorcowanieDawka.LiczCzas(ref dataGridView1, comboBox3.Text, dateTimePicker1.Value, textBox18.Text, textBox17.Text, getRownowaznikDawki());
            
            for (int i = 0; i < dane.Count; ++i)
            {
                dataGridView1.Rows[i].Cells[1].Value = dane[i].ToString("0.0");
            }
        }

        // dołącz wszystko
        private void button6_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count - 1; ++i)
            {
                dataGridView1.Rows[i].Cells[3].Value = true;
            }
        }

        // oblicz współczynniki
        private void button4_Click(object sender, EventArgs e)
        {
            List<double[]> inputList = new List<double[]>();

            try
            {
                for (UInt16 i = 0; i < dataGridView1.Rows.Count - 1; ++i)
                {
                    DataGridViewRow row = dataGridView1.Rows[i];

                    if (row.Cells["WartoscWzorcowa"].Value != null && row.Cells["Wskazanie"].Value != null && row.Cells["Dolaczyc"].Value != null)
                    {
                        if ((bool)row.Cells["Dolaczyc"].Value == true)
                            inputList.Add(new double[] { 
                                N.doubleParse(row.Cells["WartoscWzorcowa"].Value.ToString()),
                                N.doubleParse(row.Cells["Wskazanie"].Value.ToString()),
                                N.doubleParse(row.Cells["Column2"].Value.ToString())
                            });
                    }
                    else
                        return;
                }
            }
            catch (InvalidCastException)
            {
                return;
            }

            Narzedzia.Pair<double,double> wspolczynnik_niepewnosc;

            if (dateTimePicker1.Value >= DateTime.Parse("2023-09-15"))
            {
                wspolczynnik_niepewnosc = _WzorcowanieDawka.LiczWspolczynnikOrazNiepewnosc20230915(inputList, N.doubleParse(textBox18.Text), comboBox3.Text, Int32.Parse(textBox17.Text), dateTimePicker1.Value);
            }
            else
            {
                wspolczynnik_niepewnosc = _WzorcowanieDawka.LiczWspolczynnikOrazNiepewnoscOld(inputList);
            }
            
            textBox11.Text = wspolczynnik_niepewnosc.First.ToString("0.000");
            textBox12.Text = wspolczynnik_niepewnosc.Second.ToString("0.000");

            Narzedzia.Pair<double, double> poprzedni_wspolczynnik_niepewnosc = _WzorcowanieDawka.ZnajdzPoprzedniWspolczynnikOrazNiepewnosc();
            textBox13.Text = poprzedni_wspolczynnik_niepewnosc.First.ToString();
            textBox14.Text = poprzedni_wspolczynnik_niepewnosc.Second.ToString();
        }

        // rysowanie wykresu
        private void wykresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Jezyk jezyk = Jezyk.PL;
            if (sender == wykresENToolStripMenuItem)
            {
                jezyk = Jezyk.EN;
            }
            Wykres.WykresForm wykres = new Wykres.WykresForm(false, false, false, jezyk);
            wykres.CzyscDane();

            DataGridViewRow wiersz;

            for (int i = 0; dataGridView1.Rows[i].Cells["WartoscWzorcowa"].Value != null &&
                            dataGridView1.Rows[i].Cells["Wskazanie"].Value != null &&
                            dataGridView1.Rows[i].Cells["Dolaczyc"].Value != null; ++i)
            {
                wiersz = dataGridView1.Rows[i];
                
                // Dla dawki zakres zawsze jest ten sam oraz nie bierzemy pod uwagę niepewności
                wykres.DodajPunkt(N.doubleParse(wiersz.Cells["WartoscWzorcowa"].Value.ToString()), N.doubleParse(wiersz.Cells["Wskazanie"].Value.ToString()), 0.0, 0.0, bool.Parse(wiersz.Cells["Dolaczyc"].Value.ToString()));
            }

            wykres.Show();
            if (getRownowaznikDawki() == 3)
                wykres.Rysuj(textBox1.Text, dateTimePicker1.Value, "mGy", comboBox1.Text, getRownowaznikDawki());
            else
                wykres.Rysuj(textBox1.Text, dateTimePicker1.Value, "mSv", comboBox1.Text, getRownowaznikDawki());
        }

        // tworzenie protokołu
        private void protokółToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string sciezka = _DocumentationPathsLoader.GetPath("ProtokolyDawkaWynik", Jezyk.PL) + textBox1.Text + "Dawka.html";
            
        
        
        //public DaneWarunkowModel modelDaneWarunkow;
        //public DaneWspolczynnikowModel modelDaneWspolczynnikow;

            ProtokolDawkaModel model = new ProtokolDawkaModel(
                    new DanePodstawoweModel(textBox1.Text, textBox2.Text, dateTimePicker1.Value),
                    new DanePrzyrzaduModel(textBox3.Text, textBox4.Text, textBox5.Text, comboBox1.Text, comboBox2.Text, comboBox3.Text),
                    new DaneWarunkowModel(textBox7.Text, textBox8.Text, textBox9.Text, textBox10.Text),
                    new DaneWspolczynnikowModel(textBox15.Text, textBox16.Text)
                );
            model.zrodlo = textBox17.Text;
            model.odleglosc = textBox18.Text;
            model.wspolczynnik = textBox11.Text;
            model.niepewnosc = textBox12.Text;
            model.tabela = dataGridView1.Rows;


            Dokumenty.ProtokolDawka protokol = new Dokumenty.ProtokolDawka(model);
            if (false == protokol.generateDocument(sciezka))
            {
                MessageBox.Show("Nie podano wszystkich potrzebnych danych!", "Uwaga");
            }
        }

        private void PrzejdzDoNastepnegoPola(object sender, KeyPressEventArgs e)
        {
            // if enter
            if (e.KeyChar == 13)
                SelectNextControl((Control)sender, true, false, true, true);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DaneWzorcoweMocDawki okno = new DaneWzorcoweMocDawki();

            try
            {
                okno.WyswietlDane(dateTimePicker1.Value, new DateTime(int.Parse(comboBox3.Text.Substring(0, 4)), int.Parse(comboBox3.Text.Substring(5, 2)), int.Parse(comboBox3.Text.Substring(8, 2))), "uGy/h");
                okno.Show();
            }
            catch (Exception)
            {
                MessageBox.Show("Brak wszystkich danych potrzebnych do wyświetlenia danych wzorcowych.", "Uwaga");
            }
        }

        private void textBox7_Leave(object sender, EventArgs e)
        {
            textBox7.Text = Narzedzia.Format.PoprawFormat(textBox7.Text, 1);
        }

        private void textBox8_Leave(object sender, EventArgs e)
        {
            textBox8.Text = Narzedzia.Format.PoprawFormat(textBox8.Text, 1);
        }

        private void textBox9_Leave(object sender, EventArgs e)
        {
            textBox9.Text = Narzedzia.Format.PoprawFormat(textBox9.Text, 1);
        }

        private void zapiszToolStripMenuItem_Click(object sender, EventArgs e)
        {
            N.PotwierdzenieZapisz(this, ZapiszDane, false, true);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
