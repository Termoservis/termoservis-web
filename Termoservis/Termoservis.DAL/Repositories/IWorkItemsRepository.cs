using Termoservis.Contracts.Repository;
using Termoservis.Models;

namespace Termoservis.DAL.Repositories
{
    public interface IWorkItemsRepository : IAddRepository<WorkItem, long>
    {
    }
}