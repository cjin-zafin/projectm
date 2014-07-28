using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace metro
{
    class SaeValidator : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string input = value as string;

            if (input != null)
            {
                if (input != null && !input.Equals("N/A") && !input.Equals("PASS"))
                {
                    return "Red";
                }
                else
                {
                    return "Green";
                }
            }

            return "Green";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
