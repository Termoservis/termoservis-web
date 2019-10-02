using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Termoservis.Models
{
    /// <summary>
    /// The worker model.
    /// </summary>
    public class Worker
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
        [Required]
        [DisplayName("Ime i prezime")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the worker is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the worker is active; otherwise, <c>false</c>.
        /// </value>
        [DisplayName("Aktivan")]
        public bool IsActive { get; set; }
    }
}