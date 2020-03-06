using System;
using System.Collections.Generic;
using System.Linq;
using DRXLibrary.Models.Drx;
using DRXLibrary.Models.Drx.Store;
using ConsoleTables;

namespace DRXUtility
{
    public static class ReportingHelper
    {
        private class DocumentOutputModel {
            public string Title { get; private set; }
            public DrxSecurityLevel SecurityLevel { get; private set; }
            public DateTimeOffset TimeStamp { get; private set; }

            public static DocumentOutputModel From(DrxDocument document) {
                return new DocumentOutputModel() {
                    Title = document.Header.Title,
                    SecurityLevel = document.Header.SecurityLevel,
                    TimeStamp = document.Header.TimeStamp
                };
            }
        }

        private class FlagOutputModel {
            public string Name { get; private set; }
            public int Uses { get; private set; }
            public string Tag { get; private set; }
            public string Description { get; private set; }
            public DrxSecurityLevel SecurityLevel { get; private set; }

            public static FlagOutputModel From(DrxFlag flag, int uses) {
                return new FlagOutputModel() {
                    Name = flag.Name,
                    Uses = uses,
                    Tag = flag.Tag,
                    SecurityLevel = flag.SecurityLevel,
                    Description = flag.Description
                };
            }
        }

        public static void PrintDocuments(IDrxStore store, Func<DrxDocument, bool> predictate) {
            var raw = store.GetDocuments().Where(predictate);
            var documents = from entry in raw orderby entry.Header.TimeStamp descending select entry;

            var output = new List<DocumentOutputModel>();
            foreach (var document in documents) {
                output.Add(DocumentOutputModel.From(document));
            }

            ConsoleTable
                .From<DocumentOutputModel>(output)
                .Write(Format.Default);
        }

        public static void PrintFlags(IDrxStore store, Func<DrxFlag, bool> predictate) {
            var raw = store.FlagDefinitions.Where(predictate);
            var flags = from entry in raw orderby entry.Tag ascending select entry;

            var documents = store.GetDocuments();

            var output = new List<FlagOutputModel>();
            foreach (var flag in flags) {
                var total = (from document in documents where document.Header.Flags.Contains(flag.Id) select document).Count(); 
                output.Add(FlagOutputModel.From(flag, total));
            }

            ConsoleTable
                .From<FlagOutputModel>(output)
                .Write(Format.Default);
        }

        public static void PrintUnresolvableFlags(IDrxStore store) {
            foreach (var document in store.GetDocuments()) {
                foreach (var flag in document.Header.Flags) {
                    var resolved = store.ResolveFlag(flag);
                    if (resolved == null) {
                        Console.WriteLine($"Warning: cannot resolve Flag ID {flag}");
                        continue;
                    }
                }
            }
        }

        public static void PrintHighestCombinedVrel(IDrxStore store) {
            var list = new List<(DrxDocument, double)>();

            foreach (var document in store.GetDocuments()) {
                var vrel = document.Header.Vrel;
                var total = vrel.Vividity + vrel.Remembrance + vrel.Emotion + vrel.Length;

                list.Add((document, total));
            }

            foreach (var item in list.OrderByDescending(x => x.Item2).ToList()) {
                Console.WriteLine($"{item.Item2} {item.Item1.ToString()} {(item.Item1.Id)}");
            }
        }

        public static async void PrintSecurityViolations(IDrxStore store, bool correct = false, bool verbose = false) {
            foreach (var document in store.GetDocuments()) {
                var violations = new List<string>();

                foreach (var flag in document.Header.Flags) {
                    var resolved = store.ResolveFlag(flag);
                    if (resolved == null) {
                        if (verbose) Console.WriteLine($"Warning: cannot resolve Flag ID {flag}");
                        continue;
                    }

                    if (resolved.SecurityLevel > document.Header.SecurityLevel) {
                        if (correct && document.Header.SecurityLevel < resolved.SecurityLevel)
                            document.Header.SecurityLevel = resolved.SecurityLevel;

                        violations.Add($"- flag violation: {resolved.Tag} {resolved.Name} **{resolved.SecurityLevel}");
                    }
                }

                await document.LoadBodyAsync();
                document.DecryptBodyBytes();

                if (document.Header.SecurityLevel >= DrxSecurityLevel.Secret && !document.Header.Encrypted) {
                    if (correct) {
                        document.Header.Encrypted = true;
                    }

                    violations.Add("- document should be encrypted");
                }

                if (violations.Count <= 0) continue;
                if (correct) {
                    document.EncryptBodyBytes();
                    await document.SaveAsync();
                }

                Console.WriteLine($"{document.ToString()} ({document.Id}) is not compliant: ");
                foreach (var violation in violations)
                    Console.WriteLine(violation);
                
                Console.WriteLine();
            }
        }

        public static void PrintFlagAverages(IDrxStore store) {
            var flags = new Dictionary<Guid, int>();

            foreach (var document in store.GetDocuments()) {
                foreach (var flag in document.Header.Flags) {
                    if (!flags.ContainsKey(flag))
                        flags[flag] = 0;

                    flags[flag] += 1;
                }
            }

            var sorted = from entry in flags orderby entry.Value descending select entry;
            foreach (var flag in sorted) {
                var resolved = store.ResolveFlag(flag.Key);
                if (resolved == null) {
                    Console.WriteLine($"???? - {flag.Value}");
                    continue;
                }

                Console.WriteLine($"{resolved?.Tag} {resolved?.Name} - {flag.Value}");
            }
        }
    }
}