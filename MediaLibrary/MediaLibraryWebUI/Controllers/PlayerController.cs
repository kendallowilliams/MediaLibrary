using MediaLibraryBLL.Models;
using MediaLibraryBLL.Services.Interfaces;
using MediaLibraryDAL.DbContexts;
using MediaLibraryDAL.Services.Interfaces;
using MediaLibraryWebUI.Attributes;
using MediaLibraryWebUI.Models;
using MediaLibraryWebUI.Models.Configurations;
using MediaLibraryWebUI.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static MediaLibraryBLL.Enums;
using static MediaLibraryWebUI.UIEnums;
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
        private readonly IPlayerUIService playerUIService;
        private readonly IPlayerService playerService;

        [ImportingConstructor]
        public PlayerController(ITransactionService transactionService, PlayerViewModel playerViewModel, IDataService dataService,
                                IPlayerUIService playerUIService, IPlayerService playerService)
        {
            this.transactionService = transactionService;
            this.playerViewModel = playerViewModel;
            this.dataService = dataService;
            this.playerUIService = playerUIService;
            this.playerService = playerService;
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
                playerViewModel.Songs = await playerService.GetNowPlayingSongs();
            }
            else if (playerViewModel.Configuration.SelectedMediaType == MediaTypes.Podcast)
            {
                playerViewModel.PodcastItems = await playerService.GetNowPlayingPodcastItems();
            }
            else if (playerViewModel.Configuration.SelectedMediaType == MediaTypes.Television)
            {
                playerViewModel.Episodes = await playerService.GetNowPlayingEpisodes();
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

            playerConfiguration.CurrentItemIndex = items.FirstOrDefault((item) => item.IsSelected)?.Id ?? 0;
            playerConfiguration.SelectedMediaType = mediaType;
            configuration.JsonData = JsonConvert.SerializeObject(playerConfiguration);
            await dataService.Update(configuration);
            if (items != null) /*then*/ playerService.UpdateNowPlaying(items, mediaType);
        }

        public async Task ClearNowPlaying()
        {
            Configuration configuration = await dataService.Get<Configuration>(item => item.Type == nameof(MediaPages.Player));
            PlayerConfiguration playerConfiguration = null;

            if (configuration != null)
            {
                playerConfiguration = JsonConvert.DeserializeObject<PlayerConfiguration>(configuration.JsonData) ?? new PlayerConfiguration();
                playerService.ClearNowPlaying(playerConfiguration.SelectedMediaType);
            }
        }

        public async Task UpdatePlayCount(MediaTypes mediaType, int id)
        {
            await playerService.UpdatePlayCount(id, mediaType);
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

        public async Task UpdatePlayerProgress(int id, MediaTypes mediaType, int progress)
        {
            await playerService.UpdatePlayerProgress(id, mediaType, progress);
        }
    }
}