using System.Web.Mvc;

namespace Termoservis.Web
{
	/// <summary>
	/// The filter configuration.
	/// </summary>
	// ReSharper disable once ClassNeverInstantiated.Global
	public class FilterConfig
	{
		/// <summary>
		/// Registers the global filters.
		/// </summary>
		/// <param name="filters">The filters.</param>
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
		}
	}
}
