using MediaLibraryDAL.DbContexts;
using MediaLibraryWebUI.Models.Configurations;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using static MediaLibraryWebUI.Enums;

namespace MediaLibraryWebUI.Models
{
    [Export]
    public class PlayerViewModel : ViewModel<PlayerConfiguration>
    {
        [ImportingConstructor]
        public PlayerViewModel() { }

        public Playlist SelectedPlaylist { get; set; }
    }
}