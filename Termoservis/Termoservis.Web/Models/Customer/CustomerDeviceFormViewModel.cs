using System;
using Termoservis.Models;

namespace Termoservis.Web.Models.Customer
{
    /// <summary>
    /// The customer device form view model.
    /// </summary>
    /// <seealso cref="IFormViewModel" />
    public class CustomerDeviceFormViewModel : IFormViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerDeviceFormViewModel"/> class.
        /// </summary>
        public CustomerDeviceFormViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerDeviceFormViewModel"/> class.
        /// </summary>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="customerId">The customer identifier.</param>
        /// <param name="device">The device.</param>
        /// <exception cref="System.ArgumentNullException">
        /// actionName
        /// or
        /// device
        /// </exception>
        public CustomerDeviceFormViewModel(string actionName, long customerId, CustomerDevice device)
        {
            if (customerId <= 0) throw new ArgumentOutOfRangeException(nameof(customerId));
            if (string.IsNullOrWhiteSpace(actionName))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(actionName));

            this.CustomerId = customerId;
            this.ActionName = actionName;
            this.Device = device ?? throw new ArgumentNullException(nameof(device));
        }

        /// <summary>
        /// Gets or sets the customer identifier.
        /// </summary>
        /// <value>
        /// The customer identifier.
        /// </value>
        public long CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the device.
        /// </summary>
        /// <value>
        /// The device.
        /// </value>
        public CustomerDevice Device { get; set; } = new CustomerDevice();

        /// <summary>
        /// Gets or sets the name of the action.
        /// </summary>
        /// <value>
        /// The name of the action.
        /// </value>
        public string ActionName { get; set; }
    }
}
