using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Http.Results;
using System.Web.Mvc;
using Serilog;
using Termoservis.BLL;
using Termoservis.Common.Extensions;
using Termoservis.Contracts.Services;
using Termoservis.DAL;
using Termoservis.DAL.Extensions;
using Termoservis.Models;
using Termoservis.Web.Models.Customer;

namespace Termoservis.Web.Controllers
{
    /// <summary>
    /// The <see cref="Customer"/> controller.
    /// </summary>
    /// <seealso cref="Controller" />
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
        /// <param name="customerService">The customer service.</param>
        /// <param name="loggingService">The logging service.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="System.ArgumentNullException">
        /// context
        /// or
        /// customerService
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
	        this.logger = loggingService?.GetLogger<CustomersController>();
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

        //
        // GET: CustomersSearch
        /// <summary>
        /// The customers search action. Returns partial.
        /// </summary>
        /// <param name="newPage">The new page.</param>
        /// <param name="keywords">The keywords.</param>
        [HttpGet]
        public async Task<ActionResult> CustomersSearch(int newPage, string keywords)
        {
            // Create empty response
            var result = new CustomersSearchResult
            {
                CurrentPage = newPage,
                Keywords = keywords
            };

            // Create query with all customers
            var customers = this.context.Customers.AsQueryable();

            // Apply filter if needed
            IOrderedQueryable<Customer> orderedCustomers;
            if (!string.IsNullOrWhiteSpace(keywords))
            {
                var searchableKeywords = keywords.AsSearchable();
                var searchContainsQuery = searchableKeywords.AsFtsContainsString();
                var searchableKeywordsSplit = searchableKeywords.Split(
                    new[] {' '},
                    StringSplitOptions.RemoveEmptyEntries);
                orderedCustomers =
                    customers
                        .Where(c =>
                            c.Name.Contains(searchContainsQuery))
                        .Concat(
                            customers.Where(c =>
                                c.SearchKeywords.Contains(searchContainsQuery)))
                        .OrderBy(c => !searchableKeywordsSplit.Any(sk => c.Name.Contains(sk)));
            }
            else
            {
                orderedCustomers = customers.OrderBy(c => c.Name);
            }

            // Set current page content
            var toSkip = result.CurrentPage * CustomersPageSize;
            result.Customers = await orderedCustomers
                .Skip(toSkip)
                .Take(CustomersPageSize)
                .ToListAsync();

            // Calculate total pages
            var totalFound = orderedCustomers.Count();
            result.TotalPages = (int) Math.Ceiling((decimal) totalFound / CustomersPageSize);

            // Return the partial with new data
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
			var customer = await this.context.Customers
                .Include(c => c.WorkItems)
                .Include(c => c.WorkItems.Select(i => i.Worker))
                .Include(c => c.WorkItems.Select(i => i.AffectedDevices))
                .FirstOrDefaultAsync(c => c.Id.Equals(id));
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
			viewModel.TelephoneNumbers = new List<TelephoneNumber>();

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
            // Populate model with view model data
            var customer = new Customer
            {
                Name = viewModel.Name.Trim(),
                Email = viewModel.Email?.Trim(),
                Note = viewModel.Note?.Trim(),
            };

            // Retrieve required data
            var streetName = viewModel.CustomerStreetName.Trim();
            var placeId = viewModel.CustomerPlaceId;
            var userName = this.User.Identity.Name;
            var user = this.context.Users.FirstOrDefault(u => u.UserName == userName);

            // Create the customer
            var createdCustomer = await this.customerService.CreateCustomerAsync(customer, streetName, placeId, viewModel.TelephoneNumbers, user);

            // Redirect to details of created customer
            return RedirectToAction("Details", new { id = createdCustomer.Id });
        }

        //
        // GET: Customers/Edit/5        
        /// <summary>
        /// The edit customer view.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public async Task<ActionResult> Edit(long id)
        {
            // Validate identifier
            if (id <= 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            // Retrieve customer
            var customer = this.context.Customers.FirstOrDefault(c => c.Id == id);
            if (customer == null)
            {
                return HttpNotFound();
            }

            // Populate view model
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
                CustomerPlaceName = customer.Address.Place?.Name,
                CustomerStreetName = customer.Address.StreetAddress,
                Email = customer.Email,
                Id = customer.Id,
                Note = customer.Note,
                WorkItems = customer.WorkItems
            };
            await vm.PopulateLocationsAsync(this.context);

            // Return the edit view
            return View(vm);
        }

        //
        // POST: Customers/Edit
        /// <summary>
        /// Edits the customer.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CustomerFormViewModel viewModel)
        {
            // Populate model from view model
            var customer = new Customer
            {
                Id = viewModel.Id,
                Name = viewModel.Name.Trim(),
                Email = viewModel.Email?.Trim(),
                Note = viewModel.Note?.Trim(),
            };

            // Retrieve required data
            var streetName = viewModel.CustomerStreetName.Trim();
            var placeId = viewModel.CustomerPlaceId;

            // Edit the customer
            var editedCustomer = await this.customerService.EditCustomerAsync(customer, streetName, placeId, viewModel.TelephoneNumbers);

            // Display edited customer details view
            return RedirectToAction("Details", new { id = editedCustomer.Id });
        }

        //
        // POST: /Customers/CreateCustomerDevice
        /// <summary>
        /// Creates the customer device.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// viewModel
        /// or
        /// Device
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// CustomerId
        /// </exception>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateCustomerDevice(CustomerDeviceFormViewModel viewModel)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));
            if (viewModel.CustomerId <= 0)
                throw new ArgumentOutOfRangeException(nameof(viewModel.CustomerId));
            if (viewModel.Device == null)
                throw new ArgumentNullException(nameof(viewModel.Device));

            // Retrieve customer
            var customer = this.context.Customers.FirstOrDefault(c => c.Id == viewModel.CustomerId);
            if (customer == null)
                throw new ArgumentOutOfRangeException(nameof(viewModel.CustomerId));

            // Retrieve required data
            var deviceName = viewModel.Device.Name;
            var deviceManufacturer = viewModel.Device.Manufacturer;
            var deviceCommisionDate = viewModel.Device.CommissionDate;

            // Create device for customer
            await this.customerService.CreateNewCustomerDeviceAsync(
                customer,
                deviceName,
                deviceManufacturer,
                deviceCommisionDate);

            // Display edited customer details view
            return RedirectToAction("Details", new { id = customer.Id });
        }
    }
}
