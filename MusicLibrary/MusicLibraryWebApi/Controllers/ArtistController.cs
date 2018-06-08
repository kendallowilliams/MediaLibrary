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
    public class ArtistController : ApiController
    {
        private IArtistService artistService;

        [ImportingConstructor]
        public ArtistController(IArtistService artistService)
        {
            this.artistService = artistService;
        }

        // GET: api/Artist
        public async Task<IEnumerable<Artist>> Get()
        {
            return await artistService.GetArtists();
        }

        // GET: api/Artist/5
        public async Task<Artist> Get(int id)
        {
            return await artistService.GetArtist(id);
        }
    }
}
