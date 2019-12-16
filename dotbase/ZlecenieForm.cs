using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DotBase.PrzestrzenZlecenia;

namespace DotBase
{
    public partial class ZlecenieForm : Form
    {
        Zlecenie _Zlecenie;
        bool _TrybDodawaniaZlecenia;
        bool _TrybDodawaniaZleceniodawcy;
        bool _TrybEdycji;
        
        //--------------------------------------------------------------------
        public ZlecenieForm()
        //--------------------------------------------------------------------
        {
            InitializeComponent();

            button4.Enabled = false;

            textBox10.Text = Properties.Settings.Default.Przyjal;
            
            _Zlecenie = new Zlecenie();

            if (false == ZaladujDanePoczatkowe())
            {
                MessageBox.Show("Brak danych do wyświetlenia. Może być to problem z brakiem połączenia z bazą danych.");
            }
            else
            {
                WyswietlWszystkieDane(_Zlecenie.Dane);
            }

            FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Close);
            comboBox1.Items.AddRange(_Zlecenie.Zleceniodawcy);
            comboBox1.SelectedValueChanged += comboBox1_SelectedValueChanged;
            comboBox1.KeyUp += comboBox1_KeyUp;

            textBox1.TextChanged += new EventHandler(ZnajdzPoId);
            numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);

            WylaczTrybPrzegladania();
        }

        //--------------------------------------------------------------------
        // Dodawanie Zlecenia
        private void button1_Click(object sender, EventArgs e)
        //--------------------------------------------------------------------
        {
            if (_TrybDodawaniaZlecenia)
            {
                ZatwierdzDodanieZlecenia();
            }
            else
            {
                WlaczTrybDodawaniaZlecenia();
            }
        }

        //--------------------------------------------------------------------
        // Usunięcie zlecenia
        private void button2_Click(object sender, EventArgs e)
        //--------------------------------------------------------------------
        {
            if (DialogResult.No == MessageBox.Show("Czy na pewno usunąć?", "Usuwanie zlecenia", MessageBoxButtons.YesNo))
                return;

            _Zlecenie.UsunZlecenie((int)numericUpDown1.Value);

            if (DialogResult.No == MessageBox.Show("Czy usunąć karty powiązane ze zleceniem?", "Usuwanie kart", MessageBoxButtons.YesNo))
            {
                numericUpDown1.Value = _Zlecenie.ZnajdzOstatnieZlecenie();
                return;
            }
            
            _Zlecenie.UsunKarty((int)numericUpDown1.Value);
            numericUpDown1.Value = _Zlecenie.ZnajdzOstatnieZlecenie();
        }

        //--------------------------------------------------------------------
        private void ZnajdzPoId(object sender, EventArgs e)
        //--------------------------------------------------------------------
        {
            if (_Zlecenie.ZnajdzPoIdZleceniodawcy(textBox1.Text))
            {
                WyswietlDaneZleceniodawcy(_Zlecenie.Dane);
                SprawdzInnegoPlatnika();
            }
            else
            {
                CzyscOknoZleceniodawcy();
            }
        }


        //--------------------------------------------------------------------
        // Dodanie nowej karty
        private void button3_Click(object sender, EventArgs e)
        //--------------------------------------------------------------------
        {
            KartaPrzyjeciaForm  _KartaPrzyjeciaForm = new KartaPrzyjeciaForm();

            if (_KartaPrzyjeciaForm.InicjalizujTrybPelnegoDostepu((int)numericUpDown1.Value))
            {
                _KartaPrzyjeciaForm.ShowDialog();
                
                if( _Zlecenie.ZaladujDanePrzyrzadow((int)numericUpDown1.Value) )
                {
                    dataGridView1.Rows.Clear();
                    WyswietlDanePrzyrzadow(_Zlecenie.Dane);
                }
            }
            else
            {
                MessageBox.Show("Połączenie z bazą został przerwane.");
            }
        }

        //--------------------------------------------------------------------
        // Dodanie nowego zleceniodawcy
        private void button4_Click(object sender, EventArgs e)
        //--------------------------------------------------------------------
        {
            if (_TrybDodawaniaZleceniodawcy)
            {
                ZatwierdzDodanieZleceniodawcy();
                WylaczTrybDodawaniaZleceniodawcy();
                button4.Text = "Dodaj nowego zleceniodawcę";
            }
            else
            {
                WlaczTrybDodawaniaZleceniodawcy();
            }
        }

        //--------------------------------------------------------------------
        // Edycja danych zleceniodawcy
        private void button5_Click(object sender, EventArgs e)
        //--------------------------------------------------------------------
        {
            if (_TrybEdycji)
            {
                if (ZatwierdzEdycjeZleceniodawcy())
                {
                    WylaczTrybPrzegladania();
                }
                else
                {
                    MessageBox.Show(this, "Nie zapisano danych z powodu błędu.", "Błąd zapisu do bazy danych", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                WlaczTrybEdycji();
            }
        }

        //--------------------------------------------------------------------
        // Zmiana numeru zlecenia. Powoduje zapisanie obecnych danych i wyświetlenie nowych
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        //--------------------------------------------------------------------
        {
            OdblokujPrzyciski();

            if (true == _TrybDodawaniaZlecenia)
            {
                ZapiszDane();
                CzyscOkno();
                return;
            }

            if (numericUpDown1.Value > _Zlecenie.ZnajdzOstatnieZlecenie())
            {
                CzyscOkno();
                ZablokujPrzyciski();
                MessageBox.Show("Zlecenie o tym numerze nie istnieje lub połączenie z bazą zostało zerwane.");
                return;
            }
            
            if( 0 != String.Empty.CompareTo(textBox1.Text) )
                ZapiszDane();
            CzyscOkno();

            int idZlecenia = (int)numericUpDown1.Value;

            if ( _Zlecenie.ZaladujWszystkieDane(idZlecenia) )
            {
                WyswietlWszystkieDane(_Zlecenie.Dane);
            }
            else
            {
                MessageBox.Show("Brak danych dla tego numeru.");
            }
        }

        //--------------------------------------------------------------------
        private void CzyscOkno()
        //--------------------------------------------------------------------
        {
            textBox1.Text = textBox2.Text = textBox3.Text = textBox4.Text = textBox5.Text =
            textBox6.Text = textBox7.Text = textBox8.Text = textBox10.Text = emailTextBox.Text = "";
            nazwaPlatnika.Text = "";
            adresPlatnika.Text = "";
            nipPlatnika.Text = "";

            textBox9.Text = "Brak";

            comboBox1.Text = "";
            checkBox1.Checked = false;
            jestIFJ.Checked = false;

            dateTimePicker1.Value = dateTimePicker1.MinDate;
            dateTimePicker2.Value = dateTimePicker1.MinDate.AddDays(14);

            dataGridView1.Rows.Clear();

            textBox10.Text = Properties.Settings.Default.Przyjal;
        }

        //--------------------------------------------------------------------
        private void CzyscOknoZleceniodawcy()
        //--------------------------------------------------------------------
        {
            textBox2.Text = textBox3.Text = textBox4.Text = textBox5.Text = textBox6.Text = emailTextBox.Text = "";
            comboBox1.Text = "";
            nazwaPlatnika.Text = "";
            adresPlatnika.Text = "";
            nipPlatnika.Text = "";
            jestIFJ.Checked = false;
        }

        //--------------------------------------------------------------------
        private void OdblokujPrzyciski()
        //--------------------------------------------------------------------
        {
            button1.Enabled = button2.Enabled = button3.Enabled = button4.Enabled = button5.Enabled = true;
            tabControl1.Enabled = true;
        }

        //--------------------------------------------------------------------
        private void ZablokujPrzyciski()
        //--------------------------------------------------------------------
        {
            button1.Enabled = button2.Enabled = button3.Enabled = button4.Enabled = button5.Enabled = false;
            tabControl1.Enabled = false;
        }

        //--------------------------------------------------------------------
        private void comboBox1_SelectedValueChanged(object sender, EventArgs args)
        //--------------------------------------------------------------------
        {
            if (true == _Zlecenie.ZaladujWszystkichZleceniodawcow(comboBox1.Text))
            {
                _Zlecenie.WypelnijDaneZleceniodawcy();
                WyswietlDaneZleceniodawcy(_Zlecenie.Dane);
                SprawdzInnegoPlatnika();
            }
            else
            {
                MessageBox.Show("Brak danych do wyświetlenia. Może być to problem z brakiem połączenia z bazą danych.");
            }
        }

        private void SprawdzInnegoPlatnika()
        {
            innyPlatnik.Checked = (nazwaPlatnika.Text.Trim() != "" && nazwaPlatnika.Text.Trim() != "-")
                || (adresPlatnika.Text.Trim() != "" && adresPlatnika.Text.Trim() != "-")
                || (nipPlatnika.Text.Trim() != "" && nipPlatnika.Text.Trim() != "-");
        }
        
        //--------------------------------------------------------------------
        private void WlaczTrybDodawaniaZlecenia()
        //--------------------------------------------------------------------
        {
            _TrybDodawaniaZlecenia = true;
            button1.Text = "Zatwierdź";
            
            numericUpDown1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button5.Enabled = false;

            CzyscOkno();
            
            numericUpDown1.ValueChanged -= this.numericUpDown1_ValueChanged;
            numericUpDown1.Value = _Zlecenie.ZnajdzOstatnieZlecenie() + 1;
            this.numericUpDown1.ValueChanged += this.numericUpDown1_ValueChanged;

            dateTimePicker1.Value = DateTime.Today;
            dateTimePicker2.Value = DateTime.Today.AddDays(14);
        }

        //--------------------------------------------------------------------
        private void WylaczTrybDodawaniaZlecenia()
        //--------------------------------------------------------------------
        {
            numericUpDown1.Enabled = button2.Enabled = button3.Enabled = button5.Enabled = true;
            button1.Text = "Nowe zlecenie";

            numericUpDown1.Enabled = true;
            _TrybDodawaniaZlecenia = false;
        }

        //--------------------------------------------------------------------
        private void WlaczTrybDodawaniaZleceniodawcy()
        //--------------------------------------------------------------------
        {
            _TrybDodawaniaZleceniodawcy = true;
            textBox1.TextChanged -= ZnajdzPoId;

            textBox1.ReadOnly = true;
            button1.Enabled = button2.Enabled = button3.Enabled = button5.Enabled = false;

            numericUpDown1.Enabled = numericUpDown2.Enabled = false;

            textBox2.Enabled = textBox3.Enabled = textBox4.Enabled = emailTextBox.Enabled = textBox5.Enabled = textBox6.Enabled = jestIFJ.Enabled = true;
            textBox2.Text = textBox3.Text = textBox4.Text = emailTextBox.Text = textBox5.Text = textBox6.Text = "-";
            jestIFJ.Checked = false;
            nazwaPlatnika.Text = "";
            adresPlatnika.Text = "";
            nipPlatnika.Text = "";
            innyPlatnik.Checked = false;
            grupaPlatnika.Visible = false;
            nazwaPlatnika.Enabled = true;
            adresPlatnika.Enabled = true;
            nipPlatnika.Enabled = true;

            button4.Text = "Zatwierdź dodanie zleceniodawcy";

            comboBox1.Text = "";

            int id = _Zlecenie.ZnajdzMaxIDZleceniodawcy() + 1;
            textBox1.Text = id.ToString();
            textBox1.TextChanged += ZnajdzPoId;
        }

        //--------------------------------------------------------------------
        private void WylaczTrybDodawaniaZleceniodawcy()
        //--------------------------------------------------------------------
        {
            _TrybDodawaniaZleceniodawcy = false;
            textBox1.ReadOnly = false;
            button1.Enabled = true;


            button1.Enabled = button2.Enabled = button3.Enabled = button5.Enabled = true;
            numericUpDown1.Enabled = numericUpDown2.Enabled = true;
            textBox2.Enabled = textBox3.Enabled = textBox4.Enabled = emailTextBox.Enabled = textBox5.Enabled = textBox6.Enabled = jestIFJ.Enabled = false;

            nazwaPlatnika.Enabled = false;
            adresPlatnika.Enabled = false;
            nipPlatnika.Enabled = false;
            
            button4.Text = "Dodaj nowego zleceniodawcę";

        }

        //--------------------------------------------------------------------
        private void WlaczTrybEdycji()
        //--------------------------------------------------------------------
        {
            _TrybEdycji = true;
            button1.Enabled = button2.Enabled = button3.Enabled = button4.Enabled = false;
            textBox1.Enabled = numericUpDown1.Enabled = numericUpDown2.Enabled = false;
            button5.Text = "Zatwierdź edycję.";

            textBox2.Enabled = textBox3.Enabled =
            textBox4.Enabled = textBox5.Enabled = emailTextBox.Enabled = textBox6.Enabled = jestIFJ.Enabled = true;
            nazwaPlatnika.Enabled = true;
            adresPlatnika.Enabled = true;
            nipPlatnika.Enabled = true;
        }

        //--------------------------------------------------------------------
        private void WylaczTrybPrzegladania()
        //--------------------------------------------------------------------
        {
            _TrybEdycji = false;
            button1.Enabled = button2.Enabled = button3.Enabled = button4.Enabled = true;
            textBox1.Enabled = numericUpDown1.Enabled = numericUpDown2.Enabled = true;
            button5.Text = "Edytuj dane zleceniodawcy";

            textBox2.Enabled = textBox3.Enabled = textBox4.Enabled = emailTextBox.Enabled =
            textBox5.Enabled = textBox6.Enabled = jestIFJ.Enabled = false;
            nazwaPlatnika.Enabled = false;
            adresPlatnika.Enabled = false;
            nipPlatnika.Enabled = false;
        }

        //--------------------------------------------------------------------
        private void WyswietlDaneZlecenia(DaneZlecenia dane)
        //--------------------------------------------------------------------
        {
            dateTimePicker1.Value = dane.DataPrzyjecia;
            dateTimePicker2.Value = dane.DataZwrotu;
            numericUpDown2.Value = dane.Nr_rejestru;
            textBox7.Text = dane.FormaPrzyjecia;
            textBox8.Text = dane.FormaZwrotu;
            textBox9.Text = dane.Uwagi;
            textBox10.Text = dane.OsobaPrzyjmujaca;
            checkBox1.Checked = dane.Ekspress;
            innyPlatnik.Checked = dane.InnyPlatnik;
        }

        //--------------------------------------------------------------------
        private void WyswietlDanePrzyrzadow(DaneZlecenia dane)
        //--------------------------------------------------------------------
        {
            int lp = 1;
            dataGridView1.Rows.Clear();

            foreach(DanePrzyrzad przyrzad in _Zlecenie.Dane.Przyrzady)
            {
                dataGridView1.Rows.Add(przyrzad.IdKarty, przyrzad.Typ, przyrzad.NrFabryczny, przyrzad.Wykalibrowany);
                var row = dataGridView1.Rows[dataGridView1.Rows.Count - 1];
                row.HeaderCell.Value = lp.ToString();
                lp++;
            }
        }

        //--------------------------------------------------------------------
        private void WyswietlDaneZleceniodawcy(DaneZlecenia dane)
        //--------------------------------------------------------------------
        {
            textBox1.Text = dane.ZleceniodawcaInfo.Id.ToString();
            comboBox1.Text = dane.ZleceniodawcaInfo.Nazwa;
            textBox2.Text = dane.ZleceniodawcaInfo.Adres;
            textBox3.Text = dane.ZleceniodawcaInfo.Faks;
            textBox4.Text = dane.ZleceniodawcaInfo.Telefon;
            emailTextBox.Text = dane.ZleceniodawcaInfo.Email;
            textBox5.Text = dane.ZleceniodawcaInfo.OsobaKontaktowa;
            textBox6.Text = dane.ZleceniodawcaInfo.Nip;
            nazwaPlatnika.Text = dane.ZleceniodawcaInfo.NazwaPlatnika;
            adresPlatnika.Text = dane.ZleceniodawcaInfo.AdresPlatnika;
            nipPlatnika.Text = dane.ZleceniodawcaInfo.NipPlatnika;
            jestIFJ.Checked = dane.ZleceniodawcaInfo.Ifj;
        }

        //--------------------------------------------------------------------
        private void WyswietlWszystkieDane(DaneZlecenia dane)
        //--------------------------------------------------------------------
        {
            comboBox1.KeyUp -= comboBox1_KeyUp;

            numericUpDown1.Value = dane.Id;
            numericUpDown2.Value = dane.Nr_rejestru;
            WyswietlDaneZleceniodawcy(dane);
            WyswietlDaneZlecenia(dane);
            WyswietlDanePrzyrzadow(dane);

            comboBox1.KeyUp += comboBox1_KeyUp;
        }

        //--------------------------------------------------------------------
        // Zmiany zapisywane są automatycznie przy zmianie numeru zlecenia lub
        // zamykaniu okna. Nie zapisujemy zmian w liście przyrządów gdyż nie można
        // ich zmienić w oknie zlecenia.
        private void ZapiszDane()
        //--------------------------------------------------------------------
        {
            if (false == SprawdzPoprawnoscDanychZleceniodawcy())
                return;

            DaneZlecenia daneDoZapisu = new DaneZlecenia();
            daneDoZapisu.ZleceniodawcaInfo = new DaneZleceniodawcy();
            daneDoZapisu.ZleceniodawcaInfo.Id = int.Parse(textBox1.Text);
            daneDoZapisu.DataPrzyjecia = dateTimePicker1.Value;
            daneDoZapisu.DataZwrotu = dateTimePicker2.Value;
            daneDoZapisu.FormaPrzyjecia = textBox7.Text;
            daneDoZapisu.FormaZwrotu = textBox8.Text;
            daneDoZapisu.Uwagi = textBox9.Text;
            daneDoZapisu.OsobaPrzyjmujaca = textBox10.Text;
            daneDoZapisu.Ekspress = checkBox1.Checked;
            daneDoZapisu.Nip = textBox6.Text;
            daneDoZapisu.InnyPlatnik = innyPlatnik.Checked;
            daneDoZapisu.Nr_rejestru = (int)numericUpDown2.Value;

            _Zlecenie.ZapiszDane(daneDoZapisu);
        }

        //--------------------------------------------------------------------
        // Przy pierwszym otworzeniu okna wyświetlane są dane ostatniego ze zleceń
        private bool ZaladujDanePoczatkowe()
        //--------------------------------------------------------------------
        {
            int id = _Zlecenie.ZnajdzOstatnieZlecenie();

            if (false == _Zlecenie.ZaladujWszystkieDane(id) )
                return false;

            return true;
        }

        //--------------------------------------------------------------------
        // W trybie dodawania nowego zlecenia lub zleceniodawcy zezwala na wyszukiwanie
        // nazw zleceniodawców w bazie, a po ich wyborze wyświetlenie reszty danych
        private void comboBox1_KeyUp(object sender, EventArgs e)
        //--------------------------------------------------------------------
        {
            if (comboBox1.Text.Length < 3)
                return;

            int i = comboBox1.SelectionStart;
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(_Zlecenie.GetMozliwychZleceniodawcow(comboBox1.Text));
            comboBox1.SelectionStart = i;
        }

        //--------------------------------------------------------------------
        private void ZatwierdzDodanieZlecenia()
        //--------------------------------------------------------------------
        {
            if( false == SprawdzPoprawnoscDanychZleceniodawcy())
            {
                MessageBox.Show("Część danych wymaganych do zapisu nie jest podana.");
                return;
            }

            WylaczTrybDodawaniaZlecenia();

            Properties.Settings.Default.Przyjal = textBox10.Text;
            Properties.Settings.Default.Save();

            DaneZlecenia daneZlecenia = new DaneZlecenia();
            daneZlecenia.ZleceniodawcaInfo = new DaneZleceniodawcy();
            daneZlecenia.Id = (int)numericUpDown1.Value;
            daneZlecenia.Nr_rejestru = (int)numericUpDown2.Value;
            daneZlecenia.ZleceniodawcaInfo.Id = int.Parse(textBox1.Text);
            daneZlecenia.DataPrzyjecia = dateTimePicker1.Value;
            daneZlecenia.DataZwrotu = dateTimePicker1.Value;
            daneZlecenia.FormaPrzyjecia = textBox7.Text;
            daneZlecenia.FormaZwrotu = textBox8.Text;
            daneZlecenia.Uwagi = textBox9.Text;
            daneZlecenia.OsobaPrzyjmujaca = textBox10.Text;
            daneZlecenia.Ekspress = checkBox1.Checked;
            daneZlecenia.InnyPlatnik = innyPlatnik.Checked;

            _Zlecenie.DodajZlecenie(ref daneZlecenia);
            _Zlecenie.ZaladujWszystkieDane((int)numericUpDown1.Value);
            OdblokujPrzyciski();
        }

        //--------------------------------------------------------------------
        private bool SprawdzPoprawnoscDanychZleceniodawcy()
        //--------------------------------------------------------------------
        {
            if (0 == String.Empty.CompareTo(textBox1.Text) || 0 == String.Empty.CompareTo(textBox2.Text) ||
                0 == String.Empty.CompareTo(textBox3.Text) || 0 == String.Empty.CompareTo(textBox4.Text))
            {
                return false;
            }

            return true;
        }

        //--------------------------------------------------------------------
        private bool ZatwierdzDodanieZleceniodawcy()
        //--------------------------------------------------------------------
        {
            if (false == SprawdzPoprawnoscDanychZleceniodawcy())
            {
                MessageBox.Show("Nie wszystkie dane zostały wpisane. Wpisz wszystkie dane, aby dodać zleceniodawcę.", "Błąd");
                return false;
            }

            DaneZleceniodawcy zleceniodawca = new DaneZleceniodawcy(textBox2.Text, textBox3.Text, int.Parse(textBox1.Text), comboBox1.Text, textBox6.Text, textBox5.Text, textBox4.Text, emailTextBox.Text,
                nazwaPlatnika.Text, adresPlatnika.Text, nipPlatnika.Text, jestIFJ.Checked);

            if (false == _Zlecenie.DodajZleceniodawce(ref zleceniodawca))
            {
                MessageBox.Show("Podany zleceniodawca już istnieje.", "Błąd");
                return false;
            }

            return true;
        }

        //--------------------------------------------------------------------
        private bool ZatwierdzEdycjeZleceniodawcy()
        //--------------------------------------------------------------------
        {
            if (false == SprawdzPoprawnoscDanychZleceniodawcy())
                return false;

            DaneZleceniodawcy zleceniodawca = new DaneZleceniodawcy(textBox2.Text, textBox3.Text, int.Parse(textBox1.Text), comboBox1.Text, textBox6.Text, textBox5.Text, textBox4.Text, emailTextBox.Text,
                nazwaPlatnika.Text, adresPlatnika.Text, nipPlatnika.Text, jestIFJ.Checked);

            if (false == _Zlecenie.EdytujZleceniodawce(ref zleceniodawca))
                return false;

            return true;
        }

        //--------------------------------------------------------------------
        void Close(object sender, EventArgs args)
        //--------------------------------------------------------------------
        {
            if(false == _TrybDodawaniaZlecenia && false == _TrybDodawaniaZleceniodawcy && false == _TrybEdycji)
                ZapiszDane();

            this.Dispose();
        }

        //--------------------------------------------------------------------
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        //--------------------------------------------------------------------
        {
            dateTimePicker2.Value = dateTimePicker1.Value.AddDays(14);
        }

        //--------------------------------------------------------------------
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        //--------------------------------------------------------------------
        {
            if (dataGridView1.Rows.Count == 0)
                return;

            int rowIndex = e.RowIndex;
            int columnIndex = e.ColumnIndex;

            if (columnIndex != 4 || dataGridView1.Rows[rowIndex].Cells[0].Value == null)
                return;

            KartaPrzyjeciaForm okno = new KartaPrzyjeciaForm(true);
            okno.WyswietlDane((int)dataGridView1.Rows[rowIndex].Cells[0].Value);
            okno.ShowDialog();
        }

        private void podglądToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MeldunekModel model = new MeldunekModel();
            model.adresZleceniodawcy = textBox2.Text;
            model.nip = (nipPlatnika.Text.Trim() != "" && innyPlatnik.Checked) ? nipPlatnika.Text : textBox6.Text;
            model.nazwaPlatnika = nazwaPlatnika.Text;
            model.adresPlatnika = adresPlatnika.Text;
            model.innyPlatnik = innyPlatnik.Checked;
            
            for (int i = 0; i < dataGridView1.Rows.Count; ++i)
            {
                model.nrKart.Add(dataGridView1[0, i].Value.ToString());
            }
            model.zleceniodawca = comboBox1.Text;

            string path = new DocumentationPathsLoader().GetPath("MeldunekWynik") + numericUpDown1.Value + "Meldunek.html";
            WydrukiMeldunek wydrukiMeldunek = new WydrukiMeldunek(model);
            if(!wydrukiMeldunek.generateDocument(path))
            {
                MessageBox.Show("Sprawdź czy zlecenie zostało na pewno wykonane.", "Uwaga");
            }
        }

        private void innyPlatnik_CheckedChanged(object sender, EventArgs e)
        {
            grupaPlatnika.Visible = innyPlatnik.Checked;
        }


    }
}
