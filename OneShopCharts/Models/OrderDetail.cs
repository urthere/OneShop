using System;
using System.ComponentModel.DataAnnotations;

namespace OneShopCharts.Models
{
    public class OrderDetail
    {        
        [Key]
        public int DetailID { get; set; }
        public int OrderID { get; set; }
        public string ItemBarcode { get; set; }
        public decimal UnitPrice { get; set; }
        public int ItemCount { get; set; }
        public decimal Discount { get; set; }
        public decimal DetailPrice { get; set; }
        public bool IsValid { get; set; }
        public DateTime DatailDate { get; set; }
        public string Remarks { get; set; }
    }
}
    