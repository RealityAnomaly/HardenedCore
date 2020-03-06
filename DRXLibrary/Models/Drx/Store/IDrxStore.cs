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
    public interface IDrxStore : IFlagResolver {
        /// <summary>
        /// Id of this store instance.
        /// </summary>
        Guid Id { get; set; }

        /// <summary>
        /// Custom name of the store.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Whether this store supports encryption.
        /// </summary>
        bool Encrypted { get; set; }

        /// <summary>
        /// Defines the store's cryptographic provider.
        /// If this is null, no encryption is defined.
        /// </summary>
        CryptoKey Key { get; set; }

        /// <summary>
        /// Contains all store-specific flag definitions.
        /// </summary>
        List<DrxFlag> FlagDefinitions { get; set; }

        Task DeleteAsync();
        Task LoadAsync();

        /// <summary>
        /// Retrieves a full document, including the body, from the backing store.
        /// This populates its entry fully in Documents to avoid double loading.
        /// </summary>
        Task LoadDocumentBodyAsync(DrxDocument document);

        /// <summary>
        /// Saves the document to the backing store.
        /// </summary>
        Task SaveDocumentAsync(DrxDocument document);

        /// <summary>
        /// Deletes the document from the backing store.
        /// </summary>
        Task DeleteDocumentAsync(DrxDocument document);

        DrxDocument GetDocument(Guid id);

        IEnumerable<DrxDocument> GetDocuments();

        /// <summary>
        /// Enables, or sets, encryption on this store.
        /// This either generates a new key or changes the key protector.
        /// </summary>
        /// <param name="protector">Type of the <see cref="ICryptoProvider"/> to use.</param>
        /// <param name="parms">Object to initialise the <see cref="IInitiableProvider"/> with.</param>
        void EnableEncryption(string protector, object parms = null);

        /// <summary>
        /// Disables encryption on this store.
        /// </summary>
        void DisableEncryption();
    }
}