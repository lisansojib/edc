﻿using ApplicationCore.DTOs;
using ApplicationCore.Entities;
using Presentation.Participant.Models;

namespace Presentation.Participant.Automapping
{
    public class AutoMappingProfile : AutoMapper.Profile
    {
        public AutoMappingProfile()
        {
            CreateMap<RegisterBindingModel, ApplicationCore.Entities.Participant>();

            CreateMap<ApplicationCore.Entities.Participant, ParticipantDTO>();

            CreateMap<ApplicationCore.Entities.Participant, PubnubUserViewModel>();

            CreateMap<PendingSpeakerBindingModel, PendingSpeaker>();

            CreateMap<ApplicationCore.Entities.Participant, ParticipantViewModel>();

            CreateMap<ApplicationCore.Entities.Participant, UserViewModel>();

            CreateMap<Guest, UserViewModel>();

            CreateMap<ApplicationCore.Entities.Participant, ZoomUserInfo>();

            CreateMap<PollDataPoint, PollDataPointDTO>();
            CreateMap<ApplicationCore.Entities.Participant, ProfileDTO>();
        }
    }
}
