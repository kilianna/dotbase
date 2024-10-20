using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DotBase.Szablony
{
    class DocxData
    {
        public void generate(IWin32Window owner, string outputFile)
        {
            Type thisType = this.GetType();
            var win = new DocxWindow();
            win.generate(owner, thisType.Name, this, outputFile);
            win.Dispose();
        }
    }
}
