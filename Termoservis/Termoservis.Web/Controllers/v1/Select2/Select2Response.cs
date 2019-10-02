using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Termoservis.Web.Controllers.v1.Select2
{
    /// <summary>
    /// The Select2 reposnse.
    /// </summary>
    [DataContract]
    public class Select2Response
    {
        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        [DataMember(Name = "items")]
        public List<Select2Item> Items { get; set; }
    }
}