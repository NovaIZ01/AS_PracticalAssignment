using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication3.Model;
using WebApplication3.ViewModels;

namespace WebApplication3.Pages
{
    public class RegisterModel : PageModel
    {

        private UserManager<ApplicationUser> userManager { get; }
        private SignInManager<ApplicationUser> signInManager { get; }

        [BindProperty]
        public Register RegisterM { get; set; }

        public RegisterModel(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }



        public void OnGet()
        {
        }


        public async Task<IActionResult> OnPostAsync()
        {   
            if (ModelState.IsValid)
            {
                var dataProtectionProvider =DataProtectionProvider.Create("EncryptData");
                var protector = dataProtectionProvider.CreateProtector("MySecretKey");
                var user = new ApplicationUser()
                {   
                    UserName = RegisterM.Email,
                    Email = RegisterM.Email,
                    FullName = RegisterM.FullName,
                    CreditCard = protector.Protect(RegisterM.CreditCard),
                    Gender = RegisterM.Gender,
                    MobileNumber = RegisterM.MobileNumber,
                    Password = protector.Protect(RegisterM.Password),
                    ConfirmPassword = protector.Protect(RegisterM.ConfirmPassword),
                    DeliveryAddress = RegisterM.DeliveryAddress,
                    AboutMe = RegisterM.AboutMe

                };
                var result = await userManager.CreateAsync(user, RegisterM.Password);
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, false);
                    
                    return RedirectToPage("Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return Page();
        }







    }
}
