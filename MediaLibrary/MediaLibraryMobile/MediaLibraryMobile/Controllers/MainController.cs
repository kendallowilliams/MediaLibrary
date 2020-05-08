﻿using System;
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

namespace MediaLibraryMobile.Controllers
{
    [Export]
    public class MainController
    {
        private readonly MainViewModel mainViewModel;
        private readonly PlaylistViewModel playlistViewModel;
        private readonly PodcastViewModel podcastViewModel;
        private IDictionary<Pages, NavigationPage> pages;
        private readonly IWebService webService;
        private Uri baseUri;
        private readonly ISharedPreferencesService sharedPreferencesService;

        [ImportingConstructor]
        public MainController(MainViewModel mainViewModel, PlaylistViewModel playlistViewModel, PodcastViewModel podcastViewModel,
                              IWebService webService, ISharedPreferencesService sharedPreferencesService)
        {
            string baseAddress = string.Empty;

            this.mainViewModel = mainViewModel;
            this.playlistViewModel = playlistViewModel;
            this.podcastViewModel = podcastViewModel;
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
#if DEBUG
            if (string.IsNullOrWhiteSpace(baseAddress = this.sharedPreferencesService.GetString("BASE_URI_DEBUG")))
            {
                this.sharedPreferencesService.SetString("BASE_URI_DEBUG", baseAddress = "http://kserver/MediaLibraryDEV/");
            }
#else
            if (string.IsNullOrWhiteSpace(baseAddress = this.sharedPreferencesService.GetString("BASE_URI")))
            {
                this.sharedPreferencesService.SetString("BASE_URI", baseAddress = "http://kserver/MediaLibrary/");
            }
#endif
            baseUri = new Uri(baseAddress);
            pages = new Dictionary<Pages, NavigationPage>()
            {
                { Pages.Playlist, new NavigationPage(playlistViewModel.View) },
                { Pages.Podcast, new NavigationPage(podcastViewModel.View) }
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
            }
        }

        private async void MainViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MainViewModel.SelectedMenuItem))
            {
                switch (mainViewModel.SelectedMenuItem.Key)
                {
                    case Pages.Playlist:
                        await LoadPlaylists(!playlistViewModel.Playlists.Any());
                        break;
                    case Pages.Podcast:
                        await LoadPodcasts(!podcastViewModel.Podcasts.Any());
                        break;
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
                this.podcastViewModel.IsRefreshing = true;
                this.podcastViewModel.Podcasts = await webService.Get<Podcast>(baseUri, "Podcast/GetPodcasts");
                this.podcastViewModel.IsRefreshing = false;
            }
        }

        private async Task LoadPlaylists(object refresh)
        {
            if ((bool)refresh)
            {
                this.playlistViewModel.IsRefreshing = true;
                this.playlistViewModel.Playlists = await webService.Get<Playlist>(baseUri, "Playlist/GetPlaylists");
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
    }
}
