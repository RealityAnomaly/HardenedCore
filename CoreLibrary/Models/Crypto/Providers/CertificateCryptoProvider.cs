using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System;
using CoreLibrary.Utilities;
using MessagePack;

namespace CoreLibrary.Models.Crypto.Providers
{
    /// <summary>
    /// <see cref="X509Certificate2"/>-based cryptographic provider.
    /// This stores a key which is encrypted by the certificate since we are quite limited in terms of bytes.
    /// </summary>
    [CryptoProvider("Certificate")]
    public class CertificateCryptoProvider : ICryptoProvider, IStatefulProvider
    {
        private CertificateCryptoProviderState _state = new CertificateCryptoProviderState();
        private byte[] _rawKey;
        private AsymmetricAlgorithm _publicKey;
        private RSA _privateKey;

        public CertificateCryptoProvider() { }
        public CertificateCryptoProvider(string serial)
        {
            _state.Serial = serial;
        }

        public byte[] Encrypt(byte[] value)
        {
            // Generate the key if not already present
            if (_state.ProviderKey == null)
            {
                if (_rawKey == null)
                    _rawKey = CertificateUtilities.GenerateEncryptionKey(_publicKey.KeySize);
                _state.ProviderKey = new RsaCryptoProvider(_privateKey).Encrypt(_rawKey);
            }

            // Encrypt the value
            return new AesCryptoProvider(new CryptoKey(new CryptoKeyProtector("None", _rawKey))).Encrypt(value);
        }

        public byte[] Decrypt(byte[] value)
        {
            // Load the certificate if not already loaded
            if (_publicKey == null || _privateKey == null) {
                var cert = CertificateUtilities.GetCertificateFromSerial(_state.Serial);
                if (cert == null || !cert.HasPrivateKey) {
                    var pkcs = CertificateUtilities.GetPkcs11CertificateFromSerial(_state.Serial);
                    if (pkcs == null) throw new ArgumentException("A certificate with this serial could not be found.", nameof(_state.Serial));

                    _publicKey = pkcs.GetRSAPublicKey();
                    _privateKey = pkcs.GetRSAPrivateKey();
                } else {
                    _publicKey = cert.PublicKey.Key;
                    _privateKey = cert.PrivateKey as RSACng;
                }
            }

            if (_rawKey == null)
                _rawKey = new RsaCryptoProvider(_privateKey).Decrypt(_state.ProviderKey);

            // Decrypt the value
            return new AesCryptoProvider(new CryptoKey(new CryptoKeyProtector("None", _rawKey))).Decrypt(value);
        }

        public object[] SavePersistentData()
        {
            return new object[] {_state.Serial, _state.ProviderKey};
        }

        public void LoadPersistentData(object[] data)
        {
            _state = new CertificateCryptoProviderState
            {
                Serial = (string)data[0],
                ProviderKey = (byte[])data[1],
            };

            //Certificate = CertificateUtilities.GetCertificateFromSerial(_state.Serial);
        }

        [MessagePackObject]
        public class CertificateCryptoProviderState
        {
            [Key(0)]
            public string Serial { get; set; }
            [Key(1)]
            public byte[] ProviderKey { get; set; }
        }
    }
}
