using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces.Repositories;
using ApplicationCore.Interfaces.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IMapper _mapper;

        public EventsController(IEventService service
            , IEfRepository<Event> repository
            , IEventValueFieldsService eventValueFieldsService
            , IWebHostEnvironment hostEnvironment
            , IMapper mapper)
        {
            _service = service;
            _repository = repository;
            _eventValueFieldsService = eventValueFieldsService;
            _hostEnvironment = hostEnvironment;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int offset = 0, int limit = 10, string filter = null, string sort = null, string order = null)
        {
            var orderBy = string.IsNullOrEmpty(sort) ? "" : $"ORDER BY {sort} {order}";
            var records = await _service.GetPagedAsync(offset, limit, filter, orderBy);

            var response = new PagedListViewModel(records, records.FirstOrDefault()?.Total);

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
            var speakerIds = await _eventValueFieldsService.GetSpeakerIdsAsync(model.Speakers);
            var sponsorIds = await _eventValueFieldsService.GetSponsorIdsAsync(model.Sponsors);

            var entity = _mapper.Map<Event>(model);
            entity.CreatedBy = UserId;

            speakerIds.ForEach(x => entity.EventSpeakers.Add(new EventSpeaker { SpeakerId = x }));
            sponsorIds.ForEach(x => entity.EventSponsors.Add(new EventSponsor { SponsorId = x }));

            if (model.SessionId.NullOrEmpty()) entity.SessionId = Guid.NewGuid().ToString();

            entity.EventFolder = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
            var folderPath = Path.Combine(_hostEnvironment.WebRootPath, UploadFolders.UPLOAD_PATH, UploadFolders.EVENTS, entity.EventFolder);
            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
            int i = 0;
            foreach (var item in model.Files)
            {
                var filename = item.FileName.ToUniqueFileName();
                var savePath = Path.Combine(_hostEnvironment.WebRootPath, UploadFolders.UPLOAD_PATH, UploadFolders.EVENTS, entity.EventFolder, filename);
                await item.CopyToAsync(new FileStream(savePath, FileMode.Create));

                entity.EventResources[i].FilePath = new string[] { UploadFolders.UPLOAD_PATH, UploadFolders.EVENTS, entity.EventFolder, filename }.ToWebFilePath();
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
    }
}