using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Narzedzia;
using WzorcowanieMocDawkiSpace;

namespace DotBase
{
    public partial class WzorcowanieMocDawkiForm : Form
    {
        WzorcowanieMocDawki _WzorcowanieMocDawki;
        DocumentationPathsLoader _DocumentationPathsLoader = new DocumentationPathsLoader();

        //---------------------------------------------------------------------------
        public WzorcowanieMocDawkiForm(int nrKarty)
        //---------------------------------------------------------------------------
        {
            InitializeComponent();
            _WzorcowanieMocDawki = new WzorcowanieMocDawki(nrKarty, "md");
            textBox1.Text = nrKarty.ToString();
            comboBox1.SelectedIndexChanged += new System.EventHandler(comboBox1_SelectedIndexChanged);
            comboBox2.SelectedIndexChanged += new System.EventHandler(comboBox2_SelectedIndexChanged);
            comboBox4.SelectedIndexChanged += new System.EventHandler(LiczWartoscWzorcowa);
            
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(ZamykanieOknaWykresu);

            textBox21.Text = Properties.Settings.Default.Wykonal;
            textBox22.Text = Properties.Settings.Default.Sprawdzil;
        }

        //---------------------------------------------------------------
        void LiczWartoscWzorcowa()
        //---------------------------------------------------------------
        {
            if (comboBox3.SelectedIndex < 0 && comboBox3.SelectedIndex < 0)
                return;

            try
            {
                List<double> wartosciWzorcowe = _WzorcowanieMocDawki.LiczWartoscWzorcowa(ref dataGridView1, comboBox3.Text, comboBox4.Text, dateTimePicker1.Value);

                for (int i = 0; i < wartosciWzorcowe.Count; ++i)
                {
                    dataGridView1.Rows[i].Cells[5].Value = wartosciWzorcowe[i];
                }
            }
            catch (Exception)
            {
                MyMessageBox.Show("Część danych jest niepoprawnych lub ich nie wpisano.");
            }
        }

        //---------------------------------------------------------------
        private void PoprzedniArkusz(object sender, EventArgs e)
        //---------------------------------------------------------------
        {
            if (!N.PotwierdzenieZapisz(this, ZapiszDane, true, false))
                return;

            czyscOkno();
            _WzorcowanieMocDawki.CzyscStareDane();

            if (true == _WzorcowanieMocDawki.ZnajdzMniejszyArkusz())
            {
                WyswietlDane();
            }
            else
            {
                MyMessageBox.Show("Nie istnieje mniejszy arkusz dla danego wzorcowania.");
            }
        }

        //---------------------------------------------------------------
        private void NastepnyArkusz(object sender, EventArgs e)
        //---------------------------------------------------------------
        {
            if (!N.PotwierdzenieZapisz(this, ZapiszDane, true, false))
                return;

            czyscOkno();
            _WzorcowanieMocDawki.CzyscStareDane();

            if (true == _WzorcowanieMocDawki.ZnajdzWiekszyArkusz())
            {
                WyswietlDane();
            }
            else
            {
                _WzorcowanieMocDawki.StworzNowyArkusz();
                WyswietlDanePodstawowe();
                MyMessageBox.Show("To nowy arkusz który nie istnieje jeszcze w bazie. Zostanie on zapisany automatycznie.", "Uwaga");
            }
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
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        //---------------------------------------------------------------
        {
            if(comboBox3.Text != "" && comboBox4.Text != "")
                LiczWartoscWzorcowa();
        }

        //---------------------------------------------------------------
        private void czyscOkno()
        //---------------------------------------------------------------
        {
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            comboBox3.Items.Clear();
            comboBox4.Items.Clear();
            comboBox5.Items.Clear();

            comboBox1.Text = comboBox2.Text = comboBox3.Text =
            comboBox4.Text = comboBox5.Text = "";

            textBox5.Text = "0";
            textBox2.Text = "Brak";
            textBox7.Text = "Nie dotyczy";
            textBox9.Text = textBox21.Text = textBox22.Text = 
            textBox14.Text = textBox15.Text = textBox16.Text = "";

            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();

            textBox21.Text = Properties.Settings.Default.Wykonal;
            textBox22.Text = Properties.Settings.Default.Sprawdzil;
        }

        //---------------------------------------------------------------
        public bool Inicjalizuj()
        //---------------------------------------------------------------
        {
            if (_WzorcowanieMocDawki.Inicjalizuj())
            {
                textBox6.Text = _WzorcowanieMocDawki.IdArkusza.ToString();
                UstawDzisiejszaDate();
                WyswietlDane();
                return true;
            }
            else
            {
                textBox6.Text = _WzorcowanieMocDawki.IdArkusza.ToString();
                UstawDzisiejszaDate();
                WyswietlDanePodstawowe();
                comboBox3.SelectedItem = comboBox3.Items[comboBox3.Items.Count - 1];
                return true;
            }
        }

        //---------------------------------------------------------------
        private bool PrzygotujDaneDoZapisu(bool NadpisywanieStarychDanych)
        //---------------------------------------------------------------
        {
            if (false == SprawdzPoprawnoscDanych())
                return false;

            if (false == _WzorcowanieMocDawki.PrzygotujDaneOgolneDoZapisu(textBox1.Text, textBox6.Text, dateTimePicker1.Value, NadpisywanieStarychDanych))
            {
                MyMessageBox.Show("Dane ogólne są błędne.", "Uwaga");
                return false;
            }

            if (false == _WzorcowanieMocDawki.PrzygotujDanePrzyrzaduDoZapisu(textBox3.Text, textBox4.Text, textBox7.Text, textBox9.Text, comboBox1.Text, comboBox2.Text))
            {
                MyMessageBox.Show("Dane przyrządu są błędne.", "Uwaga");
                return false;
            }

            if (false == _WzorcowanieMocDawki.PrzygotujDaneWarunkowDoZapisu(textBox14.Text, textBox15.Text, textBox16.Text, textBox2.Text))
            {
                MyMessageBox.Show("Dane warunków są błędne.", "Uwaga");
                return false;
            }

            if (false == _WzorcowanieMocDawki.PrzygotujDaneWzorcowoPomiaroweDoZapisu(ref dataGridView1, comboBox3.Text, comboBox4.Text, textBox5.Text, comboBox5.Text))
            {
                MyMessageBox.Show("Dane wzorcowo-pomiarowe są błędne.", "Uwaga");
                return false;
            }

            if (false == _WzorcowanieMocDawki.PrzygotujDaneWspolczynnikowDoZapisu(ref dataGridView2, textBox21.Text, textBox22.Text, checkBox1.Checked))
            {
                MyMessageBox.Show("Dane współczynników są błędne.", "Uwaga");
                return false;
            }
            
            return true;
        }

        //---------------------------------------------------------------
        private bool SprawdzPoprawnoscDanych()
        //---------------------------------------------------------------
        {
            if (textBox1.Text == "" ||
                textBox3.Text == "" ||
                textBox4.Text == "" ||
                textBox6.Text == "" ||
                comboBox1.SelectedIndex < 0 ||
                comboBox2.SelectedIndex < 0)
            {
                MyMessageBox.Show("Błędne dane przyrządu.", "Uwaga");
                return false;
            }

            if(textBox7.Text == "")
                textBox7.Text = "Nie dotyczy";

            if(textBox9.Text == "")
                textBox9.Text = "Brak";

            if(textBox14.Text == "")
                textBox14.Text = "0";
                
            if(textBox15.Text == "")
                textBox15.Text = "0";

            if(textBox16.Text == "")
                textBox16.Text = "0";

            if(textBox2.Text == "")
                textBox2.Text = "Brak";

            if(textBox5.Text == "")
                textBox5.Text = "0";

            if(textBox21.Text == "")
                textBox21.Text = "Nie podano";

            if(textBox22.Text == "")
                textBox22.Text = "Nie podano";

            // jeśli nie podano wybierz ostatni protokół
            if(comboBox3.Text == "")
                comboBox3.SelectedItem = comboBox3.Items[comboBox3.Items.Count - 1];

            if (comboBox4.Text == "")
            {
                comboBox4.SelectedItem = comboBox4.Items[comboBox4.Items.IndexOf("uGy/h")];
                comboBox5.SelectedIndex = 0;
            }


            return true;
        }

        //---------------------------------------------------------------
        private void UstawDzisiejszaDate()
        //---------------------------------------------------------------
        {
            dateTimePicker1.Value = DateTime.Today;
        }

        //---------------------------------------------------------------
        private void WyswietlDane()
        //---------------------------------------------------------------
        {
            _WzorcowanieMocDawki.PobierzIdWzorcowania();
            textBox6.Text = _WzorcowanieMocDawki.IdArkusza.ToString();
            WyswietlDanePrzyrzadu();
            WyswietlDateWzorcowania();
        //    WyswietlSondyDlaDanegoWzorcowania();
            WyswietlDaneWarunkow();
            WyswietlWszystkieJednostki();
            WyswietlKonkretnaJednostke();
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
            checkBox1.Checked = true;

            textBox6.Text = _WzorcowanieMocDawki.IdArkusza.ToString();
            textBox7.Text = "nie dotyczy";
            textBox9.Text = "nie dotyczy";
            dateTimePicker1.Value = DateTime.Today;

            WyswietlPodstawoweDanePrzyrzadu();
            WyswietlWszystkieJednostki();
            WyswietlWszystkieProtokoly();
            WyswietlWszystkieSondy();

            int nrPoprzedniejKalibracji = _WzorcowanieMocDawki.ZnajdzNrPoprzedniejKalibracji(textBox3.Text, textBox4.Text);

            if (0 < nrPoprzedniejKalibracji)
            {
                textBox2.Text = "Numer poprzedniego świadectwa: " + nrPoprzedniejKalibracji + "/" + _WzorcowanieMocDawki.PobierzRok(nrPoprzedniejKalibracji) + ".";
            }
        }
        
        //---------------------------------------------------------------
        private void WyswietlPodstawoweDanePrzyrzadu()
        //---------------------------------------------------------------
        {
            if (_WzorcowanieMocDawki.PobierzPodstawoweDanePrzyrzadu())
            {
                textBox3.Text = _WzorcowanieMocDawki.Przyrzad.TypDozymetru;
                textBox4.Text = _WzorcowanieMocDawki.Przyrzad.NrFabrycznyDozymetru;
            }
        }

        //---------------------------------------------------------------
        private void WyswietlDanePrzyrzadu()
        //---------------------------------------------------------------
        {
            if (_WzorcowanieMocDawki.PobierzDanePrzyrzadu())
            {
                KlasyPomocniczeCez.Przyrzad przyrzad = _WzorcowanieMocDawki.Przyrzad;

                textBox3.Text = przyrzad.TypDozymetru;
                textBox4.Text = przyrzad.NrFabrycznyDozymetru;
                textBox9.Text = przyrzad.InneNastawy;
                textBox7.Text = przyrzad.NapiecieZasilaniaSondy;

                comboBox1.Items.Clear();
                comboBox2.Items.Clear();

                for (int i = 0; i <  _WzorcowanieMocDawki.Sondy.Lista.Count; ++i)
                {
                    comboBox1.Items.Add(_WzorcowanieMocDawki.Sondy.Lista[i].Typ);
                    comboBox2.Items.Add(_WzorcowanieMocDawki.Sondy.Lista[i].NrFabryczny);
                }

                Sonda s = _WzorcowanieMocDawki.ZnajdzSondeWybranaPrzezUzytkwonika();

                comboBox2.SelectedIndex = comboBox2.Items.IndexOf(s.NrFabryczny);
            }
        }

        //---------------------------------------------------------------
        private void WyswietlDaneWarunkow()
        //---------------------------------------------------------------
        {
            if (_WzorcowanieMocDawki.PobierzDaneWarunkow())
            {
                KlasyPomocniczeCez.Warunki warunki = _WzorcowanieMocDawki.Warunki;

                textBox14.Text = warunki.Cisnienie.ToString("0.0");
                textBox15.Text = warunki.Temperatura.ToString("0.0");
                textBox16.Text = warunki.Wilgotnosc.ToString("0.0");
                textBox2.Text = warunki.Uwagi;
            }
        }

        //---------------------------------------------------------------
        private string UstawFormatWspolczynnikowOrazNiepewnosci(KlasyPomocniczeMocyDawki.MocDawkiWspolczynniki.Wspolczynnik wspolczynnik)
        //---------------------------------------------------------------
        {
            string format;

            format = "0.";

            if (wspolczynnik.Wartosc >= 1.0)
                format += "00";
            else
                format = Precyzja.Ustaw(wspolczynnik.Niepewnosc);

            return format;
        }

        //---------------------------------------------------------------
        private void WyswietlDaneWspolczynnikow()
        //---------------------------------------------------------------
        {
            KlasyPomocniczeMocyDawki.MocDawkiWspolczynniki wspolczynniki = _WzorcowanieMocDawki.Wspolczynniki;

            if (_WzorcowanieMocDawki.PobierzDaneWspolczynnikow1())
            {
                textBox21.Text = wspolczynniki.Wzorcujacy;
                textBox22.Text = wspolczynniki.Sprawdzajacy;
                checkBox1.Checked = wspolczynniki.Dolacz;
                //textBox5.Text = wspolczynniki.Dane Tlo;
            }

            if (_WzorcowanieMocDawki.PobierzDaneWspolczynnikow2())
            {
                string format;

                foreach (KlasyPomocniczeMocyDawki.MocDawkiWspolczynniki.Wspolczynnik wspolczynnik in _WzorcowanieMocDawki.Wspolczynniki.Dane)
                {
                    format = UstawFormatWspolczynnikowOrazNiepewnosci(wspolczynnik);

                    dataGridView2.Rows.Add(wspolczynnik.Zakres.ToString("G"), wspolczynnik.Wartosc.ToString(format), wspolczynnik.Niepewnosc.ToString(format));
                }
            }

            LiczWartoscWzorcowa();
        }

        //---------------------------------------------------------------
        private void WyswietlDateWzorcowania()
        //---------------------------------------------------------------
        {
            if (_WzorcowanieMocDawki.PobierzDateWzorcowania())
            {
                dateTimePicker1.Value = _WzorcowanieMocDawki.OdpowiedzBazy.Rows[0].Field<DateTime>(0);
            }
        }

        //---------------------------------------------------------------
        private void WyswietlDaneWzorcoweIPomiarowe()
        //---------------------------------------------------------------
        {
            if (_WzorcowanieMocDawki.PobierzDaneWzorcoweIPomiarowe())
            {
                foreach ( KlasyPomocniczeMocyDawki.MocDawkiWartosciWzorcowoPomiarowe.MocDawkiWartoscWzorcowoPomiarowa dane in _WzorcowanieMocDawki.Pomiary.Dane )
                {
                    dataGridView1.Rows.Add(dane.Odleglosc, dane.IdZrodla, dane.Wskazanie, dane.Wahanie, dane.Zakres, "", dane.Dolaczyc);
                }
            }

            WyswietlKonkretnaJednostke();
            WyswietlKonkretnyProtokol();
            textBox5.Text = _WzorcowanieMocDawki.Pomiary.tlo;
        }

        //---------------------------------------------------------------
        private void WyswietlKonkretnaJednostke()
        //---------------------------------------------------------------
        {
            if (_WzorcowanieMocDawki.PobierzKonkretnaJednostke())
            {
                int index = comboBox4.Items.IndexOf(_WzorcowanieMocDawki.WybranaJednostka);
                
                if (index >= 0)
                    comboBox4.SelectedIndex = index;
            }
        }

        //---------------------------------------------------------------
        private void WyswietlKonkretnyProtokol()
        //---------------------------------------------------------------
        {
            if (_WzorcowanieMocDawki.PobierzKonkretnyProtokol())
            {
                int index = comboBox3.Items.IndexOf(_WzorcowanieMocDawki.WybranyProtokol);
                
                if (index >= 0)
                    comboBox3.SelectedIndex = index;
            }
        }

        //---------------------------------------------------------------
        private void WyswietlWszystkieSondy()
        //---------------------------------------------------------------
        {
            if (_WzorcowanieMocDawki.PobierzWszystkieMozliweSondy())
            {
                foreach (Sonda sonda in _WzorcowanieMocDawki.Sondy.Lista)
                {
                    comboBox1.Items.Add(sonda.Typ);
                    comboBox2.Items.Add(sonda.NrFabryczny);
                }
            }
        }

        //---------------------------------------------------------------
        private void WyswietlSondyDlaDanegoWzorcowania()
        //---------------------------------------------------------------
        {
            if (_WzorcowanieMocDawki.PobierzSondyDlaDanegoWzorcowania())
            {
                foreach (Sonda sonda in _WzorcowanieMocDawki.Sondy.Lista)
                {
                    comboBox1.Items.Add(sonda.Typ);
                    comboBox2.Items.Add(sonda.NrFabryczny);
                }
            }
        }

        //---------------------------------------------------------------
        private void WyswietlWszystkieProtokoly()
        //---------------------------------------------------------------
        {
            if (_WzorcowanieMocDawki.PobierzWszystkieProtokoly())
            {
                foreach (DateTime data in _WzorcowanieMocDawki.Protokoly.Daty)
                {
                    comboBox3.Items.Add(data.ToShortDateString());
                }
            }
        }

        //---------------------------------------------------------------
        private void WyswietlWszystkieJednostki()
        //---------------------------------------------------------------
        {
            if (_WzorcowanieMocDawki.PobierzWszystkieJednostki())
            {
                foreach (string nazwa in _WzorcowanieMocDawki.Jednostki.Nazwy)
                {
                    comboBox4.Items.Add(nazwa);
                }
            }
        }

        //---------------------------------------------------------------
        private void WyswietlWielkoscFizyczna()
        //---------------------------------------------------------------
        {
            if (_WzorcowanieMocDawki.PobierzKonkretnaWielkoscFizyczna(comboBox4.Text))
            {
                comboBox5.Text = _WzorcowanieMocDawki.OdpowiedzBazy.Rows[0].Field<string>(0);
            }
        }

        //---------------------------------------------------------------
        private bool ZapiszDane()
        //---------------------------------------------------------------
        {
            if (true == _WzorcowanieMocDawki.SprawdzCzyArkuszJestJuzZapisany())
            {
                if (false == PrzygotujDaneDoZapisu(true))
                    return false;

                // Nadpisywanie danych
                _WzorcowanieMocDawki.NadpiszDane();
            }
            else
            {
                if (false == PrzygotujDaneDoZapisu(false))
                    return false;

                Properties.Settings.Default.Wykonal = textBox21.Text;
                Properties.Settings.Default.Sprawdzil = textBox22.Text;
                Properties.Settings.Default.Save();

                // Zapisywanie danych
                _WzorcowanieMocDawki.ZapiszDane();
            }

            return true;
        }

        //---------------------------------------------------------------
        private void ZnajdzWszystkieWielkoscFizyczne(object sender, EventArgs e)
        //---------------------------------------------------------------
        {
            if (comboBox4.Text != "" && _WzorcowanieMocDawki.ZnajdzWszystkieWielkosciFizyczne(comboBox4.Text) )
            {
                comboBox5.Items.Clear();
                comboBox5.Text = "";

                foreach (DataRow row in _WzorcowanieMocDawki.OdpowiedzBazy.Rows)
                {
                    comboBox5.Items.Add( row.Field<string>(0) );
                }
            }
        }

        //---------------------------------------------------------------
        private void ZamykanieOknaWykresu(object sender, FormClosingEventArgs e)
        //---------------------------------------------------------------
        {
            e.Cancel = !N.PotwierdzenieZapisz(this, ZapiszDane, true, false);
        }

        //---------------------------------------------------------------
        private void protokółToolStripMenuItem_Click(object sender, EventArgs e)
        //---------------------------------------------------------------
        {
            string sciezka = _DocumentationPathsLoader.GetPath("ProtokolyMocDawkiWynik", Jezyk.PL) + textBox1.Text + "MocDawki.html";

            Dokumenty.ProtokolMocDawkiModel model = new Dokumenty.ProtokolMocDawkiModel(
                    new DanePodstawoweModel(textBox1.Text, textBox6.Text, dateTimePicker1.Value),
                    new DanePrzyrzaduModel(textBox3.Text, textBox4.Text, textBox9.Text, comboBox1.Text, comboBox2.Text, textBox7.Text),
                    new DaneWarunkowModel(textBox14.Text, textBox15.Text, textBox16.Text, textBox2.Text),
                    new DaneWspolczynnikowModel(textBox21.Text, textBox22.Text));

            model.jednostka = comboBox4.Text;
            model.wielkoscFizyczna = comboBox5.Text;
            model.tlo = textBox5.Text;
            model.tabela = dataGridView1.Rows;
            
            Dokumenty.ProtokolMocDawki protokol = new Dokumenty.ProtokolMocDawki(model);
            if (!protokol.generateDocument(sciezka))
            {
                MyMessageBox.Show("Nie podano wszystkich potrzebnych danych!", "Uwaga");
            }
        }

        //---------------------------------------------------------------
        private void wykresToolStripMenuItem_Click(object sender, EventArgs e)
        //---------------------------------------------------------------
        {
            Jezyk jezyk = Jezyk.PL;
            if (sender == wykresENToolStripMenuItem)
            {
                jezyk = Jezyk.EN;
            }
            Wykres.WykresForm wykres = new Wykres.WykresForm(true, checkBox2.Checked, checkBox3.Checked, jezyk);
            wykres.CzyscDane();

            DataGridViewRow wiersz;

            dataGridView1.Sort(dataGridView1.Columns[5], ListSortDirection.Descending);
            

            for (int i = 0; dataGridView1.Rows[i].Cells[2].Value != null && 
                            dataGridView1.Rows[i].Cells[3].Value != null &&
                            dataGridView1.Rows[i].Cells[4].Value != null &&
                            dataGridView1.Rows[i].Cells[5].Value != null &&
                            dataGridView1.Rows[i].Cells[6].Value != null; ++i)
            {
                wiersz = dataGridView1.Rows[i];

                wykres.DodajPunkt(N.doubleParse(wiersz.Cells[5].Value.ToString()), N.doubleParse(wiersz.Cells[2].Value.ToString()), N.doubleParse(wiersz.Cells[3].Value.ToString()), N.doubleParse(wiersz.Cells[4].Value.ToString()), bool.Parse(wiersz.Cells[6].Value.ToString()));
            }

            wykres.Show();
            wykres.Rysuj(textBox1.Text, dateTimePicker1.Value, comboBox4.Text, comboBox1.Text);
        }

        //-------------------------------------------------------------------
        void LiczWspolczynnikINiepewnosc(ref DataGridView tabela, ref DataGridView tabela2)
        //-------------------------------------------------------------------
        {
            List<double> zakresPrzyrzadu;
            List<double> wspolczynniki;
            List<double> niepewnosc;

            tabela2.Rows.Clear();

            bool result;

            if (N.proceduraOd20230915(dateTimePicker1.Value))
            {
                result = _WzorcowanieMocDawki.LiczWspolczynnikINiepewnoscOd20230918(ref tabela, ref tabela2, comboBox3.Text, dateTimePicker1.Value, comboBox4.SelectedItem as string, out zakresPrzyrzadu, out wspolczynniki, out niepewnosc);
            }
            else if (dateTimePicker1.Value >= DateTime.Parse("2018-06-11"))
            {
                result = _WzorcowanieMocDawki.LiczWspolczynnikINiepewnoscOd20180611(ref tabela, ref tabela2, comboBox3.Text, dateTimePicker1.Value, out zakresPrzyrzadu, out wspolczynniki, out niepewnosc);
            }
            else if (dateTimePicker1.Value >= DateTime.Parse("2018-03-01"))
            {
                result = _WzorcowanieMocDawki.LiczWspolczynnikINiepewnoscOd20180301(ref tabela, ref tabela2, comboBox3.Text, out zakresPrzyrzadu, out wspolczynniki, out niepewnosc);
            }
            else
            {
                result = _WzorcowanieMocDawki.LiczWspolczynnikINiepewnoscOld(ref tabela, ref tabela2, comboBox3.Text, out zakresPrzyrzadu, out wspolczynniki, out niepewnosc);
            }

            if (result)
            {
                string format = "";

                for (int i = 0; i < zakresPrzyrzadu.Count; ++i)
                {
                    KlasyPomocniczeMocyDawki.MocDawkiWspolczynniki.Wspolczynnik wspolczynnik = new KlasyPomocniczeMocyDawki.MocDawkiWspolczynniki.Wspolczynnik(wspolczynniki[i], niepewnosc[i], zakresPrzyrzadu[i]);

                    if (wspolczynnik.Wartosc > 1.0)
                    {
                        format = "0.00";
                        break;
                    }

                    string temp = UstawFormatWspolczynnikowOrazNiepewnosci(wspolczynnik);

                    if (0 < temp.CompareTo(format))
                        format = temp;
                }

                for (int i = 0; i < zakresPrzyrzadu.Count; ++i)
                {
                    tabela2.Rows.Add(zakresPrzyrzadu[i].ToString("G"), wspolczynniki[i].ToString(format), niepewnosc[i].ToString(format));
                }
            }
            else
            {
                MyMessageBox.Show("Błędne lub brak danych. Operacja nie może być wykonana.", "Uwaga!");
            }
        }

        //---------------------------------------------------------------
        private void LiczWskazanieMinMax()
        //---------------------------------------------------------------
        {
            double min, max;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                try
                {
                    min = N.doubleParse(row.Cells["Min"].Value.ToString());
                    max = N.doubleParse(row.Cells["Max"].Value.ToString());
                    double wskazanie = (min + max) / 2;
                    double niepewnosc = (max - min) / 2;

                    String text1 = min.ToString();
                    int pos1 = text1.IndexOf(',');
                    if (pos1 < 0) pos1 = text1.Length - 1;
                    int digits1 = text1.Length - pos1 - 1;

                    String text2 = max.ToString();
                    int pos2 = text2.IndexOf(',');
                    if (pos2 < 0) pos2 = text2.Length - 1;
                    int digits2 = text2.Length - pos2 - 1;


                    String text3 = wskazanie.ToString();
                    int pos3 = text3.IndexOf(',');
                    if (pos3 < 0) pos3 = text3.Length - 1;
                    int digits3 = text3.Length - pos3 - 1;

                    int digits = digits1;
                    if (digits2 > digits) digits = digits2;
                    if (digits3 > digits) digits = digits3;

                    wskazanie = Math.Round(wskazanie, digits);
                    niepewnosc = Math.Round(niepewnosc, digits);

                    row.Cells["Wskazanie"].Value = wskazanie;
                    row.Cells["Niepewnosc"].Value = niepewnosc;
                   
                }
                catch (Exception)
                {
                    // część danych w wierszach może być niewypełnionych bądź wypełnionych błędnie. Tutaj tłumimy błędy tym spowodowane.
                }
            }
        }

        //-------------------------------------------------------------------
        private void button3_Click(object sender, EventArgs e)
        //-------------------------------------------------------------------
        {
            LiczWspolczynnikINiepewnosc(ref dataGridView1, ref dataGridView2);
            ZnajdzPoprzedniWspolczynnikINiepewnosc();
        }

        //-------------------------------------------------------------------
        private void ZnajdzPoprzedniWspolczynnikINiepewnosc()
        //-------------------------------------------------------------------
        {
            _WzorcowanieMocDawki.ZnajdzPoprzedniWspolczynnikINiepewnosc(ref dataGridView2, textBox1.Text, textBox3.Text, textBox4.Text, comboBox1.Text, comboBox2.Text);
        }

        //-------------------------------------------------------------------
        // Liczenie danych wzorcowych
        private void button1_Click(object sender, EventArgs e)
        //-------------------------------------------------------------------
        {
            DaneWzorcoweMocDawki okno = new DaneWzorcoweMocDawki();

            try
            {
                okno.WyswietlDane(dateTimePicker1.Value, new DateTime(int.Parse(comboBox3.Text.Substring(0, 4)), int.Parse(comboBox3.Text.Substring(5, 2)), int.Parse(comboBox3.Text.Substring(8, 2))), comboBox4.Text);
                okno.Show();
            }
            catch (Exception)
            {
                MyMessageBox.Show("Brak wszystkich danych potrzebnych do wyświetlenia danych wzorcowych.", "Uwaga");
            }

        }

        //********************************************************************************
        private void LiczWartoscWzorcowa(object sender, EventArgs e)
        //********************************************************************************
        {
            LiczWskazanieMinMax();
            LiczWartoscWzorcowa();
        }

        //********************************************************************************
        private void button4_Click(object sender, EventArgs e)
        //********************************************************************************
        {
            for (int i = 0; i < dataGridView1.Rows.Count - 1; ++i)
            {
                dataGridView1.Rows[i].Cells[6].Value = true;
            }
        }

        //********************************************************************************
        private void PrzejdzDoKolejnegoPola(object sender, KeyEventArgs e)
        //********************************************************************************
        {
            // if enter
            if(e.KeyValue == 13)
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

        private void zapiszToolStripMenuItem_Click(object sender, EventArgs e)
        {
            N.PotwierdzenieZapisz(this, ZapiszDane, false, true);
        }

        private void label20_Click(object sender, EventArgs e)
        {

        }

        private void warunki_TextChanged(object sender, EventArgs e)
        {
            N.SprawdzZakres(sender as TextBox);
        }
    }
}
