namespace StockManagement.Core.Domain.Entities
{
    public class StockTransactionItem : BaseEntity
    {
        public int StockTransactionId { get; set; }
        public StockTransaction StockTransaction { get; set; } = null!;

        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
