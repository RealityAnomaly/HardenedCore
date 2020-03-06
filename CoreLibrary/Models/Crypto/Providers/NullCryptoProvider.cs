using System;
using System.Collections.Generic;
using System.Text;

namespace CoreLibrary.Models.Crypto.Providers
{
    /// <summary>
    /// Null crypto provider. Passes through the value without doing anything.
    /// </summary>
    [CryptoProvider("None")]
    public class NullCryptoProvider : ICryptoProvider
    {
        public byte[] Encrypt(byte[] value) => value;
        public byte[] Decrypt(byte[] value) => value;
    }
}
