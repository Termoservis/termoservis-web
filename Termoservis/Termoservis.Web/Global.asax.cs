using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Termoservis.Web
{
	/// <summary>
	/// The MVC application entry-point.
	/// </summary>
	/// <seealso cref="System.Web.HttpApplication" />
	public class MvcApplication : System.Web.HttpApplication
    {
		/// <summary>
		/// Application starting point.
		/// </summary>
		protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
