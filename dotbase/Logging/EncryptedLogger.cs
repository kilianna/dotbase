﻿using System;
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
        const int DIAGNOSTICS_OUTPUT_DAYS = 7;
        const int UNCOMPRESSED_SIZE = 4 * 1024 * 1024;
        const int COMPRESSED_SIZE = 1024 * 1024;
        const string MAGIC_LOG_FLUSH_FILE = "\r\n<<< Flushing log file >>>\r\n";
        class ControlMessage
        {
            public string password;
            public ControlMessage(string p)
            {
                password = p;
            }
        }
        class FileMessage
        {
            public byte[] data;
            public string fileName;
            public FileMessage(byte[] _data, string _fileName)
            {
                data = _data;
                fileName = _fileName;
            }
        }
        static object lk = new object();
        static Queue<string> logMessages = new Queue<string>(64);
        static Queue<FileMessage> logFiles = new Queue<FileMessage>(10);
        static Queue<ControlMessage> controlMessages = new Queue<ControlMessage>(3);
        static Thread thread = null;
        static string failure = null;
        static RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
        static byte[] signatureBytes = Encoding.UTF8.GetBytes("LogEncoded.AES128.CBC.PBKDF2\x00\x01\x02\x03");
        static byte[] verificationBytes = Encoding.UTF8.GetBytes("LogEncodedVerify");
        static byte[] footerBytes = Encoding.UTF8.GetBytes("LogEncodedEnding");
        private static string currentFilePath = "";

        public static void SetPassword(string password)
        {
            lock (lk)
            {
                checkFailure();
                if (thread == null)
                {
                    thread = new Thread(run);
                    thread.Start();
                }
                controlMessages.Enqueue(new ControlMessage(password));
                Monitor.PulseAll(lk);
            }
        }

        private static string getDirectory()
        {
            string directory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            return Path.Combine(directory, @"..\Diagnostics");
        }

        public static void Flush()
        {
            string oldPath;
            lock (lk)
            {
                oldPath = currentFilePath;
            }
            Push(MAGIC_LOG_FLUSH_FILE);
            while (true)
            {
                lock (lk)
                {
                    if (currentFilePath != oldPath || !thread.IsAlive) return;
                }
                Thread.Sleep(100);
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

        public static void PushFile(byte[] data, string fileName)
        {
            lock (lk)
            {
                checkFailure();
                logMessages.Enqueue(String.Format("Writing file \"{0}\" of size {1}\r\n", fileName, data.Length));
                logFiles.Enqueue(new FileMessage(data, fileName));
                Monitor.PulseAll(lk);
            }
        }

        private static void checkFailure()
        {
            if (failure != null && SynchronizationContext.Current != null)
            {
                Monitor.Exit(lk);
                MyMessageBox.Show("Błąd zapisywania logów diagnostycznych: " + failure, "Błąd");
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
            string password = null;
            int retryCount = 0;

            while (true)
            {
                try
                {
                    lock (lk)
                    {
                        // Wait for initial control message, skip waiting if directory is already known
                        while (password == null && controlMessages.Count == 0)
                        {
                            Monitor.Wait(lk);
                        }
                        // Get control message if available
                        while (controlMessages.Count > 0)
                        {
                            var msg = controlMessages.Dequeue();
                            password = msg.password;
                        }
                    }
                    // Go to output writing
                    do
                    {
                        var ret = writeOutput(password, ref retryCount);
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

        private static ReturnType writeOutput(string password, ref int retryCount)
        {
            var now = DateTime.Now;
            var month = String.Format("{0:yyyy-MM}", now);
            var salt = new byte[16];
            rng.GetBytes(salt);
            var iv = new byte[16];
            rng.GetBytes(iv);
            var fileName = String.Format("{0:yyyy-MM-dd HH_mm_ss.fff}.{1}.txt.enclog", now, (int)salt[0] + 256 * (int)iv[0]);
            var subdir = Path.Combine(getDirectory(), month);
            var filePath = Path.Combine(subdir, fileName);
            lock (lk)
            {
                currentFilePath = filePath;
            }
            var readBytes = 0;
            Directory.CreateDirectory(subdir);
            byte[] key;
            using (var kdf = new Rfc2898DeriveBytes(password, salt, 10000))
            {
                key = kdf.GetBytes(16);
            }
            var aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
#           if DEBUG
            using (var outputDebug = new FileStream(filePath.Replace(".enclog", ""), FileMode.CreateNew, FileAccess.Write))
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
                                    while (logMessages.Count == 0 && logFiles.Count == 0 && controlMessages.Count == 0)
                                    {
                                        Monitor.Wait(lk);
                                    }
                                    if (controlMessages.Count > 0) return ReturnType.Control;
                                    if (logFiles.Count > 0)
                                    {
                                        var inputFile = logFiles.Dequeue();
                                        int counter = 0;
                                        string logFilePath;
                                        do
                                        {
                                            counter++;
                                            logFilePath = filePath.Replace(".txt.enclog", String.Format(".{0:D3}.{1}.enclog", counter, inputFile.fileName));
                                        } while (File.Exists(logFilePath));
                                        inputLog = String.Format("Attaching file \"{0}\" as \"{1}\" of size {2}\r\n", inputFile.fileName, logFilePath, inputFile.data.Length);
                                        try
                                        {
                                            writeEncryptedFile(logFilePath, inputFile.data, key, salt);
                                        }
                                        catch (Exception ex)
                                        {
                                            inputLog = String.Format(
                                                "----------------------------------------\r\n" +
                                                " Exception during writing encrypted file \"{0}\": {1}\r\n" +
                                                "----------------------------------------\r\n",
                                                logFilePath, ex.ToString());
                                        }
                                    }
                                    else if (logMessages.Count > 0)
                                    {
                                        inputLog = logMessages.Dequeue();
                                    } else {
                                        inputLog = "This should not happen!\r\n";
                                    }
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
                                if (writtenBytes > COMPRESSED_SIZE || readBytes > UNCOMPRESSED_SIZE || inputLog == MAGIC_LOG_FLUSH_FILE)
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

        private static void writeEncryptedFile(string logFilePath, byte[] data, byte[] key, byte[] salt)
        {
            var iv = new byte[16];
            rng.GetBytes(iv);
            var aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
#           if DEBUG
            using (var outputDebug = new FileStream(logFilePath.Replace(".enclog", ""), FileMode.CreateNew, FileAccess.Write))
#           endif
            using (var output = new FileStream(logFilePath, FileMode.CreateNew, FileAccess.Write))
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
                            sha.TransformBlock(data, 0, data.Length, null, 0);
#                           if DEBUG
                            outputDebug.Write(data, 0, data.Length);
#                           endif
                            ds.Write(data, 0, data.Length);
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
            if (decryptedBytesLength >= 16 && !N.compareBytes(decryptedBytes, 0, verificationBytes))
            {
                throw new InvalidPasswordException("Invalid password");
            }

            // Get information from footer, assume truncated file if something is wrong
            int endCut = 0;
            if (decryptedBytesLength > 64)
            {
                byte[] actualFooter = new byte[16];
                if (N.compareBytes(decryptedBytes, decryptedBytesLength - 16, footerBytes))
                {
                    endCut = 48;
                }
            }

            if (endCut == 0)
            {
                if (pendingException == null) pendingException = new IOException("Truncated file");
                messages += "\r\n\r\nInvalid log file footer - file truncated.\r\n";
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

                if (endCut > 0 && !N.compareBytes(decryptedBytes, decryptedBytesLength - 48, sha.Hash))
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

        public static void CopyTo(string outputPath)
        {
            var subdirName = String.Format("Diagnostics-{0:yyyy-MM-dd HH_mm_ss}", DateTime.Now);
            outputPath = Path.Combine(outputPath, subdirName);
            Directory.CreateDirectory(outputPath);
            var startTime = DateTime.Now.AddDays(-DIAGNOSTICS_OUTPUT_DAYS);
            var inputPath = getDirectory();
            foreach (var subdir in Directory.GetDirectories(inputPath))
            {
                foreach (var file in Directory.GetFiles(Path.Combine(inputPath, subdir), "*.enclog"))
                {
                    // yyyy-MM-dd HH_mm_ss.fff.xxxxx.enclog
                    var parts = Path.GetFileNameWithoutExtension(file).Split('.');
                    if (parts.Length < 2) continue;
                    var timeStr = parts[0] + "." + parts[1];
                    DateTime date;
                    if (!DateTime.TryParseExact(timeStr, "yyyy-MM-dd HH_mm_ss.fff", null, System.Globalization.DateTimeStyles.None, out date)) continue;
                    if (date < startTime) continue;
                    try {
                        File.Copy(file, Path.Combine(outputPath, Path.GetFileName(file)));
                    } catch (Exception ex) {
                        File.WriteAllText(Path.Combine(outputPath, Path.GetFileName(file)), String.Format("Copy error: {0}", ex.ToString()));
                    }
                }
            }
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
