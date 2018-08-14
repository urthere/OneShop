using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace OneShop
{
    public class BackgroundConverterByItemCount : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var count = (int)value;
            if (count <= 0)
            {
                return new SolidColorBrush(Colors.Red);
            }
            return new SolidColorBrush(Colors.White);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
