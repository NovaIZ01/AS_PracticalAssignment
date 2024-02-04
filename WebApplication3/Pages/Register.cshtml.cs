using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Encodings.Web;
using WebApplication3.Model;
using WebApplication3.ViewModels;

namespace WebApplication3.Pages
{
	public class RegisterModel : PageModel
	{
		private readonly UserManager<ApplicationUser> userManage;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly IHttpContextAccessor _httpContextAccessor;

		[BindProperty]
		public Register RegisterM { get; set; }

		public RegisterModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IHttpContextAccessor httpContextAccessor)
		{
			userManage = userManager;
			_signInManager = signInManager;
			_httpContextAccessor = httpContextAccessor;
		}

		public void OnGet()
		{
		}

		[ValidateAntiForgeryToken]
		public async Task<IActionResult> OnPostAsync()
		{
			if (ModelState.IsValid)
			{
				var dataProtectionProvider = DataProtectionProvider.Create("Encrypt");
				var Protection = dataProtectionProvider.CreateProtector("Key");
				var httpContext = _httpContextAccessor.HttpContext;

				var Newuser = new ApplicationUser()
				{
					UserName = RegisterM.Email,
					Email = HtmlEncoder.Default.Encode(RegisterM.Email),
					FullName = HtmlEncoder.Default.Encode(Protection.Protect(RegisterM.FullName)),
					CreditCard = HtmlEncoder.Default.Encode(Protection.Protect(RegisterM.CreditCard)),
					Gender = RegisterM.Gender,
					MobileNumber = HtmlEncoder.Default.Encode(RegisterM.MobileNumber),
					DeliveryAddress = HtmlEncoder.Default.Encode(RegisterM.DeliveryAddress),
					AboutMe = HtmlEncoder.Default.Encode(RegisterM.AboutMe),
					Password = HtmlEncoder.Default.Encode(Protection.Protect(RegisterM.Password)),
					ConfirmPassword = HtmlEncoder.Default.Encode(RegisterM.ConfirmPassword)
				};

				var result = await userManage.CreateAsync(Newuser, RegisterM.Password);

				if (result.Succeeded)
				{
					await _signInManager.SignInAsync(Newuser, false);
                    HttpContext.Session.SetString("isLoggedIn", Newuser.Email);
                    httpContext.Session.SetString("Email", Newuser.Email);
					httpContext.Session.SetString("Password", Newuser.ConfirmPassword);
                    string GUID = Guid.NewGuid().ToString();
                    HttpContext.Session.SetString("AToken", GUID);
                    Response.Cookies.Append("AToken", GUID);
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
