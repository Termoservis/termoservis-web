using Microsoft.AspNet.Identity.EntityFramework;
using Termoservis.Models;

namespace Termoservis.DAL
{
	/// <summary>
	/// The application database context.
	/// </summary>
	/// <seealso cref="Microsoft.AspNet.Identity.EntityFramework.IdentityDbContext{Termoservis.Models.ApplicationUser}" />
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
		/// </summary>
		public ApplicationDbContext()
			: base("DefaultConnection", throwIfV1Schema: false)
		{
		}

		/// <summary>
		/// Creates new instance of context.
		/// </summary>
		/// <returns>Returns new instance of application context.</returns>
		public static ApplicationDbContext Create()
		{
			return new ApplicationDbContext();
		}
	}
}