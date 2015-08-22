using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace CTPPV5.Infrastructure.Security
{
    public class RSACrypto
    {
        private const int RSA_KEY_SIZE = 2048;
        private const int ENCRYPT_BUFFER_SIZE = RSA_KEY_SIZE / 8 - 11;
        private const int DECRYPT_BUFFER_SIZE = RSA_KEY_SIZE / 8;
        public byte[] ExportPubKey()
        {
            using (var rsa = new RSACryptoServiceProvider(RSA_KEY_SIZE))
            {
                return rsa.ExportCspBlob(false);
            }
        }

        public byte[] ExportPrivateKey()
        {
            using (var rsa = new RSACryptoServiceProvider(RSA_KEY_SIZE))
            {
                return rsa.ExportCspBlob(true);
            }
        }

        public byte[] Encrypt(byte[] key, byte[] plainText)
        {
            if (key == null || key.Length == 0) return new byte[0];
            if (plainText == null || plainText.Length == 0) return new byte[0];
            using (var rsa = new RSACryptoServiceProvider(RSA_KEY_SIZE))
            {
                rsa.ImportCspBlob(key);
                using (MemoryStream ms = new MemoryStream())
                {

                    byte[] buffer = new byte[ENCRYPT_BUFFER_SIZE];
                    int pos = 0;
                    int copyLength = buffer.Length;
                    while (true)
                    {
                        if (pos + copyLength > plainText.Length)
                            copyLength = plainText.Length - pos;
                        buffer = new byte[copyLength];
                        Array.Copy(plainText, pos, buffer, 0, copyLength);
                        pos += copyLength;
                        ms.Write(rsa.Encrypt(buffer, false), 0, DECRYPT_BUFFER_SIZE);
                        Array.Clear(buffer, 0, copyLength);
                        if (pos >= plainText.Length) break;
                    }
                    return ms.ToArray();
                }
            }
        }

        public byte[] Decrypt(byte[] key, byte[] cypher)
        {
            if (key == null || key.Length == 0) return new byte[0];
            if (cypher == null || cypher.Length == 0) return new byte[0];

            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(RSA_KEY_SIZE))
            {
                rsa.ImportCspBlob(key);
                using (MemoryStream ms = new MemoryStream(cypher.Length))
                {
                    int pos = 0;
                    byte[] buffer = new byte[DECRYPT_BUFFER_SIZE];
                    int copyLength = buffer.Length;
                    while (true)
                    {
                        Array.Copy(cypher, pos, buffer, 0, copyLength);
                        pos += copyLength;
                        var plaintText = rsa.Decrypt(buffer, false);
                        ms.Write(plaintText, 0, plaintText.Length);
                        Array.Clear(plaintText, 0, plaintText.Length);
                        Array.Clear(buffer, 0, copyLength);
                        if (pos >= cypher.Length) break;
                    }
                    return ms.ToArray();
                }
            }
        }
    }
}
