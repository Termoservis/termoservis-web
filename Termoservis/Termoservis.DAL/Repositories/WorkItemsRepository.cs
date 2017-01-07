using System;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using Termoservis.Contracts.Services;
using Termoservis.Models;

namespace Termoservis.DAL.Repositories
{
    /// <summary>
    /// The work items repository.
    /// </summary>
    /// <seealso cref="Termoservis.DAL.Repositories.IWorkItemsRepository" />
    public class WorkItemsRepository : IWorkItemsRepository
    {
        private readonly ApplicationDbContext context;
        private readonly ILogger logger;


        /// <summary>
        /// Initializes a new instance of the <see cref="WorkItemsRepository"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="loggingService">The logging service.</param>
        /// <exception cref="System.ArgumentNullException">
        /// context
        /// or
        /// loggingService
        /// </exception>
        public WorkItemsRepository(ApplicationDbContext context, ILoggingService loggingService)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (loggingService == null) throw new ArgumentNullException(nameof(loggingService));

            this.context = context;
            this.logger = loggingService.GetLogger<WorkItemsRepository>();
        }


        /// <summary>
        /// Gets all models from repository.
        /// </summary>
        /// <returns>
        /// Returns query for all models in the repository.
        /// </returns>
        public IQueryable<WorkItem> GetAll()
        {
            return this.context.WorkItems.AsQueryable();
        }

        /// <summary>
        /// Gets the model by specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// Returns model with specified identifier; returns null if not found.
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException">id - WorkItem identifier must not be zero.</exception>
        public WorkItem Get(long id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), "WorkItem identifier must not be zero.");

            return this.GetAll().FirstOrDefault(workItem => workItem.Id == id);
        }

        /// <summary>
        /// Adds the work item to the repository.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// Returns the work item instance that was added to the repository.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">model</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Id - Work item identifier must be zero.</exception>
        public async Task<WorkItem> AddAsync(WorkItem model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));
            if (model.Id != 0)
                throw new ArgumentOutOfRangeException(nameof(model.Id), "Work item identifier must be zero.");
            
            // Add to the repository and save
            this.context.WorkItems.Add(model);
            await this.context.SaveChangesAsync();

            this.logger.Information(
                "Added new work item ({WorkItemId}).",
                model.Id);

            return model;
        }
    }
}