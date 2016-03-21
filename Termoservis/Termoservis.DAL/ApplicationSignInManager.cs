using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Termoservis.Models;

namespace Termoservis.DAL
{
	/// <summary>
	/// The application sign-in manager.
	/// </summary>
	public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="ApplicationSignInManager"/> class.
		/// </summary>
		/// <param name="userManager">The user manager.</param>
		/// <param name="authenticationManager">The authentication manager.</param>
		public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

		/// <summary>
		/// Creates the user identity asynchronous.
		/// </summary>
		/// <param name="user">The user.</param>
		/// <returns>Returns claim identities for given user.</returns>
		public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)this.UserManager);
        }
    }
}
