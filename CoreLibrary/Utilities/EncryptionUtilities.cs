using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace CoreLibrary.Utilities
{
    public class EncryptionUtilities
    {
        /// <summary>
        /// Entropy value used to make tampering by other applications a little more difficult.
        /// Obviously this is not intended to provide much in the way of real security!
        /// </summary>
        internal static readonly byte[] Entropy = {
            0xCC, 0x5F, 0x29, 0x10, 0x5E, 0x99, 0x6D, 0x57,
            0x0B, 0x4C, 0x83, 0x0E, 0xBB, 0x4D, 0x6B, 0x9A
        };

        public static byte[] GenerateRandomBytes(int length = 512)
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var bytes = new byte[length];
                rng.GetBytes(bytes);

                return bytes;
            }
        }
    }
}
