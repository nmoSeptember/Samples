using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class CryptoFile
    {
        public static bool GenerateEncryFile(RSACryptoServiceProvider rsaServiceProvider, AesCryptoServiceProvider aesSP, string originFilePath, string newFilePath, ref string msg)
        {
            if (!File.Exists(originFilePath))
            {
                msg = "";
                return false;
            }

            byte[] keyArray = rsaServiceProvider.Encrypt(aesSP.Key, false);
            byte[] ivArray = rsaServiceProvider.Encrypt(aesSP.IV, false);

            FileStream originStream = new FileStream(originFilePath, FileMode.Open);
            byte[] originArray = new byte[originStream.Length];
            originStream.Read(originArray, 0, originArray.Length);
            originStream.Close();

            MemoryStream ms = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(ms, aesSP.CreateEncryptor(aesSP.Key, aesSP.IV), CryptoStreamMode.Write);
            cryptoStream.Write(originArray, 0, originArray.Length);
            cryptoStream.FlushFinalBlock();

            byte[] encryedData = new byte[ms.Length];
            ms.Position = 0;
            ms.Read(encryedData, 0, encryedData.Length);

            cryptoStream.Close();
            ms.Close();

            FileStream fileStream = new FileStream(newFilePath, FileMode.Create);
            fileStream.Write(keyArray, 0, keyArray.Length);
            fileStream.Write(ivArray, 0, ivArray.Length);
            fileStream.Write(encryedData, 0, encryedData.Length);
            fileStream.Close();
            return true;
        }


        public static bool DecrypFile(RSACryptoServiceProvider rsaServiceProvider, string originFilePath, string newFilePath, ref string msg)
        {
            if (!File.Exists(originFilePath))
            {
                msg = "";
                return false;
            }

            FileStream originStream = new FileStream(originFilePath, FileMode.Open);

            byte[] encryptedSymmetricKey = new byte[256];
            byte[] encryptedSymmetricIV = new byte[256];
            byte[] encryptedData = new byte[originStream.Length - 512];

            originStream.Read(encryptedSymmetricKey, 0, 256);
            originStream.Read(encryptedSymmetricIV, 0, 256);
            originStream.Read(encryptedData, 0, encryptedData.Length);

            byte[] symmetricKey = rsaServiceProvider.Decrypt(encryptedSymmetricKey, false);
            byte[] symmetricIV = rsaServiceProvider.Decrypt(encryptedSymmetricIV, false);

            AesCryptoServiceProvider aesCryptoService = new AesCryptoServiceProvider();

            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, aesCryptoService.CreateDecryptor(symmetricKey, symmetricIV), CryptoStreamMode.Write);
            cryptoStream.Write(encryptedData, 0, encryptedData.Length);
            cryptoStream.FlushFinalBlock();


            byte[] decrypData = new byte[memoryStream.Length];
            memoryStream.Position = 0;
            memoryStream.Read(decrypData, 0, decrypData.Length);

            FileStream fileStream = new FileStream(newFilePath, FileMode.Create);
            fileStream.Write(decrypData, 0, decrypData.Length);
            fileStream.Close();

            cryptoStream.Close();
            memoryStream.Close();

            return true;
        }

    }
}
