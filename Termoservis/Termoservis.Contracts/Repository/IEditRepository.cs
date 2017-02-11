using System.Threading.Tasks;

namespace Termoservis.Contracts.Repository
{
	/// <summary>
	/// The editable repository contract.
	/// </summary>
	/// <typeparam name="TModel">The type of the model.</typeparam>
	/// <typeparam name="TModelIdentifier">The type of the model identifier.</typeparam>
	public interface IEditRepository<TModel, in TModelIdentifier> : IAddRepository<TModel, TModelIdentifier>
	{
		/// <summary>
		/// Edits the model with specified identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="model">The model.</param>
		Task<TModel> EditAsync(TModelIdentifier id, TModel model);
    }
}