using System.Collections.Generic;

namespace Termoservis.Web.Models.Analytics.v1
{
    /// <summary>
    /// The worker work items count report.
    /// </summary>
    public class WorkerWorkItemsCountReport
    {
        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        public List<WorkerWorkItemsCountReportItem> Items { get; set; }
    }
}