using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.AsyncDataServices;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepo _repo;
        private readonly IMapper _mapper;
        private readonly ICommandDataClient _commandDataClient;
        private readonly IMessageBusClient _messageBusClient;

        public PlatformsController(IPlatformRepo repo, IMapper mapper, ICommandDataClient commandDataClient, IMessageBusClient messageBusClient)
        {
            _repo = repo;
            _mapper = mapper;
            _commandDataClient = commandDataClient;//This will be used to communicate with the CommandService Microservice.
            _messageBusClient = messageBusClient;//This will be used to communicate with the CommandService Microservice, using the RabbitMQ MessageBus.
        }

        //http://localhost:5278/api/platforms
        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetAllPlatforms()
        {
            var res = _repo.GetAllPlatforms();

            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(res));
        }

        //http://localhost:5278/api/platforms/{id}
        [HttpGet("{id}", Name = "GetPlatformById")]
        public ActionResult<PlatformReadDto> GetPlatformById(int id)
        {
            var res = _repo.GetPlatformById(id);

            if (res == null)
                return NotFound();

            return Ok(_mapper.Map<PlatformReadDto>(res));
        }

        [HttpPost]
        public async Task<ActionResult<PlatformReadDto>>CreatePlatform(PlatformCreateDto platformCreateDto)
        {
            var platform = _mapper.Map<Platform>(platformCreateDto);
            _repo.CreatePlatform(platform);
            _repo.SaveChanges();

            //Map to a PlatformReadDto. This will be used to send to the commandservice, using the messagebus. It will also
            //be used to return according to the REST rules
            var platformReadDto = _mapper.Map<PlatformReadDto>(platform);

            //Send Sync
            try
            {
                //Call PlatformsController syncronously in CommandService, and send the newly created platform to it.
                await _commandDataClient.SendPlatformToCommand(platformReadDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not send syncronously: {ex.Message}", ex.Message);
            }

            //Send Async
            try
            {
                //Call PlatformsController asyncronously in CommandService, and send the newly created platform to it.
                //This will be using the RabbitMQ MessageBus
                var platformPublishedDto = _mapper.Map<PlatformPublishedDto>(platformReadDto);
                platformPublishedDto.Event = "Platform_Published";
                _messageBusClient.PublishNewPlatform(platformPublishedDto);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not send asyncronously: {ex.Message}", ex.Message);
            }

            return CreatedAtRoute("GetPlatformById", new {Id= platformReadDto.Id}, platformReadDto);
        }
    }
}
