using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using Termoservis.Common.Extensions;
using Termoservis.Contracts.Services;
using Termoservis.DAL.Repositories;
using Termoservis.Models;
using Termoservis.Web.Controllers.v1.Select2;

namespace Termoservis.Web.Controllers.v1
{
    /// <summary>
    /// The <see cref="Place"/> API controller.
    /// </summary>
    /// <seealso cref="ApiController" />
    [Authorize]
    [RoutePrefix("api/v1/places")]
    public class PlacesApiController : ApiController
    {
        private const int Select2ItemPerPage = 30;

        private readonly IPlacesRepository placesRepository;
        private readonly ILoggingService loggingService;


        /// <summary>
        /// Initializes a new instance of the <see cref="PlacesApiController"/> class.
        /// </summary>
        /// <param name="placesRepository">The places repository.</param>
        /// <param name="loggingService">The logging service.</param>
        /// <exception cref="System.ArgumentNullException">
        /// placesRepository
        /// or
        /// loggingService
        /// </exception>
        public PlacesApiController(IPlacesRepository placesRepository, ILoggingService loggingService)
        {
            if (placesRepository == null) throw new ArgumentNullException(nameof(placesRepository));
            if (loggingService == null) throw new ArgumentNullException(nameof(loggingService));

            this.placesRepository = placesRepository;
            this.loggingService = loggingService;
        }


        //
        // POST api/v1/places/getall/select2        
        /// <summary>
        /// Gets all places.
        /// This will apply request data (search term and page).
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Returns collection of 30 places filtered by search term and of specified page.</returns>
        [HttpPost]
        [ResponseType(typeof(Select2Response))]
        [Route("getall/select2")]
        public IHttpActionResult GetAllSelect2(Select2RequestData request)
        {
            var response = new Select2Response();

            // Get places query
            var placesQuery = this.placesRepository.GetAll();

            // Apply term if appropriate
            var term = request.Term?.AsSearchable() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(term))
                placesQuery = placesQuery.Where(place => place.SearchKeywords.Contains(term));

            // Calcualte how many items we need to skip
            int page;
            var didParse = int.TryParse(request.Page ?? string.Empty, out page);
            var toSkip = didParse ? (page - 1) * Select2ItemPerPage : 0;

            // Execute query and transform data to Select2 items
            response.Items = placesQuery
                .OrderBy(place => place.Name)
                .Skip(toSkip)
                .Take(Select2ItemPerPage)
                .ToList()
                .Select(place => new Select2Item
                {
                    Id = place.Id.ToString(),
                    Text = place.Name
                })
                .ToList();

            return Ok(response);
        }
    }
}