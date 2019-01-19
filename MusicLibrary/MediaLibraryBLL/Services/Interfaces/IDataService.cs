using MediaLibraryBLL.Models;
using MediaLibraryBLL.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace MediaLibraryBLL.Services.Interfaces
{
    public interface IDataService
    {
        Task<T> Get<T>(object id) where T: BaseModel;

        Task<IEnumerable<T>> GetList<T>(object predicate = null) where T : BaseModel;
        
        Task Insert<T>(T entity) where T : BaseModel;

        Task<R> Insert<T,R>(T entity) where T : BaseModel;

        Task<bool> Delete<T>(object id) where T : BaseModel;

        Task<bool> Delete<T>(T entity) where T : BaseModel;

        Task<bool> Update<T>(T entity) where T : BaseModel;

        Task<IEnumerable<T>> Query<T>(string sql, object parameters, CommandType commandType = CommandType.Text) where T : BaseModel;

        Task<T> ExecuteScalar<T>(string sql, object parameters, CommandType commandType = CommandType.Text);

        Task<int> Execute(string sql, object parameters = null, CommandType commandType = CommandType.Text);

        Task<int> Count<T>(object predicate = null) where T : BaseModel;
    }
}
