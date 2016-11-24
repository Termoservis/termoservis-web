using System;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using Termoservis.Contracts.Services;
using Termoservis.Models;

namespace Termoservis.DAL.Repositories
{
    public class CustomerDevicesRepository : ICustomerDevicesRepository
    {
        private readonly ApplicationDbContext context;
        private readonly ILogger logger;


        public CustomerDevicesRepository(ApplicationDbContext context, ILoggingService loggingService)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (loggingService == null) throw new ArgumentNullException(nameof(loggingService));

            this.context = context;
            this.logger = loggingService.GetLogger<CustomerDevicesRepository>();
        }


        public IQueryable<CustomerDevice> GetAll()
        {
            return this.context.CustomerDevices.AsQueryable();
        }

        public CustomerDevice Get(long id)
        {
            return this.context.CustomerDevices.FirstOrDefault(d => d.Id == id);
        }

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