using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Narzedzia;
using System.Data;
using System.Windows.Forms;

namespace DotBase
{
    namespace Dokumenty
    {
        abstract class WydrukiCez : Wydruki
        {
            //***************************************************
            public WydrukiCez(StaleWzorcowan.stale typ)
            //***************************************************
            {
                _BazaDanych = new BazaDanychWrapper();
                WczytajSzablon(typ);
            }

            //***************************************************
            public bool PobierzDanePodstawowe(string nrKarty, string nrArkusza, DateTime data)
            //***************************************************
            {
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!nrKarty>", nrKarty);
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!nrArkusza>", nrArkusza);
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!data>", data.ToShortDateString());
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!rok>", data.Year.ToString());
                return true;
            }


            //***************************************************
            public bool PobierzDanePrzyrzadu(string typ, string nrFabryczny, string inneNastawy,
                                             string sondaTyp, string sondaNrFabryczny,
                                             string napiecieZasilaniaSondy)
            //***************************************************
            {
                string zapytanie;
                string id_dozymetru;
                string id_sondy;

                try
                {
                    zapytanie = String.Format("SELECT id_dozymetru FROM Dozymetry WHERE typ = '{0}' AND nr_fabryczny = '{1}'",
                                              typ, nrFabryczny);
                    id_dozymetru = _BazaDanych.TworzTabeleDanych(zapytanie).Rows[0].Field<int>(0).ToString();
                    zapytanie = String.Format("SELECT id_sondy FROM Sondy WHERE typ = '{0}' AND nr_fabryczny = '{1}' AND id_dozymetru = {2}",
                                              sondaTyp, sondaNrFabryczny, id_dozymetru);
                    id_sondy = _BazaDanych.TworzTabeleDanych(zapytanie).Rows[0].Field<int>(0).ToString();
                }
                catch (Exception)
                {
                    return false;
                }

                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!dozymetr_id>", id_dozymetru);
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!sonda_id>", id_sondy);
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!dozymetr_typ>", typ);
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!dozymetr_NrFab>", nrFabryczny);
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!inne_nastawy>", inneNastawy);
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!sonda_typ>", sondaTyp);
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!sonda_NrFab>", sondaNrFabryczny);
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!nap_zas_sondy>", napiecieZasilaniaSondy);

                return true;
            }

            //***************************************************
            public bool PobierzDaneWarunkow(string cisnienie, string temperatura, string wilgotnosc, string uwagi)
            //***************************************************
            {
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!cisnienie>", cisnienie);
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!temperatura>", temperatura);
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!wilgotnosc>", wilgotnosc);
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!uwagi>", uwagi);
                return true;
            }

            //***************************************************
            public bool PobierzDaneWspolczynnikow(string wykonal, string sprawdzil)
            //***************************************************
            {
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!wykonal>", wykonal);
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!sprawdzil>", sprawdzil);
                return true;
            }

            //***************************************************
            override public bool UtworzDokument(string sciezka)
            //***************************************************
            {
                return ZapiszPlikWynikowy(sciezka);
            }
        }
    }
}
