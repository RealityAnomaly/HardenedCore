using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreLibrary.Models.Crypto;
using CoreLibrary.Models.Crypto.Providers;
using CoreLibrary.Utilities;
using DRXLibrary;
using DRXLibrary.Models.Drx;
using DRXLibrary.Models.Drx.Store;

namespace DRXLibrary.Remote
{
    public class RemoteDrxStore : IDrxStore
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool Encrypted { get; set; }
        public CryptoKey Key { get; set; }
        public List<DrxFlag> FlagDefinitions { get; set; }

        public Task DeleteAsync()
        {
            throw new NotImplementedException();
        }

        public Task DeleteDocumentAsync(DrxDocument document)
        {
            throw new NotImplementedException();
        }

        public void DisableEncryption()
        {
            throw new NotImplementedException();
        }

        public void EnableEncryption(string protector, object parms = null)
        {
            throw new NotImplementedException();
        }

        public DrxDocument GetDocument(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DrxDocument> GetDocuments()
        {
            throw new NotImplementedException();
        }

        public Task LoadAsync()
        {
            throw new NotImplementedException();
        }

        public Task LoadDocumentBodyAsync(DrxDocument document)
        {
            throw new NotImplementedException();
        }

        public DrxFlag ResolveFlag(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task SaveDocumentAsync(DrxDocument document)
        {
            throw new NotImplementedException();
        }
    }
}