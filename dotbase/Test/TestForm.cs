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
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();
        }

        private void swiadHtmlVsDocxButton_Click(object sender, EventArgs e)
        {
            var test = new TestSwiadHtmlVsDocx();
            test.run();
        }
    }
}
