using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using MusicLibraryBLL.Models;
using MusicLibraryBLL.Services.Interfaces;

namespace MusicLibraryBLL.Services
{
    [Export(typeof(IAlbumService))]
    public class AlbumService : IAlbumService
    {
        [Import]
        private IDataService dataService { get; set; }

        [ImportingConstructor]
        public AlbumService()
        { }

        public async Task<IEnumerable<Album>> GetAlbums() => await dataService.GetList<Album>();

        public async Task<Album> GetAlbum(object id) =>  await dataService.Get<Album>(id);

        public async Task<int> InsertAlbum(Album album) => await dataService.Insert<Album,int>(album);

        public async Task<bool> DeleteAlbum(int id) => await dataService.Delete<Album>(id);

        public async Task<bool> DeleteAlbum(Album album) => await dataService.Delete(album);

        public async Task<bool> UpdateAlbum(Album album) => await dataService.Update(album);
    }
}