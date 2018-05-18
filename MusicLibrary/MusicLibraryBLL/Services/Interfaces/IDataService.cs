using MusicLibraryBLL.Models;
using MusicLibraryBLL.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicLibraryBLL.Services.Interfaces
{
    public interface IDataService
    {
        Task<T> Get<T>(object id) where T: BaseModel;

        Task<IEnumerable<T>> GetList<T>() where T : BaseModel;
        
        Task Insert<T>(T entity) where T : BaseModel;

        Task<R> Insert<T,R>(T entity) where T : BaseModel;

        Task<bool> Delete<T>(object id) where T : BaseModel;

        Task<bool> Delete<T>(T entity) where T : BaseModel;

        Task<bool> Update<T>(T entity) where T : BaseModel;

        Task<IEnumerable<T>> Query<T>(string sql, object parameters) where T : BaseModel;

        Task<T> ExecuteScalar<T>(string sql, object parameters);

        Task<int> Execute(string sql, object parameters);

        Task<int> Count<T>() where T : BaseModel;
    }
}
