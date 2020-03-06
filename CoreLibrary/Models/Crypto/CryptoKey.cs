using System;
using System.Collections.Generic;
using System.Linq;
using MessagePack;

namespace CoreLibrary.Models.Crypto
{
    /// <summary>
    /// Represents a single cryptographic key,
    /// which is protected by one or more key protectors.
    /// </summary>
    [MessagePackObject]
    public sealed class CryptoKey : ICloneable
    {
        #region MessagePack Properties
        /// <summary>
        /// Unique identifier for this key.
        /// </summary>
        [Key(0)]
        public Guid KeyId { get; set; } = Guid.NewGuid();
        [Key(1)]
        public List<CryptoKeyProtectorAssociation> Protectors { get; set; } = new List<CryptoKeyProtectorAssociation>();
        #endregion

        /// <summary>
        /// Gets or sets the primary key protector.
        /// </summary>
        [IgnoreMember]
        public CryptoKeyProtector PrimaryProtector
        {
            get => Protectors.FirstOrDefault(p => p.Intent == CryptoKeyProtectorIntent.Primary)?.Protector;
            set
            {
                var protector = Protectors.FirstOrDefault(p => p.Intent == CryptoKeyProtectorIntent.Primary);
                if (protector == null)
                {
                    Protectors.Add(new CryptoKeyProtectorAssociation { Intent = CryptoKeyProtectorIntent.Primary, Protector = value });
                    return;
                }

                protector.Protector = value;
            }
        }

        public CryptoKey() { }
        public CryptoKey(CryptoKeyProtector primary)
        {
            // Set the protector before the key, because SetKey
            // encrypts the key and the protector can't be null
            PrimaryProtector = primary;
        }

        public CryptoKey(CryptoKeyProtector primary, byte[] primaryKey)
        {
            PrimaryProtector = primary;
            PrimaryProtector.SetKey(primaryKey);
        }

        public object Clone()
        {
            var key = new CryptoKey {KeyId = KeyId};
            foreach (var protector in Protectors)
                key.Protectors.Add(protector);
            return key;
        }
    }
}
