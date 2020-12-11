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
using Presentation.Admin.Models;

namespace Presentation.Admin.Controllers.Api
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ApiBaseController
    {
        private readonly ICompanyService _service;
        private readonly IEfRepository<Company> _repository;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IMapper _mapper;

        public CompaniesController(ICompanyService service
            , IEfRepository<Company> repository
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

            var response = new PagedListViewModel<CompanyDTO>(records, records.FirstOrDefault()?.Total);

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var entity = await _repository.FindAsync(id);

            if (entity == null) return BadRequest(new BadRequestResponseModel(ErrorTypes.BadRequest, ErrorMessages.ItemNotFound));

            var record = _mapper.Map<CompanyDTO>(entity);

            return Ok(record);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CompanyBindingModel model)
        {
            var entity = _mapper.Map<Company>(model);
            entity.CreatedBy = UserId;

            if(model.Logo != null && model.Logo.Length > 0)
            {
                var fileName = model.Logo.FileName.ToUniqueFileName();
                var savePath = Path.Combine(_hostEnvironment.WebRootPath, UploadFolders.UPLOAD_PATH, UploadFolders.COMPANIES, fileName);
                await model.Logo.CopyToAsync(new FileStream(savePath, FileMode.Create));

                entity.LogoUrl = (new string[] { UploadFolders.UPLOAD_PATH, UploadFolders.COMPANIES, fileName }).ToWebFilePath();
            }

            await _repository.AddAsync(entity);

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromForm] CompanyBindingModel model)
        {
            var entity = await _repository.FindAsync(model.Id);

            if (entity == null) return BadRequest(new BadRequestResponseModel(ErrorTypes.BadRequest, ErrorMessages.ItemNotFound));

            entity.Name = model.Name;
            entity.Address = model.Address;
            entity.Phone = model.Phone;
            entity.Website = model.Website;

            if (model.Logo != null && model.Logo.Length > 0)
            {
                var fileName = model.Logo.FileName.ToUniqueFileName();
                var savePath = Path.Combine(_hostEnvironment.WebRootPath, UploadFolders.UPLOAD_PATH, UploadFolders.COMPANIES, fileName);
                await model.Logo.CopyToAsync(new FileStream(savePath, FileMode.Create));

                entity.LogoUrl = (new string[] { UploadFolders.UPLOAD_PATH, UploadFolders.COMPANIES, fileName }).ToWebFilePath();
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

            var filePath = Path.Combine(_hostEnvironment.WebRootPath, UploadFolders.UPLOAD_PATH, UploadFolders.COMPANIES, Path.GetFileName(entity.LogoUrl));
            System.IO.File.Delete(filePath);

            entity.LogoUrl = "";
            await _repository.UpdateAsync(entity);
            return Ok(new { }); // required to pass empty object
        }
    }
}