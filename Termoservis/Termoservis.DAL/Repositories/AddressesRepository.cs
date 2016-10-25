using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using Termoservis.Contracts.Services;
using Termoservis.Models;

namespace Termoservis.DAL.Repositories
{
	/// <summary>
	/// The <see cref="Address"/> repository.
	/// </summary>
	public class AddressesRepository : IAddressesRepository
	{
		private readonly ApplicationDbContext context;
		private readonly IPlacesRepository placesRepository;
		private readonly ILogger logger;


		/// <summary>
		/// Initializes a new instance of the <see cref="AddressesRepository"/> class.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="placesRepository">The places repository.</param>
		/// <param name="loggingService">The logging service.</param>
		/// <exception cref="ArgumentNullException">
		/// context
		/// or
		/// placesRepository
		/// or
		/// loggingService
		/// </exception>
		public AddressesRepository(ApplicationDbContext context, IPlacesRepository placesRepository, ILoggingService loggingService)
		{
			if (context == null)
				throw new ArgumentNullException(nameof(context));
			if (placesRepository == null)
				throw new ArgumentNullException(nameof(placesRepository));
			if (loggingService == null)
				throw new ArgumentNullException(nameof(loggingService));

			this.context = context;
			this.placesRepository = placesRepository;
			this.logger = loggingService.GetLogger<AddressesRepository>();
		}


		/// <summary>
		/// Gets all models from repository.
		/// </summary>
		/// <returns>
		/// Returns query for all models in the repository.
		/// </returns>
		public IQueryable<Address> GetAll()
		{
			return this.context.Addresses.AsQueryable();
		}

		/// <summary>
		/// Gets the model by specified identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns>
		/// Returns model with specified identifier; return null if not found.
		/// </returns>
		/// <exception cref="ArgumentOutOfRangeException">Address identifier must not be zero.</exception>
		public Address Get(long id)
		{
			if (id <= 0)
				throw new ArgumentOutOfRangeException(nameof(id), "Address identifier must not be zero.");

			return this.context.Addresses.FirstOrDefault(address => address.Id == id);
		}

		/// <summary>
		/// Adds the address to the repository.
		/// </summary>
		/// <param name="model">The model.</param>
		/// <returns>Returns the address instance that was added to the repository.</returns>
		/// <exception cref="ArgumentNullException">model</exception>
		/// <exception cref="ArgumentOutOfRangeException">Address identifier must be zero.</exception>
		public async Task<Address> AddAsync(Address model)
		{
			if (model == null)
				throw new ArgumentNullException(nameof(model));
			if (model.Id != 0)
				throw new ArgumentOutOfRangeException(nameof(model.Id), "Address identifier must be zero.");

			// Validate
			this.ValidateModel(model);

			// Add to the repository and save
			this.context.Addresses.Add(model);
			await this.context.SaveChangesAsync();

			this.logger.Information(
				"Added new address {StreetAddress} ({AddressId}) to {PlaceName} ({PlaceId}).",
				model.StreetAddress, model.Id, model.Place.Name, model.PlaceId);

			return model;
		}

		/// <summary>
		/// Ensures that the address exists in repository.
		/// </summary>
		/// <param name="address">The address.</param>
		/// <returns>
		/// Returns the address that matches specified address from repository.
		/// </returns>
		/// <exception cref="ArgumentNullException">address</exception>
		/// <exception cref="InvalidDataException">Invalid place identifier.</exception>
		public async Task<Address> EnsureExistsAsync(Address address)
		{
			if (address == null)
				throw new ArgumentNullException(nameof(address));

			// If exists already, return
			if (address.Id != 0)
				return address;

			// Retrieve place
			var place = this.placesRepository.Get(address.Id);
			if (place == null)
				throw new InvalidDataException("Invalid place identifier.");

			// Retrieve address with exact street name
			var addressDb = this.TryMatchStreetName(address.StreetAddress);
			if (addressDb != null)
				return addressDb;

			// Create new address
			return await this.AddAsync(address);
		}

		/// <summary>
		/// Ensures that the address exists in repository.
		/// </summary>
		/// <param name="streetAddress">The street address.</param>
		/// <param name="placeId">The place identifier.</param>
		/// <returns>
		/// Returns the address that matches specified address from repository.
		/// </returns>
		/// <exception cref="ArgumentException">Value cannot be null or whitespace.</exception>
		/// <exception cref="ArgumentOutOfRangeException">Place identifier must not be zero.</exception>
		public async Task<Address> EnsureExistsAsync(string streetAddress, int placeId)
		{
			if (string.IsNullOrWhiteSpace(streetAddress))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(streetAddress));
			if (placeId <= 0)
				throw new ArgumentOutOfRangeException(nameof(placeId), "Place identifier must not be zero.");

			return await this.EnsureExistsAsync(new Address
			{
				PlaceId = placeId,
				StreetAddress = streetAddress
			});
		}

		/// <summary>
		/// Tries to match given street address with already existing address in the repository.
		/// </summary>
		/// <param name="streetAddress">The street address.</param>
		/// <returns>Returns the matched address; <c>null</c> if no matching address is found.</returns>
		private Address TryMatchStreetName(string streetAddress)
		{
			if (string.IsNullOrWhiteSpace(streetAddress))
				return null;

			streetAddress = streetAddress.ToLower().Trim();

			return this.context.Addresses.FirstOrDefault(address =>
					address.StreetAddress.ToLower() == streetAddress);
		}

		/// <summary>
		/// Validates the model.
		/// </summary>
		/// <param name="model">The model.</param>
		/// <exception cref="ArgumentNullException">model</exception>
		/// <exception cref="InvalidDataException">
		/// Place must be assigned to the address.
		/// or
		/// Street address must not be null or empty.
		/// </exception>
		private void ValidateModel(Address model)
		{
			if (model == null)
				throw new ArgumentNullException(nameof(model));

			// Validate place
			if (model.PlaceId <= 0 && model.Place == null)
				throw new InvalidDataException("Place must be assigned to the address.");

			// Validate street address
			if (string.IsNullOrWhiteSpace(model.StreetAddress))
				throw new InvalidDataException("Street address must not be null or empty.");
			if (this.context.Addresses.Any(address => address.StreetAddress == model.StreetAddress))
				throw new InvalidDataException("Duplicate street address.");

		}
	}
}
