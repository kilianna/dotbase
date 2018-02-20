using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Globalization;
using System.Threading;

namespace DotBase
{
    public partial class LogowanieForm : Form
    {
        private Logowanie _Loger;

        private static LogowanieForm instancja = null;

        //----------------------------------------------------------------------------------
        public LogowanieForm()
        //----------------------------------------------------------------------------------
        {
            instancja = this;
            InitializeComponent();
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("pl-PL");
            richTextBox1.Text = Properties.Settings.Default.DataBaseLocation;
        }

        //----------------------------------------------------------------------------------
        private void button1_Click(object sender, EventArgs e)
        //----------------------------------------------------------------------------------
        {
            LogIn();
        }

        private void LogIn()
        {
            _Loger = new Logowanie(richTextBox1.Text, textBox2.Text, textBox3.Text);
            SetLogin();

            if (false == _Loger.LogujDoBazy())
            {
                MessageBox.Show("Nie udało połączyć się bazą danych. Sprawdź hasło, ścieżkę lub połączenie z siecią.", "Błąd");
            }
            else
            {

                MenuGlowneForm Menu = new MenuGlowneForm();
                Hide();
                Menu.AktywujPrzyciskiDlaUzytkwonika(GetUzytkownik());
                Menu.ShowDialog();
                Show();
            }
        }

        //----------------------------------------------------------------------------------
        private MenuGlowneForm.UZYTKOWNIK GetUzytkownik()
        //----------------------------------------------------------------------------------
        {
            if(_Loger.Login == "Kierownik")
                return MenuGlowneForm.UZYTKOWNIK.KIEROWNIK;
            else if(_Loger.Login == "Pracownik Biurowy")
                return MenuGlowneForm.UZYTKOWNIK.PRACOWNIK_BIUROWY;
            else
                return MenuGlowneForm.UZYTKOWNIK.PRACOWNIK_POMIAROWY;
        }

        //----------------------------------------------------------------------------------
        private void SetLogin()
        //----------------------------------------------------------------------------------
        {
            if( _PracownikPomiarowy.Checked )
                _Loger.Login = _PracownikPomiarowy.Text;
            else if( _PracownikBiurowy.Checked )
                _Loger.Login = _PracownikBiurowy.Text;
            else
                _Loger.Login = _Kierownik.Text;
            
        }

        //----------------------------------------------------------------------------------
        private void button2_Click(object sender, EventArgs e)
        //----------------------------------------------------------------------------------
        {
            Properties.Settings.Default.DataBaseLocation = richTextBox1.Text;
            Properties.Settings.Default.Save();
            Close();
        }

        //----------------------------------------------------------------------------------
        private void richTextBox1_Click(object sender, EventArgs e)
        //----------------------------------------------------------------------------------
        {
            if(DialogResult.OK == openFileDialog1.ShowDialog(this))
                richTextBox1.Text = openFileDialog1.FileName;
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keys.Enter == e.KeyCode)
                LogIn();
        }

        private void timerDoRozlaczania_Tick(object sender, EventArgs e)
        {
            if (BazaDanychWrapper.Zakoncz(false))
            {
                timerDoRozlaczania.Stop();
            }
        }

        private void MenuGlowneForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            BazaDanychWrapper.Zakoncz(true);
        }


        public static void AktywujLicznikRozlaczania()
        {
            if (instancja != null)
            {
                instancja.timerDoRozlaczania.Start();
            }
        }
    }
}
