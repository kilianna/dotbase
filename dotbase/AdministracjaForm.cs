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
    public partial class AdministracjaForm : Form
    {
        public AdministracjaForm()
        {
            InitializeComponent();
        }

        LogowanieForm.Uzytkownik[] uzytkownicy = new LogowanieForm.Uzytkownik[0];
        string hasloBazy = "";
        private string stareHasloBazy = "";

        private void AdministracjaForm_Load(object sender, EventArgs e)
        {
            uzytkownicy = new LogowanieForm.Uzytkownik[LogowanieForm.Instancja.uzytkownicy.Length];
            LogowanieForm.Instancja.uzytkownicy.CopyTo(uzytkownicy, 0);
            foreach (var usr in uzytkownicy)
            {
                int i = listaView.Rows.Add(usr.nazwa.ToLower().Trim(), "Zmień hasło", usr.admin);
                listaView.Rows[i].Cells[2].ReadOnly = (usr.nazwa == LogowanieForm.Instancja.Wybrany.nazwa);
                var r = listaView.Rows[i];
            }
            hasloBazy = LogowanieForm.Instancja.hasloBazy;
        }

        private void listaView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= uzytkownicy.Length || e.RowIndex < 0) return;
            if (e.ColumnIndex == 1)
            {
                var form = new HasloForm();
                form.zmienUzytkownika(listaView.Rows[e.RowIndex].Cells[0].Value.ToString());
                if (form.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    uzytkownicy[e.RowIndex].haslo = form.Haslo;
                }
            }
        }

        private void listaView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > uzytkownicy.Length || e.RowIndex < 0) return;

            if (e.RowIndex == uzytkownicy.Length)
            {
                LogowanieForm.Uzytkownik[] nowi = new LogowanieForm.Uzytkownik[uzytkownicy.Length + 1];
                uzytkownicy.CopyTo(nowi, 0);
                nowi[uzytkownicy.Length].nazwa = listaView.Rows[uzytkownicy.Length].Cells[0].Value != null ? listaView.Rows[uzytkownicy.Length].Cells[0].Value.ToString().Trim().ToLower() : "";
                nowi[uzytkownicy.Length].haslo = losujHaslo();
                nowi[uzytkownicy.Length].admin = listaView.Rows[uzytkownicy.Length].Cells[2].Value != null ? (bool)listaView.Rows[uzytkownicy.Length].Cells[2].Value : false;
                listaView.Rows[e.RowIndex].Cells[1].Value = "Zmień hasło";
                uzytkownicy = nowi;
            }

            if (e.ColumnIndex == 2)
            {
                uzytkownicy[e.RowIndex].admin = (bool)listaView.Rows[e.RowIndex].Cells[2].Value;
            }
            else if (e.ColumnIndex == 0)
            {
                uzytkownicy[e.RowIndex].nazwa = listaView.Rows[e.RowIndex].Cells[0].Value.ToString().Trim().ToLower();
            }
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {

        }

        private void listaView_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
        }

        private string losujHaslo()
        {
            var rnd = new Random();
            string str = "";
            for (var i = 0; i < 32; i++)
            {
                str += (char)rnd.Next((int)'a', (int)'z');
            }
            return str;
        }

        private void usunBtn_Click(object sender, EventArgs e)
        {
            int index = -1;
            for (var i = 0; i < listaView.SelectedCells.Count; i++)
            {
                if (index >= 0 && index != listaView.SelectedCells[i].RowIndex)
                {
                    MessageBox.Show(this, "Tylko jeden użytkownik na raz moży być usunięty!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                index = listaView.SelectedCells[i].RowIndex;
            }
            if (index < 0 || index >= uzytkownicy.Length) return;
            if (uzytkownicy[index].nazwa.ToLower().Trim() == LogowanieForm.Instancja.Wybrany.nazwa.ToLower().Trim()) return;
            if (MessageBox.Show(this, "Czy usunąć użytkownika: " + uzytkownicy[index].nazwa + "?", "Usuń", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                listaView.Rows.RemoveAt(index);
                uzytkownicy = uzytkownicy.Take(index).Concat(uzytkownicy.Skip(index + 1)).ToArray();
            }
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            HashSet<string> unique = new HashSet<string>();
            foreach (var usr in uzytkownicy)
            {
                if (unique.Contains(usr.nazwa))
                {
                    MessageBox.Show(this, "Tabela zawieraz zduplikowane nazwy użytkowników!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                unique.Add(usr.nazwa);
            }
            if (LogowanieForm.Instancja.hasloBazy != hasloBazy)
            {
                if (!BazaDanychWrapper.ZmienHaslo(LogowanieForm.Instancja.PlikBazy, hasloBazy, stareHasloBazy))
                {
                    if (MessageBox.Show(this, "Nie udało się zmienić hasła w pliku bazy!\r\nCzy zmienić je tylko w pliku użytkowników?", "Błąd", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == System.Windows.Forms.DialogResult.No)
                    {
                        this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                        return;
                    }
                }
            }
            LogowanieForm.Instancja.hasloBazy = hasloBazy;
            LogowanieForm.Instancja.uzytkownicy = uzytkownicy;
            LogowanieForm.Instancja.zapiszUzytkownikow();
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var form = new HasloForm();
            form.zmienWlasne("[BAZA DANYCH]", "");
            if (form.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                hasloBazy = form.Haslo;
                stareHasloBazy = form.AktHaslo;
            }
        }

    }
}
