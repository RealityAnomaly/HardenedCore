using System.IO;
using System.Security.Cryptography;
using System.Text;
using CoreLibrary.Utilities;

namespace CoreLibrary.Models.Crypto.Providers
{
    /// <inheritdoc />
    /// <summary>
    /// AesManaged backed implementation of <see cref="IInitiableProvider" />.
    /// </summary>
    [CryptoProvider("AES")]
    public class AesCryptoProvider : IInitiableProvider
    {
        private CryptoKey _key;

        private const int Iterations = 52000;
        private const PaddingMode Padding = PaddingMode.PKCS7;

        public AesCryptoProvider() { }
        public AesCryptoProvider(CryptoKey key)
        {
            Initialise(key);
        }

        public void Initialise(object key)
        {
            _key = (CryptoKey) key;
        }

        public byte[] Encrypt(byte[] value)
        {
            var privateKey = new Rfc2898DeriveBytes(_key.PrimaryProtector.GetKey(), EncryptionUtilities.Entropy, Iterations);

            using (var algorithm = new AesManaged())
            {
                algorithm.Key = privateKey.GetBytes(algorithm.KeySize / 8);
                algorithm.Padding = Padding;
                var transform = algorithm.CreateEncryptor(algorithm.Key, algorithm.IV);

                using (var stream = new MemoryStream())
                {
                    using (var writer = new BinaryWriter(stream, Encoding.UTF8, true))
                    {
                        // Prepend the stream with the IV length and IV
                        writer.Write(algorithm.IV.Length);
                        writer.Write(algorithm.IV);
                    }

                    using (var cryptoStream = new CryptoStream(stream, transform, CryptoStreamMode.Write))
                        cryptoStream.Write(value, 0, value.Length);
                    return stream.ToArray();
                }
            }
        }

        public byte[] Decrypt(byte[] value)
        {
            var privateKey = new Rfc2898DeriveBytes(_key.PrimaryProtector.GetKey(), EncryptionUtilities.Entropy, Iterations);

            using (var algorithm = new AesManaged())
            {
                algorithm.Key = privateKey.GetBytes(algorithm.KeySize / 8);

                using (var stream = new MemoryStream(value))
                {
                    using (var reader = new BinaryReader(stream, Encoding.UTF8, true))
                    {
                        // Read the IV from the stream
                        var length = reader.ReadInt32();
                        var iv = reader.ReadBytes(length);
                        algorithm.IV = iv;
                    }

                    algorithm.Padding = Padding;
                    var transform = algorithm.CreateDecryptor(algorithm.Key, algorithm.IV);

                    using (var cryptoStream = new CryptoStream(stream, transform, CryptoStreamMode.Read))
                    using (var outputStream = new MemoryStream())
                    {
                        var buffer = new byte[4096];
                        int count;
                        while ((count = cryptoStream.Read(buffer, 0, buffer.Length)) != 0)
                            outputStream.Write(buffer, 0, count);
                        return outputStream.ToArray();
                    }
                }
            }
        }
    }
}
