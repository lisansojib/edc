using ApplicationCore.DTOs;
using ApplicationCore.Entities;
using Presentation.Admin.Models;
using Presentation.Admin.Models.Home;
using System.Linq;
using System.Security.Cryptography;

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

            CreateMap<EventBindingModel, Event>()
                .ForMember(dest => dest.Speakers, opt => opt.Ignore())
                .ForMember(dest => dest.Sponsors, opt => opt.Ignore());

            CreateMap<Poll, PollDTO>();

            CreateMap<PollBindingModel, Poll>();

            CreateMap<ValueField, Select2Option>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Desc, opt => opt.MapFrom(src => src.Description));

            CreateMap<Event, EventViewModel>()
                .ForMember(dest => dest.Speakers, opt => opt.MapFrom(src => src.Speakers.Select(x => x.SpeakerId.ToString())))
                .ForMember(dest => dest.Sponsors, opt => opt.MapFrom(src => src.Sponsors.Select(x => x.SponsorId.ToString())));
        }
    }
}
