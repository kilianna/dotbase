using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using Narzedzia;
using System.Data;

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
            protected String _Zapytanie;
            protected string _NrKarty;
            protected DocumentData m_documentData;

            //****************************************************************************************
            public Wydruki(string nrKarty) : this()
            //****************************************************************************************
            {
                this._NrKarty = nrKarty;
            }

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

                m_documentData = new DocumentData();
            }

            abstract protected bool fillDocument();
            abstract protected bool retrieveAllData();
            abstract protected bool saveDocument(string path);
            
            //****************************************************************************************
            public bool generateDocument(string path)
            //****************************************************************************************
            {
                try
                {
                    if (retrieveAllData() && fillDocument() && saveDocument(path))
                    {
                        System.Diagnostics.Process.Start(path);
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }

                return false;
            }

            //****************************************************************************************
            public void ZnajdzIdZlecenia(string nrKarty)
            //****************************************************************************************
            {
                _NrKarty = nrKarty;
                m_documentData.setValue(DocumentData.DataType.ID_KARTY, nrKarty);

                _Zapytanie = String.Format("SELECT id_zlecenia FROM Karta_przyjecia WHERE id_karty = {0}", _NrKarty);
                m_documentData.setValue(DocumentData.DataType.ID_ZLECENIA, _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0].Field<int>(0).ToString());
            }

            //****************************************************************************************
            virtual public bool PobierzDaneZleceniodawcy(String nrKarty)
            //****************************************************************************************
            {
                ZnajdzIdZlecenia(nrKarty);

                _Zapytanie = "SELECT zleceniodawca, adres, osoba_kontaktowa, Z.id_zleceniodawcy, telefon, faks, email FROM Zleceniodawca "
                           + "AS Z INNER JOIN ZLECENIA AS ZL ON Z.id_zleceniodawcy = ZL.id_zleceniodawcy WHERE "
                           + String.Format("ZL.id_zlecenia={0}", m_documentData.getValue(DocumentData.DataType.ID_ZLECENIA));

                DataRow wiersz = _BazaDanych.TworzTabeleDanych(_Zapytanie).Rows[0];

                // dodanie zleceniodawcy
                m_documentData.setValue(DocumentData.DataType.ZLECENIDOAWCA, wiersz.Field<string>(0));
                // dodanie adresu
                m_documentData.setValue(DocumentData.DataType.ADRES, wiersz.Field<string>(1));
                // dodanie osoby kontaktowe
                m_documentData.setValue(DocumentData.DataType.OSOBA_KONTAKTOWA, wiersz.Field<string>(2));
                // dodanie id zleceniodawcy
                m_documentData.setValue(DocumentData.DataType.ID_ZLECENIODAWCY, wiersz.Field<int>(3).ToString());
                // dodanie telefonu
                m_documentData.setValue(DocumentData.DataType.TELEFON, wiersz.Field<string>(4));
                // dodanie faksu
                m_documentData.setValue(DocumentData.DataType.FAKS, wiersz.Field<string>(5));
                // dodanie e-mail'u
                m_documentData.setValue(DocumentData.DataType.EMAIL, wiersz.Field<string>(6));

                return true;
            }

            //********************************************************************************************
            // Szablon podstawowy - jest jedynym używanym jeśli do tworzenia dokumentu trzeba użyć tylko jedengo szablonu.
            // Szablon podstawowy - jest niepełnym/pomocniczym szablonem jeśli zachodzi potrzeba użycia większej ilości szablonów.
            // Wtedy szablon podstawowy jest dołączany do szablonu głównego. To zadanie wykonuje już jednak klasa dzidzicząca.
            protected void PobierzSzablonPodstawowy(string sciezka)
            //********************************************************************************************
            {
                StreamReader streamReader = new StreamReader(sciezka);
                _SzablonPodstawowy = new StringBuilder(streamReader.ReadToEnd());
                streamReader.Close();
            }

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

                _SzablonPodstawowy = new StringBuilder(new StreamReader(_AktualnaSciezka).ReadToEnd());

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

                szablon = new StringBuilder(new StreamReader(_AktualnaSciezka).ReadToEnd());

                return true;
            }

            //****************************************************************************************
            virtual public bool WypelnijDaneZleceniodawcy(StringBuilder documentToFill)
            //****************************************************************************************
            {
                documentToFill.Replace("<!c3>", m_documentData.getValue(DocumentData.DataType.ZLECENIDOAWCA)) //.Replace(";", "<br>")
                            .Replace("<!c4>", m_documentData.getValue(DocumentData.DataType.ADRES))           //.Replace(";", "<br>")
                            .Replace("<!c5>", m_documentData.getValue(DocumentData.DataType.OSOBA_KONTAKTOWA))
                            .Replace("<!c6>", m_documentData.getValue(DocumentData.DataType.ID_ZLECENIODAWCY))
                            .Replace("<!c7>", m_documentData.getValue(DocumentData.DataType.TELEFON))
                            .Replace("<!c8>", m_documentData.getValue(DocumentData.DataType.FAKS))
                            .Replace("<!c9>", m_documentData.getValue(DocumentData.DataType.EMAIL));

                return true;
            }

            //********************************************************************************************
            // Jako, że w klasy dziedziczące mogą wymagać większej liczby szablonów ostatecznie zapisanym/wypełnionym
            // szablonem nie musi być szablon podstawowy. Dlatego istnieje możliwość nadpisania tej metody.
            virtual protected bool ZapiszPlikWynikowy(string sciezka)
            //********************************************************************************************
            {
                StreamWriter streamWriter = new StreamWriter(sciezka, false, Encoding.UTF8);
                streamWriter.Write(_SzablonPodstawowy.ToString());
                streamWriter.Close();
                return true;
            }
        }
    }
}
