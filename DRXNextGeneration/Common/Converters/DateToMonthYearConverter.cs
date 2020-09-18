using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace DRXNextGeneration.Common.Converters
{
    public class DateToMonthYearConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var dateTime = (DateTime) value;
            return $"{CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(dateTime.Month)} {dateTime.Year}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
