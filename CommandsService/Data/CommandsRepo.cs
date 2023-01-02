using CommandsService.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace CommandsService.Data
{
    public class CommandsRepo : ICommandsRepo
    {
        private readonly AppDbContext _context;

        public CommandsRepo(AppDbContext context)
        {
            _context = context;
        }
        //Commands

        public Command GetCommand(int platformId, int commandId)
        {
            return _context.Commands.FirstOrDefault(c => c.Id == commandId && c.PlatformId == platformId);
        }

        public IEnumerable<Command> GetCommandsForPlatform(int platformId)
        {
            return _context.Commands.Where(c => c.PlatformId == platformId).ToList();
        }

        public void CreateCommand(int platformId, Command command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }
            command.PlatformId = platformId;
            _context.Add(command);
        }

        //Platforms

        public void CreatePlatform(Platform platform)
        {
            if (platform == null)
                throw new ArgumentNullException(nameof(platform));
            _context.Add(platform);
        }

        /// <summary>
        /// This will be used by Grpc to get all platforms from the PlatformsService to the CommandsService.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Platform> GetAllPlatforms()
        {
            return _context.Platforms.ToList();
        }

        public bool PlatformExists(int platformId)
        {
            return _context.Platforms.Any(p => p.Id == platformId);
        }

        public bool SaveChanges()
        {
            return _context.SaveChanges() >= 0;
        }

        public bool ExternalPlatformExists(int externalPlatformId)
{
            return _context.Platforms.Any(p => p.ExternalID == externalPlatformId);
        }
    }
}
