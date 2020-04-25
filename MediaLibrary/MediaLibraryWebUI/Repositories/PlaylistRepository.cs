using MediaLibraryBLL.Models;
using MediaLibraryBLL.Models.Interfaces;
using MediaLibraryDAL.DbContexts;
using MediaLibraryDAL.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static MediaLibraryWebUI.UIEnums;

namespace MediaLibraryWebUI.Repositories
{
    using SYSTEM_PLAYLIST = KeyValuePair<string, Func<IEnumerable<IPlayableItem>, IEnumerable<IPlayableItem>>>;

    public static class PlaylistRepository
    {
        public static IEnumerable<IListItem<object, PlaylistSort>> GetPlaylistSortItems()
        {
            yield return new ListItem<object, PlaylistSort>(null, "Date added", PlaylistSort.DateAdded);
            yield return new ListItem<object, PlaylistSort>(null, "A to Z", PlaylistSort.AtoZ);
        }

        public static IEnumerable<SYSTEM_PLAYLIST> GetSystemPlaylists<T>(int count) where T : IPlayableItem
        {
            yield return new SYSTEM_PLAYLIST($"Top {count} Most Played", items => items.OrderByDescending(item => item.PlayCount).Take(count));
            yield return new SYSTEM_PLAYLIST($"Top {count} Recently Added", items => items.OrderByDescending(item => item.CreateDate).Take(count));
            yield return new SYSTEM_PLAYLIST($"Top {count} Recently Played", items => items.Where(item => item.LastPlayedDate.HasValue)
                                                                                           .OrderByDescending(item => item.LastPlayedDate.Value)
                                                                                           .Take(count));
        }
    }
}