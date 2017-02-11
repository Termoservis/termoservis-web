using System;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using Termoservis.Contracts.Services;
using Termoservis.Models;

namespace Termoservis.DAL.Repositories
{
	/// <summary>
	/// The <see cref="Country"/> repository.
	/// </summary>
	/// <seealso cref="ICountriesRepository" />
	// ReSharper disable once UnusedMember.Global
	public class CountriesRepository : ICountriesRepository
	{
		private readonly ApplicationDbContext context;
		private readonly ILogger logger;


		/// <summary>
		/// Initializes a new instance of the <see cref="CountriesRepository"/> class.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="loggingService">The logging service.</param>
		/// <exception cref="System.ArgumentNullException">
		/// context
		/// or
		/// loggingService
		/// </exception>
		public CountriesRepository(ApplicationDbContext context, ILoggingService loggingService)
		{
			if (context == null) throw new ArgumentNullException(nameof(context));
			if (loggingService == null) throw new ArgumentNullException(nameof(loggingService));

			this.context = context;
			this.logger = loggingService.GetLogger<CountriesRepository>();
		}


		/// <summary>
		/// Gets all countries from repository..
		/// </summary>
		/// <returns>Returns collection of countries from repository.</returns>
		public IQueryable<Country> GetAll()
		{
			return this.context.Countries.AsQueryable();
		}

		/// <summary>
		/// Gets the country with specified identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns>Returns the country that matches specified identifier; <c>null</c> if no such country exists in repository.</returns>
		public Country Get(int id)
		{
			return this.context.Countries.FirstOrDefault(country => country.Id == id);
		}

		/// <summary>
		/// Adds the country to the repository.
		/// </summary>
		/// <param name="model">The model.</param>
		/// <returns>Returns country instance that was added to the repository.</returns>
		/// <exception cref="ArgumentNullException">model</exception>
		/// <exception cref="ArgumentOutOfRangeException">Country identifier must be zero.</exception>
		public async Task<Country> AddAsync(Country model)
		{
			if (model == null)
				throw new ArgumentNullException(nameof(model));
			if (model.Id != 0)
				throw new ArgumentOutOfRangeException(nameof(model.Id), "Country identifier must be zero.");

			this.context.Countries.Add(model);
			await this.context.SaveChangesAsync();

			this.logger.Information(
				"Added new country {CountryName} ({CountryId})", 
				model.Name, model.Id);

			return model;
		}

		/// <summary>
		/// Edits the country with specified identifier.
		/// </summary>
		/// <param name="id">The country identifier.</param>
		/// <param name="model">The model.</param>
		/// <returns>Returns the edited country instance.</returns>
		/// <exception cref="ArgumentNullException">model</exception>
		/// <exception cref="ArgumentOutOfRangeException">Country identifier must not be zero.</exception>
		public async Task<Country> EditAsync(int id, Country model)
		{
			if (model == null)
				throw new ArgumentNullException(nameof(model));
			if (id <= 0)
				throw new ArgumentOutOfRangeException(nameof(id), "Country identifier must not be zero.");

			// Retrieve from database
			var countryDb = this.Get(id);

			// Apply edit
			countryDb.Name = model.Name;
			
			// Save changes
			await this.context.SaveChangesAsync();

			this.logger.Information(
				"Edited country {CountryName} ({CountryId})",
				countryDb.Name, countryDb.Id);

			return countryDb;
		}

		/// <summary>
		/// Deletes the country.
		/// </summary>
		/// <param name="id">The country identifier.</param>
		/// <returns>Returns <c>True</c> if country was deleted from database; <c>False</c> otherwise.</returns>
		public async Task<bool> DeleteAsync(int id)
		{
			try
			{
				return await this.DeleteAsync(this.Get(id));
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// Deletes the country.
		/// </summary>
		/// <param name="model">The model.</param>
		/// <returns>Returns <c>True</c> if country was deleted from database; <c>False</c> otherwise.</returns>
		/// <exception cref="ArgumentNullException">model</exception>
		/// <exception cref="ArgumentOutOfRangeException">Country identifier must not be zero.</exception>
		public async Task<bool> DeleteAsync(Country model)
		{
			if (model == null)
				throw new ArgumentNullException(nameof(model));
			if (model.Id <= 0)
				throw new ArgumentOutOfRangeException(nameof(model.Id), "Country identifier must not be zero.");

			try
			{
				this.logger.Information("Deleting country {CountryId}...", model.Id);

				this.context.Countries.Remove(model);
				await this.context.SaveChangesAsync();
				return true;
			}
			catch
			{
				return false;
			}
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