using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DotBase.Test
{
    public partial class TestHtmlControl : Form
    {
        public TestHtmlControl()
        {
            InitializeComponent();
        }

        private void simpleHtmlTextBox1_HtmlChanged(object sender, EventArgs e)
        {
            textBox1.Text = simpleHtmlTextBox1.Text;
        }
    }
}
