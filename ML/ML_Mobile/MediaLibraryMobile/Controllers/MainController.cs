using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.Composition;
using MediaLibraryMobile.ViewModels;
using Xamarin.Forms;
using MediaLibraryMobile.Repository;
using System.Linq;
using static MediaLibraryMobile.Enums;
using MediaLibraryDAL.DbContexts;
using System.Threading.Tasks;
using System.ComponentModel;
using MediaLibraryMobile.Models;
using static MediaLibraryDAL.Enums;
using Xamarin.Forms.Internals;
using MediaLibraryBLL.Services.Interfaces;
using System.Threading;
using LibVLCSharp.Shared;
using Xamarin.Essentials;
using MediaLibraryMobile.Controllers.Interfaces;

namespace MediaLibraryMobile.Controllers
{
    [Export]
    public class MainController : IController
    {
        private readonly MainViewModel mainViewModel;
        private readonly PlaylistViewModel playlistViewModel;
        private readonly PodcastViewModel podcastViewModel;
        private readonly PlayerViewModel playerViewModel;
        private readonly IDictionary<Pages, NavigationPage> pages;
        private readonly IWebService webService;
        private readonly Uri baseUri;
        public readonly string username,
                               password;
        private readonly double playPreviousPosition = 5;
        private int? nextIndex;

        [ImportingConstructor]
        public MainController(MainViewModel mainViewModel, PlaylistViewModel playlistViewModel, PodcastViewModel podcastViewModel,
                              IWebService webService, PlayerViewModel playerViewModel)
        {
            string baseAddress = string.Empty;

            username = Preferences.Get(nameof(LoginViewModel.Username), default(string), "login");
            password = Preferences.Get(nameof(LoginViewModel.Password), default(string), "login");
            this.mainViewModel = mainViewModel;
            this.playlistViewModel = playlistViewModel;
            this.podcastViewModel = podcastViewModel;
            this.playerViewModel = playerViewModel;
            this.webService = webService;
            this.mainViewModel.MenuItems = MainRepository.GetMenuItems();
            this.mainViewModel.PropertyChanged += MainViewModel_PropertyChanged;
            this.playlistViewModel.PropertyChanged += PlaylistViewModel_PropertyChanged;
            this.podcastViewModel.PropertyChanged += PodcastViewModel_PropertyChanged;
            this.playerViewModel.PropertyChanged += PlayerViewModel_PropertyChanged;
            this.playlistViewModel.LoadPlaylistsCommand = new Command(async refresh => await LoadPlaylists(refresh));
            this.podcastViewModel.LoadPodcastsCommand = new Command(async refresh => await LoadPodcasts(refresh));
            this.podcastViewModel.PlayerCommand = new Command(GoToPlayer);
            this.playlistViewModel.LoadPlaylistCommand = new Command(async id => await LoadPlaylist(id));
            this.podcastViewModel.LoadPodcastCommand = new Command(async id => await LoadPodcast(id));
            this.playlistViewModel.PlayCommand = new Command(PlaylistPlay);
            this.playlistViewModel.PlayerCommand = new Command(GoToPlayer);
            this.podcastViewModel.PlayCommand = new Command(PodcastPlay);
            InitializeMediaPlayer();
#if DEBUG
            baseAddress = Preferences.Get("BASE_URI_DEBUG", default(string));
#else
            baseAddress = Preferences.Get("BASE_URI", default(string));
#endif
            baseUri = new Uri(baseAddress);
            pages = new Dictionary<Pages, NavigationPage>()
            {
                { Pages.Playlist, new NavigationPage(playlistViewModel.View) },
                { Pages.Podcast, new NavigationPage(podcastViewModel.View) }
            };
            this.mainViewModel.SelectedMenuItem = this.mainViewModel.MenuItems.FirstOrDefault();
        }

        private bool CanPlayPrevious() => playerViewModel.SelectedPlayIndex > 0 || playerViewModel.CurrentPosition > playPreviousPosition;
        private bool CanPlayNext() => (playerViewModel.SelectedPlayIndex + 1) < playerViewModel.MediaItems.Count();

        public void Startup()
        {
        }

        public void Shutdown()
        {
            playerViewModel.LibVLC?.Dispose();
            playerViewModel.MediaPlayer?.Dispose();
        }

        private void PlayerViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(PlayerViewModel.MediaItems))
            {
                if (!playerViewModel.IsPlaying) /*then*/ playerViewModel.SelectedPlayIndex = null;
            }
            else if (e.PropertyName == nameof(PlayerViewModel.SelectedPlayIndex))
            {
                if (playerViewModel.SelectedPlayIndex.HasValue)
                {
                    int index = playerViewModel.SelectedPlayIndex.Value;
                    (int Id, string Title, Uri Uri) mediaItem = playerViewModel.MediaItems.ElementAt(index);
                    Media media = new Media(playerViewModel.LibVLC, mediaItem.Uri);

                    playerViewModel.Title = mediaItem.Title;
                    playerViewModel.MediaPlayer.Media?.Dispose();
                    if (playerViewModel.IsPlaying) /*then*/ ThreadPool.QueueUserWorkItem(_ => playerViewModel.MediaPlayer.Play(media));
                }
            }
            else if (e.PropertyName == nameof(PlayerViewModel.IsRandom))
            {
                nextIndex = null;

                if (playerViewModel.IsRandom && playlistViewModel.SelectedPlaylist != null)
                {
                    playerViewModel.MediaItems = GetPlaylistMediaItems(playlistViewModel.SelectedPlaylist, true);
                    nextIndex = 0;
                }
            }
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
                IEnumerable<Podcast> podcasts = await webService.Get<IEnumerable<Podcast>>(baseUri, "Podcast/GetPodcastsJSON", username, password);

                podcastViewModel.Podcasts.Clear();
                foreach (var podcast in podcasts) { podcastViewModel.Podcasts.Add(podcast); }
                podcastViewModel.IsRefreshing = false;
            }
        }

        private async Task LoadPlaylists(object refresh)
        {
            if ((bool)refresh)
            {
                playlistViewModel.Playlists = await webService.Get<IEnumerable<Playlist>>(baseUri, "Playlist/GetPlaylistsJSON", username, password);
                playlistViewModel.IsRefreshing = false;
            }
        }

        private async Task LoadPodcast(object id)
        {
            podcastViewModel.SelectedPodcast = await webService.Get<Podcast>(baseUri, $"Podcast/GetPodcastJSON/{id}", username, password);

            podcastViewModel.SelectedPodcastItems.Clear();
            foreach (var item in podcastViewModel.SelectedPodcast.PodcastItems) { podcastViewModel.SelectedPodcastItems.Add(item); }
            await podcastViewModel.View.Navigation.PushAsync(podcastViewModel.PodcastView as ContentPage);
        }

        private async Task LoadPlaylist(object id)
        {
            playlistViewModel.SelectedPlaylist = await webService.Get<Playlist>(baseUri, $"Playlist/GetPlaylistJSON/{id}", username, password);
            await playlistViewModel.View.Navigation.PushAsync(playlistViewModel.PlaylistView as ContentPage);
        }

        private void InitializeMediaPlayer()
        {
            playerViewModel.NextCommand = new Command(Next);
            playerViewModel.PreviousCommand = new Command(Previous);
            playerViewModel.RandomCommand = new Command(ToggleRandom);
            playerViewModel.MediaPlayer.Paused += (sender, args) => playerViewModel.IsPlaying = false;
            playerViewModel.MediaPlayer.Playing += (sender, args) => playerViewModel.IsPlaying = true;
            playerViewModel.MediaPlayer.Stopped += (sender, args) => playerViewModel.IsPlaying = false;
            playerViewModel.MediaPlayer.EndReached += EndReached;
        }

        private async void EndReached(object sender, EventArgs args)
        {
            Playlist playlist = playlistViewModel.SelectedPlaylist;
            var data = new { mediaType = playlist.Type, id = GetPlaylistItemId(playlist, playerViewModel.SelectedPlayIndex.Value) };
            Task updateTask = webService.PostJSON(baseUri, "Player/UpdatePlayCount", data, username, password);
            
            Next();
            try { await updateTask; } catch (Exception ex) { };
        }

        private void Next()
        {
            int lastIndex = playerViewModel.MediaItems.Count() - 1;

            if (nextIndex.HasValue)
            {
                playerViewModel.SelectedPlayIndex = 0;
                nextIndex = null;
            }
            else if (playerViewModel.SelectedPlayIndex < lastIndex) /*then*/ playerViewModel.SelectedPlayIndex++;
        }

        private void Previous()
        {
            if (nextIndex.HasValue)
            {
                playerViewModel.SelectedPlayIndex = 0;
                nextIndex = null;
            }
            else if (playerViewModel.CurrentPosition > playPreviousPosition) /*then*/ playerViewModel.MediaPlayer.Position = 0;
            else if (playerViewModel.SelectedPlayIndex > 0) /*then*/ playerViewModel.SelectedPlayIndex--;
        }

        private void ToggleRandom() => playerViewModel.IsRandom = !playerViewModel.IsRandom;
        
        private void PlaylistPlay(object item)
        {
            Playlist playlist = playlistViewModel.SelectedPlaylist;
            int playIndex = GetPlaylistItemIndex(playlist, item),
                itemId = GetPlaylistItemId(playlist, playIndex);

            playerViewModel.MediaItems = GetPlaylistMediaItems(playlist, playerViewModel.IsRandom);
            playerViewModel.IsPlaying = true;
            playIndex = playerViewModel.IsRandom ? playerViewModel.MediaItems.IndexOf(_item => _item.Id == itemId) : playIndex;
            playerViewModel.SelectedPlayIndex = playIndex;
            playerViewModel.Title = playerViewModel.MediaItems.ElementAt(playIndex).Title;
            nextIndex = playerViewModel.IsRandom ? new int?(0) : null;
            GoToPlayer();
        }

        private void PodcastPlay(object item)
        {

        }

        private IEnumerable<(int, string, Uri)> GetPlaylistMediaItems(Playlist playlist, bool random = default)
        {
            IEnumerable<(int, string, Uri)> mediaItems = Enumerable.Empty<(int, string, Uri)>();
            PlaylistTypes playlistType = (PlaylistTypes)playlistViewModel.SelectedPlaylist.Type;
            string controller = playlistType.ToString();

            switch (playlistType)
            {
                case PlaylistTypes.Music:
                    mediaItems = playlist.PlaylistTracks.Select(item => item.Track)
                                                        .Select(track => (track.Id, track.Title, new Uri(baseUri, $"{controller}/File/{track.Id}")));
                    break;
                case PlaylistTypes.Podcast:
                    mediaItems = playlist.PlaylistPodcastItems.Select(item => item.PodcastItem)
                                                              .Select(item => (item.Id, item.Title, new Uri(baseUri, $"{controller}/File/{item.Id}")));
                    break;
                case PlaylistTypes.Television:
                    mediaItems = playlist.PlaylistEpisodes.Select(item => item.Episode)
                                                          .Select(episode => (episode.Id, episode.Title, new Uri(baseUri, $"{controller}/File/{episode.Id}")));
                    break;
                default:
                    break;
            }

            return (random ? mediaItems.OrderBy(item => Guid.NewGuid()) : mediaItems).ToList();
        }

        private int GetPlaylistItemIndex(Playlist playlist, object item)
        {
            int id;
            PlaylistTypes playlistType = (PlaylistTypes)playlistViewModel.SelectedPlaylist.Type;

            switch (playlistType)
            {
                case PlaylistTypes.Music:
                    id = playlist.PlaylistTracks.Select(_item => _item.Track).IndexOf(item);
                    break;
                case PlaylistTypes.Podcast:
                    id = playlist.PlaylistPodcastItems.Select(_item => _item.PodcastItem).IndexOf(item);
                    break;
                case PlaylistTypes.Television:
                    id = playlist.PlaylistEpisodes.Select(_item => _item.Episode).IndexOf(item);
                    break;
                default:
                    id = default;
                    break;
            }

            return id;
        }

        private int GetPlaylistItemId(Playlist playlist, int index)
        {
            int id;
            PlaylistTypes playlistType = (PlaylistTypes)playlistViewModel.SelectedPlaylist.Type;

            switch (playlistType)
            {
                case PlaylistTypes.Music:
                    id = playlist.PlaylistTracks.Select(_item => _item.Track).ElementAt(index).Id;
                    break;
                case PlaylistTypes.Podcast:
                    id = playlist.PlaylistPodcastItems.Select(_item => _item.PodcastItem).ElementAt(index).Id;
                    break;
                case PlaylistTypes.Television:
                    id = playlist.PlaylistEpisodes.Select(_item => _item.Episode).ElementAt(index).Id;
                    break;
                default:
                    id = -1;
                    break;
            }

            return id;
        }

        private async void GoToPlayer()
        {
            pages.TryGetValue(mainViewModel.SelectedMenuItem.Key, out NavigationPage target);
            await target.Navigation.PushAsync(playerViewModel.View as ContentPage);
        }
    }
}
