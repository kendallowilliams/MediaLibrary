using MusicLibraryBLL.Models;
using MusicLibraryBLL.Services.Interfaces;
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
        private readonly ITransactionService transactionService;

        [ImportingConstructor]
        public PodcastController(IPodcastService podcastService, ITransactionService transactionService)
        {
            this.podcastService = podcastService;
            this.transactionService = transactionService;
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

        // POST: api/Podcast
        public async Task<Podcast> Post([FromBody] JObject inData)
        {
            Transaction transaction = null;
            Podcast podcast = null;

            try
            {
                string path = inData["path"]?.ToString();
                bool copyFiles = false,
                     validData = !string.IsNullOrWhiteSpace(path) &&
                                 bool.TryParse(inData["copy"]?.ToString(), out copyFiles);
                string responseMessage = $"Invalid Data: [{inData}]",
                       transactionType = Enum.GetName(typeof(TransactionTypes), TransactionTypes.AddPodcast);

                transaction = await transactionService.GetNewTransaction(TransactionTypes.AddPodcast);
                if (validData) { podcast = await podcastService.AddPodcast(path); }
                else { responseMessage = $"Invalid Data: [{inData}]"; }

                await transactionService.UpdateTransactionCompleted(transaction, responseMessage);
            }
            catch (Exception ex)
            {
                await transactionService.UpdateTransactionErrored(transaction, ex);
            }

            return podcast;
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
                QueueBackgroundWorkItem(ct => podcastService.AddPodcastFile(transaction, podcastItemId), transaction);
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
    }
}
