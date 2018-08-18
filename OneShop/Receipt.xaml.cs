using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using OneShop.Model;

namespace OneShop
{
    /// <summary>
    /// Interaction logic for Receipt.xaml
    /// </summary>
    public partial class Receipt : Window
    {
        public Receipt(StockViewModel stockViewModel)
        {
            InitializeComponent();

            this.pgpDatetime.Inlines.Add(new Run("日期：" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm")));
            this.pgpSerialNumber.Inlines.Add(new Run("流水号：" + stockViewModel.SerialNumber));
            var trg = new TableRowGroup();
            foreach (var item in stockViewModel.StockList)
            {
                var trName = new TableRow();
                trName.Cells.Add(new TableCell(new Paragraph(new Run(item.ItemName))) { ColumnSpan = 4, RowSpan = 2 });
                trg.Rows.Add(trName);
                trg.Rows.Add(new TableRow());

                var trProperties = new TableRow();
                trProperties.Cells.Add(new TableCell(new Paragraph(new Run(string.Empty))));
                trProperties.Cells.Add(new TableCell(new Paragraph(new Run(item.ItemCount.ToString()))));
                trProperties.Cells.Add(new TableCell(new Paragraph(new Run(item.ItemPrice.ToString()))));
                trProperties.Cells.Add(new TableCell(new Paragraph(new Run(item.DetailPrice.ToString()))));

                trg.Rows.Add(trProperties);
            }

            this.tbDetails.RowGroups.Add(trg);
            this.pgpTotal.Inlines.Add(new Run("合计：" + stockViewModel.OrderPrice.ToString("C") + "元"));


            BeginPrint();
        }

        /// <summary>
        /// 灰化图片
        /// </summary>
        private void GrayImage()
        {
            BitmapImage myBitmapImage = new BitmapImage();
            myBitmapImage.BeginInit();
            myBitmapImage.UriSource = new Uri(@"..\..\Images\879237133.jpg", UriKind.Relative);
            myBitmapImage.DecodePixelWidth = 180;
            myBitmapImage.EndInit();
            FormatConvertedBitmap newFormatedBitmapSource = new FormatConvertedBitmap();
            newFormatedBitmapSource.BeginInit();
            newFormatedBitmapSource.Source = myBitmapImage;
            newFormatedBitmapSource.DestinationFormat = PixelFormats.Gray32Float;
            newFormatedBitmapSource.EndInit();
            //imgQR.Width = 100;
            //imgQR.Height = 100;
            //set image source
            //imgQR.Source = newFormatedBitmapSource;
        }

        private void Window_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            e.Handled = true;
            if (System.Windows.Input.Key.Enter == e.Key)
            {
                BeginPrint();
                this.Close();
            }
            if (System.Windows.Input.Key.Escape == e.Key)
            {
                this.Close();
            }
        }

        private void BeginPrint()
        {
            var page = ((IDocumentPaginatorSource)fdReceipt).DocumentPaginator;
            page.PageSize = new Size(180, Properties.Settings.Default.PrintingHeight);
            new PrintDialog().PrintDocument(page, "ReceiptPrinting");
        }
    }
}
