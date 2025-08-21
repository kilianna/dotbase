using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing;

namespace DotBase.Logging
{
    class ScreenShots
    {
        static Logger log = Log.create();

        [DllImport("user32.dll")]
        static extern bool PrintWindow(IntPtr hWnd, IntPtr hdcBlt, int nFlags);

        private static Bitmap CaptureWindow(Form form)
        {
            Bitmap bmp = new Bitmap(form.Width, form.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                IntPtr hdc = g.GetHdc();
                try
                {
                    PrintWindow(form.Handle, hdc, 0);
                }
                finally
                {
                    g.ReleaseHdc(hdc);
                }
            }
            return bmp;
        }

        public static void SaveScreeShots()
        {
            foreach (Form form in Application.OpenForms)
            {
                try
                {
                    if (form.GetType().GetMember("_noScreeshot").Length == 0)
                    {
                        var screenshot = CaptureWindow(form);
                        var memStream = new System.IO.MemoryStream();
                        screenshot.Save(memStream, System.Drawing.Imaging.ImageFormat.Png);
                        var mem = memStream.GetBuffer();
                        EncryptedLogger.PushFile(mem, form.GetType().Name + ".png");
                    }
                }
                catch (Exception ex)
                {
                    log("Error writing screen shot of {0}:\r\n", form.GetType().Name, ex.ToString());
                }
            }
        }

    }
}
