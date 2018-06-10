using MusicLibraryBLL.Models;
using MusicLibraryBLL.Services;
using MusicLibraryBLL.Services.Interfaces;
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
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class AlbumController : ApiController
    {
        private IAlbumService albumService;

        [ImportingConstructor]
        public AlbumController(IAlbumService albumService)
        {
            this.albumService = albumService;
        }

        // GET: api/Album
        public async Task<IEnumerable<Album>> Get()
        {
            return (await albumService.GetAlbums()).OrderBy(album => album.Title);
        }

        // GET: api/Album/5
        public async Task<Album> Get(int id)
        {
            return await albumService.GetAlbum(id);
        }
    }
}
