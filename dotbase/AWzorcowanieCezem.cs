using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace DotBase
{
    abstract class WzorcowanieCez : WspolneDaneWzorcowan
    {
        #region Właściowści

        public Narzedzia.Jednostki              Jednostki      { get; set; }
        public Narzedzia.ListaSond              Sondy          { get; protected set; }
        public Narzedzia.Protokoly              Protokoly      { get; set; }
        public KlasyPomocniczeCez.Przyrzad      Przyrzad       { get; protected set; }
        public KlasyPomocniczeCez.Warunki       Warunki        { get; set; }

        public string WybranaJednostka { get; protected set; }
        public string WybranyProtokol  { get; protected set; }

        #endregion

        protected string _RodzajWzorcowania;

        protected KlasyPomocniczeCez.DaneOgolne _DaneOgolneDoZapisu;
        private   KlasyPomocniczeCez.Przyrzad   _PrzyrzadDoZapisu;
        private   KlasyPomocniczeCez.Warunki    _WarunkiDoZapisu;

        //---------------------------------------------------------------
        public WzorcowanieCez(int idKarty, string rodzajWzorcowania)
        //---------------------------------------------------------------
        {
            IdKarty             = idKarty;
            Jednostki           = new Narzedzia.Jednostki();
            Protokoly           = new Narzedzia.Protokoly();
            _BazaDanych         = new BazaDanychWrapper();
            Przyrzad            = new KlasyPomocniczeCez.Przyrzad();
            _RodzajWzorcowania  = rodzajWzorcowania;
            Sondy               = new Narzedzia.ListaSond();
        }

        //---------------------------------------------------------------
        virtual public void CzyscStareDane()
        //---------------------------------------------------------------
        {   
            Sondy.Lista.Clear();
            Przyrzad.Sondy.Lista.Clear();
        }

        //---------------------------------------------------------------
        public bool NadpiszDane()
        //---------------------------------------------------------------
        {
            return (NadpiszDaneOgolne() &&
                    NadpiszDanePrzyrzadu() &&
                    NadpiszDaneWarunkow() &&
                    NadpiszDaneObliczonychWspolczynnikow() &&
                    NadpiszDaneWzorcoweIPomiarowe()
                   );
        }

        //---------------------------------------------------------------
        public bool NadpiszDaneOgolne()
        //---------------------------------------------------------------
        {
            _Zapytanie = String.Format("UPDATE Wzorcowanie_cezem SET Data_wzorcowania=#{0}# WHERE id_wzorcowania={1} AND id_arkusza={2}",
                                       _DaneOgolneDoZapisu.Data, _DaneOgolneDoZapisu.IdWzorcowania, _DaneOgolneDoZapisu.IdArkusza);

            _BazaDanych.WykonajPolecenie(_Zapytanie);

            return true;
        }

        //---------------------------------------------------------------
        public bool NadpiszDanePrzyrzadu()
        //---------------------------------------------------------------
        {
            return ZapiszDanePrzyrzadu();
        }

        //---------------------------------------------------------------
        public bool NadpiszDaneWarunkow()
        //---------------------------------------------------------------
        {
            return ZapiszDaneWarunkow();
        }

        abstract public bool NadpiszDaneObliczonychWspolczynnikow();
        abstract public bool NadpiszDaneWzorcoweIPomiarowe();

        //---------------------------------------------------------------
        public bool PobierzDanePrzyrzadu()
        //---------------------------------------------------------------
        {
            _Zapytanie = "SELECT D.Typ, D.Nr_fabryczny, Inne_nastawy, Napiecie_zasilania_Sondy FROM "
                       + "((Dozymetry D INNER JOIN Karta_przyjecia K ON D.Id_dozymetru=K.Id_dozymetru) INNER JOIN Wzorcowanie_Cezem "
                       + String.Format("AS W ON W.id_karty=K.id_karty) WHERE K.id_karty = {0} AND W.id_arkusza = {1}", IdKarty, IdArkusza);


            DataRow wiersz = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0];

            if (null == wiersz)
                return false;

            if (false == wiersz.IsNull(0))
                Przyrzad.TypDozymetru = wiersz.Field<string>(0);

            if (false == wiersz.IsNull(1))
                Przyrzad.NrFabrycznyDozymetru = wiersz.Field<string>(1);

            if (false == wiersz.IsNull(2))
                Przyrzad.InneNastawy = wiersz.Field<string>(2);

            if (false == wiersz.IsNull(3))
                Przyrzad.NapiecieZasilaniaSondy = wiersz.Field<string>(3);

            Przyrzad.Sondy.Lista.Clear();

            PobierzWszystkieMozliweSondy();

            return true;
        }

        //---------------------------------------------------------------
        public bool PobierzDaneWarunkow()
        //---------------------------------------------------------------
        {
            _Zapytanie = String.Format("SELECT Cisnienie, Temperatura, Wilgotnosc, Uwagi FROM Wzorcowanie_cezem WHERE Id_wzorcowania = {0} AND id_arkusza = {1}",
                         IdWzorcowania, IdArkusza);
            _OdpowiedzBazy = _BazaDanych.TworzTabeleDanych(_Zapytanie);

            try
            {
                Warunki = new KlasyPomocniczeCez.Warunki(_OdpowiedzBazy.Rows[0].Field<double>(0), _OdpowiedzBazy.Rows[0].Field<double>(1),
                                                         _OdpowiedzBazy.Rows[0].Field<double>(2), _OdpowiedzBazy.Rows[0].Field<string>(3));
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        //---------------------------------------------------------------
        public bool PobierzDateWzorcowania()
        //---------------------------------------------------------------
        {
            _Zapytanie = "SELECT data_wzorcowania FROM Wzorcowanie_cezem WHERE "
                       + String.Format("Id_karty = {0} AND Id_arkusza = {1}", IdKarty, IdArkusza);

            _OdpowiedzBazy = _BazaDanych.TworzTabeleDanych(_Zapytanie);

            if (_OdpowiedzBazy.Rows[0].IsNull(0))
                return false;

            return true;
        }

        //---------------------------------------------------------------
        public bool PobierzKonkretnyProtokol()
        //---------------------------------------------------------------
        {
            _Zapytanie = String.Format("SELECT id_protokolu FROM Wzorcowanie_cezem WHERE id_wzorcowania = {0}", IdWzorcowania);

            try
            {
                short idProtokolu = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<short>(0);
                _Zapytanie = String.Format("Select data_kalibracji FROM Protokoly_kalibracji_lawy WHERE id_protokolu = {0}", idProtokolu);
                WybranyProtokol = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<DateTime>(0).ToShortDateString();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        //---------------------------------------------------------------
        public bool PrzygotujDaneOgolneDoZapisu(string idKarty, string idArkusza, DateTime data, bool TrybNadpisywania)
        //---------------------------------------------------------------
        {
            _DaneOgolneDoZapisu = new KlasyPomocniczeCez.DaneOgolne();

            _DaneOgolneDoZapisu.IdKarty   = idKarty;
            _DaneOgolneDoZapisu.IdArkusza = idArkusza;
            _DaneOgolneDoZapisu.Data      = data.ToShortDateString();

            try
            {
                if (TrybNadpisywania)
                {
                    _DaneOgolneDoZapisu.IdWzorcowania = ZnajdzIdWzorcowania(idArkusza, idKarty).ToString();
                }
                else
                {
                    _DaneOgolneDoZapisu.IdWzorcowania = (ZnajdzMaksymalneIdWzorcowania() + 1).ToString();
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        //---------------------------------------------------------------
        public bool PobierzPodstawoweDanePrzyrzadu()
        //---------------------------------------------------------------
        {
            _Zapytanie = "SELECT D.Typ, D.Nr_fabryczny FROM Dozymetry D INNER JOIN Karta_przyjecia K ON D.Id_dozymetru=K.Id_dozymetru "
                       + String.Format("WHERE K.Id_karty = {0}", IdKarty);

            DataRow wiersz = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0];

            if (null == wiersz)
                return false;

            if (false == wiersz.IsNull(0))
                Przyrzad.TypDozymetru = wiersz.Field<string>(0);

            if (false == wiersz.IsNull(1))
                Przyrzad.NrFabrycznyDozymetru = wiersz.Field<string>(1);

            return true;
        }

        //--------------------------------------------------------------------
        public string PobierzRok(int nrKarty)
        //--------------------------------------------------------------------
        {
            string rok;

            _Zapytanie = String.Format("SELECT rok FROM Karta_przyjecia WHERE id_karty = {0}", nrKarty);

            try
            {
                rok = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<int>(0).ToString();
            }
            catch (Exception)
            {
                rok = "";
            }

            return rok;
        }

        //---------------------------------------------------------------
        public bool PobierzWszystkieJednostki()
        //---------------------------------------------------------------
        {
            Jednostki.Czysc();

            try
            {
                foreach (DataRow row in _BazaDanych.TworzTabeleDanych("SELECT id_jednostki, jednostka FROM Jednostki").Rows)
                {
                    Jednostki.Dodaj(row.Field<int>("id_jednostki"), row.Field<string>("jednostka"));
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        //---------------------------------------------------------------
        public bool PobierzWszystkieMozliweSondy()
        //---------------------------------------------------------------
        {
            _Zapytanie = "SELECT Typ, Nr_fabryczny FROM Sondy WHERE Id_Sondy IN (SELECT ID_Sondy FROM Sondy WHERE " +
                         "id_dozymetru = (SELECT id_dozymetru FROM Dozymetry WHERE " +
                         String.Format("typ='{0}' AND nr_fabryczny='{1}'))", Przyrzad.TypDozymetru, Przyrzad.NrFabrycznyDozymetru);

            _OdpowiedzBazy = _BazaDanych.TworzTabeleDanych(_Zapytanie);

            try
            {
                foreach (DataRow row in _OdpowiedzBazy.Rows)
                {
                    Sondy.Lista.Add(new Narzedzia.Sonda(row.Field<string>(0), row.Field<string>(1)));
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        //---------------------------------------------------------------
        public bool PobierzWszystkieProtokoly()
        //---------------------------------------------------------------
        {
            Protokoly.Czysc();
            try
            {
                foreach (DataRow row in _BazaDanych.TworzTabeleDanych("SELECT id_protokolu, data_kalibracji FROM Protokoly_kalibracji_lawy ORDER BY id_protokolu").Rows)
                {
                    Protokoly.Dodaj(row.Field<short>("id_protokolu"), row.Field<DateTime>("data_kalibracji"));
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        //---------------------------------------------------------------
        public bool PrzygotujDanePrzyrzaduDoZapisu(string typ, string nrFabryczny, string napiecieZasilania, string inneNastawy, string sondaTyp, string sondaNrFab)
        //---------------------------------------------------------------
        {
            _PrzyrzadDoZapisu = new KlasyPomocniczeCez.Przyrzad();

            _PrzyrzadDoZapisu.TypDozymetru = typ;
            _PrzyrzadDoZapisu.NrFabrycznyDozymetru = nrFabryczny;
            _PrzyrzadDoZapisu.NapiecieZasilaniaSondy = napiecieZasilania;
            _PrzyrzadDoZapisu.InneNastawy = inneNastawy;
            _PrzyrzadDoZapisu.Sondy.Lista.Add(new Narzedzia.Sonda(sondaTyp, sondaNrFab));

            return true;
        }

        //---------------------------------------------------------------
        public bool PrzygotujDaneWarunkowDoZapisu(string cisnienie, string temperatura, string wilgotnosc, string uwagi)
        //---------------------------------------------------------------
        {
            _WarunkiDoZapisu = new KlasyPomocniczeCez.Warunki();

            try
            {
                _WarunkiDoZapisu.Cisnienie = Double.Parse(cisnienie);
                _WarunkiDoZapisu.Temperatura = Double.Parse(temperatura);
                _WarunkiDoZapisu.Wilgotnosc = Double.Parse(wilgotnosc);
                _WarunkiDoZapisu.Uwagi = uwagi;
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        //---------------------------------------------------------------
        public bool SprawdzCzyArkuszJestJuzZapisany()
        //---------------------------------------------------------------
        {
            _Zapytanie = String.Format("SELECT 1 FROM Wzorcowanie_cezem WHERE id_karty = {0} AND id_arkusza = {1}", IdKarty, IdArkusza);
            try
            {
                _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<int>(0);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        //---------------------------------------------------------------
        public bool SprawdzSpojnoscRodzajowWzorcowania()
        //---------------------------------------------------------------
        {
            _Zapytanie = String.Format("SELECT rodzaj_wzorcowania FROM Wzorcowanie_cezem WHERE id_karty = {0} AND id_arkusza = {2}", IdKarty, IdArkusza);

            _OdpowiedzBazy = _BazaDanych.TworzTabeleDanych(_Zapytanie);

            string rodzajWzorcowania;

            try
            {
                rodzajWzorcowania = _OdpowiedzBazy.Rows[0].Field<string>(0);
            }
            catch (Exception)
            {
                return false;
            }

            if (rodzajWzorcowania != _RodzajWzorcowania)
                return false;

            return true;
        }

        //---------------------------------------------------------------
        public override bool StworzNowyArkusz()
        //---------------------------------------------------------------
        {
            if (false == ZnajdzMaksymalnyArkusz())
                return false;

            ++IdArkusza;

            return true;
        }

        //---------------------------------------------------------------
        public bool ZapiszDane()
        //---------------------------------------------------------------
        {
            return ZapiszDaneOgolne() &&
                   ZapiszDanePrzyrzadu() &&
                   ZapiszDaneWarunkow() &&
                   ZapiszDaneWzorcoweIPomiarowe() &&
                   ZapiszDaneObliczonychWspolczynnikow();
        }

        //---------------------------------------------------------------
        protected bool ZapiszDaneOgolne()
        //---------------------------------------------------------------
        {
            _Zapytanie = "INSERT INTO Wzorcowanie_cezem (Id_karty, Id_arkusza, Data_wzorcowania, rodzaj_wzorcowania, Id_wzorcowania) "
                       + String.Format("VALUES ({0}, {1}, #{2}#, '{3}', {4})", _DaneOgolneDoZapisu.IdKarty, _DaneOgolneDoZapisu.IdArkusza, 
                       _DaneOgolneDoZapisu.Data, _RodzajWzorcowania ,_DaneOgolneDoZapisu.IdWzorcowania);

            _BazaDanych.WykonajPolecenie(_Zapytanie);

            return true;
        }

        //---------------------------------------------------------------
        protected bool ZapiszDanePrzyrzadu()
        //---------------------------------------------------------------
        {
            _Zapytanie = "SELECT Id_Sondy FROM Sondy WHERE Id_dozymetru=(SELECT Id_dozymetru FROM Karta_przyjecia WHERE "
                       + String.Format("Id_karty = {0}) AND typ = '{1}' AND nr_fabryczny='{2}'",
                       _DaneOgolneDoZapisu.IdKarty, _PrzyrzadDoZapisu.Sondy.Lista[0].Typ, _PrzyrzadDoZapisu.Sondy.Lista[0].NrFabryczny);
            int idSondy = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<int>(0);

            _Zapytanie = String.Format("UPDATE Wzorcowanie_cezem SET Id_Sondy = {0},  Napiecie_zasilania_sondy = '{1}', ", idSondy, _PrzyrzadDoZapisu.NapiecieZasilaniaSondy)
                       + String.Format("inne_nastawy = '{0}' WHERE Id_wzorcowania = {1} AND id_arkusza={2}", _PrzyrzadDoZapisu.InneNastawy, _DaneOgolneDoZapisu.IdWzorcowania, _DaneOgolneDoZapisu.IdArkusza);

            _BazaDanych.WykonajPolecenie(_Zapytanie);

            return true;
        }

        //---------------------------------------------------------------
        public bool ZapiszDaneWarunkow()
        //---------------------------------------------------------------
        {
            _Zapytanie = String.Format("UPDATE Wzorcowanie_cezem SET Cisnienie = '{0}', Temperatura = '{1}', ", 
                                       _WarunkiDoZapisu.Cisnienie, _WarunkiDoZapisu.Temperatura)
                       + String.Format("Wilgotnosc = '{0}', Uwagi = '{1}' WHERE id_wzorcowania={2} AND id_arkusza = {3}", 
                                       _WarunkiDoZapisu.Wilgotnosc, _WarunkiDoZapisu.Uwagi, _DaneOgolneDoZapisu.IdWzorcowania, _DaneOgolneDoZapisu.IdArkusza);

            _BazaDanych.WykonajPolecenie(_Zapytanie);

            return true;
        }

        abstract public bool ZapiszDaneWzorcoweIPomiarowe();
        abstract public bool ZapiszDaneObliczonychWspolczynnikow();

        //---------------------------------------------------------------
        public bool ZnajdzArkusz(int idArkusza)
        //---------------------------------------------------------------
        {
            _Zapytanie = String.Format("SELECT Id_arkusza FROM Wzorcowanie_cezem WHERE id_karty = {0} AND id_arkusza = {2}", IdKarty, IdArkusza);

            _OdpowiedzBazy = _BazaDanych.TworzTabeleDanych(_Zapytanie);

            try
            {
                IdArkusza = _OdpowiedzBazy.Rows[0].Field<short>(0);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        //---------------------------------------------------------------
        public int ZnajdzIdWzorcowania(string idArkusza, string idKarty)
        //---------------------------------------------------------------
        {
            _Zapytanie = String.Format("SELECT id_wzorcowania FROM Wzorcowanie_cezem WHERE id_karty={0} AND Id_arkusza={1}", idKarty, idArkusza);
            return _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<int>(0);
        }

        //---------------------------------------------------------------
        public int ZnajdzMaksymalneIdWzorcowania()
        //---------------------------------------------------------------
        {
            _Zapytanie = "SELECT MAX(Id_wzorcowania) FROM Wzorcowanie_cezem";
            return _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<int>(0);
        }

        //-------------------------------------------------
        public override bool ZnajdzMinimalnyArkusz()
        //-------------------------------------------------
        {
            _Zapytanie = String.Format("SELECT MIN(Id_arkusza) FROM Wzorcowanie_cezem WHERE id_karty={0} AND rodzaj_wzorcowania='{1}'",
                         IdKarty, _RodzajWzorcowania);

            _OdpowiedzBazy = _BazaDanych.TworzTabeleDanych(_Zapytanie);

            try
            {
                IdArkusza = _OdpowiedzBazy.Rows[0].Field<short>(0);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }


        //---------------------------------------------------------------
        public override bool ZnajdzMaksymalnyArkusz()
        //---------------------------------------------------------------
        {
            _Zapytanie = String.Format("SELECT MAX(Id_arkusza) FROM Wzorcowanie_cezem WHERE id_karty = {0}", IdKarty);

            _OdpowiedzBazy = _BazaDanych.TworzTabeleDanych(_Zapytanie);

            try
            {
                IdArkusza = _OdpowiedzBazy.Rows[0].Field<short>(0);
            }
            catch (Exception)
            {
                IdArkusza = 0;
            }

            return true;
        }

        //---------------------------------------------------------------
        public override bool ZnajdzMniejszyArkusz()
        //---------------------------------------------------------------
        {

            _Zapytanie = "SELECT Max(Id_arkusza) FROM Wzorcowanie_cezem WHERE "
                       + String.Format("id_karty = {0} AND rodzaj_wzorcowania = '{1}' AND id_arkusza < {2}", 
                         IdKarty, _RodzajWzorcowania, IdArkusza);
    
            _OdpowiedzBazy = _BazaDanych.TworzTabeleDanych(_Zapytanie);

            try
            {
                IdArkusza = _OdpowiedzBazy.Rows[0].Field<short>(0);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        //------------------------------------------------------------------
        public int ZnajdzNrPoprzedniejKalibracji(string typDozymetru, string nrDozymetru)
        //------------------------------------------------------------------
        {
            if (typDozymetru == "" || nrDozymetru == "")
                return 0;

            _Zapytanie = String.Format("SELECT id_dozymetru FROM Dozymetry WHERE typ = '{0}' AND nr_fabryczny = '{1}'", typDozymetru, nrDozymetru);
            int idDozymetru = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<int>(0);

            _Zapytanie = String.Format("SELECT MAX(id_karty) FROM Karta_przyjecia WHERE id_karty < {0} AND Id_dozymetru = {1}", IdKarty, idDozymetru);
            _OdpowiedzBazy = _BazaDanych.TworzTabeleDanych(_Zapytanie);

            if (null == _OdpowiedzBazy || null == _OdpowiedzBazy.Rows || null == _OdpowiedzBazy.Rows[0])
                return 0;

            try
            {
                return _OdpowiedzBazy.Rows[0].Field<int>(0);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        //---------------------------------------------------------------
        public Narzedzia.Sonda ZnajdzSondeWybranaPrzezUzytkwonika()
        //---------------------------------------------------------------
        {
            _Zapytanie = "SELECT Typ, Nr_fabryczny FROM Sondy WHERE id_sondy = (SELECT id_sondy FROM Wzorcowanie_Cezem WHERE " +
                         String.Format("id_wzorcowania = {0})", IdWzorcowania);

            DataRow wiersz = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0];

            return new Narzedzia.Sonda(wiersz.Field<string>(0), wiersz.Field<string>(1));
        }

        //---------------------------------------------------------------
        public override bool ZnajdzWiekszyArkusz()
        //---------------------------------------------------------------
        {
            _Zapytanie = "SELECT Max(Id_arkusza) FROM Wzorcowanie_cezem WHERE "
                       + String.Format("id_karty={0} AND rodzaj_wzorcowania = '{1}' AND id_arkusza > {2}",
                        IdKarty, _RodzajWzorcowania, IdArkusza);

            _OdpowiedzBazy = _BazaDanych.TworzTabeleDanych(_Zapytanie);

            try
            {
                IdArkusza = _OdpowiedzBazy.Rows[0].Field<short>(0);
            }
            catch (Exception)
            {
                return false;
            }
    
            return true;
        }
    }
}
