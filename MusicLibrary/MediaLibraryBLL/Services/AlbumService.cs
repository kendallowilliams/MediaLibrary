using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Fody;
using MediaLibraryBLL.Models;
using MediaLibraryBLL.Services.Interfaces;

namespace MediaLibraryBLL.Services
{
    [ConfigureAwait(false)]
    [Export(typeof(IAlbumService))]
    public class AlbumService : IAlbumService
    {
        private readonly IDataService dataService;
        private readonly string findAlbumsStoredProcedure = "FindAlbums",
                                deleteAllAlbumsStoredProcedure = "DeleteAllAlbums";

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
                object parameters = new { title = album.Title };
                IEnumerable<Album> albums = await dataService.Query<Album>(findAlbumsStoredProcedure, parameters, CommandType.StoredProcedure);

                if (albums.Any()) { id = albums.FirstOrDefault().Id; }
                else { id = await dataService.Insert<Album, int>(album); }
            }

            return id;
        }

        public async Task<IEnumerable<Album>> GetAlbums() => await dataService.GetList<Album>();

        public async Task<Album> GetAlbum(object id) =>  await dataService.Get<Album>(id);

        public async Task<int> InsertAlbum(Album album) => await dataService.Insert<Album,int>(album);

        public async Task<bool> DeleteAlbum(int id) => await dataService.Delete<Album>(id);

        public async Task<bool> DeleteAlbum(Album album) => await dataService.Delete(album);

        public async Task DeleteAllAlbums() => await dataService.Execute(deleteAllAlbumsStoredProcedure, commandType: CommandType.StoredProcedure);

        public async Task<bool> UpdateAlbum(Album album) => await dataService.Update(album);
    }
}