using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using Microsoft.Data.Entity;
using Web.Areas.Portal.ViewModels.Portal;
using Web.Models;

namespace Web.Areas.Portal.Controllers
{
	/// <summary>
	/// Portal home controller.
	/// </summary>
	[Area("Portal")]
	[Authorize]
	public class HomeController : Controller
	{
		private readonly IApplicationDbContext context;


		/// <summary>
		/// Initializes a new instance of the <see cref="HomeController"/> class.
		/// </summary>
		/// <param name="context">The context.</param>
		// ReSharper disable once SuggestBaseTypeForParameter
		public HomeController(ApplicationDbContext context)
		{
			this.context = context;
		}


		// GET: /portal/home/		
		/// <summary>
		/// Overview of portal.
		/// </summary>
		/// <returns>Returns the portal home view.</returns>
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
