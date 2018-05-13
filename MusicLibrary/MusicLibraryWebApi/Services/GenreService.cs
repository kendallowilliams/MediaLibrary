using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using MusicLibraryWebApi.Models;
using MusicLibraryWebApi.Services.Interfaces;

namespace MusicLibraryWebApi.Services
{
    [Export(typeof(IGenreService))]
    public class GenreService : IGenreService
    {
        [Import]
        private IDataService dataService { get; set; }

        [ImportingConstructor]
        public GenreService()
        { }

        public async Task<IEnumerable<Genre>> GetGenres() => await dataService.GetList<Genre>();

        public async Task<Genre> GetGenre(object id) => await dataService.Get<Genre>(id);

        public async Task<int> InsertGenre(Genre genre) => await dataService.Insert<Genre,int>(genre);

        public async Task<bool> DeleteGenre(int id) => await dataService.Delete<Genre>(id);

        public async Task<bool> DeleteGenre(Genre genre) => await dataService.Delete(genre);

        public async Task<bool> UpdateGenre(Genre genre) => await dataService.Update(genre);
    }
}