using System;
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

namespace Presentation.Admin.Controllers.Api
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class SponsorsController : ApiBaseController
    {
        private readonly ISponsorService _service;
        private readonly IEfRepository<Sponsor> _repository;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IMapper _mapper;

        public SponsorsController(ISponsorService service
            , IEfRepository<Sponsor> repository
            , IWebHostEnvironment hostEnvironment
            , IMapper mapper)
        {
            _service = service;
            _repository = repository;
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

            var record = _mapper.Map<SponsorDTO>(entity);

            return Ok(record);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] SponsorBindingModel model)
        {
            var entity = _mapper.Map<Sponsor>(model);
            entity.CreatedBy = UserId;

            if(model.Logo != null && model.Logo.Length > 0)
            {
                var fileName = model.Logo.FileName.ToUniqueFileName();
                var savePath = Path.Combine(_hostEnvironment.WebRootPath, UploadFolders.UPLOAD_PATH, UploadFolders.SPONSORS, fileName);
                await model.Logo.CopyToAsync(new FileStream(savePath, FileMode.Create));

                entity.LogoUrl = (new string[] { UploadFolders.UPLOAD_PATH, UploadFolders.SPONSORS, fileName }).ToWebFilePath();
            }

            await _repository.AddAsync(entity);

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromForm] SponsorBindingModel model)
        {
            var entity = await _repository.FindAsync(model.Id);

            if (entity == null) return BadRequest(new BadRequestResponseModel(ErrorTypes.BadRequest, ErrorMessages.ItemNotFound));

            entity.CompanyName = model.CompanyName;
            entity.ContactPerson = model.ContactPerson;
            entity.ContactPersonEmail = model.ContactPersonEmail;
            entity.ContactPersonPhone = model.ContactPersonPhone;
            entity.Description = model.Description;
            entity.Website = model.Website;

            if (model.Logo != null && model.Logo.Length > 0)
            {
                var fileName = model.Logo.FileName.ToUniqueFileName();
                var savePath = Path.Combine(_hostEnvironment.WebRootPath, UploadFolders.UPLOAD_PATH, UploadFolders.SPONSORS, fileName);
                await model.Logo.CopyToAsync(new FileStream(savePath, FileMode.Create));

                entity.LogoUrl = (new string[] { UploadFolders.UPLOAD_PATH, UploadFolders.SPONSORS, fileName }).ToWebFilePath();
            }

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

        [AllowAnonymous]
        [Route("delete-photo/{id}")]
        [HttpPost]
        public async Task<IActionResult> DeletePhoto(int id)
        {
            var entity = await _repository.FindAsync(id);

            if (entity == null) return BadRequest(new BadRequestResponseModel(ErrorTypes.BadRequest, ErrorMessages.ItemNotFound));

            var filePath = Path.Combine(_hostEnvironment.WebRootPath, UploadFolders.UPLOAD_PATH, UploadFolders.SPONSORS, Path.GetFileName(entity.LogoUrl));
            System.IO.File.Delete(filePath);

            entity.LogoUrl = "";
            await _repository.UpdateAsync(entity);
            return Ok(new { }); // required to pass empty object
        }
    }
}