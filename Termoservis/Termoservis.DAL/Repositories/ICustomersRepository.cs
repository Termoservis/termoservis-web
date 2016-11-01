using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using Termoservis.Contracts.Repository;
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
			if (loggingService == null)
				throw new ArgumentNullException(nameof(loggingService));

			this.context = context;
			this.logger = loggingService.GetLogger<CustomersRepository>();
		}


		/// <summary>
		/// Gets all customers from the repository.
		/// </summary>
		/// <returns>
		/// Returns collection of all customers from the repository.
		/// </returns>
		public IQueryable<Customer> GetAll()
		{
			return this.context.Customers;
		}

		/// <summary>
		/// Gets the model with specified identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns>
		/// Returns the customer with specified identifier; returns null if not found.
		/// </returns>
		/// <exception cref="ArgumentException">
		/// Value cannot be null or whitespace.
		/// </exception>
		public Customer Get(string id)
		{
			if (string.IsNullOrWhiteSpace(id))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(id));

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
		/// Customer identifier must be null or empty string.
		/// </exception>
		public async Task<Customer> AddAsync(Customer model)
		{
			if (model == null)
				throw new ArgumentNullException(nameof(model));
			if (!string.IsNullOrWhiteSpace(model.Id))
				throw new ArgumentOutOfRangeException(nameof(model.Id), "Customer identifier must be null or empty string.");

			// Validate model
			this.ValidateModel(model);

			// Assign creation date
			model.CreationDate = DateTime.Now;

			// Add customer to the repository and save
			this.context.Customers.Add(model);
			await this.context.SaveChangesAsync();

			this.logger.Information(
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
		/// Customer's name must not be null or empty string.
		/// </exception>
		public async Task<Customer> EditAsync(string id, Customer model)
		{
			if (model == null)
				throw new ArgumentNullException(nameof(model));
			if (string.IsNullOrWhiteSpace(model.Id))
				throw new ArgumentOutOfRangeException(nameof(model.Id), "Customer identifier must not be null or empty string.");

			// Validate model
			this.ValidateModel(model);

			// Retrieve from database
			var customerDb = this.Get(id);

			// Edit the customer using the model data
			customerDb.Name = model.Name;
			customerDb.Address = null;
			customerDb.AddressId = model.AddressId;
			customerDb.Email = model.Email;
			customerDb.Note = model.Note;
			customerDb.TelephoneNumbers = model.TelephoneNumbers;

			// Save context changes
			await this.context.SaveChangesAsync();

			this.logger.Information(
				"Edited customer {CustomerName} ({CustomerId})", 
				customerDb.Name, customerDb.Id);

			return customerDb;
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
			if (model.AddressId != 0 && model.Address == null)
				throw new InvalidDataException("Customer must have address assigned.");

			// Validate name
			if (string.IsNullOrWhiteSpace(model.Name))
				throw new InvalidDataException("Customer's name must not be null or empty string.");
		}
	}

	/// <summary>
	/// The <see cref="Customer"/> repository contract.
	/// </summary>
	public interface ICustomersRepository : IEditRepository<Customer, string>
	{
	}
}