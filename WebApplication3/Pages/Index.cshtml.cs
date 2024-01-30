using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
namespace WebApplication3.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IHttpContextAccessor contxt;
        public IndexModel(IHttpContextAccessor httpContextAccessor)
        {
            contxt = httpContextAccessor;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(IFormCollection fc)
        {
            string res = fc["txtname"];
            return Page();
        }

        

        public IActionResult Index() {
            {
                contxt.HttpContext.Session.SetString("Email", "Tim");
                contxt.HttpContext.Session.SetString("Password", "Password");
                return Page();
            }    
        }
        public IActionResult Privacy()
        {
            string Email = contxt.HttpContext.Session.GetString("Email");
            string Password = contxt.HttpContext.Session.GetString("Password");
            return Page();
        }
        public void OnGet()
            {

            }
    }
}