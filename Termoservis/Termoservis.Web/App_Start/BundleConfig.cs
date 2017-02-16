using System.Web.Optimization;

namespace Termoservis.Web
{
	/// <summary>
	/// The bundle configuration.
	/// </summary>
	// ReSharper disable once ClassNeverInstantiated.Global
	public class BundleConfig
	{
		/// <summary>
		/// Registers the bundles.
		/// </summary>
		/// <param name="bundles">The bundles.</param>
		public static void RegisterBundles(BundleCollection bundles)
		{
            bundles.Add(new ScriptBundle("~/bundles/customers/create")
                .Include("~/Scripts/Customers/Create.js"));

            bundles.Add(new ScriptBundle("~/bundles/standard")
                .Include("~/Scripts/modernizr-*")
                .Include("~/Scripts/jquery-{version}.js")
                .Include("~/Scripts/jquery.unobtrusive-ajax.js")
                .Include("~/Scripts/jquery.validate*")
                .Include("~/Scripts/select2.js")
                .Include("~/Scripts/tether/tether.js")
                .Include("~/Scripts/bootstrap.js")
                .Include("~/Scripts/respond.js")
                .Include("~/Scripts/bootstrap-datepicker.js")
                .Include("~/Scripts/jquery.ba-throttle-debounce.js")
                .Include("~/Scripts/moment-with-locales.js")
                .Include("~/Scripts/mprogress.js")
                .Include("~/Scripts/markdown.js"));

		    bundles.Add(new StyleBundle("~/bundles/css")
                .Include("~/Content/tether/tether.css")
                .Include("~/Content/tether/tether-theme-basic.css")
                .Include("~/Content/bootstrap-reboot.css")
                .Include("~/Content/bootstrap-grid.css")
                .Include("~/Content/bootstrap.css")
                .Include("~/Content/site.css")
                .Include("~/Content/css/select2.css")
                .Include("~/Content/bootstrap-datepicker.standalone.css")
                .Include("~/Content/mprogress.css"));
		}
	}
}
