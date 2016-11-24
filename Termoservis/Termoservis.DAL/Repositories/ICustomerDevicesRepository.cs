using Termoservis.Contracts.Repository;
using Termoservis.Models;

namespace Termoservis.DAL.Repositories
{
    public interface ICustomerDevicesRepository : IAddRepository<CustomerDevice, long>
    {
    }
}