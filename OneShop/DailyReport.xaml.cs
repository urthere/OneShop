using OneShop.Model;
using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace OneShop
{
    /// <summary>
    /// Interaction logic for DailyReport.xaml
    /// </summary>
    public partial class DailyReport : UserControl
    {
        private OrderViewModel orderViewModel;

        public DailyReport()
        {
            InitializeComponent();

            orderViewModel = new OrderViewModel(App.DBConnectionString);
            DataContext = orderViewModel;
            DrawAxis();
        }        

        private void DrawAxis()
        {
            var date = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            switch(date)
            {
                case 31:
                case 30:
                    DrawBigSmallMonths(122, 22, ConstantVariables.DaysOfMonth.BigSmallMonth);
                    break;
                case 28:
                case 29:
                default:
                    DrawBigSmallMonths(124, 24, ConstantVariables.DaysOfMonth.February);
                    break;
            }
        }

        private void DrawBigSmallMonths(int start, int step, ConstantVariables.DaysOfMonth daysOfMonth)
        {
            //for (int i = start; i <= (int)daysOfMonth; i += step)
            //{
            //    var line = new Line() { X1 = i, Y1 = 600, X2 = i, Y2 = 590, Stroke = new SolidColorBrush(Color.FromRgb(0, 0, 0)), StrokeEndLineCap = PenLineCap.Triangle };
            //    line.StrokeThickness = 3;
            //    var tb = new TextBlock() { Text = (((i - 100) / step) + 1).ToString() };
            //    Canvas.SetLeft(tb, i - 4);
            //    Canvas.SetTop(tb, 610);
            //    canChart.Children.Add(line);
            //    canChart.Children.Add(tb);
            //}
        }
    }
}
