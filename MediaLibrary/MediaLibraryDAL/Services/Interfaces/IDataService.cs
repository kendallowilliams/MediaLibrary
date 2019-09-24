using MediaLibraryDAL.Models;
using MediaLibraryDAL.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Linq.Expressions;

namespace MediaLibraryDAL.Services.Interfaces
{
    public interface IDataService
    {
        T Get<T>(Expression<Func<T, bool>> expression = null) where T : BaseModel;

        T Get<T, TInclude>(Expression<Func<T,bool>> expression = null, Expression<Func<T, TInclude>> includeExpression = null) where T: BaseModel;

        Task<T> GetAsync<T>(Expression<Func<T, bool>> expression = null) where T : BaseModel;

        Task<T> GetAsync<T, TInclude>(Expression<Func<T, bool>> expression = null, Expression<Func<T, TInclude>> includeExpression = null) where T : BaseModel;

        Task<IEnumerable<T>> GetList<T>(Expression<Func<T, bool>> expression = null) where T : BaseModel;

        Task<IEnumerable<T>> GetList<T, TInclude>(Expression<Func<T, bool>> expression = null, Expression<Func<T, TInclude>> includeExpression = null) where T : BaseModel;

        Task<int> Insert<T>(T entity) where T : BaseModel;

        Task<int> Insert<T>(IEnumerable<T> entity) where T : BaseModel;

        Task<int> Delete<T>(object id) where T : BaseModel;

        Task<int> Delete<T>(T entity) where T : BaseModel;

        Task<int> DeleteAll<T>(Expression<Func<T, bool>> expression = null) where T : BaseModel;

        Task<int> DeleteAll<T>() where T : BaseModel;

        Task<int> Update<T>(T entity) where T : BaseModel;

        Task<IEnumerable<T>> Query<T>(string sql, object parameters) where T : BaseModel;

        Task<T> ExecuteScalar<T>(string sql, object parameters);

        Task<int> Execute(string sql, object parameters = null);

        Task<int> Count<T>(Expression<Func<T,bool>> expression = null) where T : BaseModel;
    }
}
