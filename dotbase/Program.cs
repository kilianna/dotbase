using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Threading;

namespace DotBase
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length > 0 && (args[0] == "--version" || args[0] == "--pretty-version")) {
                System.Console.Out.Write(N.Wersja(args[0] == "--version").TrimStart('!'));
                return;
            }
            var ci = System.Globalization.CultureInfo.CreateSpecificCulture("pl-PL");
            ci.NumberFormat.CurrencyDecimalSeparator = ",";
            ci.NumberFormat.NumberDecimalSeparator = ",";
            ci.NumberFormat.PercentGroupSeparator = ",";
            ci.NumberFormat.CurrencyGroupSeparator = ".";
            ci.NumberFormat.NumberGroupSeparator = ".";
            ci.NumberFormat.PercentGroupSeparator = ".";
            ci.DateTimeFormat.DateSeparator = "-";
            ci.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd";
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LogowanieForm());
        }
    }
}
