using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Termoservis.Contracts.Models;

namespace Termoservis.Models
{
	/// <summary>
	/// The country model.
	/// </summary>
	/// <seealso cref="ISearchable" />
	public class Country : ISearchable
	{
		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		/// <value>
		/// The identifier.
		/// </value>
		[Key]
		public int Id { get; set; }

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		[Required]
		[DisplayName("Država")]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the search keywords.
		/// </summary>
		/// <value>
		/// The search keywords.
		/// </value>
		[Required]
		public string SearchKeywords { get; set; }
	}
}