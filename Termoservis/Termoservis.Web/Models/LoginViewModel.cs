using System.ComponentModel.DataAnnotations;

namespace Termoservis.Web.Models
{
	/// <summary>
	/// The login view model.
	/// </summary>
	public class LoginViewModel
	{
		/// <summary>
		/// Gets or sets the email.
		/// </summary>
		/// <value>
		/// The email.
		/// </value>
		[Required]
		[Display(Name = "Email")]
		[EmailAddress]
		public string Email { get; set; }

		/// <summary>
		/// Gets or sets the password.
		/// </summary>
		/// <value>
		/// The password.
		/// </value>
		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string Password { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether user login should be remembered.
		/// </summary>
		[Display(Name = "Remember me?")]
		public bool RememberMe { get; set; }
	}
}