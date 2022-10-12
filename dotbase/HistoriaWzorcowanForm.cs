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
    public partial class HistoriaWzorcowanForm : Form
    {
        BazaDanychWrapper m_BazaDanych;
        String m_Zapytanie;

        //-------------------------------------------------------------------
        public HistoriaWzorcowanForm()
        //-------------------------------------------------------------------
        {
            InitializeComponent();
            m_BazaDanych = new BazaDanychWrapper();
            SzukajMozliweTypy();
        }

        //-------------------------------------------------------------------
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        //-------------------------------------------------------------------
        {
            comboBox2.Items.Clear();

            m_Zapytanie = String.Format("SELECT nr_fabryczny FROM Dozymetry WHERE typ='{0}' ORDER BY nr_fabryczny", comboBox1.Text);

            DataTable tabela = m_BazaDanych.TworzTabeleDanych(m_Zapytanie);

            if (null == tabela || null == tabela.Rows || 0 == tabela.Rows.Count)
                return;

            foreach (DataRow wiersz in tabela.Rows)
            {
                comboBox2.Items.Add(wiersz.Field<String>("nr_fabryczny"));
            }
        }

        // Wyszukanie wszystkich zleceniodawców w bazie danych. Metoda wywoływana tylko raz na początku przy włączeniu okna
        // Dane znalezione przez nią są porównywane z wzorcami otrzywanymi od użytkownika
        //-------------------------------------------------------------------
        private void SzukajMozliweTypy()
        //-------------------------------------------------------------------
        {
            m_Zapytanie = "SELECT DISTINCT typ FROM Dozymetry";

            List<String> typy = new List<string>();

            foreach (DataRow wiersz in m_BazaDanych.TworzTabeleDanych(m_Zapytanie).Rows)
                typy.Add(wiersz.Field<String>("typ"));

            typy.Sort();

            comboBox1.Items.AddRange(typy.ToArray());
            comboBox4.Items.AddRange(typy.ToArray());
        }

        //-------------------------------------------------------------------
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        //-------------------------------------------------------------------
        {
            dataGridView1.Rows.Clear();

            m_Zapytanie = "SELECT id_karty, rok FROM Karta_przyjecia WHERE id_dozymetru IN (SELECT id_dozymetru FROM Dozymetry "
                            + String.Format("WHERE typ='{0}' AND nr_fabryczny='{1}')", comboBox1.Text, comboBox2.Text);

            List<Tuple<int, int>> daneCzesciowe = new List<Tuple<int, int>>();

            foreach (DataRow wiersz in m_BazaDanych.TworzTabeleDanych(m_Zapytanie).Rows)
            {
                daneCzesciowe.Add(new Tuple<int, int>(wiersz.Field<int>(0), wiersz.Field<int>(1)));
            }

            // jeśli wybrano dawkę
            if (radioButton1.Checked)
            {
                for (int i = 0; i < daneCzesciowe.Count; ++i)
                {
                    m_Zapytanie = "SELECT zakres, wspolczynnik, niepewnosc FROM Wyniki_dawka WHERE id_wzorcowania = (SELECT "
                                + "id_wzorcowania FROM Wzorcowanie_cezem AS WC INNER JOIN Karta_przyjecia AS KP ON WC.id_karty = KP.id_karty WHERE "
                                + String.Format("KP.id_karty = {0} AND Rodzaj_Wzorcowania = 'd')", daneCzesciowe[i].Item1);

                    if (false == m_BazaDanych.TworzTabeleDanychWPamieci(m_Zapytanie))
                        break;

                    if (m_BazaDanych.Tabela.Rows.Count != 1)
                        continue;

                    for (int k = 0; k < m_BazaDanych.Tabela.Rows.Count; ++k)
                    {
                        DataRow odpowiedz = m_BazaDanych.Tabela.Rows[k];
                        dataGridView1.Rows.Add(odpowiedz.Field<double>(0), daneCzesciowe[i].Item1, daneCzesciowe[i].Item2, odpowiedz.Field<double>(1), odpowiedz.Field<double>(2));
                    }
                }
            }
            else
            {
                for (int i = 0; i < daneCzesciowe.Count; ++i)
                {
                    m_Zapytanie = "SELECT zakres, wspolczynnik, niepewnosc FROM Wyniki_moc_dawki WHERE id_wzorcowania = (SELECT "
                                + "id_wzorcowania FROM Wzorcowanie_cezem AS WC INNER JOIN Karta_przyjecia AS KP ON WC.id_karty = KP.id_karty WHERE "
                                + String.Format("KP.id_karty = {0} AND Rodzaj_Wzorcowania = 'md')", daneCzesciowe[i].Item1);

                    if (false == m_BazaDanych.TworzTabeleDanychWPamieci(m_Zapytanie))
                        break;

                    for (int k = 0; k < m_BazaDanych.Tabela.Rows.Count; ++k)
                    {
                        DataRow odpowiedz = m_BazaDanych.Tabela.Rows[k];
                        dataGridView1.Rows.Add(odpowiedz.Field<double>(0), daneCzesciowe[i].Item1, daneCzesciowe[i].Item2, odpowiedz.Field<double>(1), odpowiedz.Field<double>(2));
                    }
                }
            }
        }

        //-------------------------------------------------------------------
        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        //-------------------------------------------------------------------
        {
            comboBox3.Items.Clear();

            m_Zapytanie = String.Format("SELECT nr_fabryczny FROM Dozymetry WHERE typ='{0}' ORDER BY nr_fabryczny", comboBox4.Text);

            DataTable tabela = m_BazaDanych.TworzTabeleDanych(m_Zapytanie);

            if (null == tabela || null == tabela.Rows || 0 == tabela.Rows.Count)
                return;

            foreach (DataRow wiersz in tabela.Rows)
            {
                comboBox3.Items.Add(wiersz.Field<String>("nr_fabryczny"));
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView2.Rows.Clear();

            m_Zapytanie = "SELECT id_karty, rok FROM Karta_przyjecia WHERE id_dozymetru IN (SELECT id_dozymetru FROM Dozymetry "
                            + String.Format("WHERE typ='{0}' AND nr_fabryczny='{1}')", comboBox4.Text, comboBox3.Text);

            List<Tuple<int, int>> daneCzesciowe = new List<Tuple<int, int>>();

            foreach (DataRow wiersz in m_BazaDanych.TworzTabeleDanych(m_Zapytanie).Rows)
            {
                daneCzesciowe.Add(new Tuple<int, int>(wiersz.Field<int>(0), wiersz.Field<int>(1)));
            }

            for (int i = 0; i < daneCzesciowe.Count; ++i)
            {
                m_Zapytanie = "SELECT id_zrodla, wspolczynnik, niepewnosc FROM Wzorcowanie_zrodlami_powierzchniowymi WHERE id_wzorcowania = (SELECT "
                            + "id_wzorcowania FROM Wzorcowanie_zrodlami_powierzchniowymi AS WC INNER JOIN Karta_przyjecia AS KP ON WC.id_karty = KP.id_karty WHERE "
                            + String.Format("KP.id_karty = {0})", daneCzesciowe[i].Item1);

                if (false == m_BazaDanych.TworzTabeleDanychWPamieci(m_Zapytanie))
                    break;

                if (m_BazaDanych.Tabela.Rows.Count != 1)
                    continue;

                for (int k = 0; k < m_BazaDanych.Tabela.Rows.Count; ++k)
                {
                    DataRow odpowiedz = m_BazaDanych.Tabela.Rows[k];
                    dataGridView2.Rows.Add(odpowiedz.Field<int>(0), daneCzesciowe[i].Item1, daneCzesciowe[i].Item2, odpowiedz.Field<double>(1), odpowiedz.Field<double>(2));
                }
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
        /*
//-------------------------------------------------------------------
void HistoriaWzorcowanCez::WyswietlDawka(const int & gdzie)
//-------------------------------------------------------------------
{
	tabela->SetCellValue(gdzie, 0, wxT("Nie dotyczy"));
	
	if( true == DbWsk->GetNext())
	{
		DbWsk->GetData(2, SQL_C_DOUBLE, &m_oPrzetwarzanie.temp_d, 0, &m_oPrzetwarzanie.cd);
		m_oPrzetwarzanie.tekst.Printf(wxT("%.4f"), m_oPrzetwarzanie.temp_d);
		tabela->SetCellValue(gdzie, 3, m_oPrzetwarzanie.tekst);
	
		DbWsk->GetData(3, SQL_C_DOUBLE, &m_oPrzetwarzanie.temp_d, 0, &m_oPrzetwarzanie.cd);
		m_oPrzetwarzanie.tekst.Printf(wxT("%.4f"), m_oPrzetwarzanie.temp_d);
		tabela->SetCellValue(gdzie, 4, m_oPrzetwarzanie.tekst);
	}
}

//-------------------------------------------------------------------
void HistoriaWzorcowanCez::WyswietlMoc(const int & gdzie)
//-------------------------------------------------------------------
{
	DbWsk->GetData(1, SQL_C_DOUBLE, &m_oPrzetwarzanie.temp_d, 0, &m_oPrzetwarzanie.cd);
	m_oPrzetwarzanie.tekst.Printf(wxT("%.4f"), m_oPrzetwarzanie.temp_d);
	tabela->SetCellValue(gdzie, 0, m_oPrzetwarzanie.tekst);
	
	DbWsk->GetData(2, SQL_C_DOUBLE, &m_oPrzetwarzanie.temp_d, 0, &m_oPrzetwarzanie.cd);
	m_oPrzetwarzanie.tekst.Printf(wxT("%.4f"), m_oPrzetwarzanie.temp_d);
	tabela->SetCellValue(gdzie, 3, m_oPrzetwarzanie.tekst);
	
	DbWsk->GetData(3, SQL_C_DOUBLE, &m_oPrzetwarzanie.temp_d, 0, &m_oPrzetwarzanie.cd);
	m_oPrzetwarzanie.tekst.Printf(wxT("%.4f"), m_oPrzetwarzanie.temp_d);
	tabela->SetCellValue(gdzie, 4, m_oPrzetwarzanie.tekst);
}

}*/
