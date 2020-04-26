using MediaLibraryBLL.Services.Interfaces;
using MediaLibraryDAL.DbContexts;
using MediaLibraryDAL.Services.Interfaces;
using MediaLibraryWebUI.ActionResults;
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
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static MediaLibraryDAL.Enums.TransactionEnums;
using static MediaLibraryWebUI.UIEnums;

namespace MediaLibraryWebUI.Controllers
{
    [Export(nameof(MediaPages.Podcast), typeof(IController)), PartCreationPolicy(CreationPolicy.NonShared)]
    public class PodcastController : BaseController
    {
        private readonly Lazy<IPodcastUIService> lazyPodcastUIService;
        private readonly Lazy<IDataService> lazyDataService;
        private readonly Lazy<PodcastViewModel> lazyPodcastViewModel;
        private readonly Lazy<IPodcastService> lazyPodcastService;
        private readonly Lazy<IControllerService> lazyControllerService;
        private readonly Lazy<ITransactionService> lazyTransactionService;
        private readonly Lazy<IFileService> lazyFileService;
        private IPodcastUIService podcastUIService => lazyPodcastUIService.Value;
        private IDataService dataService => lazyDataService.Value;
        private PodcastViewModel podcastViewModel => lazyPodcastViewModel.Value;
        private IPodcastService podcastService => lazyPodcastService.Value;
        private IControllerService controllerService => lazyControllerService.Value;
        private ITransactionService transactionService => lazyTransactionService.Value;
        private IFileService fileService => lazyFileService.Value;

        [ImportingConstructor]
        public PodcastController(Lazy<IPodcastUIService> podcastUIService, Lazy<IDataService> dataService, Lazy<PodcastViewModel> podcastViewModel,
                                 Lazy<IPodcastService> podcastService, Lazy<IControllerService> controllerService, Lazy<ITransactionService> transactionService,
                                 Lazy<IFileService> fileService)
        {
            this.lazyPodcastUIService = podcastUIService;
            this.lazyDataService = dataService;
            this.lazyPodcastViewModel = podcastViewModel;
            this.lazyPodcastService = podcastService;
            this.lazyControllerService = controllerService;
            this.lazyTransactionService = transactionService;
            this.lazyFileService = fileService;
        }

        [CompressContent]
        public async Task<ActionResult> Index()
        {
            ActionResult result = null;
            Configuration configuration = await dataService.Get<Configuration>(item => item.Type == nameof(MediaPages.Podcast));

            if (configuration != null)
            {
                podcastViewModel.Configuration = JsonConvert.DeserializeObject<PodcastConfiguration>(configuration.JsonData);
            }

            podcastViewModel.PodcastGroups = await podcastUIService.GetPodcastGroups(podcastViewModel.Configuration.SelectedPodcastSort);

            if (podcastViewModel.Configuration.SelectedPodcastPage == PodcastPages.Podcast &&
                await dataService.Exists<Podcast>(podcast => podcast.Id == podcastViewModel.Configuration.SelectedPodcastId))
            {
                result = await Get(podcastViewModel.Configuration.SelectedPodcastId, podcastViewModel.Configuration.SelectedPodcastFilter);
            }
            else
            {
                result = PartialView(podcastViewModel);
            }

            return result;
        }

        public async Task AddPodcast(string rssFeed)
        {
            Podcast podcast = await dataService.Get<Podcast>(item => item.Url.Equals(rssFeed, StringComparison.CurrentCultureIgnoreCase)) ?? 
                              await podcastService.AddPodcast(rssFeed);

            Configuration configuration = await dataService.Get<Configuration>(item => item.Type == nameof(MediaPages.Podcast));

            if (configuration != null)
            {
                podcastViewModel.Configuration = JsonConvert.DeserializeObject<PodcastConfiguration>(configuration.JsonData);
                podcastViewModel.Configuration.SelectedPodcastId = podcast.Id;
                podcastViewModel.Configuration.SelectedPodcastPage = PodcastPages.Podcast;
                configuration.JsonData = JsonConvert.SerializeObject(podcastViewModel.Configuration);
                await dataService.Update(configuration);
            }
        }

        public async Task RemovePodcast(int id)
        {
            Configuration configuration = await dataService.Get<Configuration>(item => item.Type == nameof(MediaPages.Podcast));
            Podcast podcast = await dataService.Get<Podcast>(item => item.Id == id, default, item => item.PodcastItems);
            IEnumerable<string> episodes = podcast.PodcastItems.Where(item => !string.IsNullOrWhiteSpace(item.File))
                                                               .Select(item => item.File);

            foreach (string file in episodes) { fileService.Delete(file); }
            await dataService.Delete<Podcast>(id);

            if (configuration != null)
            {
                PodcastConfiguration podcastConfiguration = JsonConvert.DeserializeObject<PodcastConfiguration>(configuration.JsonData);

                podcastConfiguration.SelectedPodcastPage = PodcastPages.Index;
                configuration.JsonData = JsonConvert.SerializeObject(podcastConfiguration);
                await dataService.Update(configuration);
            }
        }

        private async Task<ActionResult> Get(int id, PodcastFilter filter = default(PodcastFilter))
        {   // retrieve podcast items in order to get list of all years for view
            Func<PodcastItem, bool> expression = null;

            if (filter == PodcastFilter.Downloaded) /*then*/ expression = item => !string.IsNullOrWhiteSpace(item.File);
            else if (filter == PodcastFilter.Unplayed) /*then*/ expression = item => item.PlayCount == 0;
            podcastViewModel.SelectedPodcast = await dataService.Get<Podcast>(podcast => podcast.Id == id, default, podcast => podcast.PodcastItems);
            if (expression != null) /*then*/ podcastViewModel.SelectedPodcast.PodcastItems = podcastViewModel.SelectedPodcast.PodcastItems.Where(expression).ToList();

            return PartialView("Podcast", podcastViewModel);
        }

        public async Task<ActionResult> GetPodcastItems(int id, int year, PodcastFilter filter = default(PodcastFilter))
        {
            Func<PodcastItem, bool> expression = null;
            IEnumerable<PodcastItem> podcastItems = await dataService.GetList<PodcastItem>(item => item.PodcastId == id && item.PublishDate.Year == year);
            bool hasPlaylists = await dataService.Exists<Playlist>(item => item.Type == (int)PlaylistTabs.Podcast);

            if (filter == PodcastFilter.Downloaded) /*then*/ expression = item => !string.IsNullOrWhiteSpace(item.File);
            else if (filter == PodcastFilter.Unplayed) /*then*/ expression = item => item.PlayCount == 0;
            if (expression != null) /*then*/ podcastItems = podcastItems.Where(expression);

            return PartialView("PodcastItems", (hasPlaylists, podcastItems.OrderByDescending(item => item.PublishDate)));
        }

        public async Task DownloadPodcastItem(int id)
        {
            Transaction transaction = new Transaction(TransactionTypes.DownloadEpisode);

            try
            {
                Transaction existingTransaction = await dataService.Get<Transaction>(item => item.Type == (int)TransactionTypes.DownloadEpisode &&
                                                                                             item.Status == (int)TransactionStatus.InProcess &&
                                                                                             id.ToString().Equals(item.Message));
                await dataService.Insert(transaction);

                if (existingTransaction == null)
                {
                    await controllerService.QueueBackgroundWorkItem(ct => podcastService.AddPodcastFile(transaction, id), transaction);
                }
                else
                {
                    await transactionService.UpdateTransactionCompleted(transaction, $"Podcast episode ({id}) download in progress.");
                }
            }
            catch(Exception ex)
            {
                await transactionService.UpdateTransactionErrored(transaction, ex);
            }
        }

        public async Task RefreshPodcast(int id)
        {
            await podcastService.RefreshPodcast(await dataService.Get<Podcast>(item => item.Id == id));
        }

#if !DEBUG && !DEV
        [AllowAnonymous]
#endif
        public async Task<ActionResult> File(int id)
        {
            Transaction transaction = await transactionService.GetNewTransaction(TransactionTypes.GetPodcastFile);
            ActionResult result = null;

            try
            {
                PodcastItem podcastItem = await dataService.Get<PodcastItem>(item => item.Id == id);

                if (podcastItem != null)
                {
                    if (string.IsNullOrWhiteSpace(podcastItem.File))
                    {
                        result = new RedirectResult(podcastItem.Url);
                    }
                    else
                    {
                        result = new FileRangeResult(podcastItem.File, Request.Headers["Range"], MimeMapping.GetMimeMapping(Path.GetFileName(podcastItem.File)));
                    }
                    await transactionService.UpdateTransactionCompleted(transaction);
                }
                else
                {
                    result = new HttpStatusCodeResult(HttpStatusCode.NotFound);
                    await transactionService.UpdateTransactionCompleted(transaction, $"Podcast item: {id} not found.");
                }
            }
            catch(Exception ex)
            {
                await transactionService.UpdateTransactionErrored(transaction, ex);
                result = new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }

            return result;
        }

        public async Task UpdateConfiguration(PodcastConfiguration podcastConfiguration)
        {
            if (ModelState.IsValid)
            {
                Configuration configuration = await dataService.Get<Configuration>(item => item.Type == nameof(MediaPages.Podcast));

                if (configuration == null)
                {
                    configuration = new Configuration() { Type = nameof(MediaPages.Podcast), JsonData = JsonConvert.SerializeObject(podcastConfiguration) };
                    await dataService.Insert(configuration);
                }
                else
                {
                    configuration.JsonData = JsonConvert.SerializeObject(podcastConfiguration);
                    await dataService.Update(configuration);
                }
            }
        }

        public async Task<ActionResult> PodcastConfiguration()
        {
            Configuration configuration = await dataService.Get<Configuration>(item => item.Type == nameof(MediaPages.Podcast));

            if (configuration != null)
            {
                podcastViewModel.Configuration = JsonConvert.DeserializeObject<PodcastConfiguration>(configuration.JsonData) ?? new PodcastConfiguration();
            }

            return Json(podcastViewModel.Configuration, JsonRequestBehavior.AllowGet);
        }

        public async Task AddPodcastItemToPlaylist(int itemId, int playlistId)
        {
            PlaylistPodcastItem item = new PlaylistPodcastItem() { PlaylistId = playlistId, PodcastItemId = itemId };
            Transaction transaction = null;

            try
            {
                transaction = await transactionService.GetNewTransaction(TransactionTypes.AddPlaylistPodcastItem);
                await dataService.Insert(item);
                await transactionService.UpdateTransactionCompleted(transaction, $"Playlist: {playlistId}, PodcastItem: {itemId}");
            }
            catch (Exception ex)
            {
                await transactionService.UpdateTransactionErrored(transaction, ex);
            }
        }
    }
}