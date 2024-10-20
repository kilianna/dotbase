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
    public partial class WzorcowanieZrodelForm : Form
    {
        private WzorcowanieZrodel _WzorcowanieZrodel;

        // atrybuty zapamiętujące w której komórce data ma zostać zmieniona
        private int _Kolumna;
        private int _Wiersz;

        bool _TrybDodawania;

        //----------------------------------------------------
        public WzorcowanieZrodelForm()
        //----------------------------------------------------
        {
            InitializeComponent();
            _WzorcowanieZrodel = new WzorcowanieZrodel();
            dateTimePicker1.Enabled = false;
            comboBox1.Enabled = false;
            _TrybDodawania = false;
        }

        //----------------------------------------------------
        public bool Inicjalizuj()
        //----------------------------------------------------
        {
            if (_WzorcowanieZrodel.TworzSlownik() && _WzorcowanieZrodel.ZnajdzOstatnieWzorcowanie())
            {
                comboBox1.Items.AddRange(_WzorcowanieZrodel.Zrodla.Keys.ToArray<string>());
                
                WyswietlDane();
                return true;
            }
            else
            {
                return false;
            }
        }

        //----------------------------------------------------
        private void WlaczTrybUstawianiaDaty()
        //----------------------------------------------------
        {
            button1.Enabled = false;
            button2.Enabled = false;
            dateTimePicker1.Enabled = true;
        }

        //----------------------------------------------------
        private void WlaczTrybUstawianiaZrodla()
        //----------------------------------------------------
        {
            button1.Enabled = false;
            button2.Enabled = false;
            comboBox1.Enabled = true;
        }

        //----------------------------------------------------
        private void UstawDate(object sender, EventArgs args)
        //----------------------------------------------------
        {
            button1.Enabled = true;
            if (false == _TrybDodawania)
                button2.Enabled = true;
            dateTimePicker1.Enabled = false;

            dataGridView1.Rows[_Wiersz].Cells[_Kolumna].Value = dateTimePicker1.Value.ToShortDateString();
        }

        //----------------------------------------------------
        private void WyswietlDane()
        //----------------------------------------------------
        {
            foreach(DataRow row in _WzorcowanieZrodel.OdpowiedzBazy.Rows)
            {
                dataGridView1.Rows.Add(row.Field<short>("ID_Zrodla"), 
                                       row.Field<DateTime>("Data_wzorcowania").ToShortDateString(), 
                                       row.Field<double>("Emisja_powierzchniowa"),
                                       row.Field<short>("Id_Atestu"));
            }
        }

        //----------------------------------------------------
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        //----------------------------------------------------
        {
            button1.Enabled = true;
            if(false == _TrybDodawania)
                button2.Enabled = true;
            comboBox1.Enabled = false;

            dataGridView1.Rows[_Wiersz].Cells[_Kolumna].Value = _WzorcowanieZrodel.Zrodla[comboBox1.Text];
        }

        //----------------------------------------------------
        private void UstawDaneTabelowe(object sender, EventArgs e)
        //----------------------------------------------------
        {
            _Kolumna = ((DataGridView)sender).SelectedCells[0].ColumnIndex;
            _Wiersz = ((DataGridView)sender).SelectedCells[0].RowIndex;

            if (_Kolumna == 0)
                WlaczTrybUstawianiaZrodla();
            else if (_Kolumna == 1)
                WlaczTrybUstawianiaDaty();
        }

        //----------------------------------------------------
        private void CzyscTabele()
        //----------------------------------------------------
        {
            dataGridView1.Rows.Clear();
        }

        //----------------------------------------------------
        // rozpoczęcie dodawania
        private void button1_Click(object sender, EventArgs e)
        //----------------------------------------------------
        {
            if (_TrybDodawania)
            {
                _TrybDodawania = false;
                button1.Text = "Dodaj";
                button2.Enabled = true;

                List<short> zrodlo = new List<short>();
                List<string> dataWzorcowania = new List<string>();
                List<double> emisjaPowierzchniowa = new List<double>();

                for (int i = 0; i < dataGridView1.Rows.Count - 1; ++i)
                {
                    DataGridViewRow row = dataGridView1.Rows[i];

                    zrodlo.Add(short.Parse(row.Cells[0].Value.ToString()));
                    dataWzorcowania.Add(row.Cells[1].Value.ToString());
                    emisjaPowierzchniowa.Add(N.doubleParse(row.Cells[2].Value.ToString()));
                }

                _WzorcowanieZrodel.DodajDane(zrodlo, dataWzorcowania, emisjaPowierzchniowa);
                WyswietlDane();
            }
            else
            {
                _TrybDodawania = true;
                CzyscTabele();
                button1.Text = "Zatwierdź dodanie";
                button2.Enabled = false;
            }
        }

        //----------------------------------------------------
        // aktualizacja danych
        private void button2_Click(object sender, EventArgs e)
        //----------------------------------------------------
        {
            if (false == SprawdzPoprawnoscDanych())
            {
                MyMessageBox.Show("Cześć danych jest niepoprawnych. Akcja nie zostanie podjęta.");
                return;
            }

            List<short> zrodlo = new List<short>();
            List<string> dataWzorcowania = new List<string>();
            List<double> emisjaPowierzchniowa = new List<double>();
            List<short> idAtestu = new List<short>();

            for (int i = 0; i < dataGridView1.Rows.Count - 1; ++i)
            {
                DataGridViewRow row = dataGridView1.Rows[i];

                zrodlo.Add((short)row.Cells[0].Value);
                dataWzorcowania.Add(row.Cells[1].Value.ToString());
                emisjaPowierzchniowa.Add(N.doubleParse(row.Cells[2].Value.ToString()));
                idAtestu.Add((short)row.Cells[3].Value);
            }

            _WzorcowanieZrodel.AktualizujDane(zrodlo, dataWzorcowania, emisjaPowierzchniowa, idAtestu);
        }

        //----------------------------------------------------
        private bool SprawdzPoprawnoscDanych()
        //----------------------------------------------------
        {
            for( int i = 0; i < dataGridView1.Rows.Count - 1; ++i)
            {
                try
                {
                    N.doubleParse(dataGridView1.Rows[i].Cells[2].Value.ToString());
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return true;
        }

    }
}
