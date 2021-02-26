namespace PGPEnc
{
    using Cinchoo.PGP;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using static System.Net.Mime.MediaTypeNames;

    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var keyfilePath = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            //https://pgpkeygen.com/

            string publicKey = File.ReadAllText(keyfilePath + @"\" + "publickey.asc");
            string privateKey = File.ReadAllText(keyfilePath + @"\" + "privatekey.asc");

            string secretText = "Hello world";

            string pgpName = "pgp@pgp.com";
            string pgpEmail = "pgp@pgp.com";
            string pgpComments = "pgp@pgp.com";
            string pgpPassphrase = "pgp@pgp.com";

            var encText = await EncryptRSATextToTextAsync(secretText, publicKey);

            var decText = await DecryptRSATextToTextAsync(encText, privateKey, pgpPassphrase);
        }

        public static async Task<string> EncryptRSATextToTextAsync(string text, string publicKey)
        {
            byte[] byteArrayText = Encoding.UTF8.GetBytes(text);
            MemoryStream encMemoryStream1 = new MemoryStream(byteArrayText);

            byte[] byteArrayPubKey = Encoding.UTF8.GetBytes(publicKey);
            MemoryStream streamPub = new MemoryStream(byteArrayPubKey);

            var streamEncOut = new MemoryStream();

            using (ChoPGPEncryptDecrypt pgp = new ChoPGPEncryptDecrypt())
            {
                await pgp.EncryptAsync(encMemoryStream1, streamEncOut, streamPub);
            }

            byte[] encryptedBytes = streamEncOut.ToArray();
            var encryptedText = Encoding.UTF8.GetString(encryptedBytes);

            return encryptedText;
        }

        public static async Task<string> DecryptRSATextToTextAsync(string text, string privateKey, string passphrase)
        {
            byte[] byteArrayText1 = Encoding.UTF8.GetBytes(text);
            MemoryStream encMemoryStream1 = new MemoryStream(byteArrayText1);

            byte[] byteArrayPrvKey = Encoding.UTF8.GetBytes(privateKey);
            MemoryStream streamPrv = new MemoryStream(byteArrayPrvKey);

            var streamDecOut = new MemoryStream();

            using (ChoPGPEncryptDecrypt pgp = new ChoPGPEncryptDecrypt())
            {
                await pgp.DecryptAsync(encMemoryStream1, streamDecOut, streamPrv, passphrase);
            }

            byte[] decryptedBytes = streamDecOut.ToArray();
            var decryptedText = Encoding.UTF8.GetString(decryptedBytes);

            return decryptedText;
        }
    }
}