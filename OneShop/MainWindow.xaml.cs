using System;
using System.Windows;
using System.Windows.Controls;

namespace OneShop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    { 
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CreateWindow(typeof(NewStock), Properties.Settings.Default.StockIn);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            CreateWindow(typeof(StockQuery), Properties.Settings.Default.StockQuery);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            CreateWindow(typeof(OrderQuery), Properties.Settings.Default.OrderQuery);
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            CreateWindow(typeof(Sales), Properties.Settings.Default.Sales);
        }

        private void CreateWindow(Type type, string name)
        {
            var child = Activator.CreateInstance(type);
            Title = Properties.Settings.Default.MainTitle + " - " + name;
            gridWorkArea.Children.Clear();
            gridWorkArea.Children.Add(child as UserControl);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            CreateWindow(typeof(DailyReport), Properties.Settings.Default.DailyReport);
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            CreateWindow(typeof(Statistical), Properties.Settings.Default.Statistical);
        }
    }
}
