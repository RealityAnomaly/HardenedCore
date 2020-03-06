using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreLibrary.Models.Crypto;
using CoreLibrary.Models.Crypto.Providers;
using CoreLibrary.Utilities;
using DRXLibrary.Models.Drx.Converters;
using DRXLibrary.Models.Drx.Document;
using DRXLibrary.Models.Drx.Store;

namespace DRXLibrary.Models.Drx
{
    public class DrxDocument
    {
        // DRX Format
        // Descriptor (3 bytes, always DRX)
        // Id (16 bytes)
        // Version (4 bytes)
        // MessagePack header length (8 bytes)
        // Body length (8 bytes)
        // CRC32 checksum (4 bytes)
        // JSON header
        // encryption header
        // Body bytes
        public const string Descriptor = "DRX";

        // Header
        public Guid Id = Guid.NewGuid();

        // JSON header
        public DrxDocumentHeader Header = new DrxDocumentHeader();

        // Binary body
        /// <summary>
        /// Stores the body of the document in
        /// plaintext or encrypted form.
        /// </summary>
        public byte[] Body = new byte[0];

        // Non-saved values below
        public IDrxStore Store { get; set; }

        /// <summary>
        /// Stores the body of the document in plaintext form.
        /// <see cref="DecryptBodyBytes"/> must be called to decrypt and load this.
        /// </summary>
        public byte[] PlainTextBody { get; set; }

        // Wrapper methods for the store
        // LoadBodyAsync loads the body into memory from the filesystem. It does NOT decrypt it or do anything else with it.
        public async Task LoadBodyAsync() => await Store.LoadDocumentBodyAsync(this);
        public async Task SaveAsync() => await Store.SaveDocumentAsync(this);
        public async Task DeleteAsync() => await Store.DeleteDocumentAsync(this);

        /// <summary>
        /// Moves the document to a different store.
        /// </summary>
        public async Task MoveAsync(IDrxStore destination)
        {
            await DeleteAsync();
            Store = destination;
            await SaveAsync();
        }

        /// <summary>
        /// Decrypts the body into the <see cref="PlainTextBody"/> property.
        /// This must be called even if the body is not actually encrypted.
        /// </summary>
        public void DecryptBodyBytes()
        {
            // No encryption, nothing to do
            if (!Header.Encrypted || Store.Key == null)
            {
                PlainTextBody = Body;
                return;
            }

            // Initialise the document's key.
            var keyInit = (IInitiableProvider)Header.Key.PrimaryProtector.Provider;
            keyInit.Initialise(Store.Key);

            // Decrypt the document.
            PlainTextBody = new AesCryptoProvider(Header.Key).Decrypt(Body);
        }

        /// <summary>
        /// Encrypts the body from the <see cref="PlainTextBody"/> property.
        /// This only needs to be called if the body is being saved back.
        /// </summary>
        public void EncryptBodyBytes()
        {
            // No encryption, nothing to do
            if (!Header.Encrypted || Store.Key == null)
            {
                Body = PlainTextBody;
                return;
            }

            // If no crypto key or master key has changed, create a new one
            if (Header.Key == null)
                Header.Key = new CryptoKey(new CryptoKeyProtector("AES", EncryptionUtilities.GenerateRandomBytes(), Store.Key));

            // Initialise the document's key.
            var keyInit = (IInitiableProvider)Header.Key.PrimaryProtector.Provider;
            keyInit.Initialise(Store.Key);

            // Encrypt the document with its key
            Body = new AesCryptoProvider(Header.Key).Encrypt(PlainTextBody);
        }

        /// <summary>
        /// Converts the body encoding type to the specified type.
        /// </summary>
        public byte[] GetPlainTextBodyAsType(DrxBodyType destinationType)
        {
            // Make sure we're loaded
            if (PlainTextBody == null)
                DecryptBodyBytes();

            // No need to convert anything if source and dest are the same
            if (Header.BodyType == destinationType)
                return PlainTextBody;

            // Maps converters to conversion methods.
            var mapper = new Dictionary<(DrxBodyType source, DrxBodyType dest), Func<DrxBodyType, byte[]>>
            {
                {(DrxBodyType.Rtf, DrxBodyType.PlainText), type => RtfConverters.ToPlainText(PlainTextBody)},
                {(DrxBodyType.Rtf, DrxBodyType.Html), type => RtfConverters.ToHtml(PlainTextBody)},
                {(DrxBodyType.Rtf, DrxBodyType.Markdown), type => RtfConverters.ToMarkdown(PlainTextBody)}
            };

            var converterTuple = (Header.BodyType, destinationType);
            if (!mapper.ContainsKey(converterTuple))
                throw new ArgumentException("The type of conversion is not supported.", nameof(destinationType));
            return mapper[converterTuple](Header.BodyType);
        }

        public override string ToString()
        {
            return $"{Header.Title} [{Header.SecurityLevel}]";
        }
    }
}
