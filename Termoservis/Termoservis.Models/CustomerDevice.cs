using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Termoservis.Models
{
    /// <summary>
    /// The customer device model.
    /// </summary>
    public class CustomerDevice
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
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [DisplayName("Model")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the manufacturer.
        /// </summary>
        /// <value>
        /// The manufacturer.
        /// </value>
        [DisplayName("Proizvodac")]
        public string Manufacturer { get; set; }

        /// <summary>
        /// Gets or sets the commission date.
        /// </summary>
        /// <value>
        /// The commission date.
        /// </value>
        [DisplayName("Pusten u pogon")]
        public DateTime? CommissionDate { get; set; }
    }
}