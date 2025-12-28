using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace StockManagement.Presentation.Pages.Account
{
    public class LogoutModel : PageModel
    {
        public async Task<IActionResult> OnPostAsync()
        {
            //  Sign out (remove auth cookie)
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            //  Clear session (cart, role, etc.)
            HttpContext.Session.Clear();

            //  Redirect to login page
            return RedirectToPage("/Account/Login");
        }
    }
}
