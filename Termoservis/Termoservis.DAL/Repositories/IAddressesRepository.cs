using System.Threading.Tasks;
using Termoservis.Contracts.Repository;
using Termoservis.Models;

namespace Termoservis.DAL.Repositories
{
	/// <summary>
	/// The <see cref="Address"/> repository.
	/// </summary>
	public interface IAddressesRepository : IAddRepository<Address, long>
	{
		/// <summary>
		/// Ensures that the address exists in repository.
		/// </summary>
		/// <param name="address">The address.</param>
		/// <returns>Returns the address that matches specified address from repository.</returns>
		Task<Address> EnsureExistsAsync(Address address);

		/// <summary>
		/// Ensures that the address exists in repository.
		/// </summary>
		/// <param name="streetAddress">The street address.</param>
		/// <param name="placeId">The place identifier.</param>
		/// <returns>Returns the address that matches specified address from repository.</returns>
		Task<Address> EnsureExistsAsync(string streetAddress, int? placeId);
	}
}