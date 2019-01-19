using MediaLibraryBLL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel.Composition;
using DapperExtensions;
using System.Threading.Tasks;
using System.Configuration;
using MediaLibraryBLL.Models;
using System.Linq.Expressions;
using Fody;

namespace MediaLibraryBLL.Services
{
    [ConfigureAwait(false)]
    [Export(typeof(IDataService))]
    public class DataService : IDataService
    {
        private string connectionString;
        private int timeout;

        [ImportingConstructor]
        public DataService()
        {
            timeout = 120;
            connectionString = ConfigurationManager.ConnectionStrings["MediaLibraryConnectionString"].ConnectionString;
        }

        public async Task<IEnumerable<T>> GetList<T>(object predicate = null) where T: BaseModel
        {
            IEnumerable<T> results = Enumerable.Empty<T>();

            using (IDbConnection conn = GetNewConnection())
            {
                results = await conn.GetListAsync<T>(predicate, commandTimeout: timeout);
            }

            return results;
        }

        public async Task<T> Get<T>(object id) where T : BaseModel
        {
            T result = default(T);

            using (IDbConnection conn = GetNewConnection())
            {
                result = await conn.GetAsync<T>(id, commandTimeout: timeout);
            }

            return result;
        }

        public async Task Insert<T>(T entity) where T : BaseModel
        {
            using (IDbConnection conn = GetNewConnection())
            {
                entity.ModifyDate = DateTime.Now;
                entity.CreateDate = DateTime.Now;
                await conn.InsertAsync(entity, commandTimeout: timeout);
            }
        }

        public async Task<R> Insert<T,R>(T entity) where T : BaseModel
        {
            R result = default(R);

            using (IDbConnection conn = GetNewConnection())
            {
                entity.ModifyDate = DateTime.Now;
                entity.CreateDate = DateTime.Now;
                result = await conn.InsertAsync(entity, commandTimeout: timeout);
            }

            return result;
        }

        public async Task<bool> Delete<T>(object id) where T : BaseModel
        {
            bool result = false;

            using (IDbConnection conn = GetNewConnection())
            {
                var predicate = Predicates.Field<T>(f => f.Id, Operator.Eq, id);
                result = await conn.DeleteAsync<T>(predicate, commandTimeout: timeout);
            }

            return result;
        }

        public async Task<bool> Delete<T>(T entity) where T : BaseModel
        {
            bool result = false;

            using (IDbConnection conn = GetNewConnection())
            {
                result = await conn.DeleteAsync(entity, commandTimeout: timeout);
            }

            return result;
        }

        public async Task<bool> Update<T>(T entity) where T : BaseModel
        {
            bool result = false;

            using (IDbConnection conn = GetNewConnection())
            {
                entity.ModifyDate = DateTime.Now;
                result = await conn.UpdateAsync(entity, commandTimeout: timeout);
            }

            return result;
        }

        public async Task<int> Count<T>(object predicate = null) where T : BaseModel
        {
            int result = default(int);

            using (IDbConnection conn = GetNewConnection())
            {
                result = await conn.CountAsync<T>(predicate, commandTimeout: timeout);
            }

            return result;
        }

        public async Task<IEnumerable<T>> Query<T>(string sql, object parameters = null, CommandType commandType = CommandType.Text) where T : BaseModel
        {
            IEnumerable<T> result = Enumerable.Empty<T>();

            using (IDbConnection conn = GetNewConnection())
            {
                result = await conn.QueryAsync<T>(sql, parameters, commandTimeout: timeout, commandType: commandType);
            }

            return result;
        }

        public async Task<int> Execute(string sql, object parameters = null, CommandType commandType = CommandType.Text)
        {
            int result = default(int);

            using (IDbConnection conn = GetNewConnection())
            {
                result = await conn.ExecuteAsync(sql, parameters, commandTimeout: timeout, commandType: commandType);
            }

            return result;
        }

        public async Task<T> ExecuteScalar<T>(string sql, object parameters = null, CommandType commandType = CommandType.Text)
        {
            T result = default(T);

            using (IDbConnection conn = GetNewConnection())
            {
                result = await conn.ExecuteScalarAsync<T>(sql, parameters, commandTimeout: timeout, commandType: commandType);
            }

            return result;
        }

        private IDbConnection GetNewConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}