using MediaLibraryBLL.Models;
using MediaLibraryBLL.Services.Interfaces;
using MediaLibraryDAL.DbContexts;
using MediaLibraryDAL.Services.Interfaces;
using MediaLibraryWebUI.Attributes;
using MediaLibraryWebUI.Models;
using MediaLibraryWebUI.Models.Configurations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static MediaLibraryWebUI.Enums;
using static System.Environment;
using IO_File = System.IO.File;

namespace MediaLibraryWebUI.Controllers
{
    [Export(nameof(MediaPages.Player), typeof(IController)), PartCreationPolicy(CreationPolicy.NonShared)]
    public class PlayerController : BaseController
    {
        private readonly ITransactionService transactionService;
        private readonly PlayerViewModel playerViewModel;
        private readonly IDataService dataService;
        private readonly IFileService fileService;
        private readonly string fileNamePrefix;

        [ImportingConstructor]
        public PlayerController(ITransactionService transactionService, PlayerViewModel playerViewModel, IDataService dataService,
                                IFileService fileService)
        {
            this.transactionService = transactionService;
            this.playerViewModel = playerViewModel;
            this.dataService = dataService;
            this.fileService = fileService;
#if DEV
            fileNamePrefix = $"{nameof(PlayerController)}_{nameof(UpdateNowPlaying)}_DEV";
#elif DEBUG
            fileNamePrefix = $"{nameof(PlayerController)}_{nameof(UpdateNowPlaying)}_DEBUG";
#else
            fileNamePrefix = $"{nameof(PlayerController)}_{nameof(UpdateNowPlaying)}";
#endif
        }

        [CompressContent]
        public async Task<ActionResult> Index()
        {
            Configuration configuration = await dataService.Get<Configuration>(item => item.Type == nameof(MediaPages.Player));

            if (configuration != null)
            {
                playerViewModel.Configuration = JsonConvert.DeserializeObject<PlayerConfiguration>(configuration.JsonData) ?? new PlayerConfiguration();
            }

            await LoadPlayerViewModel();

            return PartialView(playerViewModel);
        }

        public async Task UpdateConfiguration(PlayerConfiguration playerConfiguration)
        {
            if (ModelState.IsValid)
            {
                Configuration configuration = await dataService.Get<Configuration>(item => item.Type == nameof(MediaPages.Player));

                if (configuration == null)
                {
                    configuration = new Configuration() { Type = nameof(MediaPages.Player), JsonData = JsonConvert.SerializeObject(playerConfiguration) };
                    await dataService.Insert(configuration);
                }
                else
                {
                    configuration.JsonData = JsonConvert.SerializeObject(playerConfiguration);
                    await dataService.Update(configuration);
                }
            }
        }

        [CompressContent]
        public async Task<ActionResult> GetPlayerItems()
        {
            Configuration configuration = await dataService.Get<Configuration>(item => item.Type == nameof(MediaPages.Player));

            if (configuration != null)
            {
                playerViewModel.Configuration = JsonConvert.DeserializeObject<PlayerConfiguration>(configuration.JsonData) ?? new PlayerConfiguration();
            }

            await LoadPlayerViewModel();

            return PartialView("~/Views/Player/PlayerItems.cshtml", playerViewModel);
        }

        private async Task LoadPlayerViewModel()
        {
            IEnumerable<int> ids = Enumerable.Empty<int>();

            if (playerViewModel.Configuration.SelectedMediaType == MediaTypes.Song)
            {
                string path = Path.Combine(fileService.RootFolder, $"{fileNamePrefix}_{nameof(MediaTypes.Song)}.json");
                IEnumerable<ListItem<int, int>> items = Enumerable.Empty<ListItem<int, int>>();
                IEnumerable<Track> songs = Enumerable.Empty<Track>();

                if (IO_File.Exists(path)) /*then*/ items = JsonConvert.DeserializeObject<IEnumerable<ListItem<int, int>>>(IO_File.ReadAllText(path));
                ids = items.Select(item => item.Value);
                songs = await dataService.GetList<Track>(item => ids.Contains(item.Id),default, item => item.Album, item => item.Artist);
                playerViewModel.Songs = ids.Select(id => songs.FirstOrDefault(item => item.Id == id)).Where(item => item != null);
            }
            else if (playerViewModel.Configuration.SelectedMediaType == MediaTypes.Podcast)
            {
                string path = Path.Combine(fileService.RootFolder, $"{fileNamePrefix}_{nameof(MediaTypes.Podcast)}.json");
                IEnumerable<ListItem<int, int>> items = Enumerable.Empty<ListItem<int, int>>();
                IEnumerable<PodcastItem> podcastItems = Enumerable.Empty<PodcastItem>();

                if (IO_File.Exists(path)) /*then*/ items = JsonConvert.DeserializeObject<IEnumerable<ListItem<int, int>>>(IO_File.ReadAllText(path));
                ids = items.Select(item => item.Value);
                podcastItems = await dataService.GetList<PodcastItem>(item => ids.Contains(item.Id), default, item => item.Podcast);
                playerViewModel.PodcastItems = ids.Select(id => podcastItems.FirstOrDefault(item => item.Id == id)).Where(item => item != null);
            }
            else if (playerViewModel.Configuration.SelectedMediaType == MediaTypes.Television)
            {
                string path = Path.Combine(fileService.RootFolder, $"{fileNamePrefix}_{nameof(MediaTypes.Television)}.json");
                IEnumerable<ListItem<int, int>> items = Enumerable.Empty<ListItem<int, int>>();
                IEnumerable<Episode> episodes = Enumerable.Empty<Episode>();

                if (IO_File.Exists(path)) /*then*/ items = JsonConvert.DeserializeObject<IEnumerable<ListItem<int, int>>>(IO_File.ReadAllText(path));
                ids = items.Select(item => item.Value);
                episodes = await dataService.GetList<Episode>(item => ids.Contains(item.Id), default, item => item.Series);
                playerViewModel.Episodes = ids.Select(id => episodes.FirstOrDefault(item => item.Id == id)).Where(item => item != null);
            }
        }

        public async Task UpdateNowPlaying(string itemsJSON, MediaTypes mediaType)
        {
            var items = JsonConvert.DeserializeObject<IEnumerable<ListItem<int, int>>>(itemsJSON);
            Configuration configuration = await dataService.Get<Configuration>(item => item.Type == nameof(MediaPages.Player));
            PlayerConfiguration playerConfiguration = new PlayerConfiguration();

            if (configuration == null)
            {
                configuration = new Configuration() { Type = nameof(MediaPages.Player), JsonData = JsonConvert.SerializeObject(playerConfiguration) };
                await dataService.Insert(configuration);
            }
            else
            {
                playerConfiguration = JsonConvert.DeserializeObject<PlayerConfiguration>(configuration.JsonData) ?? new PlayerConfiguration();
            }

            playerConfiguration.CurrentItemIndex = items.FirstOrDefault((item) => item.IsSelected).Id;
            playerConfiguration.SelectedMediaType = mediaType;
            configuration.JsonData = JsonConvert.SerializeObject(playerConfiguration);
            await dataService.Update(configuration);

            if (items != null)
            {
                string data = JsonConvert.SerializeObject(items);

                if (!Directory.Exists(fileService.RootFolder)) /*then*/ Directory.CreateDirectory(fileService.RootFolder);

                if (mediaType == MediaTypes.Song)
                {
                    IO_File.WriteAllText(Path.Combine(fileService.RootFolder, $"{fileNamePrefix}_{nameof(MediaTypes.Song)}.json"), data);
                }
                else if (mediaType == MediaTypes.Podcast)
                {
                    IO_File.WriteAllText(Path.Combine(fileService.RootFolder, $"{fileNamePrefix}_{nameof(MediaTypes.Podcast)}.json"), data);
                }
                else if (mediaType == MediaTypes.Television)
                {
                    IO_File.WriteAllText(Path.Combine(fileService.RootFolder, $"{fileNamePrefix}_{nameof(MediaTypes.Television)}.json"), data);
                }
            }
        }

        public async Task ClearNowPlaying()
        {
            Configuration configuration = await dataService.Get<Configuration>(item => item.Type == nameof(MediaPages.Player));
            PlayerConfiguration playerConfiguration = null;

            if (configuration != null)
            {
                playerConfiguration = JsonConvert.DeserializeObject<PlayerConfiguration>(configuration.JsonData) ?? new PlayerConfiguration();

                if (playerConfiguration.SelectedMediaType == MediaTypes.Song)
                {
                    IO_File.Delete(Path.Combine(fileService.RootFolder, $"{fileNamePrefix}_{nameof(MediaTypes.Song)}.json"));
                }
                else if (playerConfiguration.SelectedMediaType == MediaTypes.Podcast)
                {
                    IO_File.Delete(Path.Combine(fileService.RootFolder, $"{fileNamePrefix}_{nameof(MediaTypes.Podcast)}.json"));
                }
                else if (playerConfiguration.SelectedMediaType == MediaTypes.Television)
                {
                    IO_File.Delete(Path.Combine(fileService.RootFolder, $"{fileNamePrefix}_{nameof(MediaTypes.Television)}.json"));
                }
            }
        }

        public async Task UpdatePlayCount(MediaTypes mediaType, int id)
        {
            if (mediaType == MediaTypes.Podcast)
            {
                PodcastItem podcastItem = await dataService.Get<PodcastItem>(item => item.Id == id);

                if (podcastItem != null)
                {
                    podcastItem.PlayCount++;
                    await dataService.Update(podcastItem);
                }
            }
            else if (mediaType == MediaTypes.Song)
            {
                Track track = await dataService.Get<Track>(item => item.Id == id);

                if (track != null)
                {
                    track.PlayCount++;
                    await dataService.Update(track);
                }
            }
            else if (mediaType == MediaTypes.Television)
            {
                Episode episode = await dataService.Get<Episode>(item => item.Id == id);

                if (episode != null)
                {
                    episode.PlayCount++;
                    await dataService.Update(episode);
                }
            }
        }

        public async Task<ActionResult> PlayerConfiguration()
        {
            Configuration configuration = await dataService.Get<Configuration>(item => item.Type == nameof(MediaPages.Player));

            if (configuration != null)
            {
                playerViewModel.Configuration = JsonConvert.DeserializeObject<PlayerConfiguration>(configuration.JsonData) ?? new PlayerConfiguration();
            }

            return Json(playerViewModel.Configuration, JsonRequestBehavior.AllowGet);
        }
    }
}