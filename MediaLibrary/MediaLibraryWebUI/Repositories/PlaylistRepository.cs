using MediaLibraryBLL.Models;
using MediaLibraryBLL.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static MediaLibraryWebUI.UIEnums;

namespace MediaLibraryWebUI.Repositories
{
    public static class PlaylistRepository
    {
        public static IEnumerable<IListItem<object, PlaylistSort>> GetPlaylistSortItems()
        {
            yield return new ListItem<object, PlaylistSort>(null, "Date added", PlaylistSort.DateAdded);
            yield return new ListItem<object, PlaylistSort>(null, "A to Z", PlaylistSort.AtoZ);
        }
    }
}