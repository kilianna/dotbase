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
    public partial class WzorcowanieSygnalizacjaDawkiForm : Form
    {
        private const string SYGNALIZACJA_DAWKI = "sd";
        private WzorcowanieSygDawki _WzorcowanieSygDawki;
        DocumentationPathsLoader _DocumentationPathsLoader = new DocumentationPathsLoader();

        //---------------------------------------------------------------
        public WzorcowanieSygnalizacjaDawkiForm(int idKarty)
        //---------------------------------------------------------------
        {
            InitializeComponent();
            
            _WzorcowanieSygDawki = new WzorcowanieSygDawki(idKarty, SYGNALIZACJA_DAWKI);

            comboBox1.SelectedIndexChanged += new System.EventHandler(comboBox1_SelectedIndexChanged);
            comboBox2.SelectedIndexChanged += new System.EventHandler(comboBox2_SelectedIndexChanged);

            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(ZamykanieOkna);
        }

        //---------------------------------------------------------------
        private void button4_Click(object sender, EventArgs e)
        //---------------------------------------------------------------
        {
            LiczCzasWzorcowy();
        }

        //---------------------------------------------------------------
        private bool PrzygotujDaneDoZapisu(bool NadpisywanieStarychDanych)
        //---------------------------------------------------------------
        {
            if (false == SprawdzPoprawnoscDanych())
                return false;

            if (false == _WzorcowanieSygDawki.PrzygotujDaneOgolneDoZapisu(textBox1.Text, textBox2.Text, dateTimePicker1.Value, NadpisywanieStarychDanych))
            {
                MessageBox.Show("Dane ogólne są błędne.", "Uwaga");
                return false;
            }

            if (false == _WzorcowanieSygDawki.PrzygotujDanePrzyrzaduDoZapisu(textBox3.Text, textBox4.Text, textBox6.Text, textBox5.Text, comboBox1.Text, comboBox2.Text))
            {
                MessageBox.Show("Dane przyrządu są błędne.", "Uwaga");
                return false;
            }

            if (false == _WzorcowanieSygDawki.PrzygotujDaneWarunkowDoZapisu(textBox7.Text, textBox8.Text, textBox9.Text, textBox10.Text))
            {
                MessageBox.Show("Dane warunków są błędne.", "Uwaga");
                return false;
            }

            if (false == _WzorcowanieSygDawki.PrzygotujDaneWzorcowoPomiaroweDoZapisu(ref dataGridView1, comboBox3.Text, textBox15.Text, textBox14.Text, comboBox4.Text))
            {
                MessageBox.Show("Dane wzorcowo-pomiarowe są błędne.", "Uwaga");
                return false;
            }

            if (false == _WzorcowanieSygDawki.PrzygotujDaneProtokolowDoZapisu(textBox11.Text, textBox12.Text, checkBox1.Checked))
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
            LiczCzasWzorcowy();
        }

        //---------------------------------------------------------------
        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        //---------------------------------------------------------------
        {
            LiczCzasWzorcowy();
        }

        //---------------------------------------------------------------
        private void CzyscOkno()
        //---------------------------------------------------------------
        {
            textBox5.Text = textBox6.Text = "nie dotyczy";
            comboBox1.Text = comboBox2.Text = "";
            textBox7.Text = textBox8.Text = textBox9.Text = "0";
            textBox10.Text = "brak";
            textBox14.Text = textBox15.Text = "0";
            comboBox3.Text = comboBox4.Text = "";
            dataGridView1.Rows.Clear();
            textBox11.Clear();
            textBox12.Text = "P. Bilski";
            checkBox1.Checked = true;
        }

        //---------------------------------------------------------------
        public bool Inicjalizuj()
        //---------------------------------------------------------------
        {
            if (_WzorcowanieSygDawki.Inicjalizuj())
            {
                textBox1.Text = _WzorcowanieSygDawki.IdKarty.ToString();
                textBox2.Text = _WzorcowanieSygDawki.IdArkusza.ToString();
                UstawDzisiejszaDate();
                WyswietlDane();
                return true;
            }
            else
            {
                textBox1.Text = _WzorcowanieSygDawki.IdKarty.ToString();
                textBox2.Text = _WzorcowanieSygDawki.IdArkusza.ToString();
                UstawDzisiejszaDate();
                WyswietlDanePodstawowe();
                return true;
            }
        }

        //---------------------------------------------------------------
        private void LiczCzasWzorcowy()
        //---------------------------------------------------------------
        {
            List<double> progi = TestujMozliwoscObliczeniaCzasu();

            if (progi != null)
            {
                List<double> wyniki = _WzorcowanieSygDawki.LiczCzasWzorcowy(Double.Parse(textBox14.Text), Int32.Parse(textBox15.Text), comboBox3.Text, comboBox4.Text, progi, dateTimePicker1.Value);

                for (UInt16 i = 0; i < wyniki.Count; ++i)
                    dataGridView1.Rows[i].Cells["tWzorcowy"].Value = wyniki[i].ToString("0.0");      
                
            }

            List<double> czas = TestujMozliwoscObliczeniaWartosciRzeczywistej();

            if (progi != null)
            {
                List<double> wyniki = _WzorcowanieSygDawki.LiczWartoscRzecyzwista(Double.Parse(textBox14.Text), Int32.Parse(textBox15.Text), comboBox3.Text, comboBox4.Text, czas, dateTimePicker1.Value);

                for (UInt16 i = 0; i < wyniki.Count; ++i)
                    dataGridView1.Rows[i].Cells["wartRzeczywista"].Value = wyniki[i].ToString("0.00");
                
            }
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
                textBox10.Text = "Brak";

            // jeśli nie podano wybierz ostatni protokół
            if (comboBox3.Text == "")
                comboBox3.SelectedItem = comboBox3.Items[comboBox3.Items.Count - 1];

            // jeśli nie podano wybierz ostatni protokół
            if (comboBox4.Text == "")
                comboBox4.SelectedItem = comboBox3.FindString("mSv/h");

            // odległość
            if (textBox14.Text == "")
                textBox14.Text = "0";

            // źródło
            if (textBox15.Text == "")
                textBox15.Text = "0";

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

            textBox2.Text = _WzorcowanieSygDawki.IdArkusza.ToString();
            dateTimePicker1.Value = DateTime.Today;

            WyswietlPodstawoweDanePrzyrzadu();
            WyswietlWszystkieJednostki();
            WyswietlWszystkieProtokoly();
            WyswietlWszystkieSondy();

            int nrPoprzedniejKalibracji = _WzorcowanieSygDawki.ZnajdzNrPoprzedniejKalibracji(textBox3.Text, textBox4.Text);

            if (0 < nrPoprzedniejKalibracji)
            {
                textBox10.Text = "Numer poprzedniego świadectwa " + nrPoprzedniejKalibracji + "/" + _WzorcowanieSygDawki.PobierzRok(nrPoprzedniejKalibracji);
            }
        }

        //---------------------------------------------------------------
        private void WyswietlDanePrzyrzadu()
        //---------------------------------------------------------------
        {
            if (_WzorcowanieSygDawki.PobierzDanePrzyrzadu())
            {
                KlasyPomocniczeCez.Przyrzad przyrzad = _WzorcowanieSygDawki.Przyrzad;

                textBox3.Text = przyrzad.TypDozymetru;
                textBox4.Text = przyrzad.NrFabrycznyDozymetru;
                textBox5.Text = przyrzad.InneNastawy;
                textBox6.Text = przyrzad.NapiecieZasilaniaSondy;

                comboBox1.Items.Clear();
                comboBox2.Items.Clear();

                for (int i = 0; i < _WzorcowanieSygDawki.Sondy.Lista.Count; ++i)
                {
                    comboBox1.Items.Add(_WzorcowanieSygDawki.Sondy.Lista[i].Typ);
                    comboBox2.Items.Add(_WzorcowanieSygDawki.Sondy.Lista[i].NrFabryczny);
                }

                Sonda s = _WzorcowanieSygDawki.ZnajdzSondeWybranaPrzezUzytkwonika();

                comboBox2.SelectedIndex = comboBox2.Items.IndexOf(s.NrFabryczny);
            }
        }

        //---------------------------------------------------------------
        private void WyswietlDateWzorcowania()
        //---------------------------------------------------------------
        {
            if (_WzorcowanieSygDawki.PobierzDateWzorcowania())
            {
                dateTimePicker1.Value = _WzorcowanieSygDawki.OdpowiedzBazy.Rows[0].Field<DateTime>(0);
            }
        }

        //---------------------------------------------------------------
        private void WyswietlDaneWarunkow()
        //---------------------------------------------------------------
        {
            if (_WzorcowanieSygDawki.PobierzDaneWarunkow())
            {
                KlasyPomocniczeCez.Warunki warunki = _WzorcowanieSygDawki.Warunki;

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
            if (_WzorcowanieSygDawki.PobierzKonkretnyProtokol())
            {
                int index = comboBox3.Items.IndexOf(_WzorcowanieSygDawki.WybranyProtokol);

                if (index >= 0)
                    comboBox3.SelectedIndex = index;
            }
        }

        //---------------------------------------------------------------
        private void WyswietlPodstawoweDanePrzyrzadu()
        //---------------------------------------------------------------
        {
            if (_WzorcowanieSygDawki.PobierzPodstawoweDanePrzyrzadu())
            {
                textBox3.Text = _WzorcowanieSygDawki.Przyrzad.TypDozymetru;
                textBox4.Text = _WzorcowanieSygDawki.Przyrzad.NrFabrycznyDozymetru;
            }
        }

        //---------------------------------------------------------------
        private void WyswietlWszystkieJednostki()
        //---------------------------------------------------------------
        {
            if (_WzorcowanieSygDawki.PobierzWszystkieJednostki())
            {
                foreach (string nazwa in _WzorcowanieSygDawki.Jednostki.Nazwy)
                {
                    comboBox4.Items.Add(nazwa);
                }
            }
        }

        //---------------------------------------------------------------
        private void WyswietlWszystkieProtokoly()
        //---------------------------------------------------------------
        {
            if (_WzorcowanieSygDawki.PobierzWszystkieProtokoly())
            {
                foreach (DateTime data in _WzorcowanieSygDawki.Protokoly.Daty)
                {
                    comboBox3.Items.Add(data.ToShortDateString());
                }
            }
        }

        //---------------------------------------------------------------
        private void WyswietlWszystkieSondy()
        //---------------------------------------------------------------
        {
            if (_WzorcowanieSygDawki.PobierzWszystkieMozliweSondy())
            {
                foreach (Sonda sonda in _WzorcowanieSygDawki.Sondy.Lista)
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
            if (_WzorcowanieSygDawki.PobierzDaneWzorcoweIPomiarowe())
            {
                foreach (KlasyPomocniczeSygDawki.DawkaWartosciWzorcowoPomiarowe.DawkaWartoscWzorcowoPomiarowa dane in _WzorcowanieSygDawki.Pomiary.Dane)
                {
                    dataGridView1.Rows.Add(dane.Prog, "", dane.Tzmierzony, dane.WartRzeczywista, dane.WartZmierzona);
                }
            }

            WyswietlKonkretnaJednostke();
            WyswietlKonkretnyProtokol();

            textBox14.Text = _WzorcowanieSygDawki.Pomiary.odleglosc;
            textBox15.Text = _WzorcowanieSygDawki.Pomiary.zrodlo;
        }

        //---------------------------------------------------------------
        private void WyswietlDaneProtokolow()
        //---------------------------------------------------------------
        {
            if (_WzorcowanieSygDawki.PobierzDaneProtokolow())
            {
                textBox11.Text = _WzorcowanieSygDawki.Protokol.Wzorcujacy;
                textBox12.Text = _WzorcowanieSygDawki.Protokol.Sprawdzajacy;
                checkBox1.Checked = _WzorcowanieSygDawki.Protokol.Dolacz;
            }
        }

        //---------------------------------------------------------------
        private void WyswietlKonkretnaJednostke()
        //---------------------------------------------------------------
        {
            if(_WzorcowanieSygDawki.PobierzKonkretnaJednostke())
            {
                int index = comboBox4.Items.IndexOf(_WzorcowanieSygDawki.WybranaJednostka);

                if (index >= 0)
                    comboBox4.SelectedIndex = index;
            }
        }

        #endregion

        //---------------------------------------------------------------
        // jeśli nie jest to możliwe zwróci null
        List<double> TestujMozliwoscObliczeniaWartosciRzeczywistej()
        //---------------------------------------------------------------
        {
            int zmiennaTestowaInt;
            double zmiennaTestowaDouble;

            if (!Double.TryParse(textBox14.Text, out zmiennaTestowaDouble) ||
                !Int32.TryParse(textBox15.Text, out zmiennaTestowaInt) ||
                "" == comboBox3.Text || "" == comboBox4.Text)
                return null;

            List<double> czasZmierzony = new List<double>();

            for (UInt16 i = 0; dataGridView1.Rows[i].Cells[0].Value != null &&
                               dataGridView1.Rows[i].Cells[1].Value != null &&
                               dataGridView1.Rows[i].Cells[2].Value != null && i < dataGridView1.Rows.Count - 1; ++i)
            {
                if (dataGridView1.Rows[i].Cells["tZmierzony"].Value != null &&
                    Double.TryParse(dataGridView1.Rows[i].Cells["tZmierzony"].Value.ToString(), out zmiennaTestowaDouble))
                {
                    czasZmierzony.Add(zmiennaTestowaDouble);
                }
                else
                {
                    return null;
                }
            }

            return czasZmierzony;
        }

        //---------------------------------------------------------------
        // jeśli nie jest to możliwe zwróci null
        List<double> TestujMozliwoscObliczeniaCzasu()
        //---------------------------------------------------------------
        {
            int zmiennaTestowaInt;
            double zmiennaTestowaDouble;

            if (!Double.TryParse(textBox14.Text, out zmiennaTestowaDouble) || 
                !Int32.TryParse(textBox15.Text, out zmiennaTestowaInt)     ||
                "" == comboBox3.Text || "" == comboBox4.Text)
                return null;


            List<double> progi = new List<double>();

            for (UInt16 i = 0; dataGridView1.Rows[i].Cells[0].Value != null && i < dataGridView1.Rows.Count - 1; ++i)
            {
                if (dataGridView1.Rows[i].Cells["Prog"].Value != null && 
                    Double.TryParse(dataGridView1.Rows[i].Cells["Prog"].Value.ToString(), out zmiennaTestowaDouble))
                {
                    progi.Add(zmiennaTestowaDouble);
                }
                else
                {
                    return null;
                }
            }

            return progi;
        }

        //---------------------------------------------------------------
        private void ZamykanieOkna(object sender, FormClosingEventArgs e)
        //---------------------------------------------------------------
        {
            if (false == ZapiszDane())
            {
                if (MessageBox.Show("Czy na pewno chcesz zamknąć okno? Dane nie zostaną zapisane z uwagi na błędy wśród wpisanych dnaych.", "Uwaga", MessageBoxButtons.YesNo) == DialogResult.No)
                    e.Cancel = true;
            }
        }

        //---------------------------------------------------------------
        private bool ZapiszDane()
        //---------------------------------------------------------------
        {
            if (true == _WzorcowanieSygDawki.SprawdzCzyArkuszJestJuzZapisany())
            {
                if (false == PrzygotujDaneDoZapisu(true))
                    return false;

                // Nadpisywanie danych
                _WzorcowanieSygDawki.NadpiszDane();
            }
            else
            {
                if (false == PrzygotujDaneDoZapisu(false))
                    return false;

                // Zapisywanie danych
                _WzorcowanieSygDawki.ZapiszDane();
            }

            return true;
        }

        // tworzenie protokołu
        private void protokółToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string sciezka = _DocumentationPathsLoader.GetPath("ProtokolySygnalizacjaDawkiWynik") + textBox1.Text + "SygnalizacjaDawki.html";

            Dokumenty.ProtokolSygnalizacjaDawkiModel model = new Dokumenty.ProtokolSygnalizacjaDawkiModel(
                new DanePodstawoweModel(textBox1.Text, textBox2.Text, dateTimePicker1.Value),
                new DanePrzyrzaduModel(textBox3.Text, textBox4.Text, textBox5.Text, comboBox1.Text, comboBox2.Text, textBox6.Text),
                new DaneWarunkowModel(textBox7.Text, textBox8.Text, textBox9.Text, textBox10.Text),
                new DaneWspolczynnikowModel(textBox11.Text, textBox12.Text));
            
            model.odleglosc = textBox14.Text;
            model.zrodlo = textBox15.Text;
            model.jednostka = comboBox4.Text;
            model.tabela = dataGridView1;
            

            Dokumenty.ProtokolSygnalizacjaDawki protokol = new Dokumenty.ProtokolSygnalizacjaDawki(model);
            if (!protokol.generateDocument(sciezka))
            {
                MessageBox.Show("Nie podano wszystkich potrzebnych danych!", "Uwaga");
            }
        }

        //*******************************************************************************************************************
        private void DodajWiersz(object sender, DataGridViewRowsAddedEventArgs e)
        //*******************************************************************************************************************
        {
            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells["tZmierzony"].Value = "0,0";
        }

        //*******************************************************************************************************************
        private void button3_Click(object sender, EventArgs e)
        //*******************************************************************************************************************
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

        private void PrzejdzDoKolejnegoPola(object sender, KeyPressEventArgs e)
        {
            // if enter
            if (e.KeyChar == 13)
                SelectNextControl((Control)sender, true, false, true, true);
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
    }
}
