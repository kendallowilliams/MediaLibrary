using MediaLibraryDAL.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MediaLibraryBLL.Services.Interfaces
{
    public interface IPlaylistService
    {
        Task<Playlist> GetPlaylist(Expression<Func<Playlist, bool>> expression = null);

        Task<IEnumerable<Playlist>> GetPlaylists(Expression<Func<Playlist, bool>> expression = null);

        Task<int> InsertPlaylist(Playlist playlist);

        Task DeletePlaylist(int id);

        Task DeleteAllPlaylists();

        Task<int> UpdatePlaylist(Playlist playlist);
    }
}
