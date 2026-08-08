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
    partial class TranslacjaForm : Form
    {
        private string polski;
        private string result;
        private Jezyk jezyk;
        Translacja translacja;

        public TranslacjaForm(string polski, Jezyk jezyk, Translacja translacja)
        {
            InitializeComponent();
            this.polski = polski.ToLower().Trim();
            this.result = this.polski;
            this.jezyk = jezyk;
            this.translacja = translacja;
            polskiBox.Text = this.polski;
            updateEnabled();
        }

        private void updateEnabled()
        {
            angielskiBox.Enabled = tlumaczRadio.Checked;
            okButton.Enabled = brakRadio.Checked || (tlumaczRadio.Checked && angielskiBox.Text.Trim() != "");
            zapamietajCheck.Enabled = okButton.Enabled;
        }

        public string getResult()
        {
            return result;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            String message = "";
            if (brakRadio.Checked)
            {
                result = polski;
                message = String.Format("Czy zapisać na stałe w bazie danych, że wyrażenie\r\n\"{0}\" nigdy nie ma być tłumaczone na język {1}?", polski, jezyk.ToString());
            }
            else if (tlumaczRadio.Checked)
            {
                result = angielskiBox.Text.Trim();
                message = String.Format("Czy dodać na stałe do bazy danych tłumaczenie\r\nwyrażenia \"{0}\" na \"{1}\" w języku {2}?", polski, result, jezyk.ToString());
            }
            if (zapamietajCheck.Checked)
            {
                if (MyMessageBox.Show(this, message, "Tłumaczenie", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }
            }
            translacja.AddTranslation(polski, result, zapamietajCheck.Checked);
            Close();
        }

        private void somethingChanged(object sender, EventArgs e)
        {
            updateEnabled();
        }
    }

}
