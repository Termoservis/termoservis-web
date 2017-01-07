namespace Termoservis.Web.Controllers.v1.Select2
{
    /// <summary>
    /// The Select2 item.
    /// </summary>
    public interface ISelect2Item
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        string Id { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        string Text { get; set; }
    }
}