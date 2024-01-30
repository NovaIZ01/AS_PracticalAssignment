using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging; // Add this import for ILogger
using System;
using WebApplication3.Model;
using WebApplication3.ViewModels;

namespace WebApplication3.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IHttpContextAccessor contxt;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ILogger<LoginModel> logger; // Add ILogger

        public LoginModel(SignInManager<ApplicationUser> signInManager, IHttpContextAccessor httpContextAccessor, ILogger<LoginModel> logger)
        {
            contxt = httpContextAccessor;
            this.signInManager = signInManager;
            this.logger = logger;
        }

        [BindProperty]
        public Login LoginM { get; set; }

        public void OnGet()
        {
            logger.LogInformation("OnGet method called");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            logger.LogInformation("OnPostAsync method called");

            if (ModelState.IsValid)
            {
                var identityResult = await signInManager.PasswordSignInAsync(LoginM.Email, LoginM.Password, LoginM.RememberMe, false);
                if (identityResult.Succeeded)
                {
                    contxt.HttpContext.Session.SetString("isLoggedIn", LoginM.Email);
                    string GUID = Guid.NewGuid().ToString();
                    contxt.HttpContext.Session.SetString("AToken", GUID);
                    Response.Cookies.Append("AToken", GUID);

                    logger.LogInformation($"User {LoginM.Email} successfully logged in.");

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
                }
                else
                {
                    logger.LogError($"Login failed for user {LoginM.Email}. Username or Password incorrect.");
                    ModelState.AddModelError("", "Username or Password incorrect");
                }
            }
            else
            {
                logger.LogWarning("ModelState is not valid.");
            }

            return Page();
        }
    }
}
