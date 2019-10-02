using System.Linq;
using Termoservis.Contracts.Repository;
using Termoservis.Models;

namespace Termoservis.DAL.Repositories
{
	/// <summary>
	/// The places repository contract.
	/// </summary>
	public interface IPlacesRepository : IRepository<Place, int>
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