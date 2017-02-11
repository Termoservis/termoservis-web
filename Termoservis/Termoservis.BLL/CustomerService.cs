using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;
using Termoservis.DAL.Repositories;
using Termoservis.Models;

namespace Termoservis.BLL
{
    /// <summary>
    /// The customer service.
    /// </summary>
    /// <seealso cref="ICustomerService" />
    public class CustomerService : ICustomerService
    {
        private readonly ICustomersRepository customersRepository;
        private readonly ITelephoneNumbersRepository telephoneNumbersRepository;
        private readonly ICustomerDevicesRepository customerDevicesRepository;
        private readonly IAddressesRepository addressesRepository;


        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerService"/> class.
        /// </summary>
        /// <param name="addressesRepository">The addresses repository.</param>
        /// <param name="customersRepository">The customers repository.</param>
        /// <param name="telephoneNumbersRepository">The telephone numbers repository.</param>
        /// <param name="customerDevicesRepository">The customer devices repository.</param>
        /// <exception cref="System.ArgumentNullException">
        /// addressesRepository
        /// or
        /// customersRepository
        /// or
        /// telephoneNumbersRepository
        /// </exception>
        public CustomerService(IAddressesRepository addressesRepository, ICustomersRepository customersRepository, ITelephoneNumbersRepository telephoneNumbersRepository, ICustomerDevicesRepository customerDevicesRepository)
        {
            if (addressesRepository == null) throw new ArgumentNullException(nameof(addressesRepository));
            if (customersRepository == null) throw new ArgumentNullException(nameof(customersRepository));
            if (telephoneNumbersRepository == null) throw new ArgumentNullException(nameof(telephoneNumbersRepository));
            if (customerDevicesRepository == null) throw new ArgumentNullException(nameof(customerDevicesRepository));

            this.customersRepository = customersRepository;
            this.telephoneNumbersRepository = telephoneNumbersRepository;
            this.customerDevicesRepository = customerDevicesRepository;
            this.addressesRepository = addressesRepository;
        }


        /// <summary>
        /// Creates the customer.
        /// </summary>
        /// <param name="customerModel">The customer model.</param>
        /// <param name="streetName">Name of the street.</param>
        /// <param name="placeId">The place identifier.</param>
        /// <param name="telephoneNumbers">The telephone numbers.</param>
        /// <param name="user">The user.</param>
        /// <returns>Returns the created customer model.</returns>
        /// <exception cref="System.ArgumentNullException">customerModel</exception>
        /// <exception cref="System.ArgumentException">Value cannot be null or empty. - streetName</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">placeId</exception>
        public async Task<Customer> CreateCustomerAsync(
            Customer customerModel, 
            string streetName, 
            int placeId,
            IEnumerable<TelephoneNumber> telephoneNumbers,
            ApplicationUser user)
        {
            if (customerModel == null) throw new ArgumentNullException(nameof(customerModel));
            if (string.IsNullOrEmpty(streetName)) throw new ArgumentException("Value cannot be null or empty.", nameof(streetName));
            if (placeId <= 0) throw new ArgumentOutOfRangeException(nameof(placeId));

            // Ensure all telephone number are created and have Id's
            var telephoneNumbersList = telephoneNumbers?.ToList() ?? new List<TelephoneNumber>();
            foreach (var telephoneNumber in telephoneNumbersList.Where(t => !string.IsNullOrWhiteSpace(t.Number)))
                await this.telephoneNumbersRepository.AddAsync(telephoneNumber);

            // Endure address had Id
            var address = await addressesRepository.EnsureExistsAsync(streetName, placeId);

            // Populate model
            customerModel.TelephoneNumbers = telephoneNumbersList.Where(t => !string.IsNullOrWhiteSpace(t.Number)).ToList();
            customerModel.AddressId = address.Id;
            customerModel.ApplicationUserId = user.Id;

            // Create the customer
            return await this.customersRepository.AddAsync(customerModel);
        }

        /// <summary>
        /// Edits the customer.
        /// </summary>
        /// <param name="customerModel">The customer model.</param>
        /// <param name="streetName">Name of the street.</param>
        /// <param name="placeId">The place identifier.</param>
        /// <param name="telephoneNumbers">The telephone numbers.</param>
        /// <returns>Returns the edited customer model.</returns>
        /// <exception cref="System.ArgumentNullException">customerModel</exception>
        /// <exception cref="System.ArgumentException">Value cannot be null or empty. - streetName</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">placeId</exception>
        public async Task<Customer> EditCustomerAsync(
            Customer customerModel, 
            string streetName, 
            int placeId, 
            IEnumerable<TelephoneNumber> telephoneNumbers)
        {
            if (customerModel == null) throw new ArgumentNullException(nameof(customerModel));
            if (string.IsNullOrEmpty(streetName)) throw new ArgumentException("Value cannot be null or empty.", nameof(streetName));
            if (placeId <= 0) throw new ArgumentOutOfRangeException(nameof(placeId));
            
            // Create telephone numbers
            var telephoneNumbersList = telephoneNumbers?.Where(t => !string.IsNullOrWhiteSpace(t.Number)).ToList() ?? new List<TelephoneNumber>();
            foreach (var telephoneNumber in telephoneNumbersList.Where(t => string.IsNullOrWhiteSpace(t.SearchKeywords)))
                await this.telephoneNumbersRepository.AddAsync(telephoneNumber);
            
            // Assign telephone number id's
            var customerDb = this.customersRepository.Get(customerModel.Id);
            foreach (var telephoneNumber in telephoneNumbersList)
            {
                var matchedNumber = customerDb.TelephoneNumbers.FirstOrDefault(t =>
                    t.SearchKeywords == telephoneNumber.SearchKeywords);
                if (matchedNumber == null)
                    continue;

                telephoneNumber.Id = matchedNumber.Id;
            }

            // Recalculate telephone numbers search keywords for existing entities
            foreach (var telephoneNumber in telephoneNumbersList.Where(t => t.Id != 0))
                await telephoneNumbersRepository.EditAsync(
                    telephoneNumber.Id,
                    telephoneNumber);

            // Make sure address had Id
            var address = await addressesRepository.EnsureExistsAsync(streetName, placeId);
            
            // Populate customer model
            customerModel.TelephoneNumbers = telephoneNumbersList;
            customerModel.AddressId = address.Id;
            customerModel.Address = address;

            // Edit the customer
            return await this.customersRepository.EditAsync(customerModel.Id, customerModel);
        }

        /// <summary>
        /// Creates the new customer device.
        /// </summary>
        /// <param name="customerModel">The customer model.</param>
        /// <param name="deviceName">Name of the device.</param>
        /// <param name="deviceManufacturer">The device manufacturer.</param>
        /// <param name="deviceCommisionDate">The device commision date.</param>
        /// <returns>
        /// Returns the create customer device model.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">customerModel</exception>
        /// <exception cref="System.ArgumentException">
        /// Value cannot be null or whitespace. - deviceName
        /// or
        /// Value cannot be null or whitespace. - deviceManufacturer
        /// </exception>
        public async Task<CustomerDevice> CreateNewCustomerDeviceAsync(
            Customer customerModel, 
            string deviceName, 
            string deviceManufacturer,
            DateTime? deviceCommisionDate)
        {
            if (customerModel == null) throw new ArgumentNullException(nameof(customerModel));
            if (string.IsNullOrWhiteSpace(deviceName))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(deviceName));
            if (string.IsNullOrWhiteSpace(deviceManufacturer))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(deviceManufacturer));
            
            // Date database fix
            if (deviceCommisionDate.HasValue)
            {
                if (deviceCommisionDate.Value < SqlDateTime.MinValue)
                    deviceCommisionDate = SqlDateTime.MinValue.Value;
                else if (deviceCommisionDate.Value > SqlDateTime.MaxValue)
                    deviceCommisionDate = SqlDateTime.MaxValue.Value;
            }

            // Create device assigned to customer
            var customer = this.customersRepository.Get(customerModel.Id);
            var customerDevice = new CustomerDevice
            {
                Name = deviceName,
                Manufacturer = deviceManufacturer,
                CommissionDate = deviceCommisionDate
            };
            customer.CustomerDevices.Add(customerDevice);

            // Save changes
            await this.customersRepository.Save();

            // Return create customer device model
            return customerDevice;
        }
    }
}