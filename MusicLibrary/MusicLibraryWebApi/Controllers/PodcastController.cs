using MusicLibraryBLL.Models;
using MusicLibraryBLL.Services.Interfaces;
using MusicLibraryWebApi.Services.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using static MusicLibraryBLL.Enums.TransactionEnums;

namespace MusicLibraryWebApi.Controllers
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class PodcastController : ApiControllerBase
    {
        private readonly IPodcastService podcastService;
        private readonly IControllerService controllerService;

        [ImportingConstructor]
        public PodcastController(IPodcastService podcastService, ITransactionService transactionService, IControllerService controllerService)
        {
            this.podcastService = podcastService;
            this.transactionService = transactionService;
            this.controllerService = controllerService;
        }

        // GET: api/Podcast
        public async Task<IEnumerable<Podcast>> Get()
        {
            IEnumerable<Podcast> podcasts = Enumerable.Empty<Podcast>();
            Transaction transaction = null;

            try
            {
                transaction = await transactionService.GetNewTransaction(TransactionTypes.GetPodcasts);
                podcasts = await podcastService.GetPodcasts();
                await transactionService.UpdateTransactionCompleted(transaction);
            }
            catch (Exception ex)
            {
                await transactionService.UpdateTransactionErrored(transaction, ex);
            }

            return podcasts.OrderBy(podcast => podcast.Title);
        }

        // GET: api/Podcast/5
        public async Task<Podcast> Get(int id)
        {
            Transaction transaction = null;
            Podcast podcast = null;

            try
            {
                transaction = await transactionService.GetNewTransaction(TransactionTypes.GetPodcast);
                podcast = await podcastService.GetPodcast(id);
                await transactionService.UpdateTransactionCompleted(transaction);
            }
            catch (Exception ex)
            {
                await transactionService.UpdateTransactionErrored(transaction, ex);
            }
            
            return podcast;
        }

        // GET: api/Podcast/GetPodcastItems/5
        [Route("api/Podcast/GetPodcastItems/{id}")]
        public async Task<IEnumerable<PodcastItem>> GetPodcastItems(int id)
        {
            Transaction transaction = null;
            IEnumerable<PodcastItem> items = null;

            try
            {
                transaction = await transactionService.GetNewTransaction(TransactionTypes.GetPodcastItems);
                items = await podcastService.GetPodcastItems(id);
                await transactionService.UpdateTransactionCompleted(transaction);
            }
            catch (Exception ex)
            {
                await transactionService.UpdateTransactionErrored(transaction, ex);
                items = Enumerable.Empty<PodcastItem>();
            }

            return items.OrderByDescending(item => item.PublishDate);
        }

        // POST: api/Podcast
        public async Task<IEnumerable<Podcast>> Post([FromBody] JObject inData)
        {
            Transaction transaction = null;
            IEnumerable<Task<Podcast>> tasks = Enumerable.Empty<Task<Podcast>>();
            IEnumerable<Podcast> podcasts = Enumerable.Empty<Podcast>();

            try
            {
                IEnumerable<string> paths = inData["paths"].Values<string>() ?? Enumerable.Empty<string>();
                bool copyFiles = false,
                     validData = paths.Any() &&
                                 bool.TryParse(inData["copy"]?.ToString(), out copyFiles);
                string responseMessage = $"Invalid Data: [{inData}]",
                       transactionType = Enum.GetName(typeof(TransactionTypes), TransactionTypes.AddPodcast);

                transaction = await transactionService.GetNewTransaction(TransactionTypes.AddPodcast);
                if (validData)
                {
                    tasks = paths.Select(path => podcastService.AddPodcast(path));
                    podcasts = await Task.WhenAll(tasks);
                }
                else { responseMessage = $"Invalid Data: [{inData}]"; }

                await transactionService.UpdateTransactionCompleted(transaction, responseMessage);
            }
            catch (Exception ex)
            {
                await transactionService.UpdateTransactionErrored(transaction, ex);
            }

            return podcasts;
        }

        // PUT: api/Podcast/5
        public async Task<bool> Put(int id, [FromBody]Podcast podcast)
        {
            Transaction transaction = null;
            bool isReplaced = false;

            try
            {
                transaction = await transactionService.GetNewTransaction(TransactionTypes.ReplacePodcast);
                isReplaced = await podcastService.UpdatePodcast(podcast);
                await transactionService.UpdateTransactionCompleted(transaction);
            }
            catch (Exception ex)
            {
                await transactionService.UpdateTransactionErrored(transaction, ex);
            }

            return isReplaced;
        }

        // DELETE: api/Podcast/5
        public async Task<bool> Delete(int id)
        {
            Transaction transaction = null;
            bool isRemoved = false;

            try
            {
                transaction = await transactionService.GetNewTransaction(TransactionTypes.RemovePodcast);
                isRemoved = await podcastService.DeletePodcast(id);
                await transactionService.UpdateTransactionCompleted(transaction);
            }
            catch (Exception ex)
            {
                await transactionService.UpdateTransactionErrored(transaction, ex);
            }

            return isRemoved;
        }

        [Route("api/Podcast/DownloadEpisode")]
        public async Task DownloadEpisode([FromBody] int podcastItemId)
        {
            Transaction transaction = null;

            try
            {
                transaction = await transactionService.GetNewTransaction(TransactionTypes.DownloadEpisode);
                transaction.Message = $"{podcastItemId}";
                controllerService.QueueBackgroundWorkItem(ct => podcastService.AddPodcastFile(transaction, podcastItemId), transaction);
            }
            catch (Exception ex)
            {
                await transactionService.UpdateTransactionErrored(transaction, ex);
            }
        }

        [Route("api/Podcast/DownloadAllEpisodes")]
        public async Task DownloadAllEpisodes(int podcastId)
        {
            Transaction transaction = null;

            try
            {
                transaction = await transactionService.GetNewTransaction(TransactionTypes.DownloadAllEpisodes);
                await Task.FromResult(default(int?));
                await transactionService.UpdateTransactionCompleted(transaction);
            }
            catch (Exception ex)
            {
                await transactionService.UpdateTransactionErrored(transaction, ex);
            }
        }

        [Route("api/Podcast/Refresh")]
        public async Task<Podcast> Refresh([FromBody] Podcast data)
        {
            Transaction transaction = null;
            Podcast podcast = null;

            try
            {
                transaction = await transactionService.GetNewTransaction(TransactionTypes.RefreshPodcast);
                podcast = await podcastService.RefreshPodcast(data);
                await transactionService.UpdateTransactionCompleted(transaction);
            }
            catch (Exception ex)
            {
                await transactionService.UpdateTransactionErrored(transaction, ex);
            }

            return podcast;
        }
    }
}
