using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    /// <summary>
    /// PlatformsController in CommandsService
    /// </summary>
    [Route("api/c/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly ICommandsRepo _repo;
        private readonly IMapper _mapper;

        public PlatformsController(ICommandsRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        //http://localhost:7119/api/c/platforms/
        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>>GetPlatforms()
        {
            Console.WriteLine("--> Getting platforms from commands service");
            var platforms = _repo.GetAllPlatforms();
            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platforms));
        }

        /// <summary>
        ///This is called from the CreatePlatform action in the Platform service, using the HttpClient
        ///This demonstrates the two services communicating internally
        /// http://localhost:5119/api/c/platforms/
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult TestInboundConnection()
        {
            Console.WriteLine("--> Inbound POST # Command Service");

            return Ok("Inbound test from platforms controller.");
        }
    }
}
