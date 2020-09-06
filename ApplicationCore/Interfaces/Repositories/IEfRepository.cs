using ApplicationCore.DTOs;
using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Repositories
{
    /// <summary>
    /// There are some common utility methods.
    /// Feel free to modify and add any new methods as you need in your project.
    /// </summary>
    /// <typeparam name="T">Entity Class</typeparam>
    public interface IEfRepository<T> where T : BaseEntity
    {
        #region Sync Methods
        T Find(int id);
        T Find(Expression<Func<T, bool>> criteria);
        bool Exists(int id);
        bool Exists(Expression<Func<T, bool>> criteria);
        int Count();
        int GetMaxId();
        List<T> ListAll();
        IQueryable<T> QueryableAll();
        IQueryable<T> QueryableAll(Expression<Func<T, bool>> criteria);
        List<T> ListAll(Expression<Func<T, bool>> criteria);
        List<T> ListAll(int page, int pageSize);
        List<T> ListAll(int offset, int limit, FilterByExpression FilterByExpression, string sort, string order, out int count);
        List<T> ListAll(Expression<Func<T, bool>> criteria, int offset, int limit, FilterByExpression FilterByExpression, string sort, string order, out int count);
        T Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        #endregion

        #region Async Methods
        Task<T> FindAsync(int id);
        Task<T> FindAsyncWithInclude(int id, Expression<Func<T, object>> include);
        Task<T> FindAsync(Expression<Func<T, bool>> criteria);
        Task<List<T>> FindAllAsync(Expression<Func<T, bool>> criteria, Expression<Func<T, object>> include);
        Task<bool> ExistsAsync(int id);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> criteria);
        Task<int> CountAsync();
        Task<int> CountAsync(Expression<Func<T, bool>> criteria);
        Task<List<T>> ListAllAsync();
        Task<List<T>> ListAllAsync(Expression<Func<T, bool>> criteria);
        Task<List<T>> ListAllAsync(int page, int pageSize);
        Task<List<T>> ListAllAsync(int offset, int limit, FilterByExpression FilterByExpression, string sort, string order);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task DeleteManyAsync(IEnumerable<T> entities);
        void RemoveFromDbSet(T entity);
        #endregion
    }
}
