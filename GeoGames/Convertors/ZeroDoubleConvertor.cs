using System;
using System.Globalization;
using Xamarin.Forms;

namespace GeoGames.Convertors
{
    public class ZeroDoubleConvertor : IValueConverter
    {
        public ZeroDoubleConvertor()
        {
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double)
            {
                double d = (double)value;
                if (d > 0)
                {
                    if (!string.IsNullOrEmpty(parameter as string))
                    {
                        return d.ToString(parameter as string);

                    }
                    else
                    {
                        return d.ToString("0.0m");
                    }
                }else{
                    return "waiting";
                }
            }

            return string.Empty;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
