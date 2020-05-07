using MediaLibraryDAL.DbContexts;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.Composition;
using MediaLibraryMobile.Views.Interfaces;

namespace MediaLibraryMobile.ViewModels
{
    [Export]
    public class PlaylistViewModel : BaseViewModel<IPlaylistView>
    {
        private readonly IPlaylistView playlistView;
        private IEnumerable<Playlist> playlists;

        [ImportingConstructor]
        public PlaylistViewModel(IPlaylistView playlistView): base(playlistView)
        {
            this.playlistView = playlistView;
        }

        public IEnumerable<Playlist> Playlists { get => playlists; set => SetProperty<IEnumerable<Playlist>>(ref playlists, value); }
    }
}
