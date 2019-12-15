using MediaLibraryBLL.Services.Interfaces;
using MediaLibraryDAL.DbContexts;
using MediaLibraryDAL.Services.Interfaces;
using MediaLibraryWebUI.ActionResults;
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
using static MediaLibraryWebUI.Enums;

namespace MediaLibraryWebUI.Controllers
{
    [Export("Podcast", typeof(IController)), PartCreationPolicy(CreationPolicy.NonShared)]
    public class PodcastController : BaseController
    {
        private readonly IPodcastUIService podcastUIService;
        private readonly IDataService dataService;
        private readonly PodcastViewModel podcastViewModel;
        private readonly IPodcastService podcastService;
        private readonly IControllerService controllerService;
        private readonly ITransactionService transactionService;
        private readonly IFileService fileService;

        [ImportingConstructor]
        public PodcastController(IPodcastUIService podcastUIService, IDataService dataService, PodcastViewModel podcastViewModel,
                                 IPodcastService podcastService, IControllerService controllerService, ITransactionService transactionService,
                                 IFileService fileService)
        {
            this.podcastUIService = podcastUIService;
            this.dataService = dataService;
            this.podcastViewModel = podcastViewModel;
            this.podcastService = podcastService;
            this.controllerService = controllerService;
            this.transactionService = transactionService;
            this.fileService = fileService;
        }

        public async Task<ActionResult> Index()
        {
            ActionResult result = null;
            Configuration configuration = await dataService.GetAsync<Configuration>(item => item.Type == nameof(MediaPages.Podcast));

            if (configuration != null)
            {
                podcastViewModel.Configuration = JsonConvert.DeserializeObject<PodcastConfiguration>(configuration.JsonData);
            }

            if (podcastViewModel.Configuration.SelectedPodcastPage == PodcastPages.Podcast)
            {
                result = await Get(podcastViewModel.Configuration.SelectedPodcastId);
            }
            else
            {
                podcastViewModel.PodcastGroups = await podcastUIService.GetPodcastGroups(podcastViewModel.Configuration.SelectedPodcastSort);
                result = PartialView(podcastViewModel);
            }

            return result;
        }

        public async Task<ActionResult> AddPodcast(string rssFeed)
        {
            Podcast podcast = await podcastService.AddPodcast(rssFeed);
            
            podcastViewModel.SelectedPodcast = podcast;

            return PartialView("Podcast", podcastViewModel);
        }

        public async Task RemovePodcast(int id)
        {
            Configuration configuration = await dataService.GetAsync<Configuration>(item => item.Type == nameof(MediaPages.Podcast));
            Podcast podcast = await dataService.Get<Podcast, ICollection<PodcastItem>>(item => item.Id == id, item => item.PodcastItems);
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

        public async Task<ActionResult> Get(int id)
        {
            podcastViewModel.DownloadedEpisodes = (await dataService.GetList<PodcastItem>(item => item.File != null && item.File != "")).Select(item => item.Id);
            podcastViewModel.SelectedPodcast = await dataService.GetAsync<Podcast, ICollection<PodcastItem>>(podcast => podcast.Id == id, podcast => podcast.PodcastItems);

            return PartialView("Podcast", podcastViewModel);
        }

        public async Task DownloadPodcastItem(int id)
        {
            Transaction transaction = new Transaction(TransactionTypes.DownloadEpisode);

            try
            {
                Transaction existingTransaction = await dataService.Get<Transaction>(item => item.Type == (int)TransactionTypes.DownloadEpisode &&
                                                                                             item.Status == (int)TransactionStatus.InProcess);
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
            await podcastService.RefreshPodcast(await dataService.GetAsync<Podcast>(item => item.Id == id));
        }

        [AllowAnonymous]
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
                Configuration configuration = await dataService.GetAsync<Configuration>(item => item.Type == nameof(MediaPages.Podcast));

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
    }
}