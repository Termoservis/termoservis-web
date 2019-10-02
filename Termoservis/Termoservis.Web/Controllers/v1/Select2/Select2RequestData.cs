using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Termoservis.Web.Controllers.v1.Select2
{
    /// <summary>
    /// The Select2 request data.
    /// </summary>
    /// <seealso cref="ISelect2RequestData" />
    [DataContract]
    public class Select2RequestData : ISelect2RequestData
    {
        /// <summary>
        /// Gets or sets the term.
        /// </summary>
        /// <value>
        /// The term.
        /// </value>
        /// <remarks>
        /// JSON Property Name: "q"
        /// </remarks>
        [DataMember(Name = "q")]
        public string Term { get; set; }

        /// <summary>
        /// Gets or sets the page.
        /// </summary>
        /// <value>
        /// The page.
        /// </value>
        /// <remarks>
        /// JSON Property Name: "page"
        /// </remarks>
        [DataMember(Name = "page")]
        public string Page { get; set; }
    }
}