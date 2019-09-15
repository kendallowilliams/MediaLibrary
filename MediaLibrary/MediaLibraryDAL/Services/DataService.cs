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
using Microsoft.EntityFrameworkCore;

namespace MediaLibraryDAL.Services
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

        public async Task<IEnumerable<T>> GetList<T>(Expression<Func<T, bool>> expression = null) where T : BaseModel
        {
            IEnumerable<T> results = Enumerable.Empty<T>();

            using (var db = new MediaLibraryContext())
            {
                results = await (expression != null ? db.Set<T>().Where(expression) : db.Set<T>()).ToListAsync<T>();
            }

            return results;
        }

        public async Task<T> Get<T>(Expression<Func<T,bool>> expression = null) where T: BaseModel
        {
            T result = default(T);

            using (var db = new MediaLibraryContext())
            {
                result = await (expression != null ? db.Set<T>().Where(expression) : db.Set<T>()).FirstOrDefaultAsync();
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

        public async Task<int> DeleteAll<T>(Expression<Func<T, bool>> expression = null) where T : BaseModel
        {
            int result = default(int);

            if (expression != null)
            {
                using (var db = new MediaLibraryContext())
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
    }
}