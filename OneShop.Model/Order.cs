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
    
    public partial class Order
    {
        public int OrderID { get; set; }
        public Nullable<int> MemberID { get; set; }
        public Nullable<int> DiscountID { get; set; }
        public decimal OrderPrice { get; set; }
        public System.DateTime OrderDate { get; set; }
        public bool IsValid { get; set; }
        public System.DateTime ModBy { get; set; }
        public string Remarks { get; set; }
        public string SerialNumber { get; set; }
    }
}
