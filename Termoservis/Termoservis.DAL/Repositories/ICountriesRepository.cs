using Termoservis.Contracts.Repository;
using Termoservis.Models;

namespace Termoservis.DAL.Repositories
{
	/// <summary>
	/// The <see cref="Country"/> repository contract.
	/// </summary>
	public interface ICountriesRepository : IRepository<Country, int>
	{
	}
}