namespace StockManagement.Core.Domain.Entities
{
    public class StockTransaction : BaseEntity
    {
        public string UserName { get; set; } = null!;
        public int TotalQuantity { get; set; }
        public string ProductName { get; set; }
        public decimal GrandTotal { get; set; }
        public DateTime SoldAt { get; set; }

        public ICollection<StockTransactionItem> Items { get; set; } = new List<StockTransactionItem>();
    }
}
