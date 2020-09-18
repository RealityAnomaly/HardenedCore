using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DRXLibrary.Models.Drx;

namespace DRXNextGeneration.Services
{
    internal class FlagService : IFlagResolver
    {
        private IList<DrxFlag> _flags = new List<DrxFlag>();

        public DrxFlag ResolveFlag(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
