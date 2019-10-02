using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Termoservis.Web.Extensions
{
    /// <summary>
    /// The <see cref="HtmlHelper"/> extensions.
    /// </summary>
    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// Displays the text with line breaks.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="html">The HTML.</param>
        /// <param name="expression">The expression.</param>
        /// <returns>Returns the HTML string with provided text content.</returns>
        public static MvcHtmlString DisplayWithBreaksFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var model = html.Encode(metadata.Model).Replace("\r\n", "<br />\r\n");

            if (string.IsNullOrEmpty(model))
                return html.DisplayFor(expression);

            return MvcHtmlString.Create(model);
        }
    }
}