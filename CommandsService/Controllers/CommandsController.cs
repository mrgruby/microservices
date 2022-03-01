using AutoMapper;
using CommandsService.Data;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    public class CommandsController : ControllerBase
    {
        private readonly ICommandsRepo _repo;
        private readonly IMapper _mapper;

        public CommandsController(ICommandsRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
    }
}
