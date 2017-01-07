using System.Reflection;
using System.Web.Mvc;

namespace Termoservis.Web.Helpers
{
    /// <summary>
    /// The AJAX attribute for controller actions.
    /// </summary>
    /// <remarks>
    /// Source: 
    /// https://blogs.msdn.microsoft.com/stuartleeks/2011/04/13/asp-net-mvc-partial-rendering-and-ajaxattribute/
    /// </remarks>
    public class AjaxAttribute : ActionMethodSelectorAttribute
    {
        private readonly bool ajax;


        /// <summary>
        /// Initializes a new instance of the <see cref="AjaxAttribute"/> class.
        /// </summary>
        /// <param name="ajax">if set to <c>true</c> action is Ajax action.</param>
        public AjaxAttribute(bool ajax)
        {
            this.ajax = ajax;
        }


        /// <summary>
        /// Determines whether the action method selection is valid for the specified controller context.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="methodInfo">Information about the action method.</param>
        /// <returns>
        /// true if the action method selection is valid for the specified controller context; otherwise, false.
        /// </returns>
        public override bool IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo)
        {
            return ajax == controllerContext.HttpContext.Request.IsAjaxRequest();
        }
    }
}