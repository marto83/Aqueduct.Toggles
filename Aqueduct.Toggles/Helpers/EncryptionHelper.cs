using System;
using System.IO;
using System.Security.Cryptography;
using Aqueduct.Toggles.Configuration;

namespace Aqueduct.Toggles.Helpers
{
    public static class EncryptionHelper
    {
        private const string Splitter = "///===///";

        internal static Func<IEncryptionConfiguration> GetEncryptionSettings =
            () => FeatureToggleConfigurationSection.Settings().Encryption;

        public static string Encrypt(this string input)
        {
            var encryption = GetEncryptionSettings();
            using (var aes = Aes.Create())
            {
                aes.Key = Convert.FromBase64String(encryption.Key);

                var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                byte[] encrypted;
                using (var memoryStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (var writer = new StreamWriter(cryptoStream))
                        {
                            writer.Write(input);
                        }
                        encrypted = memoryStream.ToArray();
                    }
                }

                var vector = Convert.ToBase64String(aes.IV);
                return vector + Splitter + Convert.ToBase64String(encrypted);
            }
        }

        public static string Decrypt(this string encrypted)
        {
            string plaintext;

            var encryption = GetEncryptionSettings();
            using (var aes = Aes.Create())
            {
                aes.Key = Convert.FromBase64String(encryption.Key);

                var inputArr = encrypted.Split(new[] {Splitter}, StringSplitOptions.None);

                if (inputArr.Length != 2)
                    throw new ArgumentException("Input string missing vector", nameof(encrypted));

                aes.IV = Convert.FromBase64String(inputArr[0]);

                var input = Convert.FromBase64String(inputArr[1]);
                
                using (var memoryStream = new MemoryStream(input))
                {
                    var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                    using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (var reader = new StreamReader(cryptoStream))
                        {
                            plaintext = reader.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }
    }
}