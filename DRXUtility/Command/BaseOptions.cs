using System;
using CommandLine;
using DRXLibrary.Models.Drx;

namespace DRXUtility.Commands {
    internal abstract class BaseOptions {
        [Option('s', "store", Required = false, HelpText = "Specifies the Store GUID to use.")]
        public Guid? Store { get; set; }

        [Option('i', "id", Required = false, HelpText = "Specifies the document ID to use.")]
        public Guid? Id { get; set; }

        [Option('t', "title", Required = false, HelpText = "Specifies the document title to use.")]
        public string Title { get; set; }

        [Option('f', "flags", Required = false, HelpText = "Flags to filter by.")]
        public string Flags { get; set; }

        [Option('v', "verbose", Required = false, Default = false)]
        public bool Verbose { get; set; }

        [Option("dump", Required = false, Default = false)]
        public bool Dump { get; set; }

        [Option("static-export", Required = false, HelpText = "Path to export to")]
        public string StaticExport { get; set; }

        [Option("show-flags", Required = false, Default = false)]
        public bool ShowFlags { get; set; }

        [Option("security-level", Required = false, HelpText = "Specifies a security level to filter by.")]
        public DrxSecurityLevel? SecurityLevel { get; set; }
    }
}
