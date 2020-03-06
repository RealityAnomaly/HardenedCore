using System;
using System.IO;
using System.Text;
using RtfPipe;
using BracketPipe;

namespace DRXLibrary.Models.Drx.Converters
{
    internal static class RtfConverters
    {
        static RtfConverters()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        public static byte[] ToPlainText(byte[] value)
        {
            throw new NotImplementedException();
        }

        public static byte[] ToHtml(byte[] value)
        {
            using (var reader = new StreamReader(new MemoryStream(value)))
            {
                var rtf = new RtfSource(reader);
                return Encoding.UTF8.GetBytes(Rtf.ToHtml(rtf));
            }
        }

        public static byte[] ToMarkdown(byte[] value) {
            using (var reader = new StreamReader(new MemoryStream(value)))
            {
                var rtf = new RtfSource(reader);
                using (var w = new System.IO.StringWriter())
                using (var md = new MarkdownWriter(w)) {
                    Rtf.ToHtml(rtf, md);
                    return Encoding.UTF8.GetBytes(w.ToString());
                }
            }
        }
    }
}
