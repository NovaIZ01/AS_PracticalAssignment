using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using WebApplication3.Model;

namespace WebApplication3.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IHttpContextAccessor contxt;
		private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<LoginModel> logger; 

        public IndexModel(IHttpContextAccessor httpContextAccessor, ILogger<LoginModel> logger, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            contxt = httpContextAccessor;
            this.signInManager = signInManager;
            this.logger = logger;
            this.userManager = userManager;
        }

        public string Email { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }
        public string MobileNumber { get; set; }
        public string CreditCard { get; set; }
        public string DeliveryAddress { get; set; }
        public string AboutMe { get; set; }

        [ValidateAntiForgeryToken]

		public async Task<IActionResult> OnGet()
		{
            var dataProtectionProvider = DataProtectionProvider.Create("Encrypt");
            var Protection = dataProtectionProvider.CreateProtector("Key");
            string email = contxt.HttpContext.Session.GetString("isLoggedIn");
			string password = contxt.HttpContext.Session.GetString("Password");

            // Check if session has timed out
            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
            {
                var user = await userManager.FindByEmailAsync(email);

                if (user != null)
                {
                    Email = user.Email;
                    FullName = user.FullName;
                    Gender = user.Gender;
                    MobileNumber = user.MobileNumber;
                    DeliveryAddress = user.DeliveryAddress;
                    CreditCard = Protection.Unprotect(user.CreditCard);
                    AboutMe = user.AboutMe;

                    return Page();
                }
            }

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
			{

				email = null;
				password = null;

				await signInManager.SignOutAsync();
				contxt.HttpContext.Session.Remove("isLoggedIn");
				contxt.HttpContext.Session.Remove("Password");
				Response.Cookies.Delete("AToken");

				return RedirectToPage("Login");
			}

			ViewData["Email"] = email;
			ViewData["Password"] = password;

			return Page();
		}
	}
}
