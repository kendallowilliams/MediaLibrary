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
using MediaLibraryDAL.Models.Interfaces;

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

        public async Task<IEnumerable<T>> GetList<T>(Expression<Func<T, bool>> expression = null) where T : class, IDataModel
        {
            IEnumerable<T> results = Enumerable.Empty<T>();

            using (var db = new MediaLibraryEntities())
            {
                results = await (expression != null ? db.Set<T>().Where(expression) : db.Set<T>()).ToListAsync();
            }

            return results;
        }

        public Task<T> Get<T>(Expression<Func<T,bool>> expression = null) where T: class, IDataModel
        {
            T result = default(T);
            TaskCompletionSource<T> tcs = new TaskCompletionSource<T>();

            using (var db = new MediaLibraryEntities())
            {
                result = (expression != null ? db.Set<T>().Where(expression) : db.Set<T>()).FirstOrDefault();
                tcs.SetResult(result);
            }

            return tcs.Task;
        }

        public async Task<T> GetAsync<T>(Expression<Func<T, bool>> expression = null) where T : class, IDataModel
        {
            T result = default(T);

            using (var db = new MediaLibraryEntities())
            {
                result = await (expression != null ? db.Set<T>().Where(expression) : db.Set<T>()).FirstOrDefaultAsync();
            }

            return result;
        }

        public async Task<int> Insert<T>(T entity) where T : class, IDataModel
        {
            int result = default(int);

            using (var db = new MediaLibraryEntities())
            {
                entity.ModifyDate = DateTime.Now;
                entity.CreateDate = DateTime.Now;
                db.Set<T>().Add(entity);
                result = await db.SaveChangesAsync();
            }

            return result;
        }

        public async Task<int> Insert<T>(IEnumerable<T> entities) where T : class, IDataModel
        {
            int result = default(int);

            using (var db = new MediaLibraryEntities())
            {
                db.Set<T>().AddRange(entities);
                result = await db.SaveChangesAsync();
            }

            return result;
        }

        public async Task<int> Delete<T>(object id) where T : class, IDataModel
        {
            int result = default(int);

            using (var db = new MediaLibraryEntities())
            {
                DbSet<T> set = db.Set<T>();
                T entity = await set.FindAsync(id);

                set.Remove(entity);
                result = await db.SaveChangesAsync();
            }

            return result;
        }

        public async Task<int> Delete<T>(T entity) where T : class, IDataModel
        {
            int result = default(int);

            using (var db = new MediaLibraryEntities())
            {
                db.Set<T>().Remove(entity);
                result = await db.SaveChangesAsync();
            }

            return result;
        }

        public async Task<int> DeleteAll<T>(Expression<Func<T, bool>> expression = null) where T : class, IDataModel
        {
            int result = default(int);

            if (expression != null)
            {
                using (var db = new MediaLibraryEntities())
                {
                    DbSet<T> set = db.Set<T>();
                    set.RemoveRange(set.Where(expression));
                    result = await db.SaveChangesAsync();
                }
            }
            else
            {
                result = await DeleteAll<T>();
            }

            return result;
        }

        public async Task<int> DeleteAll<T>() where T : class, IDataModel
        {
            int result = default(int);

            using (var db = new MediaLibraryEntities())
            {
                DbSet<T> set = db.Set<T>();
                set.RemoveRange(set);
                result = await db.SaveChangesAsync();
            }

            return result;
        }

        public async Task<int> Update<T>(T entity) where T : class, IDataModel
        {
            int result = default(int);

            using (var db = new MediaLibraryEntities())
            {
                entity.ModifyDate = DateTime.Now;
                db.Set<T>().Add(entity);
                db.Entry(entity).State = EntityState.Modified;
                result = await db.SaveChangesAsync();
            }

            return result;
        }

        public async Task<int> Count<T>(Expression<Func<T,bool>> expression = null) where T : class, IDataModel
        {
            int result = default(int);

            using (var db = new MediaLibraryEntities())
            {
                result = expression != null ? await db.Set<T>().CountAsync(expression) : await db.Set<T>().CountAsync();
            }

            return result;
        }

        public async Task<IEnumerable<T>> Query<T>(string sql, params object[] parameters)
        {
            IEnumerable<T> result = Enumerable.Empty<T>();

            using (var db = new MediaLibraryEntities())
            {
                db.Database.CommandTimeout = timeout;
                result = await db.Database.SqlQuery<T>(sql, parameters).ToListAsync();
            }

            return result;
        }

        public async Task<int> Execute(string sql, object parameters = null)
        {
            int result = default(int);

            using (var db = new MediaLibraryEntities())
            {
                db.Database.CommandTimeout = timeout;
                result = await db.Database.ExecuteSqlCommandAsync(sql, parameters);
            }

            return result;
        }

        public async Task<T> ExecuteScalar<T>(string sql, object parameters = null)
        {
            T result = default(T);

            using (var db = new MediaLibraryEntities())
            {
                result = await db.Database.SqlQuery<T>(sql, parameters).SingleOrDefaultAsync();
            }

            return result;
        }

        public Task<T> Get<T, TInclude>(Expression<Func<T, bool>> expression = null, Expression<Func<T, TInclude>> includeExpression = null) where T : class, IDataModel
        {
            T result = default(T);
            TaskCompletionSource<T> tcs = new TaskCompletionSource<T>();

            using (var db = new MediaLibraryEntities())
            {
                var query = includeExpression != null ? db.Set<T>().Include(includeExpression) : db.Set<T>();
                result = (expression != null ? query.Where(expression) : query).FirstOrDefault();
                tcs.SetResult(result);
            }

            return tcs.Task;
        }

        public async Task<T> GetAsync<T, TInclude>(Expression<Func<T, bool>> expression = null, Expression<Func<T, TInclude>> includeExpression = null) where T : class, IDataModel
        {
            T result = default(T);

            using (var db = new MediaLibraryEntities())
            {
                var query = includeExpression != null ? db.Set<T>().Include(includeExpression) : db.Set<T>();
                result = await (expression != null ? query.Where(expression) : query).FirstOrDefaultAsync();
            }

            return result;
        }

        public async Task<IEnumerable<T>> GetList<T,TInclude>(Expression<Func<T, bool>> expression = null, Expression<Func<T, TInclude>> includeExpression = null) where T : class, IDataModel
        {
            IEnumerable<T> results = Enumerable.Empty<T>();

            using (var db = new MediaLibraryEntities())
            {
                var query = includeExpression != null ? db.Set<T>().Include(includeExpression) : db.Set<T>();
                results = await (expression != null ? query.Where(expression) : query).ToListAsync();
            }

            return results;
        }

        public Task<T> Get<T, TInclude1, TInclude2>(Expression<Func<T, bool>> expression = null, 
                                              Expression<Func<T, TInclude1>> includeExpression1 = null, 
                                              Expression<Func<T, TInclude2>> includeExpression2 = null) where T : class, IDataModel
        {
            T result = default(T);
            TaskCompletionSource<T> tcs = new TaskCompletionSource<T>();

            using (var db = new MediaLibraryEntities())
            {
                var query = includeExpression1 != null ? db.Set<T>().Include(includeExpression1) : db.Set<T>();
                query = includeExpression2 != null ? query.Include(includeExpression2) : query;
                result = (expression != null ? query.Where(expression) : query).FirstOrDefault();
                tcs.SetResult(result);
            }

            return tcs.Task;
        }

        public async Task<T> GetAsync<T, TInclude1, TInclude2>(Expression<Func<T, bool>> expression = null, 
                                                               Expression<Func<T, TInclude1>> includeExpression1 = null, 
                                                               Expression<Func<T, TInclude2>> includeExpression2 = null) where T : class, IDataModel
        {
            T result = default(T);

            using (var db = new MediaLibraryEntities())
            {
                var query = includeExpression1 != null ? db.Set<T>().Include(includeExpression1) : db.Set<T>();
                query = includeExpression2 != null ? query.Include(includeExpression2) : query;
                result = await (expression != null ? query.Where(expression) : query).FirstOrDefaultAsync();
            }

            return result;
        }

        public async Task<IEnumerable<T>> GetList<T, TInclude1, TInclude2>(Expression<Func<T, bool>> expression = null, 
                                                                           Expression<Func<T, TInclude1>> includeExpression1 = null, 
                                                                           Expression<Func<T, TInclude2>> includeExpression2 = null) where T : class, IDataModel
        {
            IEnumerable<T> results = Enumerable.Empty<T>();

            using (var db = new MediaLibraryEntities())
            {
                var query = includeExpression1 != null ? db.Set<T>().Include(includeExpression1) : db.Set<T>();
                query = includeExpression2 != null ? query.Include(includeExpression2) : query;
                results = await (expression != null ? query.Where(expression) : query).ToListAsync();
            }

            return results;
        }

        public Task<T> Get<T, TInclude1, TInclude2, TInclude3>(Expression<Func<T, bool>> expression = null,
                                                         Expression<Func<T, TInclude1>> includeExpression1 = null,
                                                         Expression<Func<T, TInclude2>> includeExpression2 = null,
                                                         Expression<Func<T, TInclude3>> includeExpression3 = null) where T : class, IDataModel
        {
            T result = default(T);
            TaskCompletionSource<T> tcs = new TaskCompletionSource<T>();

            using (var db = new MediaLibraryEntities())
            {
                var query = includeExpression1 != null ? db.Set<T>().Include(includeExpression1) : db.Set<T>();
                query = includeExpression2 != null ? query.Include(includeExpression2) : query;
                query = includeExpression3 != null ? query.Include(includeExpression3) : query;
                result = (expression != null ? query.Where(expression) : query).FirstOrDefault();
                tcs.SetResult(result);
            }

            return tcs.Task;
        }

        public async Task<T> GetAsync<T, TInclude1, TInclude2, TInclude3>(Expression<Func<T, bool>> expression = null,
                                                                          Expression<Func<T, TInclude1>> includeExpression1 = null,
                                                                          Expression<Func<T, TInclude2>> includeExpression2 = null,
                                                                          Expression<Func<T, TInclude3>> includeExpression3 = null) where T : class, IDataModel
        {
            T result = default(T);

            using (var db = new MediaLibraryEntities())
            {
                var query = includeExpression1 != null ? db.Set<T>().Include(includeExpression1) : db.Set<T>();
                query = includeExpression2 != null ? query.Include(includeExpression2) : query;
                query = includeExpression3 != null ? query.Include(includeExpression3) : query;
                result = await (expression != null ? query.Where(expression) : query).FirstOrDefaultAsync();
            }

            return result;
        }

        public async Task<IEnumerable<T>> GetList<T, TInclude1, TInclude2, TInclude3>(Expression<Func<T, bool>> expression = null,
                                                                                      Expression<Func<T, TInclude1>> includeExpression1 = null,
                                                                                      Expression<Func<T, TInclude2>> includeExpression2 = null,
                                                                                      Expression<Func<T, TInclude3>> includeExpression3 = null) where T : class, IDataModel
        {
            IEnumerable<T> results = Enumerable.Empty<T>();

            using (var db = new MediaLibraryEntities())
            {
                var query = includeExpression1 != null ? db.Set<T>().Include(includeExpression1) : db.Set<T>();
                query = includeExpression2 != null ? query.Include(includeExpression2) : query;
                query = includeExpression3 != null ? query.Include(includeExpression3) : query;
                results = await (expression != null ? query.Where(expression) : query).ToListAsync();
            }

            return results;
        }

        public SqlParameter CreateParameter(string name, object value) => new SqlParameter(name, value);
    }
}