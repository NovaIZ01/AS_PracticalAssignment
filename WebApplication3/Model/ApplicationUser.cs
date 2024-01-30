using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Model
{
	public class ApplicationUser : IdentityUser
	{

		public string FullName { get; set; }

		public string CreditCard { get; set; }

		public string Gender { get; set; }

		public string MobileNumber { get; set; }

		[DataType(DataType.Password)]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		public string ConfirmPassword { get; set; }

		public string DeliveryAddress { get; set; }

		public string AboutMe { get; set; }

	}
}
