using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace metro
{
    class DimensionForeground : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string input = value as string;


            string[] datas = input.Split('/');
            int req = int.Parse(datas[0]);
            int gen = int.Parse(datas[1]);

            if (req != gen)
            {
                return "White";
            }
            else
            {
                return "Blue";
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
