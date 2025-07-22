using Application.DTOs.SessionDtos;
using Application.DTOs.SnippetsDtos;
using Application.DTOs.UserDtos;
using AutoMapper;
using Core.Entities;

namespace Application.Mapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Snippet
        CreateMap<CodeSnippet, SnippetDto>();
        CreateMap<SnippetDto, CodeSnippet>()
            .ForMember(dest => dest.Owner, opt => opt.Ignore());
        CreateMap<CodeSnippet, SnippetDto>()
            .ForMember(dest => dest.OwnerName, opt => opt.MapFrom(src => src.Owner.Username));
        CreateMap<CodeSnippet, SnippetDetailsDto>();

        // User
        CreateMap<User, UserDto>();
        CreateMap<User, UserDetailsDto>()
            .ForMember(dest => dest.CollabSessions, opt => opt.MapFrom(src => src.CollabParticipants))
            .ForMember(dest => dest.CodeSnippets, opt => opt.MapFrom(src => src.CodeSnippets))
            .ForMember(dest => dest.OwnedSessions, opt => opt.MapFrom(src => src.OwnedSessions));

        // Collab Participant
        CreateMap<CollabParticipant, CollabParticipantDto>()
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.Username))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email));
        CreateMap<CollabParticipantDto, CollabParticipant>();

        // Session Edit History
        CreateMap<SessionEditHistory, SessionEditHistoryDto>()
            .ForMember(dest => dest.EditedByUsername, opt => opt.MapFrom(src => src.EditedByUser.Username));

        // Collab Session
        CreateMap<CollabSession, CollabSessionDto>()
            .ForMember(dest => dest.CodeSnippetTitle, opt => opt.MapFrom(src => src.CodeSnippet.Title))
            .ForMember(dest => dest.OwnerName, opt => opt.MapFrom(src => src.Owner.Username))
            .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.CodeSnippet.Content))
            .ForMember(dest => dest.SnippetContent, opt => opt.MapFrom(src => src.CodeSnippet.Content))
            .ForMember(dest => dest.EditHistories, opt => opt.MapFrom(src => src.EditHistories.OrderByDescending(h => h.EditedAt)))
            .ForMember(dest => dest.Participants, opt => opt.MapFrom(src => src.Participants));

        CreateMap<CollabSession, SessionDetailsDto>()
            .ForMember(dest => dest.Owner, opt => opt.MapFrom(src => src.Owner.Username))
            .ForMember(dest => dest.CodeSnippetTitle, opt => opt.MapFrom(src => src.CodeSnippet.Title));

        CreateMap<CollabSession, CollabSessionDetailsDto>()
            .ForMember(dest => dest.OwnerName, opt => opt.MapFrom(src => src.Owner.Username))
            .ForMember(dest => dest.CodeSnippetTitle, opt => opt.MapFrom(src => src.CodeSnippet.Title))
            .ForMember(dest => dest.CodeSnippetContent, opt => opt.MapFrom(src => src.CodeSnippet.Content))
            .ForMember(dest => dest.Participants, opt => opt.MapFrom(src => src.Participants))
            .ForMember(dest => dest.EditHistories, opt => opt.MapFrom(src => src.EditHistories));

        CreateMap<CollabSession, CollabSessionEditDto>()
            .ForMember(dest => dest.OwnerName, opt => opt.MapFrom(src => src.Owner.Username))
            .ForMember(dest => dest.CodeSnippetTitle, opt => opt.MapFrom(src => src.CodeSnippet.Title))
            .ForMember(dest => dest.Participants, opt => opt.MapFrom(src => src.Participants.Select(p => p.User.Username)));

        CreateMap<CollabSessionEditDto, CollabSession>()
            .ForMember(dest => dest.Participants, opt => opt.Ignore());

        // CollabParticipant to SessionDetailsDto
        CreateMap<CollabParticipant, SessionDetailsDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Session.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Session.Name))
            .ForMember(dest => dest.JoinCode, opt => opt.MapFrom(src => src.Session.JoinCode))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.Session.IsActive))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.Session.CreatedAt))
            .ForMember(dest => dest.ExpiresAt, opt => opt.MapFrom(src => src.Session.ExpiresAt))
            .ForMember(dest => dest.EditedAt, opt => opt.MapFrom(src => src.Session.EditedAt))
            .ForMember(dest => dest.Owner, opt => opt.MapFrom(src => src.Session.Owner.Username))
            .ForMember(dest => dest.CodeSnippetTitle, opt => opt.MapFrom(src => src.Session.CodeSnippet.Title));
    }
}
