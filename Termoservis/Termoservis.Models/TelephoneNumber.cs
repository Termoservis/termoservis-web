using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Termoservis.Contracts.Models;

namespace Termoservis.Models
{
	/// <summary>
	/// The telephone number model.
	/// </summary>
	/// <seealso cref="ISearchable" />
	public class TelephoneNumber : ISearchable
	{
		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		/// <value>
		/// The identifier.
		/// </value>
		[Key]
		public long Id { get; set; }

		/// <summary>
		/// Gets or sets the number.
		/// </summary>
		/// <value>
		/// The number.
		/// </value>
		[DisplayName("Broj telefona")]
		public string Number { get; set; }

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