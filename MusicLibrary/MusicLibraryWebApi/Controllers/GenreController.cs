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
    public class GenreController : ApiController
    {
        private IGenreService genreService;

        [ImportingConstructor]
        public GenreController(IGenreService genreService)
        {
            this.genreService = genreService;
        }

        // GET: api/Genre
        public async Task<IEnumerable<Genre>> Get()
        {
            return (await genreService.GetGenres()).OrderBy(genre => genre.Name);
        }

        // GET: api/Genre/5
        public async Task<Genre> Get(int id)
        {
            return await genreService.GetGenre(id);
        }
    }
}
