using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Termoservis.Contracts.Models;

namespace Termoservis.Models
{
	/// <summary>
	/// The place model.
	/// </summary>
	/// <seealso cref="ISearchable" />
	public class Place : ISearchable
	{
		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		/// <value>
		/// The identifier.
		/// </value>
		[Key]
		[Required]
		public int Id { get; set; }

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		[Required]
		[DisplayName("Mjesto")]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the country identifier.
		/// </summary>
		/// <value>
		/// The country identifier.
		/// </value>
		[Required]
		[DisplayName("Država")]
		public int CountryId { get; set; }

		/// <summary>
		/// Gets or sets the country.
		/// </summary>
		/// <value>
		/// The country.
		/// </value>
		[ForeignKey(nameof(CountryId))]
		[DisplayName("Država")]
		public virtual Country Country { get; set; }

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