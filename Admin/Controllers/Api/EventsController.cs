using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore;
using ApplicationCore.DTOs;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces.Repositories;
using ApplicationCore.Interfaces.Services;
using ApplicationCore.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Presentation.Admin.Interfaces;
using Presentation.Admin.Models;
using Presentation.Admin.Models.Home;

namespace Presentation.Admin.Controllers.Api
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ApiBaseController
    {
        private readonly IEventService _service;
        private readonly IEfRepository<Event> _repository;
        private readonly IEventValueFieldsService _eventValueFieldsService;
        private readonly IZoomApiService _zoomApiService;
        private readonly IEmailService _emailService;
        private readonly IEfRepository<ZoomMeeting> _zoomMeetingRepository;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public EventsController(IEventService service
            , IEfRepository<Event> repository
            , IEventValueFieldsService eventValueFieldsService
            , IWebHostEnvironment hostEnvironment
            , IMapper mapper
            , IZoomApiService zoomApiService
            , IConfiguration configuration
            , IEmailService emailService
            , IEfRepository<ZoomMeeting> zoomMeetingRepository)
        {
            _service = service;
            _repository = repository;
            _eventValueFieldsService = eventValueFieldsService;
            _hostEnvironment = hostEnvironment;
            _mapper = mapper;
            _zoomApiService = zoomApiService;
            _configuration = configuration;
            _emailService = emailService;
            _zoomMeetingRepository = zoomMeetingRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int offset = 0, int limit = 10, string filter = null, string sort = null, string order = null)
        {
            var orderBy = string.IsNullOrEmpty(sort) ? "" : $"ORDER BY {sort} {order}";
            var records = await _service.GetPagedAsync(offset, limit, filter, orderBy);

            var response = new PagedListViewModel<EventDTO>(records, records.FirstOrDefault()?.Total);

            return Ok(response);
        }

        [HttpGet("new")]
        public async Task<IActionResult> GetNew()
        {
            return Ok(await _service.GetNewAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var entity = await _repository.QueryableAll(x => x.Id == id).Include(x => x.EventSpeakers).Include(x => x.EventSponsors).FirstOrDefaultAsync();

            if (entity == null) return BadRequest(new BadRequestResponseModel(ErrorTypes.BadRequest, ErrorMessages.ItemNotFound));

            var model = _mapper.Map<EventViewModel>(entity);

            var evtData = await _service.GetNewAsync();
            model.CohortList = evtData.CohortList;
            model.SpeakerList = evtData.SpeakerList;
            model.SponsorList = evtData.SponsorList;
            model.EventTypeList = evtData.EventTypeList;
            model.CTOList = evtData.CTOList;
            model.PresenterList = evtData.PresenterList;

            return Ok(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] EventBindingModel model)
        {
            ZoomMeeting zoomMeeting = null;
            if (model.CreateZoomMeeting)
            {
                var createZoomMeetingModel = new CreateingZoomMeetingDTO
                {
                    Topic = model.Title,
                    Agenda = model.Description,
                    Duration = Constants.DEFAULT_ZOOM_MEETING_DURATION,
                    StartTime = model.EventDate
                };

                createZoomMeetingModel.Settings = new ZoomMeetingSettings
                {
                    ParticipantVideo = false,
                    //AlternativeHosts = Username
                };

                var response = await _zoomApiService.CreateMeetingAsync(ZoomUserId, createZoomMeetingModel);
                if (response.StatusCode != System.Net.HttpStatusCode.Created) return BadRequest(response.ErrorMessage);

                var zoomMeetingInfo = JsonConvert.DeserializeObject<ZoomMeetingDTO>(response.Content);
                zoomMeeting = _mapper.Map<ZoomMeeting>(zoomMeetingInfo);
                zoomMeeting.CreatedBy = UserId;
                await _zoomMeetingRepository.AddAsync(zoomMeeting);
            }

            var speakerIds = await _eventValueFieldsService.GetSpeakerIdsAsync(model.Speakers);
            var sponsorIds = await _eventValueFieldsService.GetSponsorIdsAsync(model.Sponsors);

            var entity = _mapper.Map<Event>(model);
            entity.CreatedBy = UserId;

            speakerIds.ForEach(x => entity.EventSpeakers.Add(new EventSpeaker { SpeakerId = x }));
            sponsorIds.ForEach(x => entity.EventSponsors.Add(new EventSponsor { SponsorId = x }));

            if (model.SessionId.NullOrEmpty()) entity.SessionId = Guid.NewGuid().ToString();

            if(model.Files != null && model.Files.Count() > 0)
            {
                entity.EventFolder = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
                var folderPath = Path.Combine(_hostEnvironment.WebRootPath, UploadFolders.UPLOAD_PATH, UploadFolders.EVENTS, entity.EventFolder);
                if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

                int i = 0;
                foreach (var item in model.Files)
                {
                    var filename = item.FileName.ToUniqueFileName();
                    var savePath = Path.Combine(_hostEnvironment.WebRootPath, UploadFolders.UPLOAD_PATH, UploadFolders.EVENTS, entity.EventFolder, filename);
                    await item.CopyToAsync(new FileStream(savePath, FileMode.Create));

                    entity.EventResources[i].Title = entity.EventResources[i].Title.NullOrEmpty() ? "" : entity.EventResources[i].Title;
                    entity.EventResources[i].FilePath = new string[] { UploadFolders.UPLOAD_PATH, UploadFolders.EVENTS, entity.EventFolder, filename }.ToWebFilePath();
                    entity.EventResources[i].PreviewType = filename.Contains(".pdf") ? "pdf" : "image";
                }
            }

            if(zoomMeeting != null)
            {
                entity.MeetingId = zoomMeeting.Id;
                entity.MeetingPassword = zoomMeeting.Password;
            }

            await _repository.AddAsync(entity);

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromForm] EventBindingModel model)
        {
            var entity = await _repository.QueryableAll(x => x.Id == model.Id).Include(x => x.EventSpeakers).Include(x => x.EventSponsors).FirstOrDefaultAsync();

            if (entity == null) return BadRequest(new BadRequestResponseModel(ErrorTypes.BadRequest, ErrorMessages.ItemNotFound));

            var speakerIds = await _eventValueFieldsService.GetSpeakerIdsAsync(model.Speakers);
            var sponsorIds = await _eventValueFieldsService.GetSponsorIdsAsync(model.Sponsors);

            // Update Speakers
            foreach(var item in speakerIds)
            {
                if (entity.EventSpeakers.Any(x => x.SpeakerId == item)) continue;

                entity.EventSpeakers.Add(new EventSpeaker { SpeakerId = item });
            }

            // Update sponsors
            foreach (var item in sponsorIds)
            {
                if (entity.EventSponsors.Any(x => x.SponsorId == item)) continue;

                entity.EventSponsors.Add(new EventSponsor { SponsorId = item });
            }

            var deletedSpeakers = entity.EventSpeakers.Where(x => !speakerIds.Contains(x.SpeakerId)).ToList();
            deletedSpeakers.ForEach(x => entity.EventSpeakers.Remove(x));

            var deletedSponsors = entity.EventSponsors.Where(x => !sponsorIds.Contains(x.SponsorId)).ToList();
            deletedSponsors.ForEach(x => entity.EventSponsors.Remove(x));

            entity.Title = model.Title;
            entity.CohortId = model.CohortId;
            entity.EventTypeId = model.EventTypeId;
            entity.PresenterId = model.PresenterId;
            entity.SessionId = model.SessionId;
            entity.CTOId = model.CTOId;
            entity.Description = model.Description;
            entity.EventDate = model.EventDate;
            entity.UpdatedAt = DateTime.Now;
            entity.UpdatedBy = UserId;

            await _repository.UpdateAsync(entity);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _repository.FindAsync(id);

            if (entity == null) return BadRequest(new BadRequestResponseModel(ErrorTypes.BadRequest, ErrorMessages.ItemNotFound));

            await _repository.DeleteAsync(entity);

            return Ok();
        }

        [HttpPost("share-link")]
        public async Task<IActionResult> ShareLink(ShareEventLinkBindingModel model)
        {
            var guestSessionBaseUrl = _configuration.GetSection("GuestSessionBaseUrl");
            var sessionLink = guestSessionBaseUrl.Value + "?eventId=" + model.EventId;
            var messageBody = $@"Greetings,
                <br><br>
                Here is the link to connect to <b>{model.EventTitle}</b>.
                Please <a href='{sessionLink}'>follow this link</a> to directly connect to the event.
                <br><br>
                Best Regards,
                <br>
                {Username}";

            await _emailService.SendEmailAsync(Username, string.Join(',', model.GuestEmails), $"Invitation to join \"{model.EventTitle}\"", messageBody, false);
            return Ok();
        }
    }
}