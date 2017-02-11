using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Termoservis.Models;

namespace Termoservis.Web.Models.Customer
{
    /// <summary>
    /// The work item form view model.
    /// </summary>
    /// <seealso cref="IFormViewModel" />
    public class WorkItemFormViewModel : IFormViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WorkItemFormViewModel"/> class.
        /// </summary>
        public WorkItemFormViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkItemFormViewModel"/> class.
        /// </summary>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="workItem">The work item.</param>
        /// <param name="customer">The customer.</param>
        /// <exception cref="System.ArgumentNullException">
        /// actionName
        /// or
        /// workItem
        /// </exception>
        public WorkItemFormViewModel(string actionName, WorkItem workItem, Termoservis.Models.Customer customer)
        {
            if (actionName == null) throw new ArgumentNullException(nameof(actionName));
            if (workItem == null) throw new ArgumentNullException(nameof(workItem));

            this.ActionName = actionName;
            this.WorkItem = workItem;
            this.AvailableDevices = new MultiSelectList(customer.CustomerDevices, "Id", "Name");
            this.AffectedDevices = workItem.AffectedDevices?.Select(device => device.Id).ToList() ?? new List<long>();

            // Assign default if customer has only one device and work item is new
            if (workItem.Id == 0 && this.AvailableDevices.Count() == 1)
                this.AffectedDevices.Add(customer.CustomerDevices.First().Id);
        }

        /// <summary>
        /// Gets or sets the name of the action.
        /// </summary>
        /// <value>
        /// The name of the action.
        /// </value>
        public string ActionName { get; set; }

        /// <summary>
        /// Gets the work item.
        /// </summary>
        /// <value>
        /// The work item.
        /// </value>
        public WorkItem WorkItem { get; set; } = new WorkItem();

        /// <summary>
        /// Gets or sets the affected devices.
        /// </summary>
        /// <value>
        /// The affected devices.
        /// </value>
        public List<long> AffectedDevices { get; set; }

        /// <summary>
        /// Gets or sets the available devices.
        /// </summary>
        /// <value>
        /// The available devices.
        /// </value>
        public MultiSelectList AvailableDevices { get; set; }

        /// <summary>
        /// Gets the available workers.
        /// </summary>
        /// <value>
        /// The available workers.
        /// </value>
        public List<SelectListItem> AvailableWorkers { get; set; } = new List<SelectListItem>
        {
            // TODO Retrieve these values from database
            new SelectListItem {Text = "", Value = "0"},
            new SelectListItem {Text = "Marko", Value = "1"},
            new SelectListItem {Text = "Neven", Value = "2"},
            new SelectListItem {Text = "Martin", Value = "3", Disabled = true},
            new SelectListItem {Text = "Mario", Value = "4"},
            new SelectListItem {Text = "Dino", Value = "5"}
        };
    }
}