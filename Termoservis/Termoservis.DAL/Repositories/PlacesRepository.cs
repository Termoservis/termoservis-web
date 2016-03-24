using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Termoservis.Common.Extensions;
using Termoservis.Contracts.Repository;
using Termoservis.DAL.Extensions;
using Termoservis.Models;

namespace Termoservis.DAL.Repositories
{
	/// <summary>
	/// The places repository.
	/// </summary>
	public class PlacesRepository : IPlaceRepository
	{
		private readonly ApplicationDbContext context;


		/// <summary>
		/// Initializes a new instance of the <see cref="PlacesRepository"/> class.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <exception cref="System.ArgumentNullException">context</exception>
		public PlacesRepository(ApplicationDbContext context)
		{
			if (context == null) throw new ArgumentNullException(nameof(context));

			this.context = context;
		}

		/// <summary>
		/// Gets the place by specified identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns>
		/// Returns place with specified identifier; return null if not found.
		/// </returns>
		public Place Get(int id)
		{
			return this.context.Places.FirstOrDefault(place => place.Id == id);
		}

		/// <summary>
		/// Gets the specified place in specified country.
		/// </summary>
		/// <param name="place">The place name.</param>
		/// <param name="country">The country name.</param>
		/// <returns>
		/// Returns place that matches specified place name and country name.
		/// </returns>
		/// <exception cref="System.ArgumentException">
		/// Value cannot be null or empty.
		/// or
		/// Value cannot be null or empty.
		/// </exception>
		public Place Get(string place, string country)
		{
			if (string.IsNullOrEmpty(place)) throw new ArgumentException("Value cannot be null or empty.", nameof(place));
			if (string.IsNullOrEmpty(country)) throw new ArgumentException("Value cannot be null or empty.", nameof(country));

			var placeSearchable = place.AsSearchable();
			var countrySearchable = place.AsSearchable();

			return this.context.Places
				.Include(p => p.Country)
				.FirstOrDefault(p =>
					p.SearchKeywords.Contains(placeSearchable) &&
					p.Country.SearchKeywords.Contains(countrySearchable));
		}

		/// <summary>
		/// Gets the places that match specified name.
		/// </summary>
		/// <param name="place">The places name.</param>
		/// <returns>
		/// Returns places that match specified place name.
		/// </returns>
		/// <exception cref="System.ArgumentException">Value cannot be null or empty.</exception>
		public IQueryable<Place> Get(string place)
		{
			if (string.IsNullOrEmpty(place)) throw new ArgumentException("Value cannot be null or empty.", nameof(place));

			var placeSearchable = place.AsSearchable();

			return this.context.Places.Where(p => p.SearchKeywords.Contains(placeSearchable));
		}

		public Task<Place> AddAsync(Place model)
		{
			throw new NotImplementedException();
		}

		public Task<Place> EditAsync(int id, Place model)
		{
			throw new NotImplementedException();
		}

		public Task<bool> DeleteAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task<bool> DeleteAsync(Place model)
		{
			throw new NotImplementedException();
		}
	}

	/// <summary>
	/// The places repository contract.
	/// </summary>
	public interface IPlaceRepository : IRepository<Place, int>
	{
		/// <summary>
		/// Gets the specified place in specified country.
		/// </summary>
		/// <param name="place">The place name.</param>
		/// <param name="country">The country name.</param>
		/// <returns>Returns place that matches specified place name and country name.</returns>
		Place Get(string place, string country);

		/// <summary>
		/// Gets the places that match specified name.
		/// </summary>
		/// <param name="place">The places name.</param>
		/// <returns>Returns places that match specified place name.</returns>
		IQueryable<Place> Get(string place);
	}
}
