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
        /// <param name="isCreate">If set to <c>True</c> this view model is to be used for creating work item.</param>
        /// <param name="workItem">The work item.</param>
        /// <param name="customer">The customer.</param>
        /// <exception cref="System.ArgumentNullException">
        /// actionName
        /// or
        /// workItem
        /// </exception>
        public WorkItemFormViewModel(string actionName, bool isCreate, WorkItem workItem, Termoservis.Models.Customer customer)
        {
            this.IsCreate = isCreate;
            this.ActionName = actionName ?? throw new ArgumentNullException(nameof(actionName));
            this.WorkItem = workItem ?? throw new ArgumentNullException(nameof(workItem));
            this.AvailableDevices = new MultiSelectList(customer.CustomerDevices, "Id", "Name");
            this.AffectedDevices = workItem.AffectedDevices?.Select(device => device.Id).ToList() ?? new List<long>();

            // Assign default if customer has only one device and work item is new
            if (workItem.Id == 0 && this.AvailableDevices.Count() == 1)
                this.AffectedDevices.Add(customer.CustomerDevices.First().Id);
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is create.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is create; otherwise, <c>false</c>.
        /// </value>
        public bool IsCreate { get; set; }

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
            new SelectListItem {Text = "Marko", Value = "1", Disabled = true},
            new SelectListItem {Text = "Neven", Value = "2"},
            new SelectListItem {Text = "Martin", Value = "3", Disabled = true},
            new SelectListItem {Text = "Mario", Value = "4"},
            new SelectListItem {Text = "Dino K", Value = "5", Disabled = true},
            new SelectListItem {Text = "Mladen", Value = "6"},
            new SelectListItem {Text = "Dino H", Value = "7"}
        };
    }
}
