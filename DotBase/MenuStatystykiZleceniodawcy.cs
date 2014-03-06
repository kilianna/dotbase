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
    public partial class MenuStatystykiZleceniodawcy : Form
    {
        //-------------------------------------------------------------------
        public MenuStatystykiZleceniodawcy()
        //-------------------------------------------------------------------
        {
            InitializeComponent();
        }

        //-------------------------------------------------------------------
        private void button1_Click(object sender, EventArgs e)
        //-------------------------------------------------------------------
        {
            SortZleceniodawcowPoLiczbiePrzyrzadow okno = new SortZleceniodawcowPoLiczbiePrzyrzadow();
            okno.ShowDialog();
        }

        //-------------------------------------------------------------------
        private void button2_Click(object sender, EventArgs e)
        //-------------------------------------------------------------------
        {
            NieprzysylajacyForm okno = new NieprzysylajacyForm();
            okno.ShowDialog();
        }

        //-------------------------------------------------------------------
        private void button3_Click(object sender, EventArgs e)
        //-------------------------------------------------------------------
        {
            ZnajdowaniePoPrzyrzadzieForm okno = new ZnajdowaniePoPrzyrzadzieForm();
            okno.ShowDialog();
        }

        //-------------------------------------------------------------------
        private void button4_Click(object sender, EventArgs e)
        //-------------------------------------------------------------------
        {
            PrzyrzadyZleceniodawcyForm okno = new PrzyrzadyZleceniodawcyForm();
            okno.ShowDialog();
        }
        
        //-------------------------------------------------------------------
        private void button5_Click(object sender, EventArgs e)
        //-------------------------------------------------------------------
        {
            Close();
        }

    }
}
