using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using Termoservis.Common.Extensions;
using Termoservis.Contracts.Services;
using Termoservis.Models;

namespace Termoservis.DAL.Repositories
{
    /// <summary>
    /// The <see cref="Customer"/> repository.
    /// </summary>
    public class CustomersRepository : ICustomersRepository
    {
        private readonly ApplicationDbContext context;
        private readonly ILogger logger;


        /// <summary>
        /// Initializes a new instance of the <see cref="CustomersRepository"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="loggingService">The logging service.</param>
        /// <exception cref="ArgumentNullException">
        /// context
        /// or
        /// loggingService
        /// </exception>
        public CustomersRepository(
            ApplicationDbContext context,
            ILoggingService loggingService)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            this.context = context;
            this.logger = loggingService?.GetLogger<CustomersRepository>();
        }


        /// <summary>
        /// Gets all customers from the repository.
        /// </summary>
        /// <returns>
        /// Returns collection of all customers from the repository.
        /// </returns>
        public IQueryable<Customer> GetAll()
        {
            return this.context.Customers
                .Include(c => c.WorkItems)
                .Include(c => c.WorkItems.Select(i => i.Worker));
        }

        /// <summary>
        /// Gets the model with specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// Returns the customer with specified identifier; returns null if not found.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Value cannot be zero.
        /// </exception>
        public Customer Get(long id)
        {
            if (id <= 0)
                throw new ArgumentException("Value cannot be zero.", nameof(id));

            return this.GetAll().FirstOrDefault(customer => customer.Id == id);
        }

        /// <summary>
        /// Adds the customer to the repository.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// Returns the customer instance that was added to the repository.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// model
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Customer identifier must be zero.
        /// </exception>
        public async Task<Customer> AddAsync(Customer model)
        {
            return await AddAsync(model, true);
        }

        /// <summary>
        /// Adds the customer to the repository.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="shouldSaveChanges">If set to <c>True</c> changes to the context will be saved. Default is <c>True</c>.</param>
        /// <returns>
        /// Returns the customer instance that was added to the repository.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// model
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Customer identifier must be zero.
        /// </exception>
        // ReSharper disable once MethodOverloadWithOptionalParameter
        public async Task<Customer> AddAsync(Customer model, bool shouldSaveChanges = true)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));
            if (model.Id != 0)
                throw new ArgumentOutOfRangeException(nameof(model.Id), "Customer identifier must be zero.");

            // Validate model
            this.ValidateModel(model);

            // Assign creation date
            if (model.CreationDate == default(DateTime))
                model.CreationDate = DateTime.UtcNow;

            model.SearchKeywords = this.GetSearchKeywords(model);

            // Add customer to the repository and save
            this.context.Customers.Add(model);

            if (shouldSaveChanges)
                await this.context.SaveChangesAsync();

            this.logger?.Information(
                "Added new {CustomerName} ({CustomerId})",
                model.Name, model.Id);

            return model;
        }

        /// <summary>
        /// Edits the customer with specified identifier with given model data.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="model">The model.</param>
        /// <returns>
        /// Returns the edited customer instance.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// model
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Customer identifier must not be null or empty string.
        /// </exception>
        /// <exception cref="InvalidDataException">
        /// Customer must have address assigned.
        /// or
        /// Customer's name must not be zero.
        /// </exception>
        public async Task<Customer> EditAsync(long id, Customer model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));
            if (model.Id <= 0)
                throw new ArgumentOutOfRangeException(nameof(model.Id), "Customer identifier must not be zero.");

            // Validate model
            this.ValidateModel(model);

            // Retrieve from database
            var customerDb = this.Get(id);

            // Clear telephone numbers list if new list is empty
            if (!model.TelephoneNumbers.Any())
                customerDb.TelephoneNumbers.Clear();

            // Edit the customer using the model data
            customerDb.Name = model.Name;
            customerDb.Address = null;
            customerDb.AddressId = model.AddressId;
            customerDb.Email = model.Email;
            customerDb.Note = model.Note;
            customerDb.TelephoneNumbers = model.TelephoneNumbers;
            customerDb.SearchKeywords = this.GetSearchKeywords(customerDb);

            // Save context changes
            await this.context.SaveChangesAsync();

            this.logger?.Information(
                "Edited customer {CustomerName} ({CustomerId})", 
                customerDb.Name, customerDb.Id);

            return customerDb;
        }

        /// <summary>
        /// Gets the search keywords for specified model.
        /// </summary>
        /// <param name="customer">The customer.</param>
        /// <returns>Returns the search keywords string.</returns>
        /// <exception cref="System.NullReferenceException">Couldn't find address of customer.</exception>
        private string GetSearchKeywords(Customer customer)
        {
            // Retrieve address
            var address = customer.Address ?? this.context.Addresses.FirstOrDefault(a => a.Id == customer.AddressId);
            if (address == null)
                throw new NullReferenceException("Couldn't find address of customer.");

            // Construct keywords string
            var sb = new StringBuilder();
            sb.Append(customer.Name.AsSearchable());
            sb.Append(' ');
            sb.Append(address.SearchKeywords);
            if (customer.TelephoneNumbers != null)
            {
                foreach (var customerTelephoneNumber in customer.TelephoneNumbers)
                {
                    sb.Append(' ');
                    sb.Append(customerTelephoneNumber.SearchKeywords);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Validates the model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <exception cref="ArgumentNullException">
        /// model
        /// </exception>
        /// <exception cref="InvalidDataException">
        /// Customer must have address assigned.
        /// or
        /// Customer's name must not be null or empty string.
        /// </exception>
        private void ValidateModel(Customer model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            // Validate that model has address assigned
            if (model.Address == null && model.AddressId == 0)
                throw new InvalidDataException("Customer must have address assigned.");

            // Validate name
            if (string.IsNullOrWhiteSpace(model.Name))
                throw new InvalidDataException("Customer's name must not be null or empty string.");
        }

        /// <summary>
        /// Saves the changes.
        /// </summary>
        public async Task Save()
        {
            await this.context.SaveChangesAsync();
        }
    }
}