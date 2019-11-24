using MediaLibraryDAL.DbContexts;
using MediaLibraryWebUI.Models.Configurations;
using MediaLibraryWebUI.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MediaLibraryWebUI.Models
{
    [Export]
    public class PlaylistViewModel : ViewModel<PlaylistConfiguration>
    {
        [ImportingConstructor]
        public PlaylistViewModel()
        {
            PlaylistGroups = Enumerable.Empty<IGrouping<string, Playlist>>();
            PlaylistSortItems = PlaylistRepository.GetPlaylistSortItems().Select(item => new SelectListItem { Text = item.Name, Value = item.Value.ToString() });
        }

        public Playlist SelectedPlaylist { get; set; }
        public IEnumerable<IGrouping<string, Playlist>> PlaylistGroups { get; set; }
        public IEnumerable<SelectListItem> PlaylistSortItems { get; }
    }
}