using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DotBase.PrzestrzenKartyPrzyjecia;

namespace DotBase
{
    public partial class KartaPrzyjeciaForm : Form
    {
        bool _bTrybDodawania;
        bool _bTrybPrzegladaniaOrazEdycji;
        KartaPrzyjecia _KartaPrzyjecia;
        PrzyrzadForm _Przyrzad;
        DocumentationPathsLoader _DocumentationPathsLoader = new DocumentationPathsLoader();

        int poprzedieIdKarty;

        //---------------------------------------------------------
        public KartaPrzyjeciaForm(bool trybPrzegladaniaOrazEdycji = false)
        //---------------------------------------------------------
        {
            _bTrybPrzegladaniaOrazEdycji = trybPrzegladaniaOrazEdycji;

            _KartaPrzyjecia = new KartaPrzyjecia();
            _bTrybDodawania = false;

            InitializeComponent();

            comboBox1.KeyUp += new System.Windows.Forms.KeyEventHandler(AktualizujListeTypowDozymetrow);
            comboBox2.KeyUp += new System.Windows.Forms.KeyEventHandler(AktualizujListeNrFabrycznychDlaDozymetru);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(ZamknijOkno);
            
            if (_bTrybPrzegladaniaOrazEdycji)
            {
                AktywujTrybPrzegladaniaOrazEdycji();
            }
            else
            {
                AktywujTrybDodawania();
            }
        }

        //-------------------------------------------------------------
        private void AktywujTrybDodawania()
        //-------------------------------------------------------------
        {
            button2.Enabled = false;
            button6.Enabled = false;
            numericUpDownRok.Value = System.DateTime.Now.Year;
        }

        //-------------------------------------------------------------
        private void AktywujTrybPrzegladaniaOrazEdycji()
        //-------------------------------------------------------------
        {
            button4.Enabled = button5.Enabled = false;
            button6.Enabled = true;
        }

        //-------------------------------------------------------------
        private void AktualizujDane()
        //-------------------------------------------------------------
        {
            if (false == SprawdzPoprawnoscDanych())
            {
                return;
            }

            DaneKartyPrzyjecia dane = new DaneKartyPrzyjecia((int)numericUpDown1.Value, int.Parse(textBox1.Text), (int)numericUpDownRok.Value);
            dane.Przyrzad = new DanePrzyrzadu(comboBox1.Text, comboBox2.Text);
            dane.Wymagania = new WymaganiaKalibracji(checkBox1.Checked, checkBox2.Checked, checkBox3.Checked, checkBox4.Checked,
                                                     checkBox5.Checked, checkBox6.Checked, checkBox7.Checked, checkBox8.Checked,
                                                     checkBox9.Checked, checkBox10.Checked, checkBox11.Checked, checkBox13.Checked);
            dane.DaneDodatkowe = new DaneDodatkowe(textBox2.Text, textBox3.Text, checkBox12.Checked, sprawdzenieCheckBox.Checked);

            _KartaPrzyjecia.AktualizujDane(ref dane);
        }

        //-------------------------------------------------------------
        private void AktualizujListeTypowDozymetrow(object sender = null, EventArgs args = null)
        //-------------------------------------------------------------
        {
            int i = comboBox1.SelectionStart;
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange( _KartaPrzyjecia.GetMozliweTypyDozymetrow(comboBox1.Text) );
            comboBox1.SelectionStart = i;
            
        }

        //-------------------------------------------------------------
        private void AktualizujListeNrFabrycznychDlaDozymetru(object sender, EventArgs args)
        //-------------------------------------------------------------
        {
            int i = comboBox2.SelectionStart;
            comboBox2.Items.Clear();
            comboBox2.Items.AddRange(_KartaPrzyjecia.GetMozliweNrFabryczneDlaDozymetru(comboBox2.Text));
            comboBox2.SelectionStart = i;
        }


        //-------------------------------------------------------------
        // W trybie dodawania część opcji nie jest dostępna.
        private void BlokujPrzyciski(bool trybDodawania)
        //-------------------------------------------------------------
        {
            if (trybDodawania)
            {
                button1.Enabled = button3.Enabled = false;
                numericUpDown1.Enabled = false;
                textBox1.Enabled = false;
            }
            else
            {
                button4.Enabled = false;
            }
        }

        //---------------------------------------------------------
        private void button1_Click(object sender, EventArgs e)
        //---------------------------------------------------------
        {
            string SCIEZKA_DO_ZAPISU = _DocumentationPathsLoader.GetPath("KartaPrzyjeciaWynik") + numericUpDown1.Value.ToString() + "KartaPrzyjecia.html";

            Dokumenty.KartaPrzyjecia protokol = new Dokumenty.KartaPrzyjecia(numericUpDown1.Value.ToString());
            protokol.generateDocument(SCIEZKA_DO_ZAPISU);
        }

        //---------------------------------------------------------
        // Cennik
        private void button3_Click(object sender, EventArgs e)
        //---------------------------------------------------------
        {
            CennikForm _Cennik = new CennikForm();
            
            if (_Cennik.Inicjalizuj((int)numericUpDown1.Value, checkBox1.Checked, checkBox3.Checked, checkBox9.Checked, checkBox8.Checked, checkBox2.Checked, checkBox6.Checked, checkBox7.Checked, checkBox11.Checked, checkBox10.Checked, checkBox4.Checked, checkBox5.Checked, checkBox13.Checked))
            {
                //_Cennik.LiczSume(); <-- To źle liczy sumę
                _Cennik.ShowDialog();
            }
            else
                MessageBox.Show("Brak danych do obliczeń. Zapewne połączenie z bazą zostało przerwane.");
        }

        //-------------------------------------------------------------
        // Zatwierdzenie dodania
        private void button4_Click(object sender, EventArgs e)
        //-------------------------------------------------------------
        {
            if (false == SprawdzPoprawnoscDanych())
            {
                MessageBox.Show("Nie podano wszystkich potrzebnych informacji, aby dodać kartę.", "Uwaga");
                return;
            }
            if (false == SprawdzIstnieniePrzyrzadu())
            {
                MessageBox.Show("Wybrany przyrząd nie istnieje!", "Uwaga");
                return;
            }

            button4.Enabled = false;

            DaneKartyPrzyjecia dane = PrzygotujDaneDoZapisu();
            dane.IdKarty = (int)numericUpDown1.Value;

            _KartaPrzyjecia.ZapiszKarte(ref dane);
            Close();
        }

        //-------------------------------------------------------------
        private DaneKartyPrzyjecia PrzygotujDaneDoZapisu()
        //-------------------------------------------------------------
        {
            DaneKartyPrzyjecia dane;

            try
            {
                dane = new DaneKartyPrzyjecia(poprzedieIdKarty, int.Parse(textBox1.Text), (int)numericUpDownRok.Value);
            }
            catch (Exception)
            {
                return null;
            }

            dane.Przyrzad = new DanePrzyrzadu(comboBox1.Text, comboBox2.Text);
            dane.Wymagania = new WymaganiaKalibracji(checkBox1.Checked, checkBox3.Checked, checkBox9.Checked, checkBox8.Checked,
                                                     checkBox2.Checked, checkBox6.Checked, checkBox7.Checked, checkBox11.Checked,
                                                     checkBox10.Checked, checkBox4.Checked, checkBox5.Checked, checkBox13.Checked);
            dane.DaneDodatkowe = new DaneDodatkowe(textBox2.Text, textBox3.Text, checkBox12.Checked, sprawdzenieCheckBox.Checked);
            dane.Wykonano = checkBox14.Checked;
            return dane;
        }

        //-------------------------------------------------------------
        // lista dozymetrów
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        //-------------------------------------------------------------
        {
            comboBox2.Text = "";
            comboBox2.Items.Clear();

            _KartaPrzyjecia.ZnajdzWszystkieNrFabryczneDlaSondy(comboBox1.Text);

            comboBox2.Items.AddRange(_KartaPrzyjecia.NrFabryczne.ToArray());

            WyswietlSondyDanegoPrzyrzadu();

            int nrPoprzedniegoWzorcowania = _KartaPrzyjecia.ZnajdzNrPoprzedniejKalibracji(comboBox1.Text, comboBox2.Text);
            if (nrPoprzedniegoWzorcowania != 0)
                textBox3.Text = "Numer poprzedniego świadectwa: " + nrPoprzedniegoWzorcowania.ToString() + "/" + _KartaPrzyjecia.PobierzRok(nrPoprzedniegoWzorcowania);
        }

        //-------------------------------------------------------------
        // lista numerów fabrycznych
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        //-------------------------------------------------------------
        {
            if (0 == String.Empty.CompareTo(comboBox1.Text) || 0 == String.Empty.CompareTo(comboBox2.Text))
                return;

            dataGridView1.RowCount = 0;

            if(_KartaPrzyjecia.ZnajdzSondy(comboBox1.Text, comboBox2.Text))
            {
                WyswietlSondyDanegoPrzyrzadu();    
            }

            int nrPoprzedniegoWzorcowania = _KartaPrzyjecia.ZnajdzNrPoprzedniejKalibracji(comboBox1.Text, comboBox2.Text);
            if (nrPoprzedniegoWzorcowania != 0)
                textBox3.Text = "Numer poprzedniego świadectwa: " + nrPoprzedniegoWzorcowania.ToString() + "/" + _KartaPrzyjecia.PobierzRok(nrPoprzedniegoWzorcowania);
        }

        //---------------------------------------------------------
        // możliwość dodawania nowych kart/przyrządów
        public bool InicjalizujTrybPelnegoDostepu(int nrZlecenia)
        //---------------------------------------------------------
        {
            _bTrybDodawania = true;

            if (true == _KartaPrzyjecia.UtworzNowaKartePrzyjecia(nrZlecenia) )
            {
                numericUpDown1.Value = _KartaPrzyjecia.DaneKartyPrzyjecia.IdKarty;
                textBox1.Text = _KartaPrzyjecia.DaneKartyPrzyjecia.NrZlecenia.ToString();
                BlokujPrzyciski(true);
                return true;
            }

            return false;
        }

        //---------------------------------------------------------
        // możliwość przeglądania i poprawy już zapisanych danych
        public bool InicjalizujTrybPrzegladaniaOrazEdycji()
        //---------------------------------------------------------
        {
            if (true == _KartaPrzyjecia.PobierzDanePoczatkowe())
            {
                BlokujPrzyciski(false);
                return true;
            }

            return false;
        }
        
        //---------------------------------------------------------
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        //---------------------------------------------------------
        {
            if (_bTrybDodawania)
                return;

            DaneKartyPrzyjecia dane = PrzygotujDaneDoZapisu();

            if(_bTrybPrzegladaniaOrazEdycji)
                _KartaPrzyjecia.NadpiszKarte(ref dane);

            _KartaPrzyjecia.DaneKartyPrzyjecia.IdKarty = (int)numericUpDown1.Value;
            WyswietlDane(_KartaPrzyjecia.DaneKartyPrzyjecia.IdKarty);
        }

        //-------------------------------------------------------------
        private bool SprawdzIstnieniePrzyrzadu()
        //-------------------------------------------------------------
        {
            return _KartaPrzyjecia.SprawdzIstnieniePrzyrzadu(comboBox1.Text, comboBox2.Text);
        }

        //-------------------------------------------------------------
        private bool SprawdzPoprawnoscDanych()
        //-------------------------------------------------------------
        {
            if (0 == String.Empty.CompareTo(comboBox1.Text) || 0 == String.Empty.CompareTo(comboBox2.Text))
                return false;

            return checkBox1.Checked == true ||
                    checkBox2.Checked == true ||
                    checkBox3.Checked == true ||
                    checkBox4.Checked == true ||
                    checkBox5.Checked == true ||
                    checkBox6.Checked == true ||
                    checkBox7.Checked == true ||
                    checkBox8.Checked == true ||
                    checkBox9.Checked == true ||
                    checkBox10.Checked == true ||
                    checkBox11.Checked == true ||
                    checkBox12.Checked == true ||
                    checkBox13.Checked == true ||
                    sprawdzenieCheckBox.Checked == true;
        }

        //---------------------------------------------------------
        public void WyswietlDanePoczatkowe()
        //---------------------------------------------------------
        {
            dataGridView1.Rows.Clear();

            if (false == _KartaPrzyjecia.PobierzDanePoczatkowe())
            {
                MessageBox.Show("Brak danych do wyświetlenia.");
                return;
            }

            numericUpDown1.ValueChanged -= numericUpDown1_ValueChanged;
            
            DaneKartyPrzyjecia dane = _KartaPrzyjecia.DaneKartyPrzyjecia;

            numericUpDown1.Value = poprzedieIdKarty = dane.IdKarty;
            numericUpDownRok.Value = dane.rok;

            WyswietlDanePrzyrzadu();
            WyswietlSondyDanegoPrzyrzadu();
            WyswietlDodatkoweInformacje();
            WyswietlWymaganiaDotyczaceKalibracji();

            numericUpDown1.ValueChanged += numericUpDown1_ValueChanged;
        }

        //--------------------------------------------------------------------
        private void OdblokujPrzyciski()
        //--------------------------------------------------------------------
        {
            button1.Enabled = button3.Enabled = true;

            tabControl1.Enabled = true;
        }

        //--------------------------------------------------------------------
        private void ZablokujPrzyciski()
        //--------------------------------------------------------------------
        {
            button1.Enabled = button3.Enabled = false;

            tabControl1.Enabled = false;
        }

        //---------------------------------------------------------
        public void WyswietlDane(int idKart)
        //---------------------------------------------------------
        {
            dataGridView1.Rows.Clear();
            OdblokujPrzyciski();

            if (false == _KartaPrzyjecia.PobierzDane(idKart))
            {
                ZablokujPrzyciski();
                MessageBox.Show("Brak danych do wyświetlenia.");
                return;
            }

            numericUpDown1.ValueChanged -= numericUpDown1_ValueChanged;
            comboBox1.KeyUp -= AktualizujListeTypowDozymetrow;
       

            DaneKartyPrzyjecia dane = _KartaPrzyjecia.DaneKartyPrzyjecia;

            numericUpDown1.Value = poprzedieIdKarty = dane.IdKarty;
            numericUpDownRok.Value = dane.rok;
            numericUpDownRok.Value = dane.rok;
            
            WyswietlDanePrzyrzadu();
            WyswietlSondyDanegoPrzyrzadu();
            WyswietlDodatkoweInformacje();
            WyswietlWymaganiaDotyczaceKalibracji();

            textBox3.Text = dane.DaneDodatkowe.Uwagi;
       
            numericUpDown1.ValueChanged += numericUpDown1_ValueChanged;
            comboBox1.KeyUp += AktualizujListeTypowDozymetrow;
            checkBox14.Checked = dane.Wykonano;
        }

        //---------------------------------------------------------
        public void WyswietlDanePrzyrzadu()
        //---------------------------------------------------------
        {
            textBox1.Text = _KartaPrzyjecia.DaneKartyPrzyjecia.NrZlecenia.ToString();
            comboBox1.Text = _KartaPrzyjecia.Przyrzad.Typ;
            comboBox2.Text = _KartaPrzyjecia.Przyrzad.NrFabryczny;
        }

        //-------------------------------------------------------------
        void WyswietlDodatkoweInformacje()
        //-------------------------------------------------------------
        {
            DaneKartyPrzyjecia dane = _KartaPrzyjecia.DaneKartyPrzyjecia;

            textBox2.Text = dane.DaneDodatkowe.Akcesoria;
            textBox3.Text = dane.DaneDodatkowe.Uwagi;
            checkBox12.Checked = dane.DaneDodatkowe.Uszkodzony;
            sprawdzenieCheckBox.Checked = dane.DaneDodatkowe.Sprawdzenie;
        }

        //-------------------------------------------------------------
        void WyswietlSondyDanegoPrzyrzadu()
        //-------------------------------------------------------------
        {
            if (comboBox1.Text == "" || comboBox2.Text == "")
                return;

            if (false == _KartaPrzyjecia.ZnajdzSondy(comboBox1.Text, comboBox2.Text))
                return;

            DaneKartyPrzyjecia dane = _KartaPrzyjecia.DaneKartyPrzyjecia;

            foreach (Narzedzia.Sonda sonda in dane.Przyrzad.ListaSond.Lista)
            {
                dataGridView1.Rows.Add(sonda.Typ, sonda.NrFabryczny);
            }
        }

        //-------------------------------------------------------------
        void WyswietlWymaganiaDotyczaceKalibracji()
        //-------------------------------------------------------------
        {
            checkBox1.Checked = _KartaPrzyjecia.DaneKartyPrzyjecia.Wymagania.dane[(int)WymaganiaKalibracji.Stale.AMERYK];
            checkBox2.Checked = _KartaPrzyjecia.DaneKartyPrzyjecia.Wymagania.dane[(int)WymaganiaKalibracji.Stale.PLUTON];
            checkBox3.Checked = _KartaPrzyjecia.DaneKartyPrzyjecia.Wymagania.dane[(int)WymaganiaKalibracji.Stale.CHLOR];
            checkBox4.Checked = _KartaPrzyjecia.DaneKartyPrzyjecia.Wymagania.dane[(int)WymaganiaKalibracji.Stale.WEGIEL_SLABY];
            checkBox5.Checked = _KartaPrzyjecia.DaneKartyPrzyjecia.Wymagania.dane[(int)WymaganiaKalibracji.Stale.WEGIEL_SILNY];
            checkBox6.Checked = _KartaPrzyjecia.DaneKartyPrzyjecia.Wymagania.dane[(int)WymaganiaKalibracji.Stale.STRONT_SLABY];
            checkBox7.Checked = _KartaPrzyjecia.DaneKartyPrzyjecia.Wymagania.dane[(int)WymaganiaKalibracji.Stale.STRONT_SILNY];
            checkBox8.Checked = _KartaPrzyjecia.DaneKartyPrzyjecia.Wymagania.dane[(int)WymaganiaKalibracji.Stale.MOC_DAWKI];
            checkBox9.Checked = _KartaPrzyjecia.DaneKartyPrzyjecia.Wymagania.dane[(int)WymaganiaKalibracji.Stale.DAWKA];
            checkBox10.Checked = _KartaPrzyjecia.DaneKartyPrzyjecia.Wymagania.dane[(int)WymaganiaKalibracji.Stale.SYGNALIZACJA_MOCY_DAWKI];
            checkBox11.Checked = _KartaPrzyjecia.DaneKartyPrzyjecia.Wymagania.dane[(int)WymaganiaKalibracji.Stale.SYGNALIZACJA_DAWKI];
            checkBox13.Checked = _KartaPrzyjecia.DaneKartyPrzyjecia.Wymagania.dane[(int)WymaganiaKalibracji.Stale.STRONT_NAJSILNIEJSZY];
        }

        //-------------------------------------------------------------
        private void ZatwierdzDodanieKarty()
        //-------------------------------------------------------------
        {
            DaneKartyPrzyjecia dane = _KartaPrzyjecia.DaneKartyPrzyjecia;

            checkBox1.Checked = dane.Wymagania.dane[(int)WymaganiaKalibracji.Stale.AMERYK];
            checkBox2.Checked = dane.Wymagania.dane[(int)WymaganiaKalibracji.Stale.PLUTON];
            checkBox3.Checked = dane.Wymagania.dane[(int)WymaganiaKalibracji.Stale.CHLOR];
            checkBox4.Checked = dane.Wymagania.dane[(int)WymaganiaKalibracji.Stale.WEGIEL_SLABY];
            checkBox5.Checked = dane.Wymagania.dane[(int)WymaganiaKalibracji.Stale.WEGIEL_SILNY];
            checkBox6.Checked = dane.Wymagania.dane[(int)WymaganiaKalibracji.Stale.STRONT_SLABY];
            checkBox7.Checked = dane.Wymagania.dane[(int)WymaganiaKalibracji.Stale.STRONT_SILNY];
            checkBox8.Checked = dane.Wymagania.dane[(int)WymaganiaKalibracji.Stale.MOC_DAWKI];
            checkBox9.Checked = dane.Wymagania.dane[(int)WymaganiaKalibracji.Stale.DAWKA];
            checkBox10.Checked = dane.Wymagania.dane[(int)WymaganiaKalibracji.Stale.SYGNALIZACJA_MOCY_DAWKI];
            checkBox11.Checked = dane.Wymagania.dane[(int)WymaganiaKalibracji.Stale.SYGNALIZACJA_DAWKI];
            checkBox13.Checked = dane.Wymagania.dane[(int)WymaganiaKalibracji.Stale.STRONT_NAJSILNIEJSZY];
        }

        //-------------------------------------------------------------
        // Dodaj przyrząd
        private void button5_Click(object sender, EventArgs e)
        //-------------------------------------------------------------
        {
            _Przyrzad = new PrzyrzadForm((int)numericUpDown1.Value);
            _Przyrzad.ShowDialog();
            
            _KartaPrzyjecia.ZnajdzWszystkieTypyDozymetrow();
            AktualizujListeTypowDozymetrow();

            int idDozymetru = _KartaPrzyjecia.ZnajdzMaxIdDozymetru();

            int index = comboBox1.Items.IndexOf(_KartaPrzyjecia.ZnajdzTyp(idDozymetru));

            if (index >= 0)
                comboBox1.SelectedIndex = index;

            index = comboBox2.Items.IndexOf(_KartaPrzyjecia.ZnajdzNrFabryczny(idDozymetru));

            if (index >= 0)
                comboBox2.SelectedIndex = index;
        }

        //-------------------------------------------------------------
        // Edytuj przyrząd
        private void button6_Click(object sender, EventArgs e)
        //-------------------------------------------------------------
        {
            if (SprawdzPoprawnoscDanych())
            {
                int idDozymetru = _KartaPrzyjecia.ZnajdzIdDozymetru(comboBox1.Text, comboBox2.Text);

                _Przyrzad = new PrzyrzadForm((int)numericUpDown1.Value, comboBox1.Text, comboBox2.Text);
                _Przyrzad.ShowDialog();

                comboBox1.Items.Clear();
                comboBox1.Text = "";

                comboBox2.Items.Clear();
                comboBox2.Text = "";

                _KartaPrzyjecia.ZnajdzWszystkieTypyDozymetrow();
                AktualizujListeTypowDozymetrow();

                int index = comboBox1.Items.IndexOf(_KartaPrzyjecia.ZnajdzTyp(idDozymetru));
                
                if(index >= 0)
                    comboBox1.SelectedIndex = index;

                index = comboBox2.Items.IndexOf(_KartaPrzyjecia.ZnajdzNrFabryczny(idDozymetru));

                if (index >= 0)
                    comboBox2.SelectedIndex = index;
            }
            else
            {
                MessageBox.Show("Nie wybrano przyrządu który możnaby edytować.", "Uwaga");
            }
        }

        //-------------------------------------------------------------
        private void ZamknijOkno(Object sender, EventArgs args)
        //-------------------------------------------------------------
        {
            if(_bTrybPrzegladaniaOrazEdycji)
            {
                DaneKartyPrzyjecia dane = PrzygotujDaneDoZapisu();
                _KartaPrzyjecia.NadpiszKarte(ref dane);
            }
        }

        //-------------------------------------------------------------
        private void button2_Click(object sender, EventArgs e)
        //-------------------------------------------------------------
        {
            MenuWzorcowanieForm okno = new MenuWzorcowanieForm((int)numericUpDown1.Value);

            okno.ShowDialog();
        }
    }
}
