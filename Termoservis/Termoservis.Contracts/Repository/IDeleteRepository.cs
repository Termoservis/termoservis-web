using System.Threading.Tasks;

namespace Termoservis.Contracts.Repository
{
	/// <summary>
	/// The repository with delete support.
	/// </summary>
	/// <typeparam name="TModel">The type of the model.</typeparam>
	/// <typeparam name="TModelIdentifier">The type of the model identifier.</typeparam>
	public interface IDeleteRepository<in TModel, in TModelIdentifier>
		where TModelIdentifier : struct
	{
		/// <summary>
		/// Deletes the model with specified identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns>Returns <c>True</c> if model with specified identifier was deleted successfully; <c>False</c> otherwise.</returns>
		Task<bool> DeleteAsync(TModelIdentifier id);

		/// <summary>
		/// Deletes the specified.
		/// </summary>
		/// <param name="model">The model.</param>
		/// <returns>Returns <c>True</c> if specified model was deleted successfully; <c>False</c> otherwise.</returns>
		Task<bool> DeleteAsync(TModel model);

        /// <summary>
        /// Saves the changes.
        /// </summary>
        Task Save();
    }
}