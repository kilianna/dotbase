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
    public partial class MenuBiuroForm : Form
    {
        public static bool _noScreeshot;

        ZlecenieForm        _ZlecenieForm;
        KartaPrzyjeciaForm  _KartaPrzyjeciaForm;
        CennikForm          _CennikForm;
        RejestrZlecenForm   _RejestrZlecenForm;

        //--------------------------------------------------------------------
        public MenuBiuroForm()
        //--------------------------------------------------------------------
        {
            InitializeComponent();
        }

        //--------------------------------------------------------------------
        private void AktywujPrzycisk(object sender, EventArgs e)
        //--------------------------------------------------------------------
        {
            if (sender.Equals(_ZlecenieForm))
                button1.Enabled = true;
            else if (sender.Equals(_KartaPrzyjeciaForm))
                button2.Enabled = true;
            else if (sender.Equals(_CennikForm))
                button3.Enabled = true;
            else if (sender.Equals(_RejestrZlecenForm))
                button4.Enabled = true;
        }

        //--------------------------------------------------------------------
        // otworzenie zlecenia
        private void button1_Click(object sender, EventArgs e)
        //--------------------------------------------------------------------
        {
            button1.Enabled = false;
            _ZlecenieForm = new ZlecenieForm();
            _ZlecenieForm.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AktywujPrzycisk);
            _ZlecenieForm.Show();
        }

        //--------------------------------------------------------------------
        // otworzenie karty przyjęcia
        private void button2_Click(object sender, EventArgs e)
        //--------------------------------------------------------------------
        {
            button2.Enabled = false;
            _KartaPrzyjeciaForm = new KartaPrzyjeciaForm(true);
            _KartaPrzyjeciaForm.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AktywujPrzycisk);

            if (false == _KartaPrzyjeciaForm.InicjalizujTrybPrzegladaniaOrazEdycji())
            {
                MyMessageBox.Show("Brak danych lub połaczenie z bazą zostało przerwane.");
                return;
            }
            _KartaPrzyjeciaForm.WyswietlDanePoczatkowe();
            _KartaPrzyjeciaForm.Show();
        }

        //--------------------------------------------------------------------
        // cennik
        private void button3_Click(object sender, EventArgs e)
        //--------------------------------------------------------------------
        {
            _CennikForm = new CennikForm();
            _CennikForm.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AktywujPrzycisk);

            if (false == _CennikForm.Inicjalizuj())
            {
                MyMessageBox.Show("Nie udało sie załadować danych z bazy. Prawdopodobnie połączenie zostało przerwane.");
                _CennikForm.Close();
                return;
            }

            button3.Enabled = false;
            _CennikForm.Show();
        }

        //--------------------------------------------------------------------
        // rejestr zleceń
        private void button4_Click(object sender, EventArgs e)
        //--------------------------------------------------------------------
        {
            _RejestrZlecenForm = new RejestrZlecenForm();
            _RejestrZlecenForm.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AktywujPrzycisk);

            if (false == _RejestrZlecenForm.Inicjalizuj())
            {
                MyMessageBox.Show("Nie udało sie załadować danych z bazy. Prawdopodobnie połączenie zostało przerwane.");
                _RejestrZlecenForm.Close();
                return;
            }

            button4.Enabled = false;
            _RejestrZlecenForm.Show();
        }

        //--------------------------------------------------------------------
        // zakończenie
        private void button5_Click(object sender, EventArgs e)
        //--------------------------------------------------------------------
        {
            Close();
        }

    }
}
