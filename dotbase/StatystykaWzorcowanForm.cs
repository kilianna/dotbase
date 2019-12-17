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
        StatystykaWzorcowan _StatystykaWzorcowan;
        
        //--------------------------------------------------------
        public StatystykaWzorcowanForm()
        //--------------------------------------------------------
        {
            InitializeComponent();
            typZleceniodawcy.SelectedIndex = 0;
            _StatystykaWzorcowan = new StatystykaWzorcowan();
        }
           
        private void dodajWiersz(string nazwa, object wartosc = null)
        {
            dataGridView1.Rows.Add();
            var row = dataGridView1.Rows[dataGridView1.Rows.Count - 1];
            row.HeaderCell.Value = nazwa;
            if (wartosc == null)
            {
                row.HeaderCell.Style.Font = new Font(Font.FontFamily, Font.Size * 1.2f, FontStyle.Bold);
                row.HeaderCell.Style.ForeColor = Color.DarkBlue;
                row.HeaderCell.Style.BackColor = Color.FromArgb(240, 240, 240);
                row.Cells[0].Style.BackColor = Color.FromArgb(240, 240, 240);
                row.Height = (int)Math.Round((double)row.Height * 1.3);
                return;
            }
            row.Cells[0].Value = wartosc;
        }

        //--------------------------------------------------------
        private void CzyscStareDane()
        //--------------------------------------------------------
        {
            dataGridView1.Rows.Clear();
            dataGridView1.RowHeadersWidth = 240;
            dataGridView1.Columns[0].Visible = true;
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
            if (_StatystykaWzorcowan == null) return;

            CzyscStareDane();

            DateTime SzukajOd = dateTimePicker1.Value.Date;
            DateTime SzukajDo = dateTimePicker2.Value.Date.AddDays(1).AddTicks(-1);
            StatystykaWzorcowan.WidokIFJ ifj =
                typZleceniodawcy.SelectedIndex == 1 ? StatystykaWzorcowan.WidokIFJ.Nie :
                typZleceniodawcy.SelectedIndex == 2 ? StatystykaWzorcowan.WidokIFJ.Tak :
                StatystykaWzorcowan.WidokIFJ.Wszystko;
            
            if (SzukajOd > SzukajDo)
            {
                dodajWiersz("Podane daty są błędne.", "");
                dodajWiersz("Brak danych do wyświetlenia.", "");
                dataGridView1.RowHeadersWidth = dataGridView1.Width - 8;
                dataGridView1.Columns[0].Visible = false;
                return;
            }

            _StatystykaWzorcowan.ZbierzStatystyki(ref SzukajOd, ref SzukajDo, ifj);

            dodajWiersz("Ogólne");
            dodajWiersz("Liczb zleceń", _StatystykaWzorcowan.Wyniki[(int)StatystykaWzorcowan.Stale.LICZBA_ZLECEN]);
            dodajWiersz("Liczba przyrządów", _StatystykaWzorcowan.Wyniki[(int)StatystykaWzorcowan.Stale.LICZBA_PRZYRZADOW]);
            dodajWiersz("Liczba wzorcowań", _StatystykaWzorcowan.Wyniki[(int)StatystykaWzorcowan.Stale.LICZBA_WZORCOWAN]);
            dodajWiersz("Liczba wystawionych świadectw", _StatystykaWzorcowan.Wyniki[(int)StatystykaWzorcowan.Stale.LICZBA_WYSTAWIONYCH_SWIADECTW]);

            dodajWiersz("Dziedzina 18.01");
            dodajWiersz("Wystawione świadetwa", _StatystykaWzorcowan.Wyniki[(int)StatystykaWzorcowan.Stale.WZORCOWANE_CEZEM]);
            dodajWiersz("Wykonane wzorcowania", _StatystykaWzorcowan.Wyniki[(int)StatystykaWzorcowan.Stale.PROMIENIOWANIE_GAMMA]);
            dodajWiersz("Moc dawki", _StatystykaWzorcowan.Wyniki[(int)StatystykaWzorcowan.Stale.MOC_DAWKI]);
            dodajWiersz("Dawka", _StatystykaWzorcowan.Wyniki[(int)StatystykaWzorcowan.Stale.DAWKA]);
            dodajWiersz("Sygnalizacja mocy dawki", _StatystykaWzorcowan.Wyniki[(int)StatystykaWzorcowan.Stale.SYGNALIZACJA_MOCY_DAWKI]);
            dodajWiersz("Sygnalizacja dawki", _StatystykaWzorcowan.Wyniki[(int)StatystykaWzorcowan.Stale.SYGNALIZACJA_DAWKI]);

            dodajWiersz("Dziedzina 18.02");
            dodajWiersz("Wystawione świadetwa", _StatystykaWzorcowan.Wyniki[(int)StatystykaWzorcowan.Stale.WZORCOWANE_NA_SKAZENIA]);
            dodajWiersz("Wykonane wzorcowania", _StatystykaWzorcowan.Wyniki[(int)StatystykaWzorcowan.Stale.SKAZENIA]);
            dodajWiersz("Ameryk", _StatystykaWzorcowan.Wyniki[(int)StatystykaWzorcowan.Stale.AMERYK]);
            dodajWiersz("Chlor", _StatystykaWzorcowan.Wyniki[(int)StatystykaWzorcowan.Stale.CHLOR]);
            dodajWiersz("Pluton", _StatystykaWzorcowan.Wyniki[(int)StatystykaWzorcowan.Stale.PLUTON]);
            dodajWiersz("Stront słaby", _StatystykaWzorcowan.Wyniki[(int)StatystykaWzorcowan.Stale.STRONT_SLABY]);
            dodajWiersz("Stront silny", _StatystykaWzorcowan.Wyniki[(int)StatystykaWzorcowan.Stale.STRONT_SILNY]);
            dodajWiersz("Stront najsilniejszy", _StatystykaWzorcowan.Wyniki[(int)StatystykaWzorcowan.Stale.STRONT_NAJSILNIEJSZY]);
            dodajWiersz("Węgiel słaby", _StatystykaWzorcowan.Wyniki[(int)StatystykaWzorcowan.Stale.WEGIEL_SLABY]);
            dodajWiersz("Węgiel silny", _StatystykaWzorcowan.Wyniki[(int)StatystykaWzorcowan.Stale.WEGIEL_SILNY]);

        }

        private void typZleceniodawcy_SelectedIndexChanged(object sender, EventArgs e)
        {
            WyswietlDane();
        }
    }
}
