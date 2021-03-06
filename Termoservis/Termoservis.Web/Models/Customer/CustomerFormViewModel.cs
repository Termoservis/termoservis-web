﻿using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Termoservis.DAL.ViewModels;
using Termoservis.Models;

namespace Termoservis.Web.Models.Customer
{
    /// <summary>
	/// Customer form view model.
	/// </summary>
	/// <seealso cref="ILocationViewModel" />
	public class CustomerFormViewModel : Termoservis.Models.Customer, ILocationViewModel, IFormViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerFormViewModel"/> class.
        /// </summary>
        public CustomerFormViewModel()
	    {
	    }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerFormViewModel"/> class.
        /// </summary>
        /// <param name="actionName">Name of the action.</param>
        public CustomerFormViewModel(string actionName)
	    {
	        this.ActionName = actionName;
	    }


        /// <summary>
        /// Gets or sets the name of the action.
        /// </summary>
        /// <value>
        /// The name of the action.
        /// </value>
        public string ActionName { get; set; }

		/// <summary>
		/// Gets or sets the available countries.
		/// </summary>
		/// <value>
		/// The available countries.
		/// </value>
		public IEnumerable<Country> AvailableCountries { get; set; } = new List<Country>();

		/// <summary>
		/// Gets or sets the name of the street.
		/// </summary>
		/// <value>
		/// The name of the customer street.
		/// </value>
		[Required(AllowEmptyStrings = false)]
		[DisplayName("Adresa")]
		public string CustomerStreetName { get; set; }

		/// <summary>
		/// Gets or sets the place identifier.
		/// </summary>
		/// <value>
		/// The customer place identifier.
		/// </value>
		[DisplayName("Mjesto")]
		public int? CustomerPlaceId { get; set; }

        /// <summary>
        /// Gets or sets the name of the customer place.
        /// </summary>
        /// <value>
        /// The name of the customer place.
        /// </value>
        [DisplayName("Mjesto")]
        public string CustomerPlaceName { get; set; }

		/// <summary>
		/// Gets or sets the country identifier.
		/// </summary>
		/// <value>
		/// The customer country identifier.
		/// </value>
		[DisplayName("Država")]
		public int? CustomerCountryId { get; set; }
	}
}