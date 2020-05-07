using MediaLibraryDAL.DbContexts;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.Composition;
using MediaLibraryMobile.Views.Interfaces;
using System.Windows.Input;

namespace MediaLibraryMobile.ViewModels
{
    [Export]
    public class PlaylistViewModel : BaseViewModel<IPlaylistView>
    {
        private readonly IPlaylistView playlistView;
        private IEnumerable<Playlist> playlists;
        private ICommand loadCommand;
        private bool isRefreshing;

        [ImportingConstructor]
        public PlaylistViewModel(IPlaylistView playlistView): base(playlistView)
        {
            this.playlistView = playlistView;
        }

        public IEnumerable<Playlist> Playlists { get => playlists; set => SetProperty<IEnumerable<Playlist>>(ref playlists, value); }
        public ICommand LoadCommand { get => loadCommand; set => SetProperty<ICommand>(ref loadCommand, value); }
        public bool IsRefreshing { get => isRefreshing; set => SetProperty<bool>(ref isRefreshing, value); }
    }
}
