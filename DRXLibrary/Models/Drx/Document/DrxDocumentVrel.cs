using MessagePack;

namespace DRXLibrary.Models.Drx.Document
{
    /// <summary>
    /// Class that holds the "patented" VREL score.
    /// </summary>
    [MessagePackObject]
    public class DrxDocumentVrel
    {
        [Key(0)]
        public double Vividity { get; set; }
        [Key(1)]
        public double Remembrance { get; set; }
        [Key(2)]
        public double Emotion { get; set; }
        [Key(3)]
        public double Length { get; set; }

        public override string ToString() {
            return $"{Vividity}-{Remembrance}-{Emotion}-{Length}";
        }
    }
}
