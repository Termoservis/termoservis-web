using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Termoservis.Models;

namespace Termoservis.Web.Models.Customer
{
    public class WorkItemFormViewModel : IFormViewModel
    {
        public WorkItemFormViewModel()
        {
        }

        public WorkItemFormViewModel(string actionName, WorkItem workItem)
        {
            if (actionName == null) throw new ArgumentNullException(nameof(actionName));
            if (workItem == null) throw new ArgumentNullException(nameof(workItem));

            this.ActionName = actionName;
            this.WorkItem = workItem;
        }

        public string ActionName { get; set; }
        public WorkItem WorkItem { get; }

        public List<SelectListItem> AvailableWorkers => new List<SelectListItem>
        {
            new SelectListItem {Text = "", Value = "0"},
            new SelectListItem {Text = "Marko", Value = "1"},
            new SelectListItem {Text = "Neven", Value = "2"},
            new SelectListItem {Text = "Martin", Value = "3"},
            new SelectListItem {Text = "Mario", Value = "4"},
            new SelectListItem {Text = "Dino", Value = "5"}
        };
    }
}