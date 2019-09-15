using MediaLibraryDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MediaLibraryBLL.Services.Interfaces
{
    public interface IGenreService
    {
        Task<int?> AddGenre(string genres);

        Task<Genre> GetGenre(Expression<Func<Genre, bool>> expression = null);

        Task<IEnumerable<Genre>> GetGenres(Expression<Func<Genre, bool>> expression = null);

        Task<int> InsertGenre(Genre genre);

        Task<int> DeleteGenre(int id);

        Task<int> DeleteGenre(Genre genre);

        Task DeleteAllGenres();

        Task<int> UpdateGenre(Genre genre);
    }
}
