using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace DotBase.Controls
{
    public partial class SimpleHtmlTextBox : UserControl
    {
        public SimpleHtmlTextBox()
        {
            InitializeComponent();
        }

        private void contextMenu_Opening(object sender, CancelEventArgs e)
        {
            var style = rt.SelectionFont.Style;
            pogrubienieToolStripMenuItem.Checked = (style & FontStyle.Bold) != 0;
            kurysywaToolStripMenuItem.Checked = (style & FontStyle.Italic) != 0;
            podkreślenieToolStripMenuItem.Checked = (style & FontStyle.Underline) != 0;
            indeksgórnyToolStripMenuItem.Checked = rt.SelectionCharOffset > 0;
            indeksdolnyToolStripMenuItem.Checked = rt.SelectionCharOffset < 0;
            cofnijToolStripMenuItem.Enabled = rt.CanUndo;
            powtórzToolStripMenuItem.Enabled = rt.CanRedo;
        }

        private void pogrubienieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toggleTextStyle(FontStyle.Bold);
        }

        private void toggleTextStyle(FontStyle fontStyle)
        {
            rt.SelectionFont = new Font(rt.SelectionFont, rt.SelectionFont.Style ^ fontStyle);
            rt.Focus();
        }

        private void kurysywaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toggleTextStyle(FontStyle.Italic);
        }

        private void podkreślenieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toggleTextStyle(FontStyle.Underline);
        }

        private void indeksgórnyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rt.SelectionCharOffset < 5)
            {
                rt.SelectionCharOffset = 5;
                var f = rt.SelectionFont;
                f = new System.Drawing.Font(f.FontFamily, rt.Font.Size * 0.8f, f.Style, f.Unit, f.GdiCharSet, f.GdiVerticalFont);
                rt.SelectionFont = f;
            }
            else
            {
                rt.SelectionCharOffset = 0;
                var f = rt.SelectionFont;
                f = new System.Drawing.Font(f.FontFamily, rt.Font.Size, f.Style, f.Unit, f.GdiCharSet, f.GdiVerticalFont);
                rt.SelectionFont = f;
            }
        }

        private void indeksdolnyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rt.SelectionCharOffset > -3)
            {
                rt.SelectionCharOffset = -3;
                var f = rt.SelectionFont;
                f = new System.Drawing.Font(f.FontFamily, rt.Font.Size * 0.8f, f.Style, f.Unit, f.GdiCharSet, f.GdiVerticalFont);
                rt.SelectionFont = f;
            }
            else
            {
                rt.SelectionCharOffset = 0;
                var f = rt.SelectionFont;
                f = new System.Drawing.Font(f.FontFamily, rt.Font.Size, f.Style, f.Unit, f.GdiCharSet, f.GdiVerticalFont);
                rt.SelectionFont = f;
            }
        }

        private void µMikroToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rt.SelectedText = (sender as ToolStripItem).Tag.ToString();
        }

        private void wytnijToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rt.Cut();
        }

        private void kopiujToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rt.Copy();
        }

        private void wklejToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rt.Paste();
            validateContent();
        }

        private void cofnijToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rt.Undo();
        }

        private void powtórzToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rt.Redo();
        }

        private void rt_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers == Keys.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.B:
                        pogrubienieToolStripMenuItem_Click(sender, e);
                        break;
                    case Keys.I:
                        kurysywaToolStripMenuItem_Click(sender, e);
                        break;
                    case Keys.M:
                        rt.SelectedText = "µ";
                        break;
                    case Keys.Space:
                        rt.SelectedText = "‿";
                        break;
                    case Keys.C:
                        kopiujToolStripMenuItem_Click(sender, e);
                        break;
                    case Keys.V:
                        wklejToolStripMenuItem_Click(sender, e);
                        break;
                    case Keys.X:
                        wytnijToolStripMenuItem_Click(sender, e);
                        break;
                    case Keys.Z:
                        cofnijToolStripMenuItem_Click(sender, e);
                        break;
                    case Keys.Y:
                        powtórzToolStripMenuItem_Click(sender, e);
                        break;
                    case Keys.A:
                        zaznaczWszystkoToolStripMenuItem_Click(sender, e);
                        break;
                }
            }
        }

        private void rt_KeyDown(object sender, KeyEventArgs e)
        {
            var handled = false;
            if (e.Modifiers == Keys.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.B:
                    case Keys.I:
                    case Keys.M:
                    case Keys.Space:
                    case Keys.C:
                    case Keys.V:
                    case Keys.X:
                    case Keys.Z:
                    case Keys.Y:
                    case Keys.A:
                        handled = true;
                        break;
                }
            }
            e.SuppressKeyPress = handled;
            e.Handled = handled;
        }

        private string cachedRtf = "";
        private string cachedHtml = "";

        public override string Text
        {
            get {
                if (cachedRtf != rt.Rtf) {
                    cachedHtml = toSimpleHTML(rt, false);
                    cachedRtf = rt.Rtf;
                }
                return cachedHtml;
            }
            set {
                rt.Rtf = toSimpleRTF(value);
            }
        }

        private string cachedRtfWithSelection = "";
        private string cachedHtmlWithSelection = "";

        private void validateContent()
        {
            if (cachedRtfWithSelection != rt.Rtf)
            {
                cachedHtmlWithSelection = toSimpleHTML(rt, true);
                cachedRtfWithSelection = rt.Rtf;
            }
            var rtf = toSimpleRTF(cachedHtmlWithSelection);
            rt.Rtf = rtf;
            var start = rt.Find("_SELECTION_START_", RichTextBoxFinds.MatchCase | RichTextBoxFinds.NoHighlight);
            if (start >= 0)
            {
                rt.SelectionLength = 0;
                rt.SelectionStart = start;
                rt.SelectionLength = 17;
                rt.SelectedText = "";
                var end = rt.Find("_SELECTION_END_", RichTextBoxFinds.MatchCase | RichTextBoxFinds.NoHighlight);
                rt.SelectionLength = 0;
                rt.SelectionStart = end;
                rt.SelectionLength = 15;
                rt.SelectedText = "";
                rt.SelectionLength = 0;
                rt.SelectionStart = start;
                rt.SelectionLength = end - start;
            }
            else
            {
                rt.SelectAll();
                var len = rt.SelectionLength;
                rt.SelectionLength = 0;
                rt.SelectionStart = len;
            }
        }

        private void rt_Validating(object sender, CancelEventArgs e)
        {
            validateContent();
        }

        #region Converters

        private static string toSimpleRTF(string html)
        {
            var res = new StringBuilder();
            res.Append(@"{\rtf1\ansi\ansicpg1250\deff0\deflang1045{\fonttbl{\f0\fnil\fcharset238 Microsoft Sans Serif;}}");
            res.Append(@"\viewkind4\uc1\pard\f0\fs17 ");
            var rtf = html
                .Replace("\\", "\\\\")
                .Replace("{", "\\{")
                .Replace("}", "\\}")
                .Replace("\r\n", "\n")
                .Replace("<br>", "\n")
                .Replace("<br/>", "\n")
                .Replace("\n", @"\par ")
                .Replace("<b>", @"\b ")
                .Replace("<B>", @"\b ")
                .Replace("</b>", @"\b0 ")
                .Replace("</B>", @"\b0 ")
                .Replace("<i>", @"\i ")
                .Replace("<I>", @"\i ")
                .Replace("</i>", @"\i0 ")
                .Replace("</I>", @"\i0 ")
                .Replace("<u>", @"\ul ")
                .Replace("<U>", @"\ul ")
                .Replace("</u>", @"\ulnone ")
                .Replace("</U>", @"\ulnone ")
                .Replace("<sup>", @"\up7\fs13 ")
                .Replace("<SUP>", @"\up7\fs13 ")
                .Replace("</sup>", @"\up0\fs17 ")
                .Replace("</SUP>", @"\up0\fs17 ")
                .Replace("<sub>", @"\dn4\fs13 ")
                .Replace("<SUB>", @"\dn4\fs13 ")
                .Replace("</sub>", @"\up0\fs17 ")
                .Replace("</SUB>", @"\up0\fs17 ")
                .Replace("\xA0", "‿")
                .Replace("&nbsp;", "‿")
                .Replace("&amp;", "&")
                .Replace("&lt;", "<")
                .Replace("&gt;", ">")
                .Replace("&quot;", "\"")
                .Replace("&mu;", "µ")
                .Replace("<!-- SELECTION START -->", "_SELECTION_START_")
                .Replace("<!-- SELECTION END -->", "_SELECTION_END_");
            foreach (var c in rtf)
            {
                if (c > '\x7F')
                {
                    res.Append("\\u");
                    res.Append((short)c);
                    res.Append('?');
                }
                else
                {
                    res.Append(c);
                }
            }
            res.Append("}");
            return res.ToString();
        }

        private static string styleToTag(int style)
        {
            switch (style)
            {
                case 1: return "b";
                case 2: return "u";
                case 4: return "i";
                case 8: return "sup";
                case 16: return "sub";
                default: throw new ApplicationException("Invalid style index");
            }
        }

        private static string toSimpleHTML(RichTextBox rt, bool exportSelection)
        {
            bool needToEndSelection = false;
            var savedStart = rt.SelectionStart;
            var savedLen = rt.SelectionLength;
            var copy = new RichTextBox();
            copy.Rtf = rt.Rtf;
            rt = copy;
            rt.SelectAll();
            var len = rt.SelectionLength;
            /* 1 - bold
             * 2 - underline
             * 4 - italics
             * 8 - super
             * 16 - sub
             */
            int oldStyle = 0;
            var res = new StringBuilder();
            var stack = new Stack<int>();
            for (var i = 0; i < len; i++)
            {
                rt.SelectionLength = 1;
                rt.SelectionStart = i;
                rt.SelectionLength = 1;
                if (rt.SelectionFont == null || rt.SelectionLength != 1 || rt.SelectionStart != i)
                {
                    continue;
                }
                int style = 0;
                if ((rt.SelectionFont.Style & FontStyle.Bold) != 0) style |= 1;
                if ((rt.SelectionFont.Style & FontStyle.Underline) != 0) style |= 2;
                if ((rt.SelectionFont.Style & FontStyle.Italic) != 0) style |= 4;
                if (rt.SelectionCharOffset > 0) style |= 8;
                if (rt.SelectionCharOffset < 0) style |= 16;
                int offStyle = oldStyle & ~style;
                while (offStyle != 0)
                {
                    int off = stack.Pop();
                    offStyle &= ~off;
                    oldStyle &= ~off;
                    res.Append("</");
                    res.Append(styleToTag(off));
                    res.Append(">");
                }
                int onStyle = ~oldStyle & style;
                while (onStyle != 0)
                {
                    int on;
                    for (on = 1; (on & onStyle) == 0; on <<= 1) { }
                    stack.Push(on);
                    onStyle &= ~on;
                    oldStyle |= on;
                    res.Append("<");
                    res.Append(styleToTag(on));
                    res.Append(">");
                }

                if (exportSelection && i == savedStart)
                {
                    res.Append("<!-- SELECTION START -->");
                    needToEndSelection = true;
                }

                if (exportSelection && i == savedStart + savedLen)
                {
                    res.Append("<!-- SELECTION END -->");
                    needToEndSelection = false;
                }

                switch (rt.SelectedText.Length == 1 ? rt.SelectedText[0] : '\0')
                {
                    case '\n':
                        res.Append("<br/>");
                        break;
                    case '‿':
                    case '\xA0':
                        res.Append("&nbsp;");
                        break;
                    case '&':
                        res.Append("&amp;");
                        break;
                    case '<':
                        res.Append("&lt;");
                        break;
                    default:
                        res.Append(rt.SelectedText);
                        break;
                }
            }

            while (stack.Count > 0)
            {
                int off = stack.Pop();
                res.Append("</");
                res.Append(styleToTag(off));
                res.Append(">");
            }

            if (exportSelection && needToEndSelection)
            {
                res.Append("<!-- SELECTION END -->");
            }

            rt.SelectionLength = 0;
            rt.SelectionStart = savedStart;
            rt.SelectionLength = savedLen;
            return res.ToString();
        }

        #endregion

        private void zaznaczWszystkoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rt.SelectAll();
        }

        public event EventHandler HtmlChanged;

        private void rt_TextChanged(object sender, EventArgs e)
        {
            if (HtmlChanged != null)
            {
                HtmlChanged.Invoke(this, e);
            }
        }

        private void innyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this, "Skopiuj i wklej z tablicy znaków.", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
            try
            {
                System.Diagnostics.Process.Start("charmap.exe");
            }
            catch (Exception) { }
        }


        public void Clear()
        {
            rt.Clear();
        }

        new public bool Enabled
        {
            get { return rt.Enabled; }
            set { rt.Enabled = value; }
        }
    }
}
