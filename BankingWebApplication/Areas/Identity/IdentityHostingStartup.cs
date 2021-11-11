using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(BankingWebApplication.Areas.Identity.IdentityHostingStartup))]
namespace BankingWebApplication.Areas.Identity
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