using System.Security.Cryptography;
using System.Text;
using PixelLib.Models;

namespace PixelLib.Encription
{
    public class AsymmetricManager
    {
        public static KersysKeys GenerateKeys()
        {
            RSACryptoServiceProvider rsa = new(2048);
            string publicKeyXML = rsa.ToXmlString(false);
            string privateKeyXML = rsa.ToXmlString(true);
            return new(publicKeyXML, privateKeyXML);
        }

        public static string Encrypt(byte[] byteData, string key)
        {
            using var rsa = RSA.Create(2048); 
            rsa.FromXmlString(key);

            byte[] encryptedData = rsa.Encrypt(byteData, RSAEncryptionPadding.Pkcs1);

            return Convert.ToBase64String(encryptedData);
        }

        public static string Decrypt(byte[] cipherText, string key)
        {
            using var rsa = RSA.Create(2048); 
            rsa.FromXmlString(key);
            byte[] encryptedData = rsa.Decrypt(cipherText, RSAEncryptionPadding.Pkcs1);
            return Encoding.UTF8.GetString(encryptedData);
        }
    }
}

