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
        static Logger logger = Log.create();

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

        public static void log(bool skipDbLog, BazaDanychWrapper bazaDanych, string wiadomosc, string zapytanie, string parametry = "")
        {
            try
            {
                DateTime now = DateTime.Now;
                string user = "#niezalogowany";
                if (LogowanieForm.Instancja != null && LogowanieForm.Instancja.Users.CurrentUser != null)
                {
                    user = LogowanieForm.Instancja.Users.CurrentUser.Name ?? user;
                }
                string stackTrace = "\r\n" + Environment.StackTrace;
                stackTrace = Regex.Replace(stackTrace, "\n[^\n]+ (System|Microsoft|DotBase\\.Log)\\.[^\n]+", "");
                logger("Zapytanie od {0}: {1}\r\n   QUERY: {2}\r\n   {3}", user, wiadomosc, zapytanie,
                    parametry.Trim(',', ' ', '\t', '\r', '\n').Replace("\n", "\n   "));
                if (!skipDbLog)
                {
                    bazaDanych.log(now, user, wiadomosc, zapytanie, stackTrace.Trim(), parametry.Trim());
                }
            }
            catch (Exception ex)
            {
                logger("Unhandled exception while inserting database log entry: {0}", ex.Message);
            }
        }

    }
}
