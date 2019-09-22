using MediaLibraryDAL.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MediaLibraryWebUI.Models
{
    public class PlaylistViewModel
    {
        private IEnumerable<IGrouping<string, Playlist>> playlists;

        public PlaylistViewModel()
        {
            playlists = Enumerable.Empty<IGrouping<string, Playlist>>();
        }

        public IEnumerable<IGrouping<string, Playlist>> Playlists { get => playlists; set => playlists = value; }
    }
}