using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pieshop.Models;
using System;

namespace Pieshop
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();
            ILogger logger = host.Services.GetService<ILogger<Program>>();

            using (var scope = host.Services.CreateScope())
            {
                try
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                    DbInitializer.Seed(dbContext);
                    DbInitializer.SeedAdminRole(roleManager);
                    DbInitializer.SeedDefaultAdminUser(userManager);

                    //if db initialization setup has succeeded we run the app
                    host.Run();
                }
                catch(Exception ex)
                {

                    //log any exceptions, app wont start if failure happens here
                    logger.LogCritical(ex, "Starting web host failed.");

                }
            }

        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
                
                
    }
}
