using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.IO;

namespace DotBase
{
    namespace Dokumenty
    {

        class ProtokolZrodlaPowierzchniowe : Wydruki
        {
            //**************************************************************************
            public ProtokolZrodlaPowierzchniowe()
            //**************************************************************************
            {
                WczytajSzablon(Narzedzia.StaleWzorcowan.stale.PROTOKOL_SKAZENIA);
            }

            //**************************************************************************
            public void PobierzDanePodstawowe(string nrKarty, string nrArkusza, DateTime data, string idZrodla)
            //**************************************************************************
            {
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!nrKarty>", nrKarty);
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!nrArkusza>", nrArkusza);
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!data>", data.ToShortDateString());
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!rok>", data.Year.ToString());

                string zapytanie;
                zapytanie = String.Format("SELECT nazwa, numer FROM Zrodla_powierzchniowe WHERE id_zrodla = {0}", idZrodla);
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!zrodlo>", _BazaDanych.TworzTabeleDanych(zapytanie).Rows[0].Field<string>(0));
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!nr_zrodla>", _BazaDanych.TworzTabeleDanych(zapytanie).Rows[0].Field<int>(1).ToString());

                _Zapytanie = String.Format("SELECT emisja_powierzchniowa, data_wzorcowania FROM Atesty_zrodel WHERE ", data.ToShortDateString())
                + String.Format("id_zrodla = {0} AND data_wzorcowania=(SELECT MAX(data_wzorcowania) FROM Atesty_zrodel WHERE id_zrodla = {0})", idZrodla);

                double emisjaPowierzchniowa = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<double>(0);
                double czas = double.Parse((data - _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<DateTime>(1)).Days.ToString());

                _Zapytanie = String.Format("SELECT czas_polowicznego_rozpadu FROM Zrodla_powierzchniowe WHERE id_zrodla = {0}", idZrodla);
                double czas_polowicznego_rozpadu = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<float>(0);

                emisjaPowierzchniowa *= Math.Exp(-czas / 365.25 * Math.Log(2.0) / czas_polowicznego_rozpadu);

                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!emisja_pow>", emisjaPowierzchniowa.ToString("0.00"));
            }

            //**************************************************************************
            public void PobierzDanePrzyrzadu(string typ, string nrFabryczny, string zakres, string inneNastawy,
                                             string sondaTyp, string sondaNrFabryczny, string napiecieZasilaniaSondy)
            //**************************************************************************
            {
                string zapytanie;
                zapytanie = String.Format("SELECT id_dozymetru FROM Dozymetry WHERE typ = '{0}' AND nr_fabryczny = '{1}'",
                                          typ, nrFabryczny);
                string id_dozymetru = _BazaDanych.TworzTabeleDanych(zapytanie).Rows[0].Field<int>(0).ToString();
                zapytanie = String.Format("SELECT id_sondy FROM Sondy WHERE typ = '{0}' AND nr_fabryczny = '{1}' AND id_dozymetru = {2}",
                                          sondaTyp, sondaNrFabryczny, id_dozymetru);
                string id_sondy = _BazaDanych.TworzTabeleDanych(zapytanie).Rows[0].Field<int>(0).ToString();

                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!dozymetr_id>", id_dozymetru);
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!sonda_id>", id_sondy);
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!dozymetr_typ>", typ);
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!dozymetr_nrFab>", nrFabryczny);
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!inne_nastawy>", inneNastawy);
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!sonda_typ>", sondaTyp);
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!sonda_nrFab>", sondaNrFabryczny);
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!napiecie_zas_sondy>", napiecieZasilaniaSondy);
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!zakres_pomiarowy>", zakres);
            }

            //**************************************************************************
            public void PobierzDaneWarunkow(string cisnienie, string temperatura, string wilgotnosc, string uwagi,
                                            string podstawka, string odlegl_zr_sonda, string wsp_korekcyjny)
            //**************************************************************************
            {
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!cisnienie>", cisnienie);
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!temperatura>", temperatura);
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!wilgotnosc>", wilgotnosc);
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!uwagi>", uwagi);
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!podstawka>", podstawka);
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!odl_zrodlo_sonda>", odlegl_zr_sonda);
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!mnoznik_korekcyjny>", wsp_korekcyjny);
            }

            //**************************************************************************
            public void PobierzDanePomiarow(string jednostka, string uwagi, ref DataGridView tabela)
            //**************************************************************************
            {
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!jednostka>", jednostka);

                String tabelaDoWpisania = "";

                for (int i = 0; i < tabela.Rows.Count - 1; ++i)
                {
                    tabelaDoWpisania +=
                    String.Format("<tr align=\"center\" valign=\"middle\">\n<td><span>{0}</span></td>\n<td><span>{1}</span></td></tr>",
                                   tabela.Rows[i].Cells[0].Value.ToString(), tabela.Rows[i].Cells[1].Value.ToString());
                }

                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!tabela>", tabelaDoWpisania);
            }

            //**************************************************************************
            public void PobierzDaneWspolczynnikow(string wykonal, string sprawdzil, string wspol_kalibracyjny,
                                                  string niepewnosc_wspol_kalibracyjnego, string poprz_wspol_kalibracyjny)
            //**************************************************************************
            {
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!wzorcowal>", wykonal);
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!sprawdzil>", sprawdzil);

                double temp;
                string format = "G";

                if (Double.TryParse(niepewnosc_wspol_kalibracyjnego, out temp))
                {
                    format = Narzedzia.Precyzja.Ustaw(temp);
                    _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!niepewnosc>", temp.ToString(format));
                }

                if (Double.TryParse(wspol_kalibracyjny, out temp))
                {
                    _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!wspol_kalibracyjny>", temp.ToString(format));
                }

                if (Double.TryParse(poprz_wspol_kalibracyjny, out temp))
                {
                    _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!pop_wspol_kalibracyjny>", temp.ToString(format));
                }
                else
                {
                    _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!pop_wspol_kalibracyjny>", poprz_wspol_kalibracyjny);
                }

            }

            //**************************************************************************
            public override bool UtworzDokument(string sciezka)
            //**************************************************************************
            {
                ZapiszPlikWynikowy(sciezka);
                return true;
            }
        }
    }
}
