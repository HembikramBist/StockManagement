namespace StockManagement.Core.Domain.Entities
{
    public class InventoryTransaction : BaseEntity
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string Type { get; set; } = null!; 
        public DateTime CreatedAt { get; set; }
    }
}
