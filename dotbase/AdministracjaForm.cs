using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DotBase.Login;

namespace DotBase
{
    public partial class AdministracjaForm : Form
    {
        public UsersManager Users { get; private set; }

        public AdministracjaForm(UsersManager users)
        {
            Users = users.Clone();
            InitializeComponent();
        }

        private void AdministracjaForm_Load(object sender, EventArgs e)
        {
            foreach (var user in Users.Users)
            {
                int i = listaView.Rows.Add(user.Name, "Zmień hasło", user.IsAdmin);
                listaView.Rows[i].Cells[2].ReadOnly = (user.Name == LogowanieForm.Instancja.Wybrany.Name);
                var r = listaView.Rows[i];
            }
        }

        private void listaView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= Users.Users.Length || e.RowIndex < 0) return;
            if (e.ColumnIndex == 1)
            {
                var form = new HasloForm();
                form.zmienUzytkownika(listaView.Rows[e.RowIndex].Cells[0].Value.ToString());
                if (form.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    Users.ChangeUserPassword(Users.Users[e.RowIndex].Name, form.Haslo);
                }
            }
        }

        private void listaView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > Users.Users.Length || e.RowIndex < 0) return;

            if (e.RowIndex == Users.Users.Length)
            {
                var nowy = new User();
                nowy.Name = listaView.Rows[e.RowIndex].Cells[0].Value != null ? listaView.Rows[e.RowIndex].Cells[0].Value.ToString().Trim().ToLower() : "";
                nowy.ChangeUserPassword(losujHaslo(), Users.DatabasePassword);
                nowy.IsAdmin = listaView.Rows[e.RowIndex].Cells[2].Value != null ? (bool)listaView.Rows[e.RowIndex].Cells[2].Value : false;
                listaView.Rows[e.RowIndex].Cells[1].Value = "Zmień hasło";
                var tmp = new User[Users.Users.Length + 1];
                Users.Users.CopyTo(tmp, 0);
                tmp[Users.Users.Length] = nowy;
                Users.Users = tmp;
            }

            if (e.ColumnIndex == 2)
            {
                Users.Users[e.RowIndex].IsAdmin = (bool)listaView.Rows[e.RowIndex].Cells[2].Value;
            }
            else if (e.ColumnIndex == 0)
            {
                Users.Users[e.RowIndex].Name = listaView.Rows[e.RowIndex].Cells[0].Value.ToString().Trim().ToLower();
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
            var arr = new byte[32];
            N.rng.GetBytes(arr);
            string str = "";
            for (var i = 0; i < 32; i++)
                str += 'a' + (char)(arr[i] % 26);
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
            if (index < 0 || index >= Users.Users.Length) return;
            if (Users.Users[index].Name == LogowanieForm.Instancja.Wybrany.Name) return;
            if (MessageBox.Show(this, "Czy usunąć użytkownika: " + Users.Users[index].Name + "?", "Usuń", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                listaView.Rows.RemoveAt(index);
                Users.Users = Users.Users.Take(index).Concat(Users.Users.Skip(index + 1)).ToArray();
            }
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            HashSet<string> unique = new HashSet<string>();
            foreach (var usr in Users.Users)
            {
                if (unique.Contains(usr.Name))
                {
                    MessageBox.Show(this, "Tabela zawieraz zduplikowane nazwy użytkowników!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                unique.Add(usr.Name);
            }
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Zmiana hasła wpływa na całą bazę danych.\r\n\r\n" +
                "Zanim to zrobisz upewnij się, że:\r\n"+
                "1. Utworzyłeś kopię zapasową bazy danych.\r\n"+
                "2. Otworzyłeś bazę danych z lokalnego dysku (nie dysku sieciowego).\r\n" +
                "3. Żaden inny program nie kożysta z tego pliku bazy danych.\r\n\r\n" +
                "Czy checesz zmienić teraz hasło?",
                "Informacja", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            var form = new HasloForm();
            form.zmienWlasne("BAZA DANYCH", "");
            if (form.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                Users.DatabasePassword = form.Haslo;
            }
        }

    }
}
