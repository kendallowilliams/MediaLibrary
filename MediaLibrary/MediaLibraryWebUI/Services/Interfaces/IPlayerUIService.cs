using MediaLibraryBLL.Models;
using MediaLibraryDAL.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MediaLibraryWebUI.Enums;

namespace MediaLibraryWebUI.Services.Interfaces
{

    public interface IPlayerUIService
    {
        Task<IEnumerable<Track>> GetNowPlayingSongs(); 
        Task<IEnumerable<PodcastItem>> GetNowPlayingPodcastItems(); 
        Task<IEnumerable<Episode>> GetNowPlayingEpisodes();
        void UpdateNowPlaying(IEnumerable<ListItem<int, int>> items, MediaTypes mediaType);
        void ClearNowPlaying(MediaTypes mediaType);
    }
}
