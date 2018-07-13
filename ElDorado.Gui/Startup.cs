using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ElDorado.Gui.Startup))]
namespace ElDorado.Gui
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
