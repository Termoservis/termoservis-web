using System.Collections.Generic;

namespace Termoservis.Web.Models.Analytics.v1
{
    /// <summary>
    /// The workitems vs. last year performance report.
    /// </summary>
    public class WorkItemsVsLastYearPerformanceReport
    {
        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        public List<WorkItemsVsLastYearPerformanceReportItem> Items { get; set; }
    }
}
