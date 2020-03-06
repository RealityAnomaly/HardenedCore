using System;
using MessagePack;

namespace DRXLibrary.Models.Drx
{
    /// <summary>
    /// Flags store information about
    /// the type of content contained in a document.
    /// </summary>
    [MessagePackObject]
    public class DrxFlag
    {
        /// <summary>
        /// Unique identifier of the flag.
        /// </summary>
        [Key(0)]
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Tag of the flag. This is a short version of the name.
        /// e.g. A, IN, OOBE
        /// </summary>
        [Key(1)]
        public string Tag { get; set; } = string.Empty;

        /// <summary>
        /// Name of the flag.
        /// Example: Anomaly
        /// </summary>
        [Key(2)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Description - optional, provides a way
        /// to keep note of what exactly the flag specifies
        /// </summary>
        [Key(5)]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Colour of the flag, stored as a hexadecimal string.
        /// This is used for colour coding.
        /// </summary>
        [Key(3)]
        public DrxFlagColour Colour { get; set; } = DrxFlagColour.FromArgb(255, 255, 255, 255);

        /// <summary>
        /// Minimum security level the document
        /// must be to add this flag.
        /// </summary>
        [Key(4)]
        public DrxSecurityLevel SecurityLevel { get; set; } = DrxSecurityLevel.Unclassified;

        public override string ToString() => $"{Tag} ({Name})";
    }
}
