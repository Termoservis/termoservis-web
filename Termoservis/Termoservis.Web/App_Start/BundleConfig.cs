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
			bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
				"~/Scripts/jquery-{version}.js",
                "~/Scripts/jquery.unobtrusive-ajax.js"));

			bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
				"~/Scripts/jquery.validate*"));

			// Use the development version of Modernizr to develop with and learn from. Then, when you're
			// ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
			bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
				"~/Scripts/modernizr-*"));

			bundles.Add(new ScriptBundle("~/bundles/tether").Include(
				"~/Scripts/tether/tether.js"));

			bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
				"~/Scripts/bootstrap.js",
				"~/Scripts/respond.js"));

			bundles.Add(new StyleBundle("~/bundles/css").Include(
				"~/Content/tether/tether.css",
				"~/Content/tether/tether-theme-basic.css",
				"~/Content/bootstrap.css",
				"~/Content/site.css",
                "~/Content/css/select2.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/select2").Include(
                "~/Scripts/select2.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/throttledebounce").Include(
                "~/Scripts/jquery.ba-throttle-debounce.min.js"));
        }
	}
}
