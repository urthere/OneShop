﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class OneShopEntities : DbContext
    {
        public OneShopEntities()
            : base("name=OneShopEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Stock> Stocks { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Discount> Discounts { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<StockHistory> StockHistories { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
    }
}
