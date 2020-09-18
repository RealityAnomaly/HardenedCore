using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using DRXLibrary.Models.Drx;
using DRXLibrary.Models.Drx.Backers;

namespace DRXNextGeneration.Models
{
    internal class LocalBacker : IBacker
    {
        private StorageFolder _storeFolder;
        private readonly Guid _folderId;

        public LocalBacker(Guid folderId)
        {
            _folderId = folderId;
        }

        public async Task LoadAsync()
        {
            // Load the backer's folder
            var baseFolder = await ApplicationData.Current.RoamingFolder
                .CreateFolderAsync("stores", CreationCollisionOption.OpenIfExists);
            _storeFolder = await baseFolder.CreateFolderAsync($"LocalDrxStore_{_folderId}",
                CreationCollisionOption.OpenIfExists);
        }

        public async Task DeleteAsync()
        {
            await _storeFolder.DeleteAsync();
        }

        public async Task<IEnumerable<DrxDocument>> GetDocumentHeadersAsync()
        {
            var documents = new List<DrxDocument>();
            var files = await _storeFolder.GetFilesAsync();
            foreach (var file in files.Where(f => Path.GetExtension(f.Path) == ".drx"))
                using (var stream = await file.OpenAsync(FileAccessMode.Read))
                    documents.Add(DrxParser.DeserialiseHeader(stream.AsStreamForRead()));
            return documents;
        }

        public async Task<DrxDocument> GetDocumentAsync(Guid id)
        {
            var file = await _storeFolder.GetFileAsync($"{id}.drx");
            using (var stream = await file.OpenAsync(FileAccessMode.Read))
                return DrxParser.Deserialise(stream.AsStreamForRead());
        }

        public async Task SaveDocumentAsync(DrxDocument document)
        {
            var file = await _storeFolder.CreateFileAsync($"{document.Id}.drx", CreationCollisionOption.ReplaceExisting);
            using (var stream = await file.OpenAsync(FileAccessMode.ReadWrite))
                document.Serialise(stream.AsStreamForWrite());
        }

        public async Task DeleteDocumentAsync(Guid id)
        {
            var file = await _storeFolder.GetFileAsync($"{id}.drx");
            await file.DeleteAsync();
        }
    }
}
