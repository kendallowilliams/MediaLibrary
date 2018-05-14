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
    public class GenreController : ApiController
    {
        private IGenreService genreService => MefConfig.Container.GetExportedValue<IGenreService>();

        // GET: api/Genre
        public async Task<IEnumerable<Genre>> Get()
        {
            return await genreService.GetGenres();
        }

        // GET: api/Genre/5
        public async Task<Genre> Get(int id)
        {
            return await genreService.GetGenre(id);
        }
    }
}
