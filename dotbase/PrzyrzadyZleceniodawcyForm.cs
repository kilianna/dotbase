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
    public partial class PrzyrzadyZleceniodawcyForm : Form
    {
        List<string> _nazwaZleceniodawcy;
        List<int> _idZleceniodawcy;

        BazaDanychWrapper _BazaDanych;

        //-----------------------------------------------------------
        public PrzyrzadyZleceniodawcyForm()
        //-----------------------------------------------------------
        {
            InitializeComponent();

            _BazaDanych = new BazaDanychWrapper();
            _nazwaZleceniodawcy = new List<string>();
            _idZleceniodawcy = new List<int>();

            ZnajdzWszystkichZleceniodawcow();

            comboBox1.Items.AddRange(_nazwaZleceniodawcy.ToArray());
        }

        //-----------------------------------------------------------
        // filtrowanie zleceniodawców po id
        private void textBox1_TextChanged(object sender, EventArgs e)
        //-----------------------------------------------------------
        {
            int wpisaneId;

            if( int.TryParse(textBox1.Text, out wpisaneId))
            {
                int index = ZnajdzIndexZleceniodawcy(wpisaneId);

                comboBox1.Text = "";
                comboBox1.Items.Clear();

                if (index >= 0)
                {
                    comboBox1.Items.Add(_nazwaZleceniodawcy[index]);
                    comboBox1.Text = _nazwaZleceniodawcy[index];
                    WyswietlDane();
                }
            }

        }

        //-----------------------------------------------------------
        int ZnajdzIndexZleceniodawcy(int id)
        //-----------------------------------------------------------
        {
            int index = -1;

            for (int i = 0; i < _idZleceniodawcy.Count; ++i)
            {
                if (_idZleceniodawcy[i] == id)
                {
                    index = i;
                    break;
                }
            }

            return index;
        }

        //-----------------------------------------------------------
        // filtrowanie zleceniodaców po nazwie
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        //-----------------------------------------------------------
        {
            int index = 0;

            for (int i = 0; i < _nazwaZleceniodawcy.Count; ++i)
            {
                if (comboBox1.Text == _nazwaZleceniodawcy[i])
                {
                    index = i;
                    break;
                }
            }

            textBox1.Text = _idZleceniodawcy[index].ToString();
        }

        //-----------------------------------------------------------
        void FiltrujNazwyZLeceniodawcow(object sender, EventArgs e)
        //-----------------------------------------------------------
        {
            var szukaj = comboBox1.Text.ToUpper();
            var oldStart = comboBox1.SelectionStart;

            comboBox1.Items.Clear();

            if (comboBox1.Text == "")
            {
                comboBox1.Items.AddRange(_nazwaZleceniodawcy.ToArray());
                return;
            }

            var wynik1 = (from nazwa in _nazwaZleceniodawcy where nazwa.ToUpper().IndexOf(szukaj) == 0 select nazwa).ToArray<string>();
            var wynik2 = (from nazwa in _nazwaZleceniodawcy where nazwa.ToUpper().IndexOf(szukaj) > 0 select nazwa).ToArray<string>();

            comboBox1.Items.Add(comboBox1.Text);
            comboBox1.Items.AddRange(wynik1.ToArray<string>());
            comboBox1.Items.AddRange(wynik2.ToArray<string>());
            comboBox1.SelectionStart = oldStart;
            comboBox1.SelectionLength = 0;
        }

        //-----------------------------------------------------------
        // metoda wywołana jedynie raz w trakcie tworzenia obiektu
        private void ZnajdzWszystkichZleceniodawcow()
        //-----------------------------------------------------------
        {
            DataTable dane = _BazaDanych.TworzTabeleDanych("SELECT DISTINCT zleceniodawca, id_zleceniodawcy FROM Zleceniodawca");

            foreach (DataRow row in dane.Rows)
            {
                _nazwaZleceniodawcy.Add(row.Field<string>(0));
                _idZleceniodawcy.Add(row.Field<int>(1));
            }
        }

        //-----------------------------------------------------------
        private void WyswietlDane()
        //-----------------------------------------------------------
        {
            dataGridView1.Rows.Clear();

            DataTable dane = _BazaDanych.TworzTabeleDanych("SELECT DISTINCT typ, nr_fabryczny FROM ((Dozymetry AS D INNER JOIN Karta_przyjecia "
                           + "AS K ON D.id_dozymetru=K.id_dozymetru) INNER JOIN Zlecenia AS Z ON Z.id_zlecenia=K.id_zlecenia) INNER JOIN "
                           + String.Format("Zleceniodawca AS ZL ON Z.id_zleceniodawcy=ZL.id_zleceniodawcy WHERE ZL.id_zleceniodawcy = {0}", 
                             textBox1.Text));

            if (dane != null)
            {
                foreach (DataRow row in dane.Rows)
                {
                    dataGridView1.Rows.Add(row.Field<string>(0), row.Field<string>(1));
                }
            }
        }
    }
}
