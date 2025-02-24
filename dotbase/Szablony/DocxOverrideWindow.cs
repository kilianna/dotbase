using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace DotBase.Szablony
{
    public partial class DocxOverrideWindow : Form
    {
        private string[] allFiles;

        public DocxOverrideWindow(string file, string altername, string[] allFiles)
        {
            InitializeComponent();
            file = Path.GetFullPath(file);
            komunikat.Text = String.Format(komunikat.Text, Path.GetFileName(file), Path.GetDirectoryName(file));
            nowaNazwa.Text = String.Format(nowaNazwa.Text, altername);
            this.allFiles = allFiles;
            foreach (var existsingFile in allFiles)
            {
                var name = Path.GetFileName(existsingFile);
                var fileToOpen = existsingFile;
                existingMenu.Items.Add(name.Replace("&", "&&"), null, (sender, args) =>
                {
                    preview(fileToOpen);
                });
            }
        }

        private void preview(string file)
        {
            var proc = new Process();
            proc.StartInfo.FileName = file;
            proc.StartInfo.UseShellExecute = true;
            proc.Start();
            proc.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (allFiles.Length == 1)
            {
                preview(allFiles[0]);
            }
            else
            {
                existingMenu.Show(button1, 0, button1.Height);
            }
        }

    }
}
