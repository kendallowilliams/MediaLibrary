using MediaLibraryDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MediaLibraryBLL.Services.Interfaces
{
    public interface IAlbumService
    {
        Task<int?> AddAlbum(Album album);

        Task<Album> GetAlbum(Expression<Func<Album, bool>> expression = null);

        Task<IEnumerable<Album>> GetAlbums(Expression<Func<Album, bool>> expression = null);

        Task<int> InsertAlbum(Album album);

        Task<int> DeleteAlbum(int id);

        Task<int> DeleteAlbum(Album album);

        Task DeleteAllAlbums();

        Task<int> UpdateAlbum(Album album);
    }
}
