using Termoservis.Contracts.Repository;
using Termoservis.Models;

namespace Termoservis.DAL.Repositories
{
    /// <summary>
    /// WorkItem repository.
    /// </summary>
    /// <seealso cref="IEditRepository{WorkItem, Int64}" />
    /// <seealso cref="IDeleteRepository{WorkItem, Int64}" />
    public interface IWorkItemsRepository : IEditRepository<WorkItem, long>, IDeleteRepository<WorkItem, long>
    {
    }
}