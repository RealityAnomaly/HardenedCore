using System;
using System.IO;
using System.Text;
using Windows.Storage.Streams;
using Newtonsoft.Json;

namespace DRXNextGeneration.Models
{
    public class DrxDocumentClipboardData
    {
        public const string Format = "drx";

        public Guid Id { get; set; }
        public Guid StoreId { get; set; }

        public IRandomAccessStream AsStream()
        {
            var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(this));
            var stream = new InMemoryRandomAccessStream();
            var writeStream = stream.AsStreamForWrite();
            using (var writer = new BinaryWriter(writeStream, Encoding.UTF8, true))
            {
                writer.Write(bytes.Length);
                writeStream.Write(bytes, 0, bytes.Length);
            }

            stream.Seek(0);
            return stream;
        }

        public static DrxDocumentClipboardData FromStream(IRandomAccessStream stream)
        {
            var readStream = stream.AsStreamForRead();
            using (var reader = new BinaryReader(readStream, Encoding.UTF8, true))
            {
                var length = reader.ReadInt32();
                var bytes = reader.ReadBytes(length);
                return JsonConvert.DeserializeObject<DrxDocumentClipboardData>(Encoding.UTF8.GetString(bytes));
            }
        }
    }
}
