using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using RSAExtensions;

namespace Homo.Core.Helpers
{
    public class CryptographicHelper
    {
        private static Dictionary<string, RSA> _RSAEncrypters;
        public static string GenerateSaltedHash(string password, string salt)
        {
            byte[] byteOfSalt = Encoding.ASCII.GetBytes(salt);
            var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, byteOfSalt, 10000);
            var hashPassword = Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(128));
            return hashPassword;
        }

        public static string GetSpecificLengthRandomString(int size, bool isLowerCase = false, bool onlyNumber = false)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                if (onlyNumber)
                {
                    ch = Convert.ToChar(Convert.ToInt32(Math.Floor(10 * random.NextDouble() + 48)));
                }
                else
                {
                    ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                }

                builder.Append(ch);
            }
            if (isLowerCase)
            {
                return builder.ToString().ToLower();
            }
            return builder.ToString();
        }

        public static string GetRandomNumber(int size)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < size; i++)
            {
                builder.Append(random.Next(10).ToString());
            }

            return builder.ToString();
        }

        public static string GetSalt(int maxSize)
        {
            var salt = new byte[maxSize];
            using (var random = new RNGCryptoServiceProvider())
            {
                random.GetNonZeroBytes(salt);
            }
            return System.Text.Encoding.UTF8.GetString(salt);
        }

        public static string GetHiddenString(string originString, int displayStartLength, int displayEndLength)
        {
            if (originString.Length > displayStartLength + displayEndLength)
            {
                return $"{originString.Substring(0, displayStartLength)}{"".PadLeft(originString.Length - displayStartLength - displayEndLength, '*')}{originString.Substring(originString.Length - displayEndLength)}";
            }
            else
            {
                return "".PadLeft(displayStartLength + displayEndLength, '*');
            }
        }

        public static string GetRSAEncryptResult(string keyPath, string originString)
        {
            if (_RSAEncrypters == null)
            {
                _RSAEncrypters = new Dictionary<string, RSA>();
            }

            if (!_RSAEncrypters.ContainsKey(keyPath) || _RSAEncrypters[keyPath] == null)
            {
                byte[] publicKeyAsBytes = System.IO.File.ReadAllBytes(keyPath);
                RSA rsa = RSA.Create(4096);
                rsa.ImportRSAPublicKey(publicKeyAsBytes, out _);
                _RSAEncrypters.Add(keyPath, rsa);
            }

            return _RSAEncrypters[keyPath].EncryptBigData(originString, RSAEncryptionPadding.Pkcs1);
        }
    }
}
