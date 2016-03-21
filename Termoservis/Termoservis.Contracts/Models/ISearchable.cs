namespace Termoservis.Contracts.Models
{
	/// <summary>
	/// The searchable model contract.
	/// </summary>
	public interface ISearchable
	{
		/// <summary>
		/// Gets or sets the search keywords.
		/// </summary>
		/// <value>
		/// The search keywords.
		/// </value>
		string SearchKeywords { get; set; }
	}
}