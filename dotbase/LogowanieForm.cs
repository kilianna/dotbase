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
using DotBase.Logging;
using DotBase.Login;

namespace DotBase
{
    public partial class LogowanieForm : Form
    {
        private Logger log = Log.create();

        public static LogowanieForm Instancja { private set; get; }
        public UsersManager Users { get; private set; }
        public User Wybrany { get; private set; }

        private string[] info = new string[] { "", "", "" };

        //----------------------------------------------------------------------------------
        public LogowanieForm()
        //----------------------------------------------------------------------------------
        {
            Instancja = this;
            Users = new UsersManager();
            Wybrany = new User();
            InitializeComponent();
            string wersja = N.Wersja();
            if (wersja.StartsWith("!"))
            {
                wersjaLabel.ForeColor = Color.Red;
                wersjaLabel.Text += wersja.Substring(1);
            }
            else
            {
                wersjaLabel.Text += wersja;
            }
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

            EncryptedLogger.SetLocation(
                Path.Combine(Path.GetDirectoryName(Path.GetFullPath(bazaTextBox.Text)), "log"),
                Users.DatabasePassword);

            try
            {
                BazaDanychWrapper.Polacz(Path.GetFullPath(bazaTextBox.Text), Users.DatabasePassword);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Nie można połączyć się z bazą danych.\r\nSprawdź, czy z bazą jest skojażony poprawny plik użytkowników.\r\n" + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

#if DEBUG
            BazaDanychWrapper.TworzSzablon();
#endif

            MenuGlowneForm Menu = new MenuGlowneForm();
            Hide();
            var result = Menu.ShowDialog();
            Show();
            BazaDanychWrapper.Zakoncz();
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
        
        private void MenuGlowneForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            BazaDanychWrapper.Zakoncz();
        }

        private void wybierzBtn_Click(object sender, EventArgs e)
        {
            bazaFileDialog.FileName = bazaTextBox.Text;
            if (bazaFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                bazaTextBox.Text = bazaFileDialog.FileName;
            }
        }

        private void LogowanieForm_Load(object sender, EventArgs e)
        {
            spawdzBaze();
        }

        private void czytajUzytkownikow()
        {
            try
            {
                var usersPath = Path.ChangeExtension(bazaTextBox.Text, ".usr");
                Users.Read(usersPath);
            }
            catch (Exception ex)
            {
                log("Błąd czytania pliku użytkowników: {0}", ex.ToString());
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
                Users.Write(usersPath);
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
            Wybrany = new User();

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


        private bool spawdzUzytkownika()
        {
            hasloTextBox.BackColor = SystemColors.Window;
            zalogujBtn.Enabled = false;
            administracjaBtn.Visible = false;
            Wybrany = new User();

            try
            {
                if (uzytkownikTextBox.Text == "") throw new Exception("Podaj nazwę użytkownika!");
                var user = Users.GetUser(uzytkownikTextBox.Text.ToLower().Trim());
                if (user == null) throw new Exception("Nie ma takiego użytkownika!");
                Wybrany = user;
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
                if (hasloTextBox.Text == "") throw new Exception("Podaj hasło użytkownika!");
                var tempDatabasePassword = Users.LogIn(Wybrany.Name, hasloTextBox.Text);
                if (tempDatabasePassword == null || tempDatabasePassword == "") throw new Exception("Nieprawidłowe hasło!");
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
            administracjaBtn.Visible = Wybrany.IsAdmin;
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

        private void administracjaBtn_Click(object sender, EventArgs e)
        {
            if (spawdzUzytkownika())
            {
                var form = new AdministracjaForm(Users);
                hasloTextBox.Text = "";
                Hide();
                var res = form.ShowDialog(this);
                Show();
                Focus();
                BringToFront();
                Focus();
                hasloTextBox.Focus();
                if (res == System.Windows.Forms.DialogResult.OK)
                {
                    if (Users.DatabasePassword != form.Users.DatabasePassword)
                    {
                        BazaDanychWrapper.ZmienHaslo(LogowanieForm.Instancja.PlikBazy, form.Users.DatabasePassword, Users.DatabasePassword);
                    }
                    LogowanieForm.Instancja.Users = Users;
                    LogowanieForm.Instancja.zapiszUzytkownikow();
                }
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
