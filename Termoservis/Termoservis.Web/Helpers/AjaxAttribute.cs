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


        public AjaxAttribute(bool ajax)
        {
            this.ajax = ajax;
        }


        public override bool IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo)
        {
            return ajax == controllerContext.HttpContext.Request.IsAjaxRequest();
        }
    }
}