using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using Serilog;
using Termoservis.Common.Extensions;
using Termoservis.Contracts.Services;
using Termoservis.DAL;
using Termoservis.DAL.Extensions;
using Termoservis.DAL.Repositories;
using Termoservis.Models;
using Termoservis.Web.Helpers;
using Termoservis.Web.Models.Customer;

namespace Termoservis.Web.Controllers
{
    public class CustomersSearchResult
    {
        public List<Customer> Customers { get; set; }

        public int TotalPages { get; set; }

        public int CurrentPage { get; set; }

        public string Keywords { get; set; }
    }

    /// <summary>
    /// The <see cref="Customer"/> controller.
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    [Authorize]
    [RequireHttps]
    public class CustomersController : Controller
    {
	    private readonly ApplicationDbContext context;
        private readonly ICustomerService customerService;
        private readonly ILogger logger;

        private const int CustomersPageSize = 10;


        /// <summary>
        /// Initializes a new instance of the <see cref="CustomersController"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="loggingService">The logging service.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="System.ArgumentNullException">
        /// context
        /// or
        /// loggingService
        /// </exception>
        public CustomersController(ApplicationDbContext context, ICustomerService customerService, ILoggingService loggingService)
	    {
		    if (context == null) throw new ArgumentNullException(nameof(context));
	        if (customerService == null) throw new ArgumentNullException(nameof(customerService));
	        if (loggingService == null) throw new ArgumentNullException(nameof(loggingService));

		    this.context = context;
	        this.customerService = customerService;
	        this.logger = loggingService.GetLogger<CustomersController>();
	    }


        //
		// GET: Customers		
		/// <summary>
		/// The index page.
		/// </summary>
		public async Task<ActionResult> Index()
		{
            var result = new CustomersSearchResult
            {
                CurrentPage = 0
            };
            var customers = this.context.Customers.Include(c => c.Address);

            // Calculate total pages
		    var totalFound = customers.Count();
		    result.TotalPages = (int)Math.Ceiling((decimal)totalFound / CustomersPageSize);

            // Set current page content
		    result.Customers = customers
                .OrderBy(c => c.Name)
		        .Skip(result.CurrentPage * CustomersPageSize)
		        .Take(CustomersPageSize)
		        .ToList();

            return View(result);
        }

        [HttpGet]
        public async Task<ActionResult> CustomersSearch(int newPage, string keywords)
        {
            // Create empty response
            var result = new CustomersSearchResult
            {
                CurrentPage = newPage,
                Keywords = keywords
            };

            // Filter customers using keywords
            if (!string.IsNullOrWhiteSpace(keywords))
            {
                var splitKeywords = keywords
                    .AsSearchable()
                    .Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries)
                    .Where(k => !string.IsNullOrEmpty(k))
                    .ToList();
                var toSkip = result.CurrentPage * CustomersPageSize;

                var customersNameQuery = await
                    this.context.Customers
                        .Where(c => splitKeywords.Any(k => c.SearchKeywords.Contains(k)))
                        .OrderBy(c => c.Name)
                        .Skip(toSkip)
                        .Take(CustomersPageSize)
                        .ToListAsync();
                var customersAddressQuery = await
                    this.context.Customers
                        .Where(c => splitKeywords.Any(k => c.Address.SearchKeywords.Contains(k)))
                        .OrderBy(c => c.Name)
                        .Skip(toSkip)
                        .Take(CustomersPageSize)
                        .ToListAsync();
                var customersNoteQuery = await
                    this.context.Customers
                        .Where(c => splitKeywords.Any(k => c.Note.Contains(k)))
                        .OrderBy(c => c.Name)
                        .Skip(toSkip)
                        .Take(CustomersPageSize)
                        .ToListAsync();
                var customersTelephoneQuery = await
                    this.context.Customers
                        .Where(
                            c =>
                                c.TelephoneNumbers.Any() &&
                                splitKeywords.Any(k => c.TelephoneNumbers.Any(t => t.SearchKeywords.Contains(k))))
                        .OrderBy(c => c.Name)
                        .Skip(toSkip)
                        .Take(CustomersPageSize)
                        .ToListAsync();

                //// Wait all queries
                //await Task.WhenAll(
                //    customersNameQuery,
                //    customersAddressQuery,
                //    customersNoteQuery,
                //    customersTelephoneQuery);

                // Combine all queries
                var customersFiltered =
                    customersNameQuery.Union(
                            customersAddressQuery).Union(
                            customersNoteQuery).Union(
                            customersTelephoneQuery)
                        .Skip(toSkip)
                        .Take(CustomersPageSize)
                        .ToList();

                // Populate result
                result.TotalPages = customersFiltered.Count < CustomersPageSize
                    ? result.CurrentPage
                    : result.CurrentPage + 1;
                result.Customers = customersFiltered;
            }
            else
            {
                // Create query with all customers
                var customers = this.context.Customers;

                // Calculate total pages
                var totalFound = customers.Count();
                result.TotalPages = (int) Math.Ceiling((decimal) totalFound / CustomersPageSize);

                // Set current page content
                result.Customers = await customers
                    .OrderBy(c => c.Name)
                    .Skip(result.CurrentPage * CustomersPageSize)
                    .Take(CustomersPageSize)
                    .ToListAsync();
            }

            return PartialView("_CustomersTablePartial", result);
        }

        //
        // GET: Customers/Details/{id}
        /// <summary>
        /// The customer details page.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public async Task<ActionResult> Details(long id)
        {
			// Check customer identifier
			if (id == 0)
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

			// Retrieve customer
			var customer = await this.context.Customers.Include(c => c.WorkItems).Include("WorkItems.Worker").FirstOrDefaultAsync(c => c.Id.Equals(id));
			if (customer == null)
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Customer with given identifier not found.");

			return View(customer);
        }

        //
		// GET: Customers/Create		
		/// <summary>
		/// The create customer page.
		/// </summary>
		public async Task<ActionResult> Create()
        {
            var viewModel = new CustomerFormViewModel("Create");
			await viewModel.PopulateLocationsAsync(this.context);
			viewModel.TelephoneNumbers = new List<TelephoneNumber>
			{
				new TelephoneNumber()
			};

            return View(viewModel);
        }

        //
		// POST: Customers/Create		
		/// <summary>
		/// Creates the customer from submitted form view model.
		/// </summary>
		/// <param name="viewModel">The customer creation view model.</param>
		[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CustomerFormViewModel viewModel)
        {
            if (!viewModel.CustomerPlaceId.HasValue)
            {
                this.ModelState.AddModelError("", "Place is required.");

                // Validate model
                await viewModel.PopulateLocationsAsync(this.context);
                return View(viewModel);
            }

            var customer = new Customer
            {
                Name = viewModel.Name.Trim(),
                Email = viewModel.Email?.Trim(),
                Note = viewModel.Note?.Trim(),
            };

            var streetName = viewModel.CustomerStreetName.Trim();
            var placeId = viewModel.CustomerPlaceId.Value;

            var userName = this.User.Identity.Name;
            var user = this.context.Users.FirstOrDefault(u => u.UserName == userName);

            var createdCustomer = await this.customerService.CreateCustomerAsync(customer, streetName, placeId, viewModel.TelephoneNumbers, user);

            return RedirectToAction("Index");
        }

        //
        // GET: Customers/Edit/5
        public async Task<ActionResult> Edit(long id)
        {
            if (id <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var customer = this.context.Customers.FirstOrDefault(c => c.Id == id);
            if (customer == null)
            {
                return HttpNotFound();
            }

            var vm = new CustomerFormViewModel("Edit")
            {
                TelephoneNumbers = customer.TelephoneNumbers,
                Name = customer.Name,
                Address = customer.Address,
                AddressId = customer.AddressId,
                CreationDate = customer.CreationDate,
                CustomerCountryId = customer.Address.Place?.CountryId,
                CustomerDevices = customer.CustomerDevices,
                CustomerPlaceId = customer.Address.PlaceId,
                CustomerStreetName = customer.Address.StreetAddress,
                Email = customer.Email,
                Id = customer.Id,
                Note = customer.Note,
                WorkItems = customer.WorkItems
            };
            await vm.PopulateLocationsAsync(this.context);

            return View(vm);
        }

        //
        // POST: Customers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CustomerFormViewModel viewModel)
        {
            if (!viewModel.CustomerPlaceId.HasValue)
            {
                this.ModelState.AddModelError("", "Place is required.");

                // Validate model
                await viewModel.PopulateLocationsAsync(this.context);
                return View(viewModel);
            }

            var customer = new Customer
            {
                Id = viewModel.Id,
                Name = viewModel.Name.Trim(),
                Email = viewModel.Email?.Trim(),
                Note = viewModel.Note?.Trim(),
            };

            var streetName = viewModel.CustomerStreetName.Trim();
            var placeId = viewModel.CustomerPlaceId.Value;

            var editedCustomer = await this.customerService.EditCustomerAsync(customer, streetName, placeId, viewModel.TelephoneNumbers);

            return RedirectToAction("Details", new { id = editedCustomer.Id });
        }
    }

    public interface ICustomerService
    {
        Task<Customer> CreateCustomerAsync(
            Customer customerModel,
            string streetName,
            int placeId,
            IEnumerable<TelephoneNumber> telephoneNumbers,
            ApplicationUser user);

        Task<Customer> EditCustomerAsync(
            Customer customerModel, 
            string streetName, 
            int placeId, 
            IEnumerable<TelephoneNumber> telephoneNumbers);
    }

    public class CustomerService : ICustomerService
    {
        private readonly ICustomersRepository customersRepository;
        private readonly ITelephoneNumbersRepository telephoneNumbersRepository;
        private readonly IAddressesRepository addressesRepository;


        public CustomerService(IAddressesRepository addressesRepository, ICustomersRepository customersRepository, ITelephoneNumbersRepository telephoneNumbersRepository)
        {
            if (addressesRepository == null) throw new ArgumentNullException(nameof(addressesRepository));
            if (customersRepository == null) throw new ArgumentNullException(nameof(customersRepository));
            if (telephoneNumbersRepository == null) throw new ArgumentNullException(nameof(telephoneNumbersRepository));

            this.customersRepository = customersRepository;
            this.telephoneNumbersRepository = telephoneNumbersRepository;
            this.addressesRepository = addressesRepository;
        }


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

            var telephoneNumbersList = telephoneNumbers?.ToList() ?? new List<TelephoneNumber>();
            foreach (var telephoneNumber in telephoneNumbersList)
                await this.telephoneNumbersRepository.AddAsync(telephoneNumber);

            var address = await addressesRepository.EnsureExistsAsync(streetName, placeId);

            customerModel.TelephoneNumbers = telephoneNumbersList;
            customerModel.AddressId = address.Id;
            customerModel.ApplicationUserId = user.Id;

            return await this.customersRepository.AddAsync(customerModel);
        }

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
            var telephoneNumbersList = telephoneNumbers?.ToList() ?? new List<TelephoneNumber>();
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

            var address = await addressesRepository.EnsureExistsAsync(streetName, placeId);

            customerModel.TelephoneNumbers = telephoneNumbersList;
            customerModel.AddressId = address.Id;
            customerModel.Address = address;

            return await this.customersRepository.EditAsync(customerModel.Id, customerModel);
        }
    }
}
