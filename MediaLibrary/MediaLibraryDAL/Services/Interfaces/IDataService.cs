using MediaLibraryDAL.Models;
using MediaLibraryDAL.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Linq.Expressions;
using System.Threading;
using Microsoft.Data.SqlClient;

namespace MediaLibraryDAL.Services.Interfaces
{
    public interface IDataService
    {
        Task<T> Get<T>(Expression<Func<T, bool>> expression = null, CancellationToken token = default(CancellationToken), params Expression<Func<T, object>>[] includes) where T : class, IDataModel;

        Task<IEnumerable<T>> GetList<T>(Expression<Func<T, bool>> expression = null, CancellationToken token = default(CancellationToken), params Expression<Func<T, object>>[] includes) where T : class, IDataModel;

        Task<int> Insert<T>(T entity, CancellationToken token = default(CancellationToken)) where T : class, IDataModel;

        Task<int> Insert<T>(IEnumerable<T> entity, CancellationToken token = default(CancellationToken)) where T : class, IDataModel;

        Task<int> Delete<T>(object id, CancellationToken token = default(CancellationToken)) where T : class, IDataModel;

        Task<int> Delete<T>(T entity, CancellationToken token = default(CancellationToken)) where T : class, IDataModel;

        Task<int> DeleteAll<T>(Expression<Func<T, bool>> expression = null, CancellationToken token = default(CancellationToken)) where T : class, IDataModel;

        Task<int> Update<T>(T entity, CancellationToken token = default(CancellationToken)) where T : class, IDataModel;

        Task<int> Count<T>(Expression<Func<T,bool>> expression = null, CancellationToken token = default(CancellationToken)) where T : class, IDataModel;

        Task<bool> Exists<T>(Expression<Func<T, bool>> expression = null, CancellationToken token = default(CancellationToken)) where T : class, IDataModel;

        SqlParameter CreateParameter(string name, object value);
    }
}
