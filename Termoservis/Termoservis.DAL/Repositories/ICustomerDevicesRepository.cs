using Termoservis.Contracts.Repository;
using Termoservis.Models;

namespace Termoservis.DAL.Repositories
{
    /// <summary>
    /// The customer devices repository.
    /// </summary>
    /// <seealso cref="IAddRepository{CustomerDevice, Int64}" />
    public interface ICustomerDevicesRepository : IAddRepository<CustomerDevice, long>
    {
    }
}