using System;
using System.Security.Cryptography;

namespace CoreLibrary.Models.Crypto.Providers
{
    [CryptoProvider("RSA")]
    public class RsaCryptoProvider : ICryptoProvider
    {
        private readonly RSA _cng;
        private readonly RSAEncryptionPadding _padding = RSAEncryptionPadding.Pkcs1;

        public RsaCryptoProvider(RSA cng)
        {
            _cng = cng;
        }

        public RsaCryptoProvider(RSA cng, RSAEncryptionPadding padding)
        {
            _cng = cng;
            _padding = padding;
        }

        public byte[] Encrypt(byte[] value)
        {
            return _cng.Encrypt(value, _padding);
        }

        public byte[] Decrypt(byte[] value)
        {
            return _cng.Decrypt(value, _padding);
        }
    }
}
