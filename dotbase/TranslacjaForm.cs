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
    public partial class TranslacjaForm : Form
    {
        private string polski;
        private string result;
        private Jezyk jezyk;

        public TranslacjaForm(string polski, Jezyk jezyk)
        {
            InitializeComponent();
            this.polski = polski.ToLower().Trim();
            this.result = this.polski;
            this.jezyk = jezyk;
            polskiBox.Text = this.polski;
            updateEnabled();
        }

        private void updateEnabled()
        {
            angielskiBox.Enabled = tlumaczRadio.Checked;
            okButton.Enabled = brakRadio.Checked || (tlumaczRadio.Checked && angielskiBox.Text.Trim() != "");
            zapamietajCheck.Enabled = okButton.Enabled;
        }

        public static string Tlumacz(string polski, Jezyk jezyk)
        {
            double tmp;
            if (jezyk == Jezyk.PL || Double.TryParse(polski.Trim().Replace(',', '.'), out tmp))
            {
                return polski;
            }
            var _Baza = new BazaDanychWrapper();
            var begin = polski.Substring(0, Math.Min(2, polski.Length));
            DataTable table = _Baza.TworzTabeleDanych("SELECT " + jezyk.ToString() + " FROM Slownik WHERE PL=?", polski.ToLower().Trim());
            if (table.Rows.Count < 1 || table.Rows[0].Field<string>(0).Trim() == "")
            {
                var form = new TranslacjaForm(polski, jezyk);
                form.ShowDialog();
                return form.getResult();
            }
            var tr = table.Rows[0].Field<string>(0);
            if (tr == "***")
            {
                return polski;
            } else if (begin.ToLower() == begin)
            {
                return tr.ToLower();
            }
            else if (begin.ToUpper() == begin)
            {
                return tr.ToUpper();
            }
            else
            {
                return tr.Substring(0, 1).ToUpper() + tr.Substring(1).ToLower();
            }
        }

        private string getResult()
        {
            return result;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            String message = "";
            String dbResult = "";
            if (brakRadio.Checked)
            {
                result = polski;
                dbResult = "***";
                message = String.Format("Czy zapisać na stałe w bazie danych, że wyrażenie\r\n\"{0}\" nigdy nie ma być tłumaczone na język {1}?", polski, jezyk.ToString());
            }
            else if (tlumaczRadio.Checked)
            {
                result = angielskiBox.Text.ToLower().Trim();
                dbResult = result;
                message = String.Format("Czy dodać na stałe do bazy danych tłumaczenie\r\nwyrażenia \"{0}\" na \"{1}\" w języku {2}?", polski, result, jezyk.ToString());
            }
            if (zapamietajCheck.Checked)
            {
                if (MyMessageBox.Show(this, message, "Tłumaczenie", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }
                var _Baza = new BazaDanychWrapper();
                DataTable table = _Baza.TworzTabeleDanych("SELECT * FROM Slownik WHERE PL=?", polski);
                var slownik = _Baza.Slownik;
                if (table.Rows.Count == 0)
                {
                    slownik = slownik.INSERT().PL(polski);
                    if (jezyk == Jezyk.EN)
                    {
                        slownik = slownik.EN(dbResult);
                    }
                }
                else
                {
                    slownik = slownik.UPDATE();
                    if (jezyk == Jezyk.EN)
                    {
                        slownik = slownik.EN(dbResult);
                    }
                    slownik = slownik.WHERE().PL(polski);
                }
                slownik.EXECUTE();
            }
            Close();
        }

        private void somethingChanged(object sender, EventArgs e)
        {
            updateEnabled();
        }
    }

}
