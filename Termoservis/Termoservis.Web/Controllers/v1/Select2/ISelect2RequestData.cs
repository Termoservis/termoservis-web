namespace Termoservis.Web.Controllers.v1.Select2
{
    /// <summary>
    /// The Select2 request data.
    /// </summary>
    /// <seealso cref="ISelect2RequestData" />
    public interface ISelect2RequestData
    {
        /// <summary>
        /// Gets or sets the term.
        /// </summary>
        /// <value>
        /// The term.
        /// </value>
        string Term { get; set; }

        /// <summary>
        /// Gets or sets the page.
        /// </summary>
        /// <value>
        /// The page.
        /// </value>
        string Page { get; set; }
    }
}