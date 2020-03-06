using System;
using System.Collections.Generic;
using CoreLibrary.Common.CoreLibrary.Common;
using CoreLibrary.Models.Crypto;
using MessagePack;

namespace DRXLibrary.Models.Drx.Document
{
    [MessagePackObject(true)]
    public class DrxDocumentHeader : BindableBase
    {
        /// <summary>
        /// Title of the document.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Time when this document was created.
        /// </summary>
        public DateTimeOffset TimeStamp { get; set; } = DateTimeOffset.Now;

        /// <summary>
        /// The document's VREL score.
        /// </summary>
        public DrxDocumentVrel Vrel { get; set; } = new DrxDocumentVrel();

        /// <summary>
        /// Flags in the document.
        /// </summary>
        public List<Guid> Flags { get; set; } = new List<Guid>();

        /// <summary>
        /// Holds a reference to the store which holds this document.
        /// If this is null, the document is not in a store.
        /// </summary>
        public Guid Store { get; set; }

        // Operational data
        public bool Encrypted { get; set; }

        /// <summary>
        /// Contains document encryption properties.
        /// This can only be set if a valid encryption provider is set on the store.
        /// </summary>
        public CryptoKey Key { get; set; }

        /// <summary>
        /// Security level of the document.
        /// The security level can enforce encryption.
        /// </summary>
        public DrxSecurityLevel SecurityLevel { get; set; }

        /// <summary>
        /// Body encoding, after decryption.
        /// </summary>
        public DrxBodyType BodyType { get; set; }
    }
}
