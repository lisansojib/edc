using ApplicationCore.DTOs;
using ApplicationCore.Entities;
using Presentation.Participant.Models;

namespace Presentation.Participant.Automapping
{
    public class AutoMappingProfile : AutoMapper.Profile
    {
        public AutoMappingProfile()
        {
            CreateMap<RegisterBindingModel, ApplicationCore.Entities.Participant>();

            CreateMap<ApplicationCore.Entities.Participant, ParticipantDTO>()
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company.Name));

            CreateMap<ApplicationCore.Entities.Participant, ProfileViewModel>();

            CreateMap<PendingSepakerBindingModel, PendingSepaker>();
        }
    }
}
