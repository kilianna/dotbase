using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace DotBase
{
    class SwiadectwoTextsLoader
    {
        private XDocument XMLFile = new XDocument();
        private String basePath = Directory.GetCurrentDirectory();

        public SwiadectwoTextsLoader(Jezyk jezyk)
        {
            XMLFile = XDocument.Load(basePath + @"\SwiadectwoWydrukTeksty" + JezykTools.kocowka(jezyk) + ".xml");
        }

        public String GetText(String whatTake, bool dawka, bool mocDawki, bool skażenia)
        {
            var q = from document in XMLFile.Root.Descendants("zakresWzorcowania")
                    where Boolean.Parse(document.Attribute("dawka").Value) == (dawka) &&
                          Boolean.Parse(document.Attribute("mocDawki").Value) == (mocDawki) &&
                          Boolean.Parse(document.Attribute("skażenia").Value) == (skażenia)
                    select document.Element(whatTake).Value;
            return q.ElementAt(0);
        }

        public String GetTextInfo(String whatTake, bool si)
        {
            var q = from document in XMLFile.Root.Descendants("informacja")
                    where Boolean.Parse(document.Attribute("si").Value) == (si)
                    select document.Element(whatTake).Value;
            return q.ElementAt(0);
        }


        internal string GetTextInfo(string whatTake, bool si, bool polon, bool bruschweig)
        {
            var q = from document in XMLFile.Root.Descendants("informacja")
                    where Boolean.Parse(document.Attribute("si").Value) == (si) &&
                          Boolean.Parse(document.Attribute("polon").Value) == (polon) &&
                          Boolean.Parse(document.Attribute("braunschweig").Value) == (bruschweig)
                    select document.Element(whatTake).Value;
            return q.ElementAt(0);
        }
    }
}
