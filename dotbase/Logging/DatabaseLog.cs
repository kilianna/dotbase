using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Security.Cryptography;

namespace DotBase.Logging
{
    class DatabaseLog
    {

        public class Wpis
        {
            public bool dodaj;
            public string wiadomosc;
            public string zapytanieSelect;

            public Wpis()
            {
                dodaj = false;
            }

            public Wpis(string w, string sel = null)
            {
                dodaj = true;
                wiadomosc = w;
                zapytanieSelect = sel;
            }
        }

        public static void log(BazaDanychWrapper bazaDanych, string wiadomosc, string zapytanie, string parametry = "")
        {
            // TODO: Try catch, żeby log nigdy nie kończył się wyjątkiem
            DateTime now = DateTime.Now;
            string user = LogowanieForm.Instancja == null ? "#niezalogowany" :
                LogowanieForm.Instancja.Wybrany.nazwa == null ? "#niezalogowany" :
                LogowanieForm.Instancja.Wybrany.nazwa;
            string stackTrace = "\r\n" + Environment.StackTrace;
            stackTrace = Regex.Replace(stackTrace, "\n[^\n]+ (System|Microsoft|DotBase\\.Log)\\.[^\n]+", "");
            Debug.WriteLine(String.Format("### {0} {1}: {2}\r\n QUERY: {3}\r\n STACK: {4}", now, user, wiadomosc, zapytanie, stackTrace).Replace("\n", "\n###"));
            bazaDanych.log(now, user, wiadomosc, zapytanie, stackTrace.Trim(), parametry.Trim());
        }

    }
}
