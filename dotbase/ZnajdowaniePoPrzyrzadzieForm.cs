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
    public partial class ZnajdowaniePoPrzyrzadzieForm : Form
    {
        List<String> _TypyPrzyrzadow;
        BazaDanychWrapper _BazaDanych;
        DataTable _OdpowiedzBazy;
        String _Zapytanie;

        //---------------------------------------------------------
        public ZnajdowaniePoPrzyrzadzieForm()
        //---------------------------------------------------------
        {
            InitializeComponent();
            _TypyPrzyrzadow = new List<string>();
            _BazaDanych = new BazaDanychWrapper();
            dataGridView1.Columns[0].ValueType = typeof(int);
            PobierzTypyPrzyrzadow();
        }

        //---------------------------------------------------------
        private void PobierzTypyPrzyrzadow()
        //---------------------------------------------------------
        {
            _OdpowiedzBazy = _BazaDanych.TworzTabeleDanych("SELECT DISTINCT typ FROM Dozymetry");

            foreach (DataRow row in _OdpowiedzBazy.Rows)
            {
                _TypyPrzyrzadow.Add(row.Field<string>(0));
            }
        }

        //---------------------------------------------------------
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        //---------------------------------------------------------
        {
            dataGridView1.Rows.Clear();

            _Zapytanie = "SELECT DISTINCT Z.id_zleceniodawcy, zleceniodawca FROM ((Zleceniodawca AS Z INNER JOIN Zlecenia AS ZL ON "
                       + "Z.id_zleceniodawcy=ZL.id_zleceniodawcy) INNER JOIN Karta_przyjecia AS K ON K.id_zlecenia=ZL.id_zlecenia) "
                       + String.Format("INNER JOIN Dozymetry AS D ON K.id_dozymetru=D.id_dozymetru WHERE D.typ='{0}'", comboBox1.Text);

            _OdpowiedzBazy = _BazaDanych.TworzTabeleDanych(_Zapytanie);

            if (null == _OdpowiedzBazy)
                return;

            foreach(DataRow row in _OdpowiedzBazy.Rows)
            {
                dataGridView1.Rows.Add(row.Field<int>(0), row.Field<string>(1));
            }
        }

        //---------------------------------------------------------
        private void comboBox1_TextChanged(object sender, EventArgs e)
        //---------------------------------------------------------
        {
            comboBox1.Items.Clear();

            if (comboBox1.Text == "")
            {
                comboBox1.Items.AddRange(_TypyPrzyrzadow.ToArray());
                return;
            }

            var wynik = (from typ in _TypyPrzyrzadow where typ.Contains(comboBox1.Text) select typ).ToArray<string>();

            comboBox1.Items.Add(comboBox1.Text);
            comboBox1.Items.AddRange(wynik.ToArray<string>());
            comboBox1.SelectionStart = comboBox1.Text.Length;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
