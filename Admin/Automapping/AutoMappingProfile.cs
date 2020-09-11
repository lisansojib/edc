using Presentation.Admin.Models;

namespace Presentation.Admin.Automapping
{
    public class AutoMappingProfile : AutoMapper.Profile
    {
        public AutoMappingProfile()
        {
            CreateMap<RegisterBindingModel, ApplicationCore.Entities.Admin>();
        }
    }
}
