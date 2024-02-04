using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging; // Add this import for ILogger
using System;
using WebApplication3.Model;
using WebApplication3.ViewModels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication3.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IHttpContextAccessor contxt;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;

        private readonly ILogger<LoginModel> logger;
        public LoginModel(SignInManager<ApplicationUser> signInManager, IHttpContextAccessor httpContextAccessor, ILogger<LoginModel> logger, UserManager<ApplicationUser> userManager)
        {
            contxt = httpContextAccessor;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.logger = logger;
        }

        [BindProperty]
        public Login LoginM { get; set; }
        public Register RegisterM { get; set; }
        public void OnGet()
        {
            logger.LogInformation("OnGet method called");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            logger.LogInformation("OnPostAsync method called");
            
            if (ModelState.IsValid)
            {   
                var identityResult = await signInManager.PasswordSignInAsync(LoginM.Email, LoginM.Password, LoginM.RememberMe, true);
                if (identityResult.Succeeded)
                {
                    contxt.HttpContext.Session.SetString("isLoggedIn", LoginM.Email);
                    string GUID = Guid.NewGuid().ToString();
                    contxt.HttpContext.Session.SetString("AToken", GUID);
                    Response.Cookies.Append("AToken", GUID);
                    contxt.HttpContext.Session.SetString("Email", LoginM.Email);
                    contxt.HttpContext.Session.SetString("Password",LoginM.Password);

                    if (contxt.HttpContext.Session.GetString("isLoggedIn") != null && Request.Cookies["AToken"] != null)
                    {
                        if (contxt.HttpContext.Session.GetString("AToken").ToString().Equals(Request.Cookies["AToken"]))
                        {
                            logger.LogInformation("Redirecting to Login page due to AToken mismatch.");
                            Response.Redirect("Login", false);
                        }
                    }

                    logger.LogInformation("Redirecting to Index page.");
                    Response.Redirect("Index", false);
                    var user = await userManager.FindByEmailAsync(LoginM.Email);
                    if (user != null && await userManager.IsLockedOutAsync(user))
                    {
                        ModelState.AddModelError("LoginM.Email", "Account locked. Please try again later.");
                        return Page();
                    }
                    
                }
                else if (identityResult.IsLockedOut)
                    {
                        ModelState.AddModelError("LoginM.Email", "Account has been locked out");
                        ViewData["Error"] = "Account has been locked out";
                        return Page();
					}
                else
                {
					ViewData["Error"] = "Email or Password incorrect";
                    

				}
            }
            else
            {
                logger.LogWarning("ModelState is not valid.");
                ViewData["Error"] = "Please Enter a Email or Password";
            }

            return Page();
        }
    }
}
