using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app, bool isProd)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), isProd);
                //Migrate(serviceScope.ServiceProvider.GetService<AppDbContext>());
            }
        }

        private static void SeedData(AppDbContext context, bool isProd)
        {
            if (isProd)
            {
                try
                {
                    Console.WriteLine("--> Adding migrations to production database");
                    context.Database.Migrate();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not Add migrations to production database {ex.Message}");
                }
            }

            if (!context.Platforms.Any())
            {
                Console.WriteLine("Seeding data...");

                context.Platforms.AddRange(
                    new Platform {Name="Dot Net", Publisher="Microsoft", Cost="Free"},
                    new Platform { Name = "Sql Server Express", Publisher = "Microsoft", Cost = "Free" },
                    new Platform { Name = "Kubernetes", Publisher = "Cloud Native Computing Foundation", Cost = "Free" });

                context.SaveChanges();
            }
            else
                Console.WriteLine("We already have data.");
        }
        public static void Migrate(AppDbContext context)
        {
            context.Database.Migrate();

        }
    }

    
}
