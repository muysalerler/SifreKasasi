using System.Security.Cryptography;
using System.Text;

namespace SifreKasasi.Business.Services
{
    public class AesEncryptionService
    {
        private readonly string _key;

        public AesEncryptionService(string key)
        {
            _key = key;
        }

        public (string EncryptedText, string Iv) Encrypt(string plainText)
        {
            //Hazırla
            using Aes aesAlg = Aes.Create();
            aesAlg.Key = Encoding.UTF8.GetBytes(_key.PadRight(32).Substring(0, 32));
            aesAlg.GenerateIV();
            //Şifrele
            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using MemoryStream msEncrypt = new MemoryStream();
            using CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
            using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
            {
                swEncrypt.Write(plainText);
            }
            //Metne çevir
            string encryptedText = Convert.ToBase64String(msEncrypt.ToArray());
            string iv = Convert.ToBase64String(aesAlg.IV);

            return (encryptedText, iv);
        }
        //Geri tekrar şifreyi göster
        public string Decrypt(string encryptedText, string ivBase64)
        {
            using Aes aesAlg = Aes.Create();
            aesAlg.Key = Encoding.UTF8.GetBytes(_key.PadRight(32).Substring(0, 32));
            aesAlg.IV = Convert.FromBase64String(ivBase64);

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(encryptedText));
            using CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
            using StreamReader srDecrypt = new StreamReader(csDecrypt);

            return srDecrypt.ReadToEnd();
        }
    }
}