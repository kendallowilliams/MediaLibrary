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
    public class PlaylistViewModel : ViewModel<PlaylistConfiguration>
    {
        [ImportingConstructor]
        public PlaylistViewModel()
        {
            PlaylistGroups = Enumerable.Empty<IGrouping<string, Playlist>>();
            Configuration = new PlaylistConfiguration();
        }

        public Playlist SelectedPlaylist { get; set; }
        public IEnumerable<IGrouping<string, Playlist>> PlaylistGroups { get; set; }
    }
}