using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Whatsapp.Startup))]
namespace Whatsapp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
