using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;
using Termoservis.Models;

namespace Termoservis.DAL
{
	/// <summary>
	/// The <see cref="ApplicationUser"/> manager.
	/// </summary>
	public class ApplicationUserManager : UserManager<ApplicationUser>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ApplicationUserManager"/> class.
		/// </summary>
		/// <param name="store">The store.</param>
		/// <param name="dataProtectionProvider">The data protection provider.</param>
		public ApplicationUserManager(
			IUserStore<ApplicationUser> store, 
			IDataProtectionProvider dataProtectionProvider)
			: base(store)
		{
			// Configure validation logic for user names
			this.UserValidator = new UserValidator<ApplicationUser>(this)
			{
				AllowOnlyAlphanumericUserNames = true,
				RequireUniqueEmail = true
			};

			// Configure validation logic for passwords
			this.PasswordValidator = new PasswordValidator
			{
				RequiredLength = 6,
				RequireNonLetterOrDigit = false,
				RequireDigit = true,
				RequireLowercase = true,
				RequireUppercase = false
			};

			// Configure user lockout defaults
			this.UserLockoutEnabledByDefault = true;
			this.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
			this.MaxFailedAccessAttemptsBeforeLockout = 5;

			if (dataProtectionProvider != null)
			{
				this.UserTokenProvider =
					new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
			}
		}
	}
}