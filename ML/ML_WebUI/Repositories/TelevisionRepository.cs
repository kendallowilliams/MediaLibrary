using MediaLibraryBLL.Models;
using MediaLibraryBLL.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static MediaLibraryWebUI.UIEnums;

namespace MediaLibraryWebUI.Repositories
{
    public static class TelevisionRepository
    {
        public static IEnumerable<IListItem<object, SeriesSort>> GetSeriesSortItems()
        {
            yield return new ListItem<object, SeriesSort>(null, "A to Z", SeriesSort.AtoZ);
        }
    }
}