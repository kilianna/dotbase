using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace DotBase.Szablony
{
    public partial class DocxOverrideWindow : Form
    {
        public DocxOverrideWindow(string file, string altername)
        {
            InitializeComponent();
            file = Path.GetFullPath(file);
            komunikat.Text = String.Format(komunikat.Text, Path.GetFileName(file), Path.GetDirectoryName(file));
            nowaNazwa.Text = String.Format(nowaNazwa.Text, altername);
        }

    }
}
