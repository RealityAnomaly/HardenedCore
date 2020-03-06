using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DRXLibrary.Models.Drx.Backers
{
    /// <summary>
    /// Defines a method of retrieval for one or more documents
    /// from a local or remote repository.
    /// </summary>
    public interface IBacker
    {
        bool Loaded { get; }

        /// <summary>
        /// Loads the backer. This can do things such as create or load the containing folder or store on the remote server.
        /// </summary>
        Task LoadAsync();

        /// <summary>
        /// Fully deletes the store from the backing medium.
        /// </summary>
        Task DeleteAsync();

        /// <summary>
        /// Retrieves an <see cref="IEnumerable{DrxDocument}"/> of document objects with loaded headers.
        /// </summary>
        Task<IEnumerable<DrxDocument>> GetDocumentHeadersAsync();

        /// <summary>
        /// Loads the body of an individual document into memory.
        /// </summary>
        Task<DrxDocument> GetDocumentAsync(Guid id);

        /// <summary>
        /// Saves a document to the backing medium.
        /// </summary>
        Task SaveDocumentAsync(DrxDocument document);

        /// <summary>
        /// Deletes a document from the backing medium.
        /// </summary>
        Task DeleteDocumentAsync(Guid id);
    }
}
