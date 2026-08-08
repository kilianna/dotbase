using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace DotBase
{
    class Translacja
    {
        public Jezyk jezyk;
        public Szablon.Row_Slownik[] slownik;
        private BazaDanychWrapper baza;

        public Translacja(Jezyk jezyk)
        {
            this.jezyk = jezyk;
            baza = new BazaDanychWrapper();
            slownik = baza.Slownik.GET();
        }

        public void AddTranslation(string pl, string other, bool permanent)
        {
            Szablon.Row_Slownik row = null;

            if (jezyk == Jezyk.PL)
            {
                return;
            }

            foreach (Szablon.Row_Slownik row2 in slownik)
            {
                if (row2.PL == pl)
                {
                    row = row2;
                    break;
                }
            }
            if (row == null)
            {
                row = new Szablon.Row_Slownik();
                row.PL = pl;
                var slownikList = new List<Szablon.Row_Slownik>(slownik);
                slownikList.Add(row);
                slownik = slownikList.ToArray();
            }
            switch (jezyk)
            {
                case Jezyk.EN:
                    row.EN = other;
                    break;
                default:
                    throw new ApplicationException("Nie zaimplementowano tego języka");
            }

            if (permanent)
            {
                AddTranslationToDb(pl, other);
            }
        }

        private void AddTranslationToDb(string pl, string other)
        {
            var existingRow = baza.Slownik
                .SELECT()
                .WHERE().PL(pl)
                .GET_OPTIONAL();
            if (existingRow != null)
            {
                switch (jezyk)
                {
                    case Jezyk.EN:
                        baza.Slownik.UPDATE().EN(other).WHERE().PL(pl).EXECUTE();
                        break;
                    default:
                        throw new ApplicationException("Nie zaimplementowano tego języka");
                }
            }
            else
            {
                switch (jezyk)
                {
                    case Jezyk.EN:
                        baza.Slownik.INSERT().PL(pl).EN(other).EXECUTE();
                        break;
                    default:
                        throw new ApplicationException("Nie zaimplementowano tego języka");
                }
            }
        }

        string Formatter(int index, string text)
        {
            if (text == "")
            {
                return "";
            }
            switch (index)
            {
                case 0:
                    return text;
                case 1:
                    return text.ToLower();
                case 2:
                    return text.ToUpper();
                case 3:
                    return text.Substring(0, 1).ToUpper() + text.Substring(1).ToLower();
                case 4:
                    return Regex.Replace(text, @"\b\w", m => m.Value.ToUpper());
                default:
                    throw new ApplicationException();
            }
        }

        public string Translate(string text, bool allowNoTranslation = false, IWin32Window owner = null)
        {
            text = text.Trim();

            // W liczbach tłumaczy tylko kropkę/przecinek
            if (Regex.IsMatch(text, @"^[0-9.,-]+$"))
            {
                switch (jezyk)
                {
                    case Jezyk.EN:
                        return Regex.Replace(text, @"[.,]", ".");
                    case Jezyk.PL:
                        return Regex.Replace(text, @"[.,]", ",");
                    default:
                        throw new ApplicationException("Nie zaimplementowano tego języka");
                }
            }

            // Nie tłumacz z polskiego na polski
            if (jezyk == Jezyk.PL)
            {
                return text;
            }

            // Probuj przetłumaczyć różnymi formatując tekst na różne sposoby
            for (var i = 0; i < 5; i++)
            {
                foreach (var row in slownik)
                {
                    if (Formatter(i, row.PL) == text)
                    {
                        switch (jezyk)
                        {
                            case Jezyk.EN:
                                return row.EN;
                            default:
                                throw new ApplicationException("Nie zaimplementowano tego języka");
                        }
                    }
                }
            }

            // Zrwóc nie przetłumaczony tekst, jeżeli dopuszczamy brak tłumaczenia
            if (allowNoTranslation)
            {
                return text;
            }

            // Wyświetl okno ręcznego tłumaczenia
            return ShowDialog(text, owner);
        }

        public string ShowDialog(string text, IWin32Window owner = null)
        {
            var form = new TranslacjaForm(text.Trim(), jezyk, this);
            if (owner != null)
            {
                form.ShowDialog(owner);
            }
            else
            {
                form.ShowDialog();
            }
            return form.getResult();
        }
    }
}
