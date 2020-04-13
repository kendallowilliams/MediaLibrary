using MediaLibraryBLL.Models;
using MediaLibraryBLL.Models.Interfaces;
using MediaLibraryDAL.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static MediaLibraryWebUI.UIEnums;

namespace MediaLibraryWebUI.Repositories
{
    using SYSTEM_PLAYLIST = KeyValuePair<string, Func<IEnumerable<Track>, IEnumerable<Track>>>;

    public static class PlaylistRepository
    {
        public static IEnumerable<IListItem<object, PlaylistSort>> GetPlaylistSortItems()
        {
            yield return new ListItem<object, PlaylistSort>(null, "Date added", PlaylistSort.DateAdded);
            yield return new ListItem<object, PlaylistSort>(null, "A to Z", PlaylistSort.AtoZ);
        }

        public static IEnumerable<SYSTEM_PLAYLIST> GetSystemPlaylists(int count)
        {
            yield return new SYSTEM_PLAYLIST($"Top {count} Most Played", tracks => tracks.OrderByDescending(track => track.PlayCount).Take(count));
            yield return new SYSTEM_PLAYLIST($"Top {count} Recently Added", tracks => tracks.OrderByDescending(track => track.CreateDate).Take(count));
            yield return new SYSTEM_PLAYLIST($"Top {count} Recently Played", tracks => tracks.Where(track => track.LastPlayedDate.HasValue)
                                                                                             .OrderByDescending(track => track.LastPlayedDate.Value)
                                                                                             .Take(count));
        }
    }
}