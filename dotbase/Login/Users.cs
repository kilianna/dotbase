using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace DotBase.Login
{
    public class UsersManager
    {
        private static byte[] FileMagic = new byte[16] { 192, 244, 255, 93, 36, 47, 11, 127, 232, 107, 105, 4, 92, 144, 109, 91 };
        private static byte[] ObfuscationKey = new byte[16] { 134, 42, 156, 192, 244, 11, 93, 192, 167, 238, 250, 46, 132, 38, 87, 16 };

        public User[] Users = new User[0];
        public string DatabasePassword;

        public void Read(string file)
        {
            Users = new User[0];
            DatabasePassword = null;
            using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var fileHeader = new byte[32];
                var fileHeaderSize = fs.Read(fileHeader, 0, 32);
                if (fileHeaderSize != 32 || !N.compareBytes(fileHeader, 0, FileMagic))
                {
                    fs.Seek(0, SeekOrigin.Begin);
                    OldRead(fs);
                    return;
                }
                var iv = new byte[16];
                Array.Copy(fileHeader, 16, iv, 0, 16);
                using (var cs = new CryptoStream(fs, N.aes.CreateDecryptor(ObfuscationKey, iv), CryptoStreamMode.Read))
                using (var br = new BinaryReader(cs))
                {
                    var userCount = br.ReadInt32();
                    if (userCount > 1000) throw new IOException("User file corrupted");
                    var fileUsers = new User[userCount];
                    for (int i = 0; i < fileUsers.Length; i++)
                    {
                        fileUsers[i] = new User();
                        fileUsers[i].Read(br);
                    }
                    Users = fileUsers;
                }
            }
        }

        public void Write(string file)
        {
            using (var fs = new FileStream(file, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                var iv = new byte[16];
                N.rng.GetBytes(iv);
                fs.Write(FileMagic, 0, 16);
                fs.Write(iv, 0, 16);
                using (var cs = new CryptoStream(fs, N.aes.CreateEncryptor(ObfuscationKey, iv), CryptoStreamMode.Write))
                using (var bw = new BinaryWriter(cs))
                {
                    bw.Write(Users.Length);
                    foreach (var user in Users)
                    {
                        user.Write(bw);
                    }
                }
            }
        }

        public UsersManager Clone()
        {
            var clone = new UsersManager();
            clone.Users = new User[Users.Length];
            for (int i = 0; i < Users.Length; i++)
                clone.Users[i] = Users[i].Clone();
            clone.DatabasePassword = DatabasePassword;
            return clone;
        }

        public User GetUser(string name)
        {
            foreach (var user in Users)
            {
                if (user.Name == name) return user;
            }
            return null;
        }

        /// <returns>Database password on success, "" for wrong password, null for wrong user name</returns>
        public string LogIn(string name, string password)
        {
            var user = GetUser(name);
            if (user == null) return null;
            var res = user.LogIn(password);
            DatabasePassword = null;
            if (res != null && res != "")
                DatabasePassword = res;
            return res;
        }

        public void ChangeUserPassword(string name, string newPassword)
        {
            var user = GetUser(name);
            if (user == null) throw new ApplicationException(String.Format("User '{0}' does not exists.", name));
            if (DatabasePassword == null) throw new ApplicationException("You need to login before changing the password.");
            user.ChangeUserPassword(newPassword, DatabasePassword);
        }

        public void ChangeDatabasePassword(string databasePassword)
        {
            foreach (var user in Users)
            {
                user.ChangeDatabasePassword(databasePassword);
            }
            DatabasePassword = databasePassword;
        }

        private void OldRead(FileStream fs)
        {
            using (var sr = new StreamReader(fs))
            using (var ms = new MemoryStream(OldDecrypt(sr.ReadToEnd())))
            using (var br = new BinaryReader(ms))
            {
                int magic = br.ReadInt32();
                if (magic != 0x1F72D1C) throw new IOException("Nieprawidlowy plik użytkowników");
                int liczba = br.ReadInt32();
                if (liczba > 1000) throw new IOException("Nieprawidlowy plik użytkowników");
                var list = new List<Tuple<string, string, bool>>();
                for (int i = 0; i < liczba; i++)
                {
                    var name = br.ReadString();
                    var password = br.ReadString();
                    var admin = br.ReadBoolean();
                    list.Add(new Tuple<string, string, bool>(name, password, admin));
                }
                var hasloBazy = br.ReadString();
                var fileUsers = new User[liczba];
                for (int i = 0; i < liczba; i++)
                {
                    fileUsers[i] = new User();
                    fileUsers[i].Name = list[i].Item1;
                    fileUsers[i].ChangeUserPassword(list[i].Item2, hasloBazy);
                    fileUsers[i].IsAdmin = list[i].Item3;
                }
                Users = fileUsers;
            }
        }

        private byte[] OldDecrypt(string text)
        {
            var all = Convert.FromBase64String(text);
            var iv = new byte[16];
            Array.Copy(all, 0, iv, 0, iv.Length);
            var dec = N.aes.CreateDecryptor(ObfuscationKey, iv);
            return dec.TransformFinalBlock(all, iv.Length, all.Length - iv.Length);
        }
    }
}
