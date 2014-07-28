using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace metro
{
    class SCTPResentValidator : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string input = value as string;

            if (input != null)
            {
                string[] datas = input.Split('%');
                if (input != null && !input.Equals("N/A") && datas[0] != null && !datas[0].Equals(""))
                {
                    double req = double.Parse(datas[0]);

                    if (req < 1.5)
                    {
                        return "Red";
                    }
                    else
                    {
                        return "#FF003FE8";
                    }
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
