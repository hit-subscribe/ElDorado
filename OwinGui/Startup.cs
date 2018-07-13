using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OwinGui.Startup))]
namespace OwinGui
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
