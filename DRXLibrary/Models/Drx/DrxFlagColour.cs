using MessagePack;

namespace DRXLibrary.Models.Drx
{
    [MessagePackObject]
    public class DrxFlagColour
    {
        public DrxFlagColour() { }

        public static DrxFlagColour FromArgb(byte a, byte r, byte g, byte b)
            => new DrxFlagColour { A = a, R = r, G = g, B = b };

        [Key(0)]
        public byte A { get; set; }
        [Key(1)]
        public byte R { get; set; }
        [Key(2)]
        public byte G { get; set; }
        [Key(3)]
        public byte B { get; set; }
    }
}
