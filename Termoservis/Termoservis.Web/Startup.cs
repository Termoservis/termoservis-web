using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Termoservis.Web.Startup))]
namespace Termoservis.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
