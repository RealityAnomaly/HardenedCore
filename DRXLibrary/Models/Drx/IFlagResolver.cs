using System;

namespace DRXLibrary.Models.Drx
{
    /// <summary>
    /// Defines the ability to resolve a complete
    /// flag from a flag unique identifier (<see cref="Guid"/>).
    /// </summary>
    public interface IFlagResolver
    {
        DrxFlag ResolveFlag(Guid id);
    }
}
