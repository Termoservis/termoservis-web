using System.Threading.Tasks;

namespace Termoservis.Contracts.Repository
{
	/// <summary>
	/// The repository with support for adding elements.
	/// </summary>
	/// <typeparam name="TModel">The type of the model.</typeparam>
	/// <typeparam name="TModelIdentifier">The type of the model identifier.</typeparam>
	/// <seealso cref="Termoservis.Contracts.Repository.IRetrieveRepository{TModel, TModelIdentifier}" />
	public interface IAddRepository<TModel, in TModelIdentifier> : IRetrieveRepository<TModel, TModelIdentifier>
	{
		/// <summary>
		/// Adds the specified model to the repository..
		/// </summary>
		/// <param name="model">The model.</param>
		Task<TModel> AddAsync(TModel model);

        /// <summary>
        /// Saves the changes.
        /// </summary>
        Task Save();
    }
}