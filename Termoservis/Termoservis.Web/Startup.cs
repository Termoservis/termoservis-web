using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Termoservis.Web.Startup))]
namespace Termoservis.Web
{
	/// <summary>
	/// The application startup entry-point.
	/// </summary>
	public partial class Startup
    {
		/// <summary>
		/// Configurations the specified application.
		/// </summary>
		/// <param name="app">The application.</param>
		public void Configuration(IAppBuilder app)
        {
            this.ConfigureAuth(app);
        }
    }
}
