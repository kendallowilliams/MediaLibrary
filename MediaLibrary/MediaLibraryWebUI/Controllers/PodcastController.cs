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

        [ImportingConstructor]
        public PodcastController(IPodcastUIService podcastUIService, IDataService dataService, PodcastViewModel podcastViewModel,
                                 IPodcastService podcastService, IControllerService controllerService, ITransactionService transactionService)
        {
            this.podcastUIService = podcastUIService;
            this.dataService = dataService;
            this.podcastViewModel = podcastViewModel;
            this.podcastService = podcastService;
            this.controllerService = controllerService;
            this.transactionService = transactionService;
        }

        public async Task<ActionResult> Index()
        {
            ActionResult result = null;
            Configuration configuration = await dataService.GetAsync<Configuration>(item => item.Type == nameof(MediaPages.Podcasts));

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
                result = View(podcastViewModel);
            }

            return result;
        }

        public async Task<ActionResult> AddPodcast(string rssFeed)
        {
            Podcast podcast = await podcastService.AddPodcast(rssFeed);
            
            podcastViewModel.SelectedPodcast = podcast;

            return View("Podcast", podcastViewModel);
        }

        public async Task RemovePodcast(int id)
        {
            Configuration configuration = await dataService.GetAsync<Configuration>(item => item.Type == nameof(MediaPages.Podcasts));

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
            string query = @"SELECT PodcastItemId FROM PodcastFile WHERE PodcastId = @podcastId";

            podcastViewModel.DownloadedEpisodes = await dataService.Query<int>(query, dataService.CreateParameter("podcastId", id));
            podcastViewModel.SelectedPodcast = await dataService.GetAsync<Podcast, ICollection<PodcastItem>>(podcast => podcast.Id == id, podcast => podcast.PodcastItems);

            return View("Podcast", podcastViewModel);
        }

        public async Task DownloadPodcastItem(int id)
        {
            Transaction transaction = new Transaction(TransactionTypes.DownloadEpisode);

            try
            {
                await dataService.Insert(transaction);
                await controllerService.QueueBackgroundWorkItem(ct => podcastService.AddPodcastFile(transaction, id), transaction);
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
                        await podcastService.AddPodcastFile(transaction, id);
                        result = new RedirectResult(podcastItem.Url);
                    }
                    else
                    {
                        result = new FileRangeResult(podcastItem.File, Request.Headers["Range"], MimeMapping.GetMimeMapping(Path.GetFileName(podcastItem.File)));
                        await transactionService.UpdateTransactionCompleted(transaction);
                    }
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
                Configuration configuration = await dataService.GetAsync<Configuration>(item => item.Type == nameof(MediaPages.Podcasts));

                if (configuration == null)
                {
                    configuration = new Configuration() { Type = nameof(MediaPages.Podcasts), JsonData = JsonConvert.SerializeObject(podcastConfiguration) };
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