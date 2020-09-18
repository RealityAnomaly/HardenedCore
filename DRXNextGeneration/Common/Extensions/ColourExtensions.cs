using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using DRXLibrary.Models.Drx;

namespace DRXNextGeneration.Common.Extensions
{
    internal static class ColourExtensions
    {
        internal static Color FromDrxColour(DrxFlagColour colour)
            => Color.FromArgb(colour.A, colour.R, colour.G, colour.B);
        internal static DrxFlagColour ToDrxColour(this Color colour) 
            => DrxFlagColour.FromArgb(colour.A, colour.R, colour.G, colour.B);
    }
}
