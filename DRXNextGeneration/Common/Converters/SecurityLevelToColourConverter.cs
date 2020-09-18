using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Data;
using DRXLibrary.Models.Drx;

namespace DRXNextGeneration.Common.Converters
{
    public class SecurityLevelToColourConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return GuiConverterMappings.SecurityLevelToColourMappings[(DrxSecurityLevel) value];
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
