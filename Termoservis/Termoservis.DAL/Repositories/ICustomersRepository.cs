using Termoservis.Contracts.Repository;
using Termoservis.Models;

namespace Termoservis.DAL.Repositories
{
    /// <summary>
	/// The <see cref="Customer"/> repository contract.
	/// </summary>
	public interface ICustomersRepository : IEditRepository<Customer, long>
	{
	}
}