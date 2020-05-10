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
using System.Linq.Expressions;

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

        public async Task<IEnumerable<IGrouping<string, Playlist>>> GetPlaylistGroups(PlaylistConfiguration configuration)
        {
            IEnumerable<IGrouping<string, Playlist>> groups = Enumerable.Empty<IGrouping<string, Playlist>>();
            IEnumerable<Playlist> playlists = await dataService.GetList<Playlist>(default, default, playlist => playlist.PlaylistTracks,
                                                                                                    playlist => playlist.PlaylistPodcastItems,
                                                                                                    playlist => playlist.PlaylistEpisodes);
            var playlistTypeSortMappings = PlaylistRepository.GetPlaylistTypePlaylistSortMappings();

            playlists = playlists.Concat(await GetSystemPlaylists());
           
            foreach(var group in playlists.GroupBy(playlist => playlist.Type))
            {
                var mapping = playlistTypeSortMappings.FirstOrDefault(item => item.Key == (PlaylistTabs)group.Key);

                switch (mapping.Value(configuration))
                {
                    case PlaylistSort.DateAdded:
                        groups = groups.Concat(group.GroupBy(playlist => playlist.ModifyDate.ToString("MM-dd-yyyy"))
                                       .OrderByDescending(_group => DateTime.Parse(_group.Key)));
                        break;
                    case PlaylistSort.AtoZ:
                    default:
                        groups = groups.Concat(GetPlaylistsAtoZ(group.OrderBy(playlist => playlist.Name)));
                        break;
                }
            }

            return groups;
        }

        private IEnumerable<IGrouping<string, Playlist>> GetPlaylistsAtoZ(IEnumerable<Playlist> playlists)
        {
            return playlists.GroupBy(playlist => getCharLabel(playlist.Name)).OrderBy(group => group.Key);
        }

        public async Task<IEnumerable<Playlist>> GetSystemPlaylists(bool includeItems = false, bool includeReferences = false)
        {
            IEnumerable<Playlist> playlists = Enumerable.Empty<Playlist>();
            int count = 0;
            IEnumerable<Track> tracks = Enumerable.Empty<Track>();
            IEnumerable<PodcastItem> podcastItems = Enumerable.Empty<PodcastItem>();
            IEnumerable<Episode> episodes = Enumerable.Empty<Episode>();

            if (includeItems)
            {
                var trackParams = includeReferences ? new Expression<Func<Track, object>>[] { track => track.Album, track => track.Artist } :
                                                       new Expression<Func<Track, object>>[0];
                Task<IEnumerable<Track>> trackTasks = dataService.GetList<Track>(default, default, trackParams)
                                                                 .ContinueWith(task => tracks = task.Result);
                var podcastItemParams = includeReferences ? new Expression<Func<PodcastItem, object>>[] { item => item.Podcast } :
                                                            new Expression<Func<PodcastItem, object>>[0];
                Task<IEnumerable<PodcastItem>> podcastItemTask = dataService.GetList<PodcastItem>(default, default, podcastItemParams)
                                                                 .ContinueWith(task => podcastItems = task.Result);
                var episodeParams = includeReferences ? new Expression<Func<Episode, object>>[] { episode => episode.Series } :
                                                        new Expression<Func<Episode, object>>[0];
                Task<IEnumerable<Episode>> episodeTask = dataService.GetList<Episode>(default, default, episodeParams)
                                                                 .ContinueWith(task => episodes = task.Result);

                await Task.WhenAll(trackTasks, podcastItemTask, episodeTask);
            }

            playlists = PlaylistRepository.GetSystemPlaylists<Track>(25).Select((item, index) => new Playlist()
            {
                Id = -(++index + count),
                Name = $"{item.Key} [{PlaylistTabs.Music}]",
                Type = (int)PlaylistTabs.Music,
                CreateDate = DateTime.Now,
                ModifyDate = DateTime.Now,
                PlaylistTracks = item.Value(tracks).Select(track => new PlaylistTrack() { Track = (Track)track }).ToList()
            }).ToList();
            count = playlists.Count();
            playlists = playlists.Concat(PlaylistRepository.GetSystemPlaylists<PodcastItem>(25).Select((item, index) => new Playlist()
            {
                Id = -(++index + count),
                Name = $"{item.Key} [{PlaylistTabs.Podcast}]",
                Type = (int)PlaylistTabs.Podcast,
                CreateDate = DateTime.Now,
                ModifyDate = DateTime.Now,
                PlaylistPodcastItems = item.Value(podcastItems).Select(_item => new PlaylistPodcastItem() { PodcastItem = (PodcastItem)_item }).ToList()
            }).ToList());
            count = playlists.Count();
            playlists = playlists.Concat(PlaylistRepository.GetSystemPlaylists<Episode>(25).Select((item, index) => new Playlist()
            {
                Id = -(++index + count),
                Name = $"{item.Key} [{PlaylistTabs.Television}]",
                Type = (int)PlaylistTabs.Television,
                CreateDate = DateTime.Now,
                ModifyDate = DateTime.Now,
                PlaylistEpisodes = item.Value(episodes).Select(episode => new PlaylistEpisode() { Episode = (Episode)episode }).ToList()
            }).ToList());

            return playlists;
        }
    }
}