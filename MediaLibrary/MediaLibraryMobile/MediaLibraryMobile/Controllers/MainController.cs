using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.Composition;
using MediaLibraryMobile.ViewModels;
using MediaLibraryMobile.Views.Interfaces;
using Xamarin.Forms;
using MediaLibraryMobile.Repository;
using System.Linq;
using static MediaLibraryMobile.Enums;
using MediaLibraryMobile.Services.Interfaces;
using MediaLibraryDAL.DbContexts;
using System.Threading.Tasks;
using System.ComponentModel;
using Android.Util;
using MediaLibraryMobile.Models;
using static MediaLibraryDAL.Enums;
using LibVLCSharp.Shared;

namespace MediaLibraryMobile.Controllers
{
    [Export]
    public class MainController
    {
        private readonly MainViewModel mainViewModel;
        private readonly PlaylistViewModel playlistViewModel;
        private readonly PodcastViewModel podcastViewModel;
        private readonly PlayerViewModel playerViewModel;
        private IDictionary<Pages, NavigationPage> pages;
        private readonly IWebService webService;
        private Uri baseUri;
        private readonly ISharedPreferencesService sharedPreferencesService;

        [ImportingConstructor]
        public MainController(MainViewModel mainViewModel, PlaylistViewModel playlistViewModel, PodcastViewModel podcastViewModel,
                              IWebService webService, ISharedPreferencesService sharedPreferencesService, PlayerViewModel playerViewModel)
        {
            string baseAddress = string.Empty;

            this.mainViewModel = mainViewModel;
            this.playlistViewModel = playlistViewModel;
            this.podcastViewModel = podcastViewModel;
            this.playerViewModel = playerViewModel;
            this.webService = webService;
            this.sharedPreferencesService = sharedPreferencesService;
            this.mainViewModel.MenuItems = MainRepository.GetMenuItems();
            this.mainViewModel.PropertyChanged += MainViewModel_PropertyChanged;
            this.playlistViewModel.PropertyChanged += PlaylistViewModel_PropertyChanged;
            this.podcastViewModel.PropertyChanged += PodcastViewModel_PropertyChanged;
            this.playlistViewModel.LoadPlaylistsCommand = new Command(async refresh => await LoadPlaylists(refresh));
            this.podcastViewModel.LoadPodcastsCommand = new Command(async refresh => await LoadPodcasts(refresh));
            this.playlistViewModel.LoadPlaylistCommand = new Command(async id => await LoadPlaylist(id));
            this.podcastViewModel.LoadPodcastCommand = new Command(async id => await LoadPodcast(id));
            this.playlistViewModel.PlayCommand = new Command(Play);
#if DEBUG
            baseAddress = this.sharedPreferencesService.GetString("BASE_URI_DEBUG");
#else
            baseAddress = this.sharedPreferencesService.GetString("BASE_URI");
#endif
            baseUri = new Uri(baseAddress);
            pages = new Dictionary<Pages, NavigationPage>()
            {
                { Pages.Playlist, new NavigationPage(playlistViewModel.View) },
                { Pages.Podcast, new NavigationPage(podcastViewModel.View) },
                { Pages.Player, new NavigationPage(playerViewModel.View) }
            };
            this.mainViewModel.SelectedMenuItem = this.mainViewModel.MenuItems.FirstOrDefault();
        }

        private void PodcastViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(PodcastViewModel.SelectedPodcast))
            {
            }
        }

        private void PlaylistViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(PlaylistViewModel.SelectedPlaylist))
            {
                this.playlistViewModel.SelectedPlaylistHasEpisodes = this.playlistViewModel.SelectedPlaylist.PlaylistEpisodes.Any();
                this.playlistViewModel.SelectedPlaylistHasTracks = this.playlistViewModel.SelectedPlaylist.PlaylistTracks.Any();
                this.playlistViewModel.SelectedPlaylistHasPodcastItems = this.playlistViewModel.SelectedPlaylist.PlaylistPodcastItems.Any();
            }
            else if (e.PropertyName == nameof(PlaylistViewModel.Playlists))
            {
                this.playlistViewModel.PlaylistGroups = this.playlistViewModel.Playlists.GroupBy(item => ((PlaylistTypes)item.Type).ToString())
                                                                                        .Select(group => new PlaylistGroup(group.Key, group));
            }
        }

        private void MainViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MainViewModel.SelectedMenuItem))
            {
                switch (mainViewModel.SelectedMenuItem.Key)
                {
                    case Pages.Playlist:
                        if (!playlistViewModel.Playlists.Any()) /*then*/ this.playlistViewModel.IsRefreshing = true;
                        break;
                    case Pages.Podcast:
                        if (!podcastViewModel.Podcasts.Any()) /*then*/ this.podcastViewModel.IsRefreshing = true;
                        break;
                    case Pages.Player:
                    default:
                        break;
                }

                pages.TryGetValue(mainViewModel.SelectedMenuItem.Key, out NavigationPage target);
                (mainViewModel.View as MasterDetailPage).Detail = target;
                mainViewModel.IsPresented = false;
            }
        }

        public Page GetMainView() => mainViewModel.View;

        private async Task LoadPodcasts(object refresh)
        {
            if ((bool)refresh)
            {
                this.podcastViewModel.Podcasts = await webService.Get<Podcast>(baseUri, "Podcast/GetPodcastsJSON");
                this.podcastViewModel.IsRefreshing = false;
            }
        }

        private async Task LoadPlaylists(object refresh)
        {
            if ((bool)refresh)
            {
                this.playlistViewModel.Playlists = await webService.Get<Playlist>(baseUri, "Playlist/GetPlaylistsJSON");
                this.playlistViewModel.IsRefreshing = false;
            }
        }

        private async Task LoadPodcast(object id)
        {
            this.podcastViewModel.SelectedPodcast = this.podcastViewModel.Podcasts.FirstOrDefault(item => item.Id == (int)id);
            await podcastViewModel.View.Navigation.PushAsync(podcastViewModel.PodcastView as ContentPage);
        }

        private async Task LoadPlaylist(object id)
        {
            this.playlistViewModel.SelectedPlaylist = this.playlistViewModel.Playlists.FirstOrDefault(item => item.Id == (int)id);
            await playlistViewModel.View.Navigation.PushAsync(playlistViewModel.PlaylistView as ContentPage);
        }

        private void Play(object id)
        {
            string controller = ((PlaylistTypes)this.playlistViewModel.SelectedPlaylist.Type).ToString();
            Media media = new Media(this.playerViewModel.LibVLC, new Uri(baseUri, $"{controller}/File/{id}"));

            this.mainViewModel.SelectedMenuItem = this.mainViewModel.MenuItems.FirstOrDefault(item => item.Key == Pages.Player);
            this.playerViewModel.MediaPlayer.Play(media);
            
        }
    }
}
