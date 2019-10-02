using System.Threading.Tasks;
using Termoservis.Contracts.Repository;
using Termoservis.Models;

namespace Termoservis.DAL.Repositories
{
    /// <summary>
	/// The <see cref="Customer"/> repository contract.
	/// </summary>
	public interface ICustomersRepository : IEditRepository<Customer, long>
    {
        /// <summary>
        /// Adds the customer to the repository.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="shouldSaveChanges">If set to <c>True</c> changes to the context will be saved. Default is <c>True</c>.</param>
        /// <returns>
        /// Returns the customer instance that was added to the repository.
        /// </returns>
        Task<Customer> AddAsync(Customer model, bool shouldSaveChanges = true);

    }
}