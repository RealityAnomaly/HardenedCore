using System;
using System.Security.Cryptography;
using CoreLibrary.Utilities;

namespace CoreLibrary.Models.Crypto.Providers
{
    /**
    /// <summary>
    /// Windows DPAPI implementation of <see cref="ICryptoProvider"/>.
    /// </summary>
    [CryptoProvider("DPAPI")]
    public class DpapiCryptoProvider : ICryptoProvider
    {
        public byte[] Encrypt(byte[] value)
        {
            return ProtectedData.Protect(value, EncryptionUtilities.Entropy, DataProtectionScope.CurrentUser);
        }

        public byte[] Decrypt(byte[] value)
        {
            return ProtectedData.Unprotect(value, EncryptionUtilities.Entropy, DataProtectionScope.CurrentUser);
        }
    }*/
}
