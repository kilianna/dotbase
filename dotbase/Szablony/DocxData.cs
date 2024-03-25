using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Globalization;

namespace DotBase.Szablony
{
    class DocxDateTime
    {
        public Dictionary<string, string> tekst = new Dictionary<string, string>();
        public int rok;
        public int miesiac;
        public int dzien;
        public int godzina;
        public int minuta;
        public int sekunda;
        public int milisekunda;

        public DocxDateTime(DateTime dateTime)
        {
            tekst[Jezyk.EN.ToString()] = dateTime.ToString("dd MMMM yyyy", new CultureInfo("en-US"));
            tekst[Jezyk.PL.ToString()] = dateTime.ToString("dd MMMM yyyy");
            rok = dateTime.Year;
            miesiac = dateTime.Month;
            dzien = dateTime.Day;
            godzina = dateTime.Hour;
            minuta = dateTime.Minute;
            sekunda = dateTime.Second;
            milisekunda = dateTime.Millisecond;
        }
    }

    abstract class DocxData
    {
        public Constants stale;

        protected abstract string FileName { get; }

        public DocxData()
        {
            stale = Constants.getInstance();
        }

        public void Generate(IWin32Window owner)
        {
            Type thisType = this.GetType();
            var win = new DocxWindow();
            var outputFile = Path.Combine(N.getProgramDir(), FileName);
            win.generate(owner, thisType.Name, this, outputFile);
            win.Dispose();
        }

        protected string DirGroup(string id, int count = 2)
        {
            if (id.Length > count)
            {
                return id.Substring(0, id.Length - count) + new string('x', count);
            }
            else
            {
                return new string('x', count);
            }
        }
    }
}
