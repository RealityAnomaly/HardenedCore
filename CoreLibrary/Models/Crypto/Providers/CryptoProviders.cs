using System;
using System.Collections.Generic;
using System.Text;

namespace CoreLibrary.Models.Crypto.Providers
{
    public static class CryptoProviders
    {
        public static readonly IList<Type> UiProviders = new List<Type>
        {
            typeof(NullCryptoProvider),
            //typeof(DpapiCryptoProvider),
            typeof(AesCryptoProvider),
            typeof(CertificateCryptoProvider)
        };

        public static readonly IList<Type> Providers = new List<Type>
        {
            typeof(NullCryptoProvider),
            typeof(AesCryptoProvider),
            typeof(RsaCryptoProvider),
            //typeof(DpapiCryptoProvider),
            typeof(CertificateCryptoProvider),
        };
    }
}
