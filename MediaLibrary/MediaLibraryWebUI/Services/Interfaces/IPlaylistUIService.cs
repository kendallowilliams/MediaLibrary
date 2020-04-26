using MediaLibraryDAL.DbContexts;
using MediaLibraryWebUI.Models.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaLibraryWebUI.Services.Interfaces
{
    public interface IPlaylistUIService
    {
        Task<IEnumerable<IGrouping<string, Playlist>>> GetPlaylistGroups(PlaylistConfiguration configuration);
        Task<IEnumerable<Playlist>> GetSystemPlaylists();
    }
}
