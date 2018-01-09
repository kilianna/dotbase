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
    public partial class StatystykaWzorcowanForm : Form
    {
        private static int MAX_WIERSZY = 13;
        StatystykaWzorcowan _StatystykaWzorcowan;

        //--------------------------------------------------------
        public StatystykaWzorcowanForm()
        //--------------------------------------------------------
        {
            InitializeComponent();

            _StatystykaWzorcowan = new StatystykaWzorcowan();

            dataGridView1.Rows.Add(MAX_WIERSZY);

            dataGridView1.Rows[0].Cells[0].Value = "Promieniowanie gamma";
            dataGridView1.Rows[1].Cells[0].Value = "Moc dawki";
            dataGridView1.Rows[2].Cells[0].Value = "Dawka";
            dataGridView1.Rows[3].Cells[0].Value = "Sygnalizacja";
            dataGridView1.Rows[4].Cells[0].Value = "Skażenia";
            dataGridView1.Rows[5].Cells[0].Value = "Ameryk";
            dataGridView1.Rows[6].Cells[0].Value = "Chlor";
            dataGridView1.Rows[7].Cells[0].Value = "Pluton";
            dataGridView1.Rows[8].Cells[0].Value = "Stront słaby";
            dataGridView1.Rows[9].Cells[0].Value = "Stront silny";
            dataGridView1.Rows[10].Cells[0].Value = "Stront najsilniejszy";
            dataGridView1.Rows[11].Cells[0].Value = "Węgiel słaby";
            dataGridView1.Rows[12].Cells[0].Value = "Węgiel silny";

            dataGridView1.Columns[1].ValueType = typeof(int);
        }

        //--------------------------------------------------------
        private void CzyscStareDane()
        //--------------------------------------------------------
        {
            for(int i = 0; i < MAX_WIERSZY; ++i)
            {
                dataGridView1.Rows[i].Cells[1].Value = 0;
            }
        }

        //--------------------------------------------------------
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        //--------------------------------------------------------
        {
            WyswietlDane();
        }

        //--------------------------------------------------------
        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        //--------------------------------------------------------
        {
            WyswietlDane();
        }

        //--------------------------------------------------------
        private void WyswietlDane()
        //--------------------------------------------------------
        {
            CzyscStareDane();

            DateTime SzukajOd = dateTimePicker1.Value;
            DateTime SzukajDo = dateTimePicker2.Value;

            if (SzukajOd > SzukajDo)
            {
                MessageBox.Show("Data do której szukać nie może być mniejsza niż data od której szuakć.");
                return;
            }

            _StatystykaWzorcowan.ZbierzStatystyki(ref SzukajOd, ref SzukajDo);
            
            textBox1.Text = _StatystykaWzorcowan.Wyniki[(int)StatystykaWzorcowan.Stale.LICZBA_ZLECEN].ToString();
            textBox2.Text = _StatystykaWzorcowan.Wyniki[(int)StatystykaWzorcowan.Stale.LICZBA_WZORCOWAN].ToString();
            textBox3.Text = _StatystykaWzorcowan.Wyniki[(int)StatystykaWzorcowan.Stale.LICZBA_PRZYRZADOW].ToString();
            textBox4.Text = _StatystykaWzorcowan.Wyniki[(int)StatystykaWzorcowan.Stale.LICZBA_WYSTAWIONYCH_SWIADECTW].ToString();

            dataGridView1.Rows[0].Cells[1].Value = _StatystykaWzorcowan.Wyniki[(int)StatystykaWzorcowan.Stale.PROMIENIOWANIE_GAMMA];
            dataGridView1.Rows[1].Cells[1].Value = _StatystykaWzorcowan.Wyniki[(int)StatystykaWzorcowan.Stale.MOC_DAWKI];
            dataGridView1.Rows[2].Cells[1].Value = _StatystykaWzorcowan.Wyniki[(int)StatystykaWzorcowan.Stale.DAWKA];
            dataGridView1.Rows[3].Cells[1].Value = _StatystykaWzorcowan.Wyniki[(int)StatystykaWzorcowan.Stale.SYGNALIZACJA];
            dataGridView1.Rows[4].Cells[1].Value = _StatystykaWzorcowan.Wyniki[(int)StatystykaWzorcowan.Stale.SKAZENIA];
            dataGridView1.Rows[5].Cells[1].Value = _StatystykaWzorcowan.Wyniki[(int)StatystykaWzorcowan.Stale.AMERYK];
            dataGridView1.Rows[6].Cells[1].Value = _StatystykaWzorcowan.Wyniki[(int)StatystykaWzorcowan.Stale.CHLOR];
            dataGridView1.Rows[7].Cells[1].Value = _StatystykaWzorcowan.Wyniki[(int)StatystykaWzorcowan.Stale.PLUTON];
            dataGridView1.Rows[8].Cells[1].Value = _StatystykaWzorcowan.Wyniki[(int)StatystykaWzorcowan.Stale.STRONT_SLABY];
            dataGridView1.Rows[9].Cells[1].Value = _StatystykaWzorcowan.Wyniki[(int)StatystykaWzorcowan.Stale.STRONT_SILNY];
            dataGridView1.Rows[10].Cells[1].Value = _StatystykaWzorcowan.Wyniki[(int)StatystykaWzorcowan.Stale.STRONT_NAJSILNIEJSZY];
            dataGridView1.Rows[11].Cells[1].Value = _StatystykaWzorcowan.Wyniki[(int)StatystykaWzorcowan.Stale.WEGIEL_SLABY];
            dataGridView1.Rows[12].Cells[1].Value = _StatystykaWzorcowan.Wyniki[(int)StatystykaWzorcowan.Stale.WEGIEL_SILNY];
        }
    }
}
