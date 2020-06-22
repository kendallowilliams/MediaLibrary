using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using MediaLibraryBLL.Services.Interfaces;
using MediaLibraryBLL.Services;
using MediaLibraryDAL.DbContexts;
using System.Threading;
using System.Collections.ObjectModel;
using MediaLibraryDAL;
using Tizen.Content.Download;
using Tizen.System;
using System.IO;

namespace MediaLibraryWearable.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlaylistsPage : ContentPage
    {
        private readonly IWebService webService;
        private readonly Uri baseUri;

        public PlaylistsPage()
        {
            string baseAddress = string.Empty;

            InitializeComponent();
            webService = new WebService();
#if DEBUG
            baseAddress = "http://kserver/MediaLibraryDEV/";
#else
            baseAddress = "https://media.kowmylk.com/";
#endif
            baseUri = new Uri(baseAddress);

            Init();
        }

        public async void Init()
        {
            IEnumerable<Playlist> playlists = await webService.Get<IEnumerable<Playlist>>(baseUri, "Playlist/GetPlaylistsJSON");

            lstPlaylists.ItemsSource = new ObservableCollection<Playlist>(playlists.Where(playlist => playlist.Type == (int)Enums.PlaylistTypes.Music));
        }

        private void lstPlaylists_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Playlist playlist = e.Item as Playlist;
            Storage storage = StorageManager.Storages.FirstOrDefault(item => item.StorageType == StorageArea.Internal);

            foreach(var track in playlist.PlaylistTracks)
            {
                string url = $"{baseUri.OriginalString}Music/File/{track.TrackId}",
                       destPath = storage.GetAbsolutePath(DirectoryType.Music),
                       authentication = "",
                       fileName = $"{track.TrackId}.mp3";
                IDictionary<string, string> headers = new Dictionary<string, string>()
                {
                    { "Authorization", $"Basic {authentication}" }
                };
                Request request = new Request(url);

                request.FileName = fileName;
                request.Start();
                
            }
        }
    }
}