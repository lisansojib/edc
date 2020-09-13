using ApplicationCore.DTOs;
using ApplicationCore.Entities;
using Presentation.Admin.Models;

namespace Presentation.Admin.Automapping
{
    public class AutoMappingProfile : AutoMapper.Profile
    {
        public AutoMappingProfile()
        {
            CreateMap<RegisterBindingModel, ApplicationCore.Entities.Admin>();

            CreateMap<Announcement, AnnouncementDTO>();

            CreateMap<AnnouncementBindingModel, Announcement>();
        }
    }
}
