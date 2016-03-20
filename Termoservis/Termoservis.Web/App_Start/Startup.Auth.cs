using System;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Practices.Unity;
using Owin;
using Termoservis.DAL;
using Termoservis.Models;

namespace Termoservis.Web
{
	/// <summary>
	/// The application startup entry-point.
	/// </summary>
	public partial class Startup
    {
		/// <summary>
		/// Configures the authentication.
		/// </summary>
		/// <param name="app">The application.</param>
		public void ConfigureAuth(IAppBuilder app)
        {
			// Register data protection provider instance
	        UnityConfig.GetConfiguredContainer().RegisterInstance(app.GetDataProtectionProvider());

			// Configure the db context, user manager and sign-in manager to use a single instance per request
			app.CreatePerOwinContext(() => DependencyResolver.Current.GetService<ApplicationUserManager>());

			// Enable the application to use a cookie to store information for the signed in user
			// and to use a cookie to temporarily store information about a user logging in with a third party login provider
			// Configure the sign in cookie
			app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });
        }
    }
}