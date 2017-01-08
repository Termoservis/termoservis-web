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
	/// The <see cref="TelephoneNumber"/> repository.
	/// </summary>
	/// <seealso cref="Termoservis.DAL.Repositories.ITelephoneNumbersRepository" />
	public class TelephoneNumbersRepository : ITelephoneNumbersRepository
	{
		private readonly ApplicationDbContext context;
		private readonly ILogger logger;


		/// <summary>
		/// Initializes a new instance of the <see cref="TelephoneNumbersRepository"/> class.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="loggingService">The logging service.</param>
		/// <exception cref="ArgumentNullException">
		/// context
		/// or
		/// loggingService
		/// </exception>
		public TelephoneNumbersRepository(
			ApplicationDbContext context,
			ILoggingService loggingService)
		{
			if (context == null)
				throw new ArgumentNullException(nameof(context));
			if (loggingService == null)
				throw new ArgumentNullException(nameof(loggingService));

			this.context = context;
			this.logger = loggingService.GetLogger<TelephoneNumbersRepository>();
		}


		/// <summary>
		/// Gets all models from repository.
		/// </summary>
		/// <returns>
		/// Returns query for all models in the repository.
		/// </returns>
		public IQueryable<TelephoneNumber> GetAll()
		{
			return this.context.TelephoneNumbers;
		}

		/// <summary>
		/// Gets the model by specified identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns>
		/// Returns model with specified identifier; returns null if not found.
		/// </returns>
		/// <exception cref="ArgumentOutOfRangeException">Telephone number identifier must not be zero.</exception>
		public TelephoneNumber Get(long id)
		{
			if (id <= 0)
				throw new ArgumentOutOfRangeException(nameof(id), "Telephone number identifier must not be zero.");

			return this.GetAll().FirstOrDefault(telephoneNumber => telephoneNumber.Id == id);
		}

		/// <summary>
		/// Adds the telephone number to the repository.
		/// </summary>
		/// <param name="model">The model.</param>
		/// <returns>
		/// Returns the telephone number instance that was added to the repository.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		/// model
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Telephone number identifier must not be zero.
		/// </exception>
		/// <exception cref="InvalidDataException">
		/// Telephone number must not be empty.
		/// </exception>
		public async Task<TelephoneNumber> AddAsync(TelephoneNumber model)
		{
			if (model == null)
				throw new ArgumentNullException(nameof(model));
			if (model.Id != 0)
				throw new ArgumentOutOfRangeException(nameof(model.Id), "Telephone number identifier must be zero.");

		    model.Number = model.Number?.Replace(" ", "").Replace("+", "00").Trim();

			// Validate
			this.ValidateModel(model);

		    model.SearchKeywords = GetSearchKeywords(model);

			// Add to the repository and save
			this.context.TelephoneNumbers.Add(model);
			await this.context.SaveChangesAsync();

			this.logger.Information(
				"Added new telephone number {TelephoneNumber}", 
				model.Number);

			return model;
		}

        /// <summary>
        /// Edits the telephone number with specified identifier with given model data.
        /// </summary>
        /// <param name="id">The telephone number identifier to edit.</param>
        /// <param name="model">The telephone number model with new data.</param>
        /// <returns>Returns the edited telephone number instance.</returns>
        /// <exception cref="System.ArgumentNullException">model</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Id - Telephone number identifier must not be zero.</exception>
        public async Task<TelephoneNumber> EditAsync(long id, TelephoneNumber model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));
            if (id == 0)
                throw new ArgumentOutOfRangeException(nameof(model.Id), "Telephone number identifier must not be zero.");

            model.Number = model.Number?.Replace(" ", "").Replace("+", "00").Trim();

            // Validate
            this.ValidateModel(model);

            model.SearchKeywords = GetSearchKeywords(model);

            // Edit the number from repository
            var telephoneNumberDb = this.Get(id);
            telephoneNumberDb.SearchKeywords = model.SearchKeywords;
            telephoneNumberDb.Number = model.Number;
            await this.context.SaveChangesAsync();

            this.logger.Information(
                "Edited telephone number {TelephoneNumber} ({TelephoneNumberId})",
                telephoneNumberDb.Number, telephoneNumberDb.Id);

            return telephoneNumberDb;
        }

        /// <summary>
        /// Gets the search keywords for specified telephone number.
        /// </summary>
        /// <param name="telephoneNumber">The telephone number.</param>
        /// <returns>Returns the search keywords string.</returns>
        private static string GetSearchKeywords(TelephoneNumber telephoneNumber)
	    {
	        return telephoneNumber.Number.Aggregate(string.Empty, (s, c) => s + (char.IsDigit(c) ? c.ToString() : "")).Trim();
	    }

        /// <summary>
        /// Validates the model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <exception cref="ArgumentNullException">
        /// model
        /// </exception>
        /// <exception cref="InvalidDataException">
        /// Telephone number must not be empty.
        /// </exception>
        private void ValidateModel(TelephoneNumber model)
		{
			if (model == null)
				throw new ArgumentNullException(nameof(model));

			if (string.IsNullOrWhiteSpace(model.Number))
				throw new InvalidDataException("Telephone number must not be empty.");
		}

        /// <summary>
		/// Deletes the telephone number.
		/// </summary>
		/// <param name="id">The telephone number identifier.</param>
		/// <returns>Returns <c>True</c> if telephone number was deleted from repository; <c>False</c> otherwise.</returns>
	    public async Task<bool> DeleteAsync(long id)
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
        /// Deletes the telephone number.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Returns <c>True</c> if telephone number was deleted from repository; <c>False</c> otherwise.</returns>
        /// <exception cref="System.ArgumentNullException">model</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Id - Telephone number identifier must not be zero.</exception>
        public async Task<bool> DeleteAsync(TelephoneNumber model)
	    {
            if (model == null)
                throw new ArgumentNullException(nameof(model));
            if (model.Id <= 0)
                throw new ArgumentOutOfRangeException(nameof(model.Id), "Telephone number identifier must not be zero.");

            try
	        {
                this.logger.Information("Deleting telephone number {TelephoneNumber} ({TelephoneNumberId})...", model.Number, model.Id);

	            this.context.TelephoneNumbers.Remove(model);
	            await this.context.SaveChangesAsync();
	            return true;
	        }
	        catch (Exception)
	        {
	            return false;
	        }
	    }
	}
}