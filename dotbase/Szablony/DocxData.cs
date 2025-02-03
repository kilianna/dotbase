using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace DotBase.Szablony
{
    abstract class DocxData
    {
        protected abstract string FileName { get; }

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
