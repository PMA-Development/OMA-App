using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace OMA_App.Converters
{
    public class NullToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Check if the value is a Guid and whether it's Guid.Empty
            if (value is Guid guidValue)
            {
                return guidValue == Guid.Empty;
            }

            // For other types, check for null (optional for non-GUID cases)
            return value == null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("ConvertBack is not implemented for NullOrEmptyGuidToBooleanConverter.");
        }
    }
}
