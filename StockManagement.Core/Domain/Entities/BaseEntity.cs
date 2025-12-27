using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockManagement.Core.Domain.Entities
{
    public abstract class BaseEntity
    {
        [Key]  // Add this attribute
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]  // Add this
        public int id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}