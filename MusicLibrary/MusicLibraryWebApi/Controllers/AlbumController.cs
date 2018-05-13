using MusicLibraryWebApi.Models;
using MusicLibraryWebApi.Services;
using MusicLibraryWebApi.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace MusicLibraryWebApi.Controllers
{
    public class AlbumController : ApiController
    {
        private IAlbumService albumService => MefConfig.Container.GetExportedValue<IAlbumService>();

        // GET: api/Album
        public async Task<IEnumerable<Album>> Get()
        {
            return await albumService.GetAlbums();
        }

        // GET: api/Album/5
        public async Task<Album> Get(int id)
        {
            return await albumService.GetAlbum(id);
        }

        // POST: api/Album
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Album/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Album/5
        public async Task Delete(int id)
        {
            await albumService.DeleteAlbum(id);
        }
    }
}
