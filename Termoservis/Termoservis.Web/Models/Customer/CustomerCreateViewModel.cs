using System.Collections.Generic;
using System.ComponentModel;
using Termoservis.DAL.ViewModels;
using Termoservis.Models;

namespace Termoservis.Web.Models.Customer
{
	/// <summary>
	/// Customer create view model.
	/// </summary>
	/// <seealso cref="ILocationViewModel" />
	public class CustomerCreateViewModel : Termoservis.Models.Customer, ILocationViewModel
	{
		/// <summary>
		/// Gets or sets the available places.
		/// </summary>
		/// <value>
		/// The available places.
		/// </value>
		public IEnumerable<Place> AvailablePlaces { get; set; } = new List<Place>();

		/// <summary>
		/// Gets or sets the available countries.
		/// </summary>
		/// <value>
		/// The available countries.
		/// </value>
		public IEnumerable<Country> AvailableCountries { get; set; } = new List<Country>();

		/// <summary>
		/// Gets or sets the name of the street.
		/// </summary>
		/// <value>
		/// The name of the customer street.
		/// </value>
		[DisplayName("Adresa")]
		public string CustomerStreetName { get; set; }

		/// <summary>
		/// Gets or sets the place identifier.
		/// </summary>
		/// <value>
		/// The customer place identifier.
		/// </value>
		[DisplayName("Mjesto")]
		public int? CustomerPlaceId { get; set; }

		/// <summary>
		/// Gets or sets the country identifier.
		/// </summary>
		/// <value>
		/// The customer country identifier.
		/// </value>
		[DisplayName("Država")]
		public int? CustomerCountryId { get; set; }
	}
}