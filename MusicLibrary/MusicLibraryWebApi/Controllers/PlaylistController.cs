using MusicLibraryBLL.Models;
using MusicLibraryBLL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace MusicLibraryWebApi.Controllers
{
    public class PlaylistController : ApiController
    {
        private IPlaylistService playlistService => MefConfig.Container.GetExportedValue<IPlaylistService>();

        // GET: api/Playlist
        public async Task<IEnumerable<Playlist>> Get()
        {
            return await playlistService.GetPlaylists();
        }

        // GET: api/Playlist/5
        public async Task<Playlist> Get(int id)
        {
            return await playlistService.GetPlaylist(id);
        }

        // POST: api/Playlist
        public async Task<int> Post([FromBody]Playlist playlist)
        {
            return await playlistService.InsertPlaylist(playlist);
        }

        // PUT: api/Playlist/5
        public async Task<bool> Put(int id, [FromBody]Playlist playlist)
        {
            return await playlistService.UpdatePlaylist(playlist);
        }

        // DELETE: api/Playlist/5
        public async Task<bool> Delete(int id)
        {
            return await playlistService.DeletePlaylist(id);
        }
    }
}
