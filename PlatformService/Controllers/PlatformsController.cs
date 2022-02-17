using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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

        public PlatformsController(IPlatformRepo repo, IMapper mapper, ICommandDataClient commandDataClient)
        {
            _repo = repo;
            _mapper = mapper;
            _commandDataClient = commandDataClient;//This willl be used to communicate with the CommandService Microservice.
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

            var platformReadDto = _mapper.Map<PlatformReadDto>(platform);

            try
            {
                //Call PlatformsController in CommandService, and send the newly created platform to it.
                await _commandDataClient.SendPlatformToCommand(platformReadDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not send syncronously: {ex.Message}", ex.Message);
            }

            return CreatedAtRoute("GetPlatformById", new {Id= platformReadDto.Id}, platformReadDto);
        }
    }
}
