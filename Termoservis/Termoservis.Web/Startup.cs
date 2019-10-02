using System.Web.Http;
using Microsoft.Owin;
using Owin;
using Swashbuckle.Application;

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
            var config = new HttpConfiguration();
            WebApiConfig.Register(config);
            this.ConfigureAuth(app);
        }
    }
}
