using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using DRXLibrary.Models.Drx;
using DRXLibrary.Models.Drx.Converters;
using DRXNextGeneration.ViewModels;

namespace DRXNextGeneration.Common.Extensions
{
    internal static class DataPackageExtensions
    {
        /// <summary>
        /// Sets the contents of this <see cref="DataPackage"/> from the specified <see cref="DrxDocumentViewModel"/> to export.
        /// </summary>
        internal static void SetExportedDrx(this DataPackage package, DrxDocument document, IFlagResolver resolver = null)
        {
            package.Properties.Title = document.Header.Title;
            package.Properties.Description = "DRX Document";

            // Export the document to this stream
            using (var stream = new MemoryStream())
            {
                document.Export(stream, resolver);
                stream.Seek(0, SeekOrigin.Begin);

                using (var reader = new StreamReader(stream))
                {
                    var data = reader.ReadToEnd();
                    package.SetHtmlFormat(HtmlFormatHelper.CreateHtmlFormat(data));
                }
            }
        }
    }
}
