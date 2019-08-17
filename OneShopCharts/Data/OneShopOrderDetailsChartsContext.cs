using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace OneShopCharts.Models
{
    public class OneShopOrderDetailsChartsContext : DbContext
    {
        public OneShopOrderDetailsChartsContext (DbContextOptions<OneShopOrderDetailsChartsContext> options)
            : base(options)
        {
        }

        public DbSet<OneShopCharts.Models.OrderDetail> OrderDetail { get; set; }
    }
}
