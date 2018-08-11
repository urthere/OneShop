using OneShop.Model;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;

namespace OneShop
{
    public class TotalConverters : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var list = value as ObservableCollection<StockListModel>;
            var total = 0.0m;
            foreach (var item in list)
            {
                total += item.DetailPrice;
            }
            return total;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
