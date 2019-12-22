using MediaLibraryBLL.Models;
using MediaLibraryBLL.Services.Interfaces;
using MediaLibraryDAL.DbContexts;
using MediaLibraryDAL.Services.Interfaces;
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

namespace MediaLibraryWebUI.Controllers
{
    [Export("Player", typeof(IController)), PartCreationPolicy(CreationPolicy.NonShared)]
    public class PlayerController : BaseController
    {
        private readonly ITransactionService transactionService;
        private readonly PlayerViewModel playerViewModel;
        private readonly IDataService dataService;
        private readonly string dataFolder,
                                fileNamePrefix;

        [ImportingConstructor]
        public PlayerController(ITransactionService transactionService, PlayerViewModel playerViewModel, IDataService dataService)
        {
            this.transactionService = transactionService;
            this.playerViewModel = playerViewModel;
            this.dataService = dataService;
            dataFolder = Path.Combine(GetFolderPath(SpecialFolder.CommonApplicationData, SpecialFolderOption.Create), nameof(MediaLibraryWebUI));
            fileNamePrefix = $"{nameof(PlayerController)}_{nameof(UpdateNowPlaying)}";
        }
        
        public async Task<ActionResult> Index()
        {
            Configuration configuration = await dataService.GetAsync<Configuration>(item => item.Type == nameof(MediaPages.Player));

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
                Configuration configuration = await dataService.GetAsync<Configuration>(item => item.Type == nameof(MediaPages.Player));

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

        public async Task<ActionResult> GetPlayerItems()
        {
            Configuration configuration = await dataService.GetAsync<Configuration>(item => item.Type == nameof(MediaPages.Player));

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
                string path = Path.Combine(dataFolder, $"{fileNamePrefix}_{nameof(MediaTypes.Song)}.json");
                IEnumerable<ListItem<int, int>> items = Enumerable.Empty<ListItem<int, int>>();
                IEnumerable<Track> songs = Enumerable.Empty<Track>();

                if (System.IO.File.Exists(path)) /*then*/ items = JsonConvert.DeserializeObject<IEnumerable<ListItem<int, int>>>(System.IO.File.ReadAllText(path));
                ids = items.Select(item => item.Value);
                songs = await dataService.GetList<Track>(item => ids.Contains(item.Id));
                playerViewModel.Songs = ids.Select(id => songs.FirstOrDefault(item => item.Id == id));
            }
            else if (playerViewModel.Configuration.SelectedMediaType == MediaTypes.Podcast)
            {
                string path = Path.Combine(dataFolder, $"{fileNamePrefix}_{nameof(MediaTypes.Podcast)}.json");
                IEnumerable<ListItem<int, int>> items = Enumerable.Empty<ListItem<int, int>>();
                IEnumerable<PodcastItem> podcastItems = Enumerable.Empty<PodcastItem>();

                if (System.IO.File.Exists(path)) /*then*/ items = JsonConvert.DeserializeObject<IEnumerable<ListItem<int, int>>>(System.IO.File.ReadAllText(path));
                ids = items.Select(item => item.Value);
                podcastItems = await dataService.GetList<PodcastItem>(item => ids.Contains(item.Id));
                playerViewModel.PodcastItems = ids.Select(id => podcastItems.FirstOrDefault(item => item.Id == id));
            }
            else if (playerViewModel.Configuration.SelectedMediaType == MediaTypes.Television)
            {
                string path = Path.Combine(dataFolder, $"{fileNamePrefix}_{nameof(MediaTypes.Television)}.json");
                IEnumerable<ListItem<int, int>> items = Enumerable.Empty<ListItem<int, int>>();
                IEnumerable<Episode> episodes = Enumerable.Empty<Episode>();

                if (System.IO.File.Exists(path)) /*then*/ items = JsonConvert.DeserializeObject<IEnumerable<ListItem<int, int>>>(System.IO.File.ReadAllText(path));
                ids = items.Select(item => item.Value);
                episodes = await dataService.GetList<Episode>(item => ids.Contains(item.Id));
                playerViewModel.Episodes = ids.Select(id => episodes.FirstOrDefault(item => item.Id == id));
            }
        }

        public async Task UpdateNowPlaying(string itemsJSON, MediaTypes mediaType)
        {
            var items = JsonConvert.DeserializeObject<IEnumerable<ListItem<int, int>>>(itemsJSON);
            Configuration configuration = await dataService.GetAsync<Configuration>(item => item.Type == nameof(MediaPages.Player));
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

                if (!Directory.Exists(dataFolder)) /*then*/ Directory.CreateDirectory(dataFolder);

                if (mediaType == MediaTypes.Song)
                {
                    System.IO.File.WriteAllText(Path.Combine(dataFolder, $"{fileNamePrefix}_{nameof(MediaTypes.Song)}.json"), data);
                }
                else if (mediaType == MediaTypes.Podcast)
                {
                    System.IO.File.WriteAllText(Path.Combine(dataFolder, $"{fileNamePrefix}_{nameof(MediaTypes.Podcast)}.json"), data);
                }
                else if (mediaType == MediaTypes.Television)
                {
                    System.IO.File.WriteAllText(Path.Combine(dataFolder, $"{fileNamePrefix}_{nameof(MediaTypes.Television)}.json"), data);
                }
            }
        }
    }
}