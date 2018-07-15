using MusicLibraryBLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicLibraryBLL.Services.Interfaces
{
    public interface IPlaylistService
    {
        Task<Playlist> GetPlaylist(object id);

        Task<IEnumerable<Playlist>> GetPlaylists();

        Task<int> InsertPlaylist(Playlist playlist);

        Task<bool> DeletePlaylist(int id);

        Task<bool> DeletePlaylist(Playlist playlist);

        Task DeleteAllPlaylists();

        Task<bool> UpdatePlaylist(Playlist playlist);
    }
}
