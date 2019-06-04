namespace Termoservis.Web.Models.Analytics.v1
{
    /// <summary>
    /// The worker work items count report item.
    /// </summary>
    public class WorkerWorkItemsCountReportItem
    {
        /// <summary>
        /// Gets or sets the name of the worker.
        /// </summary>
        /// <value>
        /// The name of the worker.
        /// </value>
        public string WorkerName { get; set; }

        /// <summary>
        /// Gets or sets the work items count.
        /// </summary>
        /// <value>
        /// The work items count.
        /// </value>
        public int WorkItemsCount { get; set; }
    }
}