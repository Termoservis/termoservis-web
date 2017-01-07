using Termoservis.Contracts.Repository;
using Termoservis.Models;

namespace Termoservis.DAL.Repositories
{
    /// <summary>
    /// WorkItem repository.
    /// </summary>
    /// <seealso cref="IAddRepository{WorkItem, Int64}" />
    public interface IWorkItemsRepository : IAddRepository<WorkItem, long>
    {
    }
}