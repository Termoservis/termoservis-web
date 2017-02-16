using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Ionic.Zip;
using Newtonsoft.Json;
using Serilog;
using Termoservis.Contracts.Services;
using Termoservis.DAL;

namespace Termoservis.Web.Controllers.v1
{
    /// <summary>
    /// The management API controller.
    /// </summary>
    /// <seealso cref="ApiController" />
    [Authorize]
    [RoutePrefix("api/v1/management")]
    public class ManagementApiController : ApiController
    {
        private readonly ApplicationDbContext context;
        private readonly ILogger logger;


        /// <summary>
        /// Initializes a new instance of the <see cref="ManagementApiController"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="loggingService">The logging service.</param>
        /// <exception cref="System.ArgumentNullException">
        /// context
        /// or
        /// loggingService
        /// </exception>
        public ManagementApiController(ApplicationDbContext context, ILoggingService loggingService)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (loggingService == null) throw new ArgumentNullException(nameof(loggingService));

            this.context = context;
            this.logger = loggingService.GetLogger<ManagementApiController>();
        }

        //
        // GET: api/v1/management/backup
        /// <summary>
        /// Gets the database backup.
        /// </summary>
        /// <returns>Returns the .zip fole containing the database backup.</returns>
        [HttpGet]
        [Route("backup")]
        public HttpResponseMessage GetDbBackup()
        {
            var database = new
            {
                Addresses = this.context.Addresses.ToList(),
                Users = this.context.Users.ToList(),
                Countries = this.context.Countries.ToList(),
                Customers = this.context.Customers.ToList(),
                CustomerDevices = this.context.CustomerDevices.ToList(),
                Places = this.context.Places.ToList(),
                TelephoneNumbers = this.context.TelephoneNumbers.ToList(),
                Workers = this.context.Workers.ToList(),
                WorkItems = this.context.WorkItems.ToList()
            };

            var databaseSerialized = JsonConvert.SerializeObject(database, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Culture = CultureInfo.GetCultureInfo("hr-HR")
            });

            var streamContent = new PushStreamContent((outputStream, httpContext, transportContent) =>
            {
                try
                {
                    using (var zip = new ZipFile())
                    {
                        zip.AlternateEncodingUsage = ZipOption.Always;
                        zip.AlternateEncoding = Encoding.UTF8;
                        zip.AddEntry("Termoservis.json", databaseSerialized);
                        zip.Save(outputStream);
                    }
                }
                finally
                {
                    outputStream.Close();
                }
            });
            streamContent.Headers.ContentType = new MediaTypeHeaderValue("application/zip");
            streamContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = $"TermoservisDb-{DateTime.Now:yyyyMMddHHmmss}.zip",
            };

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = streamContent
            };
        }
    }
}