using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using DotBase.Logging;

namespace DotBase.Baza
{
    class AccessLocker
    {
        private Logger log = Log.create();

        public const int RETRY_DELAY_MS = 200;
        public const int SILENT_RETRY_MS = 5000;
        public const int LOG_RETRY_MS = 10000;
        string file;
        FileStream fileStream;
        int lockBits = 0;

        public AccessLocker(string file)
        {
            log("construct: {0}", file);
            this.file = file;
        }

        public void block(int bits)
        {
            if (lockBits != 0)
            {
                lockBits |= bits;
                return;
            }

            int counter = 0;

            do
            {
                blockInternal(counter, bits);
                if (lockBits != 0) return;
                counter++;
            } while (true);
        }

        public void unblock(int bits)
        {
            lockBits &= ~bits;
            if (lockBits == 0)
            {
                log("Unblock");
                if (fileStream != null)
                {
                    try { fileStream.Close(); }
                    catch (Exception) { }
                    try { fileStream.Dispose(); }
                    catch (Exception) { }
                    try { File.Delete(file); }
                    catch (Exception) { }
                    fileStream = null;
                }
            }
        }

        private void blockInternal(int counter, int bits)
        {
            try
            {
                log("Blocking {0}/{1}...", counter, SILENT_RETRY_MS / RETRY_DELAY_MS);
                fileStream = new FileStream(file, FileMode.Create, FileAccess.ReadWrite, FileShare.None);
                var content = String.Format("Blocked by: {0}\r\nOn machine:{1}\r\n",
                    Environment.UserName,
                    Environment.MachineName);
                var bytes = Encoding.UTF8.GetBytes(content);
                fileStream.Write(bytes, 0, bytes.Length);
                lockBits |= bits;
                log("Success");
            }
            catch (Exception ex)
            {
                log("File lock exception: " + ex.ToString());
                unblock(-1);
                Thread.Sleep(RETRY_DELAY_MS);
                if (counter > SILENT_RETRY_MS / RETRY_DELAY_MS)
                {
                    var res = (new AccessLockerForm(ex.Message + "\r\n\r\n" + ex.ToString())).ShowDialog();
                    log("Dialog result: {0}", res.ToString());
                    if (res == DialogResult.Ignore)
                    {
                        lockBits |= bits;
                    }
                    else if (res == DialogResult.Abort)
                    {
                        throw new ApplicationException("Aborted");
                    }
                    else if (res == DialogResult.Retry)
                    {
                        // nothing to do, retry
                    }
                }
            }
        }
    }
}
