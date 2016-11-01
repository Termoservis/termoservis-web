using System.Linq;

namespace Termoservis.Contracts.Repository
{
	/// <summary>
	/// The read-only repository contract.
	/// </summary>
	/// <typeparam name="TModel">The type of the model.</typeparam>
	/// <typeparam name="TModelIdentifier">The type of the model identifier.</typeparam>
	public interface IRetrieveRepository<out TModel, in TModelIdentifier>
	{
		/// <summary>
		/// Gets all models from repository.
		/// </summary>
		/// <returns>Returns query for all models in the repository.</returns>
		IQueryable<TModel> GetAll();

			/// <summary>
		/// Gets the model by specified identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns>Returns model with specified identifier; return null if not found.</returns>
		TModel Get(TModelIdentifier id);
	}
}