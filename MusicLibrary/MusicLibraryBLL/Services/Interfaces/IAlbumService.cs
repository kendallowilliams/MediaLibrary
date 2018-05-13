using MusicLibraryBLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicLibraryBLL.Services.Interfaces
{
    public interface IAlbumService
    {
        Task<Album> GetAlbum(object id);

        Task<IEnumerable<Album>> GetAlbums();

        Task<int> InsertAlbum(Album album);

        Task<bool> DeleteAlbum(int id);

        Task<bool> DeleteAlbum(Album album);

        Task<bool> UpdateAlbum(Album album);
    }
}
