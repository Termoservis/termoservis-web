using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using Serilog;
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
        public ActionResult CustomersSearch(int newPage)
        {
            var result = new CustomersSearchResult
            {
                CurrentPage = newPage
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
			var customer = await this.context.Customers.FirstOrDefaultAsync(c => c.Id.Equals(id));
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
            var viewModel = new CustomerCreateViewModel();
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
        public async Task<ActionResult> Create(CustomerCreateViewModel viewModel)
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

        ////
        //// GET: Customers/Edit/5
        //public async Task<ActionResult> Edit(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Customer customer = await db.Customers.FindAsync(id);
        //    if (customer == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.AddressId = new SelectList(db.Addresses, "Id", "StreetAddress", customer.AddressId);
        //    ViewBag.ApplicationUserId = new SelectList(db.ApplicationUsers, "Id", "Email", customer.ApplicationUserId);
        //    return View(customer);
        //}

        //// POST: Customers/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Note,Email,AddressId,SearchKeywords,ApplicationUserId,CreationDate")] Customer customer)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(customer).State = EntityState.Modified;
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.AddressId = new SelectList(db.Addresses, "Id", "StreetAddress", customer.AddressId);
        //    ViewBag.ApplicationUserId = new SelectList(db.ApplicationUsers, "Id", "Email", customer.ApplicationUserId);
        //    return View(customer);
        //}
    }

    public interface ICustomerService
    {
        Task<Customer> CreateCustomerAsync(
            Customer customerModel,
            string streetName,
            int placeId,
            IEnumerable<TelephoneNumber> telephoneNumbers,
            ApplicationUser user);
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
    }
}
