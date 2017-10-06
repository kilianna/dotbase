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
    public partial class MenuWyszukiwanieForm : Form
    {
        //-----------------------------------------------------------------------
        public MenuWyszukiwanieForm()
        //-----------------------------------------------------------------------
        {
            InitializeComponent();
        }

        //-----------------------------------------------------------------------
        private void button1_Click(object sender, EventArgs e)
        //-----------------------------------------------------------------------
        {
            button1.Enabled = false;
            StatystykaWzorcowanForm okno = new StatystykaWzorcowanForm();
            okno.Show();
            okno.FormClosing += new System.Windows.Forms.FormClosingEventHandler(OdblokujPrzyciski);
        }

        //-----------------------------------------------------------------------
        private void button2_Click(object sender, EventArgs e)
        //-----------------------------------------------------------------------
        {
            button2.Enabled = false;
            PrzyrzadyUszkodzoneWOkresieForm okno = new PrzyrzadyUszkodzoneWOkresieForm();
            okno.Show();
            okno.FormClosing += new System.Windows.Forms.FormClosingEventHandler(OdblokujPrzyciski);
        }

        //-----------------------------------------------------------------------
        private void button3_Click(object sender, EventArgs e)
        //-----------------------------------------------------------------------
        {
            button3.Enabled = false;
            PrzyrzadyWzorcowaneWOkresieForm okno = new PrzyrzadyWzorcowaneWOkresieForm();
            okno.Show();
            okno.FormClosing += new System.Windows.Forms.FormClosingEventHandler(OdblokujPrzyciski);
        }

        //-----------------------------------------------------------------------
        private void button4_Click(object sender, EventArgs e)
        //-----------------------------------------------------------------------
        {
            button4.Enabled = false;
            MenuStatystykiZleceniodawcy okno = new MenuStatystykiZleceniodawcy();
            okno.Show();
            okno.FormClosing += new System.Windows.Forms.FormClosingEventHandler(OdblokujPrzyciski);
        }

        //-----------------------------------------------------------------------
        private void button5_Click(object sender, EventArgs e)
        //-----------------------------------------------------------------------
        {
            button5.Enabled = false;
            HistoriaWzorcowanForm okno = new HistoriaWzorcowanForm();
            okno.Show();
            okno.FormClosing += new System.Windows.Forms.FormClosingEventHandler(OdblokujPrzyciski);
        }

        //-----------------------------------------------------------------------
        private void button6_Click(object sender, EventArgs e)
        //-----------------------------------------------------------------------
        {
            Close();
        }

        //-----------------------------------------------------------------------
        private void OdblokujPrzyciski(object sender, EventArgs args)
        //-----------------------------------------------------------------------
        {
            if (sender is StatystykaWzorcowanForm)
            {
                button1.Enabled = true;
            }
            else if (sender is PrzyrzadyUszkodzoneWOkresieForm)
            {
                button2.Enabled = true;
            }
            else if (sender is PrzyrzadyWzorcowaneWOkresieForm)
            {
                button3.Enabled = true;
            }
            else if (sender is MenuStatystykiZleceniodawcy)
            {
                button4.Enabled = true;
            }
            else if (sender is HistoriaWzorcowanForm)
            {
                button5.Enabled = true;
            }
        }

    }
}
