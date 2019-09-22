using MediaLibraryDAL.DbContexts;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;

namespace MediaLibraryWebUI.Models
{
    [Export]
    public class PlaylistViewModel
    {
        private IEnumerable<Playlist> playlists;
        private readonly HomeViewModel homeViewModel;

        [ImportingConstructor]
        public PlaylistViewModel(HomeViewModel homeViewModel)
        {
            playlists = Enumerable.Empty<Playlist>();
            this.homeViewModel = homeViewModel;
        }

        public IEnumerable<Playlist> Playlists { get => playlists; set => playlists = value; }
    }
}