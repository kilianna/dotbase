using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DotBase.Logging;

namespace DotBase
{
    public partial class AccessLockerForm : Form
    {
        private Logger log = Log.create();
        private string description;

        public AccessLockerForm(string description)
        {
            this.description = description;
            log("Construct");
            InitializeComponent();
            messageText.Text += description;
            DialogResult = System.Windows.Forms.DialogResult.Retry;
        }

        private void otherButton_Click(object sender, EventArgs e)
        {
            otherMenu.Show(otherButton, new Point(0, otherButton.Height));
        }

        private void retryButton_Click(object sender, EventArgs e)
        {
            log("Retry");
            this.DialogResult = System.Windows.Forms.DialogResult.Retry;
            this.Close();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var res = MessageBox.Show(this,
                "Zamknięcie programu grozi utraceniem ostatnio wprowadzonych danych w aktualnym oknie programu.\r\n\r\n" +
                "Czy chcesz kontynuować?",
                "Ostrzeżenie", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
            if (res == System.Windows.Forms.DialogResult.Yes)
            {
                log("Abort");
                this.DialogResult = System.Windows.Forms.DialogResult.Abort;
                this.Close();
                Environment.Exit(1);
                Application.Exit();
                throw new ApplicationException("Aborted");
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            var res = MessageBox.Show(this,
                "Zignorowanie problemu grozi pojawieniem się trudnych do naprawienia błędów w bazie danych.\r\n\r\n" +
                "Czy chcesz kontynuować?",
                "Ostrzeżenie", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
            if (res == System.Windows.Forms.DialogResult.Yes)
            {
                log("Ignore");
                this.DialogResult = System.Windows.Forms.DialogResult.Ignore;
                this.Close();
            }
        }
    }
}
