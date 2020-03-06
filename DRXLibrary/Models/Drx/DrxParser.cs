using System;
using System.IO;
using System.Linq;
using System.Text;
using DRXLibrary.Models.Drx.Document;
using Force.Crc32;
using MessagePack;

namespace DRXLibrary.Models.Drx
{
    public static class DrxParser
    {
        private static void DeserialiseHeader(this DrxDocument document, Stream stream)
        {
            using (var reader = new BinaryReader(stream, Encoding.UTF8, true))
            {
                var descriptor = reader.ReadChars(3);
                if (!DrxDocument.Descriptor.ToCharArray().SequenceEqual(descriptor))
                    throw new Exception("Source data is not a DRX document! Invalid descriptor.");
                document.Id = new Guid(reader.ReadBytes(16));
                var version = (DrxDocumentVersion)reader.ReadUInt32();

                // read and validate the header block
                var headerChecksum = reader.ReadUInt32();
                var headerLength = reader.ReadUInt32();
                var header = reader.ReadBytes((int)headerLength);
                if (Crc32Algorithm.Compute(header) != headerChecksum)
                    throw new Exception("DRX header has invalid checksum!");

                // Deserialise the header
                switch (version)
                {
                    case DrxDocumentVersion.Version002:
                        document.Header = MessagePackSerializer.Deserialize<DrxDocumentHeader>(header);
                        break;
                    default:
                        throw new InvalidOperationException("Unsupported version.");
                }
            }
        }

        public static DrxDocument DeserialiseHeader(Stream stream)
        {
            var document = new DrxDocument();
            document.DeserialiseHeader(stream);
            return document;
        }

        public static void Deserialise(this DrxDocument document, Stream stream)
        {
            using (var reader = new BinaryReader(stream, Encoding.UTF8, true))
            {
                DeserialiseHeader(document, stream);

                // read the body block
                var bodyLength = reader.ReadUInt32();
                document.Body = reader.ReadBytes((int)bodyLength);
            }
        }

        public static DrxDocument Deserialise(Stream stream)
        {
            var document = new DrxDocument();
            document.Deserialise(stream);
            return document;
        }

        public static void Serialise(this DrxDocument document, Stream stream)
        {
            using (var writer = new BinaryWriter(stream, Encoding.UTF8, true))
            {
                writer.Write(DrxDocument.Descriptor.ToCharArray());
                writer.Write(document.Id.ToByteArray());
                writer.Write((uint)DrxDocumentVersion.Current);

                // write the header block
                var header = MessagePackSerializer.Serialize(document.Header);
                writer.Write(Crc32Algorithm.Compute(header));
                writer.Write((uint)header.Length);
                writer.Write(header);

                // write the body block
                writer.Write((uint)document.Body.Length);
                writer.Write(document.Body);
            }
        }
    }
}
