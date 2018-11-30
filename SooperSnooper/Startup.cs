using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SooperSnooper.Startup))]
namespace SooperSnooper
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
