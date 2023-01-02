using CommandsService.Models;
using CommandsService.SyncDataServices.Grpc;

namespace CommandsService.Data
{
    /// <summary>
    /// This class demonstrates using Grpc to call another service to get data. PrepPopulation is called in program.cs when
    /// the commandservice is started. This 
    /// </summary>
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder appBuilder)
        {
            //We need this because we can't use constructor DI in a static class
            using (var serviceScope = appBuilder.ApplicationServices.CreateScope())
            {
                var grpcServiceClient = serviceScope.ServiceProvider.GetService<IPlatformDataClient>();

                var platforms = grpcServiceClient.ReturnAllPlatforms();

                //The CommandRepo has a method to create platforms...
                SeedData(serviceScope.ServiceProvider.GetService<ICommandsRepo>(), platforms);
            }
        }

        private static void SeedData(ICommandsRepo repo, IEnumerable<Platform> platforms)
        {
            Console.WriteLine("Seeding new platforms...");

            foreach (var item in platforms)
            {
                if (!repo.ExternalPlatformExists(item.ExternalID))
                {
                    repo.CreatePlatform(item);
                }
                repo.SaveChanges();
            }
        }
    }
}
