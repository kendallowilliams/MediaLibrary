using MediaLibraryDAL.DbContexts;
using MediaLibraryDAL.Services.Interfaces;
using MediaLibraryWebUI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using static MediaLibraryWebUI.UIEnums;
using Fody;
using MediaLibraryWebUI.Models;
using MediaLibraryWebUI.Repositories;
using MediaLibraryWebUI.Models.Configurations;

namespace MediaLibraryWebUI.Services
{
    [ConfigureAwait(false)]
    [Export(typeof(IPlaylistUIService))]
    public class PlaylistUIService : BaseUIService, IPlaylistUIService
    {
        private readonly Lazy<IDataService> lazyDataService;
        private IDataService dataService => lazyDataService.Value;

        [ImportingConstructor]
        public PlaylistUIService(Lazy<IDataService> dataService) : base()
        {
            this.lazyDataService = dataService;
        }

        public async Task<IEnumerable<IGrouping<string, Playlist>>> GetPlaylistGroups(PlaylistSort sort)
        {
            IEnumerable<IGrouping<string, Playlist>> groups = null;
            IEnumerable<Playlist> playlists = await dataService.GetList<Playlist>(default, default, playlist => playlist.PlaylistTracks,
                                                                                                    playlist => playlist.PlaylistPodcastItems,
                                                                                                    playlist => playlist.PlaylistEpisodes);

            playlists = playlists.Concat(await GetSystemPlaylists());
           
            switch (sort)
            {
                case PlaylistSort.DateAdded:
                    groups = playlists.GroupBy(playlist => playlist.ModifyDate.ToString("MM-dd-yyyy")).OrderByDescending(group => DateTime.Parse(group.Key));
                    break;
                case PlaylistSort.AtoZ:
                default:
                    groups = GetPlaylistsAtoZ(playlists.OrderBy(playlist => playlist.Name));
                    break;
            }

            return groups;
        }

        private IEnumerable<IGrouping<string, Playlist>> GetPlaylistsAtoZ(IEnumerable<Playlist> playlists)
        {
            return playlists.GroupBy(playlist => getCharLabel(playlist.Name)).OrderBy(group => group.Key);
        }

        public async Task<IEnumerable<Playlist>> GetSystemPlaylists()
        {
            IEnumerable<Track> tracks = await dataService.GetList<Track>(default, default, track => track.Album, track => track.Artist);
            IEnumerable<PodcastItem> podcastItems = await dataService.GetList<PodcastItem>(default, default);
            IEnumerable<Episode> episodes = await dataService.GetList<Episode>(default, default, episode => episode.Series);
            IEnumerable<Playlist> playlists = Enumerable.Empty<Playlist>();

            playlists = PlaylistRepository.GetSystemPlaylists<Track>(25).Select((item, index) => new Playlist()
            {
                Id = -(++index),
                Name = item.Key,
                Type = (int)PlaylistTabs.Music,
                CreateDate = DateTime.Now,
                ModifyDate = DateTime.Now,
                PlaylistTracks = item.Value(tracks).Select(track => new PlaylistTrack() { Track = (Track)track }).ToList()
            });

            playlists = playlists.Concat(PlaylistRepository.GetSystemPlaylists<PodcastItem>(25).Select((item, index) => new Playlist()
            {
                Id = -(++index),
                Name = item.Key,
                Type = (int)PlaylistTabs.Podcast,
                CreateDate = DateTime.Now,
                ModifyDate = DateTime.Now,
                PlaylistPodcastItems = item.Value(podcastItems).Select(_item => new PlaylistPodcastItem() { PodcastItem = (PodcastItem)_item }).ToList()
            }));

            playlists = playlists.Concat(PlaylistRepository.GetSystemPlaylists<Episode>(25).Select((item, index) => new Playlist()
            {
                Id = -(++index),
                Name = item.Key,
                Type = (int)PlaylistTabs.Episode,
                CreateDate = DateTime.Now,
                ModifyDate = DateTime.Now,
                PlaylistEpisodes = item.Value(episodes).Select(episode => new PlaylistEpisode() { Episode = (Episode)episode }).ToList()
            }));

            return playlists;
        }
    }
}