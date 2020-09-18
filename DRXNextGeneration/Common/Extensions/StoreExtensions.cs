using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DRXLibrary.Models.Drx.Store;
using DRXNextGeneration.Models;

namespace DRXNextGeneration.Common.Extensions
{
    internal static class StoreExtensions
    {
        /// <summary>
        /// Registers the default local backer for use with the specified store.
        /// </summary>
        internal static void RegisterLocalBacker(this DrxStore store) => store.RegisterBacker(new LocalBacker(store.Id));
    }
}
