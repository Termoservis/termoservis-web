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
        [Display(Name = "")]
        Unknown = 0,

        /// <summary>
        /// The repair work item type.
        /// </summary>
        [Display(Name = "Popravak")]
        Repair = 1,

        /// <summary>
        /// The service work item type.
        /// </summary>
        [Display(Name = "Servis")]
        Service = 2,

        /// <summary>
        /// The device commision work item type.
        /// </summary>
        [Display(Name = "Puštanje u pogon")]
        Commision = 3,

        /// <summary>
        /// The work in waranty work item type.
        /// </summary>
        [Display(Name = "Radovi u jamstvu")]
        InWaranty = 4,

        /// <summary>
        /// Service and repair item type.
        /// </summary>
        [Display(Name = "Servis i popravak")]
        ServiceAndRepair = 5
    }
}