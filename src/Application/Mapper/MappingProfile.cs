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
        CreateMap<CollabParticipant, CollabParticipantDto>()
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.Username));
        CreateMap<SessionEditHistory, SessionEditHistoryDto>()
            .ForMember(dest => dest.EditedByUsername, opt => opt.MapFrom(src => src.EditedByUser.Username));
        CreateMap<CollabSession, CollabSessionDto>()
            .ForMember(dest => dest.CodeSnippetTitle, opt => opt.MapFrom(src => src.CodeSnippet.Title))
            .ForMember(dest => dest.OwnerName, opt => opt.MapFrom(src => src.Owner.Username))
            .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.CodeSnippet.Content))
            .ForMember(dest => dest.SnippetContent, opt => opt.MapFrom(src => src.CodeSnippet.Content))
            .ForMember(dest => dest.EditHistories, opt => opt.MapFrom(src => src.EditHistories.OrderByDescending(h => h.EditedAt)))
            .ForMember(dest => dest.Participants, opt => opt.MapFrom(src => src.Participants));
        CreateMap<User, UserDetailsDto>()
            .ForMember(dest => dest.CollabSessions, opt => opt.MapFrom(src => src.CollabParticipants.Select(cp => cp.Session)))
            .ForMember(dest => dest.CodeSnippets, opt => opt.MapFrom(src => src.CodeSnippets))
            .ForMember(dest => dest.OwnedSessions, opt => opt.MapFrom(src => src.OwnedSessions));
        CreateMap<CodeSnippet, SnippetDetailsDto>();
        CreateMap<CollabSession, SessionDetailsDto>()
            .ForMember(dest => dest.Owner, opt => opt.MapFrom(src => src.Owner.Username))
            .ForMember(dest => dest.CodeSnippetTitle, opt => opt.MapFrom(src => src.CodeSnippet.Title));
    }
    
}