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
            this.mainViewModel.MenuItemCommand = new Command(MenuItemClicked);
            this.mainViewModel.SelectedMenuItem = this.mainViewModel.MenuItems.FirstOrDefault();
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
        }

        public Page GetMainView() => mainViewModel.View;

        private async void MenuItemClicked(object _page)
        {
            Pages page = (Pages)_page;
            NavigationPage target = default;

            this.mainViewModel.SelectedMenuItem = this.mainViewModel.MenuItems.FirstOrDefault(item => item.Key == (Pages)page);
            pages.TryGetValue(page, out target);
            (mainViewModel.View as MasterDetailPage).Detail = target;

            switch(page)
            {
                case Pages.Playlist:
                    await LoadPlaylists();
                    break;
                case Pages.Podcast:
                    await LoadPodcasts();
                    break;
                default:
                    break;
            }
        }

        private async Task LoadPodcasts()
        {
            this.podcastViewModel.Podcasts = await webService.Get<Podcast>(baseUri, "Podcast/GetPodcasts");
        }

        private async Task LoadPlaylists()
        {
            this.playlistViewModel.Playlists = await webService.Get<Playlist>(baseUri, "Playlist/GetPlaylists");
        }
    }
}
