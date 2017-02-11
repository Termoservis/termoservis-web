using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using Termoservis.Contracts.Models;

namespace Termoservis.Models
{
	/// <summary>
	/// The customer model.
	/// </summary>
	/// <seealso cref="ISearchable" />
	/// <seealso cref="IResponsibilityLog" />
	public class Customer : ISearchable, IResponsibilityLog
	{
		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		/// <value>
		/// The identifier.
		/// </value>
		[Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[HiddenInput(DisplayValue = false)]
		public long Id { get; set; }

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		[Required]
		[DisplayName("Naziv")]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the note.
		/// </summary>
		/// <value>
		/// The note.
		/// </value>
		[DisplayName("Bilješka")]
		public string Note { get; set; }

		/// <summary>
		/// Gets or sets the email.
		/// </summary>
		/// <value>
		/// The email.
		/// </value>
		[DisplayName("Email")]
		[EmailAddress]
		public string Email { get; set; }

		/// <summary>
		/// Gets or sets the telephone numbers.
		/// </summary>
		/// <value>
		/// The telephone numbers.
		/// </value>
		[DisplayName("Telefonski brojevi")]
		public virtual ICollection<TelephoneNumber> TelephoneNumbers { get; set; }

		/// <summary>
		/// Gets or sets the address identifier.
		/// </summary>
		/// <value>
		/// The address identifier.
		/// </value>
		[Required]
		[DisplayName("Ulica i kućni broj")]
		public int AddressId { get; set; }

		/// <summary>
		/// Gets or sets the address.
		/// </summary>
		/// <value>
		/// The address.
		/// </value>
		[ForeignKey(nameof(AddressId))]
		[DisplayName("Ulica i kućni broj")]
		public virtual Address Address { get; set; }

        /// <summary>
        /// Gets or sets the work items.
        /// </summary>
        /// <value>
        /// The work items.
        /// </value>
        [DisplayName("Stavke")]
        public virtual ICollection<WorkItem> WorkItems { get; set; }

        /// <summary>
        /// Gets or sets the customer devices.
        /// </summary>
        /// <value>
        /// The customer devices.
        /// </value>
        [DisplayName("Uredaji")]
        public virtual ICollection<CustomerDevice> CustomerDevices { get; set; }

		/// <summary>
		/// Gets or sets the search keywords.
		/// </summary>
		/// <value>
		/// The search keywords.
		/// </value>
		[Required]
		public string SearchKeywords { get; set; }

		/// <summary>
		/// Gets or sets the application user identifier.
		/// </summary>
		/// <value>
		/// The application user identifier.
		/// </value>
		[Required]
		public string ApplicationUserId { get; set; }

		/// <summary>
		/// Gets or sets the application user.
		/// </summary>
		/// <value>
		/// The application user.
		/// </value>
		[ForeignKey(nameof(ApplicationUserId))]
		public virtual ApplicationUser ApplicationUser { get; set; }

		/// <summary>
		/// Gets or sets the creation date.
		/// </summary>
		/// <value>
		/// The creation date.
		/// </value>
		[Required]
		[DisplayName("Datum stvaranja")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
		public DateTime CreationDate { get; set; }
	}
}