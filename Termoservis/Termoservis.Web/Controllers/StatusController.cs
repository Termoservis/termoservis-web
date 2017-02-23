using System.Web.Mvc;

namespace Termoservis.Web.Controllers
{
    /// <summary>
    /// The status controller.
    /// </summary>
    /// <seealso cref="Controller" />
    [Authorize]
    [RequireHttps]
    public class StatusController : Controller
    {
        //
        // GET: Releases
        /// <summary>
        /// The releases page.
        /// </summary>
        public ActionResult Releases()
        {
            return View();
        }
    }
}