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
    public partial class NieprzysylajacyForm : Form
    {
        BazaDanychWrapper _BazaDanych;
        List<int> _IdZleceniodawcy;
        List<string> _NazwaZleceniodawcy;
        DataTable _OdpowiedzBazy;
        string _Zapytanie;

        //----------------------------------------
        public NieprzysylajacyForm()
        //----------------------------------------
        {
            InitializeComponent();
            _IdZleceniodawcy = new List<int>();
            _NazwaZleceniodawcy = new List<string>();
            _BazaDanych = new BazaDanychWrapper();
            dataGridView1.Columns[0].ValueType = typeof(int);
            WyszukajMozliwychZleceniodawcow();
        }

        //----------------------------------------
        private void WyszukajMozliwychZleceniodawcow()
        //----------------------------------------
        {
            _OdpowiedzBazy = _BazaDanych.TworzTabeleDanych("Select id_zleceniodawcy, zleceniodawca FROM Zleceniodawca ORDER BY Zleceniodawca");

            foreach (DataRow row in _OdpowiedzBazy.Rows)
            {
                _IdZleceniodawcy.Add(row.Field<int>(0));
                _NazwaZleceniodawcy.Add(row.Field<string>(1));
            }
        }

        //----------------------------------------
        private void WyswietlZnalezionychZleceniodawcow(object sender, EventArgs e)
        //----------------------------------------
        {
            dataGridView1.Rows.Clear();

            _Zapytanie = "(Select DISTINCT Z1.id_zleceniodawcy, Z2.zleceniodawca From Zlecenia AS Z1 INNER JOIN Zleceniodawca Z2 "
                       + String.Format("ON Z1.id_zleceniodawcy=Z2.id_zleceniodawcy WHERE data_przyjecia BETWEEN #{0}# AND #{1}#) ",
                                       dateTimePicker1.Value.ToShortDateString(), dateTimePicker2.Value.ToShortDateString());

            if (!_BazaDanych.TworzTabeleDanychWPamieci(_Zapytanie))
                return;

            List<KeyValuePair<int, string>> z1 = new List<KeyValuePair<int, string>>();

            foreach (DataRow row in _BazaDanych.Tabela.Rows)
            {
                z1.Add(new KeyValuePair<int, string>(row.Field<int>(0), row.Field<string>(1)));
            }
            

            _Zapytanie = "(Select DISTINCT Z1.id_zleceniodawcy, Z2.zleceniodawca From Zlecenia AS Z1 INNER JOIN Zleceniodawca Z2 "
                       + String.Format("ON Z1.id_zleceniodawcy=Z2.id_zleceniodawcy WHERE data_przyjecia BETWEEN #{0}# AND #{1}#)",
                                       dateTimePicker3.Value.ToShortDateString(), dateTimePicker4.Value.ToShortDateString());

            if (_BazaDanych.TworzTabeleDanychWPamieci(_Zapytanie))
            {
                foreach (DataRow row in _BazaDanych.Tabela.Rows)
                {
                    z1.Remove(new KeyValuePair<int, string>(row.Field<int>(0), row.Field<string>(1)));
                }
            }

            foreach (KeyValuePair<int, string> data in z1)
            {
                dataGridView1.Rows.Add(data.Key, data.Value);
            }
        }
    }
}
