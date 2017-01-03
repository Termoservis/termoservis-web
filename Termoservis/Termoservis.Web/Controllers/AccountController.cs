using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Termoservis.DAL;
using Termoservis.Models;
using Termoservis.Web.Models;

namespace Termoservis.Web.Controllers
{
	/// <summary>
	/// The account controller.
	/// </summary>
	/// <seealso cref="System.Web.Mvc.Controller" />
	[Authorize]
    [RequireHttps]
    public class AccountController : Controller
    {
        private readonly ApplicationSignInManager signInManager;
        private readonly ApplicationUserManager userManager;
		private readonly IAuthenticationManager authenticationManager;


		/// <summary>
		/// Initializes a new instance of the <see cref="AccountController"/> class.
		/// </summary>
		/// <param name="userManager">The user manager.</param>
		/// <param name="signInManager">The sign in manager.</param>
		/// <param name="authenticationManager">The authentication manager.</param>
		public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, IAuthenticationManager authenticationManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
			this.authenticationManager = authenticationManager;
        }


		//
		// GET: /Account/Login		
		/// <summary>
		/// Logins user.
		/// </summary>
		/// <param name="returnUrl">The return URL.</param>
		[AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
			this.ViewBag.ReturnUrl = returnUrl;
            return this.View();
        }

		//
		// POST: /Account/Login		
		/// <summary>
		/// Logins the user.
		/// </summary>
		/// <param name="model">The model.</param>
		/// <param name="returnUrl">The return URL.</param>
		[HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
			// Validate model
			if (!this.ModelState.IsValid)
				return View(model);

			// This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await this.signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return this.RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return this.View("Lockout");
	            default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return this.View(model);
            }
        }

		//
		// GET: /Account/Register		
		/// <summary>
		/// Registers the user.
		/// </summary>
		[AllowAnonymous]
        public ActionResult Register()
        {
            return this.View();
        }

		//
		// POST: /Account/Register		
        /// <summary>
        /// Registers the user.
        /// </summary>
        /// <param name="model">The model.</param>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            // NOTE: Registrations are disabled
            //this.ModelState.AddModelError("", "Registrations are disabled. Contact the administrator for more info.");
            //return this.View(model);

            if (this.ModelState.IsValid)
            {
                // Validate email address
                if (!model.Email.EndsWith("@termoservis.hr"))
                {
                    this.ModelState.AddModelError("", "Unauthorized registration attempt.");
                    return this.View(model);
                }

                // Validate password
                if (model.Password != model.ConfirmPassword)
                {
                    this.ModelState.AddModelError("", "Passwords don't match.");
                    return this.View(model);
                }

                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = this.userManager.Create(user, model.Password);
                if (result.Succeeded)
                {
                    this.signInManager.SignIn(user, isPersistent: false, rememberBrowser: false);
                    return this.RedirectToAction("Index", "Home");
                }
                this.AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return this.View(model);
        }

        //
		// POST: /Account/LogOff		
		/// <summary>
		/// Logs the user off.
		/// </summary>
		[HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            this.authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return this.RedirectToAction("Index", "Home");
        }


		/// <summary>
		/// Releases unmanaged and - optionally - managed resources.
		/// </summary>
		/// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
		protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
	            this.userManager?.Dispose();
	            this.signInManager?.Dispose();
            }

            base.Dispose(disposing);
        }

		#region Helpers

		/// <summary>
		/// Adds errors to the model state.
		/// </summary>
		/// <param name="result">The result.</param>
		private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

		/// <summary>
		/// Redirects to only local addresses. If address is not local, defaults to application home page.
		/// </summary>
		/// <param name="returnUrl">The return URL.</param>
		/// <returns>Redirect to URL.</returns>
		private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        #endregion
    }
}