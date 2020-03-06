#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168

namespace DRXLibrary.Generated.Resolvers
{
    using System;
    using MessagePack;

    public class DRXLibraryGeneratedResolver : global::MessagePack.IFormatterResolver
    {
        public static readonly global::MessagePack.IFormatterResolver Instance = new DRXLibraryGeneratedResolver();

        DRXLibraryGeneratedResolver()
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
                var f = DRXLibraryGeneratedResolverGetFormatterHelper.GetFormatter(typeof(T));
                if (f != null)
                {
                    formatter = (global::MessagePack.Formatters.IMessagePackFormatter<T>)f;
                }
            }
        }
    }

    internal static class DRXLibraryGeneratedResolverGetFormatterHelper
    {
        static readonly global::System.Collections.Generic.Dictionary<Type, int> lookup;

        static DRXLibraryGeneratedResolverGetFormatterHelper()
        {
            lookup = new global::System.Collections.Generic.Dictionary<Type, int>(11)
            {
                {typeof(global::System.Collections.Generic.List<global::System.Guid>), 0 },
                {typeof(global::System.Collections.Generic.List<global::DRXLibrary.Models.Drx.DrxFlag>), 1 },
                {typeof(global::System.Collections.Generic.List<global::DRXLibrary.Models.Drx.Store.LocalDrxStore>), 2 },
                {typeof(global::DRXLibrary.Models.Drx.DrxSecurityLevel), 3 },
                {typeof(global::DRXLibrary.Models.Drx.DrxBodyType), 4 },
                {typeof(global::DRXLibrary.Models.Drx.Document.DrxDocumentVrel), 5 },
                {typeof(global::DRXLibrary.Models.Drx.Document.DrxDocumentHeader), 6 },
                {typeof(global::DRXLibrary.Models.Drx.DrxFlagColour), 7 },
                {typeof(global::DRXLibrary.Models.Drx.DrxFlag), 8 },
                {typeof(global::DRXLibrary.Models.Drx.Store.LocalDrxStore), 9 },
                {typeof(global::DRXLibrary.Models.Drx.Store.DrxStoreCache), 10 },
            };
        }

        internal static object GetFormatter(Type t)
        {
            int key;
            if (!lookup.TryGetValue(t, out key)) return null;

            switch (key)
            {
                case 0: return new global::MessagePack.Formatters.ListFormatter<global::System.Guid>();
                case 1: return new global::MessagePack.Formatters.ListFormatter<global::DRXLibrary.Models.Drx.DrxFlag>();
                case 2: return new global::MessagePack.Formatters.ListFormatter<global::DRXLibrary.Models.Drx.Store.LocalDrxStore>();
                case 3: return new DRXLibrary.Generated.Formatters.DRXLibrary.Models.Drx.DrxSecurityLevelFormatter();
                case 4: return new DRXLibrary.Generated.Formatters.DRXLibrary.Models.Drx.DrxBodyTypeFormatter();
                case 5: return new DRXLibrary.Generated.Formatters.DRXLibrary.Models.Drx.Document.DrxDocumentVrelFormatter();
                case 6: return new DRXLibrary.Generated.Formatters.DRXLibrary.Models.Drx.Document.DrxDocumentHeaderFormatter();
                case 7: return new DRXLibrary.Generated.Formatters.DRXLibrary.Models.Drx.DrxFlagColourFormatter();
                case 8: return new DRXLibrary.Generated.Formatters.DRXLibrary.Models.Drx.DrxFlagFormatter();
                case 9: return new DRXLibrary.Generated.Formatters.DRXLibrary.Models.Drx.Store.LocalDrxStoreFormatter();
                case 10: return new DRXLibrary.Generated.Formatters.DRXLibrary.Models.Drx.Store.DrxStoreCacheFormatter();
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

namespace DRXLibrary.Generated.Formatters.DRXLibrary.Models.Drx
{
    using System;
    using MessagePack;

    public sealed class DrxSecurityLevelFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::DRXLibrary.Models.Drx.DrxSecurityLevel>
    {
        public int Serialize(ref byte[] bytes, int offset, global::DRXLibrary.Models.Drx.DrxSecurityLevel value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            return MessagePackBinary.WriteInt32(ref bytes, offset, (Int32)value);
        }
        
        public global::DRXLibrary.Models.Drx.DrxSecurityLevel Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            return (global::DRXLibrary.Models.Drx.DrxSecurityLevel)MessagePackBinary.ReadInt32(bytes, offset, out readSize);
        }
    }

    public sealed class DrxBodyTypeFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::DRXLibrary.Models.Drx.DrxBodyType>
    {
        public int Serialize(ref byte[] bytes, int offset, global::DRXLibrary.Models.Drx.DrxBodyType value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            return MessagePackBinary.WriteUInt16(ref bytes, offset, (UInt16)value);
        }
        
        public global::DRXLibrary.Models.Drx.DrxBodyType Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            return (global::DRXLibrary.Models.Drx.DrxBodyType)MessagePackBinary.ReadUInt16(bytes, offset, out readSize);
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

namespace DRXLibrary.Generated.Formatters.DRXLibrary.Models.Drx.Document
{
    using System;
    using MessagePack;


    public sealed class DrxDocumentVrelFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::DRXLibrary.Models.Drx.Document.DrxDocumentVrel>
    {

        public int Serialize(ref byte[] bytes, int offset, global::DRXLibrary.Models.Drx.Document.DrxDocumentVrel value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedArrayHeaderUnsafe(ref bytes, offset, 4);
            offset += MessagePackBinary.WriteDouble(ref bytes, offset, value.Vividity);
            offset += MessagePackBinary.WriteDouble(ref bytes, offset, value.Remembrance);
            offset += MessagePackBinary.WriteDouble(ref bytes, offset, value.Emotion);
            offset += MessagePackBinary.WriteDouble(ref bytes, offset, value.Length);
            return offset - startOffset;
        }

        public global::DRXLibrary.Models.Drx.Document.DrxDocumentVrel Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadArrayHeader(bytes, offset, out readSize);
            offset += readSize;

            var __Vividity__ = default(double);
            var __Remembrance__ = default(double);
            var __Emotion__ = default(double);
            var __Length__ = default(double);

            for (int i = 0; i < length; i++)
            {
                var key = i;

                switch (key)
                {
                    case 0:
                        __Vividity__ = MessagePackBinary.ReadDouble(bytes, offset, out readSize);
                        break;
                    case 1:
                        __Remembrance__ = MessagePackBinary.ReadDouble(bytes, offset, out readSize);
                        break;
                    case 2:
                        __Emotion__ = MessagePackBinary.ReadDouble(bytes, offset, out readSize);
                        break;
                    case 3:
                        __Length__ = MessagePackBinary.ReadDouble(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::DRXLibrary.Models.Drx.Document.DrxDocumentVrel();
            ____result.Vividity = __Vividity__;
            ____result.Remembrance = __Remembrance__;
            ____result.Emotion = __Emotion__;
            ____result.Length = __Length__;
            return ____result;
        }
    }


    public sealed class DrxDocumentHeaderFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::DRXLibrary.Models.Drx.Document.DrxDocumentHeader>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public DrxDocumentHeaderFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "Title", 0},
                { "TimeStamp", 1},
                { "Vrel", 2},
                { "Flags", 3},
                { "Store", 4},
                { "Encrypted", 5},
                { "Key", 6},
                { "SecurityLevel", 7},
                { "BodyType", 8},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("Title"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("TimeStamp"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("Vrel"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("Flags"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("Store"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("Encrypted"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("Key"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("SecurityLevel"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("BodyType"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::DRXLibrary.Models.Drx.Document.DrxDocumentHeader value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 9);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.Title, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += formatterResolver.GetFormatterWithVerify<global::System.DateTimeOffset>().Serialize(ref bytes, offset, value.TimeStamp, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += formatterResolver.GetFormatterWithVerify<global::DRXLibrary.Models.Drx.Document.DrxDocumentVrel>().Serialize(ref bytes, offset, value.Vrel, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[3]);
            offset += formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.List<global::System.Guid>>().Serialize(ref bytes, offset, value.Flags, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[4]);
            offset += formatterResolver.GetFormatterWithVerify<global::System.Guid>().Serialize(ref bytes, offset, value.Store, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[5]);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.Encrypted);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[6]);
            offset += formatterResolver.GetFormatterWithVerify<global::CoreLibrary.Models.Crypto.CryptoKey>().Serialize(ref bytes, offset, value.Key, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[7]);
            offset += formatterResolver.GetFormatterWithVerify<global::DRXLibrary.Models.Drx.DrxSecurityLevel>().Serialize(ref bytes, offset, value.SecurityLevel, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[8]);
            offset += formatterResolver.GetFormatterWithVerify<global::DRXLibrary.Models.Drx.DrxBodyType>().Serialize(ref bytes, offset, value.BodyType, formatterResolver);
            return offset - startOffset;
        }

        public global::DRXLibrary.Models.Drx.Document.DrxDocumentHeader Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __Title__ = default(string);
            var __TimeStamp__ = default(global::System.DateTimeOffset);
            var __Vrel__ = default(global::DRXLibrary.Models.Drx.Document.DrxDocumentVrel);
            var __Flags__ = default(global::System.Collections.Generic.List<global::System.Guid>);
            var __Store__ = default(global::System.Guid);
            var __Encrypted__ = default(bool);
            var __Key__ = default(global::CoreLibrary.Models.Crypto.CryptoKey);
            var __SecurityLevel__ = default(global::DRXLibrary.Models.Drx.DrxSecurityLevel);
            var __BodyType__ = default(global::DRXLibrary.Models.Drx.DrxBodyType);

            for (int i = 0; i < length; i++)
            {
                var stringKey = global::MessagePack.MessagePackBinary.ReadStringSegment(bytes, offset, out readSize);
                offset += readSize;
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __Title__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 1:
                        __TimeStamp__ = formatterResolver.GetFormatterWithVerify<global::System.DateTimeOffset>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 2:
                        __Vrel__ = formatterResolver.GetFormatterWithVerify<global::DRXLibrary.Models.Drx.Document.DrxDocumentVrel>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 3:
                        __Flags__ = formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.List<global::System.Guid>>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 4:
                        __Store__ = formatterResolver.GetFormatterWithVerify<global::System.Guid>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 5:
                        __Encrypted__ = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                        break;
                    case 6:
                        __Key__ = formatterResolver.GetFormatterWithVerify<global::CoreLibrary.Models.Crypto.CryptoKey>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 7:
                        __SecurityLevel__ = formatterResolver.GetFormatterWithVerify<global::DRXLibrary.Models.Drx.DrxSecurityLevel>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 8:
                        __BodyType__ = formatterResolver.GetFormatterWithVerify<global::DRXLibrary.Models.Drx.DrxBodyType>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::DRXLibrary.Models.Drx.Document.DrxDocumentHeader();
            ____result.Title = __Title__;
            ____result.TimeStamp = __TimeStamp__;
            ____result.Vrel = __Vrel__;
            ____result.Flags = __Flags__;
            ____result.Store = __Store__;
            ____result.Encrypted = __Encrypted__;
            ____result.Key = __Key__;
            ____result.SecurityLevel = __SecurityLevel__;
            ____result.BodyType = __BodyType__;
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

namespace DRXLibrary.Generated.Formatters.DRXLibrary.Models.Drx
{
    using System;
    using MessagePack;


    public sealed class DrxFlagColourFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::DRXLibrary.Models.Drx.DrxFlagColour>
    {

        public int Serialize(ref byte[] bytes, int offset, global::DRXLibrary.Models.Drx.DrxFlagColour value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedArrayHeaderUnsafe(ref bytes, offset, 4);
            offset += MessagePackBinary.WriteByte(ref bytes, offset, value.A);
            offset += MessagePackBinary.WriteByte(ref bytes, offset, value.R);
            offset += MessagePackBinary.WriteByte(ref bytes, offset, value.G);
            offset += MessagePackBinary.WriteByte(ref bytes, offset, value.B);
            return offset - startOffset;
        }

        public global::DRXLibrary.Models.Drx.DrxFlagColour Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadArrayHeader(bytes, offset, out readSize);
            offset += readSize;

            var __A__ = default(byte);
            var __R__ = default(byte);
            var __G__ = default(byte);
            var __B__ = default(byte);

            for (int i = 0; i < length; i++)
            {
                var key = i;

                switch (key)
                {
                    case 0:
                        __A__ = MessagePackBinary.ReadByte(bytes, offset, out readSize);
                        break;
                    case 1:
                        __R__ = MessagePackBinary.ReadByte(bytes, offset, out readSize);
                        break;
                    case 2:
                        __G__ = MessagePackBinary.ReadByte(bytes, offset, out readSize);
                        break;
                    case 3:
                        __B__ = MessagePackBinary.ReadByte(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::DRXLibrary.Models.Drx.DrxFlagColour();
            ____result.A = __A__;
            ____result.R = __R__;
            ____result.G = __G__;
            ____result.B = __B__;
            return ____result;
        }
    }


    public sealed class DrxFlagFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::DRXLibrary.Models.Drx.DrxFlag>
    {

        public int Serialize(ref byte[] bytes, int offset, global::DRXLibrary.Models.Drx.DrxFlag value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedArrayHeaderUnsafe(ref bytes, offset, 6);
            offset += formatterResolver.GetFormatterWithVerify<global::System.Guid>().Serialize(ref bytes, offset, value.Id, formatterResolver);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.Tag, formatterResolver);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.Name, formatterResolver);
            offset += formatterResolver.GetFormatterWithVerify<global::DRXLibrary.Models.Drx.DrxFlagColour>().Serialize(ref bytes, offset, value.Colour, formatterResolver);
            offset += formatterResolver.GetFormatterWithVerify<global::DRXLibrary.Models.Drx.DrxSecurityLevel>().Serialize(ref bytes, offset, value.SecurityLevel, formatterResolver);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.Description, formatterResolver);
            return offset - startOffset;
        }

        public global::DRXLibrary.Models.Drx.DrxFlag Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadArrayHeader(bytes, offset, out readSize);
            offset += readSize;

            var __Id__ = default(global::System.Guid);
            var __Tag__ = default(string);
            var __Name__ = default(string);
            var __Description__ = default(string);
            var __Colour__ = default(global::DRXLibrary.Models.Drx.DrxFlagColour);
            var __SecurityLevel__ = default(global::DRXLibrary.Models.Drx.DrxSecurityLevel);

            for (int i = 0; i < length; i++)
            {
                var key = i;

                switch (key)
                {
                    case 0:
                        __Id__ = formatterResolver.GetFormatterWithVerify<global::System.Guid>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 1:
                        __Tag__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 2:
                        __Name__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 5:
                        __Description__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 3:
                        __Colour__ = formatterResolver.GetFormatterWithVerify<global::DRXLibrary.Models.Drx.DrxFlagColour>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 4:
                        __SecurityLevel__ = formatterResolver.GetFormatterWithVerify<global::DRXLibrary.Models.Drx.DrxSecurityLevel>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::DRXLibrary.Models.Drx.DrxFlag();
            ____result.Id = __Id__;
            ____result.Tag = __Tag__;
            ____result.Name = __Name__;
            ____result.Description = __Description__;
            ____result.Colour = __Colour__;
            ____result.SecurityLevel = __SecurityLevel__;
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

namespace DRXLibrary.Generated.Formatters.DRXLibrary.Models.Drx.Store
{
    using System;
    using MessagePack;


    public sealed class LocalDrxStoreFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::DRXLibrary.Models.Drx.Store.LocalDrxStore>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public LocalDrxStoreFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "Id", 0},
                { "Name", 1},
                { "Encrypted", 2},
                { "Key", 3},
                { "FlagDefinitions", 4},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("Id"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("Name"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("Encrypted"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("Key"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("FlagDefinitions"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::DRXLibrary.Models.Drx.Store.LocalDrxStore value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 5);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<global::System.Guid>().Serialize(ref bytes, offset, value.Id, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.Name, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.Encrypted);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[3]);
            offset += formatterResolver.GetFormatterWithVerify<global::CoreLibrary.Models.Crypto.CryptoKey>().Serialize(ref bytes, offset, value.Key, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[4]);
            offset += formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.List<global::DRXLibrary.Models.Drx.DrxFlag>>().Serialize(ref bytes, offset, value.FlagDefinitions, formatterResolver);
            return offset - startOffset;
        }

        public global::DRXLibrary.Models.Drx.Store.LocalDrxStore Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __Id__ = default(global::System.Guid);
            var __Name__ = default(string);
            var __Encrypted__ = default(bool);
            var __Key__ = default(global::CoreLibrary.Models.Crypto.CryptoKey);
            var __FlagDefinitions__ = default(global::System.Collections.Generic.List<global::DRXLibrary.Models.Drx.DrxFlag>);

            for (int i = 0; i < length; i++)
            {
                var stringKey = global::MessagePack.MessagePackBinary.ReadStringSegment(bytes, offset, out readSize);
                offset += readSize;
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __Id__ = formatterResolver.GetFormatterWithVerify<global::System.Guid>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 1:
                        __Name__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 2:
                        __Encrypted__ = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                        break;
                    case 3:
                        __Key__ = formatterResolver.GetFormatterWithVerify<global::CoreLibrary.Models.Crypto.CryptoKey>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 4:
                        __FlagDefinitions__ = formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.List<global::DRXLibrary.Models.Drx.DrxFlag>>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::DRXLibrary.Models.Drx.Store.LocalDrxStore();
            ____result.Id = __Id__;
            ____result.Name = __Name__;
            ____result.Encrypted = __Encrypted__;
            ____result.Key = __Key__;
            ____result.FlagDefinitions = __FlagDefinitions__;
            return ____result;
        }
    }


    public sealed class DrxStoreCacheFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::DRXLibrary.Models.Drx.Store.DrxStoreCache>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public DrxStoreCacheFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "Stores", 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("Stores"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::DRXLibrary.Models.Drx.Store.DrxStoreCache value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 1);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.List<global::DRXLibrary.Models.Drx.Store.LocalDrxStore>>().Serialize(ref bytes, offset, value.Stores, formatterResolver);
            return offset - startOffset;
        }

        public global::DRXLibrary.Models.Drx.Store.DrxStoreCache Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __Stores__ = default(global::System.Collections.Generic.List<global::DRXLibrary.Models.Drx.Store.LocalDrxStore>);

            for (int i = 0; i < length; i++)
            {
                var stringKey = global::MessagePack.MessagePackBinary.ReadStringSegment(bytes, offset, out readSize);
                offset += readSize;
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __Stores__ = formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.List<global::DRXLibrary.Models.Drx.Store.LocalDrxStore>>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::DRXLibrary.Models.Drx.Store.DrxStoreCache();
            ____result.Stores = __Stores__;
            return ____result;
        }
    }

}

#pragma warning restore 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612
