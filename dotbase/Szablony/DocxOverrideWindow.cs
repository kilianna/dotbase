using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DotBase.Szablony
{
    public partial class DocxOverrideWindow : Form
    {
        public DocxOverrideWindow(string file, string altername)
        {
            InitializeComponent();
            komunikat.Text = String.Format(komunikat.Text, file);
            nowaNazwa.Text = String.Format(nowaNazwa.Text, altername);
        }

    }
}
