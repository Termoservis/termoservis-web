using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Serilog;
using Termoservis.Contracts.Services;
using Termoservis.DAL.Repositories;
using Termoservis.Web.Models.Analytics.v1;

namespace Termoservis.Web.Controllers.v1
{
    /// <summary>
    /// The analytics controller.
    /// </summary>
    /// <seealso cref="ApiController" />
    [Authorize]
    [RoutePrefix("api/v1/analytics")]
    public class AnalyticsController : ApiController
    {
        private readonly IWorkItemsRepository workItemsRepository;
        private readonly ILogger logger;


        /// <summary>
        /// Initializes a new instance of the <see cref="AnalyticsController"/> class.
        /// </summary>
        /// <param name="workItemsRepository">The work items repository.</param>
        /// <param name="loggingService">The logging service.</param>
        /// <exception cref="System.ArgumentNullException">
        /// workItemsRepository
        /// or
        /// loggingService
        /// </exception>
        public AnalyticsController(
            IWorkItemsRepository workItemsRepository,
            ILoggingService loggingService)
        {
            if (workItemsRepository == null) throw new ArgumentNullException(nameof(workItemsRepository));
            if (loggingService == null) throw new ArgumentNullException(nameof(loggingService));
            
            this.workItemsRepository = workItemsRepository;
            this.logger = loggingService.GetLogger<AnalyticsController>();
        }


        //
        // GET api/v1/analytics/workerworkitems
        [HttpGet]
        [ResponseType(typeof(WorkerWorkItemsCountReport))]
        [Route("workerworkitems")]
        public async Task<IHttpActionResult> GetWorkerWorkItemsCount()
        {
            const int reportDays = 30;

            var report = new WorkerWorkItemsCountReport()
            {
                Items = new List<WorkerWorkItemsCountReportItem>()
            };

            // Current date
            var today = DateTime.Today;
            var before = today.Subtract(TimeSpan.FromDays(reportDays));

            // Retrieve work items
            var workItemsQuery = await this.workItemsRepository
                .GetAll()
                .Where(workItem =>
                    workItem.Date.HasValue &&
                    workItem.Date.Value >= before &&
                    workItem.Date.Value <= today)
                .GroupBy(workItem => workItem.Worker)
                .OrderBy(group => group.Key.Name)
                .Select(group => new {Worker = group.Key, Count = group.Count()})
                .ToListAsync();

            foreach (var workerWorkItems in workItemsQuery)
            {
                report.Items.Add(new WorkerWorkItemsCountReportItem
                {
                    WorkerName = workerWorkItems.Worker?.Name ?? "Nepoznato",
                    WorkItemsCount = workerWorkItems.Count
                });
            }

            return this.Ok(report);
        }

        //
        // GET api/v1/analytics/newwivslastyear        
        /// <summary>
        /// Gets the this years work items vs last years work items performance report.
        /// </summary>
        /// <returns>Returns the performance report.</returns>
        [HttpGet]
        [ResponseType(typeof(WorkItemsVsLastYearPerformanceReport))]
        [Route("newwivslastyear")]
        public async Task<IHttpActionResult> GetWorkItemsVsLastYearPerformance()
        {
            const int reportItemsCount = 30;

            var report = new WorkItemsVsLastYearPerformanceReport();

            // Current date
            var today = DateTime.Today;
            var thisYearFirst = today.AddDays(-reportItemsCount);
            var todayLastYear = today.AddYears(-1);
            var firstLastYear = thisYearFirst.AddYears(-1);

            // Retrieve work items 
            var workItemsQuery =
                from workItem in this.workItemsRepository.GetAll()
                where workItem.Date.HasValue &&
                      (workItem.Date.Value >= thisYearFirst && workItem.Date.Value <= today ||
                       workItem.Date.Value >= firstLastYear && workItem.Date.Value <= todayLastYear)
                group workItem by workItem.Date;
            
            // Execute query
            var workItemsDateGroups = await workItemsQuery.ToListAsync();

            // Count items
            var reportCounts = new Dictionary<DateTime, int>();
            foreach (var workItemDateGroup in workItemsDateGroups)
            {
                // Ignore no date groups
                if (!workItemDateGroup.Key.HasValue)
                    continue;

                // Prepare report item if doesn't exist
                if (!reportCounts.ContainsKey(workItemDateGroup.Key.Value))
                    reportCounts.Add(workItemDateGroup.Key.Value, 0);
                
                // Process this/last year
                reportCounts[workItemDateGroup.Key.Value] = reportCounts[workItemDateGroup.Key.Value] +
                                                            workItemDateGroup.Count();
            }

            // Generate report items
            var reportItems = new List<WorkItemsVsLastYearPerformanceReportItem>();
            for (var index = 0; index < reportItemsCount; index++)
            {
                var thisYearsDate = thisYearFirst.AddDays(index);
                var lastYearsDate = firstLastYear.AddDays(index);

                // Create report item 
                var reportItem = new WorkItemsVsLastYearPerformanceReportItem
                {
                    Date = thisYearsDate,
                    NewCount = reportCounts.FirstOrDefault(ri => ri.Key == thisYearsDate).Value,
                    OldCount = reportCounts.FirstOrDefault(ri => ri.Key == lastYearsDate).Value
                };

                reportItems.Add(reportItem);
            }

            // Assign report items to the report
            report.Items = reportItems.OrderBy(item => item.Date).ToList();

            return this.Ok(report);
        }
    }
}
