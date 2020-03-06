using System;
using System.Collections.Generic;
using System.Text;

namespace CoreLibrary.Models.Crypto.Providers
{
    /// <inheritdoc />
    /// <summary>
    /// Defines a cryptographic provider.
    /// </summary>
    public class CryptoProviderAttribute : Attribute
    {
        public string Name;

        public CryptoProviderAttribute(string name)
        {
            Name = name;
        }
    }
}
