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
    public class PlaylistViewModel : BaseViewModel<IPlaylistsView>
    {
        private readonly IPlaylistsView playlistsView;
        private readonly IPlaylistView playlistView;
        private IEnumerable<Playlist> playlists;
        private ICommand loadPlaylistsCommand;
        private ICommand loadPlaylistCommand;
        private bool isRefreshing;
        private Playlist selectedPlaylist;

        [ImportingConstructor]
        public PlaylistViewModel(IPlaylistsView playlistsView, IPlaylistView playlistView): base(playlistsView)
        {
            this.playlistsView = playlistsView;
            this.playlistView = playlistView;
            this.playlistView.BindingContext = this;
        }

        public IEnumerable<Playlist> Playlists { get => playlists; set => SetProperty<IEnumerable<Playlist>>(ref playlists, value); }
        public ICommand LoadPlaylistsCommand { get => loadPlaylistsCommand; set => SetProperty<ICommand>(ref loadPlaylistsCommand, value); }
        public ICommand LoadPlaylistCommand { get => loadPlaylistCommand; set => SetProperty<ICommand>(ref loadPlaylistCommand, value); }
        public bool IsRefreshing { get => isRefreshing; set => SetProperty<bool>(ref isRefreshing, value); }
        public Playlist SelectedPlaylist { get => selectedPlaylist; set => SetProperty<Playlist>(ref selectedPlaylist, value); }
        public IPlaylistView PlaylistView { get => playlistView; }
    }
}
