#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168

namespace CoreLibrary.Generated.Resolvers
{
    using System;
    using MessagePack;

    public class CoreLibraryGeneratedResolver : global::MessagePack.IFormatterResolver
    {
        public static readonly global::MessagePack.IFormatterResolver Instance = new CoreLibraryGeneratedResolver();

        CoreLibraryGeneratedResolver()
        {

        }

        public global::MessagePack.Formatters.IMessagePackFormatter<T> GetFormatter<T>()
        {
            return FormatterCache<T>.formatter;
        }

        static class FormatterCache<T>
        {
            public static readonly global::MessagePack.Formatters.IMessagePackFormatter<T> formatter;

            static FormatterCache()
            {
                var f = CoreLibraryGeneratedResolverGetFormatterHelper.GetFormatter(typeof(T));
                if (f != null)
                {
                    formatter = (global::MessagePack.Formatters.IMessagePackFormatter<T>)f;
                }
            }
        }
    }

    internal static class CoreLibraryGeneratedResolverGetFormatterHelper
    {
        static readonly global::System.Collections.Generic.Dictionary<Type, int> lookup;

        static CoreLibraryGeneratedResolverGetFormatterHelper()
        {
            lookup = new global::System.Collections.Generic.Dictionary<Type, int>(7)
            {
                {typeof(object[]), 0 },
                {typeof(global::System.Collections.Generic.List<global::CoreLibrary.Models.Crypto.CryptoKeyProtectorAssociation>), 1 },
                {typeof(global::CoreLibrary.Models.Crypto.CryptoKeyProtectorIntent), 2 },
                {typeof(global::CoreLibrary.Models.Crypto.CryptoKeyProtector), 3 },
                {typeof(global::CoreLibrary.Models.Crypto.CryptoKeyProtectorAssociation), 4 },
                {typeof(global::CoreLibrary.Models.Crypto.CryptoKey), 5 },
                {typeof(global::CoreLibrary.Models.Crypto.Providers.CertificateCryptoProvider.CertificateCryptoProviderState), 6 },
            };
        }

        internal static object GetFormatter(Type t)
        {
            int key;
            if (!lookup.TryGetValue(t, out key)) return null;

            switch (key)
            {
                case 0: return new global::MessagePack.Formatters.ArrayFormatter<object>();
                case 1: return new global::MessagePack.Formatters.ListFormatter<global::CoreLibrary.Models.Crypto.CryptoKeyProtectorAssociation>();
                case 2: return new CoreLibrary.Generated.Formatters.CoreLibrary.Models.Crypto.CryptoKeyProtectorIntentFormatter();
                case 3: return new CoreLibrary.Generated.Formatters.CoreLibrary.Models.Crypto.CryptoKeyProtectorFormatter();
                case 4: return new CoreLibrary.Generated.Formatters.CoreLibrary.Models.Crypto.CryptoKeyProtectorAssociationFormatter();
                case 5: return new CoreLibrary.Generated.Formatters.CoreLibrary.Models.Crypto.CryptoKeyFormatter();
                case 6: return new CoreLibrary.Generated.Formatters.CoreLibrary.Models.Crypto.Providers.CertificateCryptoProvider_CertificateCryptoProviderStateFormatter();
                default: return null;
            }
        }
    }
}

#pragma warning restore 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612

#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168

namespace CoreLibrary.Generated.Formatters.CoreLibrary.Models.Crypto
{
    using System;
    using MessagePack;

    public sealed class CryptoKeyProtectorIntentFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::CoreLibrary.Models.Crypto.CryptoKeyProtectorIntent>
    {
        public int Serialize(ref byte[] bytes, int offset, global::CoreLibrary.Models.Crypto.CryptoKeyProtectorIntent value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            return MessagePackBinary.WriteInt32(ref bytes, offset, (Int32)value);
        }
        
        public global::CoreLibrary.Models.Crypto.CryptoKeyProtectorIntent Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            return (global::CoreLibrary.Models.Crypto.CryptoKeyProtectorIntent)MessagePackBinary.ReadInt32(bytes, offset, out readSize);
        }
    }


}

#pragma warning restore 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612


#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168

namespace CoreLibrary.Generated.Formatters.CoreLibrary.Models.Crypto
{
    using System;
    using MessagePack;


    public sealed class CryptoKeyProtectorFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::CoreLibrary.Models.Crypto.CryptoKeyProtector>
    {

        public int Serialize(ref byte[] bytes, int offset, global::CoreLibrary.Models.Crypto.CryptoKeyProtector value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            value.OnBeforeSerialize();
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedArrayHeaderUnsafe(ref bytes, offset, 3);
            offset += formatterResolver.GetFormatterWithVerify<byte[]>().Serialize(ref bytes, offset, value.ProtectorKey, formatterResolver);
            offset += formatterResolver.GetFormatterWithVerify<object[]>().Serialize(ref bytes, offset, value.ProtectorState, formatterResolver);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.ProtectorName, formatterResolver);
            return offset - startOffset;
        }

        public global::CoreLibrary.Models.Crypto.CryptoKeyProtector Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadArrayHeader(bytes, offset, out readSize);
            offset += readSize;

            var __ProtectorKey__ = default(byte[]);
            var __ProtectorState__ = default(object[]);
            var __ProtectorName__ = default(string);

            for (int i = 0; i < length; i++)
            {
                var key = i;

                switch (key)
                {
                    case 0:
                        __ProtectorKey__ = formatterResolver.GetFormatterWithVerify<byte[]>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 1:
                        __ProtectorState__ = formatterResolver.GetFormatterWithVerify<object[]>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 2:
                        __ProtectorName__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::CoreLibrary.Models.Crypto.CryptoKeyProtector();
            ____result.ProtectorKey = __ProtectorKey__;
            ____result.ProtectorState = __ProtectorState__;
            ____result.ProtectorName = __ProtectorName__;
            ____result.OnAfterDeserialize();
            return ____result;
        }
    }


    public sealed class CryptoKeyProtectorAssociationFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::CoreLibrary.Models.Crypto.CryptoKeyProtectorAssociation>
    {

        public int Serialize(ref byte[] bytes, int offset, global::CoreLibrary.Models.Crypto.CryptoKeyProtectorAssociation value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedArrayHeaderUnsafe(ref bytes, offset, 2);
            offset += formatterResolver.GetFormatterWithVerify<global::CoreLibrary.Models.Crypto.CryptoKeyProtectorIntent>().Serialize(ref bytes, offset, value.Intent, formatterResolver);
            offset += formatterResolver.GetFormatterWithVerify<global::CoreLibrary.Models.Crypto.CryptoKeyProtector>().Serialize(ref bytes, offset, value.Protector, formatterResolver);
            return offset - startOffset;
        }

        public global::CoreLibrary.Models.Crypto.CryptoKeyProtectorAssociation Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadArrayHeader(bytes, offset, out readSize);
            offset += readSize;

            var __Intent__ = default(global::CoreLibrary.Models.Crypto.CryptoKeyProtectorIntent);
            var __Protector__ = default(global::CoreLibrary.Models.Crypto.CryptoKeyProtector);

            for (int i = 0; i < length; i++)
            {
                var key = i;

                switch (key)
                {
                    case 0:
                        __Intent__ = formatterResolver.GetFormatterWithVerify<global::CoreLibrary.Models.Crypto.CryptoKeyProtectorIntent>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 1:
                        __Protector__ = formatterResolver.GetFormatterWithVerify<global::CoreLibrary.Models.Crypto.CryptoKeyProtector>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::CoreLibrary.Models.Crypto.CryptoKeyProtectorAssociation();
            ____result.Intent = __Intent__;
            ____result.Protector = __Protector__;
            return ____result;
        }
    }


    public sealed class CryptoKeyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::CoreLibrary.Models.Crypto.CryptoKey>
    {

        public int Serialize(ref byte[] bytes, int offset, global::CoreLibrary.Models.Crypto.CryptoKey value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedArrayHeaderUnsafe(ref bytes, offset, 2);
            offset += formatterResolver.GetFormatterWithVerify<global::System.Guid>().Serialize(ref bytes, offset, value.KeyId, formatterResolver);
            offset += formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.List<global::CoreLibrary.Models.Crypto.CryptoKeyProtectorAssociation>>().Serialize(ref bytes, offset, value.Protectors, formatterResolver);
            return offset - startOffset;
        }

        public global::CoreLibrary.Models.Crypto.CryptoKey Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadArrayHeader(bytes, offset, out readSize);
            offset += readSize;

            var __KeyId__ = default(global::System.Guid);
            var __Protectors__ = default(global::System.Collections.Generic.List<global::CoreLibrary.Models.Crypto.CryptoKeyProtectorAssociation>);

            for (int i = 0; i < length; i++)
            {
                var key = i;

                switch (key)
                {
                    case 0:
                        __KeyId__ = formatterResolver.GetFormatterWithVerify<global::System.Guid>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 1:
                        __Protectors__ = formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.List<global::CoreLibrary.Models.Crypto.CryptoKeyProtectorAssociation>>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::CoreLibrary.Models.Crypto.CryptoKey();
            ____result.KeyId = __KeyId__;
            ____result.Protectors = __Protectors__;
            return ____result;
        }
    }

}

#pragma warning restore 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612
#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168

namespace CoreLibrary.Generated.Formatters.CoreLibrary.Models.Crypto.Providers
{
    using System;
    using MessagePack;


    public sealed class CertificateCryptoProvider_CertificateCryptoProviderStateFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::CoreLibrary.Models.Crypto.Providers.CertificateCryptoProvider.CertificateCryptoProviderState>
    {

        public int Serialize(ref byte[] bytes, int offset, global::CoreLibrary.Models.Crypto.Providers.CertificateCryptoProvider.CertificateCryptoProviderState value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedArrayHeaderUnsafe(ref bytes, offset, 2);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.Serial, formatterResolver);
            offset += formatterResolver.GetFormatterWithVerify<byte[]>().Serialize(ref bytes, offset, value.ProviderKey, formatterResolver);
            return offset - startOffset;
        }

        public global::CoreLibrary.Models.Crypto.Providers.CertificateCryptoProvider.CertificateCryptoProviderState Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadArrayHeader(bytes, offset, out readSize);
            offset += readSize;

            var __Serial__ = default(string);
            var __ProviderKey__ = default(byte[]);

            for (int i = 0; i < length; i++)
            {
                var key = i;

                switch (key)
                {
                    case 0:
                        __Serial__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 1:
                        __ProviderKey__ = formatterResolver.GetFormatterWithVerify<byte[]>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::CoreLibrary.Models.Crypto.Providers.CertificateCryptoProvider.CertificateCryptoProviderState();
            ____result.Serial = __Serial__;
            ____result.ProviderKey = __ProviderKey__;
            return ____result;
        }
    }

}

#pragma warning restore 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612
