using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Termoservis.DAL;
using Termoservis.Web.Models;

namespace Termoservis.Web.Controllers
{
	/// <summary>
	/// The manage controller.
	/// </summary>
	/// <seealso cref="System.Web.Mvc.Controller" />
	[Authorize]
	public class ManageController : Controller
	{
		private readonly ApplicationSignInManager signInManager;
		private readonly ApplicationUserManager userManager;


		/// <summary>
		/// Initializes a new instance of the <see cref="ManageController"/> class.
		/// </summary>
		/// <param name="userManager">The user manager.</param>
		/// <param name="signInManager">The sign in manager.</param>
		public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
		{
			this.userManager = userManager;
			this.signInManager = signInManager;
		}


		//
		// GET: /Manage/Index		
		/// <summary>
		/// Index page.
		/// </summary>
		/// <param name="message">The message to show to the user.</param>
		public ActionResult Index(ManageMessageId? message)
		{
			this.ViewBag.StatusMessage =
				message == ManageMessageId.ChangePasswordSuccess
					? "Your password has been changed."
					: message == ManageMessageId.Error
						? "An error has occurred."
						: "";

			return this.View();
		}


		//
		// GET: /Manage/ChangePassword		
		/// <summary>
		/// Changes password page.
		/// </summary>
		public ActionResult ChangePassword()
		{
			return View();
		}

		//
		// POST: /Manage/ChangePassword		
		/// <summary>
		/// Changes password form submit.
		/// </summary>
		/// <param name="model">The model.</param>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}
			var result =
				await this.userManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
			if (result.Succeeded)
			{
				var user = await this.userManager.FindByIdAsync(User.Identity.GetUserId());
				if (user != null)
				{
					await this.signInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
				}
				return RedirectToAction("Index", new {Message = ManageMessageId.ChangePasswordSuccess});
			}
			AddErrors(result);
			return View(model);
		}

		/// <summary>
		/// Releases unmanaged and - optionally - managed resources.
		/// </summary>
		/// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.userManager != null)
			{
				userManager.Dispose();
			}

			base.Dispose(disposing);
		}

		#region Helpers

		/// <summary>
		/// Adds the errors to the model state.
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
		/// Messages.
		/// </summary>
		public enum ManageMessageId
		{
			/// <summary>
			/// The password changed successfully message.
			/// </summary>
			ChangePasswordSuccess,

			/// <summary>
			/// The generic error message.
			/// </summary>
			Error
		}

		#endregion
	}
}