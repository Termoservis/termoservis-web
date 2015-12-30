using System.Threading.Tasks;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using Web.Areas.Portal.ViewModels.Portal;
using Web.Models;

namespace Web.Areas.Portal.Controllers
{
	/// <summary>
	/// Portal user management controller.
	/// </summary>
	[Area("Portal")]
	[Authorize]
    public class UsersController : Controller
	{
		private readonly IApplicationDbContext context;


		/// <summary>
		/// Initializes a new instance of the <see cref="UsersController"/> class.
		/// </summary>
		/// <param name="context">The context.</param>
		// ReSharper disable once SuggestBaseTypeForParameter
		public UsersController(ApplicationDbContext context)
		{
			this.context = context;
		}


		// GET: /portal/users/		
		/// <summary>
		/// Overview of portal users.
		/// </summary>
		/// <returns>Returns the portal users view.</returns>
		public async Task<IActionResult> Index()
		{
			// Retrieve current user
			var currentUser = await this.context.GetCurrentUserAsync(this);

			// Construct view model
			var model = new LayoutViewModel(currentUser);

			// Return view 
			return View(model);
		}
	}
}
