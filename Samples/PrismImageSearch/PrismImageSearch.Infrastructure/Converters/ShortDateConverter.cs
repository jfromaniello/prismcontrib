using System;
using System.Windows.Data;

namespace PrismImageSearch.Infrastructure.Converters
{
    public class ShortDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return string.Format(culture, "{0:M-dd-yyyy}", value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
