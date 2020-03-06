using System;
using System.Collections.Generic;
using System.Text;

namespace CoreLibrary.Models.Crypto.Providers
{
    /// <summary>
    /// Defines a <see cref="ICryptoProvider"/> that is able to persist some information between sessions.
    /// </summary>
    public interface IStatefulProvider
    {
        object[] SavePersistentData();
        void LoadPersistentData(object[] data);
    }
}
