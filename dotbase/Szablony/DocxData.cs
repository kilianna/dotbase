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
        public Jezyk jezyk;

        protected abstract string FileName { get; }

        protected virtual bool PreProcess(IWin32Window owner)
        {
            return true;
        }

        public DocxData()
        {
            stale = Constants.getInstance();
        }

        public void Generate(IWin32Window owner)
        {
            bool valid = PreProcess(owner);
            if (valid)
            {
                Type thisType = this.GetType();
                var win = new DocxWindow();
                var outputFile = Path.Combine(N.getProgramDir(), FileName);
                var templateFile = String.Format(@"{0}\Szablony\{1}{2}.xml", N.getProgramDir(), thisType.Name, JezykTools.kocowka(jezyk));
                if (!File.Exists(templateFile)) {
                    templateFile = String.Format(@"{0}\Szablony\{1}.xml", N.getProgramDir(), thisType.Name);
                }
                win.generate(owner, templateFile, this, outputFile);
                win.Dispose();
            }
        }

        protected object DirGroup<T>(T id, int count = 2)
        {
            var idStr = id.ToString();
            if (idStr.Length > count)
            {
                return idStr.Substring(0, idStr.Length - count) + new string('x', count);
            }
            else
            {
                return new string('x', count);
            }
        }
    }
}
