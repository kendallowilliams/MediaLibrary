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
    [Export(typeof(IPlaylistService))]
    public class PlaylistService : IPlaylistService
    {
        private readonly IDataService dataService;
        private readonly string deletePlaylistStoredProcedure = "DeletePlaylist",
                                deleteAllPlaylistsStoredProcedure = "DeleteAllPlaylists";

        [ImportingConstructor]
        public PlaylistService(IDataService dataService)
        {
            this.dataService = dataService;
        }

        public async Task<IEnumerable<Playlist>> GetPlaylists() => await dataService.GetList<Playlist>();

        public async Task<Playlist> GetPlaylist(object id) => await dataService.Get<Playlist>(id);

        public async Task<int> InsertPlaylist(Playlist playlist) => await dataService.Insert<Playlist,int>(playlist);

        public async Task DeletePlaylist(int id) => await dataService.Execute(deletePlaylistStoredProcedure, new { playlist_id = id} , commandType: CommandType.StoredProcedure);

        public async Task DeleteAllPlaylists() => await dataService.Execute(deleteAllPlaylistsStoredProcedure, commandType: CommandType.StoredProcedure);

        public async Task<bool> UpdatePlaylist(Playlist playlist) => await dataService.Update(playlist);
    }
}