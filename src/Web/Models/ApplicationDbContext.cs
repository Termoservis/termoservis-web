using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Mvc;
using Microsoft.Data.Entity;

namespace Web.Models
{
	public interface IApplicationDbContext
	{
		/// <summary>
		/// Gets or sets the application user.
		/// </summary>
		/// <value>
		/// The application user.
		/// </value>
		DbSet<ApplicationUser> ApplicationUser { get; set; }

		/// <summary>
		/// Gets the current user.
		/// </summary>
		/// <param name="controller">The controller.</param>
		/// <returns>Returns the current logged in user.</returns>
		Task<ApplicationUser> GetCurrentUserAsync(Controller controller);
	}

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
	{
	    /// <summary>
		/// Gets or sets the application user.
		/// </summary>
		/// <value>
		/// The application user.
		/// </value>
		public DbSet<ApplicationUser> ApplicationUser { get; set; }

		/// <summary>
		/// Gets the current user.
		/// </summary>
		/// <param name="controller">The controller.</param>
		/// <returns>
		/// Returns the current logged in user.
		/// </returns>
		public async Task<ApplicationUser> GetCurrentUserAsync(Controller controller)
	    {
			return await this.ApplicationUser.FirstOrDefaultAsync(user => user.UserName.Equals(controller.User.Identity.Name));
		}
	}
}
