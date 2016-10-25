using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using Termoservis.Common.Extensions;
using Termoservis.Contracts.Services;
using Termoservis.Models;

namespace Termoservis.DAL.Repositories
{
	/// <summary>
	/// The places repository.
	/// </summary>
	/// <seealso cref="IPlacesRepository" />
	// ReSharper disable once UnusedMember.Global
	public class PlacesRepository : IPlacesRepository
	{
		private readonly ApplicationDbContext context;
		private readonly ILogger logger;


		/// <summary>
		/// Initializes a new instance of the <see cref="PlacesRepository"/> class.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="logger">The logger.</param>
		/// <exception cref="System.ArgumentNullException">
		/// context
		/// or
		/// logger
		/// </exception>
		public PlacesRepository(ApplicationDbContext context, ILoggingService logger)
		{
			if (context == null)
				throw new ArgumentNullException(nameof(context));
			if (logger == null)
				throw new ArgumentNullException(nameof(logger));

			this.context = context;
			this.logger = logger.GetLogger<PlacesRepository>();
		}


		/// <summary>
		/// Gets all places from the repository..
		/// </summary>
		/// <returns>Returns collection of places from the repository.</returns>
		public IQueryable<Place> GetAll()
		{
			return this.context.Places.AsQueryable();
		}

		/// <summary>
		/// Gets the place by specified identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns>
		/// Returns place with specified identifier; return null if not found.
		/// </returns>
		public Place Get(int id)
		{
			return this.context.Places.FirstOrDefault(place => place.Id == id);
		}

		/// <summary>
		/// Gets the specified place in specified country.
		/// </summary>
		/// <param name="place">The place name.</param>
		/// <param name="country">The country name.</param>
		/// <returns>
		/// Returns place that matches specified place name and country name.
		/// </returns>
		/// <exception cref="System.ArgumentException">
		/// Value cannot be null or empty.
		/// or
		/// Value cannot be null or empty.
		/// </exception>
		public Place Get(string place, string country)
		{
			if (string.IsNullOrEmpty(place))
				throw new ArgumentException("Value cannot be null or empty.", nameof(place));
			if (string.IsNullOrEmpty(country))
				throw new ArgumentException("Value cannot be null or empty.", nameof(country));

			var placeSearchable = place.AsSearchable();
			var countrySearchable = place.AsSearchable();

			return this.context.Places
				.Include(p => p.Country)
				.FirstOrDefault(p =>
					p.SearchKeywords.Contains(placeSearchable) &&
					p.Country.SearchKeywords.Contains(countrySearchable));
		}

		/// <summary>
		/// Gets the places that match specified name.
		/// </summary>
		/// <param name="place">The places name.</param>
		/// <returns>
		/// Returns places that match specified place name.
		/// </returns>
		/// <exception cref="System.ArgumentException">Value cannot be null or empty.</exception>
		public IQueryable<Place> Get(string place)
		{
			if (string.IsNullOrEmpty(place))
				throw new ArgumentException("Value cannot be null or empty.", nameof(place));

			var placeSearchable = place.AsSearchable();

			return this.context.Places.Where(p => p.SearchKeywords.Contains(placeSearchable));
		}

		/// <summary>
		/// Adds the place to the repository.
		/// </summary>
		/// <param name="model">The model.</param>
		/// <returns>Returns the place instance that was added to the repository.</returns>
		/// <exception cref="ArgumentNullException">model</exception>
		/// <exception cref="ArgumentOutOfRangeException">Place identifier must be zero.</exception>
		/// <exception cref="InvalidDataException">
		/// Place must contain reference to country.
		/// or
		/// Place name must not be null or empty.
		/// </exception>
		public async Task<Place> AddAsync(Place model)
		{
			if (model == null)
				throw new ArgumentNullException(nameof(model));
			if (model.Id != 0)
				throw new ArgumentOutOfRangeException(nameof(model.Id), "Place identifier must be zero.");

			// Validate
			ValidateModel(model);

			// Add to context and save changes
			this.context.Places.Add(model);
			await this.context.SaveChangesAsync();

			this.logger.Information(
				"Added new place {PlaceName} ({PlaceId}) to {CountryName} ({CountryId}).", 
				model.Name, model.Id, model.Country.Name, model.CountryId);

			return model;
		}

		/// <summary>
		/// Edits the place with specified identifier with given model data.
		/// </summary>
		/// <param name="id">The place identifier to edit.</param>
		/// <param name="model">The place model with new data.</param>
		/// <returns>Returns the edited place instance.</returns>
		/// <exception cref="ArgumentNullException">model</exception>
		/// <exception cref="ArgumentOutOfRangeException">Place identifier must not be null.</exception>
		/// <exception cref="InvalidDataException">
		/// Place must contain reference to country.
		/// or
		/// Place name must not be null or empty.
		/// </exception>
		public async Task<Place> EditAsync(int id, Place model)
		{
			if (model == null)
				throw new ArgumentNullException(nameof(model));
			if (id <= 0)
				throw new ArgumentOutOfRangeException(nameof(id), "Place identifier must not be null.");

			// Validate
			ValidateModel(model);

			// Retrieve from database
			var placeDb = this.Get(id);

			// Edit the place from model data
			placeDb.Country = null;
			placeDb.CountryId = model.CountryId;
			placeDb.Name = model.Name;

			// Save context changes
			await this.context.SaveChangesAsync();

			this.logger.Information(
				"Edited place {PlaceName} ({PlaceId}) in country ({CountryId}).",
				placeDb.Name, id, placeDb.CountryId);

			return placeDb;
		}

		/// <summary>
		/// Deletes the place.
		/// </summary>
		/// <param name="id">The place identifier.</param>
		/// <returns>Returns <c>True</c> if place was deleted from repository; <c>False</c> otherwise.</returns>
		public async Task<bool> DeleteAsync(int id)
		{
			try
			{
				return await this.DeleteAsync(this.Get(id));
			}
			catch (Exception)
			{
				return false;
			}
		}

		/// <summary>
		/// Deletes the place.
		/// </summary>
		/// <param name="model">The model.</param>
		/// <returns>Returns <c>True</c> if place was deleted from repository; <c>False</c> otherwise.</returns>
		/// <exception cref="ArgumentNullException">model</exception>
		/// <exception cref="ArgumentOutOfRangeException">Place identifier must not be zero.</exception>
		public async Task<bool> DeleteAsync(Place model)
		{
			if (model == null)
				throw new ArgumentNullException(nameof(model));
			if (model.Id <= 0)
				throw new ArgumentOutOfRangeException(nameof(model.Id), "Place identifier must not be zero.");

			try
			{
				this.logger.Information("Deleting place ({PlaceId})...", model.Id);

				this.context.Places.Remove(model);
				await this.context.SaveChangesAsync();
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		/// <summary>
		/// Validates the model.
		/// </summary>
		/// <param name="model">The model.</param>
		/// <exception cref="ArgumentNullException">model</exception>
		/// <exception cref="InvalidDataException">
		/// Place must contain reference to country.
		/// or
		/// Place name must not be null or empty.
		/// </exception>
		private static void ValidateModel(Place model)
		{
			if (model == null)
				throw new ArgumentNullException(nameof(model));

			// Validate that model has country assigned
			if (model.CountryId != 0 && model.Country == null)
				throw new InvalidDataException("Place must contain reference to country.");

			// Validate country name
			if (string.IsNullOrWhiteSpace(model.Name))
				throw new InvalidDataException("Place name must not be null or empty.");
		}
	}
}
