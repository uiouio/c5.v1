using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CTPPV5.Site.Startup))]
namespace CTPPV5.Site
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
