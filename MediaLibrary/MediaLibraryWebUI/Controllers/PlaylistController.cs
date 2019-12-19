using MediaLibraryBLL.Models;
using MediaLibraryBLL.Services.Interfaces;
using MediaLibraryDAL.DbContexts;
using MediaLibraryDAL.Services.Interfaces;
using MediaLibraryWebUI.Models;
using MediaLibraryWebUI.Models.Configurations;
using MediaLibraryWebUI.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static MediaLibraryWebUI.Enums;

namespace MediaLibraryWebUI.Controllers
{
    [Export("Playlist", typeof(IController)), PartCreationPolicy(CreationPolicy.NonShared)]
    public class PlaylistController : BaseController
    {
        private readonly IPlaylistUIService playlistService;
        private readonly IDataService dataService;
        private readonly PlaylistViewModel playlistViewModel;
        private readonly ITransactionService transactionService;

        [ImportingConstructor]
        public PlaylistController(IPlaylistUIService playlistService, IDataService dataService, PlaylistViewModel playlistViewModel,
                                  ITransactionService transactionService)
        {
            this.playlistService = playlistService;
            this.dataService = dataService;
            this.playlistViewModel = playlistViewModel;
            this.transactionService = transactionService;
        }

        public async Task<ActionResult> Index()
        {
            ActionResult result = null;
            Configuration configuration = await dataService.GetAsync<Configuration>(item => item.Type == nameof(MediaPages.Playlist));

            if (configuration != null)
            {
                playlistViewModel.Configuration = JsonConvert.DeserializeObject<PlaylistConfiguration>(configuration.JsonData);
            }

            if (playlistViewModel.Configuration.SelectedPlaylistPage == PlaylistPages.Playlist)
            {
                result = await Get(playlistViewModel.Configuration.SelectedPlaylistId);
            }
            else
            {
                playlistViewModel.PlaylistGroups = await playlistService.GetPlaylistGroups(playlistViewModel.Configuration.SelectedPlaylistSort);
                result = PartialView(playlistViewModel);
            }

            return result;
        }

        public async Task AddPlaylist(string playlistName)
        {
            if (!string.IsNullOrWhiteSpace(playlistName))
            {
                Playlist playlist = new Playlist(playlistName);
                Configuration configuration = await dataService.GetAsync<Configuration>(item => item.Type == nameof(MediaPages.Playlist));

                await dataService.Insert(playlist);

                if (configuration != null)
                {
                    playlistViewModel.Configuration = JsonConvert.DeserializeObject<PlaylistConfiguration>(configuration.JsonData);
                    playlistViewModel.Configuration.SelectedPlaylistId = playlist.Id;
                    playlistViewModel.Configuration.SelectedPlaylistPage = PlaylistPages.Playlist;
                    configuration.JsonData = JsonConvert.SerializeObject(playlistViewModel.Configuration);
                    await dataService.Update(configuration);
                }
            }
        }

        public async Task RemovePlaylist(int id)
        {
            Configuration configuration = await dataService.GetAsync<Configuration>(item => item.Type == nameof(MediaPages.Playlist));
            
            await dataService.Delete<Playlist>(id);

            if (configuration != null)
            {
                PlaylistConfiguration playlistConfiguration = JsonConvert.DeserializeObject<PlaylistConfiguration>(configuration.JsonData);

                playlistConfiguration.SelectedPlaylistPage = PlaylistPages.Index;
                configuration.JsonData = JsonConvert.SerializeObject(playlistConfiguration);
                await dataService.Update(configuration);
            }
        }

        public async Task EditPlaylist(int id, string name)
        {
            Playlist playlist = await dataService.GetAsync<Playlist>(item => item.Id == id);

            playlist.Name = name;
            await dataService.Update(playlist);
        }

        private async Task<ActionResult> Get(int id)
        {
            playlistViewModel.SelectedPlaylist = await dataService.GetAsync<Playlist, IEnumerable<Track>>(item => item.Id == id, 
                                                                                                          playlist => playlist.PlaylistTracks.Select(list => list.Track));

            return PartialView("Playlist", playlistViewModel);
        }

        public async Task RemovePlaylistItem(int id, int playlistId)
        {
            await dataService.Delete<PlaylistTrack>(id);
        }

        [AllowAnonymous]
        public async Task<ActionResult> GetM3UPlaylist(int id, bool random = false)
        {
            Random rand = new Random(DateTime.Now.Millisecond);
            Playlist playlist = await dataService.GetAsync<Playlist, IEnumerable<Track>>(list => list.Id == id, 
                                                                                                 list => list.PlaylistTracks.Select(item => item.Track));
            IEnumerable<PlaylistTrack> playlistTracks = random ? playlist.PlaylistTracks.OrderBy(item => rand.Next()) :
                                                                 playlist.PlaylistTracks.OrderBy(item => item.CreateDate);
            IEnumerable <Track> tracks = playlistTracks.Select(list => list.Track);
            string domain = Request.Url.GetLeftPart(UriPartial.Authority);
            IEnumerable<string> lines = tracks.Select(track => $"#EXTINF:{(int)track.Duration},{track.Title}{Environment.NewLine}{$"{domain}/Music/File/{track.Id}"}");
            string data = $"#EXTM3U{Environment.NewLine}{string.Join(Environment.NewLine, lines)}";
            byte[] content = Encoding.UTF8.GetBytes(data);

            return new FileContentResult(content, "audio/mpegurl");
        }

        public async Task UpdateConfiguration(PlaylistConfiguration playlistConfiguration)
        {
            if (ModelState.IsValid)
            {
                Configuration configuration = await dataService.GetAsync<Configuration>(item => item.Type == nameof(MediaPages.Playlist));

                if (configuration == null)
                {
                    configuration = new Configuration() { Type = nameof(MediaPages.Playlist), JsonData = JsonConvert.SerializeObject(playlistConfiguration) };
                    await dataService.Insert(configuration);
                }
                else
                {
                    configuration.JsonData = JsonConvert.SerializeObject(playlistConfiguration);
                    await dataService.Update(configuration);
                }
            }
        }
    }
}