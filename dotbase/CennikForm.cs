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
    public partial class CennikForm : Form
    {
        Cennik _Cennik;
        BazaDanychWrapper _BazaDanych;

        //-------------------------------------------------
        public CennikForm()
        //-------------------------------------------------
        {
            _BazaDanych = new BazaDanychWrapper();
            InitializeComponent();
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(Zapisz);
        }

        //-------------------------------------------------
        public bool Inicjalizuj()
        //-------------------------------------------------
        {
            _Cennik = new Cennik();
            return _Cennik.Inicjalizuj();
        }

        //-------------------------------------------------
        public bool Inicjalizuj(int idKarty, bool ameryk, bool chlor, bool dawka, bool moc_dawki, bool pluton, bool stront_slaby, bool stront_silny,
                                bool sygnalizacja_dawki, bool sygnalizacja_mocy_dawki, bool wegiel_slaby, bool wegiel_silny, bool stront_najsilniejszy)
        //-------------------------------------------------
        {
            numericUpDown1.Enabled = false;
            numericUpDown2.Enabled = false;
            numericUpDown3.Enabled = false;
            numericUpDown4.Enabled = false;

            // ========================= Wyświetlenie ostrzerzenia

            var tab = _BazaDanych.TworzTabeleDanych("SELECT Wartosc, Nazwa FROM Stale WHERE Nazwa=?", "d");
            var d = tab.Rows[0].Field<double>(0);

            bool pokazOstrzerzenie = false;
            tab = _BazaDanych.TworzTabeleDanych("SELECT ID_wzorcowania, Rodzaj_wzorcowania, ID_karty FROM wzorcowanie_cezem WHERE ID_karty=? AND Rodzaj_wzorcowania=?", idKarty, "md");
            for (var i = 0; i < tab.Rows.Count && !pokazOstrzerzenie; i++)
            {
                var tab2 = _BazaDanych.TworzTabeleDanych("SELECT Odleglosc, ID_zrodla, ID_wzorcowania FROM Pomiary_cez WHERE ID_wzorcowania=? AND ID_zrodla=3", tab.Rows[i].Field<int>(0));
                for (var j = 0; j < tab2.Rows.Count && !pokazOstrzerzenie; j++)
                {
                    if (tab2.Rows[j].Field<double>(0) <= d)
                    {
                        pokazOstrzerzenie = true;
                    }
                }
            }

            ostrzerzenieLabel.Visible = pokazOstrzerzenie;


            _Cennik = new Cennik(idKarty);
            if (_Cennik.Inicjalizuj() && PobierzDaneAutomatycznie(ameryk, chlor, dawka, moc_dawki, pluton, stront_slaby, stront_silny, sygnalizacja_dawki, sygnalizacja_mocy_dawki,wegiel_slaby,wegiel_silny, stront_najsilniejszy))
            {
                // ========================= Pole Ekspres

                tab = _BazaDanych.TworzTabeleDanych("SELECT ID_zlecenia, Uszkodzony, Sprawdzenie, ID_karty FROM Karta_przyjecia WHERE ID_karty=?", idKarty);
                var idZlecenia = tab.Rows[0].Field<int>(0);
                var uszkodzony = tab.Rows[0].Field<Boolean>(1);
                var sprawdzenie = tab.Rows[0].Field<Boolean>(2);
                tab = _BazaDanych.TworzTabeleDanych("SELECT Ekspres, ID_zlecenia FROM Zlecenia WHERE ID_zlecenia=?", idZlecenia);
                var ekspres = tab.Rows[0].Field<Boolean>(0);

                checkBox2.Checked = ekspres;
                zepsutyCheckBox.Checked = uszkodzony;
                sprawdzenieCheckBox.Checked = sprawdzenie;

                LiczSume2();
                return true;
            }
            else
            {
                LiczSume2();
                return false;
            }
        }
       
        //-------------------------------------------------
        private void LiczSume(object sender, EventArgs args)
        //-------------------------------------------------
        {
            LiczSume2();
        }

        private void LiczSume2()
        {
            textBox1.Text = String.Format("{0:0.00}",
                            _Cennik.LiczSume((int)numericUpDown1.Value,
                                             (int)numericUpDown2.Value,
                                             (int)numericUpDown3.Value,
                                             (int)numericUpDown4.Value,
                                             wariantRozszerzonyBox.Checked,
                                             wariantTrudnyBox.Checked,
                                             dawkaDlugaBox.Checked,
                                             skazeniaDlugieBox.Checked,
                                             checkBox2.Checked,
                                             sprawdzenieCheckBox.Checked,
                                             zepsutyCheckBox.Checked,
                                             (uint)numericUpDown8.Value,
                                             (double)numericUpDown5.Value,
                                             (double)numericUpDown6.Value));
            zablokujKontrolki();
        }

        //-------------------------------------------------
        private bool PobierzDaneAutomatycznie(bool ameryk, bool chlor, bool dawka, bool moc_dawki, bool pluton, bool stront_slaby, bool stront_silny,
                                              bool sygnalizacja_dawki, bool sygnalizacja_mocy_dawki, bool wegiel_slaby, bool wegiel_silny, bool stront_najsilniejszy)
        //-------------------------------------------------
        {
            if(moc_dawki)
                numericUpDown1.Value = 1;
            else
                numericUpDown1.Value = 0;

            if (dawka)
                numericUpDown3.Value = 1;
            else
                numericUpDown3.Value = 0;

            numericUpDown4.Value = 0;
            
            if(sygnalizacja_dawki)
                ++numericUpDown4.Value;
            if(sygnalizacja_mocy_dawki)
                ++numericUpDown4.Value;

            numericUpDown2.Value = 0;
            if(ameryk)
                ++numericUpDown2.Value;
            if(chlor)
                ++numericUpDown2.Value;
            if(pluton)
                ++numericUpDown2.Value;
            if(stront_slaby)
                ++numericUpDown2.Value;
            if(stront_silny)
                ++numericUpDown2.Value;
            if(wegiel_silny)
                ++numericUpDown2.Value;
            if(wegiel_slaby)
                ++numericUpDown2.Value;
            if (stront_najsilniejszy)
                ++numericUpDown2.Value;

            return true;
        }

        //-------------------------------------------------
        private void Zapisz(object sender, EventArgs args)
        //-------------------------------------------------
        {
            _Cennik.Zapisz(textBox1.Text);
        }

        private void zablokujKontrolki()
        {
            var enabled = !zepsutyCheckBox.Checked && !sprawdzenieCheckBox.Checked;

            var jestMocDawki = (int)numericUpDown1.Value > 0;
            wariantPodstawowyBox.Enabled = enabled && jestMocDawki;
            wariantRozszerzonyBox.Enabled = enabled && jestMocDawki;
            wariantTrudnyBox.Enabled = enabled && jestMocDawki;

            checkBox2.Enabled = enabled;
            sprawdzenieCheckBox.Enabled = enabled;
            dawkaDlugaBox.Enabled = enabled && (int)numericUpDown3.Value > 0;
            skazeniaDlugieBox.Enabled = enabled && (int)numericUpDown2.Value > 0;
            numericUpDown1.Enabled = enabled;
            numericUpDown2.Enabled = enabled;
            numericUpDown3.Enabled = enabled;
            numericUpDown4.Enabled = enabled;
            numericUpDown8.Enabled = enabled && (int)numericUpDown4.Value > 0;

            numericUpDown5.Enabled = !zepsutyCheckBox.Checked;
            sprawdzenieCheckBox.Enabled = !zepsutyCheckBox.Checked;
            zepsutyCheckBox.Enabled = !sprawdzenieCheckBox.Checked;
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            LiczSume2();
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            LiczSume2();
        }

        private void CennikForm_Load(object sender, EventArgs e)
        {
            LiczSume2();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

    }
}
