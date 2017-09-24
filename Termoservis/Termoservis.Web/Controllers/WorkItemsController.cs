using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using Serilog;
using Termoservis.Contracts.Services;
using Termoservis.DAL;
using Termoservis.DAL.Repositories;
using Termoservis.Models;
using Termoservis.Web.Models.Customer;

namespace Termoservis.Web.Controllers
{
    /// <summary>
    /// The work item controller.
    /// </summary>
    /// <seealso cref="Controller" />
    [System.Web.Mvc.Authorize]
    [RequireHttps]
    public class WorkItemsController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IWorkItemsRepository workItemsRepository;
        private readonly ILogger logger;


        /// <summary>
        /// Initializes a new instance of the <see cref="WorkItemsController"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="workItemsRepository">The work items repository.</param>
        /// <param name="loggingService">The logging service.</param>
        /// <exception cref="System.ArgumentNullException">
        /// context
        /// or
        /// workItemsRepository
        /// or
        /// customersRepository
        /// </exception>
        public WorkItemsController(
            ApplicationDbContext context, 
            IWorkItemsRepository workItemsRepository,
            ILoggingService loggingService)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (workItemsRepository == null) throw new ArgumentNullException(nameof(workItemsRepository));
            
            this.context = context;
            this.workItemsRepository = workItemsRepository;
            this.logger = loggingService?.GetLogger<WorkItemsController>();
        }

        //
        // POST: WorkItems/Create        
        /// <summary>
        /// Creates the work item.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <exception cref="InvalidDataException">WorkItem is null.</exception>
        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(WorkItemFormViewModel viewModel)
        {
            // Validate work item
            if (viewModel.WorkItem == null)
                throw new InvalidDataException("WorkItem is null.");

            // Retrieve required data
            var worker = viewModel.WorkItem.WorkerId.HasValue
                ? this.context.Workers.FirstOrDefault(w => w.Id == viewModel.WorkItem.WorkerId.Value)
                : null;

            // Populate model with view model data
            var workItem = new WorkItem
            {
                CustomerId = viewModel.WorkItem.CustomerId,
                Date = viewModel.WorkItem.Date ?? DateTime.Now,
                Description = viewModel.WorkItem.Description ?? string.Empty,
                Price = viewModel.WorkItem.Price,
                Type = viewModel.WorkItem.Type,
                WorkerId = worker?.Id,
                AffectedDevices = viewModel.AffectedDevices?
                    .Select(deviceId => this.context.CustomerDevices.AsNoTracking().FirstOrDefault(device => device.Id == deviceId))
                    .ToList() ?? new List<CustomerDevice>()
            };

            // Create work item
            var createdWorkItem = await this.workItemsRepository.AddAsync(workItem);

            // Redirect to details
            return RedirectToAction("Details", "Customers", new {id = createdWorkItem.CustomerId});
        }

        //
        // POST: WorkItems/Edit        
        /// <summary>
        /// Edits the work item.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <exception cref="InvalidDataException">WorkItem is null.</exception>
        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(WorkItemFormViewModel viewModel)
        {
            // Validate work item
            if (viewModel.WorkItem == null)
                throw new InvalidDataException("WorkItem is null.");

            // Retrieve required data
            var worker = viewModel.WorkItem.WorkerId.HasValue
                ? this.context.Workers.FirstOrDefault(w => w.Id == viewModel.WorkItem.WorkerId.Value)
                : null;

            // Populate model with view model data
            var workItem = new WorkItem
            {
                Id = viewModel.WorkItem.Id,
                Date = viewModel.WorkItem.Date ?? DateTime.Now,
                Description = viewModel.WorkItem.Description ?? string.Empty,
                Price = viewModel.WorkItem.Price,
                Type = viewModel.WorkItem.Type,
                WorkerId = worker?.Id,
                AffectedDevices = viewModel.AffectedDevices?
                    .Select(deviceId => this.context.CustomerDevices.FirstOrDefault(device => device.Id == deviceId))
                    .ToList() ?? new List<CustomerDevice>()
            };

            // Edit work item
            var editedWorkItem = await this.workItemsRepository.EditAsync(workItem.Id, workItem);

            // Redirect to details
            return RedirectToAction("Details", "Customers", new { id = editedWorkItem.CustomerId });
        }

        // 
        // DELETE: WorkItems/Delete/{id}        
        /// <summary>
        /// Deletes the work item with specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Returns the Customer Details view.</returns>
        /// <exception cref="ArgumentOutOfRangeException">id</exception>
        [System.Web.Mvc.HttpGet]
        public async Task<ActionResult> Delete([FromUri]long id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id));

            // Check if work item exists
            var workItem = this.workItemsRepository.Get(id);
            if (workItem == null)
                return HttpNotFound("WorkItem with specified identifier was not found.");

            // Delete the work item
            await this.workItemsRepository.DeleteAsync(id);

            return RedirectToAction("Details", "Customers", new {id = workItem.CustomerId});
        }
    }
}