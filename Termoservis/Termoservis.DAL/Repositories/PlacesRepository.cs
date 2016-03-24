using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Termoservis.Models;

namespace Termoservis.DAL.Repositories
{
	///// <summary>
	///// The places repository.
	///// </summary>
	//public class PlacesRepository : IPlaceRepository
	//{
	//	private readonly ApplicationDbContext context;


	//	/// <summary>
	//	/// Initializes a new instance of the <see cref="PlacesRepository"/> class.
	//	/// </summary>
	//	/// <param name="context">The context.</param>
	//	/// <exception cref="System.ArgumentNullException">context</exception>
	//	public PlacesRepository(ApplicationDbContext context)
	//	{
	//		if (context == null) throw new ArgumentNullException(nameof(context));

	//		this.context = context;
	//	}
	//}

	/// <summary>
	/// The places repository contract.
	/// </summary>
	public interface IPlaceRepository
	{
		/// <summary>
		/// Gets the place by specified identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns>Returns place with specified identifier; return null if not found.</returns>
		Place Get(int id);

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
