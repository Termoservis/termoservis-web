using System.Web.Mvc;

namespace Termoservis.Web.Controllers
{
    /// <summary>
    /// The status controller.
    /// </summary>
    /// <seealso cref="Controller" />
    [Authorize]
#if !DEBUG
    [RequireHttps]
#endif
    public class StatusController : Controller
    {
        //
        // GET: Releases
        /// <summary>
        /// The releases page.
        /// </summary>
        public ActionResult Releases()
        {
            return this.View();
        }
    }
}
