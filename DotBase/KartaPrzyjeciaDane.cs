using System;
using System.Collections.Generic;
using System.Text;
using Narzedzia;
/*
namespace DotBase
{
    namespace PrzestrzenKartyPrzyjecia
    {
        //----------------------------------------------------------------------------------------------
        //-------------------------------DANE------------PRZYRZĄDU--------------------------------------
        //----------------------------------------------------------------------------------------------
        public class DaneDodatkowe
        {
            //----------------------------------------------
            public DaneDodatkowe(string akcesoria, string uwagi, bool uszkodzony)
            //----------------------------------------------
            {
                Akcesoria = akcesoria;
                Uwagi = uwagi;
                Uszkodzony = uszkodzony;
            }

            public string Akcesoria { get; private set; }
            public string Uwagi { get; private set; }
            public bool Uszkodzony { get; private set; }
        }
        //----------------------------------------------------------------------------------------------
        //-------------------------------DANE------------PRZYRZĄDU--------------------------------------
        //----------------------------------------------------------------------------------------------
        public class DanePrzyrzadu
        {
            public int IDdozymetru { get; set; }
            public string NrFabryczny { get; set; }
            public string Typ { get; set; }
            public ListaSond ListaSond { get; set; }

            //------------------------------------------------------------------
            public DanePrzyrzadu(string typ, string nrFab)
            //------------------------------------------------------------------
            {
                Typ = typ;
                NrFabryczny = nrFab;
                ListaSond = new ListaSond();
            }
        }
        //----------------------------------------------------------------------------------------------
        //-------------------------------WYMAGANIA DOTYCZĄCE KALIBRACJI---------------------------------
        //----------------------------------------------------------------------------------------------
        public class WymaganiaKalibracji
        {
            public enum Stale { AMERYK = 0, CHLOR, DAWKA, MOC_DAWKI, PLUTON, STRONT_SLABY, STRONT_SILNY, SYGNALIZACJA_DAWKI, SYGNALIZACJA_MOCY_DAWKI, WEGIEL_SLABY, WEGIEL_SILNY, LICZBA_WYMAGAN };
            public bool[] dane;

            //----------------------------------------------------------------------------------------------------------
            public WymaganiaKalibracji(bool ameryk, bool chlor, bool dawka, bool moc_dawki, bool pluton, bool stront_slaby, bool stront_silny, bool sygnalizacja_dawki, bool sygnalizacja_mocy_dawki, bool wegiel_slaby, bool wegiel_silny)
            //----------------------------------------------------------------------------------------------------------
            {
                dane = new bool[(int)Stale.LICZBA_WYMAGAN];

                dane[(int)Stale.AMERYK] = ameryk;
                dane[(int)Stale.CHLOR] = chlor;
                dane[(int)Stale.DAWKA] = dawka;
                dane[(int)Stale.MOC_DAWKI] = moc_dawki;
                dane[(int)Stale.PLUTON] = pluton;
                dane[(int)Stale.STRONT_SLABY] = stront_slaby;
                dane[(int)Stale.STRONT_SILNY] = stront_silny;
                dane[(int)Stale.SYGNALIZACJA_DAWKI] = sygnalizacja_dawki;
                dane[(int)Stale.SYGNALIZACJA_MOCY_DAWKI] = sygnalizacja_mocy_dawki;
                dane[(int)Stale.WEGIEL_SLABY] = wegiel_slaby;
                dane[(int)Stale.WEGIEL_SILNY] = wegiel_silny;
            }
        }
        //----------------------------------------------------------------------------------------------
        //-------------------------------DANE--KARTY--PRZYJĘCIA-----------------------------------------
        //----------------------------------------------------------------------------------------------
        public class DaneKartyPrzyjecia
        {
            public int IdKarty { get; set; }
            public int NrZlecenia { get; set; }
            public int idDozymetru { get; set; }
            public DaneDodatkowe DaneDodatkowe { get; set; }
            public DanePrzyrzadu Przyrzad { get; set; }
            public WymaganiaKalibracji Wymagania { get; set; }

            //----------------------------------------------------------------------------------------------
            public DaneKartyPrzyjecia()
            //----------------------------------------------------------------------------------------------
            {
            }

            //----------------------------------------------------------------------------------------------
            public DaneKartyPrzyjecia(int idKarty, int nrZlecenia)
            //----------------------------------------------------------------------------------------------
            {
                this.IdKarty = idKarty;
                this.NrZlecenia = nrZlecenia;
            }

            //----------------------------------------------------------------------------------------------
            public void UstawDaneDodatkowe(string akcesoria, string uwagi, bool uszkodzony)
            //----------------------------------------------------------------------------------------------
            {
                DaneDodatkowe = new DaneDodatkowe(akcesoria, uwagi, uszkodzony);
            }

            //----------------------------------------------------------------------------------------------
            public void UstawDanePodstawowe(int idKarty, int nrZlecenia)
            //----------------------------------------------------------------------------------------------
            {
                IdKarty = idKarty;
                NrZlecenia = nrZlecenia;
            }

            //----------------------------------------------------------------------------------------------
            public void UstawDanePrzyrzadu(string typ, string nrFab)
            //----------------------------------------------------------------------------------------------
            {
                Przyrzad = new DanePrzyrzadu(typ, nrFab);
            }

            //----------------------------------------------------------------------------------------------
            public void UstawDaneWymagan(bool ameryk, bool chlor, bool dawka, bool moc_dawki, bool pluton, bool stront_slaby, bool stront_silny,
                                         bool sygnalizacja_dawki, bool sygnalizacja_mocy_dawki, bool wegiel_slaby, bool wegiel_silny)
            //----------------------------------------------------------------------------------------------
            {
                Wymagania = new WymaganiaKalibracji(ameryk, chlor, dawka, moc_dawki, pluton, stront_slaby, stront_silny, sygnalizacja_dawki, sygnalizacja_mocy_dawki, wegiel_slaby, wegiel_silny);
            }
        }
        //----------------------------------------------------------------------------------------------
        //------------------------------------DANE--------PROTOKOŁU-------------------------------------
        //----------------------------------------------------------------------------------------------
        public class DaneProtokolu
        {
            public List<string> DanePodstawowe;
            public List<string> DaneTabelowe;

            public DaneProtokolu(List<string> danePodst, List<string> daneTabel)
            {
                DanePodstawowe = danePodst;
                DaneTabelowe = daneTabel;
            }
        }
    }
}
*/