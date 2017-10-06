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

        //-------------------------------------------------
        public CennikForm()
        //-------------------------------------------------
        {
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

            _Cennik = new Cennik(idKarty);
            if (_Cennik.Inicjalizuj() && PobierzDaneAutomatycznie(ameryk, chlor, dawka, moc_dawki, pluton, stront_slaby, stront_silny, sygnalizacja_dawki, sygnalizacja_mocy_dawki,wegiel_slaby,wegiel_silny, stront_najsilniejszy))
            {
                _Cennik.LiczSumeAutomatycznie();
                return true;
            }
            else
            {
                return false;
            }
        }

        //-------------------------------------------------
        public void LiczSume()
        //-------------------------------------------------
        {
            textBox1.Text = _Cennik.LiczSumeAutomatycznie().ToString();
        }

        //-------------------------------------------------
        private void LiczSume(object sender, EventArgs args)
        //-------------------------------------------------
        {
            LiczSume2();
        }

        private void LiczSume2()
        {
            textBox1.Text = String.Format("{0:##.00}",
                            _Cennik.LiczSume((int)numericUpDown1.Value,
                                             (int)numericUpDown2.Value,
                                             (int)numericUpDown3.Value,
                                             (int)numericUpDown4.Value,
                                             checkBox1.Checked,
                                             checkBox2.Checked,
                                             checkBox3.Checked,
                                             checkBox4.Checked,
                                             (uint)numericUpDown8.Value,
                                             (double)numericUpDown5.Value,
                                             (double)numericUpDown6.Value));
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

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            LiczSume2();

            if (checkBox4.Checked)
            {
                checkBox1.Enabled = false;
                checkBox2.Enabled = false;
                checkBox3.Enabled = false;
                numericUpDown1.Enabled = false;
                numericUpDown2.Enabled = false;
                numericUpDown3.Enabled = false;
                numericUpDown4.Enabled = false;
                numericUpDown5.Enabled = false;
                numericUpDown8.Enabled = false;
            }
            else
            {
                checkBox1.Enabled = true;
                checkBox2.Enabled = true;
                checkBox3.Enabled = true;
                numericUpDown1.Enabled = true;
                numericUpDown2.Enabled = true;
                numericUpDown3.Enabled = true;
                numericUpDown4.Enabled = true;
                numericUpDown5.Enabled = true;
                numericUpDown8.Enabled = true;
            }

        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            LiczSume2();

            if (checkBox3.Checked)
            {
                checkBox1.Enabled = false;
                checkBox2.Enabled = false;
                checkBox4.Enabled = false;
                numericUpDown1.Enabled = false;
                numericUpDown2.Enabled = false;
                numericUpDown3.Enabled = false;
                numericUpDown4.Enabled = false;
                numericUpDown8.Enabled = false;
            }
            else
            {
                checkBox1.Enabled = true;
                checkBox2.Enabled = true;
                checkBox4.Enabled = true;
                numericUpDown1.Enabled = true;
                numericUpDown2.Enabled = true;
                numericUpDown3.Enabled = true;
                numericUpDown4.Enabled = true;
                numericUpDown8.Enabled = true;
            }
        }

        private void CennikForm_Load(object sender, EventArgs e)
        {

        }
    }
}
