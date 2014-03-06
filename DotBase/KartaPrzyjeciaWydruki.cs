using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Narzedzia;

namespace DotBase
{
    namespace Dokumenty
    {
        class KartaPrzyjecia : Wydruki
        {
            private enum stale
            {
                ADRES, AKCESORIA,
                DATA_PRZYJECIA, DATA_ZWROTU, DOZYMETR_ID, DOZYMETR_NR_FABRYCZNY, DOZYMETR_TYP,
                EKSPRES, EMAIL,
                FAKS,
                ID_KARTY, ID_ZLECENIA, ID_ZLECENIODAWCY,
                OSOBA_KONTAKTOWA, OSOBA_PRZYJMUJACA,
                ROK,
                SONDA_ID, SONDA_NR_FABRYCZNY, SONDA_TYP,
                TABELA, TELEFON, TEST_NA_SKAZENIA,
                UWAGI,
                WYMAGANIA,
                ZLECENIDOAWCA,
                MAX_ELEMENTOW
            };

            //****************************************************************************************
            public KartaPrzyjecia()
            //****************************************************************************************
            {
                InicjalizujListeDanychWypelniajacych((int)stale.MAX_ELEMENTOW);
            }

            //****************************************************************************************
            public void UtworzDokument(string nrKarty, string sciezka)
            //****************************************************************************************
            {
                _DaneWypelniajace[(int)stale.ID_KARTY] = _NrKarty = nrKarty;

                _Zapytanie = String.Format("SELECT id_zlecenia FROM Karta_przyjecia WHERE id_karty = {0}", _NrKarty);
                _DaneWypelniajace[(int)stale.ID_ZLECENIA] = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<int>(0).ToString();

                UtworzDokument(sciezka);
            }

            //****************************************************************************************
            override public bool UtworzDokument(string sciezka)
            //****************************************************************************************
            {
                WczytajSzablon(StaleWzorcowan.stale.KARTA_PRZYJECIA);
                PobierzRok();
                PobierzDaneZleceniodawcy();
                PobierzDaneDozymetru();
                PobierzDaneSondy();
                PobierzDaneDotyczaceWymagan();
                PobierzEkspres();
                PobierzDatyZlecenia();
                PobierzTestyNaSkazenia();
                PobierzUwagiOrazOsobe();
                PobierzDaneAkcesoriow();
                WczytajSzablon(StaleWzorcowan.stale.KARTA_PRZYJECIA);
                WypelnijSzablon();
                ZapiszPlikWynikowy(sciezka);
                return true;
            }

            //****************************************************************************************
            private void WypelnijSzablon()
            //****************************************************************************************
            {
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!c1>", _DaneWypelniajace[(int)stale.ID_KARTY]);
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!c2>", _DaneWypelniajace[(int)stale.ROK]);
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!c3>", _DaneWypelniajace[(int)stale.ZLECENIDOAWCA].Replace(";", "<br>"));
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!c4>", _DaneWypelniajace[(int)stale.ADRES].Replace(";", "<br>"));
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!c5>", _DaneWypelniajace[(int)stale.OSOBA_KONTAKTOWA]);
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!c6>", _DaneWypelniajace[(int)stale.ID_ZLECENIODAWCY]);
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!c7>", _DaneWypelniajace[(int)stale.TELEFON]);
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!c8>", _DaneWypelniajace[(int)stale.FAKS]);
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!c9>", _DaneWypelniajace[(int)stale.EMAIL]);
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!c10>", _DaneWypelniajace[(int)stale.DOZYMETR_TYP]);
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!c11>", _DaneWypelniajace[(int)stale.DOZYMETR_NR_FABRYCZNY]);
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!c12>", _DaneWypelniajace[(int)stale.DOZYMETR_ID]);
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!tabela>", _DaneWypelniajace[(int)stale.TABELA]);
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!c13>", _DaneWypelniajace[(int)stale.AKCESORIA]);

                /* Usuwamy ostatnią spację i przecinek z listy wymagań*/
                String temp = _DaneWypelniajace[(int)stale.WYMAGANIA];
                temp = temp.Remove(temp.Length - 2);
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!c14>", temp);

                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!c15>", _DaneWypelniajace[(int)stale.DATA_PRZYJECIA]);
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!c16>", _DaneWypelniajace[(int)stale.DATA_ZWROTU]);
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!c17>", _DaneWypelniajace[(int)stale.TEST_NA_SKAZENIA]);
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!c18>", _DaneWypelniajace[(int)stale.OSOBA_PRZYJMUJACA]);
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!c19>", _DaneWypelniajace[(int)stale.UWAGI]);

            }

            #region PobieranieDanych

            //****************************************************************************************
            private void PobierzDaneAkcesoriow()
            //****************************************************************************************
            {
                // pobranie informacji na temat akcesoriów
                _Zapytanie = String.Format("SELECT Akcesoria FROM Karta_przyjecia WHERE id_karty={0}", _NrKarty);
                _DaneWypelniajace[(int)stale.AKCESORIA] = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<string>(0);
            }

            //****************************************************************************************
            private void PobierzUwagiOrazOsobe()
            //****************************************************************************************
            {
                _Zapytanie = String.Format("SELECT osoba_przyjmujaca FROM Zlecenia WHERE id_zlecenia = {0}", _DaneWypelniajace[(int)stale.ID_ZLECENIA]);
                DataRow wiersz = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0];
                _DaneWypelniajace[(int)stale.OSOBA_PRZYJMUJACA] = wiersz.Field<string>(0);

                _Zapytanie = String.Format("SELECT uwagi FROM Karta_Przyjecia WHERE id_karty = {0}", _DaneWypelniajace[(int)stale.ID_KARTY]);
                wiersz = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0];
                _DaneWypelniajace[(int)stale.UWAGI] = wiersz.Field<string>(0);
            }

            //****************************************************************************************
            private void PobierzTestyNaSkazenia()
            //****************************************************************************************
            {
                _Zapytanie = String.Format("SELECT test_na_skazenia FROM Karta_przyjecia WHERE id_karty = {0}", _NrKarty);
                _DaneWypelniajace[(int)stale.TEST_NA_SKAZENIA] = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<string>(0).ToString();
            }

            //****************************************************************************************
            private void PobierzDatyZlecenia()
            //****************************************************************************************
            {
                _Zapytanie = String.Format("SELECT data_przyjecia, data_zwrotu FROM Zlecenia WHERE id_zlecenia={0}", _DaneWypelniajace[(int)stale.ID_ZLECENIA]);
                DataRow wiersz = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0];

                _DaneWypelniajace[(int)stale.DATA_PRZYJECIA] = wiersz.Field<DateTime>(0).ToShortDateString();
                _DaneWypelniajace[(int)stale.DATA_ZWROTU] = wiersz.Field<DateTime>(1).ToShortDateString();
            }

            //****************************************************************************************
            private void PobierzEkspres()
            //****************************************************************************************
            {
                _Zapytanie = String.Format("SELECT ekspres FROM Zlecenia WHERE id_zlecenia={0}", _DaneWypelniajace[(int)stale.ID_ZLECENIA]);
                _DaneWypelniajace[(int)stale.EKSPRES] = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<bool>(0).ToString();
            }

            //****************************************************************************************
            private void PobierzDaneDotyczaceWymagan()
            //****************************************************************************************
            {
                // pobranie inf. na temat wymagań
                _Zapytanie = "SELECT moc_dawki, dawka, syg_dawki, syg_mocy_dawki, stront_slaby, stront_silny, wegiel_slaby, wegiel_silny, "
                           + String.Format("ameryk, pluton, chlor FROM Karta_przyjecia WHERE id_karty = {0}", _NrKarty);
                DataRow wiersz = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0];

                String temp = "";

                string[] nazwy = new string[11]{"moc dawki Cs-137", "dawka", "sygnalizacja dawki", "sygnalizacja mocy dawki", "Sr-90/Y-90",
                                            "Sr-90/Y-90 (silny)","C-14", "C-14 (silny)", "Am-241", "Pu-239", "Cl-36"};
                for (int i = 1; i <= 11; ++i)
                {
                    if (false != wiersz.Field<bool>(i - 1))
                    {
                        temp += String.Format("{0}, ", nazwy[i - 1]);
                    }
                }

                _DaneWypelniajace[(int)stale.WYMAGANIA] = temp;
            }

            //****************************************************************************************
            private void PobierzDaneSondy()
            //****************************************************************************************
            {
                _Zapytanie = "SELECT typ, nr_fabryczny, S.id_sondy FROM Sondy AS S INNER JOIN Karta_przyjecia AS K ON S.id_dozymetru="
                           + String.Format("K.id_dozymetru WHERE K.id_karty = {0}", _NrKarty);
                char licznik = '1';

                foreach (DataRow row in _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows)
                {
                    _DaneWypelniajace[(int)stale.TABELA] +=
                    String.Format("<tr align=\"center\" valign=\"middle\"><td><span>{0}</span></td><td><span>{1}</span></td>",
                    licznik.ToString(), row.Field<string>(0))
                    + String.Format("<td><span>{0}</span></td><td><span>{1}</span></td></tr>",
                    row.Field<string>(1), row.Field<int>(2).ToString());

                    ++licznik;
                }
            }

            //****************************************************************************************
            private void PobierzDaneDozymetru()
            //****************************************************************************************
            {
                _Zapytanie = "SELECT typ, nr_fabryczny, K.id_dozymetru FROM Dozymetry AS D INNER JOIN Karta_przyjecia AS K ON "
                           + String.Format("D.id_dozymetru=K.id_dozymetru WHERE K.id_karty = {0}", _NrKarty);
                DataRow wiersz = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0];

                // dodanie typu
                _DaneWypelniajace[(int)stale.DOZYMETR_TYP] = wiersz.Field<string>("typ");
                // dodanie typu
                _DaneWypelniajace[(int)stale.DOZYMETR_NR_FABRYCZNY] = wiersz.Field<string>("nr_fabryczny");
                // dodanie id dozymetru
                _DaneWypelniajace[(int)stale.DOZYMETR_ID] = wiersz.Field<int>("id_dozymetru").ToString();
            }

            //****************************************************************************************
            private void PobierzDaneZleceniodawcy()
            //****************************************************************************************
            {
                _Zapytanie = "SELECT zleceniodawca, adres, osoba_kontaktowa, Z.id_zleceniodawcy, telefon, faks, email FROM Zleceniodawca "
                           + "AS Z INNER JOIN ZLECENIA AS ZL ON Z.id_zleceniodawcy = ZL.id_zleceniodawcy WHERE "
                           + String.Format("ZL.id_zlecenia={0}", _DaneWypelniajace[(int)stale.ID_ZLECENIA]);

                DataRow wiersz = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0];

                // dodanie zleceniodawcy
                _DaneWypelniajace[(int)stale.ZLECENIDOAWCA] = wiersz.Field<string>(0);
                // dodanie adresu
                _DaneWypelniajace[(int)stale.ADRES] = wiersz.Field<string>(1);
                // dodanie osoby kontaktowe
                _DaneWypelniajace[(int)stale.OSOBA_KONTAKTOWA] = wiersz.Field<string>(2);
                // dodanie id zleceniodawcy
                _DaneWypelniajace[(int)stale.ID_ZLECENIODAWCY] = wiersz.Field<int>(3).ToString();
                // dodanie telefonu
                _DaneWypelniajace[(int)stale.TELEFON] = wiersz.Field<string>(4);
                // dodanie faksu
                _DaneWypelniajace[(int)stale.FAKS] = wiersz.Field<string>(5);
                // dodanie e-mail'u
                _DaneWypelniajace[(int)stale.EMAIL] = wiersz.Field<string>(6);
            }

            //****************************************************************************************
            private void PobierzRok()
            //****************************************************************************************
            {
                // dodanie roku
                _Zapytanie = String.Format("SELECT rok FROM Karta_przyjecia WHERE id_karty = {0}", _NrKarty);
                _DaneWypelniajace[(int)stale.ROK] = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<int>(0).ToString();
            }

            #endregion
        }
    }
}
