using System;
using System.Collections.Generic;
using System.Text;

namespace CoreLibrary.Models.Crypto.Providers
{
    /// <summary>
    /// Implement this interface if you want to perform some kind of cleanup action
    /// when the user switches away from this provider (deleting stored keys, etc)
    /// </summary>
    public interface IDestructibleProvider
    {
        void Destroy();
    }
}
