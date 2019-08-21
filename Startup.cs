using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pieshop.ControllerServices;
using Pieshop.FluentValidators;
using Pieshop.Interfaces;
using Pieshop.Mapping;
using Pieshop.Models;
using Pieshop.Repositories;
using Pieshop.ViewServices;
using System.Globalization;
using System.Linq;

namespace Pieshop
{
    public class Startup
    {

        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            //uk specific culture settings for this app
            var cultureInfo = new CultureInfo("en-GB");
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //setup EF Core context with SqlServer and the connection string to use
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
                );

            //Auto Mapper Configurations
            //not 100% sure about this setup
            //IMHO mapping should be scoped per request and only for the mapping types needed at the point of request
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddSingleton<ProfileOptionsService, ProfileOptionsService>();
            services.AddTransient<IPieRepository, PieRepository>();
            services.AddTransient<IPieReviewRepository, PieReviewRepository>();
            services.AddTransient<IFeedbackRepository, FeedbackRepository>();

            services.AddDistributedRedisCache(options =>
            {
                options.InstanceName = Configuration["DistributedCaching:InstanceName"]; 
                options.Configuration = Configuration["DistributedCaching:Configuration"]; 
            });

            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "imagejpeg" });
            });

            services.Configure<GzipCompressionProviderOptions>(
                options => options.Level = System.IO.Compression.CompressionLevel.Optimal);

            services.AddResponseCaching(); //when used, the AntiForgerycheck needs to be ignored. Good for static public GET ONLY views

            services.AddTransient<IDistributedCacheService, DistributedCacheService>();

            services.AddMvc(options =>
            {
                options.Filters.Add(
                    new AutoValidateAntiforgeryTokenAttribute()); //anti-forgery check on every form post in app (NOTE complicates ResponseCaching)
            }).AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<AddUserViewModelValidator>());
            //FluentValidation and validators registration (transient by default)

       
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseResponseCompression();
            app.UseResponseCaching();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}"
                    );
            }
            );

        }
    }
}
