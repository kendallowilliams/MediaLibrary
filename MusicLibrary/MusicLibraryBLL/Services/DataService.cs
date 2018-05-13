using MusicLibraryBLL.Services.Interfaces;
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
using MusicLibraryBLL.Models;
using System.Linq.Expressions;

namespace MusicLibraryBLL.Services
{
    [Export(typeof(IDataService))]
    public class DataService : IDataService
    {
        private string connectionString;

        [ImportingConstructor]
        public DataService()
        {
            connectionString = ConfigurationManager.ConnectionStrings["MusicLibraryConnectionString"].ConnectionString;
        }

        public async Task<IEnumerable<T>> GetList<T>() where T: BaseModel
        {
            IEnumerable<T> results = Enumerable.Empty<T>();

            using (IDbConnection conn = GetNewConnection())
            {
                results = await conn.GetListAsync<T>();
            }

            return results;
        }

        public async Task<T> Get<T>(object id) where T : BaseModel
        {
            T result = default(T);

            using (IDbConnection conn = GetNewConnection())
            {
                result = await conn.GetAsync<T>(id);
            }

            return result;
        }

        public async Task Insert<T>(T entity) where T : BaseModel
        {
            using (IDbConnection conn = GetNewConnection())
            {
                entity.ModifyDate = DateTime.Now;
                entity.CreateDate = DateTime.Now;
                await conn.InsertAsync(entity);
            }
        }

        public async Task<R> Insert<T,R>(T entity) where T : BaseModel
        {
            R result = default(R);

            using (IDbConnection conn = GetNewConnection())
            {
                entity.ModifyDate = DateTime.Now;
                entity.CreateDate = DateTime.Now;
                result = await conn.InsertAsync(entity);
            }

            return result;
        }

        public async Task<bool> Delete<T>(object id) where T : BaseModel
        {
            bool result = false;

            using (IDbConnection conn = GetNewConnection())
            {
                var predicate = Predicates.Field<T>(f => f.Id, Operator.Eq, id);
                result = await conn.DeleteAsync(predicate);
            }

            return result;
        }

        public async Task<bool> Delete<T>(T entity) where T : BaseModel
        {
            bool result = false;

            using (IDbConnection conn = GetNewConnection())
            {
                result = await conn.DeleteAsync(entity);
            }

            return result;
        }

        public async Task<bool> Update<T>(T entity) where T : BaseModel
        {
            bool result = false;

            using (IDbConnection conn = GetNewConnection())
            {
                entity.ModifyDate = DateTime.Now;
                result = await conn.UpdateAsync(entity);
            }

            return result;
        }

        public async Task<int> Count<T>() where T : BaseModel
        {
            int result = default(int);

            using (IDbConnection conn = GetNewConnection())
            {
                result = await conn.CountAsync<T>();
            }

            return result;
        }

        public async Task<T> ExecuteScalar<T>(string sql, object parameters)
        {
            T result = default(T);

            using (IDbConnection conn = GetNewConnection())
            {
                result = await conn.ExecuteScalarAsync<T>(sql, parameters);
            }

            return result;
        }

        private IDbConnection GetNewConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}