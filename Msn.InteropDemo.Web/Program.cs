using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System;

namespace Msn.InteropDemo.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host =  CreateWebHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();
                loggerFactory.CreateLogger<Program>().LogInformation("Inciando Aplicacion.");
                try
                {
                    var dataContext = services.GetRequiredService<Data.Context.DataContext>();
                    var userManager = services.GetRequiredService<UserManager<Entities.Identity.SystemUser>>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                    var initializer =  new  Data.DataInitialization.Initializer(dataContext, userManager, roleManager, loggerFactory);
                    initializer.SeedAsync().Wait();
                }
                catch (Exception ex)
                {
                    loggerFactory.CreateLogger<Program>().LogError(ex, "An error occurred seeding the DB.");
                }
            }

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseSerilog();
    }
}
