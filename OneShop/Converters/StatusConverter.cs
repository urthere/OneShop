using System;
using System.Globalization;
using System.Windows.Data;

namespace OneShop
{
    class StatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (bool.Parse(value.ToString()))
            {
                return "有效";
            }
            return "无效";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
