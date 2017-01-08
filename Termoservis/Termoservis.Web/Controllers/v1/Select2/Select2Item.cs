using System.Runtime.Serialization;

namespace Termoservis.Web.Controllers.v1.Select2
{
    /// <summary>
    /// The Select2 item.
    /// </summary>
    /// <seealso cref="ISelect2Item" />
    [DataContract]
    public class Select2Item : ISelect2Item
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        /// <remarks>
        /// JSON Property Name: "id"
        /// </remarks>
        [DataMember(Name = "id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        /// <remarks>
        /// JSON Property Name: "text"
        /// </remarks>
        [DataMember(Name = "text")]
        public string Text { get; set; }
    }
}