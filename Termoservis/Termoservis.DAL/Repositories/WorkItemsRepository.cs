using System;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using Termoservis.Contracts.Services;
using Termoservis.Models;

namespace Termoservis.DAL.Repositories
{
    public class WorkItemsRepository : IWorkItemsRepository
    {
        private readonly ApplicationDbContext context;
        private readonly ILogger logger;

        public WorkItemsRepository(ApplicationDbContext context, ILoggingService loggingService)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (loggingService == null) throw new ArgumentNullException(nameof(loggingService));

            this.context = context;
            this.logger = loggingService.GetLogger<WorkItemsRepository>();
        }


        public IQueryable<WorkItem> GetAll()
        {
            return this.context.WorkItems.AsQueryable();
        }

        public WorkItem Get(long id)
        {
            return this.context.WorkItems.FirstOrDefault(i => i.Id == id);
        }

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