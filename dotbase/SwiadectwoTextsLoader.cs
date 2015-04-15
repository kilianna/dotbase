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

        public SwiadectwoTextsLoader()
        {
            XMLFile = XDocument.Load(basePath + @"\SwiadectwoWydrukTeksty.xml");
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

        public String GetText(bool polon, bool brunschweig)
        {
            var q = from document in XMLFile.Root.Descendants("miejsceWzorcowaniaZrodelPowierzchniowych")
                    where Boolean.Parse(document.Attribute("polon").Value) == polon &&
                          Boolean.Parse(document.Attribute("braunschweig").Value) == brunschweig
                    select document.Value;
            return q.ElementAt(0);
        }

        public String GetText(String elementToFind)
        {
            var q = from document in XMLFile.Root.Descendants(elementToFind)
                    select document.Value;
            return q.ElementAt(0);
        }
        
    }
}
