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

            //Here, we want to explicitly map the PlatformPublishedDto Id, to the Platform ExternalId
            CreateMap<PlatformPublishedDto, Platform>().ForMember(dest => dest.ExternalID, opt => opt.MapFrom(src => src.Id));
        }
    }
}
