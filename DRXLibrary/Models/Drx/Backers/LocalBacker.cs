using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DRXLibrary.Models.Drx;
using DRXLibrary.Models.Drx.Backers;

namespace DRXLibrary.Models.Drx.Backers
{
    public class LocalBacker : IBacker
    {
        private DirectoryInfo _storeFolder;
        private readonly Guid _folderId;
        public bool Loaded { get; private set; }

        public LocalBacker(Guid folderId)
        {
            _folderId = folderId;
        }

        public async Task LoadAsync()
        {
            if (Loaded) return;

            // Load the backer's folder
            var basePath = Path.Combine(Environment.GetEnvironmentVariable("HOME"), ".local/share/drx/stores");
            if (!Directory.Exists(basePath)) Directory.CreateDirectory(basePath);

            _storeFolder = new DirectoryInfo(Path.Combine(basePath, $"LocalDrxStore_{_folderId}"));
            if (!_storeFolder.Exists) _storeFolder.Create();

            Loaded = true;
        }

        public async Task DeleteAsync()
        {
            _storeFolder.Delete();
        }

        public async Task<IEnumerable<DrxDocument>> GetDocumentHeadersAsync()
        {
            var documents = new List<DrxDocument>();
            var files = _storeFolder.EnumerateFiles();
            foreach (var file in files.Where(f => f.Extension == ".drx"))
                using (var stream = file.OpenRead())
                    documents.Add(DrxParser.DeserialiseHeader(stream));
            return documents;
        }

        public async Task<DrxDocument> GetDocumentAsync(Guid id)
        {
            using (var stream = File.OpenRead(Path.Combine(_storeFolder.FullName, $"{id}.drx")))
                return DrxParser.Deserialise(stream);
        }

        public async Task SaveDocumentAsync(DrxDocument document)
        {
            using (var stream = File.OpenWrite(Path.Combine(_storeFolder.FullName, $"{document.Id}.drx"))) {
                stream.Seek(0, SeekOrigin.Begin);
                stream.SetLength(0);

                document.Serialise(stream);
            }
        }

        public async Task DeleteDocumentAsync(Guid id)
        {
            File.Delete(Path.Combine(_storeFolder.FullName, $"{id}.drx"));
        }
    }
}
