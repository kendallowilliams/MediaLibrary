using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Fody;
using MediaLibraryDAL.Services.Interfaces;
using System.Linq.Expressions;
using MediaLibraryBLL.Services.Interfaces;
using MediaLibraryDAL.DbContexts;

namespace MediaLibraryBLL.Services
{
    [ConfigureAwait(false)]
    [Export(typeof(IAlbumService))]
    public class AlbumService : IAlbumService
    {
        private readonly IDataService dataService;

        [ImportingConstructor]
        public AlbumService(IDataService dataService)
        {
            this.dataService = dataService;
        }

        public async Task<int?> AddAlbum(Album album)
        {
            int? id = default(int?);

            if (album != null)
            {
                Album dbAlbum = await dataService.GetAsync<Album>(item => item.Title == album.Title);

                if (dbAlbum != null) { id = dbAlbum.Id; }
                else
                {
                    await dataService.Insert(album);
                    id = album.Id;
                }
            }

            return id;
        }

        public async Task<IEnumerable<Album>> GetAlbums(Expression<Func<Album, bool>> expression = null) => await dataService.GetList(expression);

        public async Task<Album> GetAlbum(Expression<Func<Album, bool>> expression = null) =>  await dataService.GetAsync(expression);

        public async Task<int> InsertAlbum(Album album) => await dataService.Insert<Album>(album);

        public async Task<int> DeleteAlbum(int id) => await dataService.Delete<Album>(id);

        public async Task<int> DeleteAlbum(Album album) => await dataService.Delete(album);

        public async Task DeleteAllAlbums() => await dataService.DeleteAll<Album>();

        public async Task<int> UpdateAlbum(Album album) => await dataService.Update(album);
    }
}