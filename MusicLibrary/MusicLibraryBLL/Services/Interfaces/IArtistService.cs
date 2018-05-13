using MusicLibraryBLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicLibraryBLL.Services.Interfaces
{
    public interface IArtistService
    {
        Task<Artist> GetArtist(object id);

        Task<IEnumerable<Artist>> GetArtists();

        Task<int> InsertArtist(Artist artist);

        Task<bool> DeleteArtist(int id);

        Task<bool> DeleteArtist(Artist artist);

        Task<bool> UpdateArtist(Artist artist);
    }
}
