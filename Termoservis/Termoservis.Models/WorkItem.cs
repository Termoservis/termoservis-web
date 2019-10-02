using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Termoservis.Models
{
    /// <summary>
    /// The work item model.
    /// </summary>
    public class WorkItem
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>
        /// The date.
        /// </value>
        [DisplayName("Datum")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Date { get; set; } = DateTime.Today;

        /// <summary>
        /// Gets or sets the price.
        /// </summary>
        /// <value>
        /// The price.
        /// </value>
        [DisplayName("Cijena")]
        public double Price { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [DataType(DataType.MultilineText)]
        [DisplayName("Opis")]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        [DisplayName("Vrsta")]
        public WorkItemType Type { get; set; } = WorkItemType.Service;

        /// <summary>
        /// Gets or sets the worker identifier.
        /// </summary>
        /// <value>
        /// The worker identifier.
        /// </value>
        [DisplayName("Serviser")]
        public long? WorkerId { get; set; }

        /// <summary>
        /// Gets or sets the worker.
        /// </summary>
        /// <value>
        /// The worker.
        /// </value>
        [DisplayName("Serviser")]
        [ForeignKey(nameof(WorkerId))]
        public Worker Worker { get; set; }

        /// <summary>
        /// Gets or sets the customer identifier.
        /// </summary>
        /// <value>
        /// The customer identifier.
        /// </value>
        [Required]
        [Display(Name = "Korisnik")]
        public long CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the customer.
        /// </summary>
        /// <value>
        /// The customer.
        /// </value>
        [JsonIgnore]
        [ForeignKey(nameof(CustomerId))]
        [Display(Name = "Korisnik")]
        public Customer Customer { get; set; }

        /// <summary>
        /// Gets or sets the affected devices.
        /// </summary>
        /// <value>
        /// The affected devices.
        /// </value>
        [Display(Name = "Uredaji")]
        public virtual ICollection<CustomerDevice> AffectedDevices { get; set; }
    }
}