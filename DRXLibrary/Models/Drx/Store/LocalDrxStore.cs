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
using DRXLibrary.Models.Drx.Backers;
using MessagePack;

namespace DRXLibrary.Models.Drx.Store
{
    [MessagePackObject(true)]
    public class LocalDrxStore : IDrxStore, ICloneable
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public bool Encrypted { get; set; }
        public CryptoKey Key { get; set; }
        public List<DrxFlag> FlagDefinitions { get; set; } = new List<DrxFlag>();

        [IgnoreMember] private readonly IList<DrxDocument> _documents = new List<DrxDocument>();

        [IgnoreMember] private IBacker _backer = null;

        public async Task DeleteAsync()
        {
            await _backer.DeleteAsync();

            // Destroy the protector if we need to
            if (Key != null && Key is CryptoKey protectedKey && protectedKey.PrimaryProtector.Provider is IDestructibleProvider provider)
                provider.Destroy();
        }

        public async Task LoadAsync()
        {
            if (_backer == null) {
                _backer = new LocalBacker(Id);
                await _backer.LoadAsync();
            }

            _documents.Clear();
            foreach (var document in await _backer.GetDocumentHeadersAsync())
                await AddDocumentAsync(document);
        }

        /// <summary>
        /// Retrieves a full document, including the body, from the backing store.
        /// This populates its entry fully in Documents to avoid double loading.
        /// </summary>
        public async Task LoadDocumentBodyAsync(DrxDocument document)
        {
            var temp = await _backer.GetDocumentAsync(document.Id);
            document.Body = temp.Body;
        }

        public async Task SaveDocumentAsync(DrxDocument document) => await _backer.SaveDocumentAsync(document);

        public async Task DeleteDocumentAsync(DrxDocument document)
        {
            _documents.Remove(document);
            await _backer.DeleteDocumentAsync(document.Id);
        }

        /// <summary>
        /// Starts tracking changes for the specified document.
        /// </summary>
        protected async Task AddDocumentAsync(DrxDocument document)
        {
            // Set its store and store ID to this
            document.Store = this;
            document.Header.Store = Id;

            // Check if it already exists in the cache
            var existing = _documents.FirstOrDefault(d => d.Id == document.Id);
            if (existing == null)
            {
                _documents.Add(document);
                return;
            }

            // Replace the existing cache document if there
            var pos = _documents.IndexOf(existing);
            _documents.Remove(existing);
            _documents.Insert(pos, document);
        }

        public DrxDocument GetDocument(Guid id) => _documents.FirstOrDefault(d => d.Id == id);
        public IEnumerable<DrxDocument> GetDocuments() => _documents;

        public async Task BackupStoreAsync(Stream stream, bool exportDecrypted = false)
        {
            using (var zipStream = new GZipStream(stream, CompressionMode.Compress, true))
            {
                // Create a new store for backing up this one, and copy our properties over
                // We randomise the GUID to avoid conflicts
                var backupStore = (LocalDrxStore) Clone();
                if (backupStore.Encrypted && exportDecrypted)
                {
                    // Create and save the escrow key
                    backupStore.Key.Protectors.Add(
                        new CryptoKeyProtectorAssociation
                        {
                            Intent = CryptoKeyProtectorIntent.Escrow,
                            Protector = new CryptoKeyProtector("None", backupStore.Key.PrimaryProtector.GetKey())
                        });
                }

                using (var writer = new BinaryWriter(zipStream, Encoding.UTF8, true))
                {
                    // Serialise and write the store
                    var storeBytes = MessagePackSerializer.Serialize(backupStore);
                    writer.Write(storeBytes.Length);
                    writer.Write(storeBytes);
                }

                // Serialise and write all documents
                foreach (var document in _documents)
                {
                    await document.LoadBodyAsync();
                    document.Serialise(zipStream);
                }
            }
        }

        public async Task RestoreStoreAsync(Stream stream)
        {
            using (var zipStream = new GZipStream(stream, CompressionMode.Decompress, true))
            {
                using (var memStream = new MemoryStream())
                {
                    zipStream.CopyTo(memStream);
                    memStream.Seek(0, SeekOrigin.Begin);
                    using (var reader = new BinaryReader(memStream, Encoding.UTF8, true))
                    {
                        // Read and deserialise the store
                        var length = reader.ReadInt32();
                        var storeBytes = reader.ReadBytes(length);

                        // Deserialise object and copy over our properties
                        var tempStore = MessagePackSerializer.Deserialize<LocalDrxStore>(storeBytes);
                        Name = tempStore.Name + " (backup)";
                        Key = tempStore.Key;
                        Encrypted = tempStore.Encrypted;
                        FlagDefinitions = tempStore.FlagDefinitions;

                        await LoadAsync();
                    }

                    // TODO: For now, just remove the escrow key(s) from the store, we don't use them yet
                    Key.Protectors.RemoveAll(k => k.Intent == CryptoKeyProtectorIntent.Escrow);

                    // Read documents until there are none
                    // and save them all to the filesystem
                    do
                    {
                        var document = new DrxDocument();
                        document.Deserialise(memStream);
                        await SaveDocumentAsync(document);
                    } while (memStream.Position != memStream.Length);

                    // Load the store documents
                    await LoadAsync();
                }
            }
        }

        public void EnableEncryption(string protector, object parms = null)
        {
            if (Key != null)
            {
                // If the key's protector is the same, no reason to change it
                //if (protectedKey.ProtectorType == protectorType)
                //   return;

                // Set the new protector on the key
                Key.PrimaryProtector = new CryptoKeyProtector(protector, Key.PrimaryProtector.GetKey(), parms);
            }
            else
            {
                // Create a new encryption key with 4096 bits and the specified provider
                Key = new CryptoKey(new CryptoKeyProtector(protector, EncryptionUtilities.GenerateRandomBytes(), parms));
            }

            Encrypted = true;
        }

        public void DisableEncryption()
        {
            if (Key != null)
            {
                // Key is not protected anyway, so no need to change it
                if (Key.PrimaryProtector.ProtectorName == "None")
                {
                    Encrypted = false;
                    return;
                }

                // Key already exists, so to avoid decrypting every document,
                // we just unwrap the key, which makes the key plaintext
                Key.PrimaryProtector = new CryptoKeyProtector("None", Key.PrimaryProtector.ProtectorKey);
            }

            Encrypted = false;
        }

        public DrxFlag ResolveFlag(Guid id) => FlagDefinitions.FirstOrDefault(f => f.Id == id);

        public object Clone()
        {
            var store = (LocalDrxStore)Activator.CreateInstance(GetType());
            store.Id = Id;
            store.Name = Name;
            store.Encrypted = Encrypted;
            store.FlagDefinitions = FlagDefinitions;

            if (Key != null)
                store.Key = (CryptoKey)Key.Clone();

            return store;
        }

        public override string ToString() => Name;
    }
}
