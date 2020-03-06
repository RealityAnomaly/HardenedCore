using System;
using System.IO;
using System.Text;
using DRXLibrary.Models.Drx;
using DRXLibrary.Models.Drx.Store;
using MessagePack;
using System.Linq;
using System.Collections.Generic;
using CommandLine;

using DRXUtility.Commands;

namespace DRXUtility
{
    internal class Program
    {
        private static readonly string STORE_PATH = Path.Combine(Environment.GetEnvironmentVariable("HOME"), ".local/share/drx/stores");

        public static void Main(string[] args)
        {
            var raw = File.ReadAllBytes(Path.Combine(STORE_PATH, "stores.pack"));
            var cache = MessagePackSerializer.Deserialize<DrxStoreCache>(raw);

            Parser.Default.ParseArguments<Options>(args).WithParsed(async o => {
                // list stores
                if (o.Store == null) {
                    foreach (var store in cache.Stores) {
                        Console.WriteLine($"{store.Name} ({store.Id})");
                    }

                    return;
                }

                var found = cache.Stores.FirstOrDefault(s => s.Id == o.Store);
                if (found == null) return;

                await found.LoadAsync();

                if (o.ShowFlags) {
                    ReportingHelper.PrintFlags(found, f => true);
                    return;
                }

                IEnumerable<DrxFlag> findFlags = null;
                IEnumerable<Guid> flagIds = null;
                if (o.Flags != null) {
                    findFlags = from def in found.FlagDefinitions 
                                join flag in o.Flags.Split(',') on def.Tag.ToUpper() equals flag.ToUpper()
                                select def;
                    flagIds = from f in findFlags select f.Id;
                }

                if (o.Id == null && o.Title == null) {
                    if (o.StaticExport != null) {
                        await ExportHelper.StaticExportAsync(found, o.StaticExport);
                        return;
                    }

                    ReportingHelper.PrintDocuments(found, d => {
                        if (o.SecurityLevel.HasValue && o.SecurityLevel != d.Header.SecurityLevel)
                            return false;

                        if (flagIds != null && !flagIds.All(f => d.Header.Flags.Contains(f)))
                            return false;
                        
                        return true;
                    });

                    //ReportingHelper.PrintHighestCombinedVrel(found);
                    //ReportingHelper.PrintSecurityViolations(found, true, false);
                    return;
                }

                try {
                    DrxDocument doc;
                    if (o.Id != null) {
                        doc = found.GetDocument(o.Id.Value);
                    } else {
                        doc = found.GetDocuments().FirstOrDefault(d => d.Header.Title == o.Title);
                    }

                    if (doc == null) return;

                    await doc.LoadBodyAsync();
                    if (o.Dump) {
                        DebugHelper.DumpDocument(doc);
                        return;
                    }

                    var html = doc.GetPlainTextBodyAsType(DrxBodyType.Markdown);
                    Console.WriteLine(Encoding.UTF8.GetString(html));
                } catch (ArgumentException e) {
                    if (e.ParamName == "Serial") // hack lmao
                        Console.WriteLine("No valid certificate could be found to decrypt the document.");
                        return;

                    throw e;
                } catch (Exception e) {
                    Console.WriteLine($"Unable to load the document: {e.GetType()} ({e.Message})");
                    Console.WriteLine(e.StackTrace);
                    return;
                }
            });

            //Console.WriteLine("Displaying all stores.\n");

            //foreach (var store in cache.Stores) {
            //    Console.WriteLine($"{store.Name} (UUID {store.Id})");
            //}

            //ReportingHelper.PrintFlagAverages(personal);
            //return;

            //ReportingHelper.PrintDocuments(personal);
            //return;
        }
    }
}
