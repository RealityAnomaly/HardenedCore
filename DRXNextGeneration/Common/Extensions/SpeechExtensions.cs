using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.SpeechRecognition;

namespace DRXNextGeneration.Common.Extensions
{
    internal static class SpeechExtensions
    {
        internal static string SemanticInterpretation(this SpeechRecognitionResult result, string interpretationKey)
        {
            return result.SemanticInterpretation.Properties[interpretationKey].FirstOrDefault();
        }
    }
}
