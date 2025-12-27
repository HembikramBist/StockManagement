using StockManagement.Core.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace StockManagement.Core.Domain.Entities
{
    public class User : BaseEntity
    {
        
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
       

    }
}
