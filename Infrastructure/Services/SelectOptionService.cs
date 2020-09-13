using ApplicationCore.DTOs;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces.Repositories;
using ApplicationCore.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class SelectOptionService : ISelectOptionService
    {
        private readonly AppDbContext _dbContext;

        public SelectOptionService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Select2Option>> GetCompaniesAsync()
        {
            var _dbSet = await _dbContext.Set<Company>().ToListAsync();
            return _dbSet.Select(x => new Select2Option { Id = x.Id.ToString(), Text = x.Name });
        }
    }
}
