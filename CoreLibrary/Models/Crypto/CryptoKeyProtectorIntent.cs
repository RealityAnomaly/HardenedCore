using System;
using System.Collections.Generic;
using System.Text;

namespace CoreLibrary.Models.Crypto
{
    /// <summary>
    /// Defines the intent, or use, for a key protector on a <see cref="CryptoKey"/>.
    /// </summary>
    public enum CryptoKeyProtectorIntent
    {
        /// <summary>
        /// Primary key protector on the key. Only one may be defined.
        /// Use for primary unlock methods, for example a smart card certificate.
        /// </summary>
        Primary,

        /// <summary>
        /// Secondary key protector.
        /// Use for alternate unlock methods, for example a server holding a key.
        /// </summary>
        Secondary,

        /// <summary>
        /// Escrow key protector.
        /// Use for protectors such as recovery keys.
        /// </summary>
        Escrow
    }
}
