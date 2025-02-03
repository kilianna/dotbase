using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace DotBase.Test
{
    class TestSwiadHtmlVsDocx : TestBase
    {
        Szablon.Row_Karta_przyjecia[] karty;
        Szablon.Row_Karta_przyjecia karta;
        HashSet<int> doneIds = new HashSet<int>();
        int index = 0;
        bool force;
        bool zalTabPun;

        public void run(string fromIdText, string toIdText, bool force, bool zalTabPun)
        {
            int fromId = 0;
            int toId = 999999999;
            if (fromIdText.Trim().Length > 0)
                fromId = Int32.Parse(fromIdText.Trim());
            if (toIdText.Trim().Length > 0)
                toId = Int32.Parse(toIdText.Trim());
            DebugOptions.stopTest = false;
            karty = baza.Karta_przyjecia
                .SELECT().ID_karty()
                .WHERE()
                    .ID_karty(fromId, "***>=?")
                    .ID_karty(toId, "***<=?")
                .ORDER_BY().ID_karty(Order.DESC)
                .GET();
            index = 0;
            DebugOptions.docx = true;
            DebugOptions.dontSave = true;
            DebugOptions.nieOtwieraj = true;
            DebugOptions.messageBox = true;
            var menuForm = getForm<MenuGlowneForm>();
            press(field<Button>(menuForm, "button2"));
            this.force = force;
            this.zalTabPun = zalTabPun;
            string[] lines = new string[0];
            try
            {
                lines = File.ReadAllLines(N.getProgramDir() + @"\..\wyniki\Swiadectwo\done.txt");
            }
            catch { }
            foreach (var line in lines)
            {
                int x;
                if (Int32.TryParse(line, out x))
                {
                    doneIds.Add(x);
                }
            }
            wait(run2);
        }

        private void run2()
        {
            var wzorcowanieForm = getForm<MenuWzorcowanieForm>();
            while (true)
            {
                if (index >= karty.Length)
                {
                    Debug.WriteLine("----- DONE -----");
                    return;
                }
                karta = karty[index];
                if (!force && doneIds.Contains(karta.ID_karty ?? -1))
                {
                    Debug.WriteLine(String.Format("Karta {0}/{1}, id {2} - ALREADY DONE", karty.Length - index, karty.Length, karty[index].ID_karty));
                    index++;
                    continue;
                }
                else
                {
                    Debug.WriteLine(String.Format("Karta {0}/{1}, id {2} - PROCESSING", karty.Length - index, karty.Length, karty[index].ID_karty));
                    break;
                }
            }
            var id = field<NumericUpDown>(wzorcowanieForm, "numericUpDown1");
            id.Select(0, id.Text.Length);
            send(id, karta.ID_karty + "{TAB}");
            wait(() =>
            {
                press(field<Button>(wzorcowanieForm, "button13"));
                wait(() =>
                {
                    var swiadectwoForm = getForm<MenuPismaSwiadectwaForm>();
                    if (zalTabPun)
                    {
                        var tabPunBox = field<CheckBox>(swiadectwoForm, "dolaczTabPunktyBox");
                        tabPunBox.Checked = true;
                    }
                    press(field<Button>(swiadectwoForm, "button1"));
                    wait(800, () =>
                    {
                        var overrideForm = getForm<Szablony.DocxOverrideWindow>(true);
                        if (overrideForm != null)
                        {
                            press(field<Button>(overrideForm, "nadpisz"));
                        }
                        wait(() =>
                        {
                            var docxWindow = getForm<Szablony.DocxWindow>(true);
                            wait(docxWindow == null ? 0 : 3000, () =>
                            {
                                docxWindow = getForm<Szablony.DocxWindow>(true);
                                if (docxWindow != null)
                                {
                                    throw new ApplicationException("Converting took too long or error happend.");
                                }
                                var messageBox = getForm<MyMessageBox>(true);
                                if (messageBox != null)
                                {
                                    var messageText = field<TextBox>(messageBox, "messageBox").Text;
                                    if (messageBox.Text.StartsWith("Uwaga"))
                                    {
                                        File.WriteAllText(N.getProgramDir() + @"\..\wyniki\Swiadectwo\old-" + karta.ID_karty + ".txt", messageText);
                                    }
                                    else if (messageBox.Text.StartsWith("Błąd"))
                                    {
                                        File.WriteAllText(N.getProgramDir() + @"\..\wyniki\Swiadectwo\new-" + karta.ID_karty + ".txt", messageText);
                                    }
                                    else
                                    {
                                        throw new ApplicationException("Unexpected message box.");
                                    }
                                    press(messageBox.AcceptButton as Control);
                                }
                                wait(() =>
                                {
                                    messageBox = getForm<MyMessageBox>(true);
                                    if (messageBox != null)
                                    {
                                        var messageText = field<TextBox>(messageBox, "messageBox").Text;
                                        if (messageBox.Text.StartsWith("Uwaga"))
                                        {
                                            File.WriteAllText(N.getProgramDir() + @"\..\wyniki\Swiadectwo\old-" + karta.ID_karty + ".txt", messageText);
                                        }
                                        else
                                        {
                                            throw new ApplicationException("Unexpected message box.");
                                        }
                                        press(messageBox.AcceptButton as Control);
                                    }
                                    wait(() =>
                                    {
                                        swiadectwoForm.Close();
                                        doneIds.Add(karta.ID_karty ?? -1);
                                        File.WriteAllText(N.getProgramDir() + @"\..\wyniki\Swiadectwo\done.txt", string.Join("\n", doneIds.ToArray()));
                                        index++;
                                        if (!DebugOptions.stopTest)
                                            wait(run2);
                                    });
                                });
                            });
                        });
                    });
                });
            });
        }

    }
}
