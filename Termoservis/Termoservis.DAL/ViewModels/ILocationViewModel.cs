﻿using System.Collections.Generic;
using Termoservis.Models;

namespace Termoservis.DAL.ViewModels
{
	/// <summary>
	/// Location view model contract.
	/// </summary>
	public interface ILocationViewModel
	{
		/// <summary>
		/// Gets or sets the available places.
		/// </summary>
		/// <value>
		/// The available places.
		/// </value>
		IEnumerable<Place> AvailablePlaces { get; set; }

		/// <summary>
		/// Gets or sets the available countries.
		/// </summary>
		/// <value>
		/// The available countries.
		/// </value>
		IEnumerable<Country> AvailableCountries { get; set; }
	}
}