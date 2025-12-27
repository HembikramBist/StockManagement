using System.ComponentModel.DataAnnotations.Schema;

namespace StockManagement.Core.Domain.Entities
{
    public  class Product : BaseEntity
    {
        [Column("Name")]
        public string ProductName { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
