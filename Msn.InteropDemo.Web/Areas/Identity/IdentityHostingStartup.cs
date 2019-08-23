using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(Msn.InteropDemo.Web.Areas.Identity.IdentityHostingStartup))]
namespace Msn.InteropDemo.Web.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}