using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Pieshop.Models;

[assembly: HostingStartup(typeof(Pieshop.Areas.Identity.IdentityHostingStartup))]
namespace Pieshop.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                //services.AddDefaultIdentity<IdentityUser>()
                //    .AddEntityFrameworkStores<AppDbContext>(); //remove default scaffold setup with below

                //services.AddIdentity<IdentityUser, IdentityRole>(options => //replcaed with extended User
                services.AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                        options.Password.RequiredLength = 8;
                        options.User.RequireUniqueEmail = true;
                        options.Password.RequireNonAlphanumeric = true;
                        options.Password.RequireDigit = true;
                })
               .AddEntityFrameworkStores<AppDbContext>()
               .AddDefaultUI()
               .AddDefaultTokenProviders();


            });
        }
    }
}