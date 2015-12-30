using Web.Models;

namespace Web.Areas.Portal.ViewModels.Portal
{
	/// <summary>
	/// The layout page view model.
	/// </summary>
	public class LayoutViewModel
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="LayoutViewModel"/> class.
		/// </summary>
		public LayoutViewModel()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="LayoutViewModel"/> class.
		/// </summary>
		/// <param name="currentUser">The current user.</param>
		public LayoutViewModel(ApplicationUser currentUser)
		{
			this.User = currentUser;
		}

		/// <summary>
		/// Gets or sets the user.
		/// </summary>
		/// <value>
		/// The user.
		/// </value>
		public ApplicationUser User { get; set; }
    }
}
