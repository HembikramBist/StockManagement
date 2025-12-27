using Microsoft.EntityFrameworkCore;
using StockManagement.Core.Domain.Entities;

namespace StockManagement.Infrastructure.Persistence.Data
{
    public class StockDbConnect : DbContext
    {
        public StockDbConnect(DbContextOptions options) 
            : base(options)
        {
        }

        public DbSet<Product> Products {  get; set; }
        public DbSet<StockTransaction> StockTransactions { get; set; }
        public DbSet<StockTransactionItem> StockTransactionItems { get; set; }
        public DbSet<InventoryTransaction> InventoryTransactions { get; set; }

        public DbSet<User> Users { get; set; }
    }
}
