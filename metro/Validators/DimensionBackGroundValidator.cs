using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace metro
{
    class DimensionBackGroundValidator : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string input = value as string;


            string[] datas = input.Split('/');
            if (datas[0]!= null && datas[1] != null && !datas[0].Equals("") && !datas[0].Equals(""))
            {
                int req = int.Parse(datas[0]);
                int gen = int.Parse(datas[1]);

                if (req != gen)
                {
                    return "Red";
                }
                else
                {
                    return "White";
                }
            }

            return "White";

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
