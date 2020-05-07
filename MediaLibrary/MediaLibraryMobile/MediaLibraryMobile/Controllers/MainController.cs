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

        [ImportingConstructor]
        public MainController(MainViewModel mainViewModel, PlaylistViewModel playlistViewModel, PodcastViewModel podcastViewModel,
                              IWebService webService)
        {
            this.mainViewModel = mainViewModel;
            this.playlistViewModel = playlistViewModel;
            this.podcastViewModel = podcastViewModel;
            this.webService = webService;
            this.mainViewModel.MenuItems = MainRepository.GetMenuItems();
            this.mainViewModel.MenuItemCommand = new Command(MenuItemClicked);
            this.mainViewModel.SelectedMenuItem = this.mainViewModel.MenuItems.FirstOrDefault();

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

            //await LoadPodcasts();
        }

        private async Task LoadPodcasts()
        {
            this.podcastViewModel.Podcasts = await webService.Get<Podcast>("https://10.0.2.2:44373", "Podcast/GetPodcasts");
        }
    }
}
