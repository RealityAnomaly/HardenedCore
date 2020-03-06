using System;
using System.Collections.Generic;
using System.Text;

namespace CoreLibrary.Models.Crypto.Providers
{
    /// <summary>
    /// Defines a <see cref="ICryptoProvider"/> that must be initialised with an object before use.
    /// </summary>
    public interface IInitiableProvider : ICryptoProvider
    {
        /// <summary>
        /// Initialises this <see cref="IInitiableProvider"/> instance with the specified object.
        /// </summary>
        void Initialise(object value);
    }
}
