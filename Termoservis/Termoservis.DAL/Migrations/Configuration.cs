using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security.DataProtection;
using Termoservis.Models;

namespace Termoservis.DAL.Migrations
{
	using System.Data.Entity.Migrations;

	/// <summary>
	/// The migrations configuration.
	/// </summary>
	// ReSharper disable once UnusedMember.Global
	public sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="Configuration"/> class.
		/// </summary>
		public Configuration()
        {
            this.AutomaticMigrationsEnabled = false;
        }

		/// <summary>
		/// Seeds the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		protected override void Seed(ApplicationDbContext context)
		{
		    context.Countries.AddOrUpdate(c => c.Id, new Country {Name = "Hrvatska", SearchKeywords = "hrvatska"});

            //  This method will be called after migrating to the latest version.

		    //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
		    //  to avoid creating duplicate seed data. E.g.
		    //
		    //    context.People.AddOrUpdate(
		    //      p => p.FullName,
		    //      new Person { FullName = "Andrew Peters" },
		    //      new Person { FullName = "Brice Lambson" },
		    //      new Person { FullName = "Rowan Miller" }
		    //    );
		    //
		}
    }
}
