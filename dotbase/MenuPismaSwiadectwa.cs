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
    public partial class MenuPismaSwiadectwaForm : Form
    {
        int _NumerKarty;
        uint _NrPisma;
        BazaDanychWrapper _Baza;
        DocumentationPathsLoader _DocumentationPathsLoader;

        public MenuPismaSwiadectwaForm(int numerKarty)
        {
            InitializeComponent();
            _NumerKarty = numerKarty;
            _Baza = new BazaDanychWrapper();
            _DocumentationPathsLoader = new DocumentationPathsLoader();

            try
            {
                _NrPisma = (uint)_Baza.TworzTabeleDanych(String.Format("SELECT nr_pisma FROM Karta_przyjecia WHERE id_karty = {0}", _NumerKarty)).Rows[0].Field<int>(0);
                if( 0 == _NrPisma )
                    _NrPisma = (uint)_Baza.TworzTabeleDanych("SELECT MAX(nr_pisma) FROM Karta_przyjecia WHERE rok=Year(Date())").Rows[0].Field<int>(0) + 1;
            }
            catch (Exception)
            {
                try
                {
                    _NrPisma = (uint)_Baza.TworzTabeleDanych("SELECT MAX(nr_pisma) FROM Karta_przyjecia WHERE rok=Year(Date())").Rows[0].Field<int>(0) + 1;
                }
                catch (Exception)
                {
                    _NrPisma = 1;
                }
            }
            
            try
            {
                DataTable table = _Baza.TworzTabeleDanych("SELECT Data_wystawienia, Data_wykonania, Autoryzowal, Uwaga, Waznosc_dwa_lata, Poprawa, UwagaMD, UwagaD, UwagaS, UwagaSMD, UwagaSD " +
                String.Format("FROM Swiadectwo WHERE id_karty = {0}", _NumerKarty));

                dataWystawienia.Value = table.Rows[0].Field<DateTime>("Data_wystawienia");
                textBox4.Text = table.Rows[0].Field<String>("Autoryzowal");
                textBox1.Text = table.Rows[0].Field<String>("Uwaga");
                checkBox1.Checked = table.Rows[0].Field<Boolean>("Waznosc_dwa_lata");
                checkBox2.Checked = table.Rows[0].Field<Boolean>("Poprawa");
                dataWykonania.Value = table.Rows[0].Field<DateTime>("Data_wykonania");
                uwMD.Text = table.Rows[0].Field<String>("UwagaMD");
                uwD.Text = table.Rows[0].Field<String>("UwagaD");
                uwS.Text = table.Rows[0].Field<String>("UwagaS");
                uwSMD.Text = table.Rows[0].Field<String>("UwagaSMD");
                uwSD.Text = table.Rows[0].Field<String>("UwagaSD");
            }
            catch (Exception)
            {}

                textBox2.Text = _NrPisma.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sciezka = _DocumentationPathsLoader.GetPath("SwiadectwoWynik") + _NumerKarty + "SwiadectwoWynik.html";

            DataTable table = _Baza.TworzTabeleDanych("SELECT Data_przyjecia " +
                "FROM Zlecenia INNER JOIN Karta_przyjecia " +
                "ON Zlecenia.ID_zlecenia = Karta_przyjecia.ID_zlecenia " +
                "WHERE Karta_przyjecia.ID_karty=?", _NumerKarty);

            Dokumenty.Swiadectwo swiadectwo = new Dokumenty.Swiadectwo(_NumerKarty, 
                                                                       dataWystawienia.Value,
                                                                       dataWykonania.Value,
                                                                       table.Rows[0].Field<DateTime>("Data_przyjecia"),
                                                                       textBox4.Text,
                                                                       checkBox2.Checked.ToString(),
                                                                       uwMD.Text,
                                                                       uwD.Text,
                                                                       uwS.Text,
                                                                       uwSMD.Text,
                                                                       uwSD.Text);
            if (swiadectwo.UtworzDokument(sciezka))
            {
                System.Diagnostics.Process.Start(sciezka);
            }
            else
            {
                MessageBox.Show("Nie istnieją dane z których można by sporządzić świadectwo.", "Uwaga!");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (false == uint.TryParse(textBox2.Text, out _NrPisma))
            {
                MessageBox.Show("Nie podano numeru pisma! Lub numer pisma nie jest liczbą naturalną!", "Błąd!");
                return;
            }

            _Baza.WykonajPolecenie(String.Format("UPDATE Karta_przyjecia SET nr_Pisma = {0} WHERE id_Karty = {1}", _NrPisma, _NumerKarty));

            string sciezka = _DocumentationPathsLoader.GetPath("PismoPrzewodnieWynik") + _NrPisma + "PismoPrzewodnieWynik" + _NumerKarty + ".html";

			Dokumenty.PismoPrzewodnie pismo = new Dokumenty.PismoPrzewodnie(_NumerKarty, dataWystawienia.Value, dataWykonania.Value, textBox1.Text, textBox2.Text, checkBox1.Checked, checkBox2.Checked);
            if (!pismo.generateDocument(sciezka))
            {
                MessageBox.Show("Nie można stowrzyć dokumentu z powodu braku danych lub ich błędnych wartości.", "Uwaga");
            }
        }

        private void WstawZnakSpecjalny(object sender, KeyEventArgs e)
        {
            TextBox uwTextBox = (TextBox)sender;

            int i = uwTextBox.SelectionStart;

            if (e.Alt && 77 == e.KeyValue)
            {    
                const string stringDoWstawienia = "&mu;";
                int dlugoscWstawienia = stringDoWstawienia.Length;

                uwTextBox.Text = uwTextBox.Text.Insert(uwTextBox.SelectionStart, stringDoWstawienia);
                uwTextBox.SelectionStart = i + dlugoscWstawienia;
            }
            else if (e.Alt && 84 == e.KeyValue)
            {
                const string stringDoWstawienia = "&nbsp;";
                int dlugoscWstawienia = stringDoWstawienia.Length;

                uwTextBox.Text = uwTextBox.Text.Insert(uwTextBox.SelectionStart, stringDoWstawienia);
                uwTextBox.SelectionStart = i + dlugoscWstawienia;
            }
            else if (e.Alt && 66 == e.KeyValue)
            {
                i = uwTextBox.SelectionStart;
                int i2 = i + uwTextBox.SelectionLength;

                uwTextBox.Text = uwTextBox.Text.Insert(i2, "</b>");
                uwTextBox.Text = uwTextBox.Text.Insert(i, "<b>");

                uwTextBox.SelectionStart = i2 + 7;
            }
            else if (e.Alt && 80 == e.KeyValue)
            {
                const string stringDoWstawienia = "<br>";
                int dlugoscWstawienia = stringDoWstawienia.Length;

                uwTextBox.Text = uwTextBox.Text.Insert(uwTextBox.SelectionStart, stringDoWstawienia);
                uwTextBox.SelectionStart = i + dlugoscWstawienia;
            }
            else if (e.Alt && 68 == e.KeyValue)
            {
                i = uwTextBox.SelectionStart;
                int i2 = i + uwTextBox.SelectionLength;

                uwTextBox.Text = uwTextBox.Text.Insert(i2, "</sub>");
                uwTextBox.Text = uwTextBox.Text.Insert(i, "<sub>");

                uwTextBox.SelectionStart = i2 + 11;
            }
            else if (e.Alt && 71 == e.KeyValue)
            {
                i = uwTextBox.SelectionStart;
                int i2 = i + uwTextBox.SelectionLength;

                uwTextBox.Text = uwTextBox.Text.Insert(i2, "</sup>");
                uwTextBox.Text = uwTextBox.Text.Insert(i, "<sup>");

                uwTextBox.SelectionStart = i2 + 11;
            }
        }

        private void ZamykanieOkna(object sender, FormClosingEventArgs e)
        {
            if (textBox4.Text == "" || false == UInt32.TryParse(textBox2.Text, out _NrPisma))
            {
                MessageBox.Show("Błędne dane. Nie można zapisać.", "Uwaga");
                e.Cancel = true;
                return;
            }

            if (0 == _Baza.TworzTabeleDanych(String.Format("SELECT 1 FROM Swiadectwo WHERE id_karty = {0}", _NumerKarty)).Rows.Count)
            {
                _Baza.WykonajPolecenie("INSERT INTO Swiadectwo (id_karty, Data_wystawienia, Data_wykonania, Autoryzowal, Uwaga, Waznosc_dwa_lata, Poprawa, UwagaMD, UwagaD, UwagaS, UwagaSMD, UwagaSD) "
                + String.Format("VALUES ({0}, '{1}', '{2}', '{3}', '{4}', {5}, {6}, '{7}', '{8}', '{9}', '{10}', '{11}')", 
                _NumerKarty, 
                dataWystawienia.Value.ToShortDateString(),
                dataWykonania.Value.ToShortDateString(),
                textBox4.Text, 
                textBox1.Text, 
                checkBox1.Checked,
                checkBox2.Checked,
                uwMD.Text,
                uwD.Text,
                uwS.Text,
                uwSMD.Text,
                uwSD.Text));
            }
            else
            {
                _Baza.WykonajPolecenie(String.Format("UPDATE Swiadectwo SET Data_wystawienia='{0}', Data_wykonania='{1}', Autoryzowal='{2}', " +
                "Uwaga='{3}', Waznosc_dwa_lata={4}, Poprawa={5}, UwagaMD='{6}', UwagaD='{7}', UwagaS='{8}', UwagaSMD='{9}', UwagaSD='{10}' WHERE id_karty={11}", 
                dataWystawienia.Value.ToShortDateString(),
                dataWykonania.Value.ToShortDateString(),
                textBox4.Text, 
                textBox1.Text, 
                checkBox1.Checked, 
                checkBox2.Checked,
                uwMD.Text,
                uwD.Text,
                uwS.Text,
                uwSMD.Text,
                uwSD.Text,
                _NumerKarty));
            }
        }
        
    }
}