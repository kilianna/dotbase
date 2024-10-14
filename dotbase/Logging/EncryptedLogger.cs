using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Diagnostics;

namespace DotBase.Logging
{
    static class EncryptedLogger
    {
        const int UNCOMPRESSED_SIZE = 4 * 1024 * 1024;
        const int COMPRESSED_SIZE = 1024 * 1024;
        class ControlMessage
        {
            public string directory;
            public string password;
            public ControlMessage(string d, string p)
            {
                directory = d;
                password = p;
            }
        }
        static object lk = new object();
        static Queue<string> logMessages = new Queue<string>(64);
        static Queue<ControlMessage> controlMessages = new Queue<ControlMessage>(3);
        static Thread thread = null;
        static string failure = null;
        static RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
        static byte[] signatureBytes = Encoding.UTF8.GetBytes("LogEncoded.AES128.CBC.PBKDF2\x00\x01\x02\x03");
        static byte[] verificationBytes = Encoding.UTF8.GetBytes("LogEncodedVerify");
        static byte[] footerBytes = Encoding.UTF8.GetBytes("LogEncodedEnding");

        public static void SetLocation(string directory, string password)
        {
            lock (lk)
            {
                checkFailure();
                if (thread == null)
                {
                    thread = new Thread(run);
                    thread.Start();
                }
                controlMessages.Enqueue(new ControlMessage(directory, password));
                Monitor.PulseAll(lk);
            }
        }


        public static void Stop()
        {
            Thread tmp;
            lock (lk)
            {
                checkFailure();
                tmp = thread;
                logMessages.Enqueue(null);
                Monitor.PulseAll(lk);
            }
            if (tmp != null)
            {
                tmp.Join();
            }
        }

        public static void Push(string message)
        {
            if (message == null) return;
            lock (lk)
            {
                checkFailure();
                logMessages.Enqueue(message);
                Monitor.PulseAll(lk);
            }
        }

        private static void checkFailure()
        {
            if (failure != null && SynchronizationContext.Current != null)
            {
                Monitor.Exit(lk);
                MessageBox.Show("Błąd zapisywania logów diagnostycznych: " + failure, "Błąd");
                Monitor.Enter(lk);
                failure = null;
            }
        }

        enum ReturnType
        {
            Repeat,
            Control,
            Exit,
        };

        static void run()
        {
            string directory = null;
            string password = null;
            int retryCount = 0;

            while (true)
            {
                try
                {
                    lock (lk)
                    {
                        // Wait for initial control message, skip waiting if directory is already known
                        while (directory == null && controlMessages.Count == 0)
                        {
                            Monitor.Wait(lk);
                        }
                        // Get control message if available
                        while (controlMessages.Count > 0)
                        {
                            var msg = controlMessages.Dequeue();
                            directory = msg.directory;
                            password = msg.password;
                        }
                    }
                    // Go to output writing
                    do
                    {
                        var ret = writeOutput(directory, password, ref retryCount);
                        if (ret == ReturnType.Exit) return;
                        if (ret == ReturnType.Control) break;
                        // ReturnType.Repeat otherwise
                    } while (true);
                }
                catch (Exception ex)
                {
                    retryCount++;
                    Debug.WriteLine("Logger thread exception {0} of 10: {1}", retryCount, ex.ToString());
                    if (retryCount > 10)
                    {
                        // Too many retries, only new control message may help
                        retryCount = 0;
                        lock (lk)
                        {
                            failure = ex.ToString();
                            while (controlMessages.Count == 0)
                            {
                                while (logMessages.Count > 0)
                                {
                                    var msg = logMessages.Dequeue();
                                    if (msg == null) return;
                                }
                                Monitor.Wait(lk);
                            }
                        }
                    }
                    lock (lk)
                    {
                        logMessages.Enqueue(String.Format(
                            "----------------------------------------\r\n" +
                            " Exception in logger thread: {0}\r\n" +
                            "----------------------------------------\r\n",
                            ex.ToString()));
                    }
                }
            }
        }

        private static ReturnType writeOutput(string directory, string password, ref int retryCount)
        {
            var now = DateTime.Now;
            var month = String.Format("{0:yyyy-MM}", now);
            var salt = new byte[16];
            rng.GetBytes(salt);
            var iv = new byte[16];
            rng.GetBytes(iv);
            var fileName = String.Format("{0:yyyy-MM-dd HH_mm_ss.fff}.{1}.enclog", now, (int)salt[0] + 256 * (int)iv[0]);
            var subdir = Path.Combine(directory, month);
            var filePath = Path.Combine(subdir, fileName);
            var readBytes = 0;
            Directory.CreateDirectory(subdir);
            byte[] key;
            using (var kdf = new Rfc2898DeriveBytes(password, salt, 10000))
                key = kdf.GetBytes(16);
            var aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
#           if DEBUG
            using (var outputDebug = new FileStream(filePath + ".txt", FileMode.CreateNew, FileAccess.Write))
#           endif
            using (var output = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write))
            {
                output.Write(signatureBytes, 0, signatureBytes.Length);
                output.Write(salt, 0, salt.Length);
                output.Write(iv, 0, iv.Length);
                using (var sha = SHA256.Create())
                using (var cs = new CryptoStream(output, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(verificationBytes, 0, verificationBytes.Length);
                    try
                    {
                        using (var ds = new DeflateStream(cs, CompressionMode.Compress, true))
                        {
                            while (true)
                            {
                                string inputLog;
                                lock (lk)
                                {
                                    while (logMessages.Count == 0 && controlMessages.Count == 0)
                                    {
                                        Monitor.Wait(lk);
                                    }
                                    if (controlMessages.Count > 0) return ReturnType.Control;
                                    inputLog = logMessages.Dequeue();
                                    if (inputLog == null) return ReturnType.Exit;
                                }
                                var data = Encoding.UTF8.GetBytes(inputLog);
                                sha.TransformBlock(data, 0, data.Length, null, 0);
#                               if DEBUG
                                outputDebug.Write(data, 0, data.Length);
#                               endif
                                ds.Write(data, 0, data.Length);
                                readBytes += data.Length;
                                var writtenBytes = output.Position;
                                if (writtenBytes > 0)
                                {
                                    retryCount = 0;
                                }
                                if (writtenBytes > COMPRESSED_SIZE || readBytes > UNCOMPRESSED_SIZE)
                                {
                                    return ReturnType.Repeat;
                                }
                            }
                        }
                    }
                    finally
                    {
                        try
                        {
                            sha.TransformFinalBlock(new byte[0], 0, 0);
                            cs.Write(sha.Hash, 0, sha.Hash.Length);
                            cs.Write(footerBytes, 0, footerBytes.Length);
                        }
                        catch { }
                    }
                }
            }
        }

        public static void decodeDirectory(string inputFile, string outputFile, string password)
        {
            // TODO: decode directory
        }

        public static void decodeFile(string inputFile, string outputFile, string password)
        {
            byte[] decryptedBytes = new byte[0];
            int decryptedBytesLength = 0;
            Exception pendingException = null;
            string messages = "";

            // Decrypt to memory
            try
            {
                using (var input = new FileStream(inputFile, FileMode.Open, FileAccess.Read))
                {
                    byte[] signature = new byte[32];
                    byte[] salt = new byte[16];
                    byte[] iv = new byte[16];
                    input.Read(signature, 0, 32);
                    if (!signature.SequenceEqual(signatureBytes))
                        throw new InvalidFileException("Invalid input file signature");
                    input.Read(salt, 0, 16);
                    input.Read(iv, 0, 16);
                    byte[] key;
                    using (var kdf = new Rfc2898DeriveBytes(password, salt, 10000))
                        key = kdf.GetBytes(16);
                    var aes = Aes.Create();
                    aes.Key = key;
                    aes.IV = iv;
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;
                    decryptedBytes = new byte[input.Length + 128];
                    using (var cs = new CryptoStream(input, aes.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        decryptedBytesLength = 0;
                        do
                        {
                            var n = cs.Read(decryptedBytes, decryptedBytesLength, 16);
                            if (n == 0) break;
                            decryptedBytesLength += n;
                        } while (true);
                    }
                }
            }
            catch (Exception ex)
            {
                // In case of exception, postpone throwing
                pendingException = ex;
                messages += String.Format("\r\n\r\nLog file decryption error: {0}\r\n", ex.ToString());
            }

            // Invalid verification bytes - wrong password
            if (decryptedBytesLength >= 16 && !compareBytes(decryptedBytes, 0, verificationBytes))
            {
                throw new InvalidPasswordException("Invalid password");
            }

            // Get information from footer, assume truncantenated file if something is wrong
            int endCut = 0;
            if (decryptedBytesLength > 64)
            {
                byte[] actualFooter = new byte[16];
                if (compareBytes(decryptedBytes, decryptedBytesLength - 16, footerBytes))
                {
                    endCut = 48;
                }
            }

            if (endCut == 0)
            {
                if (pendingException == null) pendingException = new IOException("Truncatenated file");
                messages += "\r\n\r\nInvalid log file footer - file truncatenated.\r\n";
            }

            int chunkSize = endCut > 0 ? 65536 : 16;
            byte[] buffer = new byte[chunkSize];
            using (var sha = SHA256.Create())
            using (var output = new FileStream(outputFile, FileMode.Create, FileAccess.Write))
            {
                try
                {
                    using (var bs = new MemoryStream(decryptedBytes, 16, decryptedBytesLength - endCut - 16))
                    using (var ds = new DeflateStream(bs, CompressionMode.Decompress, true))
                    {
                        do
                        {
                            int n = ds.Read(buffer, 0, buffer.Length);
                            if (n == 0) break;
                            output.Write(buffer, 0, n);
                            sha.TransformBlock(buffer, 0, n, null, 0);
                        } while (true);
                    }
                }
                catch (Exception ex)
                {
                    if (pendingException == null) pendingException = ex;
                    messages += String.Format("\r\n\r\nLog file decompression error: {0}\r\n", ex.ToString());
                }
                finally
                {
                    sha.TransformFinalBlock(new byte[0], 0, 0);
                }

                if (endCut > 0 && !compareBytes(decryptedBytes, decryptedBytesLength - 48, sha.Hash))
                {
                    if (pendingException == null) pendingException = new IOException("Wrong hash");
                    messages += "\r\n\r\nLog file hash verification failed.\r\n";
                }

                if (messages.Length > 0)
                {
                    var tmp = Encoding.UTF8.GetBytes(messages);
                    output.Write(tmp, 0, tmp.Length);
                }
            }

            if (pendingException != null)
            {
                throw pendingException;
            }
        }

        private static bool compareBytes(byte[] buffer, int offset, byte[] expected)
        {
            if (offset + expected.Length > buffer.Length || offset < 0) return false;
            var tmp = new byte[expected.Length];
            Array.Copy(buffer, offset, tmp, 0, tmp.Length);
            return expected.SequenceEqual(tmp);
        }
    }

    public class InvalidPasswordException : CryptographicException
    {
        public InvalidPasswordException(string p) : base(p) { }
    }

    public class InvalidFileException : IOException
    {
        public InvalidFileException(string p) : base(p) { }
    }
}
