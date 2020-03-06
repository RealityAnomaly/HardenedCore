using System.Collections.Generic;

namespace DRXLibrary.Models.Drx.Converters
{
    public static class ConverterMappings
    {
        public static readonly IDictionary<DrxSecurityLevel, string> SecurityLevelToSecFlag = new Dictionary<DrxSecurityLevel, string>
        {
            { DrxSecurityLevel.Unclassified, "UCF" },
            { DrxSecurityLevel.Protected, "PRO" },
            { DrxSecurityLevel.Confidential, "CNF" },
            { DrxSecurityLevel.Secret, "SCT" },
            { DrxSecurityLevel.TopSecret, "TSCT" },
            { DrxSecurityLevel.BlackGold, "BG" },
            { DrxSecurityLevel.StormVault, "SV" },
            { DrxSecurityLevel.DitVault, "DIT" }
        };

        public static readonly IDictionary<DrxSecurityLevel, string> SecurityLevelToName = new Dictionary<DrxSecurityLevel, string>
        {
            { DrxSecurityLevel.Unclassified, "Unclassified (UCF)" },
            { DrxSecurityLevel.Protected, "Protected (PRO)" },
            { DrxSecurityLevel.Confidential, "Confidential (CNF)" },
            { DrxSecurityLevel.Secret, "Secret (SCT)" },
            { DrxSecurityLevel.TopSecret, "Top Secret (TSCT)" },
            { DrxSecurityLevel.BlackGold, "BlackGold (BG)" },
            { DrxSecurityLevel.StormVault, "StormVault (SV)" },
            { DrxSecurityLevel.DitVault, "DIT Vault (DIT)" }
        };
    }
}
