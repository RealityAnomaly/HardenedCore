using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using DRXLibrary.Models.Drx;

namespace DRXNextGeneration.Common.Converters
{
    public static class GuiConverterMappings
    {
        public static readonly IDictionary<DrxSecurityLevel, Color> SecurityLevelToColourMappings = new Dictionary<DrxSecurityLevel, Color>()
        {
            { DrxSecurityLevel.Unclassified, Colors.DarkGray },
            { DrxSecurityLevel.Protected, Colors.DarkGreen },
            { DrxSecurityLevel.Confidential, Colors.LightSeaGreen },
            { DrxSecurityLevel.Secret, Colors.DarkOrange },
            { DrxSecurityLevel.TopSecret, Colors.OrangeRed },
            { DrxSecurityLevel.BlackGold, Colors.Red },
            { DrxSecurityLevel.StormVault, Colors.Black },
            { DrxSecurityLevel.DitVault, Colors.Black }
        };
    }
}
