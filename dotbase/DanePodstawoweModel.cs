using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotBase
{
    class DanePodstawoweModel
    {
        public string nrKarty;
        public string nrArkusza;
        public DateTime data;

        public DanePodstawoweModel(string nrKarty, string nrArkusza, DateTime data)
        {
            this.nrKarty = nrKarty;
            this.nrArkusza = nrArkusza;
            this.data = data;
        }
    }
}
