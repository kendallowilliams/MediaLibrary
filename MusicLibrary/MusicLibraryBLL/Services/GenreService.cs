using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
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
        [Import]
        private IDataService dataService { get; set; }

        [ImportingConstructor]
        public GenreService()
        { }

        public async Task<int?> AddGenre(string genres)
        {
            int? id = default(int?);

            if (!string.IsNullOrWhiteSpace(genres))
            {
                string existsQuery = $"SELECT id FROM genre WHERE name = @genres";
                id = await dataService.ExecuteScalar<int?>(existsQuery, new { genres });
                Genre genre = new Genre(genres);

                if (!id.HasValue)
                {
                    id = await dataService.Insert<Genre, int>(genre);
                }
            }

            return id;
        }

        public async Task<IEnumerable<Genre>> GetGenres() => await dataService.GetList<Genre>();

        public async Task<Genre> GetGenre(object id) => await dataService.Get<Genre>(id);

        public async Task<int> InsertGenre(Genre genre) => await dataService.Insert<Genre,int>(genre);

        public async Task<bool> DeleteGenre(int id) => await dataService.Delete<Genre>(id);

        public async Task<bool> DeleteGenre(Genre genre) => await dataService.Delete(genre);

        public async Task DeleteAllGenres() => await dataService.Execute(@"DELETE genre;");

        public async Task<bool> UpdateGenre(Genre genre) => await dataService.Update(genre);
    }
}