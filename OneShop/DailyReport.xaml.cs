﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using OneShop.Model;

namespace OneShop
{
    /// <summary>
    /// Interaction logic for DailyReport.xaml
    /// </summary>
    public partial class DailyReport : UserControl
    {

        public DailyReport()
        {
            InitializeComponent();

            this.DataContext = new OrderViewModel(App.DBConnectionString);
        }        

        private void DrawAxis()
        {

        }
    }
}
