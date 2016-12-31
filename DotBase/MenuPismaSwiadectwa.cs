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
                _NrPisma = (uint)_Baza.TworzTabeleDanych("SELECT MAX(nr_pisma) FROM Karta_przyjecia WHERE rok=Year(Date())").Rows[0].Field<int>(0) + 1;
            }
            
            try
            {
                DataTable table = _Baza.TworzTabeleDanych("SELECT Data_wystawienia, Data_wykonania, Autoryzowal, Uwaga, Waznosc_dwa_lata, Poprawa " +
                String.Format("FROM Swiadectwo WHERE id_karty = {0}", _NumerKarty));

                dataWystawienia.Value = table.Rows[0].Field<DateTime>("Data_wystawienia");
                textBox4.Text = table.Rows[0].Field<String>("Autoryzowal");
                textBox1.Text = table.Rows[0].Field<String>("Uwaga");
                checkBox1.Checked = table.Rows[0].Field<Boolean>("Waznosc_dwa_lata");
                checkBox2.Checked = table.Rows[0].Field<Boolean>("Poprawa");
                dataWykonania.Value = table.Rows[0].Field<DateTime>("Data_wykonania");
            }
            catch (Exception)
            {}

                textBox2.Text = _NrPisma.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sciezka = _DocumentationPathsLoader.GetPath("SwiadectwoWynik") + _NumerKarty + "SwiadectwoWynik.html";

            Dokumenty.Swiadectwo swiadectwo = new Dokumenty.Swiadectwo(_NumerKarty, 
                                                                       dataWystawienia.Value,
                                                                       dataWykonania.Value,
                                                                       textBox4.Text,
                                                                       checkBox2.Checked.ToString());
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
            int i = textBox1.SelectionStart;

            if (e.Alt && 77 == e.KeyValue)
            {    
                const string stringDoWstawienia = "&mu;";
                int dlugoscWstawienia = stringDoWstawienia.Length;

                textBox1.Text = textBox1.Text.Insert(textBox1.SelectionStart, stringDoWstawienia);
                textBox1.SelectionStart = i + dlugoscWstawienia;
            }
            else if (e.Alt && 84 == e.KeyValue)
            {
                const string stringDoWstawienia = "&nbsp;";
                int dlugoscWstawienia = stringDoWstawienia.Length;

                textBox1.Text = textBox1.Text.Insert(textBox1.SelectionStart, stringDoWstawienia);
                textBox1.SelectionStart = i + dlugoscWstawienia;
            }
            else if (e.Alt && 66 == e.KeyValue)
            {
                i = textBox1.SelectionStart;
                int i2 = i + textBox1.SelectionLength;

                textBox1.Text = textBox1.Text.Insert(i2, "</b>");
                textBox1.Text = textBox1.Text.Insert(i, "<b>");

                textBox1.SelectionStart = i2 + 7;
            }
            else if (e.Alt && 80 == e.KeyValue)
            {
                const string stringDoWstawienia = "<br>";
                int dlugoscWstawienia = stringDoWstawienia.Length;

                textBox1.Text = textBox1.Text.Insert(textBox1.SelectionStart, stringDoWstawienia);
                textBox1.SelectionStart = i + dlugoscWstawienia;
            }
            else if (e.Alt && 68 == e.KeyValue)
            {
                i = textBox1.SelectionStart;
                int i2 = i + textBox1.SelectionLength;

                textBox1.Text = textBox1.Text.Insert(i2, "</sub>");
                textBox1.Text = textBox1.Text.Insert(i, "<sub>");

                textBox1.SelectionStart = i2 + 11;
            }
            else if (e.Alt && 71 == e.KeyValue)
            {
                i = textBox1.SelectionStart;
                int i2 = i + textBox1.SelectionLength;

                textBox1.Text = textBox1.Text.Insert(i2, "</sup>");
                textBox1.Text = textBox1.Text.Insert(i, "<sup>");

                textBox1.SelectionStart = i2 + 11;
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
                _Baza.WykonajPolecenie("INSERT INTO Swiadectwo (id_karty, Data_wystawienia, Data_wykonania, Autoryzowal, Uwaga, Waznosc_dwa_lata, Poprawa) "
                + String.Format("VALUES ({0}, '{1}', '{2}', '{3}', '{4}', {5}, {6})", 
                _NumerKarty, 
                dataWystawienia.Value.ToShortDateString(),
                dataWykonania.Value.ToShortDateString(),
                textBox4.Text, 
                textBox1.Text, 
                checkBox1.Checked,
                checkBox2.Checked));
            }
            else
            {
                _Baza.WykonajPolecenie(String.Format("UPDATE Swiadectwo SET Data_wystawienia='{0}', Data_wykonania='{1}', Autoryzowal='{2}', " +
                "Uwaga='{3}', Waznosc_dwa_lata={4}, Poprawa={5} WHERE id_karty={6}", 
                dataWystawienia.Value.ToShortDateString(),
                dataWykonania.Value.ToShortDateString(),
                textBox4.Text, 
                textBox1.Text, 
                checkBox1.Checked, 
                checkBox2.Checked,
                _NumerKarty));
            }
        }
        
    }
}