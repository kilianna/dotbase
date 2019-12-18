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
using System.IO;
using System.Security.Cryptography;

namespace DotBase
{
    public partial class LogowanieForm : Form
    {

        private BazaDanychWrapper _BazaDanych;
        public static LogowanieForm Instancja { set; get; }

        private string[] info = new string[] { "", "", "" };

        private Aes aes;
        public Aes Aes { get { return (aes == null) ? (aes = Aes.Create()) : aes; } }
        private RandomNumberGenerator rng;
        public RandomNumberGenerator Rng { get { return (rng == null) ? (rng = RandomNumberGenerator.Create()) : rng; } }

        private byte[] kk = new byte[16] { 134, 42, 156, 192, 244, 11, 93, 192, 167, 238, 250, 46, 132, 38, 87, 16 };

        //----------------------------------------------------------------------------------
        public LogowanieForm()
        //----------------------------------------------------------------------------------
        {
            Instancja = this;
            InitializeComponent();
            wersjaLabel.Text += N.Wersja();
        }

        //----------------------------------------------------------------------------------
        private void button1_Click(object sender, EventArgs e)
        //----------------------------------------------------------------------------------
        {
            LogIn();
        }

        private void LogIn()
        {
            if (!spawdzUzytkownika())
            {
                return;
            }

            hasloTextBox.Text = "";

            _BazaDanych = new BazaDanychWrapper();
            _BazaDanych.TworzConnectionString(Path.GetFullPath(bazaTextBox.Text), hasloBazy);

            try
            {
                BazaDanychWrapper.LastException = null;
                DataTable dane = _BazaDanych.TworzTabeleDanych("SELECT haslo FROM Hasla");
                if (dane == null)
                {
                    if (BazaDanychWrapper.LastException != null)
                    {
                        throw BazaDanychWrapper.LastException;
                    }
                    else
                    {
                        throw new Exception("Nie można wykonać zapytania SQL.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Nie można połączyć się z bazą danych.\r\nSprawdź, czy z bazą jest skojażony poprawny plik użytkowników.\r\n"+ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

#if DEBUG
            _BazaDanych.TworzSzablon();
#endif

            MenuGlowneForm Menu = new MenuGlowneForm();
            Hide();
            var result = Menu.ShowDialog();
            Show();
            Focus();
            BringToFront();
            Focus();
            hasloTextBox.Focus();
#if DEBUG
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                Close();
            }
#endif
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
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
            if (Instancja != null)
            {
                Instancja.timerDoRozlaczania.Start();
            }
        }

        private void wybierzBtn_Click(object sender, EventArgs e)
        {
            bazaFileDialog.FileName = bazaTextBox.Text;
            if (bazaFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                bazaTextBox.Text = bazaFileDialog.FileName;
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void LogowanieForm_Load(object sender, EventArgs e)
        {
            spawdzBaze();
        }

        private string szyfruj(byte[] data)
        {
            byte[] iv = new byte[16];
            Rng.GetBytes(iv);
            var enc = Aes.CreateEncryptor(kk, iv);
            var block = enc.TransformFinalBlock(data, 0, data.Length);
            byte[] all = new byte[iv.Length + block.Length];
            iv.CopyTo(all, 0);
            block.CopyTo(all, iv.Length);
            return Convert.ToBase64String(all);
        }

        private byte[] deszyfruj(string text)
        {
            var all = Convert.FromBase64String(text);
            var iv = new byte[16];
            Array.Copy(all, 0, iv, 0, iv.Length);
            var dec = Aes.CreateDecryptor(kk, iv);
            return dec.TransformFinalBlock(all, iv.Length, all.Length - iv.Length);
        }

        public struct Uzytkownik
        {
            public string nazwa;
            public string haslo;
            public bool admin;
        }

        public Uzytkownik[] uzytkownicy = new Uzytkownik[0];
        public string hasloBazy = "";

        public void zmienHaslo(string nazwa, string haslo)
        {
            for (var i = 0; i < uzytkownicy.Length; i++)
            {
                if (uzytkownicy[i].nazwa.ToLower().Trim() == nazwa.ToLower().Trim())
                {
                    uzytkownicy[i].haslo = haslo;
                    zapiszUzytkownikow();
                    break;
                }
            }
        }

        public void zmienUprawnienia(string nazwa, bool admin)
        {
            for (var i = 0; i < uzytkownicy.Length; i++)
            {
                if (uzytkownicy[i].nazwa.ToLower().Trim() == nazwa.ToLower().Trim())
                {
                    uzytkownicy[i].admin = admin;
                    zapiszUzytkownikow();
                    break;
                }
            }
        }

        private void czytajUzytkownikow()
        {
            try
            {
                var usersPath = Path.ChangeExtension(bazaTextBox.Text, ".usr");
                if (!File.Exists(usersPath)) throw new ApplicationException("Plik użytkowników nie istnieje");
                var enc = File.ReadAllText(usersPath);
                var dec = deszyfruj(enc);
                var s = new MemoryStream(dec);
                var r = new BinaryReader(s);
                int magic = r.ReadInt32();
                if (magic != 0x1F72D1C) throw new ApplicationException("Nieprawidlowy plik użytkowników");
                int liczba = r.ReadInt32();
                if (liczba > dec.Length) throw new ApplicationException("Nieprawidlowy plik użytkowników");
                uzytkownicy = new Uzytkownik[liczba];
                for (int i = 0; i < liczba; i++)
                {
                    uzytkownicy[i].nazwa = r.ReadString();
                    uzytkownicy[i].haslo = r.ReadString();
                    uzytkownicy[i].admin = r.ReadBoolean();
                }
                hasloBazy = r.ReadString();
            }
            catch (Exception ex)
            {
                uzytkownicy = new Uzytkownik[0];
                hasloBazy = "";
                if (!(ex is ApplicationException))
                {
                    ex = new ApplicationException("Błąd czytania pliku użytkowników");
                }
                throw ex;
            }
        }

        public void zapiszUzytkownikow()
        {
            try
            {
                var usersPath = Path.ChangeExtension(bazaTextBox.Text, ".usr");

                var s = new MemoryStream();
                var w = new BinaryWriter(s);

                w.Write((int)0x1F72D1C);
                w.Write(uzytkownicy.Length);
                for (int i = 0; i < uzytkownicy.Length; i++)
                {
                    w.Write(uzytkownicy[i].nazwa);
                    w.Write(uzytkownicy[i].haslo);
                    w.Write(uzytkownicy[i].admin);
                }
                w.Write(hasloBazy);
                var enc = szyfruj(s.ToArray());
                File.WriteAllText(usersPath, enc);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Błąd zapisu pliku użytkowników!\r\n" + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void spawdzBaze()
        {
            uzytkownikTextBox.BackColor = SystemColors.Window;
            hasloTextBox.BackColor = SystemColors.Window;
            zalogujBtn.Enabled = false;
            administracjaBtn.Visible = false;
            Wybrany = new Uzytkownik();

            try
            {
                var path = bazaTextBox.Text;
                if (!File.Exists(path)) throw new Exception("Nie można znaleźć podanego pliku bazy danych!");
                var usersPath = Path.ChangeExtension(path, ".usr");
                if (!File.Exists(usersPath)) throw new Exception("Do podanego pliku bazy danych nie ma dołączonego pliku użytkowników!");
                czytajUzytkownikow();
                bazaTextBox.BackColor = SystemColors.Window;
                odswierzInfo(0, "");
            }
            catch (Exception ex)
            {
                bazaTextBox.BackColor = Color.MistyRose;
                odswierzInfo(0, ex.Message);
                return;
            }

            spawdzUzytkownika();
        }

        public Uzytkownik Wybrany { get; set; }

        private bool spawdzUzytkownika()
        {
            hasloTextBox.BackColor = SystemColors.Window;
            zalogujBtn.Enabled = false;
            administracjaBtn.Visible = false;
            Wybrany = new Uzytkownik();

            try
            {
                if (uzytkownikTextBox.Text == "") throw new Exception("Podaj nazwę użytkownika!");
                foreach (var usr in uzytkownicy)
                {
                    if (usr.nazwa.ToLower().Trim() == uzytkownikTextBox.Text.ToLower().Trim())
                    {
                        Wybrany = usr;
                    }
                }
                if (Wybrany.nazwa == null) throw new Exception("Nie ma takiego użytkownika!");
                uzytkownikTextBox.BackColor = SystemColors.Window;
                odswierzInfo(1, "");
            }
            catch (Exception ex)
            {
                uzytkownikTextBox.BackColor = Color.MistyRose;
                odswierzInfo(1, ex.Message);
                return false;
            }

            try
            {
#if !DEBUG
                if (hasloTextBox.Text == "") throw new Exception("Podaj hasło użytkownika!");
                if (Wybrany.haslo != hasloTextBox.Text) throw new Exception("Nieprawidłowe hasło!");
#endif
                hasloTextBox.BackColor = SystemColors.Window;
                odswierzInfo(2, "");
            }
            catch (Exception ex)
            {
                hasloTextBox.BackColor = Color.MistyRose;
                odswierzInfo(2, ex.Message);
                return false;
            }

            zalogujBtn.Enabled = true;
            administracjaBtn.Visible = Wybrany.admin;
            return true;
        }

        private void odswierzInfo(int index, string text)
        {
            info[index] = text;
            for (int i = 0; i < info.Length; i++)
            {
                if (info[i].Length > 0)
                {
                    infoLabel.Text = info[i];
                    return;
                }
            }
            infoLabel.Text = "";
        }

        private void bazaTextBox_TextChanged(object sender, EventArgs e)
        {
            spawdzBaze();
        }

        private void uzytkownikTextBox_TextChanged(object sender, EventArgs e)
        {
            spawdzUzytkownika();
        }

        private void hasloTextBox_TextChanged(object sender, EventArgs e)
        {
            spawdzUzytkownika();
        }

        private void LogowanieForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Properties.Settings.Default.Save();
        }


        public string PobierzZalogowany()
        {
            return uzytkownikTextBox.Text.ToLower().Trim();
        }

        private void administracjaBtn_Click(object sender, EventArgs e)
        {
            if (spawdzUzytkownika())
            {
                var form = new AdministracjaForm();
                hasloTextBox.Text = "";
                Hide();
                form.ShowDialog(this);
                Show();
                Focus();
                BringToFront();
                Focus();
                hasloTextBox.Focus();
            }
        }

        public string PlikBazy
        {
            get
            {
                return Path.GetFullPath(bazaTextBox.Text);
            }
        }

        private void LogowanieForm_Shown(object sender, EventArgs e)
        {
#if DEBUG
            LogIn();
#endif
        }
    }
}
