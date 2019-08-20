using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pieshop.Models;
using Serilog;
using System;
using System.IO;

namespace Pieshop
{
    public class Program
    {
        public static void Main(string[] args)
        {

            ConfigureLogging();
           
            var host = CreateWebHostBuilder(args).Build();
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
                    Log.Fatal(ex, "Starting web host failed.");
                } 
                finally
                {
                    Log.CloseAndFlush();
                }
            
            }

        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseSerilog();

                //REPLACED BELOW With Serilog configuration in appsettings.json files
                //.ConfigureLogging(options =>
                //{
                //    //clear default providers setup by DefaultWebHostBuilder
                //    options.ClearProviders();
                //    //add desired providers
                //    if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == EnvironmentName.Development)
                //    {
                //        options.AddDebug();
                //        options.AddConsole();
                //    }
                //    options.AddConsole(); //use with care - slow
                //});

        private static void ConfigureLogging()
        {
            //get configuration for appsettings.json environment
            var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{ Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") }.json", optional: true)
            .Build();

            //apply serilog configuration from file
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .CreateLogger();

        }

}
}
