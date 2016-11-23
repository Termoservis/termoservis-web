using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using Termoservis.Models;

namespace Termoservis.DAL
{
	/// <summary>
	/// The application database context.
	/// </summary>
	/// <seealso cref="ApplicationUser" />
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
		/// <summary>
		/// Gets or sets the addresses.
		/// </summary>
		/// <value>
		/// The addresses.
		/// </value>
		public IDbSet<Address> Addresses { get; set; }

		/// <summary>
		/// Gets or sets the countries.
		/// </summary>
		/// <value>
		/// The countries.
		/// </value>
		public IDbSet<Country> Countries { get; set; }

		/// <summary>
		/// Gets or sets the customers.
		/// </summary>
		/// <value>
		/// The customers.
		/// </value>
		public IDbSet<Customer> Customers { get; set; }

		/// <summary>
		/// Gets or sets the places.
		/// </summary>
		/// <value>
		/// The places.
		/// </value>
		public IDbSet<Place> Places { get; set; }

		/// <summary>
		/// Gets or sets the telephone numbers.
		/// </summary>
		/// <value>
		/// The telephone numbers.
		/// </value>
		public IDbSet<TelephoneNumber> TelephoneNumbers { get; set; }

        public IDbSet<WorkItem> WorkItems { get; set; }

	    public IDbSet<CustomerDevice> CustomerDevices { get; set; }


	    /// <summary>
		/// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
		/// </summary>
		public ApplicationDbContext()
			: base("DefaultConnection", throwIfV1Schema: false)
		{
		}
	}
}