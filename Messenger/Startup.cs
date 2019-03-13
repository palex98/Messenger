using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Messenger.Startup))]

namespace Messenger
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
