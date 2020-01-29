using System.Web.Http;
using Mindscape.Raygun4Net.WebApi;
using Newtonsoft.Json;

namespace Termoservis.Web
{
    /// <summary>
    /// The Web API configuration.
    /// </summary>
    public class WebApiConfig
    {
        /// <summary>
        /// Registers the Web API configuration.
        /// </summary>
        /// <param name="config">The configuration.</param>
        public static void Register(HttpConfiguration config)
        {
            // Web API routes
            config.MapHttpAttributeRoutes();

            // Convention-based routing
            config.Routes.MapHttpRoute(
                name: "ApiV1",
                routeTemplate: "api/v1/{controller}/{id}",
                defaults: new {id = RouteParameter.Optional});

            // Configure JSON
            var json = config.Formatters.JsonFormatter;
            json.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            // Configure WebAPI
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.LocalOnly;

            RaygunWebApiClient.Attach(config);
        }
    }
}
