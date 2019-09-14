using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Fody;
using MediaLibraryDAL.Models;
using MediaLibraryBLL.Services.Interfaces;
using MediaLibraryDAL.Services.Interfaces;
using System.Linq.Expressions;

namespace MediaLibraryBLL.Services
{
    [ConfigureAwait(false)]
    [Export(typeof(IPlaylistService))]
    public class PlaylistService : IPlaylistService
    {
        private readonly IDataService dataService;

        [ImportingConstructor]
        public PlaylistService(IDataService dataService)
        {
            this.dataService = dataService;
        }

        public IEnumerable<Playlist> GetPlaylists(Expression<Func<Playlist, bool>> expression = null) => dataService.GetList(expression);

        public Playlist GetPlaylist(Expression<Func<Playlist, bool>> expression = null) => dataService.Get(expression);

        public async Task<int> InsertPlaylist(Playlist playlist) => await dataService.Insert(playlist);

        public async Task DeletePlaylist(int id) => await dataService.Delete<Playlist>(id);

        public async Task DeleteAllPlaylists() => await dataService.DeleteAll<Playlist>();

        public async Task<int> UpdatePlaylist(Playlist playlist) => await dataService.Update(playlist);
    }
}