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
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Playlist/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Playlist/5
        public async Task Delete(int id)
        {
            await playlistService.DeletePlaylist(id);
        }
    }
}
