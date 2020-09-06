using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Repositories
{
    /// <summary>
    /// Use this repository to call sql query, stored procedures and functions
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISqlQueryRepository<T> where T : class
    {
        Task<List<T>> RawSqlQueryAsync(string query);

        Task<List<CT>> RawSqlQueryAsync<CT>(string query) where CT : class;

        Task<List<CT>> RawSqlQueryAsync<CT>(string query, object param) where CT : class;

        Task<object> RawSqlQuerySingleColumnAsync(string query, object param);


        int RunSqlCommand(string query, params object[] parameters);

        Task<int> RunSqlCommandAsync(string query, params object[] parameters);
    }
}
