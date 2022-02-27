using CommandsService.Models;

namespace CommandsService.Data
{
    public class CommandsRepo : ICommandsRepo
    {
        public void CreateCommand(int platformId, Command command)
        {
            throw new NotImplementedException();
        }

        public void CreatePlatform(Platform plaform)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Command> GetCommand(int platformId, int commandId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Command> GetCommandsForPlatform(int platformId)
        {
            throw new NotImplementedException();
        }

        public bool PlatformExists(int platformId)
        {
            throw new NotImplementedException();
        }

        public bool SaveChanges()
        {
            throw new NotImplementedException();
        }
    }
}
