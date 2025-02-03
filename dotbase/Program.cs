using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Threading;
using DotBase.Logging;

namespace DotBase
{
    static class Program
    {
        static Logger log = Log.create();

        [STAThread]
        static void Main(string[] args)
        {
            zmienJezyk(Jezyk.PL);
            if (args.Length > 0 && (args[0] == "--version" || args[0] == "--pretty-version"))
            {
                System.Console.Out.Write(N.Wersja(args[0] == "--version").TrimStart('!'));
                return;
            }
            log("Start programu, wersja {0}", N.Wersja());
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            Application.Idle += new EventHandler(Application_Idle);
            Application.ApplicationExit += new EventHandler(Application_ApplicationExit);
            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.AddMessageFilter(new GlobalKeyFilter());
            //Application.Run(new Test.TestHtmlControl());
            Application.Run(new LogowanieForm());
            log("Normalne wyjście z aplikacji");
            Cleanup();
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            EncryptedLogger.Push("Non-UI thread unhandled exception: " + e.ExceptionObject.ToString() + "\r\n");
            EncryptedLogger.Stop();
        }

        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            log("Unhandled exception");
            var ex = e.Exception;
            EmergencyExit(ex.Message + "\r\n\r\n" + ex.ToString());
        }

        public static void EmergencyExit(string failureDescription)
        {
            log("Emergency exit with: {0}", failureDescription ?? "Unknown failure");
            BazaDanychWrapper.failure();
            BazaDanychWrapper.Zakoncz();
            if (failureDescription != null)
            {
                (new ExceptionForm(failureDescription)).ShowDialog();
            }
            Cleanup();
            Environment.Exit(99);
        }

        static void Cleanup()
        {
            log("Cleanup");
            BazaDanychWrapper.Zakoncz();
            EncryptedLogger.Stop();
        }

        static void Application_ApplicationExit(object sender, EventArgs e)
        {
            log("Wyjście z aplikacji - event");
            Cleanup();
        }

        static void Application_Idle(object sender, EventArgs e)
        {
            BazaDanychWrapper.onApplicationIdle();
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
