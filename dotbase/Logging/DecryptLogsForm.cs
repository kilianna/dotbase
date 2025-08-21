using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DotBase.Logging
{
    public partial class DecryptLogsForm : Form
    {
        private string defaultPassword = "";
        private StringBuilder info = new StringBuilder();
        private List<string> files = new List<string>();
        private int dirs;
        private Dictionary<string, List<string>> problems = new Dictionary<string, List<string>>();

        public DecryptLogsForm(string defaultPassword)
        {
            this.defaultPassword = defaultPassword;
            InitializeComponent();
        }

        private void wybierzBtn_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.SelectedPath = katalogText.Text.Trim();
            var res = folderBrowserDialog.ShowDialog(this);
            if (res != System.Windows.Forms.DialogResult.OK) return;
            katalogText.Text = folderBrowserDialog.SelectedPath;
        }

        private void DecryptLogsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        private void odszyfrujBtn_Click(object sender, EventArgs e)
        {
            string password = passwordBox.Text == "" ? defaultPassword : passwordBox.Text;
            info.Clear();
            files.Clear();
            problems.Clear();
            dirs = 0;
            listDirectory(katalogText.Text.Trim());
            info.AppendFormat("Found {0} files in {1} directories.\r\n", files.Count, dirs);
            foreach (var file in files) {
                try {
                    EncryptedLogger.decodeFile(file, Path.ChangeExtension(file, ""), password);
                } catch (Exception ex) {
                    string msg = ex.Message.Replace(file, "???");
                    if (!problems.ContainsKey(msg)) problems[msg] = new List<string>();
                    problems[msg].Add(file);
                }
            }
            info.Append("------------------------------------------------\r\n");
            foreach (var problem in problems) {
                info.AppendFormat("Error ({0} files): {1}\r\n", problem.Value.Count, problem.Key);
            }
            info.Append("------------------------------------------------\r\n");
            foreach (var problem in problems) {
                info.AppendFormat("Error: {0}\r\n", problem.Key);
                foreach (var file in problem.Value) {
                    info.AppendFormat("    {0}\r\n", file);
                }
            }
            resultsBox.Text = info.ToString();
        }

        private void listDirectory(string path)
        {
            dirs++;
            foreach (var dir in Directory.GetDirectories(path)) {
                listDirectory(dir);
            }
            foreach (var file in Directory.GetFiles(path, "*.enclog")) {
                files.Add(file);
            }
        }
    }
}
