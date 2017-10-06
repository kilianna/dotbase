using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace DotBase
{
    class DocumentationPathsLoader
    {
        private XDocument XMLFile = new XDocument();
        private String basePath = Directory.GetCurrentDirectory();

        public DocumentationPathsLoader()
        {
            XMLFile = XDocument.Load(basePath + @"\PathConfig.xml");
        }

        public String GetPath(String type)
        {
            var q = from document in XMLFile.Descendants("document")
                    where document.Attribute("type").Value == type
                    select document.Attribute("path").Value;
            return String.Format("{0}/{1}", basePath, q.ElementAt(0));
        }
    }
}
