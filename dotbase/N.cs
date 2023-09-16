using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DotBase
{
    public static class N
    {

        public static bool proceduraOd20230915(DateTime date)
        {
            //return true;
            return date >= DateTime.Parse("2023-09-15");
        }

        public static double doubleParse(string text)
        {
            double result;
            text = text.Trim().Replace(" ", "");
            if (double.TryParse(text, out result)) return result;
            if (text.IndexOf('.') >= 0) return double.Parse(text.Replace('.', ','));
            return double.Parse(text.Replace(',', '.'));
        }

        public static bool doubleTryParse(string text, out double result)
        {
            text = text.Trim().Replace(" ", "");
            if (double.TryParse(text, out result)) return true;
            if (text.IndexOf('.') >= 0) text = text.Replace('.', ',');
            else if (text.IndexOf(',') >= 0) text = text.Replace(',', '.');
            return double.TryParse(text, out result);
        }

        public static string Wersja(bool nazwaPliku = false)
        {
            var na = nazwaPliku ? "unknown" : "N/A";
            string gitLog = Properties.Resources.GitVersion.Trim();
            if (gitLog.Length == 0) return na;
            string[] lines = gitLog.Split('\n');
            if (lines.Length < 1) return na;
            string gitStatus = "";
            for (int i = 1; i < lines.Length; i++)
            {
                gitStatus += lines[i].Trim();
            }
            string[] parts = lines[0].Trim().Split(' ');
            if (parts.Length < 2) return na;
            if (parts[0].Length < 7) return na;
            if (nazwaPliku)
            {
                if (gitStatus == "")
                {
                    return parts[1] + "-" + parts[0].Substring(0, 7).ToLower();
                }
                else
                {
                    return "patched-" + parts[1] + "-" + parts[0].Substring(0, 7).ToLower();
                }
            }
            else
            {
                if (gitStatus == "")
                {
                    return parts[1] + " [" + parts[0].Substring(0, 7).ToLower() + "]";
                }
                else
                {
                    return "!zmodyfikowana " + parts[1] + " [" + parts[0].Substring(0, 7).ToLower() + "]";
                }
            }
        }

        public delegate bool ZapiszDaneFunc();

        public static bool PotwierdzenieZapisz(Form form, ZapiszDaneFunc func, bool canContinue, bool force)
        {
            if (!force)
            {
                if (MessageBox.Show(form, "Czy zapisać dane?", "Potwierdzenie", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return true;
                }
            }

            if (!func())
            {
                if (canContinue)
                {
                    return MessageBox.Show(form, "Czy na pewno chcesz kontunuować?\r\nDane nie zostaną zapisane z uwagi na błędy wśród wpisanych dnaych.", "Uwaga", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes;
                }
                else
                {
                    MessageBox.Show(form, "Nie można kontunuować.\r\nDane nie zostaną zapisane z uwagi na błędy wśród wpisanych dnaych.", "Uwaga", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            return true;
        }
    }
}
