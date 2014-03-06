using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
namespace DotBase
{
    namespace Dokumenty
    {
        class PismoPrzewodnie : Wydruki
        {
            enum stale
            {
                ADRES,
                DATA, DATA_PLUS_ROK, DATA_MESIAC_SLOWNIE,
                INFO_SONDY, INFO_WYKRES, INFO_WYKRES_KALIB,
                NR_FABRYCZNY, NR_KARTY,
                ZLECENIODAWCA,
                NR_PISMA,
                ROK,
                TYP,
                UWAGA,
                MAX_ELEMENTOW
            };

            //****************************************************************************************
            public PismoPrzewodnie(int nrKarty, DateTime data, string uwaga, string nrPisma, bool przedluzonaWaznosc)
            //****************************************************************************************
            {
                InicjalizujListeDanychWypelniajacych((int)stale.MAX_ELEMENTOW);

                _DaneWypelniajace[(int)stale.NR_KARTY] = _NrKarty = nrKarty.ToString();
                _DaneWypelniajace[(int)stale.DATA] = data.ToString("dd.MM.yyyy");
                _DaneWypelniajace[(int)stale.DATA_MESIAC_SLOWNIE] = data.ToString("dd<!>MMMM yyyy").Replace("<!>", "&nbsp;");

                if (przedluzonaWaznosc)
                    _DaneWypelniajace[(int)stale.DATA_PLUS_ROK] = data.AddYears(2).ToString("dd<!>MMMM yyyy").Replace("<!>", "&nbsp;");
                else
                    _DaneWypelniajace[(int)stale.DATA_PLUS_ROK] = data.AddYears(1).ToString("dd<!>MMMM yyyy").Replace("<!>", "&nbsp;");

                _DaneWypelniajace[(int)stale.ROK] = data.Year.ToString();
                _DaneWypelniajace[(int)stale.UWAGA] = uwaga;
                _DaneWypelniajace[(int)stale.NR_PISMA] = nrPisma;
            }

            //********************************************************************************************
            public void PobierzDane()
            //********************************************************************************************
            {
                PobierzDaneZleceniodawcy();
                PobierzDaneDozymetru();
                PobierzDaneSond();
                UstawUwage();

                PobierzDodatkowaInformacjeOWykresie();

                _Zapytanie = String.Format("SELECT COUNT(*) FROM Wzorcowanie_cezem WHERE id_karty = {0} AND rodzaj_wzorcowania = 'd'", _NrKarty);

                if (0 != _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<int>(0))
                {
                    UsunKomentarzeDotWzorcowaniaCezem();
                    _DaneWypelniajace[(int)stale.INFO_WYKRES_KALIB] = "<li>Wykres kalibracyjny w zakresie dawki</li>";
                }
            }

            //********************************************************************************************
            private void PobierzDaneDozymetru()
            //********************************************************************************************
            {
                _Zapytanie = "SELECT typ, nr_fabryczny FROM Dozymetry WHERE id_dozymetru=(SELECT id_dozymetru FROM Karta_przyjecia WHERE "
                           + String.Format("id_karty = {0})", _NrKarty);

                DataRow wiersz = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0];
                _DaneWypelniajace[(int)stale.TYP] = wiersz.Field<string>(0);
                _DaneWypelniajace[(int)stale.NR_FABRYCZNY] = wiersz.Field<string>(1);
            }

            //********************************************************************************************
            private void PobierzDaneSond()
            //********************************************************************************************
            {
                List<KeyValuePair<string, string>> sondy = new List<KeyValuePair<string, string>>();
                _Zapytanie = "SELECT typ, nr_fabryczny FROM Sondy WHERE id_dozymetru=(SELECT id_dozymetru FROM Karta_przyjecia "
                           + String.Format("WHERE id_karty = {0})", _NrKarty);

                int licznik = 0;

                foreach (DataRow wiersz in _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows)
                {
                    ++licznik;
                    sondy.Add(new KeyValuePair<string, string>(wiersz.Field<string>(0), wiersz.Field<string>(1)));
                }

                String NIE_DOTYCZY = "nie dotyczy";
                String infoSondy = "";

                if (1 == licznik && NIE_DOTYCZY != sondy[0].Value)
                {
                    infoSondy = String.Format("wraz z sondą <b>{0}</b> nr&nbsp;fab. <b>{1}</b> ", sondy[0].Key.Replace(" ", "&nbsp;"), sondy[0].Value.Replace(" ", "&nbsp;"));
                }
                else if (1 == licznik && NIE_DOTYCZY == sondy[0].Value)
                {
                    infoSondy = "";
                }
                else if ((NIE_DOTYCZY == sondy[0].Value ^ NIE_DOTYCZY == sondy[1].Value) && licznik == 2)
                {
                    if (sondy[0].Value != NIE_DOTYCZY)
                        infoSondy = String.Format("wraz z sondą <b>{0}</b> nr&nbsp;fab. <b>{1}</b> ", sondy[0].Key.Replace(" ", "&nbsp;"), sondy[0].Value.Replace(" ", "&nbsp;"));
                    else
                        infoSondy = String.Format("wraz z sondą <b>{0}</b> nr&nbsp;fab. <b>{1}</b> ", sondy[1].Key.Replace(" ", "&nbsp;"), sondy[1].Value.Replace(" ", "&nbsp;"));
                }
                else if (2 == licznik && NIE_DOTYCZY != sondy[0].Value && NIE_DOTYCZY != sondy[1].Value)
                {
                    infoSondy = String.Format("wraz z sondami <b>{0}</b> nr&nbsp;fab. <b>{1}</b> i <b>{2}</b> nr&nbsp;fab. <b>{3}</b> ",
                                sondy[0].Key.Replace(" ", "&nbsp;"), sondy[0].Value.Replace(" ", "&nbsp;"), sondy[1].Key.Replace(" ", "&nbsp;"), sondy[1].Value.Replace(" ", "&nbsp;"));
                }
                else if (2 < licznik)
                {
                    int k;
                    for (k = 0; k < licznik; ++k)
                    {
                        if (NIE_DOTYCZY == sondy[k].Value)
                            break;
                    }

                    String temp;
                    infoSondy = "wraz z sondami ";

                    for (int p = 0; p < licznik; ++p)
                    {
                        if (p != k)
                        {
                            temp = String.Format("<b>{0}</b> nr&nbsp;fab. <b>{1}</b>", sondy[p].Key.Replace(" ", "&nbsp;"), sondy[p].Value.Replace(" ", "&nbsp;"));
                            infoSondy += temp;
                        }
                    }
                }

                _DaneWypelniajace[(int)stale.INFO_SONDY] = infoSondy;
            }

            //********************************************************************************************
            private void PobierzDaneZleceniodawcy()
            //********************************************************************************************
            {
                _Zapytanie = "SELECT Zleceniodawca, Adres FROM Zleceniodawca WHERE id_zleceniodawcy=(SELECT id_zleceniodawcy FROM Zlecenia WHERE "
                           + String.Format("id_zlecenia=(SELECT id_zlecenia FROM Karta_przyjecia WHERE id_karty = {0}))",
                           _DaneWypelniajace[(int)stale.NR_KARTY]);

                DataRow wiersz = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0];

                _DaneWypelniajace[(int)stale.ZLECENIODAWCA] = wiersz.Field<string>(0).Replace(";", "<br>");
                _DaneWypelniajace[(int)stale.ADRES] = wiersz.Field<string>(1).Replace(";", "<br>");
            }

            //********************************************************************************************
            private void PobierzDodatkowaInformacjeOWykresie()
            //********************************************************************************************
            {
                _Zapytanie = String.Format("SELECT COUNT(*) FROM Wzorcowanie_cezem WHERE rodzaj_wzorcowania='md' AND id_karty = {0}", _NrKarty);

                if (0 != _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<int>(0))
                {
                    UsunKomentarzeDotWzorcowaniaCezem();
                    _DaneWypelniajace[(int)stale.INFO_WYKRES] = "<li>Wykres kalibracyjny w zakresie mocy dawki</li>";
                }

                _Zapytanie = String.Format("SELECT COUNT(*) FROM Wzorcowanie_cezem WHERE rodzaj_wzorcowania IN('sd', 'sm') AND id_karty = {0}", _NrKarty);

                if (0 != _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<int>(0))
                {
                    UsunKomentarzeDotWzorcowaniaCezem();
                }
            }

            //********************************************************************************************
            private void UstawUwage()
            //********************************************************************************************
            {
                if ("" != _DaneWypelniajace[(int)stale.UWAGA])
                    _DaneWypelniajace[(int)stale.UWAGA] = "<b>Uwaga:</b> " + _DaneWypelniajace[(int)stale.UWAGA];
            }

            //********************************************************************************************
            void UsunKomentarzeDotWzorcowaniaCezem()
            //********************************************************************************************
            {
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!--", "").Replace("-->", "");
            }

            //********************************************************************************************
            override public bool UtworzDokument(string sciezka)
            //********************************************************************************************
            {
                WczytajSzablon(Narzedzia.StaleWzorcowan.stale.PISMO_PRZEWODNIE);
                PobierzDane();

                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!c1>", _DaneWypelniajace[(int)stale.DATA]);
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!data_slownie>", _DaneWypelniajace[(int)stale.DATA_MESIAC_SLOWNIE]);
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!c2>", _DaneWypelniajace[(int)stale.ZLECENIODAWCA]);
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!c3>", _DaneWypelniajace[(int)stale.ADRES]);
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!c4>", _DaneWypelniajace[(int)stale.NR_PISMA]);
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!c5>", _DaneWypelniajace[(int)stale.ROK]);
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!c6>", _DaneWypelniajace[(int)stale.TYP]);
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!c7>", _DaneWypelniajace[(int)stale.NR_FABRYCZNY]);
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!c8>", _DaneWypelniajace[(int)stale.NR_KARTY]);
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!c9>", _DaneWypelniajace[(int)stale.UWAGA]);
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!c10>", _DaneWypelniajace[(int)stale.DATA_PLUS_ROK]);
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!sonda>", _DaneWypelniajace[(int)stale.INFO_SONDY]);
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!c11>", _DaneWypelniajace[(int)stale.INFO_WYKRES] + _DaneWypelniajace[(int)stale.INFO_WYKRES_KALIB]);

                ZapiszPlikWynikowy(sciezka);

                return true;
            }
        }
    }
}
