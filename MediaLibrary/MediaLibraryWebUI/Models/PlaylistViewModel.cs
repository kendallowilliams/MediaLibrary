using MediaLibraryDAL.DbContexts;
using MediaLibraryWebUI.Models.Configurations;
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
        [ImportingConstructor]
        public PlaylistViewModel(HomeViewModel homeViewModel)
        {
            PlaylistGroups = Enumerable.Empty<IGrouping<string, Playlist>>();
            this.HomeViewModel = homeViewModel;
            Configuration = new PlaylistConfiguration();
        }

        public Playlist SelectedPlaylist { get; set; }
        public IEnumerable<IGrouping<string, Playlist>> PlaylistGroups { get; set; }
        public HomeViewModel HomeViewModel { get; }
        public PlaylistConfiguration Configuration { get; set; }
    }
}