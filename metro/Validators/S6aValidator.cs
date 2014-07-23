using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace metro
{
    class S6aValidator : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string input = value as string;


            string[] datas = input.Split('%');
            if (datas[0] != null && !datas[0].Equals(""))
            {
                double req = double.Parse(datas[0]);

                if (req < 98.0)
                {
                    return "Red";
                }
                else
                {
                    return "#FF003FE8";
                }
            }

            return "#FF003FE8";

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
