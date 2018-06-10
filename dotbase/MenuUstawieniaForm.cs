using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace DotBase
{
    public partial class MenuUstawieniaForm : Form
    {
        //---------------------------------------------------------
        public MenuUstawieniaForm()
        //---------------------------------------------------------
        {
            InitializeComponent();
            
        }

        //---------------------------------------------------------
        // Budzet niepewnosc
        private void button1_Click(object sender, EventArgs e)
        //---------------------------------------------------------
        {
            BudzetNiepewnosciForm okno = new BudzetNiepewnosciForm();

            if (false == okno.Inicjalizuj())
            {
                MessageBox.Show("Brak odpowiednich danych w bazie do wyświetlenia.");
            }

            okno.ShowDialog();
        }

        //---------------------------------------------------------
        // protokoły kalibracyjne ławy
        private void button2_Click(object sender, EventArgs e)
        //---------------------------------------------------------
        {
            ProtokolyKalibracyjneForm okno = new ProtokolyKalibracyjneForm();
            
            if (false == okno.Inicjalizuj())
            {
                MessageBox.Show("Brak odpowiednich danych w bazie do wyświetlenia.");
            }

            okno.ShowDialog();
        }

        //---------------------------------------------------------
        // wzorcowanie źródeł
        private void button3_Click(object sender, EventArgs e)
        //---------------------------------------------------------
        {
            WzorcowanieZrodelForm okno = new WzorcowanieZrodelForm();
            
            if (false == okno.Inicjalizuj())
            {
                MessageBox.Show("Brak odpowiednich danych w bazie do wyświetlenia.");
            }

            okno.ShowDialog();
        }

        //---------------------------------------------------------
        // stałe
        private void button4_Click(object sender, EventArgs e)
        //---------------------------------------------------------
        {
            StaleForm okno = new StaleForm();

            if (false == okno.Inicjalizuj())
            {
                MessageBox.Show("Brak odpowiednich danych w bazie do wyświetlenia.");
            }

            okno.ShowDialog();
        }

        //---------------------------------------------------------
        private void button5_Click(object sender, EventArgs e)
        //---------------------------------------------------------
        {
            Close();
        }

        private void hasloBtn_Click(object sender, EventArgs e)
        {
            var form = new HasloForm();
            if (form.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                LogowanieForm.Instancja.zmienHaslo(form.Nazwa, form.Haslo);
            }
        }

        private void eksportujBtn_Click(object sender, EventArgs e)
        {
            if (eksportujDlg.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                var form = new HasloForm();
                form.zmienWlasne("NOWA BAZA DANYCH", null);
                if (form.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    if (!BazaDanychWrapper.Eksportuj(LogowanieForm.Instancja.PlikBazy, LogowanieForm.Instancja.hasloBazy, eksportujDlg.FileName, form.Haslo, new BazaDanychWrapper.TransformujBazeDelegate(UsunDanePersonalne)))
                    {
                        MessageBox.Show(this, "Nie udało się wyeskportować bazy danych.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void UsunDanePersonalne(BazaDanychWrapper baza)
        {
            baza.WykonajPolecenie("UPDATE Zleceniodawca SET Osoba_kontaktowa='Xxxx Yyyy', Telefon='000 000 000', Faks='000 000 000', email='xxxx.yyyy@zzzz.ww'");
        }

    }
}
