using System;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace Encrypted_Notebook.Class
{
    class EncryptionManager
    {
        readonly Random rng = new Random();

        public byte[] GetNewSalt()
        {
            byte[] newSalt = new byte[] { (byte)rng.Next(1, 64), (byte)rng.Next(1, 64), (byte)rng.Next(1, 64), (byte)rng.Next(1, 64), (byte)rng.Next(1, 64), (byte)rng.Next(1, 64), (byte)rng.Next(1, 64), (byte)rng.Next(1, 64) };
            return newSalt;
        }

        public string GetHash_SHA512(string input)
        {
            try
            {
                if (input == null)
                    return null;

                StringBuilder Sb = new StringBuilder();
                using (var hash = SHA512.Create())
                {
                    Encoding enc = Encoding.UTF8;
                    Byte[] result = hash.ComputeHash(enc.GetBytes(input));

                    foreach (Byte b in result)
                        Sb.Append(b.ToString("x2"));
                }
                return Sb.ToString();
            }
            catch { return null; }
        }

        public string EncryptAES256Salt(string input, string password, byte[] salt)
        {
            try 
            {
                // Get the bytes of the string
                byte[] bytesToBeEncrypted = Encoding.UTF8.GetBytes(input);
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

                // Hash the password with SHA256
                passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

                byte[] bytesEncrypted = AES256_Encrypt(bytesToBeEncrypted, passwordBytes, salt);

                string result = Convert.ToBase64String(bytesEncrypted);

                return result;
            }
            catch { return null; }
        }
        public string DecryptAES256Salt(string input, string password, byte[] salt)
        {
            try
            {
                // Get the bytes of the string
                byte[] bytesToBeDecrypted = Convert.FromBase64String(input);
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

                byte[] bytesDecrypted = AES256_Decrypt(bytesToBeDecrypted, passwordBytes, salt);

                string result = Encoding.UTF8.GetString(bytesDecrypted);

                return result;
            }
            catch { return null; }
        }

        private byte[] AES256_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes, byte[] salt)
        {
            try
            {
                byte[] encryptedBytes = null;

                using (MemoryStream ms = new MemoryStream())
                {
                    using (RijndaelManaged AES = new RijndaelManaged())
                    {
                        AES.KeySize = 256;
                        AES.BlockSize = 128;

                        var key = new Rfc2898DeriveBytes(passwordBytes, salt, 1000);
                        AES.Key = key.GetBytes(AES.KeySize / 8);
                        AES.IV = key.GetBytes(AES.BlockSize / 8);

                        AES.Mode = CipherMode.CBC;

                        using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                            cs.Close();
                        }
                        encryptedBytes = ms.ToArray();
                    }
                }

                return encryptedBytes;
            }
            catch { return null; }
        }
        private byte[] AES256_Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes, byte[] salt)
        {
            try
            {
                byte[] decryptedBytes = null;

                using (MemoryStream ms = new MemoryStream())
                {
                    using (RijndaelManaged AES = new RijndaelManaged())
                    {
                        AES.KeySize = 256;
                        AES.BlockSize = 128;

                        var key = new Rfc2898DeriveBytes(passwordBytes, salt, 1000);
                        AES.Key = key.GetBytes(AES.KeySize / 8);
                        AES.IV = key.GetBytes(AES.BlockSize / 8);

                        AES.Mode = CipherMode.CBC;

                        using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                            cs.Close();
                        }
                        decryptedBytes = ms.ToArray();
                    }
                }

                return decryptedBytes;
            }
            catch { return null; }
        }
    }
}
