using MediaLibraryDAL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using System.Configuration;
using MediaLibraryDAL.Models;
using System.Linq.Expressions;
using Fody;
using MediaLibraryDAL.DbContexts;
using System.Data.Entity;

namespace MediaLibraryBLL.Services
{
    [ConfigureAwait(false)]
    [Export(typeof(IDataService))]
    public class DataService : IDataService
    {
        private int timeout;

        [ImportingConstructor]
        public DataService()
        {
            timeout = 120;
        }

        public IEnumerable<T> GetList<T>(Expression<Func<T, bool>> expression = null) where T : BaseModel
        {
            IEnumerable<T> results = Enumerable.Empty<T>();

            using (var db = new MediaLibraryContext())
            {
                results = (expression != null ? db.Set<T>().Where(expression) : db.Set<T>()).ToList();
            }

            return results;
        }

        public T Get<T>(Expression<Func<T,bool>> expression = null) where T: BaseModel
        {
            T result = default(T);

            using (var db = new MediaLibraryContext())
            {
                result = (expression != null ? db.Set<T>().Where(expression) : db.Set<T>()).FirstOrDefault();
            }

            return result;
        }

        public async Task<int> Insert<T>(T entity) where T : BaseModel
        {
            int result = default(int);

            using (var db = new MediaLibraryContext())
            {
                entity.ModifyDate = DateTime.Now;
                entity.CreateDate = DateTime.Now;
                db.Set<T>().Add(entity);
                result = await db.SaveChangesAsync();
            }

            return result;
        }

        public async Task<int> Insert<T>(IEnumerable<T> entities) where T : BaseModel
        {
            int result = default(int);

            using (var db = new MediaLibraryContext())
            {
                db.Set<T>().AddRange(entities);
                result = await db.SaveChangesAsync();
            }

            return result;
        }

        public async Task<int> Delete<T>(object id) where T : BaseModel
        {
            int result = default(int);

            using (var db = new MediaLibraryContext())
            {
                DbSet<T> set = db.Set<T>();
                T entity = await set.FindAsync(id);

                set.Remove(entity);
                result = await db.SaveChangesAsync();
            }

            return result;
        }

        public async Task<int> Delete<T>(T entity) where T : BaseModel
        {
            int result = default(int);

            using (var db = new MediaLibraryContext())
            {
                db.Set<T>().Remove(entity);
                result = await db.SaveChangesAsync();
            }

            return result;
        }

        public async Task<int> DeleteAll<T>() where T : BaseModel
        {
            int result = default(int);

            using (var db = new MediaLibraryContext())
            {
                DbSet<T> set = db.Set<T>();
                set.RemoveRange(set);
                result = await db.SaveChangesAsync();
            }

            return result;
        }

        public async Task<int> Update<T>(T entity) where T : BaseModel
        {
            int result = default(int);

            using (var db = new MediaLibraryContext())
            {
                entity.ModifyDate = DateTime.Now;
                db.Set<T>().Add(entity);
                db.Entry(entity).State = EntityState.Modified;
                result = await db.SaveChangesAsync();
            }

            return result;
        }

        public async Task<int> Count<T>(Expression<Func<T,bool>> expression = null) where T : BaseModel
        {
            int result = default(int);

            using (var db = new MediaLibraryContext())
            {
                result = expression != null ? await db.Set<T>().CountAsync(expression) : await db.Set<T>().CountAsync();
            }

            return result;
        }

        public async Task<IEnumerable<T>> Query<T>(string sql, object parameters = null) where T : BaseModel
        {
            IEnumerable<T> result = Enumerable.Empty<T>();

            using (var db = new MediaLibraryContext())
            {
                db.Database.CommandTimeout = timeout;
                result = await db.Database.SqlQuery<T>(sql, parameters).ToListAsync();
            }

            return result;
        }

        public async Task<int> Execute(string sql, object parameters = null)
        {
            int result = default(int);

            using (var db = new MediaLibraryContext())
            {
                db.Database.CommandTimeout = timeout;
                result = await db.Database.ExecuteSqlCommandAsync(sql, parameters);
            }

            return result;
        }

        public async Task<T> ExecuteScalar<T>(string sql, object parameters = null)
        {
            T result = default(T);

            using (var db = new MediaLibraryContext())
            {
                result = await db.Database.SqlQuery<T>(sql, parameters).SingleOrDefaultAsync();
            }

            return result;
        }
    }
}