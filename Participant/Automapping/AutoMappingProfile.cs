using Presentation.Participant.Models;

namespace Presentation.Participant.Automapping
{
    public class AutoMappingProfile : AutoMapper.Profile
    {
        public AutoMappingProfile()
        {
            CreateMap<RegisterBindingModel, ApplicationCore.Entities.Participant>();
        }
    }
}
