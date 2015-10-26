using System;
using System.Security.Cryptography;
using Aqueduct.Toggles.Configuration;
using Aqueduct.Toggles.Helpers;
using Moq;
using NUnit.Framework;

namespace Aqueduct.Toggles.Tests.Helpers
{
    [TestFixture]
    public class EncryptionHelperTests
    {
        [TestCase("short")]
        [TestCase("much much much much much much much much much much much much much much much much much much much much longer")]
        [TestCase("string containing special characters like §ƒÜ╗¯4Iô")]
        public void Encrypt_Then_Decrypt_ReturnsOriginalString(string original)
        {
            var encrypted = original.Encrypt();
            
            Assert.AreNotEqual(original, encrypted);
            Console.WriteLine(encrypted);

            var decrypted = encrypted.Decrypt();

            Assert.AreEqual(original, decrypted);
        }
    }
}
