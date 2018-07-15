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
    [Export(typeof(IPlaylistService))]
    public class PlaylistService : IPlaylistService
    {
        [Import]
        private IDataService dataService { get; set; }

        [ImportingConstructor]
        public PlaylistService()
        { }

        public async Task<IEnumerable<Playlist>> GetPlaylists() => await dataService.GetList<Playlist>();

        public async Task<Playlist> GetPlaylist(object id) => await dataService.Get<Playlist>(id);

        public async Task<int> InsertPlaylist(Playlist playlist) => await dataService.Insert<Playlist,int>(playlist);

        public async Task<bool> DeletePlaylist(int id) => await dataService.Delete<Playlist>(id);

        public async Task<bool> DeletePlaylist(Playlist playlist) => await dataService.Delete(playlist);

        public async Task DeleteAllPlaylists()
        {
            await dataService.Execute(@"DELETE playlist_track;");
            await dataService.Execute(@"DELETE playlist;");
        }

        public async Task<bool> UpdatePlaylist(Playlist playlist) => await dataService.Update(playlist);
    }
}