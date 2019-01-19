using MediaLibraryBLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaLibraryBLL.Services.Interfaces
{
    public interface IPlaylistService
    {
        Task<Playlist> GetPlaylist(object id);

        Task<IEnumerable<Playlist>> GetPlaylists();

        Task<int> InsertPlaylist(Playlist playlist);

        Task DeletePlaylist(int id);

        Task DeleteAllPlaylists();

        Task<bool> UpdatePlaylist(Playlist playlist);
    }
}
