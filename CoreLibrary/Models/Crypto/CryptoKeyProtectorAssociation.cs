using MessagePack;

namespace CoreLibrary.Models.Crypto
{
    [MessagePackObject]
    public class CryptoKeyProtectorAssociation
    {
        [Key(0)]
        public CryptoKeyProtectorIntent Intent { get; set; } = CryptoKeyProtectorIntent.Primary;
        [Key(1)]
        public CryptoKeyProtector Protector { get; set; }
    }
}
