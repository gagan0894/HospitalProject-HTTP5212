using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HospitalProject_HTTP5212.Startup))]
namespace HospitalProject_HTTP5212
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
