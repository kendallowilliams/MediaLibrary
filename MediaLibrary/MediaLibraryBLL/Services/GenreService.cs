using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Fody;
using MediaLibraryDAL.DbContexts;
using MediaLibraryBLL.Services.Interfaces;
using MediaLibraryDAL.Services.Interfaces;
using System.Linq.Expressions;

namespace MediaLibraryBLL.Services
{
    [ConfigureAwait(false)]
    [Export(typeof(IGenreService))]
    public class GenreService : IGenreService
    {
        private readonly IDataService dataService;

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
                Genre dbGenre = await dataService.GetAsync<Genre>(item => item.Name == strGenres),
                      genre = new Genre(strGenres);

                if (dbGenre != null) { id = dbGenre.Id; }
                else
                {
                    await dataService.Insert(genre);
                    id = genre.Id;
                }
            }

            return id;
        }
    }
}