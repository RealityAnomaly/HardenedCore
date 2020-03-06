namespace CoreLibrary.Models.Crypto.Providers
{
    public interface ICryptoProvider
    {
        /// <summary>
        /// Encrypts the specified bytes.
        /// </summary>
        byte[] Encrypt(byte[] value);

        /// <summary>
        /// Decrypts the specified bytes.
        /// </summary>
        byte[] Decrypt(byte[] value);
    }
}
