using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;
using DRXLibrary.Models.Drx;
using DRXLibrary.Models.Drx.Store;
using MessagePack;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using DRXUtility.Commands;

namespace DRXUtility
{
    internal static class ExportHelper
    {
        internal static async Task StaticExportAsync(IDrxStore store, string name) {
            var dir = Directory.CreateDirectory(name);

            // the index file
            var builder = new StringBuilder();
            builder.AppendLine("# DRX Store Export");
            builder.AppendLine($"Store ID: {store.Id}");
            builder.AppendLine($"Export Date: {DateTime.Now.ToString()}");
            builder.AppendLine();

            var names = new Dictionary<string, int>();

            var raw = store.GetDocuments();
            var documents = from entry in raw orderby entry.Header.TimeStamp descending select entry;
            foreach (var document in documents) {
                var findFlags = from def in store.FlagDefinitions 
                                join flag in document.Header.Flags on def.Id equals flag
                                select def.Tag;
                var flagNames = string.Join(", ", findFlags);

                int nameIdx = 0;
                var nameJoin = StringHelper.UrlFriendly(document.Header.Title.Replace(' ', '_'));
                if (names.ContainsKey(nameJoin)) {
                    nameIdx = names[nameJoin];
                    nameIdx += 1;
                }

                names[nameJoin] = nameIdx;

                var fileName = nameIdx == 0 ? $"{nameJoin}.md" : $"{nameJoin}-{nameIdx}.md";

                if (document.Header.SecurityLevel >= DrxSecurityLevel.Confidential || document.Header.Encrypted) {
                    builder.AppendLine($"- {document.Header.Title} ({document.Header.SecurityLevel}) {flagNames} {document.Header.TimeStamp}");
                    continue;
                } else {
                    builder.AppendLine($"- [{document.Header.Title}](index.html#!{fileName}) ({document.Header.SecurityLevel}) {flagNames} {document.Header.TimeStamp}");
                }

                await document.LoadBodyAsync();
                var docBuilder = new StringBuilder();
                docBuilder.AppendLine($"# {document.Header.Title}");
                docBuilder.AppendLine("[Back to Index](index.html#!index.md)");
                docBuilder.AppendLine($"ID: {document.Id}");
                docBuilder.AppendLine($"VREL: {document.Header.Vrel}");
                docBuilder.AppendLine($"Flags: {flagNames}");
                docBuilder.AppendLine($"Timestamp: {document.Header.TimeStamp}");
                docBuilder.AppendLine($"Security Level: {document.Header.SecurityLevel}");
                docBuilder.AppendLine();
                docBuilder.Append(Encoding.UTF8.GetString(document.GetPlainTextBodyAsType(DrxBodyType.Markdown)).Replace(@"\", "\n"));

                File.WriteAllText($"{dir.FullName}/{fileName}", docBuilder.ToString());
            }

            File.WriteAllText($"{dir.FullName}/index.md", builder.ToString());
            //ZipFile.CreateFromDirectory(dir.FullName, "export.zip");
        }
    }
}