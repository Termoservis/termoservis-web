using System;
using System.Threading.Tasks;
using Termoservis.Contracts.Repository;

namespace Termoservis.Common.Extensions
{
	/// <summary>
	/// The <see cref="IEditRepository{TModel,TModelIdentifier}"/> extensions.
	/// </summary>
	public static class EditRepositoryExtensions
	{
		/// <summary>
		/// Adds the or retrieves model with specified identifier.
		/// </summary>
		/// <typeparam name="TModel">The type of the model.</typeparam>
		/// <typeparam name="TModelIdentifier">The type of the model identifier.</typeparam>
		/// <param name="repository">The repository.</param>
		/// <param name="model">The model.</param>
		/// <param name="id">The identifier.</param>
		/// <returns>Returns model that matches specified identifier.</returns>
		/// <exception cref="System.ArgumentNullException">
		/// repository
		/// or
		/// model
		/// </exception>
		public static async Task<TModel> AddOrGetAsync<TModel, TModelIdentifier>(
			this IEditRepository<TModel, TModelIdentifier> repository,
			TModel model, TModelIdentifier id)
			where TModelIdentifier : struct
		{
			if (repository == null) throw new ArgumentNullException(nameof(repository));
			if (model == null) throw new ArgumentNullException(nameof(model));

			if (repository.Exists(id))
				return repository.Get(id);
			return await repository.AddAsync(model);
		}
	}
}