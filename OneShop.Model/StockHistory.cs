//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OneShop.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class StockHistory
    {
        public int HisID { get; set; }
        public string ItemBarcode { get; set; }
        public string ItemName { get; set; }
        public Nullable<int> ItemCount { get; set; }
        public Nullable<decimal> ItemPrice { get; set; }
        public string HistoryBy { get; set; }
        public System.DateTime HistoryDate { get; set; }
    }
}
