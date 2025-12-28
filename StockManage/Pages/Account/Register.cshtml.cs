using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StockManagement.Core.Domain.Entities;
using StockManagement.Infrastructure.Persistence.Data;

namespace StockManagement.Presentation.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly StockDbConnect _context;

        public RegisterModel(StockDbConnect context)
        {
            _context = context;
        }

        [BindProperty]
        public RegisterInputModel Input { get; set; } = new();

        public string? ErrorMessage { get; set; }
        public string? SuccessMessage { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            //  already exists
            if (await _context.Users.AnyAsync(u => u.UserName == Input.Username))
            {
                ErrorMessage = "Username already exists!";
                return Page();
            }

            var user = new User
            {
                UserName = Input.Username,
                Password = Input.Password, 
                Role = Input.Role
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            SuccessMessage = "Registration successful!";
            return Page();
        }
    }

    public class RegisterInputModel
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
