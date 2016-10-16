using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Serilog;
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
	/// <seealso cref="System.Web.Mvc.Controller" />
	public class CustomersController : Controller
    {
	    private readonly ApplicationDbContext context;
	    private readonly ILogger logger;


		/// <summary>
		/// Initializes a new instance of the <see cref="CustomersController"/> class.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="loggingService">The logging service.</param>
		/// <exception cref="System.ArgumentNullException">
		/// context
		/// or
		/// loggingService
		/// </exception>
		public CustomersController(ApplicationDbContext context, ILoggingService loggingService)
	    {
		    if (context == null) throw new ArgumentNullException(nameof(context));
		    if (loggingService == null) throw new ArgumentNullException(nameof(loggingService));

		    this.context = context;
		    this.logger = loggingService.GetLogger<CustomersController>();
	    }


		// GET: Customers		
		/// <summary>
		/// The index page.
		/// </summary>
		public async Task<ActionResult> Index()
        {
            var customers = this.context.Customers.Include(c => c.Address).Include(c => c.ApplicationUser);
            return View(await customers.ToListAsync());
        }

		// GET: Customers/Details/{id}
		/// <summary>
		/// The customer details page.
		/// </summary>
		/// <param name="id">The identifier.</param>
		public async Task<ActionResult> Details(string id)
        {
			// Check customer identifier
			if (id == null)
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

			// Retrieve customer
			var customer = await this.context.Customers.FirstOrDefaultAsync(c => c.Id.Equals(id));
			if (customer == null)
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Customer with given identifier not found.");

			return View(customer);
        }

		// GET: Customers/Create		
		/// <summary>
		/// The create customer page.
		/// </summary>
		public async Task<ActionResult> Create()
        {
            var viewModel = new CustomerCreateViewModel();
			await viewModel.PopulateLocationsAsync(this.context);
			viewModel.TelephoneNumbers = new List<TelephoneNumber>()
			{
				new TelephoneNumber()
			};

            return View(viewModel);
        }

        // POST: Customers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CustomerCreateViewModel viewModel)
        {
			// Validate model
            if (ModelState.IsValid)
            {
				// Retrieve or create place


                //this.context.Customers.Add(customer);
                //await this.context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

			return View(viewModel);
        }

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
}
