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

        public void generate(IWin32Window owner, string templateName, object data, string outputFile)
        {
            this.owner = owner;
            this.outputFile = outputFile;
            outputFileDir = Path.GetDirectoryName(outputFile);
            if (outputFileDir == "") outputFileDir = ".";
            outputFileWithoutExt = Path.GetFileNameWithoutExtension(outputFile);
            templateFile = String.Format(@"{0}\Szablony\{1}.xml", N.getProgramDir(), templateName);
            tempFile = Path.GetTempPath() + Guid.NewGuid().ToString() + ".docx";
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
                //try { File.Delete(tempFile); }
                //catch { };
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

            if (all.Count == 0)
            {
                log(String.Format("Moving file from {0} to {1}.", tempFile, outputFile));
                File.Move(tempFile, outputFile);
                readyFile(outputFile);
                return;
            }
            else
            {
                log("Istniejące warianty pliku:\n" + String.Join("\n", all));
            }

            /* TODO: compare temp file with main and all others
             * if the same as main: inform that nothing changed
             * if the same as some other: inform that:
             * Plik o nazwie "{0}" już istnije.
             * Istnieje też plik o nazwie "{1}", który jest identyczny z wygenerowanym aktualnie.
             * Co chcesz zrobić?
             * - Nadpisz plik "{0}"
             * - Pokaż plik "{1}"
             * Otherwise, do normal procedure.
             */
            bool theSame = false; 
            if (theSame)
            {
                Close();
                MessageBox.Show(owner, "Wygenerowany dokument jest identyczny z wcześniej zapisanym.\r\n\r\n" +
                    "Nie wprowadzono żadnych zmian.", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (File.Exists(outputFile)) {
                int i = 1;
                string alternateName = String.Format(@"{0}\{1} ({2}).docx", outputFileDir, outputFileWithoutExt, i);
                while (all.Contains(alternateName))
                {
                    i++;
                    alternateName = String.Format(@"{0}\{1} ({2}).docx", outputFileDir, outputFileWithoutExt, i);
                }
                log("Plik docelowy istnieje, alternatywna nazwa: " + alternateName);
                var win = new DocxOverrideWindow(outputFile, Path.GetFileName(alternateName));
                var result = win.ShowDialog();
                win.Dispose();
                if (result == System.Windows.Forms.DialogResult.Cancel)
                {
                    log("Cancel");
                    Close();
                }
                else
                {
                    if (result == System.Windows.Forms.DialogResult.OK)
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
                    var result2 = MessageBox.Show(
                        this,
                        String.Format("Nie można zapisać pliku \"{0}\".\r\nSprawdź, czy nie masz go " +
                            "otwartego w programie Word.", Path.GetFileName(file)),
                        "Błąd zapisu",
                        MessageBoxButtons.RetryCancel,
                        MessageBoxIcon.Error);
                    if (result2 == System.Windows.Forms.DialogResult.Cancel)
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
            Close();
            var proc = new Process();
            proc.StartInfo.FileName = file;
            proc.StartInfo.UseShellExecute = true;
            proc.Start();
            proc.Dispose();
        }
    }
}
/*
 * if exists:
 *     generate in temp
 *     compare
 *     if the same
 *          close window
 *          return
 *     ask what to do:
 *          override
 *          rename to xyz (10).docx
 *          cancel
 *              additionally: show old, show new
 *     while override error:
 *          ask to close Word and retry
 * else (not exists)
 *      generate directly
 * 
 * on generate error:
 *      show error message
 */