using OMA_App.API;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMA_App.Converters
{
    public class LevelEnumToStringConverter : IValueConverter
    {
        //TODO: Converter has a problem when you escalate a task more than twice before changing page
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is LevelEnum level)
            {
                switch (level)
                {
                    case LevelEnum._1:
                        return "Level 1";
                    case LevelEnum._2:
                        return "Level 2";
                    case LevelEnum._3:
                        return "Level 3";
                    default:
                        return "Unknown Level";
                }
            }
            return "Invalid Level";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("LevelEnumToStringConverter does not support ConvertBack.");
        }
    }
}
