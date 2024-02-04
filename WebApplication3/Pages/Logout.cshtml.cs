using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication3.Model;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Http;
namespace WebApplication3.Pages
{

    public class LogoutModel : PageModel

    {
        private readonly SignInManager<ApplicationUser> signInManager;

        private readonly IHttpContextAccessor contxt;
        public LogoutModel(IHttpContextAccessor httpContextAccessor, SignInManager<ApplicationUser> signInManager)
        {
            contxt = httpContextAccessor;
            this.signInManager = signInManager;
        }
        public void OnGet() { }
        public async Task<IActionResult> OnPostLogoutAsync()
        {
            
            await signInManager.SignOutAsync();
            contxt.HttpContext.Session.Clear();
            contxt.HttpContext.Session.Remove("Email");
            contxt.HttpContext.Session.Remove("Password");
            contxt.HttpContext.Session.Remove("SessionIdentifier");
            Response.Cookies.Delete("AToken");
            contxt.HttpContext.Session.Remove("IsLoggedIn");
            return RedirectToPage("Login");
        }
        public async Task<IActionResult> OnPostDontLogoutAsync()
        {
            return RedirectToPage("Index");
        }
    }
}
