using ApplicationCore.DTOs;
using ApplicationCore.Entities;
using Presentation.Admin.Models;
using Presentation.Admin.Models.Home;
using System.Linq;

namespace Presentation.Admin.Automapping
{
    public class AutoMappingProfile : AutoMapper.Profile
    {
        public AutoMappingProfile()
        {
            CreateMap<RegisterBindingModel, ApplicationCore.Entities.Admin>();

            CreateMap<Announcement, AnnouncementDTO>();

            CreateMap<AnnouncementBindingModel, Announcement>();

            CreateMap<Event, EventDTO>();

            CreateMap<EventResourceBindingModel, EventResource>();

            CreateMap<EventBindingModel, Event>()
                .ForMember(dest => dest.EventSpeakers, opt => opt.Ignore())
                .ForMember(dest => dest.EventSponsors, opt => opt.Ignore())
                .ForMember(dest => dest.EventResources, opt => opt.MapFrom(src => src.Resources));

            CreateMap<Poll, PollDTO>();

            CreateMap<PollBindingModel, Poll>();

            CreateMap<ValueField, Select2Option>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Desc, opt => opt.MapFrom(src => src.Description));

            CreateMap<Event, EventViewModel>()
                .ForMember(dest => dest.SpeakerIds, opt => opt.MapFrom(src => src.EventSpeakers.Select(x => x.SpeakerId.ToString())))
                .ForMember(dest => dest.SponsorIds, opt => opt.MapFrom(src => src.EventSponsors.Select(x => x.SponsorId.ToString())));

            CreateMap<Team, TeamViewModel>()
                .ForMember(dest => dest.Participants, opt => opt.MapFrom(src => src.ParticipantTeams.Select(x => x.TeamMemberId.ToString())));

            CreateMap<Participant, ParticipantTeamDTO>();

            CreateMap<TeamBindingModel, Team>()
                .ForMember(dest => dest.ParticipantTeams, opt => opt.MapFrom(src => src.Participants.Select(x => new ParticipantTeam { TeamMemberId = x})));

            CreateMap<Participant, Select2Option>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Email));

            CreateMap<Company, CompanyDTO>();

            CreateMap<CompanyBindingModel, Company>();

            CreateMap<Participant, ParticipantDTO>();

            CreateMap<ParticipantBindingModel, Participant>();

            CreateMap<Sponsor, SponsorDTO>();

            CreateMap<SponsorBindingModel, Sponsor>();

            CreateMap<Speaker, SpeakerDTO>();

            CreateMap<SpeakerBindingModel, Speaker>();

            CreateMap<PendingSpeaker, PendingSpeakerDTO>();

            CreateMap<PendingSpeakerBindingModel, PendingSpeaker>();

        }
    }
}
