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
            zmienJezyk(Jezyk.PL);
            if (args.Length > 0 && (args[0] == "--version" || args[0] == "--pretty-version"))
            {
                System.Console.Out.Write(N.Wersja(args[0] == "--version").TrimStart('!'));
                return;
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.AddMessageFilter(new HelpKeyFilter());
            Application.Run(new LogowanieForm());
        }

        public static void zmienJezyk(Jezyk jezyk)
        {
            System.Globalization.CultureInfo ci;

            switch (jezyk)
            {
                case Jezyk.EN:
                    ci = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
                    ci.NumberFormat.CurrencyDecimalSeparator = ".";
                    ci.NumberFormat.NumberDecimalSeparator = ".";
                    ci.NumberFormat.PercentDecimalSeparator = ".";
                    ci.NumberFormat.CurrencyGroupSeparator = ",";
                    ci.NumberFormat.NumberGroupSeparator = ",";
                    ci.NumberFormat.PercentGroupSeparator = ",";
                    ci.DateTimeFormat.DateSeparator = "/";
                    ci.DateTimeFormat.ShortDatePattern = "M/d/yyyy";
                    break;
                default:
                    ci = System.Globalization.CultureInfo.CreateSpecificCulture("pl-PL");
                    ci.NumberFormat.CurrencyDecimalSeparator = ",";
                    ci.NumberFormat.NumberDecimalSeparator = ",";
                    ci.NumberFormat.PercentDecimalSeparator = ",";
                    ci.NumberFormat.CurrencyGroupSeparator = ".";
                    ci.NumberFormat.NumberGroupSeparator = ".";
                    ci.NumberFormat.PercentGroupSeparator = ".";
                    ci.DateTimeFormat.DateSeparator = "-";
                    ci.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd";
                    break;
            }
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
        }
    }
}
