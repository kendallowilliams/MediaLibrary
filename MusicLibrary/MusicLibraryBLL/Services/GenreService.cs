using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Fody;
using MusicLibraryBLL.Models;
using MusicLibraryBLL.Services.Interfaces;

namespace MusicLibraryBLL.Services
{
    [ConfigureAwait(false)]
    [Export(typeof(IGenreService))]
    public class GenreService : IGenreService
    {
        private readonly IDataService dataService;
        private readonly string findGenresStoredProcedure = "FindGenres",
                                deleteAllGenresStoredProcedure = "DeleteAllGenres";

        [ImportingConstructor]
        public GenreService(IDataService dataService)
        {
            this.dataService = dataService;
        }

        public async Task<int?> AddGenre(string strGenres)
        {
            int? id = default(int?);

            if (!string.IsNullOrWhiteSpace(strGenres))
            {
                object parameters = new { name = strGenres };
                IEnumerable<Genre> genres = await dataService.Query<Genre>(findGenresStoredProcedure, parameters, CommandType.StoredProcedure);
                Genre genre = new Genre(strGenres);

                if (genres.Any()) { id = genres.FirstOrDefault().Id; }
                else { id = await dataService.Insert<Genre, int>(genre); }
            }

            return id;
        }

        public async Task<IEnumerable<Genre>> GetGenres() => await dataService.GetList<Genre>();

        public async Task<Genre> GetGenre(object id) => await dataService.Get<Genre>(id);

        public async Task<int> InsertGenre(Genre genre) => await dataService.Insert<Genre,int>(genre);

        public async Task<bool> DeleteGenre(int id) => await dataService.Delete<Genre>(id);

        public async Task<bool> DeleteGenre(Genre genre) => await dataService.Delete(genre);

        public async Task DeleteAllGenres() => await dataService.Execute(deleteAllGenresStoredProcedure, commandType: CommandType.StoredProcedure);

        public async Task<bool> UpdateGenre(Genre genre) => await dataService.Update(genre);
    }
}