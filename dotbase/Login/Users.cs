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
        public class NotFound : Exception { }
        public class WrongPassword : Exception { }
        public class NotLoggedIn : Exception { }

        private static byte[] FileMagic = new byte[16] { 192, 244, 255, 93, 36, 47, 11, 127, 232, 107, 105, 4, 92, 144, 109, 91 };
        private static byte[] ObfuscationKey = new byte[16] { 134, 42, 156, 192, 244, 11, 93, 192, 167, 238, 250, 46, 132, 38, 87, 16 };

        private User[] Users = new User[0];

        public UserInfo CurrentUser { get; private set; }
        public string DatabasePassword { get; private set; }

        public bool Read(string file)
        {
            Users = new User[0];
            CurrentUser = null;
            DatabasePassword = null;
            using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var fileHeader = new byte[32];
                var fileHeaderSize = fs.Read(fileHeader, 0, 32);
                if (fileHeaderSize != 32 || !N.compareBytes(fileHeader, 0, FileMagic))
                {
                    fs.Seek(0, SeekOrigin.Begin);
                    OldRead(fs);
                    return false;
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
            return true;
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
            {
                clone.Users[i] = Users[i].Clone();
                if (CurrentUser == Users[i].info)
                    clone.CurrentUser = clone.Users[i].info;
            }
            clone.DatabasePassword = DatabasePassword;
            return clone;
        }

        private User GetUser(string name)
        {
            foreach (var user in Users)
                if (user.info.Name == name) return user;
            return null;
        }

        /// <returns>Database password on success, "" for wrong password, null for wrong user name</returns>
        public string LogIn(string name, string password)
        {
            CurrentUser = null;
            DatabasePassword = null;
            var user = GetUser(name);
            if (user == null) throw new NotFound();
            var res = user.LogIn(password);
            if (res == null) throw new WrongPassword();
            CurrentUser = user.info;
            DatabasePassword = res;
            return res;
        }

        public void ChangeUserPassword(string name, string newPassword)
        {
            var user = GetUser(name);
            if (user == null) throw new NotFound();
            if (DatabasePassword == null) throw new NotLoggedIn();
            user.ChangeUserPassword(newPassword, DatabasePassword);
        }

        public void ChangeDatabasePassword(string databasePassword)
        {
            foreach (var user in Users)
                user.ChangeDatabasePassword(databasePassword);
            if (DatabasePassword != null)
                DatabasePassword = databasePassword;
        }

        public UserInfo[] GetUsers()
        {
            var res = new UserInfo[Users.Length];
            for (int i = 0; i < res.Length; i++)
                res[i] = Users[i].info;
            return res;
        }

        public UserInfo NewUser(string name, string password, bool isAdmin)
        {
            if (DatabasePassword == null) throw new NotLoggedIn();
            var user = new User();
            user.info.Name = name;
            user.info.Password = password;
            user.info.IsAdmin = isAdmin;
            user.ChangeUserPassword(password, DatabasePassword);
            var list = new List<User>(Users);
            list.Add(user);
            Users = list.ToArray();
            return user.info;
        }

        internal void RemoveUser(UserInfo userInfo)
        {
            for (int i = 0; i < Users.Length; i++) {
                var user = Users[i];
                if (user.info == userInfo) {
                    Users = Users.Take(i).Concat(Users.Skip(i + 1)).ToArray();
                    return;
                }
            }
        }
        private void OldRead(FileStream fs)
        {
            using (var sr = new StreamReader(fs))
            using (var ms = new MemoryStream(OldDecrypt(sr.ReadToEnd())))
            using (var br = new BinaryReader(ms))
            {
                int magic = br.ReadInt32();
                if (magic != 0x1F72D1C) throw new IOException("Nieprawidłowy plik użytkowników");
                int liczba = br.ReadInt32();
                if (liczba > 1000) throw new IOException("Nieprawidłowy plik użytkowników");
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
                    fileUsers[i].info.Name = list[i].Item1;
                    fileUsers[i].ChangeUserPassword(list[i].Item2, hasloBazy);
                    fileUsers[i].info.IsAdmin = list[i].Item3;
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
