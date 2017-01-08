using System;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using Termoservis.Contracts.Services;
using Termoservis.Models;

namespace Termoservis.DAL.Repositories
{
    /// <summary>
    /// The customer devices repository.
    /// </summary>
    /// <seealso cref="ICustomerDevicesRepository" />
    public class CustomerDevicesRepository : ICustomerDevicesRepository
    {
        private readonly ApplicationDbContext context;
        private readonly ILogger logger;


        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerDevicesRepository"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="loggingService">The logging service.</param>
        /// <exception cref="ArgumentNullException">
        /// context
        /// or
        /// loggingService
        /// </exception>
        public CustomerDevicesRepository(ApplicationDbContext context, ILoggingService loggingService)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (loggingService == null) throw new ArgumentNullException(nameof(loggingService));

            this.context = context;
            this.logger = loggingService.GetLogger<CustomerDevicesRepository>();
        }


        /// <summary>
        /// Gets all customer devices from the repository.
        /// </summary>
        /// <returns>Returns collection of all customer devices from the repository.</returns>
        public IQueryable<CustomerDevice> GetAll()
        {
            return this.context.CustomerDevices;
        }

        /// <summary>
        /// Gets the model by specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// Returns model with specified identifier; returns null if not found.
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException">id - Customer device identifier must not be zero.</exception>
        public CustomerDevice Get(long id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), "Customer device identifier must not be zero.");

            return this.GetAll().FirstOrDefault(d => d.Id == id);
        }

        /// <summary>
        /// Adds the customer device to the repository.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// Returns the customer device instance that was added to the repository.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">model</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Id - Customer device identifier must be zero.</exception>
        public async Task<CustomerDevice> AddAsync(CustomerDevice model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));
            if (model.Id != 0)
                throw new ArgumentOutOfRangeException(nameof(model.Id), "Customer device identifier must be zero.");

            // Add to the repository and save
            this.context.CustomerDevices.Add(model);
            await this.context.SaveChangesAsync();

            this.logger.Information(
                "Added new customer device {CustomerDeviceName} ({CustomerDeviceId}).",
                model.Name, model.Id);

            return model;
        }
    }
}