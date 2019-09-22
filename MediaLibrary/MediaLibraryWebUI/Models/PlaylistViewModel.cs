using MediaLibraryDAL.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MediaLibraryWebUI.Models
{
    public class PlaylistViewModel
    {
        private IEnumerable<Playlist> playlists;

        public PlaylistViewModel()
        {
            playlists = Enumerable.Empty<Playlist>();
        }

        public IEnumerable<Playlist> Playlists { get => playlists; set => playlists = value; }
    }
}