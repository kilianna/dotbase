using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using DotBase.Logging;
using System.Threading;

namespace DotBase
{
    class GlobalKeyFilter : IMessageFilter
    {
        Logger log = Log.create();
        const int WM_KEYUP = 0x0101;
        const int VK_F1 = 0x70;
        const int VK_F5 = 0x74;
        const int VK_F12 = 0x7B;

        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == WM_KEYUP)
            {
                switch (m.WParam.ToInt32())
                {
                    case VK_F1:
                        log("Starting manual");
                        StartManual();
                        break;
                    case VK_F5:
                        DiagnosticsOutput();
                        break;
                    case VK_F12:
                        DebugOptions.stopTest = true;
                        break;
                }
            }
            return false;
        }

        private void DiagnosticsOutput()
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "Folder diagnostyczny";
                folderDialog.ShowNewFolderButton = true;
                folderDialog.RootFolder = Environment.SpecialFolder.Desktop;
                folderDialog.Description = "Wybierz folder, gdzie zostaną zapisane pliki konieczne do zdiagnozowania problemu.";
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    string path = folderDialog.SelectedPath;
                    log("F5 - Saving diagnostics to '{0}'", path);
                    EncryptedLogger.Flush();
                    try {
                        EncryptedLogger.CopyTo(path);
                        MyMessageBox.Show(
                            String.Format("Pliki skopiowane do {0}", path),
                            "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    } catch (Exception ex) {
                        log("Saving diagnostics error: {0}", ex.ToString());
                        MyMessageBox.Show(
                            String.Format("Błąd kopiowania plików: {0}\r\n\r\nSkopiuj pliki ręczenie.", ex.Message),
                            "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
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
                MyMessageBox.Show("Plik PDF z instrukcją nie został znaleziony.\r\n" +
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
