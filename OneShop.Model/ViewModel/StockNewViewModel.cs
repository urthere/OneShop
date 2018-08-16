﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Input;

namespace OneShop.Model
{
    public class StockNewViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private OneShopEntities entity;
        private Stock Stock;
        private ObservableCollection<Stock> stocks;
        private bool AddOrEditFlag;
        private int newCount = 0;
        private int totalPage = 0;

        public StockNewViewModel(string conStr, string barcode = "")
        {
            entity = new OneShopEntities();
            entity.Database.Connection.ConnectionString = conStr;
            Stock = new Stock();
            stocks = new ObservableCollection<Stock>();
            AddNewStockCommand = new DelegateCommand(this.AddOrEditStocks, this.IsExistsStock);
        }

        public StockNewViewModel(string conStr)
        {
            entity = new OneShopEntities();
            entity.Database.Connection.ConnectionString = conStr;
            QueryStockListCommand = new DelegateCommand(this.QueryStockListPage, this.IsExistsStock);
            stocks = new ObservableCollection<Stock>();
            this.PageSize = 20000;
        }

        public ICommand AddNewStockCommand { get; set; }
        public ICommand QueryStockListCommand { get; set; }
        public ICommand SaveStockCommand { get; set; }
        public ICommand NextPageCommand { get; set; }
        public ICommand PreviousPageCommand { get; set; }

        private void QueryStockListPage(object page)
        {            
            Expression<Func<Stock, bool>> a;
            if (string.IsNullOrEmpty(page.ToString()))
            {
                a = (s) => 1 == 1;
            }
            else
            {
                a = s => s.ItemBarcode.Equals(page.ToString());
            }

            if (PageIndex * PageSize <= this.totalPage)
            {
                var list = Common<Stock>.LoadPageItems(
                    this.PageSize,
                    this.PageIndex == 0 ? 1 : this.PageIndex,
                    out totalPage,
                    a,
                    p => p.ModDate,
                    true,
                    this.entity).ToList();
                if (list.Count > 0)
                {
                    this.stocks.Clear();
                    this.TotalPrice = 0;
                    list.ForEach(p => { this.stocks.Add(p); this.TotalPrice += (int)p.ItemCount * (decimal)p.ItemPrice; });
                    this.RaisePropertyChanged("Stocks");
                    this.RaisePropertyChanged("TotalPrice");
                }                
            }
        }

        private void NextPage(object page)
        {
            this.PageIndex += 1;
            if (PageIndex * PageSize <= this.totalPage)
            {
                this.QueryStockListPage(null);
            }
        }

        private void PreviousPage(object page)
        {
            this.PageIndex -= 1;
            if (this.PageIndex > 0)
            {
                this.QueryStockListPage(null);
            }
        }

        public Stock StockInfo
        {
            get => this.Stock;
            set
            {
                this.Stock = value;
                this.RaisePropertyChanged("StockInfo");
            }
        }

        public void SetStockInfo(Stock value)
        {
            this.Stock = value;            
        }

        public ObservableCollection<Stock> Stocks
        {
            get => this.stocks;
        }

        public string Barcode
        {
            get => Stock.ItemBarcode;
            set
            {
                Stock.ItemBarcode = value;
                this.RaisePropertyChanged("Barcode");
            }
        }

        public string Name
        {
            get => Stock.ItemName;
            set
            {
                Stock.ItemName = value;
                this.RaisePropertyChanged("Name");
            }
        }

        public decimal UnitPrice
        {
            get => (decimal)Stock.ItemPrice;
            set
            {
                if (decimal.TryParse(value.ToString(), out decimal i) && i >= 0)
                {
                    Stock.ItemPrice = value;
                    this.RaisePropertyChanged("UnitPrice");
                }
            }
        }

        public int NewCount
        {
            get => this.newCount;
            set => this.newCount = value;
        }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public int TotalPage { get => totalPage; set => totalPage = value; }
        public decimal TotalPrice { get; private set; } = 0.0m;

        public void GetStock(string barcode)
        {
            var stock = entity.Stocks.FirstOrDefault(x => x.ItemBarcode.Equals(barcode));
            if (this.AddOrEditFlag = null != stock ? true : false)
            {
                this.Stock = stock;
                this.RaisePropertyChanged("Barcode");
            }
            else
            {
                this.Stock = new Stock();
            }
            this.RaisePropertyChanged("Name");
            this.RaisePropertyChanged("UnitPrice");
            this.RaisePropertyChanged("Count");            
        }        

        public void AddOrEditStocks(object stock)
        {
            this.Stock.ModBy = "admin";
            this.Stock.ModDate = System.DateTime.Now;
            if (this.AddOrEditFlag)
            {
                this.Stock.ItemCount += this.newCount;
                entity.Entry(Stock).State = System.Data.Entity.EntityState.Modified;
            }
            else
            {
                this.Stock.StoredBy = "admin";
                this.Stock.StoredDate = System.DateTime.Now;
                entity.Stocks.Add(Stock);
            }
            SaveStockHistory();
            entity.SaveChanges();


            this.FindExistsStock();
            this.Stock = new Stock();
            this.RaisePropertyChanged("Barcode");
            this.RaisePropertyChanged("Name");
            this.RaisePropertyChanged("UnitPrice");
            this.newCount = 0;
            this.RaisePropertyChanged("NewCount");
        }

        private void SaveStockHistory()
        {
            entity.StockHistories.Add(new StockHistory()
            {
                ItemName = Stock.ItemName,
                ItemCount = this.newCount,
                ItemBarcode = Stock.ItemBarcode,
                ItemPrice = Stock.ItemPrice,
                HistoryBy = Stock.ModBy,
                HistoryDate = Stock.ModDate
            });
        }

        private void FindExistsStock()
        {
            var tmp = this.stocks.FirstOrDefault(x => this.Stock.ItemBarcode.Equals(x.ItemBarcode));
            if (null == tmp)
            {
                this.stocks.Add(this.Stock);
            }
            else
            {
                this.stocks.Remove(tmp);
                this.stocks.Add(this.Stock);
                RaisePropertyChanged("Count");
                RaisePropertyChanged("UnitPrice");
                RaisePropertyChanged("Name");
            }
        }

        public void EditStock(object para)
        {
            
            entity.SaveChanges();
        }

        public bool IsExistsStock(object para)
        {
            //if (null == Stock)
            //{
            //    return false;
            //}
            //return Stock.ItemCount >= 0 && Stock.ItemPrice >= 0 ? true : false;
            return true;
        }

        private void RaisePropertyChanged(string name)
        {
            var handler = this.PropertyChanged;
            if (null != handler)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}