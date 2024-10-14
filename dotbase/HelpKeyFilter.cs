using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using DotBase.Logging;

namespace DotBase
{
    class HelpKeyFilter : IMessageFilter
    {
        Logger log = Log.create();
        const int WM_KEYUP = 0x0101;
        const int VK_F1 = 0x70;

        const int WM_LBUTTONDOWN = 0x0201;
        const int WM_LBUTTONUP = 0x0202;

        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == WM_LBUTTONUP || m.Msg == WM_LBUTTONDOWN)
            {
                var c = Control.FromChildHandle(m.HWnd);
                if (c == null)
                {
                    log("msg 0x{0:X16}, msg 0x{1:X8}, LParam 0x{2:X8}, WParam 0x{3:X8}", m.HWnd.ToInt64(), m.Msg, m.LParam.ToInt64(), m.WParam.ToInt64());
                }
                else
                {
                    var path = "";
                    var form = c;
                    while (form != null && !(form is Form))
                    {
                        path += ">" + form.Name;
                        form = form.Parent;
                    }
                    log("msg 0x{0:X16}, msg 0x{1:X8}, LParam 0x{2:X8}, WParam 0x{3:X8}, {4}, {5}", m.HWnd.ToInt64(), m.Msg, m.LParam.ToInt64(), m.WParam.ToInt64(), form == null ? "unknown form" : form.Name, path);
                }
            }
            if (m.Msg == WM_KEYUP && m.WParam.ToInt32() == VK_F1)
            {
                StartManual();
            }
            return false;
        }

        private void StartManual()
        {
            string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string pdf = null;
            for (int i = 0; i < 10; i++)
            {
                pdf = path + @"\Instrukcja.pdf";
                if (CheckPath(pdf)) break;
                pdf = path + @"\manual\Instrukcja.pdf";
                if (CheckPath(pdf)) break;
                pdf = path + @"\Instrukcja\Instrukcja.pdf";
                if (CheckPath(pdf)) break;
                pdf = null;
                path = path + @"\..";
            }
            if (pdf != null)
            {
                System.Diagnostics.Process.Start(pdf);
            }
            else
            {
                MessageBox.Show("Plik PDF z instrukcją nie został znaleziony.\r\n" +
                    "Sprawdź, czy plik \"Instrukcja.pdf\" istnieje i znajduje się w katalogu programu lub " +
                    "w jednym z katalogów nadrzędnych.", "Nie można znaleźć pliku pomocy", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool CheckPath(string pdf)
        {
            try
            {
                return File.Exists(pdf);
            }
            catch
            {
                return false;
            }
        }
    }
}
