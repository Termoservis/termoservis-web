using Microsoft.Owin;
using Microsoft.Owin.Security.DataProtection;
using Owin;

[assembly: OwinStartupAttribute(typeof(Termoservis.Web.Startup))]
namespace Termoservis.Web
{
    public partial class Startup
    {
		internal static IDataProtectionProvider DataProtectionProvider { get; private set; }

		public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
