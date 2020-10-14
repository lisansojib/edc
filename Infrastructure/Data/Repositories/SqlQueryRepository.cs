using ApplicationCore.Interfaces.Repositories;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repositories
{
    public class SqlQueryRepository<T> : ISqlQueryRepository<T> where T: class
    {
        protected readonly AppDbContext _dbContext;

        public SqlQueryRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<T>> RawSqlQueryAsync(string query)
        {
            var connection = _dbContext.Database.GetDbConnection();

            try
            {
                var records = await connection.QueryAsync<T>(query);
                return records.ToList();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }

        public async Task<T> FirstOrDefaultAsync(string query)
        {
            var connection = _dbContext.Database.GetDbConnection();

            try
            {
                return await connection.QueryFirstOrDefaultAsync<T>(query);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }

        public async Task<List<CT>> RawSqlQueryAsync<CT>(string query) where CT : class
        {
            var connection = _dbContext.Database.GetDbConnection();

            try
            {
                var records = await connection.QueryAsync<CT>(query);
                return records.ToList();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }

        public async Task<List<CT>> RawSqlQueryAsync<CT>(string query, object param) where CT : class
        {
            var connection = _dbContext.Database.GetDbConnection();

            try
            {
                var records = await connection.QueryAsync<CT>(query, param);
                return records.ToList();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }

        public async Task<object> RawSqlQuerySingleColumnAsync(string query, object param)
        {
            var connection = _dbContext.Database.GetDbConnection();

            try
            {
                var record = await connection.ExecuteScalarAsync<object>(query, param);
                return record;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }

        public int RunSqlCommand(string query, params object[] parameters)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var result = _dbContext.Database.ExecuteSqlRaw(query, parameters);
                    _dbContext.SaveChanges();
                    transaction.Commit();
                    return result;
                }
                catch (System.Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public async Task<int> RunSqlCommandAsync(string query, params object[] parameters)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var result = await _dbContext.Database.ExecuteSqlRawAsync(query, parameters);
                    await _dbContext.SaveChangesAsync();
                    transaction.Commit();
                    return result;
                }
                catch (System.Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
