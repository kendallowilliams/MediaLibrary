using MediaLibraryDAL.DbContexts;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.Composition;
using MediaLibraryMobile.Views.Interfaces;
using System.Windows.Input;
using System.Linq;
using MediaLibraryMobile.Models;

namespace MediaLibraryMobile.ViewModels
{
    [Export]
    public class PlaylistViewModel : BaseViewModel<IPlaylistsView>
    {
        private readonly IPlaylistView playlistView;
        private IEnumerable<Playlist> playlists;
        private IEnumerable<PlaylistGroup> playlistGroups;
        private ICommand loadPlaylistsCommand,
                         loadPlaylistCommand,
                         playCommand,
                         playerCommand;
        private bool isRefreshing,
                     selectedPlaylistHasTracks,
                     selectedPlaylistHasEpisodes,
                     selectedPlaylistHasPodcastItems;
        private Playlist selectedPlaylist;

        [ImportingConstructor]
        public PlaylistViewModel(IPlaylistsView playlistsView, IPlaylistView playlistView): base(playlistsView)
        {
            this.playlistView = playlistView;
            this.playlistView.BindingContext = this;
            playlists = Enumerable.Empty<Playlist>();
        }

        public IEnumerable<Playlist> Playlists { get => playlists; set => SetProperty<IEnumerable<Playlist>>(ref playlists, value); }
        public IEnumerable<PlaylistGroup> PlaylistGroups { get => playlistGroups; set => SetProperty<IEnumerable<PlaylistGroup>>(ref playlistGroups, value); }
        public ICommand LoadPlaylistsCommand { get => loadPlaylistsCommand; set => SetProperty<ICommand>(ref loadPlaylistsCommand, value); }
        public ICommand LoadPlaylistCommand { get => loadPlaylistCommand; set => SetProperty<ICommand>(ref loadPlaylistCommand, value); }
        public ICommand PlayCommand { get => playCommand; set => SetProperty<ICommand>(ref playCommand, value); }
        public ICommand PlayerCommand { get => playerCommand; set => SetProperty<ICommand>(ref playerCommand, value); }
        public bool IsRefreshing { get => isRefreshing; set => SetProperty<bool>(ref isRefreshing, value); }
        public bool SelectedPlaylistHasTracks { get => selectedPlaylistHasTracks; set => SetProperty<bool>(ref selectedPlaylistHasTracks, value); }
        public bool SelectedPlaylistHasEpisodes { get => selectedPlaylistHasEpisodes; set => SetProperty<bool>(ref selectedPlaylistHasEpisodes, value); }
        public bool SelectedPlaylistHasPodcastItems { get => selectedPlaylistHasPodcastItems; set => SetProperty<bool>(ref selectedPlaylistHasPodcastItems, value); }
        public Playlist SelectedPlaylist { get => selectedPlaylist; set => SetProperty<Playlist>(ref selectedPlaylist, value); }
        public IPlaylistView PlaylistView { get => playlistView; }
    }
}
