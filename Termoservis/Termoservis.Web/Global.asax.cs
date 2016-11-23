using System;
using System.Data.Entity;
using System.Data.Entity.Migrations.Model;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Termoservis.DAL;
using Termoservis.DAL.Migrations;

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

            // Migrate database
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration>());
        }
    }
}
