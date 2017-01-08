using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Termoservis.Web.Helpers
{
    /// <summary>
    /// The HTML helpers.
    /// </summary>
    public static class HtmlHelpers
	{
	    /// <summary>
	    /// Bootstrap navbar menu link.
	    /// </summary>
	    /// <param name="htmlHelper">The HTML helper.</param>
	    /// <param name="linkText">The link text.</param>
	    /// <param name="actionName">Name of the action.</param>
	    /// <param name="controllerName">Name of the controller.</param>
	    /// <param name="htmlAttributes">The HTML attributes.</param>
	    /// <param name="activeInController">If set to <c>True</c> link will be active as long the user is in specified controller, action name will not be checked.</param>
	    /// <returns>Returns the bootstrap navbar menu link.</returns>
	    /// <remarks>
	    /// Source: http://chrisondotnet.com/2012/08/setting-active-link-twitter-bootstrap-navbar-aspnet-mvc/
	    /// </remarks>
	    public static MvcHtmlString MenuLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, object htmlAttributes, bool activeInController = false)
	    {
	        var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext, htmlHelper.RouteCollection);
            var currentAction = htmlHelper.ViewContext.RouteData.GetRequiredString("action");
            var currentController = htmlHelper.ViewContext.RouteData.GetRequiredString("controller");

            var link = new TagBuilder("a");
            link.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
            link.MergeAttribute("href", urlHelper.Action(actionName, controllerName));
            link.SetInnerText(linkText);

            if (controllerName == currentController && (activeInController || actionName == currentAction))
                link.AddCssClass("active");

            return new MvcHtmlString(link.ToString());
        }

        /// <summary>
        /// Displays the column name for specified model property.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TClass">The type of the class.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="helper">The helper.</param>
        /// <param name="model">The model.</param>
        /// <param name="expression">The expression.</param>
        /// <returns>Returns the column name for model property.</returns>
        public static MvcHtmlString DisplayColumnNameFor<TModel, TClass, TProperty>(this HtmlHelper<TModel> helper,
	        IEnumerable<TClass> model, Expression<Func<TClass, TProperty>> expression)
	    {
	        var name = ExpressionHelper.GetExpressionText(expression);
	        var metadata = ModelMetadataProviders.Current.GetMetadataForProperty(
	            () => Activator.CreateInstance<TClass>(), typeof(TClass), name);

	        var returnName = metadata.DisplayName;
	        if (string.IsNullOrEmpty(returnName))
	            returnName = metadata.PropertyName;

	        return new MvcHtmlString(returnName);
	    }

	    /// <summary>
        /// 'Add' link helper.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="linkText">The link text.</param>
        /// <param name="containerElement">The container element.</param>
        /// <param name="counterElement">The counter element.</param>
        /// <param name="collectionProperty">The collection property.</param>
        /// <param name="nestedType">Type of the nested.</param>
        /// <returns>Returns the HTML string that represents the 'add' link.</returns>
        /// <remarks>
        /// The link will trigger `addNestedForm` function with following parameters:
        /// containerElement, counterElement, ticks, partial
        /// </remarks>
        public static IHtmlString AddLink<TModel>(
			this HtmlHelper<TModel> htmlHelper,
			string linkText,
			string containerElement,
			string counterElement,
			string collectionProperty,
			Type nestedType)
		{
			var ticks = DateTime.UtcNow.Ticks;
			var nestedObject = Activator.CreateInstance(nestedType);
			var partial = htmlHelper.EditorFor(x => nestedObject).ToHtmlString().JsEncode();
			partial = partial.Replace("id=\\\"nestedObject", "id=\\\"" + collectionProperty + "_" + ticks + "_");
			partial = partial.Replace("name=\\\"nestedObject", "name=\\\"" + collectionProperty + "[" + ticks + "]");
			var js = $"javascript:addNestedForm('{containerElement}','{counterElement}','{ticks}','{partial}');return false;";
			TagBuilder tb = new TagBuilder("a");
			tb.Attributes.Add("href", "#");
			tb.Attributes.Add("onclick", js);
			tb.InnerHtml = linkText;
			var tag = tb.ToString(TagRenderMode.Normal);
			return MvcHtmlString.Create(tag);
		}

        /// <summary>
        /// Encodes the string as JavaScript.
        /// </summary>
        /// <param name="s">The string.</param>
        /// <returns>Returns the JavaScript encoded string.</returns>
        private static string JsEncode(this string s)
		{
			if (string.IsNullOrEmpty(s)) return "";
			int i;
			int len = s.Length;
			StringBuilder sb = new StringBuilder(len + 4);
			string t;
			for (i = 0; i < len; i += 1)
			{
				char c = s[i];
				switch (c)
				{
					case '>':
					case '"':
					case '\\':
						sb.Append('\\');
						sb.Append(c);
						break;
					case '\b':
						sb.Append("\\b");
						break;
					case '\t':
						sb.Append("\\t");
						break;
					case '\n':
						break;
					case '\f':
						sb.Append("\\f");
						break;
					case '\r':
						break;
					default:
						if (c < ' ')
						{
							string tmp = new string(c, 1);
							t = "000" + int.Parse(tmp, System.Globalization.NumberStyles.HexNumber);
							sb.Append("\\u" + t.Substring(t.Length - 4));
						}
						else
						{
							sb.Append(c);
						}
						break;
				}
			}
			return sb.ToString();
		}
	}
}