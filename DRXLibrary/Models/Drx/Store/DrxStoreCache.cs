using System.Collections.Generic;
using MessagePack;

namespace DRXLibrary.Models.Drx.Store
{
    /// <summary>
    /// Holds the local stores.
    /// </summary>
    [MessagePackObject(true)]
    public class DrxStoreCache
    {
        public List<LocalDrxStore> Stores { get; set; } = new List<LocalDrxStore>();
    }
}
