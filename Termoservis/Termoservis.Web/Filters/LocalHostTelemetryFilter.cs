using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace Termoservis.Web.Filters
{
    /// <summary>
    /// The ApplicationInsights local host telemetry filter.
    /// This will disable telemetry pushing for local host configurations.
    /// </summary>
    /// <seealso cref="ITelemetryProcessor" />
    public class LocalHostTelemetryFilter : ITelemetryProcessor
    {
        private readonly ITelemetryProcessor next;


        /// <summary>
        /// Initializes a new instance of the <see cref="LocalHostTelemetryFilter"/> class.
        /// </summary>
        /// <param name="next">The next processor.</param>
        public LocalHostTelemetryFilter(ITelemetryProcessor next)
        {
            this.next = next;
        }


        /// <summary>
        /// Process a collected telemetry item.
        /// </summary>
        /// <param name="item">A collected Telemetry item.</param>
        public void Process(ITelemetry item)
        {
            // Ignore local host telemetry item
            var requestTelemetry = item as RequestTelemetry;
            if (requestTelemetry != null && requestTelemetry.Url.Host.ToLower() == "localhost")
                return;

            this.next.Process(item);
        }
    }
}
