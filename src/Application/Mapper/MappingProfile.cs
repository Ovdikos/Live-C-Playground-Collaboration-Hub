using Application.DTOs;
using AutoMapper;
using Core.Entities;

namespace Application.Mapper;

public class MappingProfile : Profile
{

    public MappingProfile()
    {
        CreateMap<CodeSnippet, SnippetDto>();
    }
    
}