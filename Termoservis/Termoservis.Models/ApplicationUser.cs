using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Termoservis.Models
{
	/// <summary>
	/// The application user model.
	/// </summary>
	/// <seealso cref="Microsoft.AspNet.Identity.EntityFramework.IdentityUser" />
	public class ApplicationUser : IdentityUser
    {
		/// <summary>
		/// Generates the user identity asynchronous.
		/// </summary>
		/// <param name="manager">The manager.</param>
		/// <returns>Returns user identity.</returns>
		public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            // NOTE: Add custom user claims here

            return userIdentity;
        }
    }
}