using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRXNextGeneration.Views
{
    /// <summary>
    /// Defines a view that is able to accept externally passed commands.
    /// </summary>
    internal interface IViewCommandHandler
    {
        void HandleCommand(object command);
    }
}
