using Termoservis.Contracts.Repository;
using Termoservis.Models;

namespace Termoservis.DAL.Repositories
{
	/// <summary>
	/// The <see cref="TelephoneNumber"/> repository contract.
	/// </summary>
	public interface ITelephoneNumbersRepository : IAddRepository<TelephoneNumber, int>
	{
	}
}