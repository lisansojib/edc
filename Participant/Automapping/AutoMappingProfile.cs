using ApplicationCore.Entities;
using Presentation.Models;

namespace Presentation.Automapping
{
    public class AutoMappingProfile : AutoMapper.Profile
    {
        public AutoMappingProfile()
        {
            CreateMap<RegisterBindingModel, Participant>();
        }
    }
}
