using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace WebApplication3.ViewModels
{
    public class Register
    {
		[Required]
		[DataType(DataType.Text)]
        public string FullName { get; set; }

		[Required]
		[DataType(DataType.CreditCard)]
        [RegularExpression("[0-9]{16,16}",
            ErrorMessage = "Enter a valid Credit card number without any spacing or -"
            )]
        public string CreditCard { get; set; }

		[Required]
		public string Gender { get; set; }

		[Required]
		[DataType(DataType.PhoneNumber)]
		[RegularExpression("[0-9]{8,8}",
			ErrorMessage = "Enter a valid Phone Number that is 8 numbers long"
			)]
		
		public string MobileNumber { get; set; }

		[Required]
		public string DeliveryAddress { get; set; }

		[Required]
		[RegularExpression("^[\\w\\-\\.]+@([\\w-]+\\.)+[\\w-]{2,}$",
			ErrorMessage = "Enter a Valid Email")]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }
		[Required]
        [DataType(DataType.Password)]

        [RegularExpression(@"^((?=\S*?[A-Z])(?=\S*?[a-z])(?=\S*?[0-9]).{8,})\S$", 
		ErrorMessage = "Password must be at least 12 characters long and contain at least one uppercase letter, one lowercase letter, one digit, and one special character.")]
        public string Password { get; set; }
		[Required]
		[DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Password and confirmation password does not match")]
        public string ConfirmPassword { get; set; }

		//[Required]
  //      [DataType(DataType.Upload)]
  //      public IFormFile? ImageJPG { get; set; }
		[Required]
		public string AboutMe { get; set; }

	}
}
