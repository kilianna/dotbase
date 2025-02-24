using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace DotBase.Szablony
{
    public partial class DocxWindow : Form
    {
        private DocxGenerator gen = new DocxGenerator();
        string templateFile;
        string tempFile;
        string outputFile;
        string outputFileDir;
        string outputFileWithoutExt;
        IWin32Window owner;

        public DocxWindow()
        {
            InitializeComponent();
            gen.onFinished += new DocxGenerator.onFinishedDelegate(gen_onFinished);
        }

        void log(string text)
        {
            logBox.Text += text.TrimEnd().Replace("\r", "").Replace("\n", "\r\n") + "\r\n";
            logBox.SelectionLength = 0;
            logBox.SelectionStart = logBox.Text.Length;
            logBox.ScrollToCaret();
        }

        void error(string text)
        {
            log("\n" + text);
            log("\n\nBłąd");
            log("");
            infoLabel.Text = "Błąd. Informacje diagnostyczne:";
            infoLabel.ForeColor = Color.Red;
        }

        public void generate(IWin32Window owner, string templateFile, object data, string outputFile)
        {
            this.owner = owner;
            this.outputFile = outputFile;
            this.templateFile = templateFile;
            outputFileDir = Path.GetDirectoryName(outputFile);
            if (outputFileDir == "") outputFileDir = ".";
            outputFileWithoutExt = Path.GetFileNameWithoutExtension(outputFile);
            tempFile = Path.GetTempPath() + Guid.NewGuid().ToString() + ".docx";
            if (DebugOptions.docx)
            {
                tempFile = outputFileDir + "\\" + outputFileWithoutExt + ".tmp.docx";
                if (!Directory.Exists(outputFileDir))
                {
                    Directory.CreateDirectory(outputFileDir);
                }
            }
            log(String.Format("Szablon: {0}\nPlik tymczasowy: {1}\nWyjście: {2}\nGenerowanie...", templateFile, tempFile, outputFile));
            gen.generate(templateFile, data, tempFile);
            ShowDialog(owner);
        }

        delegate void OnFinishedDelegate(bool success, string errorMessage);

        void gen_onFinished(bool success, string errorMessage)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new OnFinishedDelegate(gen_onFinished), success, errorMessage);
            }
            else
            {
                if (success)
                {
                    postGenerate();
                }
                else
                {
                    error(errorMessage);
                }
                try { File.Delete(tempFile); }
                catch { };
            }
        }

        private void postGenerate()
        {
            var all = new List<string>();
            if (File.Exists(outputFile))
                all.Add(outputFile);
            if (!Directory.Exists(outputFileDir))
            {
                Directory.CreateDirectory(outputFileDir);
            }
            foreach (var file in Directory.EnumerateFiles(outputFileDir, outputFileWithoutExt + " (*).docx"))
                all.Add(file);

            if (all.Count > 0)
            {
                log("Istniejące warianty pliku:\n" + String.Join("\n", all));
                foreach (var file in all)
                {
                    if (!EnsureAccess(file)) continue;
                    if (compareDocx(file, tempFile))
                    {
                        var result = MyMessageBox.Show(
                            this,
                            String.Format("Wygenerowany dokument jest identyczny z wcześniej zapisanym " +
                            "pod nazwą \"{0}\".\r\n\r\n" +
                            "Czy chcesz otworzyć poprzedni dokument?", Path.GetFileName(file)),
                            "Plik już istnieje",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);
                        if (result == System.Windows.Forms.DialogResult.Yes)
                        {
                            readyFile(file);
                            return;
                        }
                        break;
                    }
                }
            }

            if (!File.Exists(outputFile))
            {
                log(String.Format("Przenoszenie pliku z {0} do {1}.", tempFile, outputFile));
                File.Move(tempFile, outputFile);
                readyFile(outputFile);
                return;
            }
            else
            {
                int i = 1;
                string alternateName = String.Format(@"{0}\{1} ({2}).docx", outputFileDir, outputFileWithoutExt, i);
                while (all.Contains(alternateName))
                {
                    i++;
                    alternateName = String.Format(@"{0}\{1} ({2}).docx", outputFileDir, outputFileWithoutExt, i);
                }
                log("Plik docelowy istnieje, alternatywna nazwa: " + alternateName);
                var win = new DocxOverrideWindow(outputFile, Path.GetFileName(alternateName), all.ToArray());
                var result = win.ShowDialog();
                win.Dispose();
                if (result == System.Windows.Forms.DialogResult.Cancel)
                {
                    log("Cancel");
                    Close();
                }
                else if (result == System.Windows.Forms.DialogResult.OK)
                {
                    log("Overriding...");
                    saveToFile(outputFile);
                }
                else
                {
                    log("Renaming to: " + alternateName);
                    saveToFile(alternateName);
                }
            }
        }

        private bool EnsureAccess(string file)
        {
            while (true)
            {
                try
                {
                    File.Open(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete).Dispose();
                    return true;
                }
                catch (Exception)
                {
                    var result = MyMessageBox.Show(
                        this,
                        String.Format("Nie można uzyskać dostępu do pliku \"{0}\". " +
                        "Może on być używany przez program Word. " +
                        "Zamknij wszystkie programy używające ten plik "+
                        "i spróbuj ponownie.", Path.GetFileName(file)),
                        "Brak dostępu do pliku",
                        MessageBoxButtons.RetryCancel,
                        MessageBoxIcon.Warning);
                    if (result == System.Windows.Forms.DialogResult.Retry)
                    {
                        continue;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        private bool compareDocx(string file1, string file2)
        {
            try
            {
                return DocxCompare.Compare(file1, file2);
            }
            catch (Exception ex)
            {
                log(String.Format("Porównanie \"{0}\" i \"{1}\" nie jest możliwe: {2}", file1, file2, ex.Message));
                return false;
            }
        }

        private void saveToFile(string file)
        {
            log("Writing to: " + file);
            bool copySuccess;
            do
            {
                try
                {
                    File.Copy(tempFile, file, true);
                    copySuccess = true;
                }
                catch (Exception ex)
                {
                    log(ex.ToString());
                    copySuccess = false;
                    var result = MyMessageBox.Show(
                        this,
                        String.Format("Nie można zapisać pliku \"{0}\".\r\nSprawdź, czy nie masz go " +
                            "otwartego w programie Word.", Path.GetFileName(file)),
                        "Błąd zapisu",
                        MessageBoxButtons.RetryCancel,
                        MessageBoxIcon.Error);
                    if (result == System.Windows.Forms.DialogResult.Cancel)
                    {
                        log("Cancel");
                        return;
                    }
                }
            } while (!copySuccess);
            readyFile(file);
        }

        private void readyFile(string file)
        {
            try { Close(); }
            catch (Exception) { }
            if (DebugOptions.nieOtwieraj) return;
            var proc = new Process();
            proc.StartInfo.FileName = file;
            proc.StartInfo.UseShellExecute = true;
            proc.Start();
            proc.Dispose();
        }
    }
}
