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

        //Commands
        IEnumerable<Command> GetCommandsForPlatform(int platformId);

        IEnumerable<Command> GetCommand(int platformId, int commandId);

        void CreateCommand(int platformId, Command command);
    }
}
