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
using System.Threading;
using System.Data.Common;
using System.Reflection;

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

        public async Task<IEnumerable<T>> GetList<T>(Expression<Func<T, bool>> expression = null, 
                                                     CancellationToken token = default(CancellationToken),
                                                     params Expression<Func<T, object>>[] includes) where T : class, IDataModel
        {
            IEnumerable<T> results = Enumerable.Empty<T>();

            using (var db = new MediaLibraryEntities())
            {
                IQueryable<T> query = db.Set<T>();

                db.Database.CommandTimeout = timeout;
                db.Configuration.ProxyCreationEnabled = false;

                foreach (var include in includes)
                {
                    query = query.Include(include);
                }

                results = await (expression != null ? query.Where(expression) : query).ToListAsync(token);
            }

            return results;
        }

        public async Task<T> Get<T>(Expression<Func<T, bool>> expression = null, 
                                    CancellationToken token = default(CancellationToken),
                                    params Expression<Func<T, object>>[] includes) where T : class, IDataModel
        {
            T result = default(T);

            using (var db = new MediaLibraryEntities())
            {
                IQueryable<T> query = db.Set<T>();

                db.Database.CommandTimeout = timeout;
                db.Configuration.ProxyCreationEnabled = false;

                foreach (var include in includes)
                {
                    query = query.Include(include);
                }

                result = await (expression != null ? query.Where(expression) : query).FirstOrDefaultAsync(token);
            }

            return result;
        }

        public async Task<int> Insert<T>(T entity, CancellationToken token = default(CancellationToken)) where T : class, IDataModel
        {
            int result = default(int);

            using (var db = new MediaLibraryEntities())
            {
                db.Database.CommandTimeout = timeout;
                entity.ModifyDate = DateTime.Now;
                entity.CreateDate = DateTime.Now;
                db.Set<T>().Add(entity);
                result = await db.SaveChangesAsync(token);
            }

            return result;
        }

        public async Task<int> Insert<T>(IEnumerable<T> entities, CancellationToken token = default(CancellationToken)) where T : class, IDataModel
        {
            int result = default(int);
            IList<T> items = entities.ToList();

            using (var db = new MediaLibraryEntities())
            {
                foreach (var item in items)
                {
                    item.CreateDate = DateTime.Now;
                    item.ModifyDate = DateTime.Now;
                }

                db.Database.CommandTimeout = timeout;
                db.Set<T>().AddRange(items);
                result = await db.SaveChangesAsync(token);
            }

            return result;
        }

        public async Task<int> Delete<T>(object id, CancellationToken token = default(CancellationToken)) where T : class, IDataModel
        {
            int result = default(int);

            using (var db = new MediaLibraryEntities())
            {
                DbSet<T> set = null;
                T entity = null;

                db.Database.CommandTimeout = timeout;
                set = db.Set<T>();
                entity = await set.FindAsync(id);
                set.Remove(entity);
                result = await db.SaveChangesAsync(token);
            }

            return result;
        }

        public async Task<int> Delete<T>(T entity, CancellationToken token = default(CancellationToken)) where T : class, IDataModel
        {
            return await Delete<T>(entity?.Id, token);
        }

        public async Task<int> DeleteAll<T>(Expression<Func<T, bool>> expression = null, CancellationToken token = default(CancellationToken)) where T : class, IDataModel
        {
            int result = default(int);

            if (expression != null)
            {
                using (var db = new MediaLibraryEntities())
                {
                    DbSet<T> set = null;

                    db.Database.CommandTimeout = timeout;
                    set = db.Set<T>();
                    set.RemoveRange(set.Where(expression));
                    result = await db.SaveChangesAsync(token);
                }
            }
            else
            {
                result = await DeleteAll<T>(token: token);
            }

            return result;
        }

        public async Task<int> Update<T>(T entity, CancellationToken token = default(CancellationToken)) where T : class, IDataModel
        {
            int result = default(int);

            using (var db = new MediaLibraryEntities())
            {
                db.Database.CommandTimeout = timeout;
                entity.ModifyDate = DateTime.Now;
                db.Entry(entity).State = EntityState.Modified;
                result = await db.SaveChangesAsync(token);
            }

            return result;
        }

        public async Task<int> Count<T>(Expression<Func<T,bool>> expression = null, CancellationToken token = default(CancellationToken)) where T : class, IDataModel
        {
            int result = default(int);

            using (var db = new MediaLibraryEntities())
            {
                db.Database.CommandTimeout = timeout;
                result = expression != null ? await db.Set<T>().CountAsync(expression, token) : await db.Set<T>().CountAsync(token);
            }

            return result;
        }
        
        public async Task<bool> Exists<T>(Expression<Func<T, bool>> expression = null, CancellationToken token = default(CancellationToken)) where T : class, IDataModel
        {
            bool result = default(bool);

            using (var db = new MediaLibraryEntities())
            {
                db.Database.CommandTimeout = timeout;
                result = (expression != null ? await db.Set<T>().FirstOrDefaultAsync(expression, token) : await db.Set<T>().FirstOrDefaultAsync(token)) != null;
            }

            return result;
        }

        public async Task<IEnumerable<T>> Query<T>(string sql, CancellationToken token = default(CancellationToken), params object[] parameters)
        {
            IEnumerable<T> result = Enumerable.Empty<T>();

            using (var db = new MediaLibraryEntities())
            {
                db.Database.CommandTimeout = timeout;
                result = await db.Database.SqlQuery<T>(sql, parameters).ToListAsync(token);
            }

            return result;
        }

        public async Task<int> Execute(string sql, CancellationToken token = default(CancellationToken), params object[] parameters)
        {
            int result = default(int);

            using (var db = new MediaLibraryEntities())
            {
                db.Database.CommandTimeout = timeout;
                result = await db.Database.ExecuteSqlCommandAsync(sql, token, parameters);
            }

            return result;
        }

        public async Task<T> ExecuteScalar<T>(string sql, CancellationToken token = default(CancellationToken), params object[] parameters)
        {
            T result = default(T);

            using (var db = new MediaLibraryEntities())
            {
                db.Database.CommandTimeout = timeout;
                result = await db.Database.SqlQuery<T>(sql, token, parameters).SingleOrDefaultAsync(token);
            }

            return result;
        }

        public SqlParameter CreateParameter(string name, object value) => new SqlParameter(name, value);

        public async Task<IEnumerable<T>> ExecuteStoredProcedure<T>(string sql, 
                                                                    CancellationToken token = default(CancellationToken), 
                                                                    params object[] parameters) where T : class, new()
        {
            List<T> entities = new List<T>();

            using (var db = new MediaLibraryEntities())
            {
                using (DbCommand command = db.Database.Connection.CreateCommand())
                {
                    DbDataReader reader = null;
                    PropertyInfo[] properties = typeof(T).GetProperties();
                    IDictionary<int, string> columns = new Dictionary<int, string>();

                    await command.Connection.OpenAsync();
                    command.CommandTimeout = timeout;
                    command.CommandText = sql;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(parameters);
                    reader = await command.ExecuteReaderAsync(token);
                    columns = Enumerable.Range(0, reader.FieldCount)
                                        .Select(ordinal => new { Key = ordinal, Value = reader.GetName(ordinal) })
                                        .ToDictionary(column => column.Key, column => column.Value);

                    while (await reader.ReadAsync())
                    {
                        T entity = new T();

                        foreach (var column in columns)
                        {
                            object value = await reader.IsDBNullAsync(column.Key, token) ? null : reader[column.Value];

                            properties.FirstOrDefault(property => property.Name == column.Value)?.SetValue(entity, value);
                        }

                        entities.Add(entity);
                    }
                }
            }

            return entities;
        }
    }
}