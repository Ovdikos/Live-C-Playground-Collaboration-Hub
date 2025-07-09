using Application.DTOs;
using AutoMapper;
using Core.Entities;

namespace Application.Mapper;

public class MappingProfile : Profile
{

    public MappingProfile()
    {
        CreateMap<CodeSnippet, SnippetDto>();
        CreateMap<User, UserDto>();
        CreateMap<CodeSnippet, SnippetDto>()
            .ForMember(dest => dest.OwnerName, opt => 
                opt.MapFrom(src => src.Owner.Username));
    }
    
}