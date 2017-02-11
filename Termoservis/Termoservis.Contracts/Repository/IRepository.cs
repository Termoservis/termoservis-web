using System.Threading.Tasks;

namespace Termoservis.Contracts.Repository
{
	/// <summary>
	/// The repository contract.
	/// </summary>
	/// <typeparam name="TModel">The type of the model.</typeparam>
	/// <typeparam name="TModelIdentifier">The type of the model identifier.</typeparam>
	public interface IRepository<TModel, in TModelIdentifier> : 
		IEditRepository<TModel, TModelIdentifier>,
		IDeleteRepository<TModel, TModelIdentifier>
		where TModelIdentifier : struct
	{
	}
}