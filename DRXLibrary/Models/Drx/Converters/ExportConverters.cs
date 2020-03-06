using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using BracketPipe;

namespace DRXLibrary.Models.Drx.Converters
{
    public static class ExportConverters
    {
        /// <summary>
        /// Exports the document as HTML to the specified stream.
        /// </summary>
        public static void Export(this DrxDocument document, Stream stream, IFlagResolver resolver = null)
        {
            var rtfHtml = Encoding.UTF8.GetString(document.GetPlainTextBodyAsType(DrxBodyType.Html));

            // Execute redactions
            var regex = new Regex(@"\<span[^\<]*background:#FF0000[^\<]*\>(.+?)\<\/span\>");
            rtfHtml = regex.Replace(rtfHtml, @"<span>[REDACTED]</span>");

            // Build the "short" security level ID
            var secKey = document.Header.SecurityLevel.ToString().ToUpper();
            var secValue = ConverterMappings.SecurityLevelToSecFlag[document.Header.SecurityLevel];

            var flagBuilder = new StringBuilder();
            var count = document.Header.Flags.Count;
            if (resolver != null && count > 0)
            {
                var i = 0;
                foreach (var id in document.Header.Flags)
                {
                    var flag = resolver.ResolveFlag(id);
                    flagBuilder.Append(flag.Tag);
                    if (i < count)
                        flagBuilder.Append("/");
                    i++;
                }

                // strip the trailing slash
                flagBuilder.Length--;
            }

            // Write it out to HTML
            using (var s = new StreamWriter(stream, Encoding.UTF8, 1024, true))
            using (var w = new HtmlTextWriter(s))
            {
                w["html"]
                    ["head"]
                        ["title"].Text(document.Header.Title)["/title"]
                    ["/head"]
                    ["body"].Attr("style", "font-family: Arial;")
                        ["h1"].Text(document.Header.Title)["/h1"]
                        ["h3"].Text($"{secKey} {secValue}//{flagBuilder}")["/h3"].Flush();
                w.WriteRaw(rtfHtml);
                w["/body"]["/html"].Flush();
            }
        }
    }
}
