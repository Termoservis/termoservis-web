using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Termoservis.Contracts.Models;

namespace Termoservis.Models
{
    /// <summary>
	/// The address model.
	/// </summary>
	/// <seealso cref="ISearchable" />
	public class Address : ISearchable
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
		/// Gets or sets the street address.
		/// </summary>
		/// <value>
		/// The street address.
		/// </value>
		[Required]
		[DisplayName("Ulica i kućni broj")]
		public string StreetAddress { get; set; }

		/// <summary>
		/// Gets or sets the place identifier.
		/// </summary>
		/// <value>
		/// The place identifier.
		/// </value>
		[DisplayName("Mjesto")]
		public int? PlaceId { get; set; }

		/// <summary>
		/// Gets or sets the place.
		/// </summary>
		/// <value>
		/// The place.
		/// </value>
		[ForeignKey(nameof(PlaceId))]
		[DisplayName("Mjesto")]
		public virtual Place Place { get; set; }

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