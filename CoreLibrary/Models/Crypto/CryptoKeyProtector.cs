using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using CoreLibrary.Models.Crypto.Providers;
using MessagePack;

namespace CoreLibrary.Models.Crypto
{
    [MessagePackObject]
    public class CryptoKeyProtector : IMessagePackSerializationCallbackReceiver
    {
        #region MessagePack Properties
        /// <summary>
        /// Key that this protector is protecting.
        /// </summary>
        [Key(0)]
        public byte[] ProtectorKey { get; set; }

        /// <summary>
        /// Persistent state of the key protector.
        /// </summary>
        [Key(1)]
        public object[] ProtectorState { get; set; }

        /// <summary>
        /// Type of the key protector.
        /// </summary>
        [Key(2)]
        public string ProtectorName { get; set; }
        #endregion

        /// <summary>
        /// The crypto provider implementation.
        /// </summary>
        [IgnoreMember]
        public ICryptoProvider Provider;

        [IgnoreMember]
        public bool Unlocked => _decryptedKey != null;

        /// <summary>
        /// Decrypted version of <see cref="ProtectorKey"/> that is stored in memory.
        /// </summary>
        private byte[] _decryptedKey;

        public CryptoKeyProtector() { }
        public CryptoKeyProtector(string name, byte[] key, object parms = null, object[] state = null)
        {
            _decryptedKey = key;

            ProtectorName = name;
            ProtectorState = state;
            InstantiateProvider();

            // Initialise if possible
            if (Provider is IInitiableProvider initiable)
                initiable.Initialise(parms);

            // Finally, set the provider's key
            if (_decryptedKey != null)
                ProtectorKey = Provider.Encrypt(_decryptedKey);
        }

        private void InstantiateProvider()
        {
            var type = CryptoProviders.Providers.First(p => ((CryptoProviderAttribute) p.GetCustomAttribute(typeof(CryptoProviderAttribute))).Name == ProtectorName);
            Provider = (ICryptoProvider)Activator.CreateInstance(type);
        }

        [OnSerializing]
        internal void OnSerializingMethod(StreamingContext context) => OnBeforeSerialize();
        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context) => OnAfterDeserialize();

        public void OnBeforeSerialize()
        {
            // Save the provider's persistent data
            if (Provider is IStatefulProvider statefulProtector)
                ProtectorState = statefulProtector.SavePersistentData();
        }

        public void OnAfterDeserialize()
        {
            // Instantiate the provider object using reflection
            InstantiateProvider();
            // Load the provider's persistent data
            if (Provider is IStatefulProvider statefulProtector)
                statefulProtector.LoadPersistentData(ProtectorState);
        }

        /// <summary>
        /// Returns the plaintext of the key this protector protects,
        /// decrypting it if required.
        /// </summary>
        public byte[] GetKey()
        {
            if (_decryptedKey != null)
                return _decryptedKey;

            _decryptedKey = Provider.Decrypt(ProtectorKey);
            return _decryptedKey;
        }

        /// <summary>
        /// Sets and encrypts a new key on this protector.
        /// </summary>
        public void SetKey(byte[] key)
        {
            if (_decryptedKey == key)
                return;

            _decryptedKey = key;
            ProtectorKey = Provider.Encrypt(_decryptedKey);
        }

        
    }
}
