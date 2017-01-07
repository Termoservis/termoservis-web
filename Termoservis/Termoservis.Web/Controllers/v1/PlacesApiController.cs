using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Web.Http;
using System.Web.Http.Description;
using Newtonsoft.Json;
using Termoservis.Common.Extensions;
using Termoservis.Contracts.Services;
using Termoservis.DAL.Repositories;
using Termoservis.Models;

namespace Termoservis.Web.Controllers.v1
{
    /// <summary>
    /// The Select2 request data.
    /// </summary>
    /// <seealso cref="ISelect2RequestData" />
    public interface ISelect2RequestData
    {
        /// <summary>
        /// Gets or sets the term.
        /// </summary>
        /// <value>
        /// The term.
        /// </value>
        string Term { get; set; }

        /// <summary>
        /// Gets or sets the page.
        /// </summary>
        /// <value>
        /// The page.
        /// </value>
        string Page { get; set; }
    }

    /// <summary>
    /// The Select2 request data.
    /// </summary>
    /// <seealso cref="ISelect2RequestData" />
    [DataContract]
    public class Select2RequestData : ISelect2RequestData
    {
        /// <summary>
        /// Gets or sets the term.
        /// </summary>
        /// <value>
        /// The term.
        /// </value>
        /// <remarks>
        /// JSON Property Name: "q"
        /// </remarks>
        [DataMember(Name = "q")]
        public string Term { get; set; }

        /// <summary>
        /// Gets or sets the page.
        /// </summary>
        /// <value>
        /// The page.
        /// </value>
        /// <remarks>
        /// JSON Property Name: "page"
        /// </remarks>
        [DataMember(Name = "page")]
        public string Page { get; set; }
    }

    /// <summary>
    /// The Select2 item.
    /// </summary>
    /// <seealso cref="ISelect2Item" />
    [DataContract]
    public class Select2Item : ISelect2Item
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        /// <remarks>
        /// JSON Property Name: "id"
        /// </remarks>
        [DataMember(Name = "id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        /// <remarks>
        /// JSON Property Name: "text"
        /// </remarks>
        [DataMember(Name = "text")]
        public string Text { get; set; }
    }

    /// <summary>
    /// The Select2 item.
    /// </summary>
    public interface ISelect2Item
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        string Id { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        string Text { get; set; }
    }

    /// <summary>
    /// The Select2 reposnse.
    /// </summary>
    [DataContract]
    public class Select2Response
    {
        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        [DataMember(Name = "items")]
        public List<Select2Item> Items { get; set; }
    }

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
            var page = 0;
            int.TryParse(request.Page ?? string.Empty, out page);
            var toSkip = page * Select2ItemPerPage;

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