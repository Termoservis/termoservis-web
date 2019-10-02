using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Termoservis.DAL.ViewModels;

namespace Termoservis.DAL.Extensions
{
	/// <summary>
	/// The <see cref="ILocationViewModel"/> extensions.
	/// </summary>
	// ReSharper disable once InconsistentNaming
	public static class ILocationViewModelExtensions
	{
		/// <summary>
		/// Populates the locations.
		/// </summary>
		/// <param name="viewModel">The view model.</param>
		/// <param name="context">The context.</param>
		/// <exception cref="System.ArgumentNullException">
		/// viewModel
		/// or
		/// context
		/// </exception>
		public static async Task PopulateLocationsAsync(this ILocationViewModel viewModel, ApplicationDbContext context)
		{
			if (viewModel == null) throw new ArgumentNullException(nameof(viewModel));
			if (context == null) throw new ArgumentNullException(nameof(context));
            
			var countries = await context.Countries.ToListAsync();

			viewModel.AvailableCountries = countries;
		}
	}
}
