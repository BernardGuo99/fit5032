using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Mel_Medicare_Location_Reservation_System.Startup))]
namespace Mel_Medicare_Location_Reservation_System
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
