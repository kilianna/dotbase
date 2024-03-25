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
    public partial class MenuGlowneForm : Form
    {
        private MenuWyszukiwanieForm _MenuWyszukiwanie;
        private MenuBiuroForm        _MenuBiuro;
        private MenuWzorcowanieForm      _Wzorcowanie;
        private MenuUstawieniaForm       _MenuUstawienia;

        //-------------------------------------------------------------------
        public MenuGlowneForm()
        //-------------------------------------------------------------------
        {
            InitializeComponent();
        }

        //-------------------------------------------------------------------
        private void button1_Click(object sender, EventArgs e)
        //-------------------------------------------------------------------
        {
            button1.Enabled = false;
            _MenuBiuro = new MenuBiuroForm();
            _MenuBiuro.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AktywujPrzycisk);
            _MenuBiuro.Show();
        }

        //-------------------------------------------------------------------
        private void button2_Click(object sender, EventArgs e)
        //-------------------------------------------------------------------
        {
            button2.Enabled = false;
            _Wzorcowanie = new MenuWzorcowanieForm();
            _Wzorcowanie.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AktywujPrzycisk);
            if (_Wzorcowanie.Inicjalizuj())
            {
                _Wzorcowanie.Show();
            }
            else
            {
                MessageBox.Show("Brak odpowiednich danych. Możliwy jest brak połączenia z bazą danych.");
                _Wzorcowanie.Close();   
            }
        }

        //-------------------------------------------------------------------
        private void button3_Click(object sender, EventArgs e)
        //-------------------------------------------------------------------
        {
            button3.Enabled = false;
            _MenuWyszukiwanie = new MenuWyszukiwanieForm();
            _MenuWyszukiwanie.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AktywujPrzycisk);
            _MenuWyszukiwanie.Show();
        }

        //-------------------------------------------------------------------
        private void button4_Click(object sender, EventArgs e)
        //-------------------------------------------------------------------
        {
            button4.Enabled = false;
            _MenuUstawienia = new MenuUstawieniaForm();
            _MenuUstawienia.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AktywujPrzycisk);
            _MenuUstawienia.Show();

        }

        //-------------------------------------------------------------------
        private void button5_Click(object sender, EventArgs e)
        //-------------------------------------------------------------------
        {
            if (_MenuWyszukiwanie != null && _MenuWyszukiwanie.Visible)
                _MenuWyszukiwanie.Close();
            if (_MenuBiuro != null && _MenuBiuro.Visible)
                _MenuBiuro.Close();
            if (_MenuUstawienia != null && _MenuUstawienia.Visible)
                _MenuUstawienia.Close();

            DialogResult = System.Windows.Forms.DialogResult.Retry;
            Close();
        }

        //-------------------------------------------------------------------
        private void AktywujPrzycisk(object sender, EventArgs e)
        //-------------------------------------------------------------------
        {
            if (sender.Equals(_MenuBiuro))
                button1.Enabled = true;
            else if (sender.Equals(_Wzorcowanie))
                button2.Enabled = true;
            else if (sender.Equals(_MenuWyszukiwanie))
                button3.Enabled = true;
            else if (sender.Equals(_MenuUstawienia))
                button4.Enabled = true;
        }

        private void MenuGlowneForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult != System.Windows.Forms.DialogResult.Retry)
            {
                DialogResult = System.Windows.Forms.DialogResult.OK;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            var dat = new Szablony.swiad_wzor();
            dat.nr_karty = "______";
            dat.jezyk = Jezyk.EN;
            dat.Generate(this);
        }

    }
}
