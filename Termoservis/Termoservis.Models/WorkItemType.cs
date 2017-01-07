using System.ComponentModel.DataAnnotations;

namespace Termoservis.Models
{
    /// <summary>
    /// The work item type.
    /// </summary>
    public enum WorkItemType
    {
        /// <summary>
        /// The unknown work item type.
        /// </summary>
        [Display(Name = "Nepoznato")]
        Unknown,

        /// <summary>
        /// The repair work item type.
        /// </summary>
        [Display(Name = "Popravak")]
        Repair,

        /// <summary>
        /// The service work item type.
        /// </summary>
        [Display(Name = "Servis")]
        Service
    }
}