using AutoMapper;
using CommandsService.Dtos;
using CommandsService.Models;

namespace CommandsService.MappingProfiles
{
    public class CommandsProfile : Profile
    {
        public CommandsProfile()
        {
            CreateMap<Platform, PlatformReadDto>().ReverseMap();
            CreateMap<Command, CommandReadDto>().ReverseMap();
            CreateMap<Command, CommandCreateDto>().ReverseMap();
        }
    }
}
