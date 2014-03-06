using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using Narzedzia;

namespace DotBase
{
    namespace Dokumenty
    {
        abstract class Wydruki
        {
            #region Scieżki
            DocumentationPathsLoader _DocumentationPathsLoader = new DocumentationPathsLoader();

            // pliki pomocnicze
            private readonly String SCIEZKA_PLIK_POMOCNICZY_DAWKA;
            private readonly String SCIEZKA_PLIK_POMOCNICZY_MOC_DAWKI;
            private readonly String SCIEZKA_PLIK_POMOCNICZY_SKAZENIA;
            private readonly String SCIEZKA_PLIK_POMOCNICZY_SYGNALIZACJA_DAWKI;
            private readonly String SCIEZKA_PLIK_POMOCNICZY_SYGNALIZACJA_MOCY_DAWKI;
            private readonly String SCIEZKA_PLIK_POMOCNICZY_SYGNALIZACJA_MOCY_DAWKI_OPIS;
            
            // pisma całościowe
            private readonly String SCIEZKA_KARTA_PRZYJECIA;
            private readonly String SCIEZKA_MELDUNEK;
            private readonly String SCIEZKA_PISMO_PRZEWODNIE;
            private readonly String SCIEZKA_SWIADECTWO;

            private readonly String SCIEZKA_PROTOKOL_DAWKA;
            private readonly String SCIEZKA_PROTOKOL_MOC_DAWKI;
            private readonly String SCIEZKA_PROTOKOL_SKAZENIA;
            private readonly String SCIEZKA_PROTOKOL_SYGNALIZACJA_DAWKI;
            private readonly String SCIEZKA_PROTOKOL_SYGNALIZACJA_MOCY_DAWKI_OPIS;
            private readonly String SCIEZKA_PROTOKOL_SYGNALIZACJA_MOCY_DAWKI_TABELA;

            #endregion

            private string _AktualnaSciezka;

            protected BazaDanychWrapper _BazaDanych;
            protected StringBuilder _SzablonPodstawowy;
            protected List<String> _DaneWypelniajace; // dane które wypełnią szablon
            protected String _Zapytanie;
            protected string _NrKarty;
            protected StreamReader _Fin;
            protected StreamWriter _Fout;

            //****************************************************************************************
            public Wydruki()
            //****************************************************************************************
            {
                _BazaDanych = new BazaDanychWrapper();
                
               // pisma pomocnicze
               SCIEZKA_PLIK_POMOCNICZY_DAWKA = _DocumentationPathsLoader.GetPath("SwiadectwoSzablonPlikiPomocnicze") + "Dawka.txt";
               SCIEZKA_PLIK_POMOCNICZY_MOC_DAWKI = _DocumentationPathsLoader.GetPath("SwiadectwoSzablonPlikiPomocnicze") + "MocDawki.txt";
               SCIEZKA_PLIK_POMOCNICZY_SKAZENIA = _DocumentationPathsLoader.GetPath("SwiadectwoSzablonPlikiPomocnicze") + "Skazenia.txt";
               SCIEZKA_PLIK_POMOCNICZY_SYGNALIZACJA_DAWKI = _DocumentationPathsLoader.GetPath("SwiadectwoSzablonPlikiPomocnicze") + "SygDawki.txt";
               SCIEZKA_PLIK_POMOCNICZY_SYGNALIZACJA_MOCY_DAWKI = _DocumentationPathsLoader.GetPath("SwiadectwoSzablonPlikiPomocnicze") + "SygMocyDawki.txt";
               SCIEZKA_PLIK_POMOCNICZY_SYGNALIZACJA_MOCY_DAWKI_OPIS = _DocumentationPathsLoader.GetPath("SwiadectwoSzablonPlikiPomocnicze") + "SygMocyDawkiOpis.txt";

            // pliki całościowe
               SCIEZKA_KARTA_PRZYJECIA = _DocumentationPathsLoader.GetPath("KartaPrzyjeciaSzablon") + "KartaPrzyjecia.html";
               SCIEZKA_MELDUNEK = _DocumentationPathsLoader.GetPath("MeldunekSzablon") + "Meldunek.html";
               SCIEZKA_PISMO_PRZEWODNIE = _DocumentationPathsLoader.GetPath("PismoPrzewodnieSzablon") + "PismoPrzewodnie.html";
               SCIEZKA_SWIADECTWO = _DocumentationPathsLoader.GetPath("SwiadectwoSzablon") + "Swiadectwo.html";

               SCIEZKA_PROTOKOL_DAWKA = _DocumentationPathsLoader.GetPath("ProtokolyDawkaSzablon") + "Dawka.html";
               SCIEZKA_PROTOKOL_MOC_DAWKI = _DocumentationPathsLoader.GetPath("ProtokolyMocDawkiSzablon") + "MocDawki.html";
               SCIEZKA_PROTOKOL_SKAZENIA = _DocumentationPathsLoader.GetPath("ProtokolySkazeniaSzablon") + "Skazenia.html";
               SCIEZKA_PROTOKOL_SYGNALIZACJA_DAWKI = _DocumentationPathsLoader.GetPath("ProtokolySygnalizacjaDawkiSzablon") + "SygDawki.html";
               SCIEZKA_PROTOKOL_SYGNALIZACJA_MOCY_DAWKI_OPIS = _DocumentationPathsLoader.GetPath("ProtokolySygnalizacjaMocyDawkiSzablon") + "SygMocyDawkiOpis.html";
               SCIEZKA_PROTOKOL_SYGNALIZACJA_MOCY_DAWKI_TABELA = _DocumentationPathsLoader.GetPath("ProtokolySygnalizacjaMocyDawkiSzablon") + "SygMocyDawkiTabela.html";

            }

            //****************************************************************************************
            protected void InicjalizujListeDanychWypelniajacych(int rozmiar)
            //****************************************************************************************
            {
                _DaneWypelniajace = new List<string>(rozmiar);
                for (int i = 0; i < rozmiar; ++i)
                    _DaneWypelniajace.Add("");
            }

            //********************************************************************************************
            // Szablon podstawowy - jest jedynym używanym jeśli do tworzenia dokumentu trzeba użyć tylko jedengo szablonu.
            // Szablon podstawowy - jest niepełnym/pomocniczym szablonem jeśli zachodzi potrzeba użycia większej ilości szablonów.
            // Wtedy szablon podstawowy jest dołączany do szablonu głównego. To zadanie wykonuje już jednak klasa dzidzicząca.
            protected void PobierzSzablonPodstawowy(string sciezka)
            //********************************************************************************************
            {
                _Fin = new StreamReader(sciezka);
                _SzablonPodstawowy = new StringBuilder(_Fin.ReadToEnd());
                _Fin.Close();
            }

            //****************************************************************************************
            // Tworzenie dokumentu
            abstract public bool UtworzDokument(string sciezka);
            //****************************************************************************************

            //****************************************************************************************
            protected bool WczytajSzablon(StaleWzorcowan.stale typ)
            // wczytuje odpowiedni szablon który po uzupełnieniu stanie się dokumentem do wydruku
            //****************************************************************************************
            {
                switch (typ)
                {
                    case StaleWzorcowan.stale.KARTA_PRZYJECIA:
                        _AktualnaSciezka = SCIEZKA_KARTA_PRZYJECIA;
                        break;
                    case StaleWzorcowan.stale.MELDUNEK:
                        _AktualnaSciezka = SCIEZKA_MELDUNEK;
                        break;
                    case StaleWzorcowan.stale.PLIK_POMOCNICZY_DAWKA:
                        _AktualnaSciezka = SCIEZKA_PLIK_POMOCNICZY_DAWKA;
                        break;
                    case StaleWzorcowan.stale.PLIK_POMOCNICZY_MOC_DAWKI:
                        _AktualnaSciezka = SCIEZKA_PLIK_POMOCNICZY_MOC_DAWKI;
                        break;
                    case StaleWzorcowan.stale.PLIK_POMOCNICZY_SKAZENIA:
                        _AktualnaSciezka = SCIEZKA_PLIK_POMOCNICZY_SKAZENIA;
                        break;
                    case StaleWzorcowan.stale.PLIK_POMOCNICZY_SYGNALIZACJA_DAWKI:
                        _AktualnaSciezka = SCIEZKA_PLIK_POMOCNICZY_SYGNALIZACJA_DAWKI;
                        break;
                    case StaleWzorcowan.stale.PLIK_POMOCNICZY_SYGNALIZACJA_MOCY_DAWKI:
                        _AktualnaSciezka = SCIEZKA_PLIK_POMOCNICZY_SYGNALIZACJA_MOCY_DAWKI;
                        break;
                    case StaleWzorcowan.stale.PLIK_POMOCNICZY_SYGNALIZACJA_MOCY_DAWKI_OPIS:
                        _AktualnaSciezka = SCIEZKA_PLIK_POMOCNICZY_SYGNALIZACJA_MOCY_DAWKI_OPIS;
                        break;
                    case StaleWzorcowan.stale.PISMO_PRZEWODNIE:
                        _AktualnaSciezka = SCIEZKA_PISMO_PRZEWODNIE;
                        break;
                    case StaleWzorcowan.stale.PROTOKOL_DAWKA:
                        _AktualnaSciezka = SCIEZKA_PROTOKOL_DAWKA;
                        break;
                    case StaleWzorcowan.stale.PROTOKOL_MOC_DAWKI:
                        _AktualnaSciezka = SCIEZKA_PROTOKOL_MOC_DAWKI;
                        break;
                    case StaleWzorcowan.stale.PROTOKOL_SKAZENIA:
                        _AktualnaSciezka = SCIEZKA_PROTOKOL_SKAZENIA;
                        break;
                    case StaleWzorcowan.stale.PROTOKOL_SYGNALIZACJA_DAWKI:
                        _AktualnaSciezka = SCIEZKA_PROTOKOL_SYGNALIZACJA_DAWKI;
                        break;
                    case StaleWzorcowan.stale.PROTOKOL_SYGNALIZACJA_MOCY_DAWKI_OPIS:
                        _AktualnaSciezka = SCIEZKA_PROTOKOL_SYGNALIZACJA_MOCY_DAWKI_OPIS;
                        break;
                    case StaleWzorcowan.stale.PROTOKOL_SYGNALIZACJA_MOCY_DAWKI_TABELA:
                        _AktualnaSciezka = SCIEZKA_PROTOKOL_SYGNALIZACJA_MOCY_DAWKI_TABELA;
                        break;
                    case StaleWzorcowan.stale.SWIADECTWO:
                        _AktualnaSciezka = SCIEZKA_SWIADECTWO;
                        break;
                    default:
                        return false;
                }

                StreamReader fin = new StreamReader(_AktualnaSciezka);
                _SzablonPodstawowy = new StringBuilder(fin.ReadToEnd());

                return true;
            }

            //****************************************************************************************
            protected bool WczytajSzablon(StaleWzorcowan.stale typ, ref StringBuilder szablon)
            // wczytuje odpowiedni szablon który po uzupełnieniu stanie się dokumentem do wydruku
            //****************************************************************************************
            {
                switch (typ)
                {
                    case StaleWzorcowan.stale.KARTA_PRZYJECIA:
                        _AktualnaSciezka = SCIEZKA_KARTA_PRZYJECIA;
                        break;
                    case StaleWzorcowan.stale.MELDUNEK:
                        _AktualnaSciezka = SCIEZKA_MELDUNEK;
                        break;
                    case StaleWzorcowan.stale.PLIK_POMOCNICZY_DAWKA:
                        _AktualnaSciezka = SCIEZKA_PLIK_POMOCNICZY_DAWKA;
                        break;
                    case StaleWzorcowan.stale.PLIK_POMOCNICZY_MOC_DAWKI:
                        _AktualnaSciezka = SCIEZKA_PLIK_POMOCNICZY_MOC_DAWKI;
                        break;
                    case StaleWzorcowan.stale.PLIK_POMOCNICZY_SKAZENIA:
                        _AktualnaSciezka = SCIEZKA_PLIK_POMOCNICZY_SKAZENIA;
                        break;
                    case StaleWzorcowan.stale.PLIK_POMOCNICZY_SYGNALIZACJA_DAWKI:
                        _AktualnaSciezka = SCIEZKA_PLIK_POMOCNICZY_SYGNALIZACJA_DAWKI;
                        break;
                    case StaleWzorcowan.stale.PLIK_POMOCNICZY_SYGNALIZACJA_MOCY_DAWKI:
                        _AktualnaSciezka = SCIEZKA_PLIK_POMOCNICZY_SYGNALIZACJA_MOCY_DAWKI;
                        break;
                    case StaleWzorcowan.stale.PISMO_PRZEWODNIE:
                        _AktualnaSciezka = SCIEZKA_PISMO_PRZEWODNIE;
                        break;
                    case StaleWzorcowan.stale.PROTOKOL_DAWKA:
                        _AktualnaSciezka = SCIEZKA_PROTOKOL_DAWKA;
                        break;
                    case StaleWzorcowan.stale.PROTOKOL_MOC_DAWKI:
                        _AktualnaSciezka = SCIEZKA_PROTOKOL_MOC_DAWKI;
                        break;
                    case StaleWzorcowan.stale.PROTOKOL_SKAZENIA:
                        _AktualnaSciezka = SCIEZKA_PROTOKOL_SKAZENIA;
                        break;
                    case StaleWzorcowan.stale.PROTOKOL_SYGNALIZACJA_DAWKI:
                        _AktualnaSciezka = SCIEZKA_PROTOKOL_SYGNALIZACJA_DAWKI;
                        break;
                    case StaleWzorcowan.stale.PROTOKOL_SYGNALIZACJA_MOCY_DAWKI_OPIS:
                        _AktualnaSciezka = SCIEZKA_PROTOKOL_SYGNALIZACJA_MOCY_DAWKI_OPIS;
                        break;
                    case StaleWzorcowan.stale.PROTOKOL_SYGNALIZACJA_MOCY_DAWKI_TABELA:
                        _AktualnaSciezka = SCIEZKA_PROTOKOL_SYGNALIZACJA_MOCY_DAWKI_TABELA;
                        break;
                    case StaleWzorcowan.stale.SWIADECTWO:
                        _AktualnaSciezka = SCIEZKA_SWIADECTWO;
                        break;
                    default:
                        return false;
                }

                StreamReader fin = new StreamReader(_AktualnaSciezka);
                szablon = new StringBuilder(fin.ReadToEnd());

                return true;
            }

            //********************************************************************************************
            // Jako, że w klasy dziedziczące mogą wymagać większej liczby szablonów ostatecznie zapisanym/wypełnionym
            // szablonem nie musi być szablon podstawowy. Dlatego istnieje możliwość nadpisania tej metody.
            virtual protected bool ZapiszPlikWynikowy(string sciezka)
            //********************************************************************************************
            {
                _Fout = new StreamWriter(sciezka, false, Encoding.UTF8);
                _Fout.Write(_SzablonPodstawowy.ToString());
                _Fout.Close();
                return true;
            }
        }
    }
}
