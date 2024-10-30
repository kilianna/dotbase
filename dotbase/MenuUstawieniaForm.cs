using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using DotBase.Baza;
using System.Data.OleDb;

namespace DotBase
{
    public partial class MenuUstawieniaForm : Form
    {
        //---------------------------------------------------------
        public MenuUstawieniaForm()
        //---------------------------------------------------------
        {
            InitializeComponent();
            
        }

        //---------------------------------------------------------
        // Budzet niepewnosc
        private void button1_Click(object sender, EventArgs e)
        //---------------------------------------------------------
        {
            BudzetNiepewnosciForm okno = new BudzetNiepewnosciForm();

            if (false == okno.Inicjalizuj())
            {
                MyMessageBox.Show("Brak odpowiednich danych w bazie do wyświetlenia.");
            }

            okno.ShowDialog();
        }

        //---------------------------------------------------------
        // protokoły kalibracyjne ławy
        private void button2_Click(object sender, EventArgs e)
        //---------------------------------------------------------
        {
            ProtokolyKalibracyjneForm okno = new ProtokolyKalibracyjneForm();
            
            if (false == okno.Inicjalizuj())
            {
                MyMessageBox.Show("Brak odpowiednich danych w bazie do wyświetlenia.");
            }

            okno.ShowDialog();
        }

        //---------------------------------------------------------
        // wzorcowanie źródeł
        private void button3_Click(object sender, EventArgs e)
        //---------------------------------------------------------
        {
            WzorcowanieZrodelForm okno = new WzorcowanieZrodelForm();
            
            if (false == okno.Inicjalizuj())
            {
                MyMessageBox.Show("Brak odpowiednich danych w bazie do wyświetlenia.");
            }

            okno.ShowDialog();
        }

        //---------------------------------------------------------
        // stałe
        private void button4_Click(object sender, EventArgs e)
        //---------------------------------------------------------
        {
            StaleForm okno = new StaleForm();

            if (false == okno.Inicjalizuj())
            {
                MyMessageBox.Show("Brak odpowiednich danych w bazie do wyświetlenia.");
            }

            okno.ShowDialog();
        }

        //---------------------------------------------------------
        private void button5_Click(object sender, EventArgs e)
        //---------------------------------------------------------
        {
            Close();
        }

        private void hasloBtn_Click(object sender, EventArgs e)
        {
            var form = new HasloForm();
            if (form.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                LogowanieForm.Instancja.Users.ChangeUserPassword(form.Nazwa, form.Haslo);
            }
        }

        private void eksportujBtn_Click(object sender, EventArgs e)
        {
            if (eksportujDlg.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                var form = new HasloForm();
                form.zmienInne("NOWA BAZA DANYCH");
                if (form.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    var usunPersonalne = MessageBox.Show(this, "Czy usunąć dane personalne zleceniodawców?", "Pytanie", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes;
                    if (!BazaDanychWrapper.Eksportuj(LogowanieForm.Instancja.PlikBazy, LogowanieForm.Instancja.Users.DatabasePassword, eksportujDlg.FileName, form.Haslo, usunPersonalne ? new BazaDanychWrapper.TransformujBazeDelegate(UsunDanePersonalne) : null))
                    {
                        MyMessageBox.Show(this, "Nie udało się wyeskportować bazy danych.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        #region Generator Lorem Ipsum do usuwania danych personalnych

        static string[] loremIpsumWords = @"Lorem Ipsum Id Scelerisque Ullamcorper Auctor Ex Cras Tempus Turpis Luctus Maecenas Quis Ornare Semper Lacus Libero Pulvinar Laoreet Et Ligula Hac Varius Non Sodales Sapien Quisque Dictumst Condimentum Dapibus Rhoncus Adipiscing Curabitur Leo Nullam Tincidunt Metus Imperdiet Nisi Eros Diam Lobortis Erat Pharetra Justo Bibendum Purus Nam Eu Feugiat Tristique Fusce Venenatis Ultricies Platea Arcu Vivamus Aliquet In Mattis Vulputate Lacinia Habitasse Porttitor Efficitur At Nibh Nulla Neque Odio Velit Magna Pellentesque Donec Commodo Malesuada Suspendisse Fermentum Eget Etiam Suscipit Amet Dui Interdum Rutrum Porta Volutpat Viverra Ac Vitae Tellus Vel Felis Congue Potenti Iaculis Gravida Massa Maximus Nec Euismod Ante Risus Est Convallis Elementum Orci Praesent Placerat Elit Duis Lectus Blandit Vestibulum Nisl Sed Facilisis Urna Cursus Hendrerit Sit Pretium Sollicitudin Enim Aenean Ut Facilisi Sem".Split(' ');

        private string loremIpsum(int minWords, int maxWords, int id, Random rnd)
        {
            int count = rnd.Next(minWords, maxWords + 1);
            var s = new StringBuilder();
            while (id > 0)
            {
                s.Append(loremIpsumWords[id % loremIpsumWords.Length]);
                s.Append(' ');
                id = id / loremIpsumWords.Length;
                count--;
            }
            if (count > 0 && s.Length > 0) {
                s.Append("Per ");
                count--;
            }
            for (int i = 0; i < count; i++)
            {
                s.Append(loremIpsumWords[rnd.Next(loremIpsumWords.Length)]);
                s.Append(' ');
            }
            return s.ToString().Substring(0, s.Length - 1);
        }

        private void UsunDanePersonalne(ConnectionManager manager)
        {
            var polecenie = manager.command(@"UPDATE Zleceniodawca SET
                Zleceniodawca = ID_zleceniodawcy,
                Adres = ID_zleceniodawcy,
                Osoba_kontaktowa = ID_zleceniodawcy,
                Telefon = ID_zleceniodawcy,
                Faks = ID_zleceniodawcy,
                email = ID_zleceniodawcy,
                Uwagi = ID_zleceniodawcy,
                NIP = ID_zleceniodawcy");
            polecenie.ExecuteNonQuery();
            polecenie.Dispose();
            polecenie = manager.command(@"SELECT ID_zleceniodawcy FROM Zleceniodawca");
            var adapter = new OleDbDataAdapter(polecenie);
            var dane = new DataTable();
            adapter.Fill(dane);
            for(int i = 0; i < dane.Rows.Count; i++)
            {
                int id = dane.Rows[i].Field<int>(0);
                var polecenie2 = manager.command(@"UPDATE Zleceniodawca SET 
                    Zleceniodawca = ?,
                    Adres = ?,
                    Osoba_kontaktowa = ?,
                    Telefon = ?,
                    Faks = ?,
                    email = ?,
                    Uwagi = ?,
                    NIP = ? WHERE ID_zleceniodawcy = ?");
                var rnd = new Random(id);
                addParameter(0, polecenie2, loremIpsum(3, 6, id, rnd));
                addParameter(1, polecenie2, loremIpsum(1, 2, 0, rnd) + " " + rnd.Next(10, 300) + "\r\n"
                    + rnd.Next(10, 99) + "-" + rnd.Next(100, 999) + " " + loremIpsum(1, 2, 0, rnd));
                addParameter(2, polecenie2, loremIpsum(2, 2, 0, rnd));
                addParameter(3, polecenie2, rnd.Next(100000000, 999999999).ToString());
                addParameter(4, polecenie2, rnd.Next(100000000, 999999999).ToString());
                addParameter(5, polecenie2, (loremIpsum(1, 2, 0, rnd) + "@" + loremIpsum(1, 1, 0, rnd) + ".pl").Replace(' ', '.').ToLower());
                addParameter(6, polecenie2, rnd.Next(10) == 0 ? loremIpsum(5, 20, 0, rnd) : "-");
                addParameter(7, polecenie2, rnd.Next(10000000, 99999999).ToString());
                addParameter(8, polecenie2, id);
                polecenie2.ExecuteNonQuery();
                polecenie2.Dispose();
            }
            dane.Dispose();
            adapter.Dispose();
            polecenie.Dispose();
        }

        private void addParameter(int id, OleDbCommand polecenie, string value)
        {
            var p = polecenie.Parameters.Add("a" + id, OleDbType.LongVarWChar, value.Length);
            p.Value = value;
            p.Direction = ParameterDirection.Input;
        }

        private void addParameter(int id, OleDbCommand polecenie, int value)
        {
            var p = polecenie.Parameters.Add("a" + id, OleDbType.Integer);
            p.Value = value;
            p.Direction = ParameterDirection.Input;
        }

        #endregion

    }
}
