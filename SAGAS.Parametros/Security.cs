using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace SAGAS.Parametros
{


    public sealed class Security
    {                                
        private static string pwd = "120712";    
        private static string vector = "zanzibar12345678";
        private static string saltValue = "sagas";
        private static int iterationRef = 1;
        private static int sizeKey = 256;
        private static string hash = "SHA1";


        #region <<< Encriptar >>>


        public static string Encrypt(string PlainText, string Password, string SaltValue, string hashAlgorithm, int PasswordIterations, string InitialVector, int KeySize)
        {
            try
            {
                byte[] InitialVectorBytes = Encoding.ASCII.GetBytes(InitialVector);
                byte[] saltValueBytes = Encoding.ASCII.GetBytes(SaltValue);
                byte[] plainTextBytes = Encoding.UTF8.GetBytes(PlainText);

                PasswordDeriveBytes password = new PasswordDeriveBytes(Password, saltValueBytes, hashAlgorithm, PasswordIterations);

                byte[] keyBytes = password.GetBytes(KeySize / 8);

                RijndaelManaged symmetricKey = new RijndaelManaged();

                symmetricKey.Mode = CipherMode.CBC;

                ICryptoTransform enSecurityr = symmetricKey.CreateEncryptor(keyBytes, InitialVectorBytes);

                MemoryStream memoryStream = new MemoryStream();

                CryptoStream SecurityStream = new CryptoStream(memoryStream, enSecurityr, CryptoStreamMode.Write);

                SecurityStream.Write(plainTextBytes, 0, plainTextBytes.Length);

                SecurityStream.FlushFinalBlock();

                byte[] cipherTextBytes = memoryStream.ToArray();

                memoryStream.Close();
                SecurityStream.Close();

                string cipherText = Convert.ToBase64String(cipherTextBytes);

                return cipherText;
            }
            catch (Exception ex)
            {
                
                return ex.ToString();
            }
        }


        public static string Encrypt(string PlainText)
        {
            return Security.Encrypt(PlainText, pwd, saltValue, hash, iterationRef, vector, sizeKey);
        }

        public static string Encrypt(string PlainText, string keyWord)
        {
            return Security.Encrypt(PlainText, keyWord, saltValue, hash, iterationRef, vector, sizeKey);
        }

        #endregion

        #region <<< Desencriptar >>>

        public static string Decrypt(string PlainText, string Password, string SaltValue, string HashAlgorithm, int PasswordIterations, string InitialVector, int KeySize)
        {
            try
            {
                byte[] InitialVectorBytes = Encoding.ASCII.GetBytes(InitialVector);
                byte[] saltValueBytes = Encoding.ASCII.GetBytes(SaltValue);

                byte[] cipherTextBytes = Convert.FromBase64String(PlainText);

                PasswordDeriveBytes password = new PasswordDeriveBytes(Password, saltValueBytes, HashAlgorithm, PasswordIterations);

                byte[] keyBytes = password.GetBytes(KeySize / 8);

                RijndaelManaged symmetricKey = new RijndaelManaged();

                symmetricKey.Mode = CipherMode.CBC;

                ICryptoTransform deSecurityr = symmetricKey.CreateDecryptor(keyBytes, InitialVectorBytes);

                MemoryStream memoryStream = new MemoryStream(cipherTextBytes);

                CryptoStream SecurityStream = new CryptoStream(memoryStream, deSecurityr, CryptoStreamMode.Read);

                byte[] plainTextBytes = new byte[cipherTextBytes.Length];

                int decryptedByteCount = SecurityStream.Read(plainTextBytes, 0, plainTextBytes.Length);

                memoryStream.Close();
                SecurityStream.Close();

                string plainText = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);

                return plainText;
            }
            catch
            {
                return null;
            }
        }

        public static string Decrypt(string PlainText)
        {
            return Security.Decrypt(PlainText, pwd, saltValue, hash, iterationRef, vector, sizeKey);
        }

        public static string Decrypt(string PlainText, string keyWord)
        {
            return Security.Decrypt(PlainText, keyWord, saltValue, hash, iterationRef, vector, sizeKey);
        }

        #endregion

    }







}

