using MusicLibraryBLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicLibraryBLL.Services.Interfaces
{
    public interface IGenreService
    {
        Task<int> AddGenre(string[] genres);

        Task<Genre> GetGenre(object id);

        Task<IEnumerable<Genre>> GetGenres();

        Task<int> InsertGenre(Genre genre);

        Task<bool> DeleteGenre(int id);

        Task<bool> DeleteGenre(Genre genre);

        Task<bool> UpdateGenre(Genre genre);
    }
}
