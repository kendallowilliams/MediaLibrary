using MediaLibraryBLL.Models;
using MediaLibraryBLL.Models.Interfaces;
using MediaLibraryDAL.DbContexts;
using MediaLibraryDAL.Models.Interfaces;
using MediaLibraryWebUI.Models.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static MediaLibraryWebUI.UIEnums;

namespace MediaLibraryWebUI.Repositories
{
    using SYSTEM_PLAYLIST = KeyValuePair<string, Func<IEnumerable<IPlayableItem>, IEnumerable<IPlayableItem>>>;
    using PLAYLIST_TYPE_SORT_MAPPING = KeyValuePair<PlaylistTabs, Func<PlaylistConfiguration, PlaylistSort>>;

    public static class PlaylistRepository
    {
        public static IEnumerable<IListItem<object, PlaylistSort>> GetPlaylistSortItems()
        {
            yield return new ListItem<object, PlaylistSort>(null, "Date added", PlaylistSort.DateAdded);
            yield return new ListItem<object, PlaylistSort>(null, "A to Z", PlaylistSort.AtoZ);
        }

        public static IEnumerable<SYSTEM_PLAYLIST> GetSystemPlaylists<T>(int count) where T : IPlayableItem
        {
            yield return new SYSTEM_PLAYLIST($"Top {count} Most Played", items => items.Where(item => item.PlayCount > 0)
                                                                                       .OrderByDescending(item => item.PlayCount)
                                                                                       .Take(count));
            yield return new SYSTEM_PLAYLIST($"Top {count} Recently Added", items => items.OrderByDescending(item => item.CreateDate)
                                                                                          .Take(count));
            yield return new SYSTEM_PLAYLIST($"Top {count} Recently Played", items => items.Where(item => item.LastPlayedDate.HasValue)
                                                                                           .OrderByDescending(item => item.LastPlayedDate.Value)
                                                                                           .Take(count));
        }

        public static IEnumerable<PLAYLIST_TYPE_SORT_MAPPING>GetPlaylistTypePlaylistSortMappings()
        {
            yield return new PLAYLIST_TYPE_SORT_MAPPING(PlaylistTabs.Music, configuration => configuration.SelectedMusicPlaylistSort);
            yield return new PLAYLIST_TYPE_SORT_MAPPING(PlaylistTabs.Podcast, configuration => configuration.SelectedPodcastPlaylistSort);
            yield return new PLAYLIST_TYPE_SORT_MAPPING(PlaylistTabs.Television, configuration => configuration.SelectedTelevisionPlaylistSort);
        }
    }
}