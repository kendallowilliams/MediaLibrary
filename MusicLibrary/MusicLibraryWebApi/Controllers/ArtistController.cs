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
    public class ArtistController : ApiController
    {
        private IArtistService artistService;

        public ArtistController()
        {
            artistService = MefConfig.Container.GetExportedValue<IArtistService>();
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
