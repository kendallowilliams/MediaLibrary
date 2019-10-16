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
        T Get<T>(Expression<Func<T, bool>> expression = null) where T : class, IDataModel;

        T Get<T, TInclude>(Expression<Func<T,bool>> expression = null, 
                           Expression<Func<T, TInclude>> includeExpression = null) where T: class, IDataModel;

        T Get<T, TInclude1, TInclude2>(Expression<Func<T, bool>> expression = null, 
                                       Expression<Func<T, TInclude1>> includeExpression1 = null, 
                                       Expression<Func<T, TInclude2>> includeExpression2 = null) where T : class, IDataModel;

        Task<T> GetAsync<T>(Expression<Func<T, bool>> expression = null) where T : class, IDataModel;

        Task<T> GetAsync<T, TInclude>(Expression<Func<T, bool>> expression = null, 
                                      Expression<Func<T, TInclude>> includeExpression = null) where T : class, IDataModel;

        Task<T> GetAsync<T, TInclude1, TInclude2>(Expression<Func<T, bool>> expression = null,
                                                  Expression<Func<T, TInclude1>> includeExpression1 = null,
                                                  Expression<Func<T, TInclude2>> includeExpression2 = null) where T : class, IDataModel;

        Task<IEnumerable<T>> GetList<T>(Expression<Func<T, bool>> expression = null) where T : class, IDataModel;

        Task<IEnumerable<T>> GetList<T, TInclude>(Expression<Func<T, bool>> expression = null, 
                                                  Expression<Func<T, TInclude>> includeExpression = null) where T : class, IDataModel;

        Task<IEnumerable<T>> GetList<T, TInclude1, TInclude2>(Expression<Func<T, bool>> expression = null,
                                                              Expression<Func<T, TInclude1>> includeExpression1 = null,
                                                              Expression<Func<T, TInclude2>> includeExpression2 = null) where T : class, IDataModel;

        Task<int> Insert<T>(T entity) where T : class, IDataModel;

        Task<int> Insert<T>(IEnumerable<T> entity) where T : class, IDataModel;

        Task<int> Delete<T>(object id) where T : class, IDataModel;

        Task<int> Delete<T>(T entity) where T : class, IDataModel;

        Task<int> DeleteAll<T>(Expression<Func<T, bool>> expression = null) where T : class, IDataModel;

        Task<int> DeleteAll<T>() where T : class, IDataModel;

        Task<int> Update<T>(T entity) where T : class, IDataModel;

        Task<IEnumerable<T>> Query<T>(string sql, object parameters) where T : class, IDataModel;

        Task<T> ExecuteScalar<T>(string sql, object parameters);

        Task<int> Execute(string sql, object parameters = null);

        Task<int> Count<T>(Expression<Func<T,bool>> expression = null) where T : class, IDataModel;
    }
}
