using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StockManagement.Infrastructure.Persistence.Data;
using System.Security.Claims;

namespace StockManagement.Presentation.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly StockDbConnect _context;

        public LoginModel(StockDbConnect context)
        {
            _context = context;
        }

        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u =>
                    u.UserName == Username &&
                    u.Password == Password);

            if (user == null)
            {
                ErrorMessage = "Invalid username or password";
                return Page();
            }

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim(ClaimTypes.Role, user.Role)
    };

            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal);


            return user.Role switch
            {
                "Viewer" => RedirectToPage("/Products/Index"),
                "Admin" => RedirectToPage("/Dashboard/Index"),
                "StockManager" => RedirectToPage("/Dashboard/Index"),
                _ => RedirectToPage("/Products/Index")
            };
        }

    }
}
