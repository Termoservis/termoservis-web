using System.ComponentModel.DataAnnotations;

namespace Termoservis.Web.Models
{
	/// <summary>
	/// The change password view model.
	/// </summary>
	public class ChangePasswordViewModel
    {
		/// <summary>
		/// Gets or sets the old password.
		/// </summary>
		/// <value>
		/// The old password.
		/// </value>
		[Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

		/// <summary>
		/// Gets or sets the new password.
		/// </summary>
		/// <value>
		/// The new password.
		/// </value>
		[Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

		/// <summary>
		/// Gets or sets the confirmed password.
		/// </summary>
		/// <value>
		/// The confirmed password.
		/// </value>
		[DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}