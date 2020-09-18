using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Windows.Storage.Streams;
using Windows.UI.Text;
using DRXLibrary.Models.Drx;

namespace DRXNextGeneration.Utilities
{
    public static class RtfExtensions
    {
        /// <summary>
        /// Loads the contents of this <see cref="ITextDocument"/> from the specified <see cref="DrxDocument"/>.
        /// </summary>
        public static void LoadFromDrx(this ITextDocument rtf, DrxDocument document)
        {
            if (document.Header.BodyType != DrxBodyType.Rtf)
                throw new Exception("Document is of an unsupported encoding.");

            var bytes = document.GetPlainTextBodyAsType(DrxBodyType.Rtf);
            if (bytes.Length <= 0)
            {
                // Remember to clear the document,
                // as we don't want leftover state for new ones
                rtf.SetText(TextSetOptions.None, string.Empty);
                return;
            } 

            using (var stream = new InMemoryRandomAccessStream())
            {
                var writeStream = stream.AsStreamForWrite();
                writeStream.Write(bytes, 0, bytes.Length);
                writeStream.Position = 0;

                stream.Seek(0);
                rtf.LoadFromStream(TextSetOptions.FormatRtf, stream);
            }
        }

        /// <summary>
        /// Saves the contents of this <see cref="ITextDocument"/> to the specified <see cref="DrxDocument"/>.
        /// </summary>
        public static void SaveToDrx(this ITextDocument rtf, DrxDocument document)
        {
            if (document.Header.BodyType != DrxBodyType.Rtf)
                throw new Exception("Document is of an unsupported encoding.");

            using (var stream = new InMemoryRandomAccessStream())
            {
                rtf.SaveToStream(TextGetOptions.FormatRtf, stream);
                using (var readStream = stream.AsStreamForRead())
                using (var memoryStream = new MemoryStream())
                {
                    readStream.CopyTo(memoryStream);
                    document.PlainTextBody = memoryStream.ToArray();
                    document.EncryptBodyBytes();
                }
            }
        }
    }
}
