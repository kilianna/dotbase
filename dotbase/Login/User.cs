using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace DotBase.Login
{
    /*
     * User entry content:
     *     Name: string - user name
     *     Flags: int32 - user flags (admin on bit 0)
     *     Salt: byte[16] - salt data for KDF
     *     IV: byte[16] - initialization vector
     *     UserEncrypted: VarBytes - data encrypted with AES128(IV, KDF(Salt, user_password))
     *         VerificationPattern: byte[16]
     *         privateKeyXml: string
     *     PublicKeyXml: string - public key
     *     DatabasePasswordEcrypted: VarBytes - database password encrypted with RSA3072(PublicKeyXml, PKCS#1 v1.5)
     *     
     * VarBytes:
     *     size: int32
     *     data: byte[size]
     */
    public class User
    {
        private const uint IS_ADMIN_FLAG = 1;
        private const int RSA_KEY_SIZE = 3072;
        private const int RFC_2898_ITERATIONS = 100000;

        private static byte[] VerificationPattern = new byte[] {
            225, 19, 149, 216, 123, 212, 76, 226,
            146, 149, 132, 14, 212, 46, 112, 102,
        };

        public string Name = "";
        public string Password = "";
        private uint Flags;
        private byte[] Salt;
        private byte[] IV;
        private byte[] UserEncrypted;
        private string PublicKeyXml = "";
        private byte[] DatabasePasswordEcrypted;

        public bool IsAdmin
        {
            get { return (Flags & IS_ADMIN_FLAG) != 0; }
            set { Flags &= ~IS_ADMIN_FLAG; if (value) Flags |= IS_ADMIN_FLAG; }
        }

        /// <summary>Verify user login</summary>
        /// <param name="password">User password</param>
        /// <returns>Database password on success, "" for wrong password</returns>
        public string LogIn(string password)
        {
            if (Salt == null || IV == null || UserEncrypted == null || DatabasePasswordEcrypted == null)
                return "";
            // Make key from password
            byte[] key;
            using (var kdf = new Rfc2898DeriveBytes(password, Salt, RFC_2898_ITERATIONS))
                key = kdf.GetBytes(16);

            bool wrongPassword = false;

            // Decrypt UserEncrypted
            try
            {
                using (var ms = new MemoryStream(UserEncrypted))
                using (var cs = new CryptoStream(ms, N.aes.CreateDecryptor(key, IV), CryptoStreamMode.Read))
                using (var br = new BinaryReader(cs))
                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(RSA_KEY_SIZE))
                {
                    // Check verification pattern
                    byte[] verificationPattern = br.ReadBytes(16);
                    if (!N.compareBytes(verificationPattern, 0, VerificationPattern))
                    {
                        // Wrong password
                        wrongPassword = true;
                        return null;
                    }
                    // Decrypt database password
                    var privateKeyXml = br.ReadString();
                    rsa.FromXmlString(privateKeyXml);
                    var databasePassword = Encoding.UTF8.GetString(rsa.Decrypt(DatabasePasswordEcrypted, false));
                    Password = password;
                    return databasePassword;
                }
            }
            catch
            {
                if (wrongPassword) return null;
                throw;
            }
        }

        public void ChangeUserPassword(string newPassword, string databasePassowrd)
        {
            Salt = new byte[16];
            N.rng.GetBytes(Salt);
            IV = new byte[16];
            N.rng.GetBytes(IV);
            byte[] key;
            using (var kdf = new Rfc2898DeriveBytes(newPassword, Salt, RFC_2898_ITERATIONS))
                key = kdf.GetBytes(16);
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(RSA_KEY_SIZE))
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, N.aes.CreateEncryptor(key, IV), CryptoStreamMode.Write))
                using (var bw = new BinaryWriter(cs))
                {
                    bw.Write(VerificationPattern);
                    var privateKeyXml = rsa.ToXmlString(true);
                    bw.Write(privateKeyXml);
                }
                UserEncrypted = ms.ToArray();
                PublicKeyXml = rsa.ToXmlString(false);
                DatabasePasswordEcrypted = rsa.Encrypt(Encoding.UTF8.GetBytes(databasePassowrd), false);
                Password = newPassword;
            }
        }

        public void ChangeDatabasePassword(string newDatabasePassword)
        {
            var newPasswordBytes = Encoding.UTF8.GetBytes(newDatabasePassword);
            if (newPasswordBytes.Length > RSA_KEY_SIZE / 8 - 11)
            {
                throw new ArgumentException("Database password is too long.");
            }
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(RSA_KEY_SIZE))
            {
                rsa.FromXmlString(PublicKeyXml);
                DatabasePasswordEcrypted = rsa.Encrypt(Encoding.UTF8.GetBytes(newDatabasePassword), false);
            }
        }

        public void Write(BinaryWriter output)
        {
            output.Write(Name);
            output.Write(Flags);
            output.Write(Salt);
            output.Write(IV);
            WriteVarBytes(output, UserEncrypted);
            output.Write(PublicKeyXml);
            WriteVarBytes(output, DatabasePasswordEcrypted);
        }

        public void Read(BinaryReader input)
        {
            Name = input.ReadString();
            Flags = input.ReadUInt32();
            Salt = input.ReadBytes(16);
            IV = input.ReadBytes(16);
            UserEncrypted = ReadVarBytes(input);
            PublicKeyXml = input.ReadString();
            DatabasePasswordEcrypted = ReadVarBytes(input);
        }

        private byte[] ReadVarBytes(BinaryReader input)
        {
            var size = input.ReadInt32();
            return input.ReadBytes(size);
        }

        private void WriteVarBytes(BinaryWriter output, byte[] data)
        {
            output.Write(data.Length);
            output.Write(data);
        }

        public User Clone()
        {
            var clone = new User();
            clone.Name = Name;
            clone.Flags = Flags;
            clone.Salt = new byte[16];
            Salt.CopyTo(clone.Salt, 0);
            clone.IV = new byte[16];
            IV.CopyTo(clone.IV, 0);
            clone.UserEncrypted = new byte[UserEncrypted.Length];
            UserEncrypted.CopyTo(clone.UserEncrypted, 0);
            clone.PublicKeyXml = PublicKeyXml;
            clone.DatabasePasswordEcrypted = new byte[DatabasePasswordEcrypted.Length];
            DatabasePasswordEcrypted.CopyTo(clone.DatabasePasswordEcrypted, 0);
            return clone;
        }
    }
}
