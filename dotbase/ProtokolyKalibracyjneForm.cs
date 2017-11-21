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
    public partial class ProtokolyKalibracyjneForm : Form
    {
        private ProtokolyKalibracyjne _Protokol;
        // zapobiega wywoływaniu zdarzeń zmiany wartości w polach okna przy wpisywaniu danych pobranych z bazy
        private bool _TrybWpisywaniaDanych;
        private bool _TrybDodawaniaDanych;

        //----------------------------------------------------
        public ProtokolyKalibracyjneForm()
        //----------------------------------------------------
        {
            InitializeComponent();
            _Protokol = new ProtokolyKalibracyjne();
            _TrybWpisywaniaDanych = false;
        }

        //----------------------------------------------------
        public void CzyscTabele()
        //----------------------------------------------------
        {
            dataGridView1.Rows.Clear();
        }

        //----------------------------------------------------
        public bool Inicjalizuj()
        //----------------------------------------------------
        {
            if (true == _Protokol.ZnajdzOstatni())
            {
                WyswietlDane();
                return true;
            }

            return false;
        }

        //----------------------------------------------------
        // szukaj po id
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        //----------------------------------------------------
        {
            if (_TrybWpisywaniaDanych || _TrybDodawaniaDanych)
                return;

            button2.Enabled = true;
            CzyscTabele();

            if (true == _Protokol.WyszukajProtokolPoId((int)numericUpDown1.Value))
                WyswietlDane();
            else
            {
                button2.Enabled = false;
                MessageBox.Show("Brak danych dla danego id.");
            }
        }

        //----------------------------------------------------
        // szukaj po dacie
        private void dateTimePicker1_CloseUp(object sender, EventArgs e)
        //----------------------------------------------------
        {
            if (_TrybWpisywaniaDanych || _TrybDodawaniaDanych)
                return;

            button2.Enabled = true;
            CzyscTabele();

            if (true == _Protokol.WyszukajProtokolPoDacie(dateTimePicker1.Value))
                WyswietlDane();
            else
            {
                button2.Enabled = false;
                MessageBox.Show("Brak danych dla danej daty.");
            }

        }

        //----------------------------------------------------
        private void WyswietlDane()
        //----------------------------------------------------
        {
            _TrybWpisywaniaDanych = true;

            numericUpDown1.Value = _Protokol.OdpowiedzBazy.Rows[0].Field<short>(0);
            dateTimePicker1.Value = _Protokol.OdpowiedzBazy.Rows[0].Field<DateTime>(1);

            foreach (DataRow row in _Protokol.OdpowiedzBazy.Rows)
            {
                dataGridView1.Rows.Add(row.Field<short>(2), row.Field<double>(3), row.Field<double>(4), row.Field<double>(5));
            }

            _TrybWpisywaniaDanych = false;
        }

        //----------------------------------------------------
        // rozpoczęcie dodawania lub potwierdzenie dodania
        private void button1_Click(object sender, EventArgs e)
        //----------------------------------------------------
        {
            if (_TrybDodawaniaDanych)
            {
                WylaczTrybDodawania();
                AktualizujLubZapiszDane();
            }
            else
            {
                CzyscTabele();
                WlaczTrybDodawania();
                ++numericUpDown1.Value;
                dateTimePicker1.Value = DateTime.Today;
            }
        }

        //----------------------------------------------------
        // edytuj dane
        private void button2_Click(object sender, EventArgs e)
        //----------------------------------------------------
        {
            if (_TrybDodawaniaDanych)
            {
                dataGridView1.Rows.Clear();

                if (DialogResult.OK == openFileDialog1.ShowDialog())
                {
                    DataTable danePobraneZExcela = PobierzDaneZExcela(openFileDialog1.FileName, "Arkusz1$");

                    if (danePobraneZExcela == null || danePobraneZExcela.Rows.Count == 0 || danePobraneZExcela.Columns.Count != 4)
                    {
                        string wiadomosc =  "Wskazany plik nie mógł zostać przetworzony! ";
                               wiadomosc += "Plik wejściowy Excel'a powinien składać się z 1 wiersza będącego nagłówkiem oraz ";
                               wiadomosc += "pozostałych wierszy danych. Powinien zawierać 4 kolumny. ";
                               wiadomosc += "Dane powinny znajdować się w arkuszu = Arkusz1.";
                               
                        MessageBox.Show(wiadomosc, "Błąd");
                        return;
                    }
                    
                    foreach (DataRow row in danePobraneZExcela.Rows)
                    {
                        dataGridView1.Rows.Add(row.ItemArray);
                    }
                }
            }
            else
            {
                AktualizujLubZapiszDane();
            }
        }

        //----------------------------------------------------
        private void WlaczTrybDodawania()
        //----------------------------------------------------
        {
            _TrybDodawaniaDanych = true;
            numericUpDown1.Enabled = false;
            button1.Text = "Zatwierdź dodanie";
            button2.Text = "Zaimportuj z Excela";
        }

        //----------------------------------------------------
        private void WylaczTrybDodawania()
        //----------------------------------------------------
        {
            _TrybDodawaniaDanych = false;
            numericUpDown1.Enabled = true;
            button1.Text = "Dodaj";
            button2.Text = "Zatwierdź Poprawki";
        }

        //----------------------------------------------------
        private void AktualizujLubZapiszDane()
        //----------------------------------------------------
        {
            if (false == SprawdzMozliwoscAktualizacjiLubDodania())
            {
                MessageBox.Show("Dane nie są poprawne. Akcja nie zostanie podjęta.");
                return;
            }

            List<int> idZrodla = new List<int>();
            List<double> odleglosc = new List<double>();
            List<double> mocKermy = new List<double>();
            List<double> niepewnosc = new List<double>();

            for (int i = 0; i < dataGridView1.Rows.Count - 1; ++i)
            {
                idZrodla.Add(int.Parse(dataGridView1.Rows[i].Cells[0].Value.ToString()));
                odleglosc.Add(double.Parse(dataGridView1.Rows[i].Cells[1].Value.ToString()));
                mocKermy.Add(double.Parse(dataGridView1.Rows[i].Cells[2].Value.ToString()));
                niepewnosc.Add(double.Parse(dataGridView1.Rows[i].Cells[3].Value.ToString()));
            }

            _Protokol.ZapiszDane((int)numericUpDown1.Value, dateTimePicker1.Value, idZrodla, odleglosc, mocKermy, niepewnosc);
        }

        //----------------------------------------------------
        private bool SprawdzMozliwoscAktualizacjiLubDodania()
        //----------------------------------------------------
        {
            if (1 >= dataGridView1.Rows.Count)
                return false;

            for (int i = 0; i < dataGridView1.Rows.Count - 1; ++i)
            {
                try
                {
                    int.Parse(dataGridView1.Rows[i].Cells[0].Value.ToString());
                    double.Parse(dataGridView1.Rows[i].Cells[1].Value.ToString());
                    double.Parse(dataGridView1.Rows[i].Cells[2].Value.ToString());
                    double.Parse(dataGridView1.Rows[i].Cells[3].Value.ToString());
                }
                catch(Exception)
                {
                    return false;
                }
            }

            return true;
        }

        //----------------------------------------------------
        private DataTable PobierzDaneZExcela(string plik, string workShet)
        //----------------------------------------------------
        {
            //string connectionString =            @"Provider=Microsoft.Jet.OleDb.4.0;Data source=c:  Extended Properties=""Excel 8.0;HDR=No;IMEX=1""";
            string connectionString = String.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR=Yes;IMEX=1';", plik);
            try
            {
                System.Data.Common.DbProviderFactory factory = System.Data.Common.DbProviderFactories.GetFactory("System.Data.OleDb");
                using (System.Data.Common.DbConnection connection = factory.CreateConnection())
                {
                    connection.ConnectionString = connectionString;
                    using (System.Data.OleDb.OleDbCommand command = (System.Data.OleDb.OleDbCommand)connection.CreateCommand())
                    {
                        command.CommandText = string.Format("SELECT * FROM [{0}]", workShet);
                        connection.Open();

                        System.Data.OleDb.OleDbDataAdapter adapter = new System.Data.OleDb.OleDbDataAdapter();
                        adapter.SelectCommand = command;
                        DataTable dataContainer = new DataTable();
                        adapter.Fill(dataContainer);
                        return dataContainer;
                    }
                }
            }
            catch(Exception)
            {
                return null;
            }
        }

    }
}
