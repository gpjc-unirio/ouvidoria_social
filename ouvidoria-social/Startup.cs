using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ouvidoria_social.Startup))]
namespace ouvidoria_social
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
