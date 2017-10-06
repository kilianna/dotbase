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
    // Klasa jest tak prosta, że nie została rozbita na część GUI i Backend'owa.
    public partial class PrzyrzadyWzorcowaneWOkresieForm : Form
    {
        private string _Zapytanie;
        private BazaDanychWrapper _Baza;
        private DataTable _Dane;
        private List<string> _Typy;

        //---------------------------------------------------------------------
        public PrzyrzadyWzorcowaneWOkresieForm()
        //---------------------------------------------------------------------
        {
            InitializeComponent();
            _Baza = new BazaDanychWrapper();
            _Typy = new List<string>();

            dataGridView1.Columns[1].ValueType = typeof(int);
        }

        //---------------------------------------------------------------------
        private void CzyscStareDane()
        //---------------------------------------------------------------------
        {
            dataGridView1.Rows.Clear();
            _Typy.Clear();
        }

        //---------------------------------------------------------------------
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        //---------------------------------------------------------------------
        {
            Wyszukaj();
        }

        //---------------------------------------------------------------------
        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        //---------------------------------------------------------------------
        {
            Wyszukaj();
        }

        //---------------------------------------------------------------------
        private void Wyszukaj()
        //---------------------------------------------------------------------
        {
            CzyscStareDane();

            _Zapytanie = "SELECT DISTINCT typ FROM (Dozymetry AS D INNER JOIN Karta_przyjecia AS K ON D.id_dozymetru=K.id_dozymetru) " 
                       + " INNER JOIN Zlecenia AS Z ON K.id_zlecenia=Z.id_zlecenia WHERE Z.data_przyjecia BETWEEN "
                       + String.Format("#{0}# AND #{1}# ORDER BY typ", dateTimePicker1.Value.ToShortDateString(), dateTimePicker2.Value.ToShortDateString());
            
            _Dane = _Baza.TworzTabeleDanych(_Zapytanie);

            foreach (DataRow row in _Dane.Rows)
            {
                _Typy.Add(row.Field<string>("typ"));
            }

            for(int i = 0; i < _Typy.Count; ++i)
            {
                _Zapytanie = "SELECT COUNT(*) AS ILE FROM (karta_przyjecia AS K INNER JOIN Dozymetry D ON K.id_dozymetru=D.id_dozymetru) "
                           + "INNER JOIN Zlecenia AS Z ON K.id_zlecenia=Z.id_zlecenia WHERE D.typ = "
                           + String.Format("'{0}' AND Z.data_przyjecia BETWEEN #{1}# AND #{2}#", _Typy[i], 
                           dateTimePicker1.Value.ToShortDateString(), dateTimePicker2.Value.ToShortDateString());
                
                _Dane = _Baza.TworzTabeleDanych(_Zapytanie);

                dataGridView1.Rows.Add(1);
                dataGridView1.Rows[dataGridView1.Rows.Count-1].Cells["Typ"].Value = _Typy[i];
                dataGridView1.Rows[dataGridView1.Rows.Count-1].Cells["LiczbaWzorcowan"].Value = _Dane.Rows[0].Field<int>(0);
            }
        }
    }
}
