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
    public partial class HasloForm : Form
    {
        string aktHaslo = "";

        public HasloForm()
        {
            InitializeComponent();
            uzytkownikLabel.Text = "Użytkownik:";
            nazwaTextBox.Text = LogowanieForm.Instancja.Wybrany.nazwa;
            nazwaTextBox.Visible = true;
            hasloAktTextBox.Enabled = true;
            aktHaslo = LogowanieForm.Instancja.Wybrany.haslo;
        }

        private void hasloTextBox_TextChanged(object sender, EventArgs e)
        {
            sprawdzForm();
        }

        private bool sprawdzForm()
        {
            bool ok = true;
            ok = ok && (hasloTextBox.Text == hasloPowtTextBox.Text);
            ok = ok && poprawneHaslo(hasloTextBox.Text);
            if (hasloAktTextBox.Enabled && aktHaslo.Length > 0)
            {
                ok = ok && aktHaslo == hasloAktTextBox.Text;
            }
            okBtn.Enabled = ok;
            return ok;
        }

        private bool poprawneHaslo(string haslo)
        {
            bool litery = false;
            bool cyfry = false;
            foreach (var c in haslo)
            {
                if (c >= '0' && c <= '9') cyfry = true;
                if (c >= 'a' && c <= 'z') litery = true;
                if (c >= 'A' && c <= 'Z') litery = true;
            }
            return litery && cyfry && haslo.Length >= 1;
        }

        public void zmienUzytkownika(string nazwa)
        {
            uzytkownikLabel.Text = "Użytkownik:";
            nazwaTextBox.Text = nazwa;
            nazwaTextBox.Visible = true;
            hasloTextBox.Text = "";
            hasloPowtTextBox.Text = "";
            hasloAktTextBox.Text = "";
            hasloAktTextBox.Enabled = false;
        }

        private void HasloForm_Shown(object sender, EventArgs e)
        {
            sprawdzForm();
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            if (sprawdzForm())
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            else
            {
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            }
            Close();
        }

        private void anulujBtn_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        public string Haslo
        {
            get { return hasloTextBox.Text; }
        }

        public string Nazwa
        {
            get { return nazwaTextBox.Text; }
        }

        internal void zmienWlasne(string nazwa, string haslo)
        {
            uzytkownikLabel.Text = nazwa;
            nazwaTextBox.Visible = false;
            hasloTextBox.Text = "";
            hasloPowtTextBox.Text = "";
            hasloAktTextBox.Text = "";
            hasloAktTextBox.Enabled = (haslo != null);
            aktHaslo = haslo;
        }

        public string AktHaslo
        {
            get { return hasloAktTextBox.Text; }
        }
    }
}
