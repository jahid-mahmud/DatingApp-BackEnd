
using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using DatingApp.API.Data;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host =CreateWebHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope ()) {
                var services = scope.ServiceProvider;
                try {
                    var context=services.GetRequiredService<DataContext>();
                    context.Database.Migrate();
                    Seed.SeedUser(context);

                }
                catch (Exception ex) {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex,"an error occured");

                    
                }
            }
            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
