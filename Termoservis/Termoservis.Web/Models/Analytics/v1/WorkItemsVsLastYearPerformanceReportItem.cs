using System;

namespace Termoservis.Web.Models.Analytics.v1
{
    /// <summary>
    /// The work items vs. last year performance report item.
    /// </summary>
    public class WorkItemsVsLastYearPerformanceReportItem
    {
        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>
        /// The date.
        /// </value>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the new count.
        /// </summary>
        /// <value>
        /// The new count.
        /// </value>
        public int NewCount { get; set; }

        /// <summary>
        /// Gets or sets the old count.
        /// </summary>
        /// <value>
        /// The old count.
        /// </value>
        public int OldCount { get; set; }
    }
}