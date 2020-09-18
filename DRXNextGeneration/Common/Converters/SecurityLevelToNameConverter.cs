using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using DRXLibrary.Models.Drx;
using DRXLibrary.Models.Drx.Converters;

namespace DRXNextGeneration.Common.Converters
{
    public class SecurityLevelToNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return ConverterMappings.SecurityLevelToName[(DrxSecurityLevel) value];
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return ConverterMappings.SecurityLevelToName.First(s => s.Value == (string) value).Key;
        }
    }
}
