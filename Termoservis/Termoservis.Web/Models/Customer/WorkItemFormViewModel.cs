using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Termoservis.Models;

namespace Termoservis.Web.Models.Customer
{
    /// <summary>
    /// The work item form view model.
    /// </summary>
    /// <seealso cref="Termoservis.Web.Models.Customer.IFormViewModel" />
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
        /// <exception cref="System.ArgumentNullException">
        /// actionName
        /// or
        /// workItem
        /// </exception>
        public WorkItemFormViewModel(string actionName, WorkItem workItem)
        {
            if (actionName == null) throw new ArgumentNullException(nameof(actionName));
            if (workItem == null) throw new ArgumentNullException(nameof(workItem));

            this.ActionName = actionName;
            this.WorkItem = workItem;
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
        public WorkItem WorkItem { get; }

        /// <summary>
        /// Gets the available workers.
        /// </summary>
        /// <value>
        /// The available workers.
        /// </value>
        public List<SelectListItem> AvailableWorkers => new List<SelectListItem>
        {
            // TODO Retrieve these values from database
            new SelectListItem {Text = "", Value = "0"},
            new SelectListItem {Text = "Marko", Value = "1"},
            new SelectListItem {Text = "Neven", Value = "2"},
            new SelectListItem {Text = "Martin", Value = "3"},
            new SelectListItem {Text = "Mario", Value = "4"},
            new SelectListItem {Text = "Dino", Value = "5"}
        };
    }
}