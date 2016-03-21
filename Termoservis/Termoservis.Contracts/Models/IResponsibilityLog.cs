using System;

namespace Termoservis.Contracts.Models
{
	/// <summary>
	/// The model responsibility log contract.
	/// </summary>
	public interface IResponsibilityLog
	{
		/// <summary>
		/// Gets or sets the application user identifier.
		/// </summary>
		/// <value>
		/// The application user identifier.
		/// </value>
		string ApplicationUserId { get; set; }

		/// <summary>
		/// Gets or sets the creation date.
		/// </summary>
		/// <value>
		/// The creation date.
		/// </value>
		DateTime CreationDate { get; set; }
	}
}