using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MachineQuotes.Startup))]
namespace MachineQuotes
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
