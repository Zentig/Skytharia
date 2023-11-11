using System.Security.Cryptography;
using System.IO;
using System.Text;
using System;

namespace Skytharia.SaveManagement.Serializers
{
    public static class Encryptor
    {
        public static string EncryptString(string toEncrypt, string key) 
        {
            byte[] initVector = new byte[16];
            byte[] dataArray;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = initVector;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memStream = new())
                {
                    using (CryptoStream crypStream = new(memStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter writer = new(crypStream))
                        {
                            writer.Write(toEncrypt);
                        }

                        dataArray = memStream.ToArray();
                    }
                }
            }
            return Convert.ToBase64String(dataArray);
        }
        public static string DecryptString(string toDecrypt, string key) 
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(toDecrypt);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}   