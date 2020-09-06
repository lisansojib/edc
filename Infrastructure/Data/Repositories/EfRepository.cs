using ApplicationCore.DTOs;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using ApplicationCore.Specifications;

namespace Infrastructure.Data.Repositories
{
    public class EfRepository<T> : IEfRepository<T> where T : BaseEntity
    {
        protected readonly AppDbContext _dbContext;
        protected readonly DbSet<T> _dbSet;

        public EfRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public T Add(T entity)
        {
            _dbSet.Add(entity);
            _dbContext.SaveChanges();
            return entity;
        }

        public async Task<T> AddAsync(T entity)
        {
            _dbSet.Add(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public int Count()
        {
            return _dbSet.Count();
        }

        public async Task<int> CountAsync()
        {
            return await _dbSet.CountAsync();
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> criteria)
        {
            return await _dbSet.Where(criteria).CountAsync();
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
            _dbContext.SaveChanges();
        }

        public async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteManyAsync(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
            await _dbContext.SaveChangesAsync();
        }

        public bool Exists(int id)
        {
            return _dbSet.Any(x => x.Id == id);
        }

        public bool Exists(Expression<Func<T, bool>> criteria)
        {
            return _dbSet.Any(criteria);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _dbSet.AnyAsync(x => x.Id == id);
        }

        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> criteria)
        {
            return await _dbSet.AnyAsync(criteria);
        }

        public T Find(int id)
        {
            return _dbSet.Find(id);
        }

        public T Find(Expression<Func<T, bool>> criteria)
        {
            return _dbSet.FirstOrDefault(criteria);
        }

        public async Task<T> FindAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<T> FindAsync(Expression<Func<T, bool>> criteria)
        {
            return await _dbSet.FirstOrDefaultAsync(criteria);
        }

        public async Task<T> FindAsyncWithInclude(int id, Expression<Func<T, object>> include)
        {
            if (include != null)
            {
                return await _dbSet.Include(include).FirstOrDefaultAsync(x => x.Id == id);
            }
            return await _dbSet.FindAsync(id);
        }

        public async Task<List<T>> FindAllAsync(Expression<Func<T, bool>> criteria, Expression<Func<T, object>> include)
        {
            if (include != null)
            {
                return await _dbSet.Include(include).Where(criteria).ToListAsync();
            }
            else
            {
                return await _dbSet.Where(criteria).ToListAsync();
            }
        }
        public int GetMaxId()
        {
            var entity = _dbSet.OrderByDescending(x => x.Id).FirstOrDefault();
            if (entity != null)
                return entity.Id + 1;
            return 1;
        }

        public List<T> ListAll()
        {
            return _dbSet.ToList();
        }

        public IQueryable<T> QueryableAll()
        {
            return _dbSet.AsQueryable();
        }

        public IQueryable<T> QueryableAll(Expression<Func<T, bool>> criteria)
        {
            return _dbSet.Where(criteria);
        }

        public List<T> ListAll(Expression<Func<T, bool>> criteria)
        {
            return _dbSet.Where(criteria).ToList();
        }

        public List<T> ListAll(int page, int pageSize)
        {
            return _dbSet
                .OrderBy(x => x.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public List<T> ListAll(int offset, int limit, FilterByExpression filterByExpression, string sort, string order, out int count)
        {
            try
            {
                count = 0;
                if (filterByExpression == null && string.IsNullOrEmpty(sort))
                {
                    count = _dbSet.Count();
                    return _dbSet.OrderByDescending(x => x.Id).Skip(offset).Take(limit).ToList();
                }
                else if (filterByExpression == null && !string.IsNullOrEmpty(sort))
                {
                    var orderByExpression = $"{sort} {order}";
                    count = _dbSet.Count();
                    return _dbSet.OrderBy(orderByExpression).Skip(offset).Take(limit).ToList();
                }
                else if (filterByExpression != null && string.IsNullOrEmpty(sort))
                {
                    var records = _dbSet.Where(filterByExpression.Expression, filterByExpression.Parameters.ToArray());
                    count = records.Count();
                    return records.OrderByDescending(x => x.Id).Skip(offset).Take(limit).ToList();
                }
                else
                {
                    var orderByExpression = $"{sort} {order}";
                    var records = _dbSet.Where(filterByExpression.Expression, filterByExpression.Parameters.ToArray());
                    count = records.Count();
                    return records.OrderBy(orderByExpression).Skip(offset).Take(limit).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<T> ListAll(Expression<Func<T, bool>> criteria, int offset, int limit, FilterByExpression filterByExpression, string sort, string order, out int count)
        {
            try
            {
                count = 0;
                var filteredDbSet = _dbSet.Where(criteria);
                if (filterByExpression == null && string.IsNullOrEmpty(sort))
                {
                    count = filteredDbSet.Count();
                    return filteredDbSet.OrderByDescending(x => x.Id).Skip(offset).Take(limit).ToList();
                }
                else if (filterByExpression == null && !string.IsNullOrEmpty(sort))
                {
                    var orderByExpression = $"{sort} {order}";
                    count = filteredDbSet.Count();
                    return filteredDbSet.OrderBy(orderByExpression).Skip(offset).Take(limit).ToList();
                }
                else if (filterByExpression != null && string.IsNullOrEmpty(sort))
                {
                    var records = filteredDbSet.Where(filterByExpression.Expression, filterByExpression.Parameters.ToArray());
                    count = records.Count();
                    return records.OrderByDescending(x => x.Id).Skip(offset).Take(limit).ToList();
                }
                else
                {
                    var orderByExpression = $"{sort} {order}";
                    var records = filteredDbSet.Where(filterByExpression.Expression, filterByExpression.Parameters.ToArray());
                    count = records.Count();
                    return records.OrderBy(orderByExpression).Skip(offset).Take(limit).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<T>> ListAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<List<T>> ListAllAsync(int page, int pageSize)
        {
            return await _dbSet
                .OrderBy(x => x.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<List<T>> ListAllAsync(int offset, int limit, FilterByExpression filterByExpression, string sort, string order)
        {
            try
            {
                if (filterByExpression == null && string.IsNullOrEmpty(sort))
                {
                    return await _dbSet.OrderByDescending(x => x.Id).Skip(offset).Take(limit).ToListAsync();
                }
                else if (filterByExpression == null && !string.IsNullOrEmpty(sort))
                {
                    var orderByExpression = $"{sort} {order}";
                    return await _dbSet.OrderBy(orderByExpression).Skip(offset).Take(limit).ToListAsync();
                }
                else if (filterByExpression != null && string.IsNullOrEmpty(sort))
                {
                    return await _dbSet.Where(filterByExpression.Expression, filterByExpression.Parameters.ToArray()).OrderByDescending(x => x.Id).Skip(offset).Take(limit).ToListAsync();
                }
                else
                {
                    var orderByExpression = $"{sort} {order}";
                    return await _dbSet.Where(filterByExpression.Expression, filterByExpression.Parameters.ToArray()).OrderBy(orderByExpression).Skip(offset).Take(limit).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<T>> ListAllAsync(Expression<Func<T, bool>> criteria)
        {
            return await _dbSet.Where(criteria).ToListAsync();
        }

        public void RemoveFromDbSet(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void Update(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }

        public async Task UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<T>> FindAllAsync(Specification<T> specification)
        {
            var resultsWithIncludes = specification.Includes
                .Aggregate(_dbContext.Set<T>().AsQueryable(),
                    (current, include) => current.Include(include));

            return await resultsWithIncludes.Where(specification.ToExpression()).ToListAsync();
        }
    }
}
