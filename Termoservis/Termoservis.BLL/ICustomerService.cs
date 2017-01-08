using System.Collections.Generic;
using System.Threading.Tasks;
using Termoservis.Models;

namespace Termoservis.BLL
{
    /// <summary>
    /// The customer service.
    /// </summary>
    public interface ICustomerService
    {
        /// <summary>
        /// Creates the customer.
        /// </summary>
        /// <param name="customerModel">The customer model.</param>
        /// <param name="streetName">Name of the street.</param>
        /// <param name="placeId">The place identifier.</param>
        /// <param name="telephoneNumbers">The telephone numbers.</param>
        /// <param name="user">The user.</param>
        /// <returns>Returns the created customer model.</returns>
        Task<Customer> CreateCustomerAsync(
            Customer customerModel,
            string streetName,
            int placeId,
            IEnumerable<TelephoneNumber> telephoneNumbers,
            ApplicationUser user);

        /// <summary>
        /// Edits the customer.
        /// </summary>
        /// <param name="customerModel">The customer model.</param>
        /// <param name="streetName">Name of the street.</param>
        /// <param name="placeId">The place identifier.</param>
        /// <param name="telephoneNumbers">The telephone numbers.</param>
        /// <returns>Returns the edited customer model.</returns>
        Task<Customer> EditCustomerAsync(
            Customer customerModel, 
            string streetName, 
            int placeId, 
            IEnumerable<TelephoneNumber> telephoneNumbers);
    }
}