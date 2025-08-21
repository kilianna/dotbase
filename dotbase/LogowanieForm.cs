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
        public static bool _noScreeshot;

        private Logger log = Log.create();

        public static LogowanieForm Instancja { private set; get; }
        public UsersManager Users { get; private set; }

        private string[] info = new string[] { "", "", "" };
        private string komunikatBazy = null;
        private string komunikatUzytkownika = null;

        //----------------------------------------------------------------------------------
        public LogowanieForm()
        //----------------------------------------------------------------------------------
        {
            Instancja = this;
            Users = new UsersManager();
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
            log("LogIn");

            if (!sprawdzUzytkownika())
            {
                log("Wrong user");
                return;
            }

            hasloTextBox.Text = "";

            EncryptedLogger.SetPassword(Users.DatabasePassword);

            try
            {
                log("Connecting to {0}", Path.GetFullPath(bazaTextBox.Text));
                BazaDanychWrapper.Polacz(Path.GetFullPath(bazaTextBox.Text), Users.DatabasePassword);
            }
            catch (Exception ex)
            {
                log("Connection failed: {0}", ex.ToString());
                MyMessageBox.Show(this, "Nie można połączyć się z bazą danych.\r\nSprawdź, czy z bazą jest skojarzony poprawny plik użytkowników.\r\n" + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            log("Close button");
            Close();
        }

        private void MenuGlowneForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            log("Closing");
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
                log("Reading users from '{0}'", usersPath);
                bool newVersion = Users.Read(usersPath);
                if (!newVersion) {
                    log("Old users file in '{0}'. Overriding with new version.", usersPath);
                    Users.Write(usersPath);
                }
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

        private void spawdzBaze()
        {
            uzytkownikTextBox.BackColor = SystemColors.Window;
            hasloTextBox.BackColor = SystemColors.Window;
            zalogujBtn.Enabled = false;
            administracjaBtn.Visible = false;

            try
            {
                var path = bazaTextBox.Text;
                log("Checking database at {0}", path);
                if (!File.Exists(path)) throw new Exception("Nie można znaleźć podanego pliku bazy danych!");
                var usersPath = Path.ChangeExtension(path, ".usr");
                if (!File.Exists(usersPath)) throw new Exception("Do podanego pliku bazy danych nie ma dołączonego pliku użytkowników!");
                czytajUzytkownikow();
                bazaTextBox.BackColor = SystemColors.Window;
                komunikatBazy = null;
                sprawdzUzytkownika();
            }
            catch (Exception ex)
            {
                log("Checking database failed {0}", ex.ToString());
                bazaTextBox.BackColor = Color.MistyRose;
                komunikatBazy = ex.Message;
            }
            odswiezInfo();
        }


        private bool sprawdzUzytkownika()
        {
            hasloTextBox.BackColor = SystemColors.Window;
            zalogujBtn.Enabled = false;
            administracjaBtn.Visible = false;

            try
            {
                log("Checking user '{0}'", uzytkownikTextBox.Text);
                if (uzytkownikTextBox.Text == "") throw new Exception("Podaj nazwę użytkownika!");
                if (hasloTextBox.Text == "") throw new Exception("Podaj hasło użytkownika!");
                Users.LogIn(uzytkownikTextBox.Text.Trim().ToLower(), hasloTextBox.Text);
                hasloTextBox.BackColor = SystemColors.Window;
                zalogujBtn.Enabled = true;
                administracjaBtn.Visible = Users.CurrentUser.IsAdmin;
                komunikatUzytkownika = null;
                odswiezInfo();
                return true;
            }
            catch (Exception ex)
            {
                log("Checking user failed {0}", ex.ToString());
                hasloTextBox.BackColor = Color.MistyRose;
                if (ex is UsersManager.WrongPassword) {
                    komunikatUzytkownika = "Nieprawidłowe hasło";
                } else if (ex is UsersManager.NotFound) {
                    komunikatUzytkownika = "Nieprawidłowa nazwa użytkownika";
                } else {
                    komunikatUzytkownika = ex.Message;
                }
                odswiezInfo();
                return false;
            }
        }

        private void odswiezInfo()
        {
            infoLabel.Text = komunikatBazy ?? komunikatUzytkownika ?? "";
        }

        private void bazaTextBox_TextChanged(object sender, EventArgs e)
        {
            spawdzBaze();
        }

        private void uzytkownikTextBox_TextChanged(object sender, EventArgs e)
        {
            sprawdzUzytkownika();
        }

        private void hasloTextBox_TextChanged(object sender, EventArgs e)
        {
            sprawdzUzytkownika();
        }

        private void LogowanieForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            log("Saving settings");
            Properties.Settings.Default.Save();
        }

        private bool edycjaOstrzezenie(string pytanie) {
            var res = MyMessageBox.Show(this, 
                String.Format(
                    "Zmiana ustawień użytkowników jest krytyczną czynnością, którą należy przeprowadzać w odpowiedni sposób.\r\n\r\n" +
                    "Zanim to zrobisz upewnij się, że:\r\n" +
                    "1. Utworzyłeś kopię zapasową bazy danych wraz z plikiem użytkowników (z rozszerzeniem '.usr').\r\n" +
                    "2. Otworzyłeś bazę danych na aktualnym komputerze - nie na dysku sieciowym innego komputera.\r\n" +
                    "3. Żaden inny program nie korzysta z tego pliku bazy danych.\r\n\r\n{0}", pytanie),
                    "Informacja", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            return res == System.Windows.Forms.DialogResult.Yes;
        }

        private void administracjaBtn_Click(object sender, EventArgs e)
        {
            adminMenu.Show(administracjaBtn, 0, administracjaBtn.Height);
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
            hasloTextBox.Text = "123";
            LogIn();
#endif
        }

        private void adminMenuItem_Click(object sender, EventArgs e)
        {
            if (sprawdzUzytkownika())
            {
                if (!edycjaOstrzezenie("Czy chcesz kontynuować?")) return;

                log("Administrator form shown");
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
                    if (!edycjaOstrzezenie("Czy chcesz teraz zapisać wszystkie zmiany?")) return;
                    log("Administrator form accepted");
                    if (Users.DatabasePassword != form.Users.DatabasePassword)
                    {
                        log("Changing database password");
                        BazaDanychWrapper.ZmienHaslo(PlikBazy, form.Users.DatabasePassword, Users.DatabasePassword);
                        EncryptedLogger.SetPassword(Users.DatabasePassword);
                    }
                    LogowanieForm.Instancja.Users = form.Users;
                    try
                    {
                        var usersPath = Path.ChangeExtension(bazaTextBox.Text, ".usr");
                        var usersPathNew = Path.ChangeExtension(bazaTextBox.Text, ".new.usr");
                        var usersPathBackup = Path.ChangeExtension(bazaTextBox.Text, ".backup.usr");
                        log("Writing users to '{0}'", usersPathNew);
                        Users.Write(usersPathNew);
                        log("Deleting '{0}'", usersPathBackup);
                        File.Delete(usersPathBackup);
                        log("Moving from '{0}' to '{1}'", usersPath, usersPathBackup);
                        File.Move(usersPath, usersPathBackup);
                        log("Moving from '{0}' to '{1}'", usersPathNew, usersPath);
                        File.Move(usersPathNew, usersPath);
                    }
                    catch (Exception ex)
                    {
                        log("Writing users exception: {0}", ex.ToString());
                        Program.EmergencyExit("Błąd zapisu pliku użytkowników!\r\n" + ex.Message);
                    }
                }
            }
        }

        private void podlądLogówDiagnostycznychToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Users.DatabasePassword == null) return;
            var f = new DecryptLogsForm(Users.DatabasePassword);
            f.ShowDialog(this);
        }
    }
}
