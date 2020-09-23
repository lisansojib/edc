﻿using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore;
using ApplicationCore.DTOs;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces.Repositories;
using ApplicationCore.Interfaces.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Presentation.Admin.Models;
using Presentation.Interfaces;

namespace Presentation.Admin.Controllers.Api
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class ParticipantsController : ApiBaseController
    {
        private readonly IParticipantService _service;
        private readonly IEfRepository<Participant> _repository;
        private readonly ISelectOptionService _selectOptionService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IMapper _mapper;

        public ParticipantsController(IParticipantService service
            , IEfRepository<Participant> repository
            , ISelectOptionService selectOptionService
            , IPasswordHasher passwordHasher
            , IWebHostEnvironment hostEnvironment
            , IMapper mapper)
        {
            _service = service;
            _repository = repository;
            _selectOptionService = selectOptionService;
            _passwordHasher = passwordHasher;
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

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var entity = await _repository.FindAsync(id);

            if (entity == null) return BadRequest(new BadRequestResponseModel(ErrorTypes.BadRequest, ErrorMessages.ItemNotFound));

            var record = _mapper.Map<ParticipantDTO>(entity);
            record.CompanyList = await _selectOptionService.GetCompaniesAsync();

            return Ok(record);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ParticipantBindingModel model)
        {
            var entity = _mapper.Map<Participant>(model);
            entity.CreatedBy = UserId;

            if (model.Photo != null && model.Photo.Length > 0)
            {
                var fileName = model.Photo.FileName.ToUniqueFileName();
                var savePath = Path.Combine(_hostEnvironment.WebRootPath, UploadFolders.UPLOAD_PATH, UploadFolders.PARTICIPANTS, fileName);
                await model.Photo.CopyToAsync(new FileStream(savePath, FileMode.Create));

                entity.PhotoUrl = (new string[] { UploadFolders.UPLOAD_PATH, UploadFolders.PARTICIPANTS, fileName }).ToWebFilePath();
            }

            if(entity.Password.NotNullOrEmpty()) entity.Password = _passwordHasher.Hash(model.Password);

            await _repository.AddAsync(entity);

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromForm] ParticipantBindingModel model)
        {
            var entity = await _repository.FindAsync(model.Id);

            if (entity == null) return BadRequest(new BadRequestResponseModel(ErrorTypes.BadRequest, ErrorMessages.ItemNotFound));

            entity.Username = model.Username;
            entity.FirstName = model.FirstName;
            entity.LastName = model.LastName;
            entity.Email = model.Email;
            entity.Verified = model.Verified;
            entity.Phone = model.Phone;
            entity.Mobile = model.Mobile;
            entity.Title = model.Title;
            entity.Active = model.Active;
            entity.EmailCorp = model.EmailCorp;
            entity.PhoneCorp = model.PhoneCorp;
            entity.LinkedinUrl = model.LinkedinUrl;
            entity.CompanyId = model.CompanyId;

            if (model.Photo != null && model.Photo.Length > 0)
            {
                var fileName = model.Photo.FileName.ToUniqueFileName();
                var savePath = Path.Combine(_hostEnvironment.WebRootPath, UploadFolders.UPLOAD_PATH, UploadFolders.PARTICIPANTS, fileName);
                await model.Photo.CopyToAsync(new FileStream(savePath, FileMode.Create));

                entity.PhotoUrl = (new string[] { UploadFolders.UPLOAD_PATH, UploadFolders.PARTICIPANTS, fileName }).ToWebFilePath();
            }

            if (entity.Password.NotNullOrEmpty()) entity.Password = _passwordHasher.Hash(model.Password);

            entity.UpdatedBy = UserId;
            entity.UpdatedAt = DateTime.Now;

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

        [AllowAnonymous]
        [Route("delete-photo/{id}")]
        [HttpPost]
        public async Task<IActionResult> DeletePhoto(int id)
        {
            var entity = await _repository.FindAsync(id);

            if (entity == null) return BadRequest(new BadRequestResponseModel(ErrorTypes.BadRequest, ErrorMessages.ItemNotFound));

            var filePath = Path.Combine(_hostEnvironment.WebRootPath, UploadFolders.UPLOAD_PATH, UploadFolders.COMPANIES, Path.GetFileName(entity.PhotoUrl));
            System.IO.File.Delete(filePath);

            entity.PhotoUrl = "";
            await _repository.UpdateAsync(entity);
            return Ok(new { }); // required to pass empty object
        }
    }
}