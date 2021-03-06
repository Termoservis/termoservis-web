﻿using System.Web.Mvc;

namespace Termoservis.Web.Controllers
{
	/// <summary>
	/// The home controller.
	/// </summary>
	/// <seealso cref="System.Web.Mvc.Controller" />
	[Authorize]
#if !DEBUG
    [RequireHttps]
#endif
    public class HomeController : Controller
	{
		/// <summary>
		/// Index page.
		/// </summary>
		public ActionResult Index()
		{
			return this.View();
		}
	}
}
