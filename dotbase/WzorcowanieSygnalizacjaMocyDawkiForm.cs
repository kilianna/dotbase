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
    public partial class WzorcowanieSygnalizacjaMocyDawkiForm : Form
    {
        private const string SYGNALIZACJA_MOC_DAWKI = "sm";
        private WzorcowanieSygMocyDawki _WzorcowanieSygMocyDawki;
        DocumentationPathsLoader _DocumentationPathsLoader = new DocumentationPathsLoader();

        public WzorcowanieSygnalizacjaMocyDawkiForm(int idKarty)
        {
            InitializeComponent();

            _WzorcowanieSygMocyDawki = new WzorcowanieSygMocyDawki(idKarty, SYGNALIZACJA_MOC_DAWKI);

            comboBox1.SelectedIndexChanged += new System.EventHandler(comboBox1_SelectedIndexChanged);
            comboBox2.SelectedIndexChanged += new System.EventHandler(comboBox2_SelectedIndexChanged);

            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(ZamykanieOkna);

            toolTip1.SetToolTip(textBox13, "Wpisane dane nie mogą być dłuższe niż 200 znaków.");

            textBox11.Text = Properties.Settings.Default.Wykonal;
            textBox12.Text = Properties.Settings.Default.Sprawdzil;
        }

        //---------------------------------------------------------------
        private void button4_Click(object sender, EventArgs e)
        //---------------------------------------------------------------
        {
            LiczWartoscOrazNiepewnosc();
        }

        //---------------------------------------------------------------
        private bool PrzygotujDaneDoZapisu(bool NadpisywanieStarychDanych)
        //---------------------------------------------------------------
        {
            if (false == SprawdzPoprawnoscDanych())
                return false;

            if (false == _WzorcowanieSygMocyDawki.PrzygotujDaneOgolneDoZapisu(textBox1.Text, textBox2.Text, dateTimePicker1.Value, NadpisywanieStarychDanych))
            {
                MessageBox.Show("Dane ogólne są błędne.", "Uwaga");
                return false;
            }

            if (false == _WzorcowanieSygMocyDawki.PrzygotujDanePrzyrzaduDoZapisu(textBox3.Text, textBox4.Text, textBox6.Text, textBox5.Text, comboBox1.Text, comboBox2.Text))
            {
                MessageBox.Show("Dane przyrządu są błędne.", "Uwaga");
                return false;
            }

            if (false == _WzorcowanieSygMocyDawki.PrzygotujDaneWarunkowDoZapisu(textBox7.Text, textBox8.Text, textBox9.Text, textBox10.Text))
            {
                MessageBox.Show("Dane warunków są błędne.", "Uwaga");
                return false;
            }

            if (false == _WzorcowanieSygMocyDawki.PrzygotujDaneWzorcowoPomiaroweDoZapisu(ref dataGridView1, comboBox3.Text, comboBox4.Text, textBox13.Text, radioButton1.Checked))
            {
                MessageBox.Show("Dane wzorcowo-pomiarowe są błędne.", "Uwaga");
                return false;
            }

            if (false == _WzorcowanieSygMocyDawki.PrzygotujDaneProtokolowDoZapisu(textBox11.Text, textBox12.Text, checkBox1.Checked))
            {
                MessageBox.Show("Dane współczynników są błędne.", "Uwaga");
                return false;
            }

            return true;
        }

        //---------------------------------------------------------------
        void comboBox1_SelectedIndexChanged(Object sender, EventArgs args)
        //---------------------------------------------------------------
        {
            comboBox2.SelectedIndex = comboBox1.SelectedIndex;
        }

        //---------------------------------------------------------------
        void comboBox2_SelectedIndexChanged(Object sender, EventArgs args)
        //---------------------------------------------------------------
        {
            comboBox1.SelectedIndex = comboBox2.SelectedIndex;
        }

        //---------------------------------------------------------------
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        //---------------------------------------------------------------
        {
            LiczWartoscOrazNiepewnosc();
        }

        //---------------------------------------------------------------
        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        //---------------------------------------------------------------
        {
            LiczWartoscOrazNiepewnosc();
        }

        //---------------------------------------------------------------
        private void CzyscOkno()
        //---------------------------------------------------------------
        {
            textBox5.Text = textBox6.Text = "nie dotyczy";
            comboBox1.Text = comboBox2.Text = "";
            textBox7.Text = textBox8.Text = textBox9.Text = "0";
            textBox10.Text = "Brak";
            comboBox3.Text = comboBox4.Text = "";
            dataGridView1.Rows.Clear();
            textBox13.Clear();
            textBox11.Clear();
            textBox12.Text = "P. Bilski";
            checkBox1.Checked = true;

            textBox11.Text = Properties.Settings.Default.Wykonal;
            textBox12.Text = Properties.Settings.Default.Sprawdzil;
        }

        //---------------------------------------------------------------
        public bool Inicjalizuj()
        //---------------------------------------------------------------
        {
            if (_WzorcowanieSygMocyDawki.Inicjalizuj())
            {
                textBox1.Text = _WzorcowanieSygMocyDawki.IdKarty.ToString();
                textBox2.Text = _WzorcowanieSygMocyDawki.IdArkusza.ToString();
                UstawDzisiejszaDate();
                WyswietlDane();
                return true;
            }
            else
            {
                textBox1.Text = _WzorcowanieSygMocyDawki.IdKarty.ToString();
                textBox2.Text = _WzorcowanieSygMocyDawki.IdArkusza.ToString();
                UstawDzisiejszaDate();
                WyswietlDanePodstawowe();
                return true;
            }
        }

        //---------------------------------------------------------------
        private bool LiczWartoscOrazNiepewnosc()
        //---------------------------------------------------------------
        {
            if (comboBox3.Text == "" || comboBox4.Text == "")
                return false;

            WzorcowanieSygMocyDawki.DaneWzorcowoPomiarowe daneWejsciowe = new WzorcowanieSygMocyDawki.DaneWzorcowoPomiarowe(comboBox3.Text, comboBox4.Text, dateTimePicker1.Value);

            for (UInt16 i = 0; i < dataGridView1.RowCount - 1 && 
                               dataGridView1.Rows[i].Cells[1].Value != null &&
                               dataGridView1.Rows[i].Cells[2].Value != null &&
                               dataGridView1.Rows[i].Cells[3].Value != null &&
                               dataGridView1.Rows[i].Cells[4].Value != null; 
                               ++i)
            {
                try
                {
                    daneWejsciowe.m_Odleglosc1.Add( N.doubleParse(dataGridView1.Rows[i].Cells[1].Value.ToString()));
                    daneWejsciowe.m_Zrodlo1.Add( UInt16.Parse(dataGridView1.Rows[i].Cells[2].Value.ToString()));
                    daneWejsciowe.m_Odleglosc2.Add( N.doubleParse(dataGridView1.Rows[i].Cells[3].Value.ToString()));
                    daneWejsciowe.m_Zrodlo2.Add( UInt16.Parse(dataGridView1.Rows[i].Cells[4].Value.ToString()));
                    daneWejsciowe.m_Prog.Add(N.doubleParse(dataGridView1.Rows[i].Cells[0].Value.ToString()));
                }
                catch (Exception)
                {
                    return false;
                }
            }

            List<double>[] daneWynikowe;

            if (N.proceduraOd20230915(dateTimePicker1.Value))
            {
                daneWynikowe = _WzorcowanieSygMocyDawki.LiczWartoscOrazNiepewnosc20230915(daneWejsciowe);
            }
            else
            {
                daneWynikowe = _WzorcowanieSygMocyDawki.LiczWartoscOrazNiepewnoscOld(daneWejsciowe);
            }

            if (daneWynikowe == null)
                return false;

            for (UInt16 i = 0; i < daneWynikowe[0].Count; ++i)
            {
                dataGridView1.Rows[i].Cells[5].Value = daneWynikowe[0][i].ToString("0.00");
                dataGridView1.Rows[i].Cells[6].Value = daneWynikowe[1][i].ToString("0.00");
                dataGridView1.Rows[i].Cells[7].Value = daneWynikowe[2][i].ToString("0.00");
                dataGridView1.Rows[i].Cells[8].Value = daneWynikowe[3][i].ToString("0.00");
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

            // inne nastawy
            if (textBox5.Text == "")
                textBox5.Text = "Brak";

            // napięcie zasilania
            if (textBox6.Text == "")
                textBox6.Text = "Nie dotyczy";

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
                textBox10.Text = "brak";

            // jeśli nie podano wybierz ostatni protokół
            if (comboBox3.Text == "")
                comboBox3.SelectedItem = comboBox3.Items[comboBox3.Items.Count - 1];

            // jeśli nie podano wybierz ostatni protokół
            if (comboBox4.Text == "")
                comboBox4.SelectedItem = comboBox3.FindString("mSv/h");

            if (radioButton1.Checked && ("" == comboBox3.Text || "" == comboBox4.Text))
                    return false;
            else if (textBox13.Text == "")
            {
                textBox13.Text = "Brak opisu.";
            }


            // wykonał
            if (textBox11.Text == "")
                textBox11.Text = "nie podano";

            // sprawdził
            if (textBox12.Text == "")
                textBox12.Text = "nie podano";

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
            WyswietlDanePrzyrzadu();
            WyswietlDateWzorcowania();
            WyswietlDaneWarunkow();
            WyswietlWszystkieJednostki();
            WyswietlKonkretnaJednostke();
            WyswietlWszystkieProtokoly();
            WyswietlKonkretnyProtokol();
            WyswietlDaneWzorcoweIPomiarowe();
            WyswietlDaneProtokolow();
        }

        //---------------------------------------------------------------
        private void WyswietlDanePodstawowe()
        //---------------------------------------------------------------
        {
            CzyscOkno();

            textBox2.Text = _WzorcowanieSygMocyDawki.IdArkusza.ToString();
            dateTimePicker1.Value = DateTime.Today;

            WyswietlPodstawoweDanePrzyrzadu();
            WyswietlWszystkieJednostki();
            WyswietlWszystkieProtokoly();
            WyswietlWszystkieSondy();

            int nrPoprzedniejKalibracji = _WzorcowanieSygMocyDawki.ZnajdzNrPoprzedniejKalibracji(textBox3.Text, textBox4.Text);

            if (0 < nrPoprzedniejKalibracji)
            {
                textBox5.Text = "Numer poprzedniego świadectwa " + nrPoprzedniejKalibracji + "/" + _WzorcowanieSygMocyDawki.PobierzRok(nrPoprzedniejKalibracji);
            }
        }

        //---------------------------------------------------------------
        private void WyswietlDanePrzyrzadu()
        //---------------------------------------------------------------
        {
            if (_WzorcowanieSygMocyDawki.PobierzDanePrzyrzadu())
            {
                KlasyPomocniczeCez.Przyrzad przyrzad = _WzorcowanieSygMocyDawki.Przyrzad;

                textBox3.Text = przyrzad.TypDozymetru;
                textBox4.Text = przyrzad.NrFabrycznyDozymetru;
                textBox5.Text = przyrzad.InneNastawy;
                textBox6.Text = przyrzad.NapiecieZasilaniaSondy;

                comboBox1.Items.Clear();
                comboBox2.Items.Clear();

                for (int i = 0; i < _WzorcowanieSygMocyDawki.Sondy.Lista.Count; ++i)
                {
                    comboBox1.Items.Add(_WzorcowanieSygMocyDawki.Sondy.Lista[i].Typ);
                    comboBox2.Items.Add(_WzorcowanieSygMocyDawki.Sondy.Lista[i].NrFabryczny);
                }

                Sonda s = _WzorcowanieSygMocyDawki.ZnajdzSondeWybranaPrzezUzytkwonika();

                comboBox2.SelectedIndex = comboBox2.Items.IndexOf(s.NrFabryczny);
            }
        }

        //---------------------------------------------------------------
        private void WyswietlDateWzorcowania()
        //---------------------------------------------------------------
        {
            if (_WzorcowanieSygMocyDawki.PobierzDateWzorcowania())
            {
                dateTimePicker1.Value = _WzorcowanieSygMocyDawki.OdpowiedzBazy.Rows[0].Field<DateTime>(0);
            }
        }

        //---------------------------------------------------------------
        private void WyswietlDaneWarunkow()
        //---------------------------------------------------------------
        {
            if (_WzorcowanieSygMocyDawki.PobierzDaneWarunkow())
            {
                KlasyPomocniczeCez.Warunki warunki = _WzorcowanieSygMocyDawki.Warunki;

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
            if (_WzorcowanieSygMocyDawki.PobierzKonkretnyProtokol())
            {
                int index = comboBox3.Items.IndexOf(_WzorcowanieSygMocyDawki.WybranyProtokol);

                if (index >= 0)
                    comboBox3.SelectedIndex = index;
            }
        }

        //---------------------------------------------------------------
        private void WyswietlPodstawoweDanePrzyrzadu()
        //---------------------------------------------------------------
        {
            if (_WzorcowanieSygMocyDawki.PobierzPodstawoweDanePrzyrzadu())
            {
                textBox3.Text = _WzorcowanieSygMocyDawki.Przyrzad.TypDozymetru;
                textBox4.Text = _WzorcowanieSygMocyDawki.Przyrzad.NrFabrycznyDozymetru;
            }
        }

        //---------------------------------------------------------------
        private void WyswietlWszystkieJednostki()
        //---------------------------------------------------------------
        {
            if (_WzorcowanieSygMocyDawki.PobierzWszystkieJednostki())
            {
                foreach (string nazwa in _WzorcowanieSygMocyDawki.Jednostki.Nazwy)
                {
                    if(nazwa == "uSv/h" || nazwa == "uGy/h" || nazwa == "mSv/h" || nazwa == "nA/kg" || nazwa =="mR/h")
                        comboBox4.Items.Add(nazwa);
                }
            }
        }

        //---------------------------------------------------------------
        private void WyswietlWszystkieProtokoly()
        //---------------------------------------------------------------
        {
            if (_WzorcowanieSygMocyDawki.PobierzWszystkieProtokoly())
            {
                foreach (DateTime data in _WzorcowanieSygMocyDawki.Protokoly.Daty)
                {
                    comboBox3.Items.Add(data.ToShortDateString());
                }
            }
        }

        //---------------------------------------------------------------
        private void WyswietlWszystkieSondy()
        //---------------------------------------------------------------
        {
            if (_WzorcowanieSygMocyDawki.PobierzWszystkieMozliweSondy())
            {
                foreach (Sonda sonda in _WzorcowanieSygMocyDawki.Sondy.Lista)
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
            if (_WzorcowanieSygMocyDawki.PobierzDaneWzorcoweIPomiaroweFormaUwag())
            {
                radioButton2.Checked = true;
                WyswietlDaneWzorcoweIPomiaroweFormaUwag();
            }
            else if (_WzorcowanieSygMocyDawki.PobierzDaneWzorcoweIPomiaroweFormaTabelowa())
            {
                WyswietlDaneWzorcoweIPomiaroweFormaTabelowa();
            }
        }

        //---------------------------------------------------------------
        private void WyswietlDaneWzorcoweIPomiaroweFormaUwag()
        //---------------------------------------------------------------
        {
            textBox13.Text = _WzorcowanieSygMocyDawki.Pomiary.Uwagi;
        }

        //---------------------------------------------------------------
        private void WyswietlDaneWzorcoweIPomiaroweFormaTabelowa()
        //---------------------------------------------------------------
        {
            foreach (KlasyPomocniczeSygMocyDawki.DawkaWartosciWzorcowoPomiarowe.DawkaWartoscWzorcowoPomiarowa dane in _WzorcowanieSygMocyDawki.Pomiary.Dane)
            {
                dataGridView1.Rows.Add(dane.Prog, dane.Odleglosc1, dane.Zrodlo1, dane.Odleglosc2, dane.Zrodlo2, dane.WartoscZmierzona, dane.Niepewnosc, dane.Wspolczynnik, dane.NiepewnoscWspolczynnika);
            }

            WyswietlKonkretnaJednostke();
            WyswietlKonkretnyProtokol();
        }

        //---------------------------------------------------------------
        private void WyswietlDaneProtokolow()
        //---------------------------------------------------------------
        {
            if (_WzorcowanieSygMocyDawki.PobierzDaneProtokolow())
            {
                textBox11.Text = _WzorcowanieSygMocyDawki.Protokol.Wzorcujacy;
                textBox12.Text = _WzorcowanieSygMocyDawki.Protokol.Sprawdzajacy;
                checkBox1.Checked = _WzorcowanieSygMocyDawki.Protokol.Dolacz;
            }
        }

        //---------------------------------------------------------------
        private void WyswietlKonkretnaJednostke()
        //---------------------------------------------------------------
        {
            if (_WzorcowanieSygMocyDawki.PobierzKonkretnaJednostke())
            {
                int index = comboBox4.Items.IndexOf(_WzorcowanieSygMocyDawki.WybranaJednostka);

                if (index >= 0)
                    comboBox4.SelectedIndex = index;
            }
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
            if (true == _WzorcowanieSygMocyDawki.SprawdzCzyArkuszJestJuzZapisany())
            {
                if (false == PrzygotujDaneDoZapisu(true))
                    return false;

                // Nadpisywanie danych
                _WzorcowanieSygMocyDawki.NadpiszDane();
            }
            else
            {
                if (false == PrzygotujDaneDoZapisu(false))
                    return false;

                Properties.Settings.Default.Wykonal = textBox11.Text;
                Properties.Settings.Default.Sprawdzil = textBox12.Text;
                Properties.Settings.Default.Save();

                // Zapisywanie danych
                _WzorcowanieSygMocyDawki.ZapiszDane();
            }

            return true;
        }

        // włącz tryb tabelowy
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                textBox13.Enabled = false;
                textBox13.Clear();
                dataGridView1.Enabled = true;
                comboBox3.Enabled = comboBox4.Enabled = true;
                button4.Enabled = true;
            }
        }

        // włącz tryb opisowy
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                textBox13.Enabled = true;
                dataGridView1.Enabled = false;
                comboBox3.Enabled = comboBox4.Enabled = false;
                button4.Enabled = false;
            }
        }

        // tworzenie protokołu
        private void protokółToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            string sciezka = _DocumentationPathsLoader.GetPath("ProtokolySygnalizacjaMocyDawkiWynik", Jezyk.PL) + textBox1.Text + "SygnalizacjaMocyDawki.html";
            WzorcowanieSygnalizacjaMocyDawkiDataModel dataModel = new WzorcowanieSygnalizacjaMocyDawkiDataModel (
                new DanePodstawoweModel(textBox1.Text, textBox2.Text, dateTimePicker1.Value),
                new DanePrzyrzaduModel(textBox3.Text, textBox4.Text, textBox5.Text, comboBox1.Text, comboBox2.Text, textBox6.Text),
                new DaneWarunkowModel(textBox7.Text, textBox8.Text, textBox9.Text, textBox10.Text),
                new DaneWspolczynnikowModel(textBox11.Text, textBox12.Text));

            // opis
            Dokumenty.Wydruki dokument = null;
            if(radioButton2.Checked)
            {
                dataModel.uwagiWzorcowe = textBox13.Text;
                dokument = new Dokumenty.ProtokolSygMocyDawkiOpis(dataModel);
            }
            else
            {
                dataModel.uwagiDoWarunkow = textBox10.Text;
                dataModel.jednostka = comboBox4.Text;
                dataModel.dataGridView = dataGridView1;
                dokument = new Dokumenty.ProtokolSygMocyDawkiTabela(dataModel);
            }

            if (false == dokument.generateDocument(sciezka))
            {
                MessageBox.Show("Nie podano wszystkich potrzebnych danych!", "Uwaga");
            }
        }

        //****************************************************************************************
        private void comboBox3_SelectedIndexChanged_1(object sender, EventArgs e)
        //****************************************************************************************
        {
            LiczWartoscOrazNiepewnosc();
        }

        //****************************************************************************************
        private void comboBox4_SelectedIndexChanged_1(object sender, EventArgs e)
        //****************************************************************************************
        {
            LiczWartoscOrazNiepewnosc();
        }

        //****************************************************************************************
        private void button4_Click_1(object sender, EventArgs e)
        //****************************************************************************************
        {
            LiczWartoscOrazNiepewnosc();
        }

        //****************************************************************************************
        private void button3_Click(object sender, EventArgs e)
        //****************************************************************************************
        {
            DaneWzorcoweMocDawki okno = new DaneWzorcoweMocDawki();

            try
            {
                okno.WyswietlDane(dateTimePicker1.Value, new DateTime(int.Parse(comboBox3.Text.Substring(0, 4)), int.Parse(comboBox3.Text.Substring(5, 2)), int.Parse(comboBox3.Text.Substring(8, 2))), comboBox4.Text);
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

        private void PrzejdzDoKolejnegoPola(object sender, KeyPressEventArgs e)
        {
            // if enter
            if (e.KeyChar == 13)
                SelectNextControl((Control)sender, true, false, true, true);
        }

        private void zapiszToolStripMenuItem_Click(object sender, EventArgs e)
        {
            N.PotwierdzenieZapisz(this, ZapiszDane, false, true);
        }

    }
}
