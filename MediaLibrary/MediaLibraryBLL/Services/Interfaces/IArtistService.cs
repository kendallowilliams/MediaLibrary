using MediaLibraryDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MediaLibraryBLL.Services.Interfaces
{
    public interface IArtistService
    {
        Task<int?> AddArtist(string artists);

        Artist GetArtist(Expression<Func<Artist, bool>> expression = null);

        IEnumerable<Artist> GetArtists(Expression<Func<Artist, bool>> expression = null);

        Task<int> InsertArtist(Artist artist);

        Task<int> DeleteArtist(int id);

        Task<int> DeleteArtist(Artist artist);

        Task DeleteAllArtists();

        Task<int> UpdateArtist(Artist artist);
    }
}
