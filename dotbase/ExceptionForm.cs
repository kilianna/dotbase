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
    public partial class ExceptionForm : Form
    {
        public static bool _noScreeshot;

        public ExceptionForm(string description)
        {
            InitializeComponent();
            messageText.Text += description;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int pos = messageText.Text.IndexOf("problemu:");
            if (pos > 0)
            {
                pos += 9;
                while (pos < messageText.Text.Length && messageText.Text[pos] <= ' ')
                {
                    pos++;
                }
                messageText.Select(pos, messageText.Text.Length - pos);
            }
            else
            {
                messageText.SelectAll();
            }
            messageText.Copy();
            messageText.Focus();
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
