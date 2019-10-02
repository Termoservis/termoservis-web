using System.Collections.Generic;

namespace Termoservis.Web.Models.Customer
{
    /// <summary>
    /// The customers search result.
    /// </summary>
    public class CustomersSearchResult
    {
        /// <summary>
        /// Gets or sets the customers.
        /// </summary>
        /// <value>
        /// The customers.
        /// </value>
        public List<Termoservis.Models.Customer> Customers { get; set; }

        /// <summary>
        /// Gets or sets the total pages.
        /// </summary>
        /// <value>
        /// The total pages.
        /// </value>
        public int TotalPages { get; set; }

        /// <summary>
        /// Gets or sets the current page. Zero is first page.
        /// </summary>
        /// <value>
        /// The current page.
        /// </value>
        public int CurrentPage { get; set; }

        /// <summary>
        /// Gets or sets the keywords.
        /// </summary>
        /// <value>
        /// The keywords.
        /// </value>
        public string Keywords { get; set; }
    }
}