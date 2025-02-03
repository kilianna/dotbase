using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DotBase
{
    public partial class MyMessageBox : Form
    {
#if DEBUG
        private const bool debug = true;
#else
        private const bool debug = false;
#endif
        public MyMessageBox()
        {
            InitializeComponent();
        }

        public static DialogResult Show(string text) { return Show(text, "Message", MessageBoxButtons.OK, MessageBoxIcon.None); }
        public static DialogResult Show(IWin32Window owner, string text) { return Show(owner, text, "Message", MessageBoxButtons.OK, MessageBoxIcon.None); }
        public static DialogResult Show(string text, string caption) { return Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.None); }
        public static DialogResult Show(IWin32Window owner, string text, string caption) { return Show(owner, text, caption, MessageBoxButtons.OK, MessageBoxIcon.None); }
        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons) { return Show(text, caption, buttons, MessageBoxIcon.None); }
        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons) { return Show(owner, text, caption, buttons, MessageBoxIcon.None); }
        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return Show((IWin32Window)null, text, caption, buttons, icon);
        }
        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return Show(owner, text, caption, buttons, icon, new MessageBoxDefaultButton[0]);
        }
        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defButton)
        {
            return Show(owner, text, caption, buttons, icon, new MessageBoxDefaultButton[] { defButton });
        }
        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton[] defButton)
        {
            if (!DebugOptions.messageBox || !debug)
            {
                if (defButton.Length > 0)
                    return MessageBox.Show(owner, text, caption, buttons, icon, defButton[0]);
                else
                    return MessageBox.Show(owner, text, caption, buttons, icon);
            }
            var mb = new MyMessageBox();
            mb.Text = caption;
            mb.messageBox.Text = text;
            switch (icon)
            {
                case MessageBoxIcon.Asterisk:
                    mb.infoBox.Visible = true;
                    break;
                case MessageBoxIcon.Error:
                    mb.errorBox.Visible = true;
                    break;
                case MessageBoxIcon.Exclamation:
                    mb.warningBox.Visible = true;
                    break;
                case MessageBoxIcon.Question:
                    mb.questionBox.Visible = true;
                    break;
                case MessageBoxIcon.None:
                    break;
                default:
                    throw new ApplicationException("Not implemented");
            }
            switch (buttons)
            {
                case MessageBoxButtons.OK:
                    mb.okButton.Visible = true;
                    mb.AcceptButton = mb.okButton;
                    break;
                case MessageBoxButtons.OKCancel:
                    mb.okButton.Visible = true;
                    mb.cancelButton.Visible = true;
                    mb.AcceptButton = mb.okButton;
                    mb.CancelButton = mb.cancelButton;
                    if (defButton.Length > 0 && defButton[0] == MessageBoxDefaultButton.Button1) mb.okButton.Focus();
                    if (defButton.Length > 0 && defButton[0] == MessageBoxDefaultButton.Button2) mb.cancelButton.Focus();
                    break;
                case MessageBoxButtons.RetryCancel:
                    mb.retryButton.Visible = true;
                    mb.cancelButton.Visible = true;
                    mb.AcceptButton = mb.retryButton;
                    mb.CancelButton = mb.cancelButton;
                    if (defButton.Length > 0 && defButton[0] == MessageBoxDefaultButton.Button1) mb.retryButton.Focus();
                    if (defButton.Length > 0 && defButton[0] == MessageBoxDefaultButton.Button2) mb.cancelButton.Focus();
                    break;
                case MessageBoxButtons.AbortRetryIgnore:
                    mb.abortButton.Visible = true;
                    mb.retryButton.Visible = true;
                    mb.ignoreButton.Visible = true;
                    mb.AcceptButton = mb.retryButton;
                    mb.CancelButton = mb.abortButton;
                    if (defButton.Length > 0 && defButton[0] == MessageBoxDefaultButton.Button1) mb.abortButton.Focus();
                    if (defButton.Length > 0 && defButton[0] == MessageBoxDefaultButton.Button2) mb.retryButton.Focus();
                    if (defButton.Length > 0 && defButton[0] == MessageBoxDefaultButton.Button3) mb.ignoreButton.Focus();
                    break;
                case MessageBoxButtons.YesNo:
                    mb.yesButton.Visible = true;
                    mb.noButton.Visible = true;
                    mb.AcceptButton = mb.yesButton;
                    mb.CancelButton = mb.noButton;
                    if (defButton.Length > 0 && defButton[0] == MessageBoxDefaultButton.Button1) mb.yesButton.Focus();
                    if (defButton.Length > 0 && defButton[0] == MessageBoxDefaultButton.Button2) mb.noButton.Focus();
                    break;
                case MessageBoxButtons.YesNoCancel:
                    mb.yesButton.Visible = true;
                    mb.noButton.Visible = true;
                    mb.cancelButton.Visible = true;
                    mb.AcceptButton = mb.yesButton;
                    mb.CancelButton = mb.cancelButton;
                    if (defButton.Length > 0 && defButton[0] == MessageBoxDefaultButton.Button1) mb.yesButton.Focus();
                    if (defButton.Length > 0 && defButton[0] == MessageBoxDefaultButton.Button2) mb.cancelButton.Focus();
                    break;
                default:
                    throw new ApplicationException("Not implemented");
            }
            return mb.ShowDialog(owner);
        }
    }
}
