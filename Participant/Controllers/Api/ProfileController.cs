using ApplicationCore;
using ApplicationCore.DTOs;
using ApplicationCore.Interfaces.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Presentation.Participant.Interfaces;
using Presentation.Participant.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Presentation.Participant.Controllers.Api
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ApiBaseController
    {
        private readonly IEfRepository<ApplicationCore.Entities.Participant> _participantRepository;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IImageHelper _imageHelper;
        private readonly IMapper _mapper;

        public ProfileController(IEfRepository<ApplicationCore.Entities.Participant> participantRepository
            , IMapper mapper
            , IWebHostEnvironment hostEnvironment
            , IImageHelper imageHelper)
        {
            _participantRepository = participantRepository;
            _mapper = mapper;
            _hostEnvironment = hostEnvironment;
            _imageHelper = imageHelper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var entity = await _participantRepository.FindAsync(UserId);

            if (entity == null) return BadRequest(new BadRequestResponseModel(ErrorTypes.BadRequest, ErrorMessages.ItemNotFound));

            var record = _mapper.Map<ParticipantDTO>(entity);

            return Ok(record);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromForm] ParticipantBindingModel model)
        {
            var entity = await _participantRepository.FindAsync(UserId);

            if (entity == null) return BadRequest(new BadRequestResponseModel(ErrorTypes.BadRequest, ErrorMessages.ItemNotFound));

            entity.Title = model.Title;
            entity.FirstName = model.FirstName;
            entity.LastName = model.LastName;
            entity.EmailCorp = model.EmailCorp;
            entity.EmailPersonal = model.EmailPersonal;
            entity.Phone = model.Phone;
            entity.PhoneCorp = model.PhoneCorp;
            entity.Active = model.Active;
            entity.LinkedinUrl = model.LinkedinUrl;
            entity.CompanyName = model.CompanyName;

            if (model.Photo != null && model.Photo.Length > 0)
            {
                var fileName = model.Photo.FileName.ToUniqueFileName();
                var savePath = Path.Combine(_hostEnvironment.WebRootPath, UploadFolders.UPLOAD_PATH, UploadFolders.PARTICIPANTS, fileName);
                await model.Photo.CopyToAsync(new FileStream(savePath, FileMode.Create));

                entity.PhotoUrl = (new string[] { UploadFolders.UPLOAD_PATH, UploadFolders.PARTICIPANTS, fileName }).ToWebFilePath();

                // Create Thumbnail Image
                var thumbnailImagePath = fileName.ToThumbnailImagePath();
                savePath = Path.Combine(_hostEnvironment.WebRootPath, UploadFolders.UPLOAD_PATH, UploadFolders.PARTICIPANTS, thumbnailImagePath);
                var thumbnailImage = _imageHelper.ResizeImage(model.Photo, 100, 100, false);
                using var fileStream = new FileStream(savePath, FileMode.Create);
                await thumbnailImage.CopyToAsync(fileStream);
            }

            entity.UpdatedBy = UserId;
            entity.UpdatedAt = DateTime.Now;

            await _participantRepository.UpdateAsync(entity);

            return Ok();
        }
    }
}
