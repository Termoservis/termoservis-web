using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Termoservis.Models
{
    /// <summary>
    /// The customer device model.
    /// </summary>
    public class CustomerDevice : IEquatable<CustomerDevice>, IEqualityComparer<CustomerDevice>
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
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime? CommissionDate { get; set; }

        /// <summary>
        /// Gets or sets the work items.
        /// </summary>
        /// <value>
        /// The work items.
        /// </value>
        public virtual ICollection<WorkItem> WorkItems { get; set; }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        /// <summary>
        /// Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public bool Equals(CustomerDevice other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id && string.Equals(Name, other.Name) && string.Equals(Manufacturer, other.Manufacturer) && CommissionDate.Equals(other.CommissionDate);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id.GetHashCode();
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Manufacturer != null ? Manufacturer.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ CommissionDate.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        /// Equalses the specified x.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public bool Equals(CustomerDevice x, CustomerDevice y)
        {
            return x.Equals(y);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public int GetHashCode(CustomerDevice obj)
        {
            return obj.GetHashCode();
        }
    }
}