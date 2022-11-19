using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace DotBase
{
    public enum Jezyk
    {
        PL,
        EN,
    }

    public static class JezykTools
    {
        public static string kocowka(Jezyk jezyk)
        {
            switch (jezyk)
            {
                case Jezyk.PL: return "";
                default: return "-" + jezyk.ToString().ToLower();
            }
        }
    }

    class DocumentationPathsLoader
    {
        private XDocument XMLFile = new XDocument();
        private String basePath = Directory.GetCurrentDirectory();

        public DocumentationPathsLoader()
        {
            XMLFile = XDocument.Load(basePath + @"\PathConfig.xml");
        }

        public String GetPath(String type, Jezyk jezyk)
        {
            var a = jezyk.ToString().ToLower();
            try
            {
                var q1 = from document in XMLFile.Descendants("document")
                         where document.Attribute("type").Value == type + JezykTools.kocowka(jezyk)
                         select document.Attribute("path").Value;
                return String.Format("{0}/{1}", basePath, q1.ElementAt(0));
            }
            catch (Exception) { }
            var q2 = from document in XMLFile.Descendants("document")
                     where document.Attribute("type").Value == type
                     select document.Attribute("path").Value;
            return String.Format("{0}/{1}", basePath, q2.ElementAt(0));
        }
    }
}
