using StockManagement.Core.Domain.Enums;

namespace StockManagement.Core.Application
{
    public class RegisterModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
