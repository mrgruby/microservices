using CommandsService.Models;

namespace CommandsService.Data
{
    public interface ICommandsRepo
    {
        bool SaveChanges();

        //Platforms

        IEnumerable<Platform> GetAllPlatforms();

        void CreatePlatform(Platform plaform);

        bool PlatformExists(int platformId);

        bool ExternalPlatformExists(int externalPlatformId);

        //Commands
        IEnumerable<Command> GetCommandsForPlatform(int platformId);

        Command GetCommand(int platformId, int commandId);

        void CreateCommand(int platformId, Command command);
    }
}
