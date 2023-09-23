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
    public partial class RejestrZlecenForm : Form
    {
        private BazaDanychWrapper _BazaDanych;
        private string _Zapytanie;
        private uint MAX_ROK, MIN_ROK;
        DateTime DZISIAJ;

        public uint Rok { get; private set; }

        //----------------------------------------------------------
        public RejestrZlecenForm()
        //----------------------------------------------------------
        {
            InitializeComponent();
            _BazaDanych = new BazaDanychWrapper();
            DZISIAJ = DateTime.Today;
        }

        //----------------------------------------------------------
        // metoda wywoływana tylko raz zaraz po stworzeniu obiektu
        public bool Inicjalizuj()
        //----------------------------------------------------------
        {
            if (ZnajdzOstatniRok())
            {
                numericUpDown1.Value = Rok;
                numericUpDown1.ValueChanged += numericUpDown1_ValueChanged;
                PobierzDanePodstawowe();
                PobierzDaneStatystyczne();
                return true;
            }

            return false;
        }

        //----------------------------------------------------------
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        //----------------------------------------------------------
        {
            Rok = (uint)numericUpDown1.Value;
            dataGridView1.Rows.Clear();

            if (MAX_ROK < Rok ||  Rok < MIN_ROK)
            {
                return;
            }

            if (false == PobierzDanePodstawowe() || false == PobierzDaneStatystyczne())
                MessageBox.Show("Nie można pobrać danych. Prawdopodobnie połączenie z bazą danych zostało przerwane.");
        }

        //----------------------------------------------------------
        private bool PobierzDanePodstawowe()
        //----------------------------------------------------------
        {
            DateTime terminPlanowanegoWykonania;

            _Zapytanie = "SELECT  Z1.id_zlecenia, zleceniodawca, data_przyjecia, forma_przyjecia, data_zwrotu, forma_zwrotu, Z1.uwagi, Ekspres "
                       + "FROM (Zlecenia AS Z1 INNER JOIN Zleceniodawca Z2 ON Z1.id_zleceniodawcy=Z2.id_zleceniodawcy) "
                       + String.Format("WHERE YEAR(data_przyjecia) = {0} AND MONTH(data_przyjecia) = {1} ORDER BY Z1.id_zlecenia", Rok, numericUpDown2.Value);

            DataTable dane = _BazaDanych.TworzTabeleDanych(_Zapytanie);
            try
            {
                for (int i = 0; i < dane.Rows.Count; ++i)
                {
                    DataRow row = dane.Rows[i];

                    dataGridView1.Rows.Add(row.Field<int>("id_zlecenia"), 
                                           row.Field<string>("zleceniodawca"),
                                           "", 
                                           row.Field<string>("forma_przyjecia"),
                                           "",
                                           row.Field<string>("forma_zwrotu"), 
                                           "", 
                                           row.Field<string>("uwagi"), 
                                           "",
                                           "", 
                                           "");
                    
                    

                    if (false == row.IsNull(2))
                    {
                        dataGridView1.Rows[i].Cells[2].Value = row.Field<DateTime>(2).ToShortDateString();

                        if (row.Field<bool>("Ekspres"))
                        {
                            terminPlanowanegoWykonania = row.Field<DateTime>(2).AddDays(14);
                            PodswietlPola(ref terminPlanowanegoWykonania, ref i, ref row);
                        }
                        else
                        {
                            terminPlanowanegoWykonania = row.Field<DateTime>(2).AddDays(1);
                            PodswietlEkspres(ref i);
                        }

                        dataGridView1.Rows[i].Cells[6].Value = terminPlanowanegoWykonania.ToShortDateString();
                    }

                    if (false == row.IsNull(4))
                    {
                        dataGridView1.Rows[i].Cells[4].Value = row.Field<DateTime>(4).ToShortDateString();
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        //----------------------------------------------------------
        private void PodswietlPola(ref DateTime terminPlanowanegoWykonania, ref int i, ref DataRow row)
        //----------------------------------------------------------
        {
            if (false == row.Field<bool>("Ekspres"))
            {
                if (terminPlanowanegoWykonania.AddDays(3) >= DZISIAJ)
                    dataGridView1.Rows[i].DefaultCellStyle.ForeColor = Color.Yellow;
                else if (terminPlanowanegoWykonania < DZISIAJ)
                    dataGridView1.Rows[i].DefaultCellStyle.ForeColor = Color.Red;
            }
            else
            {
                dataGridView1.Rows[i].DefaultCellStyle.ForeColor = Color.Green;
            }
        }

        //----------------------------------------------------------
        private void PodswietlEkspres(ref int i)
        //----------------------------------------------------------
        {
            dataGridView1.Rows[i].DefaultCellStyle.ForeColor = Color.Blue;
        }

        //----------------------------------------------------------
        private bool PobierzDaneStatystyczne()
        //----------------------------------------------------------
        {
            progressBar1.Maximum = dataGridView1.Rows.Count;

            for (int i = 0; i < dataGridView1.Rows.Count; ++i)
            {
                int idZLecenia = (int)dataGridView1.Rows[i].Cells[0].Value;
                _Zapytanie = "SELECT typ, COUNT(*) FROM Karta_przyjecia AS K INNER JOIN Dozymetry AS D ON K.id_dozymetru=D.id_dozymetru "
                           + String.Format("WHERE id_zlecenia={0} GROUP BY typ", idZLecenia);
                
                DataTable dane = _BazaDanych.TworzTabeleDanych(_Zapytanie);
                
                try
                {
                    foreach (DataRow wiersz in dane.Rows)
                        dataGridView1.Rows[i].Cells[11].Value += String.Format("{0} {1}", wiersz.Field<string>(0), wiersz.Field<int>(1).ToString());
                }
                catch (Exception)
                {
                    return false;
                }
                
                _Zapytanie = String.Format("SELECT COUNT(*) FROM Karta_przyjecia WHERE id_zlecenia = {0} ", idZLecenia);
                dane = _BazaDanych.TworzTabeleDanych(_Zapytanie);
                dataGridView1.Rows[i].Cells[10].Value = dane.Rows[0].Field<int>(0);
                _Zapytanie = String.Format("SELECT COUNT(*) FROM Karta_przyjecia WHERE id_zlecenia = {0} AND wykonano = false", idZLecenia);
                dane = _BazaDanych.TworzTabeleDanych(_Zapytanie);
                int liczbeNiewykonanych = dane.Rows[0].Field<int>(0);
                dataGridView1.Rows[i].Cells[9].Value = liczbeNiewykonanych;
                dataGridView1.Rows[i].Cells[8].Value = liczbeNiewykonanych == 0;

                progressBar1.Value = i;
            }

            return true;
        }

        //----------------------------------------------------------
        private bool ZnajdzOstatniRok()
        //----------------------------------------------------------
        {
            try
            {
                MAX_ROK = Rok = (uint)_BazaDanych.TworzTabeleDanych("SELECT MAX(rok) FROM Karta_przyjecia").Rows[0].Field<int>(0);
            }
            catch(Exception)
            {
                return false;
            }

            return true;
        }

        //----------------------------------------------------------
        private bool ZnajdzPierwszyRok()
        //----------------------------------------------------------
        {
            try
            {
                MIN_ROK = (uint)_BazaDanych.TworzTabeleDanych("SELECT MIN(rok) FROM Karta_przyjecia").Rows[0].Field<short>(0);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        //----------------------------------------------------------
        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        //----------------------------------------------------------
        {
            dataGridView1.Rows.Clear();

            if (false == PobierzDanePodstawowe() || false == PobierzDaneStatystyczne())
                MessageBox.Show("Nie można pobrać danych. Prawdopodobnie połączenie z bazą danych zostało przerwane.");
        }

        //----------------------------------------------------------
        private void PrzejdzDoZlecenia(object sender, EventArgs e)
        //----------------------------------------------------------
        {
            MessageBox.Show(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
        }

        private void RejestrZlecenForm_Load(object sender, EventArgs e)
        {
            numericUpDown1.Value = DZISIAJ.Year;
            numericUpDown2.Value = DZISIAJ.Month;
        }
    }
}
