using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Fody;
using MusicLibraryBLL.Models;
using MusicLibraryBLL.Services.Interfaces;

namespace MusicLibraryBLL.Services
{
    [ConfigureAwait(false)]
    [Export(typeof(IAlbumService))]
    public class AlbumService : IAlbumService
    {
        [Import]
        private IDataService dataService { get; set; }

        [ImportingConstructor]
        public AlbumService()
        { }

        public async Task<int?> AddAlbum(Album album)
        {
            int? id = default(int?);

            if (album != null)
            {
                string existsQuery = $"SELECT id FROM album WHERE title = @title";
                id = await dataService.ExecuteScalar<int?>(existsQuery, new { album.Title });

                if (!id.HasValue)
                {
                    id = await dataService.Insert<Album, int>(album);
                }
            }

            return id;
        }

        public async Task<IEnumerable<Album>> GetAlbums() => await dataService.GetList<Album>();

        public async Task<Album> GetAlbum(object id) =>  await dataService.Get<Album>(id);

        public async Task<int> InsertAlbum(Album album) => await dataService.Insert<Album,int>(album);

        public async Task<bool> DeleteAlbum(int id) => await dataService.Delete<Album>(id);

        public async Task<bool> DeleteAlbum(Album album) => await dataService.Delete(album);

        public async Task DeleteAllAlbums() => await dataService.Execute(@"DELETE album;");

        public async Task<bool> UpdateAlbum(Album album) => await dataService.Update(album);
    }
}