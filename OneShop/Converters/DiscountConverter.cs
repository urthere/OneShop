using System;
using System.Globalization;
using System.Windows.Data;

namespace OneShop
{
    public class DiscountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((decimal)value)
            {
                case 0 : return "100%";
                case 1 : return "98%";
                case 2 :
                default :
                    return "95%";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
