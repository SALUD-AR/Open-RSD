using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Msn.InteropDemo.Integration.Configuration;
using Serilog;
using Microsoft.Extensions.Options;

namespace Msn.InteropDemo.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            Log.Logger = new LoggerConfiguration()
                            .ReadFrom.Configuration(configuration)
                            .CreateLogger();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSession();
            services.AddHttpContextAccessor();
            services.AddLogging();

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<Data.Context.DataContext>();
            services.AddIdentity<Entities.Identity.SystemUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;

                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;

            })
                .AddEntityFrameworkStores<Data.Context.DataContext>()
                .AddDefaultUI(UIFramework.Bootstrap4);


            //*** DEPENDENCY INJECTIONS FOR OBJECTS ************************************************
            services.AddHttpContextAccessor();
            services.Configure<IntegrationServicesConfiguration>(Configuration.GetSection("IntegrationServices"));
            services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<IntegrationServicesConfiguration>>().Value);
            services.AddTransient(typeof(Data.Context.DataContext));

            services.AddTransient<Helpers.ISelectListHelper, Helpers.SelectListHelper>();

            services.AddTransient<Fhir.IPatientManager, Fhir.Implementacion.PatientManager>();

            services.AddTransient<AppServices.Core.ICurrentContext, Security.CurrentContext>();

            services.AddTransient<AppServices.IPacienteAppService, AppServices.Implementation.AppServices.PacienteAppService>();
            services.AddTransient<AppServices.IEvolucionAppService, AppServices.Implementation.AppServices.EvolucionAppService>();
            services.AddTransient<AppServices.ILogActivityAppService, AppServices.Implementation.AppServices.LogActivityAppService>();

            services.AddTransient<Snowstorm.ISnowstormManager, Snowstorm.Implementation.SnowstormManager>();
            //**************************************************************************************

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSession();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                  name: "areaRoute",
                  template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
