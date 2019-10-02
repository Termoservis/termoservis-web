using System;
using Termoservis.Contracts.Repository;

namespace Termoservis.Common.Extensions
{
	/// <summary>
	/// The <see cref="IRetrieveRepository{TModel, TModelIdentifier}"/> extensions.
	/// </summary>
	public static class RetrieveRepositoryExtensions
	{
		/// <summary>
		/// Determines whether model with specified identifier exists in repository.
		/// </summary>
		/// <typeparam name="TModel">The type of the model.</typeparam>
		/// <typeparam name="TModelIdentifier">The type of the model identifier.</typeparam>
		/// <param name="repository">The repository.</param>
		/// <param name="id">The identifier.</param>
		/// <returns>Returns <c>True</c> if model with specified identifier exists in repository; <c>False</c> otherwise.</returns>
		/// <exception cref="ArgumentNullException">repository</exception>
		public static bool Exists<TModel, TModelIdentifier>(this IRetrieveRepository<TModel, TModelIdentifier> repository, TModelIdentifier id) 
			where TModelIdentifier : struct 
		{
			if (repository == null) throw new ArgumentNullException(nameof(repository));

			return repository.Get(id) != null;
		}
	}
}
