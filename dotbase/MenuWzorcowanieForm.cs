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
    public partial class MenuWzorcowanieForm : Form
    {
        private MenuWzorcowanie _Wzorcowanie;
        private DataTable _Dane;
        private bool _TrybInterakcjiZUzytkownikiem;
        enum Stale { AMERYK = 7, CHLOR = 10, STRONT_SLABY = 2, WEGIEL_SLABY = 3, STRONT_SILNY = 8, WEGIEL_SILNY = 9, PLUTON = 17, STRONT_NAJSILNIEJSZY = 18};

        //--------------------------------------------------------------
        public MenuWzorcowanieForm()
        //--------------------------------------------------------------
        {
            InitializeComponent();
            _Wzorcowanie = new MenuWzorcowanie();
            _TrybInterakcjiZUzytkownikiem = false;
            BlokujPrzyciski();
            AktywujPrzyciskiOdpowiednichWzorcowan();
        }

        //--------------------------------------------------------------
        public MenuWzorcowanieForm(int nrKarty)
        //--------------------------------------------------------------
        {
            InitializeComponent();
            _Wzorcowanie = new MenuWzorcowanie();
            _Wzorcowanie.ZnajdzKartePoNumerze(nrKarty);
            _Dane = _Wzorcowanie.Dane;
            WyswietlDane();
            BlokujPrzyciski();
            AktywujPrzyciskiOdpowiednichWzorcowan();
            _TrybInterakcjiZUzytkownikiem = true;
        }

        //-------------------------------------------------------------------
        private void AktywujPrzyciskiOdpowiednichWzorcowan()
        //-------------------------------------------------------------------
        {
            if (false == _Wzorcowanie.ZnajdzWzorcowaniaDlaDanejKarty((int)numericUpDown1.Value))
                return;

            _Dane = _Wzorcowanie.Dane;

            if (null == _Dane)
                return;

            if (_Dane.Rows[0].Field<bool>("Moc_dawki"))
                button1.Enabled = true;
            if (_Dane.Rows[0].Field<bool>("Dawka"))
                button2.Enabled = true;
            if (_Dane.Rows[0].Field<bool>("Syg_mocy_dawki"))
                button3.Enabled = true;
            if (_Dane.Rows[0].Field<bool>("Syg_dawki"))
                button4.Enabled = true;
            if (_Dane.Rows[0].Field<bool>("Stront_slaby"))
                button5.Enabled = true;
            if (_Dane.Rows[0].Field<bool>("Stront_silny"))
                button6.Enabled = true;
            if (_Dane.Rows[0].Field<bool>("Wegiel_slaby"))
                button7.Enabled = true;
            if (_Dane.Rows[0].Field<bool>("Wegiel_silny"))
                button8.Enabled = true;
            if (_Dane.Rows[0].Field<bool>("Chlor"))
                button9.Enabled = true;
            if (_Dane.Rows[0].Field<bool>("Ameryk"))
                button10.Enabled = true;
            if (_Dane.Rows[0].Field<bool>("Pluton"))
                button11.Enabled = true;
            if (_Dane.Rows[0].Field<bool>("Stront_najsilniejszy"))
                button12.Enabled = true;

            button13.Enabled = button14.Enabled = true;
        }

        //-------------------------------------------------------------------
        void AktywujTrybInterakcji(object sender, EventArgs args)
        //-------------------------------------------------------------------
        {
            _TrybInterakcjiZUzytkownikiem = true;
            numericUpDown1.Enabled = true;
        }

        //-------------------------------------------------------------------
        void AktywujTrybInterakcji()
        //-------------------------------------------------------------------
        {
            _TrybInterakcjiZUzytkownikiem = false;
            numericUpDown1.Enabled = false;
        }

        //-------------------------------------------------------------------
        private void BlokujPrzyciski()
        //-------------------------------------------------------------------
        {
            button1.Enabled = button2.Enabled = button3.Enabled = button4.Enabled = button5.Enabled =
            button6.Enabled = button7.Enabled = button8.Enabled = button9.Enabled = button10.Enabled =
            button11.Enabled = button12.Enabled = button13.Enabled = button14.Enabled = false;
        }

        //--------------------------------------------------------------
        public bool Inicjalizuj()
        //--------------------------------------------------------------
        {
            BlokujPrzyciski();
            if (false == _Wzorcowanie.Inicjalizuj())
                return false;

            _Dane = _Wzorcowanie.Dane;

            if (null == _Dane)
                return false;

            WyswietlDane();
            AktywujPrzyciskiOdpowiednichWzorcowan();

            _TrybInterakcjiZUzytkownikiem = true;

            return true;
        }

        //-------------------------------------------------------------------
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        //-------------------------------------------------------------------
        {
            if (_TrybInterakcjiZUzytkownikiem)
            {
                BlokujPrzyciski();

                if (false == _Wzorcowanie.ZnajdzKartePoNumerze((int)numericUpDown1.Value))
                {
                    MyMessageBox.Show("Karta o danym numerze nie istnieje lub połączenie z bazą zostało zerwane.");
                    return;
                }

                _Dane = _Wzorcowanie.Dane;

                if (null == _Dane)
                    return;

                try
                {
                    WyswietlDane();
                }
                catch (Exception)
                {
                    MyMessageBox.Show("Nie można wyświetlić danych dla tego numeru.");
                    _TrybInterakcjiZUzytkownikiem = true;
                    return;
                }

                AktywujPrzyciskiOdpowiednichWzorcowan();
            }
        }

        //--------------------------------------------------------------
        private void WyswietlDane()
        //--------------------------------------------------------------
        {
            _TrybInterakcjiZUzytkownikiem = false;
            numericUpDown1.Value = _Dane.Rows[0].Field<int>("Id_karty");
            textBox1.Text = _Dane.Rows[0].Field<string>("Zleceniodawca");
            textBox3.Text = _Dane.Rows[0].Field<string>("Nr_fabryczny");
            textBox2.Text = _Dane.Rows[0].Field<string>("Typ");
            textBox4.Text = _Dane.Rows[0].Field<int>("Id_zlecenia").ToString();
            _TrybInterakcjiZUzytkownikiem = true;

        }

        //--------------------------------------------------------------
        // Moc dawki
        private void button1_Click(object sender, EventArgs e)
        //--------------------------------------------------------------
        {
            if (_TrybInterakcjiZUzytkownikiem)
            {
                _TrybInterakcjiZUzytkownikiem = false;
                WzorcowanieMocDawkiForm okno = new WzorcowanieMocDawkiForm((int)numericUpDown1.Value);
                okno.FormClosing += AktywujTrybInterakcji;
                okno.Inicjalizuj();
                okno.Show();
            }
        }

        //--------------------------------------------------------------
        // Dawka
        private void button2_Click(object sender, EventArgs e)
        //--------------------------------------------------------------
        {
            if (_TrybInterakcjiZUzytkownikiem)
            {
                _TrybInterakcjiZUzytkownikiem = false;
                WzorcowanieDawkaForm okno = new WzorcowanieDawkaForm((int)numericUpDown1.Value);
                okno.FormClosing += AktywujTrybInterakcji;
                okno.Inicjalizuj();
                okno.Show();
            }

        }

        //--------------------------------------------------------------
        // Sygnalizacja mocy dawki
        private void button3_Click(object sender, EventArgs e)
        //--------------------------------------------------------------
        {
            if (_TrybInterakcjiZUzytkownikiem)
            {
                _TrybInterakcjiZUzytkownikiem = false;
                WzorcowanieSygnalizacjaMocyDawkiForm _WzorcowanieMocDawkiForm = new WzorcowanieSygnalizacjaMocyDawkiForm((int)numericUpDown1.Value);
                _WzorcowanieMocDawkiForm.FormClosing += AktywujTrybInterakcji;
                _WzorcowanieMocDawkiForm.Inicjalizuj();
                _WzorcowanieMocDawkiForm.Show();
            }
        }

        //--------------------------------------------------------------
        // sygnalizacja dawki
        private void button4_Click(object sender, EventArgs e)
        //--------------------------------------------------------------
        {
            if (_TrybInterakcjiZUzytkownikiem)
            {
                _TrybInterakcjiZUzytkownikiem = false;
                WzorcowanieSygnalizacjaDawkiForm _WzorcowanieMocDawkiForm = new WzorcowanieSygnalizacjaDawkiForm((int)numericUpDown1.Value);
                _WzorcowanieMocDawkiForm.FormClosing += AktywujTrybInterakcji;
                _WzorcowanieMocDawkiForm.Inicjalizuj();
                _WzorcowanieMocDawkiForm.Show();
            }
        }

        //--------------------------------------------------------------
        // stront
        private void button5_Click(object sender, EventArgs e)
        //--------------------------------------------------------------
        {
            if (_TrybInterakcjiZUzytkownikiem)
            {
                _TrybInterakcjiZUzytkownikiem = false;
                WzorcowanieZrodlamiPowForm _WzorcowanieStrontemSlabym = new WzorcowanieZrodlamiPowForm((int)numericUpDown1.Value, (int)Stale.STRONT_SLABY);
                _WzorcowanieStrontemSlabym.Text = "Wzorcowanie źródłami powierzchniowymi - Stront Słaby";
                _WzorcowanieStrontemSlabym.FormClosing += AktywujTrybInterakcji;
                _WzorcowanieStrontemSlabym.Show();
            }
        }

        //--------------------------------------------------------------
        // stront silny
        private void button6_Click(object sender, EventArgs e)
        //--------------------------------------------------------------
        {
            if (_TrybInterakcjiZUzytkownikiem)
            {
                _TrybInterakcjiZUzytkownikiem = false;
                WzorcowanieZrodlamiPowForm _WzorcowanieStrontemSilnym = new WzorcowanieZrodlamiPowForm((int)numericUpDown1.Value, (int)Stale.STRONT_SILNY);
                _WzorcowanieStrontemSilnym.Text = "Wzorcowanie źródłami powierzchniowymi - Stront Silny";
                _WzorcowanieStrontemSilnym.FormClosing += AktywujTrybInterakcji;
                _WzorcowanieStrontemSilnym.Show();
            }
        }

        //--------------------------------------------------------------
        // Węgiel
        private void button7_Click(object sender, EventArgs e)
        //--------------------------------------------------------------
        {
            if (_TrybInterakcjiZUzytkownikiem)
            {
                _TrybInterakcjiZUzytkownikiem = false;
                WzorcowanieZrodlamiPowForm _WzorcowanieWeglemSlabym = new WzorcowanieZrodlamiPowForm((int)numericUpDown1.Value, (int)Stale.WEGIEL_SLABY);
                _WzorcowanieWeglemSlabym.Text = "Wzorcowanie źródłami powierzchniowymi - Węgiel Słaby";
                _WzorcowanieWeglemSlabym.FormClosing += AktywujTrybInterakcji;
                _WzorcowanieWeglemSlabym.Show();
            }
        }

        //--------------------------------------------------------------
        // Węgiel silny
        private void button8_Click(object sender, EventArgs e)
        //--------------------------------------------------------------
        {
            if (_TrybInterakcjiZUzytkownikiem)
            {
                _TrybInterakcjiZUzytkownikiem = false;
                WzorcowanieZrodlamiPowForm _WzorcowanieWeglemSilnym = new WzorcowanieZrodlamiPowForm((int)numericUpDown1.Value, (int)Stale.WEGIEL_SILNY);
                _WzorcowanieWeglemSilnym.Text = "Wzorcowanie źródłami powierzchniowymi - Węgiel Silny";
                _WzorcowanieWeglemSilnym.FormClosing += AktywujTrybInterakcji;
                _WzorcowanieWeglemSilnym.Show();
            }
        }

        //--------------------------------------------------------------
        // chlor
        private void button9_Click(object sender, EventArgs e)
        //--------------------------------------------------------------
        {
            if (_TrybInterakcjiZUzytkownikiem)
            {
                _TrybInterakcjiZUzytkownikiem = false;
                WzorcowanieZrodlamiPowForm _WzorcowanieChlorem = new WzorcowanieZrodlamiPowForm((int)numericUpDown1.Value, (int)Stale.CHLOR);
                _WzorcowanieChlorem.Text = "Wzorcowanie źródłami powierzchniowymi - Chlor";
                _WzorcowanieChlorem.FormClosing += AktywujTrybInterakcji;
                _WzorcowanieChlorem.Show();
            }
        }

        //--------------------------------------------------------------
        // ameryk
        private void button10_Click(object sender, EventArgs e)
        //--------------------------------------------------------------
        {
            if (_TrybInterakcjiZUzytkownikiem)
            {
                _TrybInterakcjiZUzytkownikiem = false;
                WzorcowanieZrodlamiPowForm _WzorcowanieAmerykiem = new WzorcowanieZrodlamiPowForm((int)numericUpDown1.Value, (int)Stale.AMERYK);
                _WzorcowanieAmerykiem.Text = "Wzorcowanie źródłami powierzchniowymi - Ameryk";
                _WzorcowanieAmerykiem.FormClosing += AktywujTrybInterakcji;
                _WzorcowanieAmerykiem.Show();
            }
        }

        //--------------------------------------------------------------
        // pluton
        private void button11_Click(object sender, EventArgs e)
        //--------------------------------------------------------------
        {
            if (_TrybInterakcjiZUzytkownikiem)
            {
                _TrybInterakcjiZUzytkownikiem = false;
                WzorcowanieZrodlamiPowForm _WzorcowaniePlutonem = new WzorcowanieZrodlamiPowForm((int)numericUpDown1.Value, (int)Stale.PLUTON);
                _WzorcowaniePlutonem.Text = "Wzorcowanie źródłami powierzchniowymi - Pluton";
                _WzorcowaniePlutonem.FormClosing += AktywujTrybInterakcji;
                _WzorcowaniePlutonem.Show();
            }
        }

        //--------------------------------------------------------------
        private void button13_Click(object sender, EventArgs e)
        //--------------------------------------------------------------
        {
            MenuPismaSwiadectwaForm okno = new MenuPismaSwiadectwaForm((int)numericUpDown1.Value);
            okno.ShowDialog();
        }

        //-------------------------------------------------------------------
        private void button14_Click(object sender, EventArgs e)
        //-------------------------------------------------------------------
        {
            BlokujPrzyciski();
            _Wzorcowanie.ZnajdzOstatniaKarte();
            _Dane = _Wzorcowanie.Dane;
            WyswietlDane();
            AktywujPrzyciskiOdpowiednichWzorcowan();
        }

        //-------------------------------------------------------------------
        // poprzednie wzorcowanie dla danego przyrządu
        private void button15_Click(object sender, EventArgs e)
        //-------------------------------------------------------------------
        {
            if (_Wzorcowanie.ZnajdzPoprzednieWzorcowanieDlaDanegoPrzyrzadu((int)numericUpDown1.Value))
            {
                BlokujPrzyciski();
                _Dane = _Wzorcowanie.Dane;
                WyswietlDane();
                AktywujPrzyciskiOdpowiednichWzorcowan();
            }
            else
                MyMessageBox.Show("Nie istnieje poprzednia karta.");
        }

        //-------------------------------------------------------------------
        // następne wzorcowanie dla danego przyrządu
        private void button16_Click(object sender, EventArgs e)
        //-------------------------------------------------------------------
        {
            if (_Wzorcowanie.ZnajdzNastepneWzorcowanieDlaDanegoPrzyrzadu((int)numericUpDown1.Value))
            {
                BlokujPrzyciski();
                _Dane = _Wzorcowanie.Dane;
                WyswietlDane();
                AktywujPrzyciskiOdpowiednichWzorcowan(); 
            }
            else
                MyMessageBox.Show("Nie istnieje następna karta.");
        }

        //-------------------------------------------------------------------
        // stront najsilniejszy
        private void button12_Click_1(object sender, EventArgs e)
        //-------------------------------------------------------------------
        {
            if (_TrybInterakcjiZUzytkownikiem)
            {
                _TrybInterakcjiZUzytkownikiem = false;
                WzorcowanieZrodlamiPowForm _WzorcowaniePlutonem = new WzorcowanieZrodlamiPowForm((int)numericUpDown1.Value, (int)Stale.STRONT_NAJSILNIEJSZY);
                _WzorcowaniePlutonem.Text = "Wzorcowanie źródłami powierzchniowymi - Stront najsilniejszy";
                _WzorcowaniePlutonem.FormClosing += AktywujTrybInterakcji;
                _WzorcowaniePlutonem.Show();
            }
        }
    }
}
