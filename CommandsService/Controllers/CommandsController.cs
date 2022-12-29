using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    [Route("api/c/platforms/{platformId}/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly ICommandsRepo _repo;
        private readonly IMapper _mapper;

        public CommandsController(ICommandsRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>>GetCommandsForPlatform(int platformId)
        {
            Console.WriteLine($"--> Hitting GetCommandsForPlatform with platformId: {platformId}");
            //Check if the platform exists...
            if (!_repo.PlatformExists(platformId))
                return NotFound();

            var commands = _repo.GetCommandsForPlatform(platformId);
            return (Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commands)));
        }

        //platformId is a part of the controller route, but commandId is not, so it must be added to the route like this:
        [HttpGet("{commandId}", Name = "GetCommandForPlatform")]
        public ActionResult<CommandReadDto> GetCommandForPlatform(int platformId, int commandId)
        {
            Console.WriteLine($"--> Hit GetCommandForPlatform: {platformId} / {commandId}");

            //Check if the platform exists...
            if (!_repo.PlatformExists(platformId))
                return NotFound();

            var command = _repo.GetCommand(platformId, commandId);

            if (command == null)
                return NotFound();

            return (Ok(_mapper.Map<CommandReadDto>(command)));
        }

        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommandForPlatform(int platformId, CommandCreateDto commandDto)
        {
            Console.WriteLine($"--> Hit CreateCommandForPlatform: {platformId}");

            //Check if the platform exists...
            if (!_repo.PlatformExists(platformId))
                return NotFound();

            var command = _mapper.Map<Command>(commandDto);

            _repo.CreateCommand(platformId, command);
            _repo.SaveChanges();

            var commandReadDto = _mapper.Map<CommandReadDto>(command);
            //At this point, "command" has a valid id, because it has been saved by _repo.SaveChanges()

            //Create a CreatedAtRouteResult, that also contains a URL to the newly created resource.
            return CreatedAtRoute(nameof(GetCommandForPlatform),
                new { platformId = platformId, commandId = commandReadDto.Id }, commandReadDto);
        }
    }
}