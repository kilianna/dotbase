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
        public UserInfo[] list;

        public AdministracjaForm(UsersManager users)
        {
            Users = users.Clone();
            list = users.GetUsers();
            InitializeComponent();
        }

        private void AdministracjaForm_Load(object sender, EventArgs e)
        {
            zaladujTabele();
        }

        private void zaladujTabele()
        {
            listaView.Rows.Clear();
            list = Users.GetUsers();
            foreach (var user in list)
            {
                int i = listaView.Rows.Add(user.Name, "Zmień hasło", user.IsAdmin);
                var row = listaView.Rows[i];
                row.Cells[2].ReadOnly = (Users.CurrentUser == user);
                row.Tag = user;
            }
        }

        private void listaView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= list.Length || e.RowIndex < 0) return;
            if (e.ColumnIndex == 1)
            {
                var form = new HasloForm();
                var row = listaView.Rows[e.RowIndex];
                var user = row.Tag as UserInfo;
                form.zmienUzytkownika(user.Name);
                if (form.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    Users.ChangeUserPassword(user.Name, form.Haslo);
                }
            }
        }

        private void listaView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > list.Length || e.RowIndex < 0) return;

            var row = listaView.Rows[e.RowIndex];

            if (e.RowIndex == list.Length)
            {
                var nowy = Users.NewUser((string)row.Cells[0].Value ?? "", losujHaslo(), (bool)(row.Cells[2].Value ?? false));
                row.Tag = nowy;
                list = Users.GetUsers();
                row.Cells[1].Value = "Zmień hasło";
            }

            if (e.ColumnIndex == 2)
            {
                (row.Tag as UserInfo).IsAdmin = (bool)(row.Cells[2].Value ?? false);
            }
            else if (e.ColumnIndex == 0)
            {
                (row.Tag as UserInfo).Name = ((string)row.Cells[0].Value ?? "").Trim().ToLower();
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
                str += (char)('a' + (arr[i] % 26));
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
            if (index < 0 || index >= list.Length) return;
            if (list[index] == Users.CurrentUser) return;
            if (MessageBox.Show(this, "Czy usunąć użytkownika: " + list[index].Name + "?", "Usuń", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                Users.RemoveUser(list[index]);
                zaladujTabele();
            }
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            HashSet<string> unique = new HashSet<string>();
            foreach (var usr in list)
            {
                if (unique.Contains(usr.Name))
                {
                    MessageBox.Show(this, "Tabela zawiera zdublowane nazwy użytkowników!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (usr.Name == "")
                {
                    MessageBox.Show(this, "Tabela zawiera puste nazwy użytkowników!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                unique.Add(usr.Name);
            }
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var form = new HasloForm();
            form.zmienInne("BAZA DANYCH");
            if (form.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                Users.ChangeDatabasePassword(form.Haslo);
            }
        }

    }
}
