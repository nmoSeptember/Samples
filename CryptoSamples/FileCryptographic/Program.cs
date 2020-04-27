using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FileCryptographic
{
    class Program
    {
        static void Main(string[] args)
        {
            RSACryptoServiceProvider provider = PemKeyUtils.GetRSAProviderFromPemFile(@"C:\Users\nmose\Desktop\rsa\rsa_2048_pub.pem");
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();


            string originFile = @"C:\Users\nmose\Desktop\rsa\info.txt";
            string encryFile = @"C:\Users\nmose\Desktop\rsa\info-crypto.bin";
            string decrypFile = @"C:\Users\nmose\Desktop\rsa\info-decrypto.txt";
            string msg = "";

            CryptoFile.GenerateEncryFile(provider, aes, originFile, encryFile, ref msg);

            RSACryptoServiceProvider providerPrivate = PemKeyUtils.GetRSAProviderFromPemFile(@"C:\Users\nmose\Desktop\rsa\rsa_2048_priv.pem");

            CryptoFile.DecrypFile(providerPrivate, encryFile, decrypFile, ref msg);
        }
    }
}
