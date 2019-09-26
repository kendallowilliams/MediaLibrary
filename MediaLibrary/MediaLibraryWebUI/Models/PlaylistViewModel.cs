using MediaLibraryDAL.DbContexts;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;

namespace MediaLibraryWebUI.Models
{
    [Export]
    public class PlaylistViewModel : ViewModel
    {
        private IEnumerable<IGrouping<string, Playlist>> playlistGroups;
        private readonly HomeViewModel homeViewModel;
        private Playlist selectedPlaylist;

        [ImportingConstructor]
        public PlaylistViewModel(HomeViewModel homeViewModel)
        {
            playlistGroups = Enumerable.Empty<IGrouping<string, Playlist>>();
            this.homeViewModel = homeViewModel;
        }
        
        public Playlist SelectedPlaylist { get => selectedPlaylist; set => selectedPlaylist = value; }
        public IEnumerable<IGrouping<string, Playlist>> PlaylistGroups { get => playlistGroups; set => playlistGroups = value; }
        public HomeViewModel HomeViewModel { get => homeViewModel; }
    }
}